using DevExpress.DashboardCommon.Native;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.Web.Internal;
using DevExpress.Xpo.DB.Helpers;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class View_Dashboard_CheckSheet : System.Web.UI.Page
{
    protected string pstrMenuId = "";
    protected clsDatabase db = new clsDatabase();
    protected string query = "";
    protected DataRowView master = null;
    protected string pstrErrMsg = "";

    protected string pstrCmd = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["EmpNo"] ?? "").ToString() == "")
        {
            try
            {
                HttpContext.Current.Response.Redirect("~/View/SessionError.aspx");
            }
            catch (ApplicationException)
            {
                HttpContext.Current.Response.RedirectLocation =
                                     System.Web.VirtualPathUtility.ToAbsolute("~/View/SessionError.aspx");
            }
        }

        if (!IsPostBack)
        {
            if ((Request.QueryString["id"] ?? "") == "")
            {
                pstrErrMsg = "메뉴ID를 가져올수 없습니다.";
                return;
            }

            query = string.Format("exec {0}_ReportDB.dbo.Code_UseSession '{0}', N'{1}', '{2}', '{3}', N'{4}'", Session["CorpId"], Session["EmpNo"], Session["SessionNo"], (Request.QueryString["id"] ?? ""), "Open");
            db.ExecuteQuery(query);
        }

        pstrCmd = Request.QueryString["cmd"] ?? "";

        pstrMenuId = Request.QueryString["id"] ?? "";
        master = IsExistDashboardItem(pstrMenuId);

        if (master == null)
        {
            pstrErrMsg = "선택된 메뉴는 대시보드 작성 폼이 아닙니다.";
            return;
        }

        if (master["XML"].ToString() == "")
        {
            pstrErrMsg = "선택된 메뉴는 대시보드가 작성되지 않았습니다.";
            return;
        }

        master["XML"] = master["XML"].ToString().Replace("?<?", "<?");

        byte[] m_Buffer = System.Text.Encoding.UTF8.GetBytes(master["XML"].ToString());
        MemoryStream ms = new MemoryStream(m_Buffer, 0, m_Buffer.Length);

        XDocument doc = XDocument.Load(ms);

        if (doc.Element("Dashboard").Element("Parameters") != null)
        {
            IEnumerable<XElement> parameters = doc.Element("Dashboard").Element("Parameters").Elements("Parameter");

            if (parameters.Count() > 0)
            {
                foreach (XElement param in parameters)
                {
                    if (param.Attribute("Type") == null) continue;

                    if (param.Attribute("Type").Value == "System.DateTime")
                    {
                        if (param.Attribute("Name").Value.ToLower().IndexOf("start") >= 0 || param.Attribute("Name").Value.ToLower().IndexOf("from") >= 0)
                        {
                            param.Attribute("Value").Value = DateTime.Today.AddDays(-1).ToShortDateString();
                        }
                        else if (param.Attribute("Name").Value.ToLower().IndexOf("end") >= 0 || param.Attribute("Name").Value.ToLower().IndexOf("to") >= 0)
                        {
                            param.Attribute("Value").Value = DateTime.Today.ToShortDateString();
                        }
                        else
                        {
                            param.Attribute("Value").Value = DateTime.Today.ToShortDateString();
                        }
                    }
                }
            }
        }

        WebDashboard.OpenDashboard(doc);
        ms.Flush();
        ms.Close();
    }

    protected DataRowView IsExistDashboardItem(string menuId)
    {
        query = string.Format("Select bi.*, mg.Title_ko MenuName, pmg.Title_ko ParentMenuName "
                            + "From {0}_ReportDB.dbo.DashBoardItem bi "
                                + "Inner Join {0}_ReportDB.dbo.MenuGroup mg on bi.CorpId = mg.CorpId and bi.MenuId = mg.Id "
                                + "Left Join {0}_ReportDB.dbo.MenuGroup pmg on mg.CorpId = pmg.CorpId and mg.ParentId = pmg.Id "
                            + "Where bi.CorpId = '{0}' and bi.MenuId = '{1}'",
                        Session["CorpId"], menuId);

        return db.GetDataRecord(query);
    }

    protected void WebDashboard_ConfigureDataConnection(object sender, DevExpress.DashboardWeb.ConfigureDataConnectionWebEventArgs e)
    {
        if (e.ConnectionParameters.GetType().Name != "MsSqlConnectionParameters") return;

        MsSqlConnectionParameters parameters = (MsSqlConnectionParameters)e.ConnectionParameters;
        parameters.UserName = Session["DbId"].ToString();
        parameters.Password = Session["DbPw"].ToString();
    }
}