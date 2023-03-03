using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using VTMES3_RE.Common;
using VTMES3_RE.Models;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.CodeParser;

namespace VTMES3_RE.View.Systems
{
    public partial class frmAuthorGroup : DevExpress.XtraEditors.XtraForm
    {
        clsCode user = new clsCode();

        public frmAuthorGroup()
        {
            InitializeComponent();

        }

        private void frmAuthorGroup_Load(object sender, EventArgs e)
        {
            //if (WrGlobal.AuthorList.Contains((this.Tag ?? "").ToString() + "02"))
            //{
            //    cmdInsert.Visible = true;
            //    cmdSave.Visible = true;
            //    cmdDelete.Visible = true;
            //}

            this.codeAuthorGroupTableAdapter.Fill(this.codeDataSet.CodeAuthorGroup, WrGlobal.CorpID);
        }

        private void cmdInsert_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            string newItemYn = "N";
            foreach (DataRowView item in codeAuthorGroupBindingSource)
            {
                if (item["GroupCode"].ToString() == "")
                {
                    newItemYn = "Y";
                    break;
                }
            }

            if (newItemYn == "Y") return;

            try
            {
                codeAuthorGroupBindingSource.AddNew();
                DataRowView newItem = (DataRowView)codeAuthorGroupBindingSource.Current;

                newItem["CorpId"] = WrGlobal.CorpID;
                newItem["GroupCode"] = "";
                newItem["GroupName"] = "";

                newItem["CreDt"] = DateTime.Now;
                newItem["CreId"] = WrGlobal.LoginID;
                newItem["CreIP"] = WrGlobal.ClientHostName;

                codeAuthorGroupBindingSource.EndEdit();

                gvCodeAuthorGroup.MoveLast();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void gvCodeAuthorGroup_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvCodeAuthorGroup.FocusedRowHandle < 0) return;

            this.menuGroupTableAdapter.FillByAuthorGroup(codeDataSet.MenuGroup, WrGlobal.CorpID, gvCodeAuthorGroup.GetRowCellValue(e.FocusedRowHandle, "GroupCode").ToString());
            MenuTreeList.ExpandAll();
        }

        private void cmdSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                this.Validate();

                foreach (DataRowView drv in codeAuthorGroupBindingSource)
                {
                    if (drv.Row.RowState == DataRowState.Added)
                    {
                        drv["GroupCode"] = user.GetNewAuthorGroupCode();
                    }
                    else if (drv.Row.RowState == DataRowState.Modified)
                    {
                        drv["ModDt"] = DateTime.Now;
                        drv["ModId"] = WrGlobal.LoginID;
                        drv["ModIP"] = WrGlobal.ClientHostName;
                    }
                }

                codeAuthorGroupBindingSource.EndEdit();
                this.codeAuthorGroupTableAdapter.Update(this.codeDataSet.CodeAuthorGroup);

                List<TreeListNode> Listnodes = MenuTreeList.GetNodeList();
                List<string> queryList = new List<string>();

                queryList.Add(string.Format("DELETE FROM {0}_ReportDB.dbo.CodeAuthorGroupDetail WHERE CorpId = '{0}' and GroupCode = '{1}'", WrGlobal.CorpID, gvCodeAuthorGroup.GetFocusedRowCellValue("GroupCode").ToString()));

