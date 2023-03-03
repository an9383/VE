using DevExpress.DataAccess.ConnectionParameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class TabsContent_ucDashboardView : System.Web.UI.UserControl
{
    protected string pstrMenuId = "";
    protected clsDatabase db = new clsDatabase();
    protected string query = "";
    protected DataRowView master = null;
    protected string pstrErrMsg = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        pstrMenuId = this.ID.TrimStart(new char[] { 'u', 'c' });

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
        WebDashboard.OpenDashboard(XDocument.Load(ms));
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


    protected void WebDashboard_DashboardLoading(object sender, DevExpress.DashboardWeb.DashboardLoadingWebEventArgs e)
    {

    }
}