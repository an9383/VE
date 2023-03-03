using DevExpress.DashboardCommon;
using DevExpress.Utils.DragDrop;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using VTMES3_RE.Common;
using VTMES3_RE.Models;
using VTMES3_RE.View.Dashboards.Tools;
using VTMES3_RE.View.Reports.Tools;

namespace VTMES3_RE.View.Systems
{
    public partial class frmSetMenu : DevExpress.XtraEditors.XtraForm
    {
        clsCode code = new clsCode();

        public frmSetMenu()
        {
            InitializeComponent();
        }

        private void frmSetMenu_Load(object sender, EventArgs e)
        {
            DisplayMenu();
        }

        private void DisplayMenu()
        {
            this.menuGroupTableAdapter.FillByList(codeDataSet.MenuGroup, WrGlobal.CorpID);
            MenuTreeList.ExpandAll();
        }

        private void cmdSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                this.Validate();

                SetNodeLevel(MenuTreeList.Nodes);

                int cnt = 0;

                List<TreeListNode> Listnodes = MenuTreeList.GetNodeList();

                foreach (TreeListNode node in Listnodes)
                {
                    MenuTreeList.SetRowCellValue(node, "RowNum", ++cnt);
                }

                foreach (DataRowView drv in menuGroupBindingSource)
                {
                    if (drv.Row.RowState == DataRowState.Added)
                    {
                        drv["CreId"] = WrGlobal.LoginID;
                        drv["CreIP"] = WrGlobal.ClientHostName;
                        drv["CreDt"] = DateTime.Now;
                    }
                    else
                    {
                        drv["ModId"] = WrGlobal.LoginID;
                        drv["ModIP"] = WrGlobal.ClientHostName;
                        drv["ModDt"] = DateTime.Now;
                    }
                    //drv["RowNum"] = ++cnt;
                }
                menuGroupBindingSource.EndEdit();

                this.menuGroupTableAdapter.Update(codeDataSet.MenuGroup);

                code.SetDashboadMenuItem();
                code.SetCodeAuthorByMenu();

                MessageBox.Show("저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdInsert_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                menuGroupBindingSource.AddNew();
                DataRowView newItem = (DataRowView)menuGroupBindingSource.Current;

                newItem["CorpId"] = WrGlobal.CorpID;
                newItem["Id"] = code.GetNextSequence("MenuId_Seq").ToString();
                newItem["ParentId"] = "10000";
                newItem["ProjectName"] = WrGlobal.ProJectName;
                newItem["GroupLevel"] = 1;
                newItem["GroupYn"] = "0";
                newItem["ExpandYn"] = "0";
                newItem["PopupYn"] = "N";
                newItem["UseYn"] = "Y";

                newItem["CreDt"] = DateTime.Now;
                newItem["CreId"] = WrGlobal.LoginID;
                newItem["CreIP"] = WrGlobal.ClientHostName;

                menuGroupBindingSource.EndEdit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetNodeLevel(TreeListNodes nodes)
        {
            if (nodes.Count == 0) return;

            foreach(TreeListNode node in nodes)
            {
                node.SetValue("GroupLevel", node.Level);
                if (node.Nodes.Count > 0)
                {
                    node.SetValue("GroupYn", "1");
                    SetNodeLevel(node.Nodes);
                }
                else
                {
                    node.SetValue("GroupYn", "0");
                }
            }
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

        private void MenuTreeList_CustomNodeCellEdit(object sender, DevExpress.XtraTreeList.GetCustomNodeCellEditEventArgs e)
        {
            if (e.Column.FieldName == "NodeDel")
            {
                if (e.Node.ParentNode == null)
                {
                    e.RepositoryItem = btnNoDelete;
                }
                else
                {
                    e.RepositoryItem = btnNodeDelete;
                }
            }
        }

        private void btnNodeDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (MessageBox.Show("선택한 항목을 삭제 하시겠습니까?\r\n하위항목이 존재하는 경우 하위 항목도 같이 삭제됩니다.", "항목삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                List<TreeListNode> nodes = new List<TreeListNode>();
                nodes.Add(MenuTreeList.FocusedNode);
                DropNode(nodes);
            }
        }

        // 노드 삭제
        void DropNode(IEnumerable<TreeListNode> nodes)
        {
            List<TreeListNode> _nodes = new List<TreeListNode>(nodes);
            foreach (TreeListNode node in _nodes)
            {
                if (node.HasChildren)
                    DropNode(node.Nodes);
                DataRowView rowView = MenuTreeList.GetRow(node.Id) as DataRowView;
                if (rowView == null)
                    return;
                MenuTreeList.Nodes.Remove(node);
            }
        }

        private void cmdClose_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void cmdDashboardDesign_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            TreeListNode fNode = MenuTreeList.FocusedNode;

            DataRowView drv = code.IsExistDashboardItem((fNode.GetValue("Id") ?? "").ToString());

            if (drv == null)
            {
                MessageBox.Show("신규 등록된 메뉴는 저장 후 작성하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmDashBoardDesign form = new frmDashBoardDesign(drv);
            form.ShowDialog();
        }

        private void cmdDashboardView_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            TreeListNode fNode = MenuTreeList.FocusedNode;

            DataRowView drv = code.IsExistDashboardItem((fNode.GetValue("Id") ?? "").ToString());

            if (drv == null || drv["XML"].ToString() == "")
            {
                MessageBox.Show("대시보드가 작성되지 않았습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmDashBoardView form = new frmDashBoardView(drv);
            form.ShowDialog();
        }

        private void cmdDisplay_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            DisplayMenu();
        }
    }
}