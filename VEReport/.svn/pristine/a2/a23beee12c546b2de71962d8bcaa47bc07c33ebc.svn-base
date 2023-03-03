<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkOrderTA.aspx.cs" Inherits="View_Dashboard_WorkOrder" %>

<%@ Register assembly="DevExpress.Dashboard.v22.1.Web.WebForms, Version=22.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.DashboardWeb" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <link href="../assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="../framestyle.css" rel="stylesheet" type="text/css" />

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

        .dx-data-row {  
            user-select:text;  
        }  
        .dx-row-lines {  
            user-select:text!important;  
        }

    </style>
    <script type="text/javascript">

        var BO_Report = '<svg id="customReportIcon1" height="24px" width="24px" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" '
            + 'viewBox = "0 0 32 32" style = "enable-background:new 0 0 32 32;" xml: space="preserve" >'
            + '<style type="text/css"> '
            + '.Blue{fill:#1177D7;} '
            + '.Yellow{fill:#FFB115;} '
            + '.Red{fill:#D11C1C;} '
            + '.Green{fill:#039C23;} '
            + '.Black{fill:#727272;} '
            + '.White{fill:#FFFFFF;} '
            + '.st0{opacity:0.5;} '
            + '.st1{opacity:0.75;} '
            + '.st2{display:none;} '
            + '.st3{display:inline;fill:#FFB115;} '
            + '.st4{display:inline;} '
            + '.st5{display:inline;opacity:0.75;} '
            + '.st6{display:inline;opacity:0.5;} '
            + '.st7{display:inline;fill:#039C23;} '
            + '.st8{display:inline;fill:#D11C1C;} '
            + '.st9{display:inline;fill:#1177D7;} '
            + '.st10{display:inline;fill:#FFFFFF;} '
            + '</style> '
            + '<g id="Report"> '
            + '<path class="Yellow" d="M27,4h-3v22H8V4H5C4.4,4,4,4.4,4,5v24c0,0.6,0.4,1,1,1h22c0.6,0,1-0.4,1-1V5C28,4.4,27.6,4,27,4z"/> '
            + '<path class="Black" d="M20,4V3c0-0.6-0.4-1-1-1h-6c-0.6,0-1,0.4-1,1v1h-2v4h3h6h3V4H20z"/> '
            + '<g class="st1"> '
            + '<path class="Black" d="M10,20h12v2H10V20z M10,12h12v2H10V12z M10,16h12v2H10V16z"/> '
            + '</g> '
            + '</g>'
            + '</svg >';

        var BO_SelectAll = '<svg id="customReportIcon2" height="24px" width="24px" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" '
            + 'viewBox="0 0 32 32" style="enable-background:new 0 0 32 32;" xml:space="preserve"> '
            + '<style type="text/css"> '
            + '.Yellow{fill:#FFB115;} '
            + '.Red{fill:#D11C1C;} '
            + '.Blue{fill:#1177D7;} '
            + '.Green{fill:#039C23;} '
            + '.Black{fill:#727272;} '
            + '.White{fill:#FFFFFF;} '
            + '.st0{opacity:0.75;} '
            + '</style> '
            + '<g id="SelectAll"> '
            + '<path class="Black" d="M4,2h4v2H4v4H2V4V2H4z M10,4h4V2h-4V4z M16,4h4V2h-4V4z M26,2h-4v2h4v4h2V2H26z M4,10H2v4h2V10z M4,16H2v4h2 '
            + 'V16z M4,22H2v6h2h4v-2H4V22z M10,28h4v-2h-4V28z M16,28h4v-2h-4V28z M26,26h-4v2h6v-1v-1v-4h-2V26z M26,20h2v-4h-2V20z M26,14h2v-4 '
            + 'h-2V14z"/> '
            + '<rect x="6" y="6" class="Blue" width="18" height="18"/> '
            + '</g> '
            + '</svg>';

        var BO_Label = '<svg id="customReportIcon3" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" '
            + 'y = "0px" viewBox = "0 0 32 32" style = "enable-background:new 0 0 32 32;" xml: space = "preserve" > '
            + '<style type="text/css"> '
            + '.Black{fill:#727272;} '
            + '</style> '
            + '<path class="Black" d="M10,2h12v8h2V0H8v10h2V2z M28,8h-2v3c0,0.6-0.4,1-1,1H7c-0.6,0-1-0.4-1-1V8H4c-1.1,0-2,0.9-2,2v12 '
            + 'c0,1.1,0.9,2,2,2h4v6h16v-6h4c1.1,0,2-0.9,2-2V10C30,8.9,29.1,8,28,8z M22,22v2v4H10v-4v-2v-4h12V22z M20,24h-8v2h8V24z M20,20h-8v2 '
            + 'h8V20z"/> '
            + '</svg >';

        var BO_LabelAll = '<svg version="1.1" id="customReportIcon4" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" '
            + 'viewBox = "0 0 32 32" style = "enable-background:new 0 0 32 32;" xml: space = "preserve" > '
            + '<style type="text/css"> '
            + '.Yellow{fill:#FFB115;} '
            + '.Red{fill:#D11C1C;} '
            + '.Blue{fill:#1177D7;} '
            + '.Green{fill:#039C23;} '
            + '.Black{fill:#727272;} '
            + '.White{fill:#FFFFFF;} '
            + '.st0{opacity:0.75;} '
            + '</style> '
            + '<g id="SinglePageView"> '
            + '<path class="Black" d="M27,0H3C2.4,0,2,0.4,2,1v28c0,0.6,0.4,1,1,1h24c0.6,0,1-0.4,1-1V1C28,0.4,27.6,0,27,0z M26,28H4V2h22V28z"/> '
            + '<path class="Blue" d="M22,8H8V6h14V8z M22,10H8v2h14V10z M22,14H8v2h14V14z M22,18H8v2h14V18z M22,22H8v2h14V22z"/> '
            + '</g> '
            + '</svg > ';

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
            sheet.innerHTML += ".dx-pivotgrid .dx-pivotgrid-horizontal-headers td { color: white; background-color: steelblue; }";
            document.head.appendChild(sheet); 

            DevExpress.Dashboard.ResourceManager.registerIcon(BO_Report);
            DevExpress.Dashboard.ResourceManager.registerIcon(BO_SelectAll);
            DevExpress.Dashboard.ResourceManager.registerIcon(BO_Label);
            DevExpress.Dashboard.ResourceManager.registerIcon(BO_LabelAll);
        }
        //타이틀바에 버튼 추가
        function onDashboardTitleToolbarUpdated(sender, args) {
            var items = args.Options.actionItems;
            addItem(items, "customReportIcon1", OpenPop_SelReport, "성적서 보기");
            addItem(items, "customReportIcon2", OpenPop_AllReport, "전체 성적서 보기");
            addItem(items, "customReportIcon3", OpenPop_SelLabel, "라벨 보기");
            addItem(items, "customReportIcon4", OpenPop_AllLabel, "전체 라벨 보기");
        }

        var itemsRequireClick;
        //아이템바에 버튼 추가
        function onItemCaptionToolbarUpdated(sender, args) {
            //var items = args.Options.actionItems;
            //addItem(items, "customReportIcon", OpenPop_PQC, "성적서 보기");
            if (itemsRequireClick[args.ItemName]) {
                var item = args.Options.actionItems.filter(function (item) { return item.name === "multiselection" })[0];
                if (!!item) {
                    item.checked = true;
                    item.click();
                }
                itemsRequireClick[args.ItemName] = false;
            }
        }


        var grid;
        var keyFieldName = "ContainerId";
        var keyDataField = "";

        function onItemWidgetCreated(sender, args) {
            if (args.ItemName == "gridDashboardItem1") {
                grid = args.GetWidget();
                grid.pageSize(1000);

                for (var i = 0; i < grid.getVisibleColumns().length; i++) {
                    if (grid.getVisibleColumns()[i].caption == keyFieldName) {
                        keyDataField = grid.getVisibleColumns()[i].dataField;
                        break;
                    }
                }

                grid.columnOption("ContainerId", {
                    visible: false
                });
            }

            if (!itemsRequireClick[args.ItemName]) {
                itemsRequireClick[args.ItemName] = true;
                sender.UpdateItemCaptionToolbar(args.ItemName);
            }
        }

        function onItemWidgetUpdated(sender, args) {
            if (args.ItemName == "gridDashboardItem1") {
                var grid = args.GetWidget();
                grid.columnOption("ContainerId", {
                    visible: false
                });
            }
        }

        function onDashboardInitialized(sender, args) {
            if (cmd != 'main' && webDashboard.GetParameters().GetParameterList().length > 0)
                webDashboard.ShowParametersDialog();

            itemsRequireClick = sender.GetDashboardControl().dashboard().items()
                .map(function (value) {
                    return { item: value.componentName(), isClickRequire: false }
                })
                .reduce(function (map, obj) {
                    map[obj.item] = obj.isClickRequire;
                    return map;
                }, {});
        }

        function addItem(items, iconID, eventHandler, hintText) {
            items.unshift({
                type: "button",
                icon: iconID,
                hint: hintText,
                click: function () { eventHandler.call(); }
            });
        }

        function OpenPop_AllReport() {
            webDashboard.ClearMasterFilter("gridDashboardItem1");
            grid.selectAll();
            setTimeout(OpenPop_SelReport, 1000);
        }

        function OpenPop_SelReport() {
            var urlParam = "";
            var strKeyValue = "";
            var selRows = grid.getSelectedRowsData();

            if (selRows.length == 0) {
                alertify.alert("선택 항목이 없습니다.");
                return;
            }

            if (keyDataField == "") {
                alertify.alert("Key 컬럼을 찾을수 없습니다.");
                return;
            }

            selRows.forEach(function (item, index, arr) {
                strKeyValue += "," + item[keyDataField];
            })

            if (strKeyValue != "") urlParam = "&pConId=" + strKeyValue.substring(1, strKeyValue.length);

            var url = "../ReportView.aspx?reportid=2" + urlParam;
            var win = window.open(url, "_blank", "toolbar=no,scrollbars=yes,resizable=yes,top=100,left=200,width=900,height=700");
        }

        function OpenPop_AllLabel() {
            webDashboard.ClearMasterFilter("gridDashboardItem1");
            grid.selectAll();
            setTimeout(OpenPop_SelLabel, 1000);
        }

        function OpenPop_SelLabel() {
            var urlParam = "";
            var strKeyValue = "";
            var selRows = grid.getSelectedRowsData();

            if (selRows.length == 0) {
                alertify.alert("선택 항목이 없습니다.");
                return;
            }

            if (keyDataField == "") {
                alertify.alert("Key 컬럼을 찾을수 없습니다.");
                return;
            }

            selRows.forEach(function (item, index, arr) {
                strKeyValue += "," + item[keyDataField];
            })

            if (strKeyValue != "") urlParam = "&pConId=" + strKeyValue.substring(1, strKeyValue.length);

            var url = "../ReportView.aspx?reportid=3" + urlParam;
            var win = window.open(url, "_blank", "toolbar=no,scrollbars=yes,resizable=yes,top=100,left=200,width=900,height=700");
        }
    </script>
