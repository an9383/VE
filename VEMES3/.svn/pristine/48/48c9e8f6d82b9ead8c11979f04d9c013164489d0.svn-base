using DevExpress.XtraEditors.Repository;
using VTMES3_RE.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.TextEditController.Win32;
using System.Reflection;

namespace VTMES3_RE.Models
{
    public class clsCode
    {
        Database db = new Database();
        string query = "";

        public DataView GetAuthorViewByGroupCode(string groupcode)
        {
            query = string.Format("exec {0}_ReportDB.dbo.Code_GetCodeAuthorByGroupCode '{0}', '{1}'", WrGlobal.CorpID, groupcode);

            return db.GetDataView("사용자권한", query);
        }

        public string GetNewAuthorGroupCode()
        {
            query = string.Format("SELECT CONVERT(NVARCHAR(4), ISNULL(MAX(GroupCode), 1000) + 1) GroupCode FROM {0}_ReportDB.dbo.CodeAuthorGroup Where CorpId = '{0}'", WrGlobal.CorpID);
            DataRowView drv = db.GetDataRecord(query);

            return drv["GroupCode"].ToString();
        }

        public void ExecuteAuthorGroupDetailDeleteByGroupCode(string groupcode)
        {
            db.ExecuteQuery(string.Format("DELETE FROM {0}_ReportDB.dbo.CodeAuthorGroupDetail WHERE CorpId = '{0}' and GroupCode = '{1}'", WrGlobal.CorpID, groupcode));
            db.ExecuteQuery(string.Format("UPDATE {0}_ReportDB.dbo.CodeUser SET AuthorGroup = REVERSE(STUFF(REVERSE(Replace(Replace(AuthorGroup, ' ', '') + ',', '{1},', '')), 1, 1, '')) Where CorpId = '{0}'", WrGlobal.CorpID, groupcode));
        }
 
        public DataView GetEmployeeMenuList()
        {
            query = string.Format("exec {0}_ReportDB.dbo.Code_GetEmployeeMenuList '{0}', '{1}', '{2}'", WrGlobal.CorpID, WrGlobal.LoginID, WrGlobal.ProgramType);
            return db.GetDataView("MenuGroup", query);
        }


        public DataRowView GetUserInfo(string uid)
        {
            query = string.Format("SELECT cu.*, emp.FullName FROM {0}_ReportDB.dbo.CodeUser cu "
                                + "Inner Join CAMDBsh.Employee emp on cu.EmployeeName = emp.EmployeeName "
                            + "WHERE cu.CorpId = '{0}' and cu.EmployeeName = '{1}'", WrGlobal.CorpID, uid);
            return db.GetDataRecord(query);
        }

        public DataRowView AddUser(string uid, string upw)
        {
            query = string.Format("Insert Into {0}_ReportDB.dbo.CodeUser(CorpId, EmployeeName, Password) Values('{0}', '{1}', '{2}')", WrGlobal.CorpID, uid, clsCommon.SHA256Hash(upw));
            db.ExecuteQuery(query);

            return GetUserInfo(uid);
        }

        public DataView GetUser()
        {
            query = string.Format("Select Employee.EmployeeName ID, Employee.FullName 사용자명, Factory.FactoryName 사업부 "
                             + "From CAMDBsh.Employee "
                                 + "Inner Join CAMDBsh.SessionValues on Employee.EmployeeId = SessionValues.EmployeeId "
                                 + "LEFT Join CAMDBsh.Factory on SessionValues.FactoryId = Factory.FactoryId "
                             + "Where Factory.FactoryName = 'VTENG' OR Factory.FactoryName IS NULL "
                             + "Order By Factory.FactoryName, Employee.FullName");
            return db.GetDataView("사용자", query);
        }

        //test
        public void SetUserAuthor()
        {
            query = string.Format("SELECT AuthorGroup FROM {0}_ReportDB.dbo.CodeUser WHERE CorpId = '{0}' and EmployeeName = '{1}'", WrGlobal.CorpID, WrGlobal.LoginID);
            DataRowView drv = db.GetDataRecord(query);

            if (drv[0] == DBNull.Value) return;

            drv[0] = drv[0].ToString().Replace(" ", "");

            string[] arrGroup = drv[0].ToString().Split(new char[] { ',' });
            string inGroupStr = "";

            foreach (string item in arrGroup)
            {
                if (item == "") continue;

                if (inGroupStr != "") inGroupStr = inGroupStr + ",";
                inGroupStr = inGroupStr + "''" + item + "''";
                                                   }

            query = string.Format("exec {0}_ReportDB.dbo.Code_GetAuthorByAuthorGroups '{0}', '{1}', '{2}'", WrGlobal.CorpID, WrGlobal.ProgramType, inGroupStr);
            DataView dv = db.GetDataView("권한", query);

            foreach (DataRowView view in dv)
            {
                clsAuthor author = new clsAuthor(view["MenuID"].ToString(), view["ParentID"].ToString(), view["AuthorCode"].ToString());
                WrGlobal.AuthorList.Add(author);
            }
        }

