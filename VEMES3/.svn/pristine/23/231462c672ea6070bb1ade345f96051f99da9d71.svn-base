using DevExpress.Skins;
using DevExpress.Skins.XtraForm;
using DevExpress.Utils;
using DevExpress.XtraBars.Navigation;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using VTMES3_RE.Common;
using VTMES3_RE.Models;
using VTMES3_RE.Properties;

namespace VTMES3_RE
{
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {
        clsCode code = new clsCode();
        public frmMain()
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = System.Windows.Forms.Screen.GetBounds(MousePosition).Location;

            InitializeComponent();

            Icon = VTMES3_RE.Properties.Resources.vt_icon;
            mainAccordion.Footer.Visible = false;

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                this.Text = this.Text + " (" + System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString() + ")";
            }

            setUserAccordionMenu();
        }

        private void setUserAccordionMenu()
        {
            //DevExpress.XtraBars.Navigation.AccordionControlElement parentGroup = mainAccordion.Elements.add

            DevExpress.XtraBars.Navigation.AccordionControlElement rootGroup = null;
            DevExpress.XtraBars.Navigation.AccordionControlElement parentGroup1 = null;
            DevExpress.XtraBars.Navigation.AccordionControlElement parentGroup2 = null;
            DevExpress.XtraBars.Navigation.AccordionControlElement parentGroup3 = null;
            DevExpress.XtraBars.Navigation.AccordionControlElement addGroup = null;
            DevExpress.XtraBars.Navigation.AccordionControlElement eleItem = null;

            DataView dv = code.GetEmployeeMenuList();

            foreach (DataRowView drv in dv)
            {

                if (drv["GroupLevel"].ToString() == "0")
                {
                    rootGroup = mainAccordion.Elements.Add();
                    rootGroup.Name = drv["ID"].ToString();
                    rootGroup.Text = drv["Title_" + WrGlobal.Language].ToString();
                    rootGroup.ImageOptions.SvgImage = svgImages[drv["ImageKey"].ToString()];
                    rootGroup.ImageOptions.SvgImageSize = new Size(24, 24);
                    rootGroup.Appearance.Normal.ForeColor = Color.LightCyan;
                }
                else
                {
                    if (drv["GroupYn"].ToString() == "1")
                    {   // 그룹
                        addGroup = new DevExpress.XtraBars.Navigation.AccordionControlElement(DevExpress.XtraBars.Navigation.ElementStyle.Group);
                        addGroup.Name = drv["ID"].ToString();
                        addGroup.Text = drv["Title_" + WrGlobal.Language].ToString();

                        if (drv["ImageKey"].ToString() != "")
                        {
                            addGroup.ImageOptions.SvgImage = svgImages[drv["ImageKey"].ToString()];
                            addGroup.ImageOptions.SvgImageSize = new Size(24, 24);
                            addGroup.Appearance.Normal.ForeColor = Color.LightCyan;
                        }
                        addGroup.Expanded = drv["ExpandYn"].ToString() == "0" ? false : true;

                        if (drv["GroupLevel"].ToString() == "1")
                        {
                            rootGroup.Elements.Add(addGroup);
                            parentGroup1 = null;
                            parentGroup1 = addGroup;
                        }
                        else if (drv["GroupLevel"].ToString() == "2")
                        {
                            parentGroup1.Elements.Add(addGroup);
                            parentGroup2 = null;
                            parentGroup2 = addGroup;
                        }
                        else if (drv["GroupLevel"].ToString() == "3")
                        {
                            parentGroup2.Elements.Add(addGroup);
                            parentGroup3 = null;
                            parentGroup3 = addGroup;
                        }

                        addGroup = null;
                    }
                    else
                    {   // 아이템
                        eleItem = new DevExpress.XtraBars.Navigation.AccordionControlElement(DevExpress.XtraBars.Navigation.ElementStyle.Item);
                        eleItem.Name = drv["ID"].ToString();
                        eleItem.Text = drv["Title_" + WrGlobal.Language].ToString();
                        eleItem.Tag = drv["ProjectName"].ToString() + "|" + drv["FolderName"].ToString() + "|" + drv["FormName"].ToString();
                        if (drv["ImageKey"].ToString() != "")
                        {
                            eleItem.ImageOptions.SvgImage = svgImages[drv["ImageKey"].ToString()];
                            eleItem.ImageOptions.SvgImageSize = new Size(24, 24);
                        }

                        if (drv["GroupLevel"].ToString() == "1")
                        {
                            rootGroup.Elements.Add(eleItem);
                        }
                        else if (drv["GroupLevel"].ToString() == "2")
                        {
                            parentGroup1.Elements.Add(eleItem);
                        }
                        else if (drv["GroupLevel"].ToString() == "3")
                        {
                            parentGroup2.Elements.Add(eleItem);
                        }
                        else if (drv["GroupLevel"].ToString() == "4")
                        {
                            parentGroup3.Elements.Add(eleItem);
                        }

                        eleItem = null;
                    }
                }
            }
        }

        private void mainAccordion_ElementClick(object sender, ElementClickEventArgs e)
        {
            if (e.Element.Style == DevExpress.XtraBars.Navigation.ElementStyle.Group) return;
            if (e.Element.Tag == null) return;

            string[] arrTag = e.Element.Tag.ToString().Split(new char[] { '|' });

            if (!IsOpen(arrTag[2], e.Element.Name))
            {
                string temp = arrTag[0] + "." + arrTag[1] + "." + arrTag[2] + ",VEMES3";
                WrGlobal.OpeningMenuId = e.Element.Name;
                var frm = Activator.CreateInstance(Type.GetType(temp)) as Form;
                frm.Name = arrTag[2];
                frm.Text = e.Element.Text;
                frm.Tag = e.Element.Name;
                doOpenForm(frm);
            }
        }

        private void doOpenForm(Form frm)
        {
            if (frm.GetType().Name == "frmPasswordChange")
            {
                Rectangle r = this.RectangleToScreen(this.Bounds);
                frm.Left = r.Left + (r.Width - this.Width) / 2;
                frm.Top = r.Top + (r.Height - this.Height) / 4;

                if (frm.ShowDialog() == DialogResult.OK)
                {

                }
            }
            else
            {
                code.UseSession((frm.Tag ?? "").ToString(), "Open");

                frm.MdiParent = this;
                frm.Closed += new EventHandler(MDIChildrenCleanup);
                frm.Dock = DockStyle.Fill;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
                Application.DoEvents();
                frm.BringToFront();

            }

        }//end function

        private bool IsOpen(string frmName, string tagName)
        {
            bool m_fReturn = false;
            foreach (var m_form in this.MdiChildren)
            {
                if (m_form.Name.Equals(frmName) && (m_form.Tag ?? "").ToString() == tagName)
                {
                    m_form.Focus();
                    m_fReturn = true;
                    break;
                }//end if

            }//end foreach
            return m_fReturn;
        }//end function

        private void MDIChildrenCleanup(object sender, EventArgs e)
        {
            ((Form)sender).Dispose();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WrGlobal.SessionNo != "")
            {
                code.CloseSession();
            }
        }

        private void btnChangePw_Click(object sender, EventArgs e)
        {
            var frm = Activator.CreateInstance(Type.GetType(string.Format("{0}.View.Systems.frmPasswordChange", WrGlobal.ProJectName)), WrGlobal.LoginID) as Form;
            doOpenForm(frm);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("사용자({0}) : 로그아웃 하겠습니까?", WrGlobal.LoginID), "삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                Application.Restart();
            }//end if

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Skin currentSkin = FormSkins.GetSkin(this.LookAndFeel);
            SkinElement element = currentSkin[FormSkins.SkinFormCaption];
            element.ContentMargins.Top = 6;
            element.ContentMargins.Bottom = 6;

            //element.Color.SolidImageCenterColor = Color.FromArgb(255, 33, 80, 126);
            this.LookAndFeel.UpdateStyleSettings();
        }

        protected override DevExpress.Skins.XtraForm.FormPainter CreateFormBorderPainter()
        {
            return new MyFormPainter(this, LookAndFeel, Resources.eng_icon, new Size(100, 20));
        }

        public class MyFormPainter : FormPainter
        {
            private readonly Icon _icon;
            private readonly Size _size;

            public MyFormPainter(Control owner, ISkinProvider provider) : base(owner, provider) { }

            public MyFormPainter(Control owner, ISkinProvider provider, Icon icon, Size size) : base(owner, provider)
            {
                _icon = icon;
                _size = size;
            }

            protected override Size GetIconSize() { return _size; }
            protected override Icon GetIcon() { return _icon; }

            protected override void DrawText(DevExpress.Utils.Drawing.GraphicsCache cache)
            {
                string text = Text;
                if (text == null || text.Length == 0 || this.TextBounds.IsEmpty) return;
                AppearanceObject appearance = new AppearanceObject(GetDefaultAppearance());
                appearance.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                appearance.TextOptions.Trimming = Trimming.EllipsisCharacter;
                appearance.ForeColor = Color.FromArgb(0, 100, 180);
                Rectangle r = RectangleHelper.GetCenterBounds(TextBounds, new Size(TextBounds.Width, appearance.CalcDefaultTextSize(cache.Graphics).Height));
                DrawTextShadow(cache, appearance, r);
                cache.DrawString(text, appearance.Font, appearance.GetForeBrush(cache), r, appearance.GetStringFormat());
            }
        }
    }
}