</head>
<body style="padding-top: 4px;">
    <form id="form1" runat="server">
        <div style="position: absolute; left: 0px; right: 0px; top: 4px; bottom: 0px;">
            <dx:ASPxDashboard ID="WebDashboard" runat="server" ClientInstanceName="webDashboard" WorkingMode="ViewerOnly" OnConfigureDataConnection="WebDashboard_ConfigureDataConnection" Height="100%" ClientIDMode="Static" CanCreateNewJsonDataSource="False" DashboardId="" DashboardState="" DisableHttpHandlerValidation="False" Width="100%">
                <ClientSideEvents DashboardInitialized="onDashboardInitialized" 
                    BeforeRender="onBeforeRender" 
                    ItemWidgetCreated="onItemWidgetCreated" 
                    ItemCaptionToolbarUpdated="onItemCaptionToolbarUpdated"
                    DashboardTitleToolbarUpdated="onDashboardTitleToolbarUpdated" ItemWidgetUpdated="onItemWidgetUpdated">
                </ClientSideEvents>

                <BackendOptions Uri=""></BackendOptions>

                <DataRequestOptions ItemDataRequestMode="Auto" ItemDataLoadingMode="Always"></DataRequestOptions>
            </dx:ASPxDashboard>
        </div>
    </form>

    <!-- jQuery  -->
    <script src="../assets/js/bootstrap.bundle.min.js"></script>
    <script src="../assets/js/jquery.slimscroll.js"></script>
    <script src="../assets/js/waves.min.js"></script>

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

        function SetParametersDialog(msg) {
            if (isLoad) {
                webDashboard.HideParametersDialog();
            }
        }

    </script>

    <!-- Alertify js -->
    <script src="../../plugins/alertify/js/alertify.js"></script>
</body>
</html>
