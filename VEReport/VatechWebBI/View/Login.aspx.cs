using DevExpress.Xpo;
using DevExpress.Xpo.DB.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_Login : System.Web.UI.Page
{
    clsDatabase db = new clsDatabase();
    string _query;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if ((Session["SessionNo"] ?? "").ToString() != "")
            {
                _query = string.Format("exec {0}_ReportDB.dbo.Code_CloseSession '{0}', N'{1}', '{2}'", Session["CorpId"], Session["EmpNo"], Session["SessionNo"]);
                db.ExecuteQuery(_query);
            }

            Session.Abandon();

            Session["CorpId"] = "RY";
        }
    }

    protected void CbPnlLogin_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        if (cboUserId.Value == null)
        {
            CbPnlLogin.JSProperties["cpLogin"] = "false";
            CbPnlLogin.JSProperties["cpMsg"] = "Camstar에 등록된 계정이 아닙니다.";
            return;
        }

        _query = string.Format("SELECT cu.*, emp.FullName FROM {0}_ReportDB.dbo.CodeUser cu "
                    + "Inner Join CAMDBsh.Employee emp on cu.EmployeeName = emp.EmployeeName "
                + "WHERE cu.CorpId = '{0}' and cu.EmployeeName = '{1}'", Session["CorpId"], cboUserId.Value.ToString());

        DataRowView drv = db.GetDataRecord(_query);

        if (drv == null)
        {
            drv = AddUser(cboUserId.Value.ToString(), txtPassword.Text);
        }

        if (clsCommon.SHA256Hash(txtPassword.Text) == clsCommon.getString(drv["Password"]))
        {
            Session["EmpNo"] = (drv["EmployeeName"] ?? "").ToString();
            Session["EmpName"] = (drv["FullName"] ?? "").ToString();
            Session["EmpIp"] = GetUserIP();
            Session["ProgramType"] = "1";

            Session["Author"] = GetUserAuthor();

            CreateSession();

            CbPnlLogin.JSProperties["cpLogin"] = "true";
        }
        else
        {
            CbPnlLogin.JSProperties["cpLogin"] = "false";
            CbPnlLogin.JSProperties["cpMsg"] = "비밀번호가 일치 하지 않습니다. 다시 입력하세요.";
        }
    }

    private void CreateSession()
    {
        Session["SessionNo"] = DateTime.Now.ToString("yyyyMMddHHmmss");

        _query = string.Format("exec {0}_ReportDB.dbo.Code_CreateSession '{0}', N'{1}', '{2}', '{3}'", Session["CorpId"], Session["EmpNo"], Session["SessionNo"], "WEB");
        db.ExecuteQuery(_query);
    }

    private DataRowView AddUser(string uid, string upw)
    {
        _query = string.Format("Insert Into {0}_ReportDB.dbo.CodeUser(CorpId, EmployeeName, Password) Values('{0}', '{1}', '{2}')", Session["CorpId"].ToString(), uid, clsCommon.SHA256Hash(upw));
        db.ExecuteQuery(_query);

        _query = string.Format("SELECT cu.*, emp.FullName FROM {0}_ReportDB.dbo.CodeUser cu "
                    + "Inner Join CAMDBsh.Employee emp on cu.EmployeeName = emp.EmployeeName "
                + "WHERE cu.CorpId = '{0}' and cu.EmployeeName = '{1}'", Session["CorpId"], uid);
        return db.GetDataRecord(_query);
    }

    private DataView GetUser()
    {
        _query = string.Format("Select EmployeeName ID, FullName 사용자명 From CAMDBsh.Employee Order By FullName", Session["CorpId"].ToString());
        return db.GetDataView("사용자", _query);
    }

    private List<clsAuthor> GetUserAuthor()
    {
        _query = string.Format("SELECT AuthorGroup FROM {0}_ReportDB.dbo.CodeUser WHERE CorpId = '{0}' and EmployeeName = '{1}'", Session["CorpId"].ToString(), Session["EmpNo"].ToString());
        DataRowView drv = db.GetDataRecord(_query);

        if (drv[0] == DBNull.Value) return null;

        drv[0] = drv[0].ToString().Replace(" ", "");

        string[] arrGroup = drv[0].ToString().Split(new char[] { ',' });
        string inGroupStr = "";

        foreach (string item in arrGroup)
        {
            if (item == "") continue;

            if (inGroupStr != "") inGroupStr = inGroupStr + ",";
            inGroupStr = inGroupStr + "''" + item + "''";
        }

        _query = string.Format("exec {0}_ReportDB.dbo.Code_GetAuthorByAuthorGroups '{0}', '{1}', '{2}'", Session["CorpId"].ToString(), Session["ProgramType"].ToString(), inGroupStr);
        DataView dv = db.GetDataView("권한", _query);

        List<clsAuthor> AuthorList = new List<clsAuthor>();

        foreach (DataRowView view in dv)
        {
            clsAuthor author = new clsAuthor(view["MenuID"].ToString(), view["ParentID"].ToString(), view["AuthorCode"].ToString());
            AuthorList.Add(author);
        }

        return AuthorList;
    }

    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }

        return Request.ServerVariables["REMOTE_ADDR"];
    }
}