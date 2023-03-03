using DevExpress.Xpo.DB.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class View_Main : System.Web.UI.Page
{
    clsDatabase db = new clsDatabase();
    string _query;

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["EmpNo"] ?? "").ToString() == "")
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            // 프로필사진
            _query = string.Format("select Photo from {0}_ReportDB.dbo.CodeUser Where CorpId = '{0}' and EmployeeName = '{1}'", Session["CorpId"].ToString(), Session["EmpNo"].ToString());
            DataRowView drv = db.GetDataRecord(_query);

            if (drv != null && drv["Photo"] != DBNull.Value)
            {
                byte[] imagem = (byte[])drv["Photo"];
                string base64String = Convert.ToBase64String(imagem);
                imgUser.Src = String.Format("data:image/jpg;base64,{0}", base64String);
            }
            //else
            //{
            //    if (drv != null)
            //    {
            //        if (drv["Sex"].ToString() == "001")
            //        {
            //            imgUser.Src = "../Images/man.png";
            //        }
            //        else
            //        {
            //            imgUser.Src = "../Images/woman.png";
            //        }
            //    }
            //}

            SetUserSideMenu();
        }
    }
    // 사용자 권한 리스트 -> 문자열
    private void SetUserSideMenu()
    {
        _query = string.Format("exec {0}_ReportDB.dbo.Code_GetEmployeeMenuList '{0}', '{1}', '{2}'",
                    Session["CorpId"].ToString(), Session["EmpNo"].ToString(), Session["ProgramType"].ToString());

        DataView dv = db.GetDataView("MenuGroup", _query);

        if (dv != null)
        {
            string sideMenu = "", sideIconCss = "", preParentId = "";
            int preLevel = 1;


            for (int i = 0; i < dv.Count; i++)
            {
                if (Convert.ToInt32(dv[i]["GroupLevel"]) == 0)
                {
                    preParentId = dv[i]["ParentId"].ToString();
                    continue;
                }

                if (preLevel > Convert.ToInt32(dv[i]["GroupLevel"]))
                {   //이전과 레벨이 다를때
                    for (int k = 0; k < preLevel - Convert.ToInt32(dv[i]["GroupLevel"]); k++)
                    {
                        sideMenu = sideMenu + Environment.NewLine + "</ul></li>";
                    }
                }

                sideIconCss = dv[i]["WebImageKey"].ToString();
                if (sideIconCss != "")
                {
                    sideIconCss = string.Format("<i class=\"{0}\"></i>", sideIconCss);
                }

                if (dv[i]["GroupYn"].ToString() == "1")
                {   //그룹 메뉴이면
                    sideMenu = sideMenu + Environment.NewLine + string.Format("<li><a href=\"javascript: void(0);\" class=\"waves-effect\">{0}<span> {1} <span class=\"float-right menu-arrow\"><i class=\"mdi mdi-chevron-right\"></i></span> </span></a>"
                                                    + "<ul class=\"submenu\">",
                                    sideIconCss, dv[i]["Title_ko"].ToString());

                }
                else
                {
                    sideMenu = sideMenu + Environment.NewLine + string.Format("<li id=\"m{0}\">"
                                + "<a href=\"javascript:MoveToLocationByMenuClick('{0}', '{1}', '{2}')\" class=\"waves-effect\">{3}</i><span> {4} </span></a>"
                                + "</li>",
                                    dv[i]["Id"].ToString(), dv[i]["Title_ko"].ToString(), dv[i]["FormName"].ToString(), sideIconCss, dv[i]["Title_ko"].ToString());
                }

                preLevel = Convert.ToInt32(dv[i]["GroupLevel"]);
                preParentId = dv[i]["ParentId"].ToString();

            }

            ltlSideMenu.Text = sideMenu;
        }
    }
}