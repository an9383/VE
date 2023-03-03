using DevExpress.DashboardCommon.DataProcessing;
using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_Report_TACheckSheet : System.Web.UI.Page
{
    protected string pstrReportId = "";
    protected string pstrErrMsg = "";
    protected clsDatabase db = new clsDatabase();
    protected string query = "";
    protected DataRowView master = null;

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

        }

        if ((Request.QueryString["BrandName"] ?? "") == "")
        {
            pstrErrMsg = "레포트를 가져올수 없습니다.";
            return;
        }

        query = "SELECT [ReportId]" +
        "" +
        "  FROM [VE_ReportDB].[dbo].[ReportItem]" +
        "" +
        "  WHERE ',' + Bigo + ',' LIKE '%," + Request.QueryString["BrandName"] + ",%'";

        DataView dv = db.GetDataView("ReportItem", query);

        if (dv.Count == 0) 
        {
            pstrErrMsg = "레포트를 가져올수 없습니다.";
            return;
        }



        pstrReportId = dv[0][0].ToString();
        //pstrReportId = "14";

        master = IsExistReportItem(pstrReportId);

        if (master == null)
        {
            pstrErrMsg = "선택된 메뉴는 대시보드 작성 폼이 아닙니다.";
            return;
        }

        if (master["LayoutData"].ToString() == "")
        {
            pstrErrMsg = "선택된 메뉴는 대시보드가 작성되지 않았습니다.";
            return;
        }

        master["LayoutData"] = master["LayoutData"].ToString().Replace("?<?", "<?");

        XtraReport rpt = new XtraReport();   
        // Obtain the report from the string.
        using (StreamWriter sw = new StreamWriter(new MemoryStream()))
        {
            sw.Write(master["LayoutData"]);
            sw.Flush();
            rpt.LoadLayoutFromXml(sw.BaseStream);
        }

        foreach (DevExpress.XtraReports.Parameters.Parameter param in rpt.Parameters)
        {
            if (Request.QueryString[param.Name] != null)
            {
                param.Value = Request.QueryString[param.Name];
            }
        }

        rpt.RequestParameters = false;

        WebCodumentViewer.OpenReport(rpt);

        //WebCodumentViewer.OpenReportXmlLayout(System.Text.Encoding.UTF8.GetBytes(master["LayoutData"].ToString()));
    }

    protected DataRowView IsExistReportItem(string reportId)
    {
        query = string.Format("Select * From {0}_ReportDB.dbo.ReportItem Where CorpId = '{0}' and ReportId = {1}",
                        Session["CorpId"], reportId);

        return db.GetDataRecord(query);
    }
}