using VTMES3_RE.Common;
using VTMES3_RE.Models;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace VTMES3_RE
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        private string InitFile = Application.StartupPath + @"\ve_login.ini";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        clsCode code = new clsCode();

        public frmLogin()
        {
            InitializeComponent();

            searchEmpNo.Properties.DataSource = code.GetUser();
            searchEmpNo.Properties.DisplayMember = "사용자명";
            searchEmpNo.Properties.ValueMember = "ID";

            if (System.IO.File.Exists(InitFile))
            {
                searchEmpNo.EditValue = IniReadValue("LOGIN", "LoginID").Trim();
            }

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if ((searchEmpNo.EditValue ?? "").ToString() != "")
            {
                searchEmpNo.Focus();
            }
        }

        // 로그인
        private void btnLogin_Click(object sender, EventArgs e)
        {
            SetLogin();
        }

        private void SetLogin()
        {
            if ((searchEmpNo.EditValue ?? "").ToString() == "")
            {
                MessageBox.Show("사용자를 선택하세요", "로그인 에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                searchEmpNo.Focus();
                return;
            }
            if (txtPW.Text.Equals(""))
            {
                MessageBox.Show("Password를 입력하세요", "로그인 에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPW.Focus();
                return;
            }

            DataRowView dr = code.GetUserInfo((searchEmpNo.EditValue ?? "").ToString());

            if (dr == null)
            {
                dr = code.AddUser((searchEmpNo.EditValue ?? "").ToString(), txtPW.Text);
            }

            if (clsCommon.SHA256Hash(txtPW.Text) == clsCommon.getString(dr["Password"]))
            {
                WrGlobal.LoginID = (dr["EmployeeName"] ?? "").ToString();
                WrGlobal.LoginUserNM = (dr["FullName"] ?? "").ToString();

                //권한 세팅
                code.SetUserAuthor();
                //세션 설정
                code.CreateSession();
            }
            else
            {
                MessageBox.Show("비밀번호가 일치 하지 않습니다. 다시 입력하세요");
                txtPW.Text = "";
                txtPW.Focus();
                return;
            }

            WritePrivateProfileString("LOGIN", "LoginID", (searchEmpNo.EditValue ?? "").ToString(), InitFile);

            this.Close();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }


        private string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, InitFile);
            return temp.ToString();
        }

        private void txtPW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Return))
            {
                btnLogin.PerformClick();
            }
        }

    }
}