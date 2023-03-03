﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTMES3_RE.Common
{
    public class WrGlobal
    {
        public static string CorpID = "VE";
        public static string LoginID = string.Empty;         // 로그인한 사용자 ID
        public static string LoginUserNM = string.Empty;     // 로그인한 사용자 이름

        public static string ProgramType = "2";       
        public static string ClientHostName = System.Net.Dns.GetHostName();    //접속 PC

        public static string ProJectName = "VTMES3_RE";
        public static string Language = "ko";

        public static string SessionNo = "";
        public static List<clsAuthor> AuthorList = new List<clsAuthor>();

        public static string DBServer = "VE-MESDB-SVR01,1435";
        public static string DBUserName = "sa";
        public static string DBUserPassword = "Dentalimageno.1";

        public static string OpeningMenuId = "";

        public static string Camstar_SQL_SERVER = "";
        public static string Camstar_SQL_Database = "";
        public static string Camstar_SQL_Id = "";
        public static string Camstar_SQL_Password = "";

        public static string Camstar_Host = "";
        public static int Camstar_Port = 443;
        public static string Camstar_UserName = "";
        public static string Camstar_Password = "";

        public static CamstarCommon Camster_Common = null;
    }
}
