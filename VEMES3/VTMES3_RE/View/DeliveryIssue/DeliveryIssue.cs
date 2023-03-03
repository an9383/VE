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
using DevExpress.XtraGrid.Views.Grid;

namespace VTMES3_RE.View.DeliveryIssue
{
    public partial class DeliveryIssue : DevExpress.XtraEditors.XtraForm
    {
        //[DXCategory("Appearance")]
        //public event RowStyleEventHandler RowStyle;
        public DeliveryIssue()
        {            
            InitializeComponent();

            barEditStartDate.EditValue = DateTime.Today;
            barEditEndDate.EditValue = DateTime.Today;   
            
            deliveryIssueBindingSource.AllowNew = false;
            
        }
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {               

                string category = View.GetRowCellDisplayText(e.RowHandle, View.Columns[4]);
                if (category == "O")
                {                    
                }
                else 
                {
                    e.Appearance.BackColor = Color.Pink;
                    e.HighPriority = true;
                }
            }
        }

        private void DeliveryIssue_Load(object sender, EventArgs e)
        {
            this.deliveryIssueTableAdapter.InsertDeliveryIssue();
            //this.deliveryIssueTableAdapter.Fill(iFVEDataSet.DeliveryIssue);
            DisplayData();

            (this.gcDeliveryIssue.MainView as GridView).RowStyle += gridView1_RowStyle;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DisplayData();
        }
        private void DisplayData()
        {
            
            this.deliveryIssueTableAdapter.FillByDate(this.iFVEDataSet.DeliveryIssue, (DateTime)barEditStartDate.EditValue, (DateTime)barEditEndDate.EditValue);
            
        }
        
        private void cmdSave_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            try
            {
                this.Validate();

                foreach (DataRowView drv in deliveryIssueBindingSource.List)
                {
                    if (drv.Row.RowState == DataRowState.Added)
                    {
                    }
                    else if (drv.Row.RowState == DataRowState.Modified)
                    {
                        drv["ModId"] = WrGlobal.LoginID;
                        drv["ModDt"] = DateTime.Now;                        
                    }                
                    
                }
                
                deliveryIssueBindingSource.EndEdit();
                deliveryIssueTableAdapter.Update(this.iFVEDataSet.DeliveryIssue);

                MessageBox.Show("저장되었습니다.", "저장", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DisplayData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdClose_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            this.Close();
        }
    }
}