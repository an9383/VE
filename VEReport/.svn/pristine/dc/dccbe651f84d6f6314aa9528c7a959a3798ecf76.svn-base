<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardView_BuiltIn.aspx.cs" Inherits="View_Dashboard_BuiltInBoard_DashboardView_BuiltIn" %>

<%@ Register assembly="DevExpress.Dashboard.v22.1.Web.WebForms, Version=22.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <link href="../../assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../assets/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="../../framestyle.css" rel="stylesheet" type="text/css" />

    <style>
        html, body {
            margin: 0;
            height: 100%;
            overflow: hidden;
        }

        .dx-dashboard-viewer .dx-datagrid-rowsview .dx-selection > td, .dx-dashboard-viewer .dx-datagrid-rowsview .dx-selection.dx-row:hover > td {
            background-color: lightblue !important;
            color: black !important;
        }

        .dx-dashboard-item-shield, .dx-overlay-wrapper.dx-overlay-shader {  
            display:none !important;  
        }  
        .dx-dashboard-item-loading-panel {  
            display: none !important;  
        }
        
        .dx-data-row {  
            user-select:text;  
        }  
        .dx-row-lines {  
            user-select:text!important;  
        }
        .dx-tab.dx-tab-selected .dx-toolbar-items-container {
            background-color: pink!important;
            color: pink!important
        }
    </style>
    <script type="text/javascript">
        var BO_Refresh = '<svg version="1.1" id="BO_Refresh" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" ' +
            'viewBox="0 0 32 32" style="enable-background:new 0 0 32 32;" xml:space="preserve"> ' +
            '<style type="text/css"> ' +
            '.Blue{fill:#1177D7;} ' +
            '.Yellow{fill:#FFB115;} ' +
            '.Black{fill:#727272;} ' +
            '.Green{fill:#039C23;} ' +
            '.Red{fill:#D11C1C;} ' +
            '.st0{opacity:0.75;} ' +
            '.st1{opacity:0.5;} ' +
            '</style> ' +
            '<g id="Refresh_1_"> ' +
            '<path class="Green" d="M24.5,7.5C22.3,5.3,19.3,4,16,4C10.1,4,5.1,8.3,4.2,14h4.1c0.9-3.4,4-6,7.7-6c2.2,0,4.2,0.9,5.6,2.4L18,14 ' +
            'h5.7h4.1H28V4L24.5,7.5z"/> ' +
            '<path class="Green" d="M16.2,24c-2.2,0-4.2-0.9-5.6-2.4l3.6-3.6H8.4H4.4H4.2v10l3.5-3.5c2.2,2.2,5.2,3.5,8.5,3.5 ' +
            'C22.1,28,27,23.7,28,18h-4.1C23,21.4,19.9,24,16.2,24z"/> ' +
            '</g> ' +
            '</svg> ';

        function NotReload() {
            if ((event.ctrlKey == true && (event.keyCode == 78 || event.keyCode == 82)) || (event.keyCode == 116)) {
                event.keyCode = 0;
                event.cancelBubble = true;
                event.returnValue = false;
            }
        }
        document.onkeydown = NotReload;

        function onBeforeRender(sender, args) {
            //DevExpress.Dashboard.ResourceManager.setLocalizationMessages(
            //    { 'DashboardStringId.ParametersFormCaption': '검색 조건' }
            //);
            var sheet = document.createElement('style')
            sheet.innerHTML = ".dx-dashboard-title-toolbar .dx-item-content.dx-toolbar-item-content { font-size: 22px; font-weight: bold; color: darkslateblue; } ";
            sheet.innerHTML += ".dx-dashboard-caption-toolbar .dx-toolbar-item { font-size: 16px; font-weight: bold; color: royalblue; }";
            sheet.innerHTML += ".dx-datagrid-headers { color: white; background-color: steelblue; }";
            document.head.appendChild(sheet);  

            DevExpress.Dashboard.ResourceManager.registerIcon(BO_Refresh);
        }

        function onDashboardTitleToolbarUpdated(sender, args) {
            var items = args.Options.actionItems;
            addItem(items, "BO_Refresh", Event_Refresh, "새로고침");
        }

        var itemsRequireClick;
        function onItemCaptionToolbarUpdated(sender, args) {
            if (itemsRequireClick[args.ItemName]) {
                var item = args.Options.actionItems.filter(function (item) { return item.name === "multiselection" })[0];
                if (!!item) {
                    item.checked = true;
                    item.click();
                }
                itemsRequireClick[args.ItemName] = false;
            }
        }

        function onItemWidgetCreated(sender, args) {
            if (!itemsRequireClick[args.ItemName]) {
                itemsRequireClick[args.ItemName] = true;
                sender.UpdateItemCaptionToolbar(args.ItemName);
            }
        }

        function onDashboardInitialized(sender, args) {
            itemsRequireClick = sender.GetDashboardControl().dashboard().items()
                .map(function (value) {
                    return { item: value.componentName(), isClickRequire: false }
                })
                .reduce(function (map, obj) {
                    map[obj.item] = obj.isClickRequire;
                    return map;
                }, {});

            setInterval(SetTabIndexChangeCycle, 30000);
        }

        var tabIdx = 0;

        function SetTabIndexChangeCycle() {
            tabIdx++;
            if (tabIdx > 5) tabIdx = 0; 
            //alert(tabIdx);
            webDashboard.SetSelectedTabPageIndex("tabContainerDashboardItem1", tabIdx);

            switch (tabIdx) {
                case 0:
                    webDashboard.Refresh(["gridDashboardItem1"])
                    break;
                case 1:
                    webDashboard.Refresh(["gridDashboardItem2"])
                    break;
                case 2:
                    webDashboard.Refresh(["gridDashboardItem3"])
                    break;
                case 3:
                    webDashboard.Refresh(["gridDashboardItem4"])
                    break;
                case 4:
                    webDashboard.Refresh(["gridDashboardItem5"])
                    break;
                case 5:
                    webDashboard.Refresh(["gridDashboardItem6"])
                    break;

            }
            
            //webDashboard.reloadData();
        }

        function addItem(items, iconID, eventHandler, hintText) {
            items.unshift({
                type: "button",
                icon: iconID,
                hint: hintText,
                click: function () { eventHandler.call(); }
            });
        }

        function Event_Refresh() {
            webDashboard.ReloadData();
        }

    </script>
