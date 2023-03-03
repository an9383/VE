using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_LogOut : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    [WebMethod(EnableSession = true)]
    public static void LogOut()
    {
        if ((HttpContext.Current.Session["EmpNo"] ?? "").ToString() != "")
        {
            clsDatabase db = new clsDatabase();

            string query = string.Format("exec {0}_ReportDB.dbo.Code_CloseSession '{0}', N'{1}', '{2}'", HttpContext.Current.Session["CorpId"], HttpContext.Current.Session["EmpNo"], HttpContext.Current.Session["SessionNo"]);
            db.ExecuteQuery(query);

            //HttpContext.Current.Session.Abandon();
        }        
    }

    [WebMethod(EnableSession = true)]
    public static string SessionCheck()
    {
        return (HttpContext.Current.Session["EmpNo"] ?? "").ToString();
    }
}