                foreach (TreeListNode node in Listnodes)
                {
                    if (node.GetValue("GroupYn").ToString() == "1") continue;

                    if (node.GetValue("Author1").ToString() == "1")
                    {
                        queryList.Add(string.Format("INSERT INTO {0}_ReportDB.dbo.CodeAuthorGroupDetail(CorpId, GroupCode, MenuID, AuthorCode, UseYn, CreId, CreIP, CreDt) VALUES("
                                        + "'{0}', '{1}', {2}, '{3}', 'Y', '{4}', '{5}', getdate())",
                                        WrGlobal.CorpID, gvCodeAuthorGroup.GetFocusedRowCellValue("GroupCode").ToString(), node.GetValue("Id"), node.GetValue("Code1"), WrGlobal.LoginID, WrGlobal.ClientHostName));
                    }

                    if (node.GetValue("Author2").ToString() == "1")
                    {
                        queryList.Add(string.Format("INSERT INTO {0}_ReportDB.dbo.CodeAuthorGroupDetail(CorpId, GroupCode, MenuID, AuthorCode, UseYn, CreId, CreIP, CreDt) VALUES("
                                        + "'{0}', '{1}', {2}, '{3}', 'Y', '{4}', '{5}', getdate())",
                                        WrGlobal.CorpID, gvCodeAuthorGroup.GetFocusedRowCellValue("GroupCode").ToString(), node.GetValue("Id"), node.GetValue("Code2"), WrGlobal.LoginID, WrGlobal.ClientHostName));
                    }
                }

                if (user.ExecuteAuthorGroupDetailQueryList(queryList))
                {
                    MessageBox.Show("저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.menuGroupTableAdapter.FillByAuthorGroup(codeDataSet.MenuGroup, WrGlobal.CorpID, gvCodeAuthorGroup.GetFocusedRowCellValue("GroupCode").ToString());
                MenuTreeList.ExpandAll();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdDelete_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            if (codeAuthorGroupBindingSource.Current == null) return;

            if (MessageBox.Show("선택한 자료를 삭제 하시겠습니까?", "삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            try
            {
                DataRowView drv = (DataRowView)codeAuthorGroupBindingSource.Current;

                string groupcode = drv["GroupCode"].ToString();

                user.ExecuteAuthorGroupDetailDeleteByGroupCode(groupcode);

                codeAuthorGroupBindingSource.RemoveCurrent();
                codeAuthorGroupTableAdapter.Update(codeDataSet.CodeAuthorGroup);

                MessageBox.Show("삭제되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdClose_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }


        private void MenuTreeList_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Selected)
                {
                    if (e.Node.HasChildren)
                    {
                        e.NodeImageIndex = 3;
                    }
                    else
                    {
                        e.NodeImageIndex = 4;
                    }//end if
                }
                else
                {
                    if (e.Node.HasChildren)
                    {
                        e.NodeImageIndex = 1;
                    }
                    else
                    {
                        e.NodeImageIndex = 0;
                    }//end if
                }//end if

            }//end if
        }

        private void chkAuthor1_CheckedChanged(object sender, EventArgs e)
        {
            string groupYn = (MenuTreeList.GetFocusedRowCellValue("GroupYn") ?? "").ToString();
            if (groupYn != "1") return;

            TreeListNode node = MenuTreeList.FindNodeByKeyID((MenuTreeList.GetFocusedRowCellValue("Id") ?? ""));
            SetNodeCheck(node.Nodes, "Author1", ((CheckEdit)sender).Checked);
            node.SetValue("Author1", ((CheckEdit)sender).Checked ? "1" : "0");
        }
        private void chkAuthor2_CheckedChanged(object sender, EventArgs e)
        {
            string groupYn = (MenuTreeList.GetFocusedRowCellValue("GroupYn") ?? "").ToString();
            if (groupYn != "1") return;

            TreeListNode node = MenuTreeList.FindNodeByKeyID((MenuTreeList.GetFocusedRowCellValue("Id") ?? ""));
            SetNodeCheck(node.Nodes, "Author2", ((CheckEdit)sender).Checked);
            node.SetValue("Author2", ((CheckEdit)sender).Checked ? "1" : "0");
        }

        private void SetNodeCheck(TreeListNodes nodes, string colName, bool isChecked)
        {
            if (nodes.Count == 0) return;

            foreach (TreeListNode node in nodes)
            {
                if (node.GetValue("GroupYn").ToString() == "1")
                {
                    node.SetValue(colName, isChecked ? "1" : "0");
                    SetNodeCheck(node.Nodes, colName, isChecked);
                }
                else
                {
                    node.SetValue(colName, isChecked ? "1" : "0");
                }
            }
        }
    }
}