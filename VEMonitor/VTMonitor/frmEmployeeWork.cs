using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ButtonsPanelControl;
using SmartMES.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VEMonitor.Models;

namespace VEMonitor
{
    public partial class frmEmployeeWork : DevExpress.XtraEditors.XtraForm
    {
        clsCode code = new clsCode();

        string containerName = "";

        DateTime noStartDate;

        public frmEmployeeWork()
        {
            InitializeComponent();

            Rectangle res = Screen.PrimaryScreen.Bounds;
            this.Location = new Point(res.Width - Size.Width - 40, 4);

            try
            {
                factorySearchLookUpEdit.Properties.DataSource = code.GetFilterTags();
                factorySearchLookUpEdit.Properties.DisplayMember = "명칭";
                factorySearchLookUpEdit.Properties.ValueMember = "코드";
                factorySearchLookUpEdit.Properties.ForceInitialize();
                factorySearchLookUpEdit.ItemIndex = 0;

                DataView reasonView = code.GetReleaseReason();

                foreach (DataRowView item in reasonView)
                {
                    reasonComobBoxEdit.Properties.Items.Add(item[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmEmployeeWork_Load(object sender, EventArgs e)
        {
            txtContainerName.Focus();

        }

        private void txtContainerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (txtContainerName.Text == "")
                {
                    timer1.Enabled = false;
                    txtStep.Text = "";
                    txtWorkTime.Text = "";
                    txtNonWorkTime.Text = "";

                    containerName = "";
                }
                else
                {
                    containerName = txtContainerName.Text;
                    DisplayWorkData();
                }
            }
        }

        private string ConvertBySecond(int sec)
        {
            int hours = sec / 3600;
            int minute = sec % 3600 / 60;

            return string.Format("{0} : {1}", hours.ToString("00"), minute.ToString("00"));
        }

        private void InitWorkControl()
        {
            txtContainerName.Text = "";
            txtStep.Text = "";
            txtWorkTime.Text = "";
            txtNonWorkTime.Text = "";
            timer1.Enabled = false;

            txtContainerName.Focus();
        }

        private void DisplayWorkData()
        {
            timer1.Enabled = false;
            DataRowView item = null;
            try
            {
                item = code.GetNowWorkTimeByContainer(containerName);
                layoutErrorItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            catch (Exception ex)
            {
                layoutErrorItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                ((DevExpress.Utils.ToolTipTitleItem)errSvgImage.SuperTip.Items[0]).Text = ex.GetType().ToString();
                ((DevExpress.Utils.ToolTipItem)errSvgImage.SuperTip.Items[1]).Text = ex.Message;

                txtContainerName.Focus();
                timer1.Enabled = true;

                return;
            }

            if (item == null)
            {
                InitWorkControl();
            }
            else
            {
                txtStep.Text = item["StepName"].ToString();

                txtWorkTime.Text = ConvertBySecond(Convert.ToInt32(item["WorkSecond"]));
                txtNonWorkTime.Text = ConvertBySecond(Convert.ToInt32(item["NonWorkSecond"]));

                if (item["Status"].ToString() == "2")
                {   //비가동
                    Root.AppearanceGroup.BackColor = Color.Red;
                }
                else if (item["Status"].ToString() == "1")
                {   //가동
                    Root.AppearanceGroup.BackColor = Color.DodgerBlue;
                }
                else
                {   // 완료
                    Root.AppearanceGroup.BackColor = Color.FromArgb(0, 0, 0, 0);
                }

                txtContainerName.Focus();
                timer1.Enabled = true;
            }
        }

        private void frmEmployeeWork_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
        }

        private void factorySearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            employeeSearchLookUpEdit.Properties.DataSource = code.GetEmployeeByFilter((factorySearchLookUpEdit.EditValue ?? "").ToString());
            employeeSearchLookUpEdit.Properties.DisplayMember = "명칭";
            employeeSearchLookUpEdit.Properties.ValueMember = "코드";
        }

        private void employeeSearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            reasonComobBoxEdit.EditValue = null;

            if ((employeeSearchLookUpEdit.EditValue ?? "").ToString() == "")
            {
                timer2.Enabled = false;
            }
            else
            {
                timer2.Enabled = true;
            }

            DisplayNoData(false);
        }
        //무가동 시작
        private void btnStart_Click(object sender, EventArgs e)
        {
            if ((employeeSearchLookUpEdit.EditValue ?? "").ToString() == "" || (employeeSearchLookUpEdit.EditValue ?? "").ToString() == "--작업자--")
            {
                this.Text = "!알림 : 작업자를 선택하세요.";
                return;
            }

            this.Text = "";

            code.GetNoOpeartionStart("0", (employeeSearchLookUpEdit.EditValue ?? "").ToString(), "");
            this.Text = "!알림 : 무가동 시작.";

            DisplayNoData(false);
        }
        // 무가동 종료
        private void btnEnd_Click(object sender, EventArgs e)
        {
            if ((employeeSearchLookUpEdit.EditValue ?? "").ToString() == "" || (employeeSearchLookUpEdit.EditValue ?? "").ToString() == "--작업자--")
            {
                this.Text = "!알림 : 작업자를 선택하세요.";
                return;
            }
            if ((reasonComobBoxEdit.EditValue ?? "").ToString() == "" || (reasonComobBoxEdit.EditValue ?? "").ToString() == "--무가동사유--")
            {
                this.Text = "!알림 : 무가동 사유를 선택하세요.";
                return;
            }

            this.Text = "";

            code.GetNoOpeartionEnd("0", (employeeSearchLookUpEdit.EditValue ?? "").ToString(), (reasonComobBoxEdit.EditValue ?? "").ToString());
            this.Text = "!알림 : 가동 시작.";
            reasonComobBoxEdit.EditValue = null;

            DisplayNoData(false);
        }

        private void DisplayNoData(bool isTimer = false)
        {
            DataRowView item = null;
            try
            {
                item = code.GetNoOpeartionTimeByEmployee((employeeSearchLookUpEdit.EditValue ?? "").ToString());
                layoutErrorItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            catch (Exception ex)
            {
                layoutErrorItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                ((DevExpress.Utils.ToolTipTitleItem)errSvgImage2.SuperTip.Items[0]).Text = ex.GetType().ToString();
                ((DevExpress.Utils.ToolTipItem)errSvgImage2.SuperTip.Items[1]).Text = ex.Message;
                return;
            }

            if (item == null)
            {
                btnStart.Enabled = true;
                btnEnd.Enabled = false;
            }
            else
            {
                if (item["Gubun"].ToString() == "0")
                {   // 무가동중
                    noStartDate = Convert.ToDateTime(item["StartDate"]).AddSeconds(-1 * Convert.ToInt32(item["NoTime"]));
                    TimeSpan dateDiff = DateTime.Now - noStartDate;
                    txtNoTime.Text = ConvertBySecond(Convert.ToInt32(dateDiff.TotalSeconds));
                    btnStart.Enabled = false;
                    btnEnd.Enabled = true;
                    Root2.AppearanceGroup.BackColor = Color.Red;
                }
                else
                {   // 가동중
                    txtNoTime.Text = ConvertBySecond(Convert.ToInt32(item["NoTime"]));
                    btnStart.Enabled = true;
                    btnEnd.Enabled = false;
                    Root2.AppearanceGroup.BackColor = Color.DodgerBlue;
                }
            }

            if (!isTimer)
            {
                txtContainerName.Focus();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DisplayWorkData();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DisplayNoData(true);
        }

    }
}