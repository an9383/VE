<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TACheckSheet.aspx.cs" Inherits="View_Report_TACheckSheet" %>

<%@ Register assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="framestyle.css" rel="stylesheet" type="text/css" />

    <style>
        html, body {
            margin: 0;
            height: 100%;
            overflow: hidden;
        }
    </style>
</head>
<body style="padding-top: 4px;">
    <form id="form1" runat="server">
        <div style="position: absolute; left: 0px; right: 0px; top: 4px; bottom: 0px;">

            <dx:ASPxWebDocumentViewer ID="WebCodumentViewer" runat="server" ClientInstanceName="webCodumentViewer" Height="100%">
            </dx:ASPxWebDocumentViewer>

        </div>
    </form>

    <!-- jQuery  -->
    <script src="assets/js/bootstrap.bundle.min.js"></script>
    <script src="assets/js/jquery.slimscroll.js"></script>
    <script src="assets/js/waves.min.js"></script>

    <script type="text/javascript">
        var reportId = "<%=pstrReportId%>";
        var errMsg = "<%=pstrErrMsg%>";

        $(window).on('load', onLoad);

        function onLoad() {
            isLoad = true;
            if (errMsg != "") {
                alertify.alert(errMsg);
                errMsg = "";
            }
        }
    </script>

    <!-- Alertify js -->
    <script src="../plugins/alertify/js/alertify.js"></script>
</body>
</html>
