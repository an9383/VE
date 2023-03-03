using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VTMES3_RE.Common;
using VTMES3_RE.View.Reports.Tools;
using DevExpress.XtraReports.Extensions;

namespace VTMES3_RE
{
    internal static class Program
    {
        public static ReportStorageExtension reportStorage;
        public static ReportStorageExtension ReportStorage
        {
            get
            {
                return reportStorage;
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.DashboardCommon.Localization.DashboardLocalizer.Active = new clsDashboardCommonLocalizer();
            DevExpress.DashboardWin.Localization.DashboardWinLocalizer.Active = new clsDashboardWinLocalizer();

            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            bool isFontInstall = false;
            foreach (FontFamily fontFamily in installedFontCollection.Families)
            {
                if (fontFamily.Name.Equals("Pretendard SemiBold"))
                {
                    isFontInstall = true;
                }
            }

            if (!isFontInstall)
            {
                Shell32.Shell shell = new Shell32.Shell();
                Shell32.Folder fontFolder = shell.NameSpace(0x14);
                fontFolder.CopyHere(Application.StartupPath + @"\Fonts\Pretendard-SemiBold.ttf");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WrGlobal.CorpID = "VE";

            WrGlobal.Camstar_SQL_SERVER = "VE-MESDB-SVR01,1435";
            WrGlobal.Camstar_SQL_Database = "VECAMDB";
            WrGlobal.Camstar_SQL_Id = "sa";
            WrGlobal.Camstar_SQL_Password = "Dentalimageno.1";

            WrGlobal.Camstar_Host = "VE-MESAPP-SVR01​";
            WrGlobal.Camstar_Port = 443;
            WrGlobal.Camstar_UserName = "VE_MES3";
            WrGlobal.Camstar_Password = "Dentalimageno.1";
            
            reportStorage = new DataSetReportStorage();
            ReportStorageExtension.RegisterExtensionGlobal(reportStorage);

            if (new frmLogin().ShowDialog() == DialogResult.OK)
            {
                Application.Run(new frmMain());
            }

            //Application.Run(new View.DeliveryIssue.DeliveryIssue());
        }
    }
}
