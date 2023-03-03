<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportManager.aspx.cs" Inherits="View_ReportManager" %>

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

    <script type="text/javascript">

        function onToolbarItemClick(s, e) {
            switch (e.item.name) {
                case "ReportUpdate":
                    if (reportGrid.GetFocusedRowIndex() >= 0) {
                        reportGrid.StartEditRow(reportGrid.GetFocusedRowIndex());
                    }

                    break;
                case "ReportDelete":
                    if (reportGrid.GetFocusedRowIndex() >= 0) {
                        if (confirm("선택된 레포트를 삭제하겠습니까?")) {
                            reportGrid.DeleteRow(reportGrid.GetFocusedRowIndex());
                        }
                    }
                    break;

            }
        }

        //function callResize() {
        //    var height = document.body.scrollHeight;
        //    parent.resizeTopIframe(height);
        //}

        function reportGrid_EndCallback(s, e) {
            callResize();

            if (s.cpMsg) {
                alertify.alert(s.cpMsg);
                delete s.cpMsg;
            }
        }

    </script>
</head>
<body style="padding-top: 4px;">
    <form id="form1" runat="server">
        <div style="position: absolute; left: 0px; right: 0px; top: 4px; bottom: 0px;">
            <!-- START ROW -->
            <div class="row">
                <div class="col-xl-12">
                    <div class="card m-b-10">
                        <div class="card-body">
                            <h4 class="mt-0 header-title mb-1">레포트 리스트</h4>
                            <dx:BootstrapGridView ID="ReportGrid" ClientInstanceName="reportGrid" runat="server" Width="100%" AutoGenerateColumns="False"
                                KeyFieldName="ReportId" DataSourceID="SqlDataSource2">
                                <SettingsPopup>
                                    <HeaderFilter MinHeight="140px"></HeaderFilter>
                                </SettingsPopup>
                                <SettingsText CommandCancel="취소" CommandDelete="삭제" CommandEdit="수정" CommandNew="신규" CommandSelect="선택" CommandUpdate="저장" ConfirmDelete="삭제하겠습니까?" EmptyDataRow="자료가 없습니다." PopupEditFormCaption="입력 팝업" />
                                <SettingsEditing EditFormColumnSpan="12" Mode="PopupEditForm"></SettingsEditing>
                                <SettingsDataSecurity AllowEdit="true" AllowDelete="true" />
                                <Toolbars>
                                    <dx:BootstrapGridViewToolbar>
                                        <Items>
                                            <dx:BootstrapGridViewToolbarItem Text="수정" Name="ReportUpdate" />
                                            <dx:BootstrapGridViewToolbarItem Text="삭제" Name="ReportDelete" />
                                        </Items>
                                    </dx:BootstrapGridViewToolbar>
                                </Toolbars>
                                <Columns>
                                    <dx:BootstrapGridViewTextColumn FieldName="ReportId" Caption="레포트ID" VisibleIndex="0">
                                        <PropertiesTextEdit>
                                            <ValidationSettings>
                                                <RequiredField ErrorText="필수 입력 항목입니다." IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <CssClasses HeaderCell="text-center" />
                                        <SettingsEditForm ColumnSpan="12" Visible="False" />
                                    </dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="ReportName" Caption="레포트명" VisibleIndex="1" Width="120px">
                                        <CssClasses HeaderCell="text-center" />
                                        <SettingsEditForm ColumnSpan="12" />
                                    </dx:BootstrapGridViewTextColumn>
                                    <dx:BootstrapGridViewTextColumn FieldName="Remark" Caption="메모" VisibleIndex="2" Width="300px">
                                        <SettingsEditForm ColumnSpan="12" />
                                    </dx:BootstrapGridViewTextColumn>
                                </Columns>
                                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" />
                                <CssClasses Table="table table-striped table-bordered dt-responsive nowrap dataTable no-footer dtr-inline" />
                                <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit"></SettingsAdaptivity>
                                <Settings HorizontalScrollBarMode="Auto" />
                                <SettingsBehavior AllowFocusedRow="true" AllowClientEventsOnLoad="false" ConfirmDelete="True" />
                                <SettingsPager AlwaysShowPager="true" ShowEmptyDataRows="true" PageSize="10">
                                    <Summary EmptyText="" />
                                </SettingsPager>
                                <ClientSideEvents EndCallback="reportGrid_EndCallback" ToolbarItemClick="onToolbarItemClick" />
                            </dx:BootstrapGridView>
                        </div>
                    </div>
                </div>

            </div>
            <!-- END ROW -->
        </div>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ReportdbConnectionString %>"
            SelectCommand="Select * From ReprotItem Where CorpId = @CorpId Order by ReportId" SelectCommandType="Text"
            UpdateCommand="Update ReprotItem Set ReportName = @ReportName, RevisionName = @RevisionName, Bigo = @Bigo, ModId = @EmpNo, ModIP = @EmpIp, ModDt = GETDATE() Where CorpId = @CorpId and ReportId = @ReportId"
            DeleteCommand="DELETE FROM ReprotItem WHERE CorpId = @CorpId and ReportId = @ReportId"> 
            <SelectParameters>
                <asp:SessionParameter Name="CorpId" SessionField="CorpId" />
            </SelectParameters>
            <DeleteParameters>
                <asp:SessionParameter Name="CorpId" SessionField="CorpId" />
                <asp:Parameter Name="ReportId" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:SessionParameter Name="CorpId" SessionField="CorpId" />
                <asp:SessionParameter Name="EmpNo" SessionField="EmpNo" />
                <asp:SessionParameter Name="EmpIp" SessionField="EmpIp" />
                <asp:Parameter Name="ReportId" />
                <asp:Parameter Name="ReportName" />
                <asp:Parameter Name="RevisionName" />
                <asp:Parameter Name="Bigo" /> 
            </UpdateParameters>
        </asp:SqlDataSource>
    </form>

    <!-- jQuery  -->
    <script src="assets/js/bootstrap.bundle.min.js"></script>
    <script src="assets/js/jquery.slimscroll.js"></script>
    <script src="assets/js/waves.min.js"></script>

    <!-- Alertify js -->
    <script src="../plugins/alertify/js/alertify.js"></script>
</body>
</html>