</head>
<body style="padding-top: 4px;">
    <form id="form1" runat="server">
        <div style="position: absolute; left: 0px; right: 0px; top: 4px; bottom: 0px;">
            <dx:ASPxDashboard ID="WebDashboard" runat="server" ClientInstanceName="webDashboard" WorkingMode="ViewerOnly" OnConfigureDataConnection="WebDashboard_ConfigureDataConnection" Height="100%" ClientIDMode="Static" DashboardId="" DashboardState="" DisableHttpHandlerValidation="False" Width="100%">
                <PdfExportOptions FontInfo-GdiCharSet="1" FontInfo-Name="돋움" FontInfo-UseCustomFontInfo="True" />
                <ClientSideEvents DashboardInitialized="onDashboardInitialized"
                    BeforeRender="onBeforeRender"
                    ItemWidgetCreated="onItemWidgetCreated" 
                    ItemCaptionToolbarUpdated="onItemCaptionToolbarUpdated"
                    DashboardTitleToolbarUpdated="onDashboardTitleToolbarUpdated"
                    >
                </ClientSideEvents>

                <BackendOptions Uri=""></BackendOptions>

                <DataRequestOptions ItemDataRequestMode="Auto" ItemDataLoadingMode="Always"></DataRequestOptions>
            </dx:ASPxDashboard>
        </div>
    </form>

    <!-- jQuery  -->
    <script src="../../assets/js/bootstrap.bundle.min.js"></script>
    <script src="../../assets/js/jquery.slimscroll.js"></script>
    <script src="../../assets/js/waves.min.js"></script>

    <script type="text/javascript">

        var cmd = "<%=pstrCmd%>";
        var menuId = "<%=pstrMenuId%>";
        var errMsg = "<%=pstrErrMsg%>";
        var isLoad = false;

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
    <script src="../../../plugins/alertify/js/alertify.js"></script>
</body>
</html>