        public DataView GetGroupAuthorList()
        {
            query = string.Format("SELECT GroupCode, GroupName FROM {0}_ReportDB.dbo.CodeAuthorGroup Where CorpId = '{0}' ORDER BY GroupName", WrGlobal.CorpID);
            return db.GetDataView("CodeAuthorGroup", query);
        }

        public bool ChangePassword(string frpwd, string topwd)
        {
            bool bRet = false;

            query = string.Format("SELECT * FROM {0}_ReportDB.dbo.CodeUser WHERE CorpId = '{0}' and EmployeeName = '{1}'", WrGlobal.CorpID, WrGlobal.LoginID);

            DataRowView drv = db.GetDataRecord(query);

            if (clsCommon.SHA256Hash(frpwd) == clsCommon.getString(drv["Password"]))
            {
                query = string.Format("UPDATE {0}_ReportDB.dbo.CodeUser SET Password = '{2}' WHERE CorpId = '{0}' and EmployeeName = '{1}'", WrGlobal.CorpID, WrGlobal.LoginID, clsCommon.SHA256Hash(topwd));
                bRet = db.ExecuteQuery(query);
            }
            else
            {
                bRet = false;
            }

            return bRet;
        }


        public bool ExecuteAuthorGroupDetailQueryList(List<string> list)
        {
            return db.ExecuteQueryList(list);
        }

        public int GetNextSequence(string seqName)
        {
            int nextSeq = 1;
            query = string.Format("SELECT NEXT VALUE FOR {0}_ReportDB.dbo.{1}", WrGlobal.CorpID, seqName);
            DataRowView drv = db.GetDataRecord(query);

            if (drv != null)
            {
                nextSeq = Convert.ToInt32(drv[0]);
            }

            return nextSeq;
        }

        public void SetDashboadMenuItem()
        {
            query = string.Format("Insert Into {0}_ReportDB.dbo.DashBoardItem(CorpId, MenuId, CreId, CreIP, CreDt) "
                            + "Select mg.CorpId, mg.Id MenuId, '{1}', HOST_NAME(), getdate() "
                                + "From {0}_ReportDB.dbo.MenuGroup mg "
                                    + "Left Join {0}_ReportDB.dbo.DashBoardItem bi on mg.CorpId = bi.CorpId and mg.Id = bi.MenuId "
                                + "Where mg.CorpId = '{0}' and bi.MenuId is null", 
                                WrGlobal.CorpID, WrGlobal.LoginID);

            db.ExecuteQuery(query);
        }

        public DataRowView IsExistDashboardItem(string menuId)
        {
            query = string.Format("Select bi.*, mg.Title_ko MenuName, pmg.Title_ko ParentMenuName "
                                + "From {0}_ReportDB.dbo.DashBoardItem bi "
                                    + "Inner Join {0}_ReportDB.dbo.MenuGroup mg on bi.CorpId = mg.CorpId and bi.MenuId = mg.Id "
                                    + "Left Join {0}_ReportDB.dbo.MenuGroup pmg on mg.CorpId = pmg.CorpId and mg.ParentId = pmg.Id "
                                + "Where bi.CorpId = '{0}' and bi.MenuId = '{1}'",
                            WrGlobal.CorpID, menuId);

            return db.GetDataRecord(query);
        }

        public void SetCodeAuthorByMenu()
        {
            query = string.Format("exec {0}_ReportDB.dbo.Code_SetCodeAuthorByMenu '{0}'",
                                WrGlobal.CorpID);

            db.ExecuteQuery(query);
        }
        //세션 생성
        public void CreateSession()
        {
            WrGlobal.SessionNo = DateTime.Now.ToString("yyyyMMddHHmmss");

            query = string.Format("exec {0}_ReportDB.dbo.Code_CreateSession '{0}', N'{1}', '{2}', '{3}'", WrGlobal.CorpID, WrGlobal.LoginID, WrGlobal.SessionNo, "CS");
            db.ExecuteQuery(query);
        }
        //세션 종료
        public void CloseSession()
        {
            query = string.Format("exec {0}_ReportDB.dbo.Code_CloseSession '{0}', N'{1}', '{2}'", WrGlobal.CorpID, WrGlobal.LoginID, WrGlobal.SessionNo);
            db.ExecuteQuery(query);
        }
        //메뉴 사용
        public void UseSession(string menuId, string useType)
        {
            query = string.Format("exec {0}_ReportDB.dbo.Code_UseSession '{0}', N'{1}', '{2}', '{3}', N'{4}'", WrGlobal.CorpID, WrGlobal.LoginID, WrGlobal.SessionNo, menuId, useType);
            db.ExecuteQuery(query);
        }

        public DataView GetBrandNameList()
        {
            query = string.Format("exec [CAMDBsh].[VE_VR_GetBrandNameList]");
            return db.GetDataView("BrandNameList", query);
        }

        public DataView GetSpecNameList()
        {
            query = string.Format("exec [CAMDBsh].[VE_VR_GetSpecList]");
            return db.GetDataView("SpecNameList", query);
        }
    }
}
