using DevExpress.XtraGrid.Views.Grid;
using VTMES3_RE.Common;
using VTMES3_RE.Models;
using System;
using System.Data;
using System.Windows.Forms;

namespace VTMES3_RE.View.Systems
{
    public partial class frmUserView : DevExpress.XtraEditors.XtraForm
    {
        clsCode user = new clsCode();

        public frmUserView()
        {
            InitializeComponent();

            authorCheckedComboBoxEdit.DataSource = user.GetGroupAuthorList();
        }
        private void frmUserView_Load(object sender, EventArgs e)
        {
            cmdDisplay_ElementClick(null, null);
        }

        private void DisplayData()
        {
            codeUserTableAdapter.FillByList(codeDataSet.CodeUser, WrGlobal.CorpID);
        }

        // 저장
        private void cmdSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                this.Validate();

                foreach (DataRowView drv in codeUserBindingSource.List)
                {
                    if (drv.Row.RowState == DataRowState.Added || drv.Row.RowState == DataRowState.Modified)
                    {
                        if (drv["Password2"].ToString() != "")
                        {
                            drv["Password"] = clsCommon.SHA256Hash(drv["Password2"].ToString());
                            drv["Password2"] = "";
                        }
                    }
                }

                codeUserBindingSource.EndEdit();
                codeUserTableAdapter.Update(codeDataSet.CodeUser);
                MessageBox.Show("사용자 정보를 저장했습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 삭제
        private void cmdDelete_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            string Id = gvCodeUser.GetFocusedRowCellDisplayText("EmployeeName") == null ? "" : gvCodeUser.GetFocusedRowCellDisplayText("EmployeeName");

            if (MessageBox.Show(string.Format("선택한 '{0}' 사용자를 삭제하시겠습니까?", Id), "삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }//end if

            codeUserBindingSource.RemoveCurrent();
            codeUserTableAdapter.Update(codeDataSet.CodeUser);
            MessageBox.Show("자료가 삭제 되었습니다.");
        }
        // 엑셀출력
        private void cmdExcel_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Worksheets|*.Xls";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                gvCodeUser.ExportToXls(sfd.FileName);
                System.Diagnostics.Process.Start(sfd.FileName);
            }//end fnction
        }
        // 닫기
        private void cmdClose_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
        // 조회
        private void cmdDisplay_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            DisplayData();
        }

        private void gvUser_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.RowHandle == view.FocusedRowHandle)
            {
                //Apply the appearance of the SelectedRow
                e.Appearance.Assign(view.PaintAppearance.SelectedRow);
                e.Appearance.Options.UseForeColor = true;
            }//end if
        }

    }
}