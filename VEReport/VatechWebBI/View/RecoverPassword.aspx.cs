using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_RecoverPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void CbPnlChange_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
    {
        if (txtNewPassword.Text != txtConfirmPassword.Text)
        {
            CbPnlChange.JSProperties["cpMsg"] = "변경할 비밀번호와 확인을 동일하게 입력하세요.";
            return;
        }

        clsDatabase db = new clsDatabase();
        string _query = _query = string.Format("SELECT * FROM {0}_ReportDB.dbo.CodeUser "
                            + "WHERE CorpId = '{0}' and EmployeeName = '{1}'", Session["CorpId"], cboUserId.Value.ToString());

        SqlDataReader reader = db.GetDataReader(_query);

        if (reader.HasRows)
        {
            if (reader.Read())
            {
                if (clsCommon.SHA256Hash(txtPassword.Text) != clsCommon.getString(reader["Password"]))
                {
                    CbPnlChange.JSProperties["cpMsg"] = "현재 비밀번호가 아닙니다.";
                    return;
                }
            }

            reader.Close();
        }
        else
        {
            CbPnlChange.JSProperties["cpMsg"] = "입력한 아이디가 존재하지 않습니다.";
            return;
        }

        _query = string.Format("UPDATE {0}_ReportDB.dbo.CodeUser SET Password = '{2}' WHERE CorpId = '{0}' and EmployeeName = '{1}'", 
                    Session["CorpId"], cboUserId.Value.ToString(), clsCommon.SHA256Hash(txtNewPassword.Text));

        db.ExecuteQuery(_query);
        db = null;

        CbPnlChange.JSProperties["cpChange"] = "true";
        CbPnlChange.JSProperties["cpMsg"] = "변경되었습니다.";
    }
}