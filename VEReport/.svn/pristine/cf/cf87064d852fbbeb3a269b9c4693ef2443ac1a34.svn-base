<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecoverPassword.aspx.cs" Inherits="View_RecoverPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>비밀번호 변경</title>
    <link rel="shortcut icon" href="../Images/rayence_logoicon.ico" />
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/metismenu.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .input-placeholder {
            color:#6c757d;
            opacity:1
        }
    </style>

    <script type="text/javascript">

        function OnChangeClick(s, e) {
            if (ASPxClientEdit.ValidateGroup('Change')) {
                if (chkPW(txtNewPassword.GetText())) {
                    cbPnlChange.PerformCallback();
                }
            };
        }

        function CbPnlChange_EndCallback(s, e) {
            if (s.cpMsg) {
                alert(s.cpMsg);
                delete s.cpMsg;
            };

            if (s.cpChange) {
                delete s.cpChange;
                window.location = "Login.aspx";
            };
        }

        function chkPW(pw) {
            return true;
            //var num = pw.search(/[0-9]/g);
            //var eng = pw.search(/[a-z]/ig);
            //var spe = pw.search(/[`~!@@#$%^&*|₩₩₩'₩";:₩/?]/gi);

            //if (pw.length < 8 || pw.length > 20) {
            //    alert("8자리 ~ 20자리 이내로 입력해주세요.");
            //    return false;
            //} else if (pw.search(/\s/) != -1) {
            //    alert("비밀번호는 공백 없이 입력해주세요.");
            //    return false;
            //} else if (num < 0 || eng < 0 || spe < 0) {
            //    alert("영문,숫자, 특수문자를 혼합하여 입력해주세요.");
            //    return false;
            //} else {
            //    return true;
            //}
        }

        function CbPnlChange_Init(s, e) {
            cboUserId.SetValue(getCookie("ve_loginid"));
        }

        function getCookie(cookie_name) {
            var x, y;
            var val = document.cookie.split(';');

            for (var i = 0; i < val.length; i++) {
                x = val[i].substr(0, val[i].indexOf('='));
                y = val[i].substr(val[i].indexOf('=') + 1);
                x = x.replace(/^\s+|\s+$/g, ''); // 앞과 뒤의 공백 제거하기
                if (x == cookie_name) {
                    return unescape(y); // unescape로 디코딩 후 값 리턴
                }
            }
        }

    </script>
</head>
<body>
    <!-- Begin page -->
    <div class="accountbg"></div>
    <div class="home-btn d-none d-sm-block">
        <a href="../index.html" class="text-white"><i class="fas fa-home h2"></i></a>
    </div>
    <div class="wrapper-page">
        <div class="card card-pages shadow-none">

            <div class="card-body">
                <div class="text-center m-t-0 m-b-15">
                    <a href="index.html" class="logo logo-admin">
                        <img src="../Images/hy_logo.png" alt="" height="64" /></a>
                </div>
                <h5 class="font-18 text-center">비밀번호 변경</h5>

                <form id="form2" runat="server">
                    <dx:BootstrapCallbackPanel ID="CbPnlChange" runat="server" ClientInstanceName="cbPnlChange" OnCallback="CbPnlChange_Callback">
                        <ContentCollection>
                            <dx:ContentControl runat="server">
                                <div class="form-group">
                                    <div class="col-12">
                                        <label>아이디</label>
                                        <dx:BootstrapComboBox ID="cboUserId" runat="server" ClientIDMode="Static" ClientInstanceName="cboUserId" NullText="아이디" DataSourceID="employeeSqlDataSource" ValueField="ID" TextField="FullName" EnableMultiColumn="True">
                                            <CssClasses NullText="input-placeholder" />
                                            <Fields>
                                                <dx:BootstrapListBoxField FieldName="ID">
                                                </dx:BootstrapListBoxField>
                                                <dx:BootstrapListBoxField FieldName="FullName">
                                                </dx:BootstrapListBoxField>
                                            </Fields>
                                            <ValidationSettings ValidationGroup="Change" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="아이디를 입력하세요." />
                                            </ValidationSettings>
                                        </dx:BootstrapComboBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-12">
                                        <label>현재 비밀번호</label>
                                        <dx:BootstrapTextBox runat="server" Password="True" ID="txtPassword" ClientIDMode="Static" ClientInstanceName="txtPassword" NullText="현재 비밀번호">
                                            <CssClasses NullText="input-placeholder" />
                                            <ValidationSettings ValidationGroup="Change" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText=" 현재 비밀번호를 입력하세요." />
                                            </ValidationSettings>
                                        </dx:BootstrapTextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-12">
                                        <label>비밀번호</label>
                                        <dx:BootstrapTextBox runat="server" Password="True" ID="txtNewPassword" ClientIDMode="Static" ClientInstanceName="txtNewPassword" NullText="비밀번호">
                                            <CssClasses NullText="input-placeholder" />
                                            <ValidationSettings ValidationGroup="Change" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="변경할 비밀번호를 입력하세요." />
                                            </ValidationSettings>
                                        </dx:BootstrapTextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-12">
                                        <label>비밀번호 확인</label>
                                        <dx:BootstrapTextBox runat="server" Password="True" ID="txtConfirmPassword" ClientIDMode="Static" ClientInstanceName="txtConfirmPassword" NullText="비밀번호 확인">
                                            <CssClasses NullText="input-placeholder" />
                                            <ValidationSettings ValidationGroup="Change" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="비밀번호 확인을 입력하세요." />
                                            </ValidationSettings>
                                        </dx:BootstrapTextBox>
                                    </div>
                                </div>
                            </dx:ContentControl>
                        </ContentCollection>
                        <ClientSideEvents EndCallback="CbPnlChange_EndCallback" Init="CbPnlChange_Init" />
                    </dx:BootstrapCallbackPanel>

                    <div class="form-group text-center m-t-20">     
                        <div class="col-12">
                            <dx:BootstrapButton runat="server" AutoPostBack="False" Text="적 용" ID="btnChange" ClientInstanceName="btnChange">
                                <CssClasses Control="btn btn-primary btn-block btn-lg waves-effect waves-light" />
                                <SettingsBootstrap RenderOption="Primary" />
                                <ClientSideEvents Click="OnChangeClick" />
                            </dx:BootstrapButton>
                        </div>
                    </div>
                    <asp:SqlDataSource ID="employeeSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ReportdbConnectionString %>"
                        SelectCommand="Select cu.EmployeeName ID, emp.FullName From CodeUser cu Left Join VECAMDB.CAMDBsh.Employee emp on cu.EmployeeName = emp.EmployeeName Order By emp.FullName">
                        <SelectParameters>
                            <asp:SessionParameter Name="pCorpId" SessionField="CorpID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </form>
            </div>

        </div>
    </div>
    <!-- END wrapper -->

    <!-- jQuery  -->
    <script src="assets/js/jquery.min.js"></script>
    <script src="assets/js/bootstrap.bundle.min.js"></script>
    <script src="assets/js/metismenu.min.js"></script>
    <script src="assets/js/jquery.slimscroll.js"></script>
    <script src="assets/js/waves.min.js"></script>

    <!-- App js -->
    <script src="assets/js/app.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

        });

        function ChkInput() {
            //if ($("#txtPassword").val() == "") {
            //    alert("Password를 입력하세요.");
            //    return false;
            //};
        }

    </script>
</body>
</html>
