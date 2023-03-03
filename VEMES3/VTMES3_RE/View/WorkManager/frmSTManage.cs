using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTMES3_RE.Common;
using VTMES3_RE.Models;

namespace VTMES3_RE.View.WorkManager
{
    public partial class frmSTManage : DevExpress.XtraEditors.XtraForm
    {
        clsCode code = new clsCode();
        public frmSTManage()
        {
            InitializeComponent();


            ModelcolmnSearchLookUpEdit.DataSource = code.GetBrandNameList();
            ModelcolmnSearchLookUpEdit.DisplayMember = "BrandName";
            ModelcolmnSearchLookUpEdit.ValueMember = "BrandName";

            SpecNameSearchLookUpEdit.DataSource = code.GetSpecNameList();
            SpecNameSearchLookUpEdit.DisplayMember = "SpecName";
            SpecNameSearchLookUpEdit.ValueMember = "SpecName";

        }

        private void frmSTManage_Load(object sender, EventArgs e)
        {
            // TODO: 이 코드는 데이터를 'iFVEDataSet.VE_StTime' 테이블에 로드합니다. 필요 시 이 코드를 이동하거나 제거할 수 있습니다.
            this.vE_StTimeTableAdapter.Fill(this.iFVEDataSet.VE_StTime);

        }

        private void cmdInsert_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            vE_StTimeBindingSource.AddNew();
        }

        private void cmdDelete_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            string Model = gvVeSTTime.GetFocusedRowCellDisplayText("Model") == null ? "" : gvVeSTTime.GetFocusedRowCellDisplayText("Model");
            string Process = gvVeSTTime.GetFocusedRowCellDisplayText("Process") == null ? "" : gvVeSTTime.GetFocusedRowCellDisplayText("Process");

            if (MessageBox.Show(string.Format("선택한 '{0}'의 '{1}'을 삭제하시겠습니까?", Model, Process), "삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }//end if

            vE_StTimeBindingSource.RemoveCurrent();//선택된 Row 삭제
            vE_StTimeTableAdapter.Update(iFVEDataSet.VE_StTime);//삭제된 Row를 DB에 반영
            MessageBox.Show("선택된 정보가 삭제 되었습니다.");
        }

        private void cmdClose_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }

        private void cmdSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                this.Validate();

                foreach (DataRowView drv in vE_StTimeBindingSource)
                {
                    if (drv.Row.RowState == DataRowState.Added)
                    {
                        drv["cUser"] = WrGlobal.LoginID;
                        drv["cDate"] = DateTime.Now;
                    }
                    else if (drv.Row.RowState == DataRowState.Modified)
                    {
                        drv["uUser"] = WrGlobal.LoginID;
                        drv["uDate"] = DateTime.Now;
                    }
                }

                vE_StTimeBindingSource.EndEdit();
                vE_StTimeTableAdapter.Update(iFVEDataSet.VE_StTime);
                MessageBox.Show("수정/입력 정보를 저장했습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}