<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="View_Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>바텍ENG-MES DashBoard</title>
    <link rel="shortcut icon" href="../Images/vt_icon.ico" />
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/metismenu.min.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/icons.css" rel="stylesheet" type="text/css" />
    <link href="assets/css/style.css" rel="stylesheet" type="text/css" />

    <style>
        html, body {
            height: 100%;
        }

        body {
            margin: 0;
            padding: 0;
        }

        [contenteditable] {
            border: solid 1px lightgreen;
            padding: 5px;
            border-radius: 3px;
        }

        .nav-item a {
            color: #007bff !important;
            text-decoration: none;
            background-color: transparent;
        }

        .nav-link.active {
            color: #495057 !important;
            background-color: #fff;
            border-color: #dee2e6 #dee2e6 #fff;
        }
    </style>

    <script type="text/javascript">


        history.pushState(null, document.title, location.href);  // push
        window.addEventListener('popstate', function (event) {    //  뒤로가기 이벤트 등록
            // 특정 페이지로 가고싶다면 location.href = '';
            history.pushState(null, document.title, location.href);  // 다시 push함으로 뒤로가기 Block
        });

        function NotReload() {
            if ((event.ctrlKey == true && (event.keyCode == 78 || event.keyCode == 82)) || (event.keyCode == 116)) {
                event.keyCode = 0;
                event.cancelBubble = true;
                event.returnValue = false;
            }
        }
        document.onkeydown = NotReload;

        function MoveToLocationByMenuClick(menuId, menuName, formName) {
            if ($('#tab' + menuId).length == 0) {
                if ($('#tab-list')[0].children.length > 9) {
                    alertify.alert("탭은 8개까지 생성할 수 있습니다.");
                    return;
                }

                $('#tab-list').append($('<li class="nav-item"><a id="tablink' + menuId + '" href="#tab' + menuId + '" role="tab" data-toggle="tab" class="nav-link"> ' + menuName + ' <img class="close ml-1" src = "../Images/Close_16x16.png" alt="Remove this page"></a></li>'));
                var frameStr = '<iframe id="ifra' + menuId + '" name="ifra' + menuId + '" class="demo-frame loading" style="z-index: 101; width: 100%; height: calc(100vh - 90px);" marginwidth="0" '
                    + 'marginheight="0" src="' + formName + (formName.indexOf("?") > -1 ? '&' : '?') + 'id=' + menuId + '" frameborder="0" scrolling="yes"></iframe>';

                $('#tab-content').append($('<div class="tab-pane fade" id="tab' + menuId + '">' + frameStr + '</div>'));
            }
            $('#tablink' + menuId).tab('show');
        }

        var openPopup = function (_pUrl) {
            var _pLeft = (screen.width - 750) / 2;
            var _pTop = 0;
            var _pID = "popup_" + new Date().getTime();
            var _pObj = window.open(_pUrl, _pID, 'menubar=0,resizable=1,scrollbars=1,status=no,titlebar=0,toolbar=no,width=750,height=600,left=' + _pLeft + ',top=' + _pTop);
            if (_pObj.focus) {
                _pObj.focus();
            }
        };

        function bodyUnload() {
            $.ajax({
                type: "POST",
                url: "LogOut.aspx/LogOut",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });
        }

        function bodyLoad() {
            $.ajax({
                type: "POST",
                url: "LogOut.aspx/SessionCheck",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            })
                .done(function (result) {
                    if (result.d == "") {
                        alert("로그인 정보가 삭제되어 로그인 페이지로 이동합니다.");
                        window.location.href = 'Login.aspx';
                    }
                });
        }

    </script>
</head>
<body onload="bodyLoad();" onunload="bodyUnload();">
    <form id="form1" runat="server" style="height: 100%">
        <!-- Begin page -->
        <div id="wrapper">

            <!-- Top Bar Start -->
            <div class="topbar">

                <!-- LOGO -->
                <div class="topbar-left">
                    <a href="javascript:void();" class="logo">
                        <span class="logo-light">MES DashBoard
                        </span>
                        <span class="logo-sm">
                            <img src="../Images/vatech_logo.png" alt="" height="30" />
                        </span>
                    </a>
                </div>

                <nav class="navbar-custom">
                    <ul class="navbar-right list-inline float-right mb-0">

                        <!-- full screen -->
                        <li class="dropdown notification-list list-inline-item d-none d-md-inline-block">
                            <a class="nav-link waves-effect" href="#" id="btn-fullscreen">
                                <i class="mdi mdi-arrow-expand-all noti-icon"></i>
                            </a>
                        </li>

                        <!-- notification -->
                        <li class="dropdown notification-list list-inline-item">
                            <div class="dropdown notification-list nav-pro-img">
                                <a class="dropdown-toggle nav-link arrow-none nav-user" data-toggle="dropdown" href="#" role="button" aria-haspopup="false" aria-expanded="false">
                                    <img id="imgUser" runat="server" src="assets/images/users/user-4.jpg" alt="user" class="rounded-circle" />
                                    <%=Session["EmpName"].ToString()%> <span class="mdi mdi-chevron-down"></span>
                                </a>
                                <div class="dropdown-menu dropdown-menu-right profile-dropdown ">
                                    <!-- item-->
                                    <a class="dropdown-item text-danger" href="Login.aspx"><i class="mdi mdi-power text-danger"></i>Logout</a>
                                </div>
                            </div>
                        </li>

                    </ul>

                    <ul class="list-inline menu-left mb-0">
                        <li class="float-left">
                            <button class="button-menu-mobile open-left waves-effect">
                                <i class="mdi mdi-menu"></i>
                            </button>
                        </li>
                    </ul>
                </nav>

            </div>
            <!-- Top Bar End -->

            <!-- ========== Left Sidebar Start ========== -->
            <div class="left side-menu">
                <div class="slimscroll-menu" id="remove-scroll">

                    <!--- Sidemenu -->
                    <div id="sidebar-menu">
                        <!-- Left Menu Start -->
                        <ul class="metismenu" id="side-menu">
                            <asp:Literal ID="ltlSideMenu" Text="" runat="server" />
                        </ul>

                    </div>
                    <!-- Sidebar -->
                    <div class="clearfix"></div>

                </div>
                <!-- Sidebar -left -->

            </div>
            <!-- Left Sidebar End -->

            <!-- ============================================================== -->
            <!-- Start right Content here -->
            <!-- ============================================================== -->
            <div class="content-page" style="height: 100%">
                <div class="content" style="height: 100%;">
                    <!-- Nav tabs -->
                    <ul id="tab-list" class="nav nav-tabs" role="tablist">
                        <li class="nav-item active"><a id="tablinkMain" href="#tabMain" role="tab" data-toggle="tab" class="nav-link">Main</a></li>
                    </ul>

                    <!-- Tab panes -->
                    <div id="tab-content" class="tab-content">
                        <div class="tab-pane fade in active show" id="tabMain">
                            <iframe id="ifraMain" name="ifraMain" class="demo-frame loading" style="z-index: 101; width: 100%; height: calc(100vh - 90px);" marginwidth="0"
                                marginheight="0" src="DashboardView.aspx?cmd=main&id=10000" frameborder="0" scrolling="no"></iframe>
                        </div>
                    </div>
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
            $('#tab-list').on('click', '.close', function () {
                var tabID = $(this).parents('a').attr('href');
                var preTab;

                $("#tab-list").children('li').children('a').each(function (idx, val) {
                    if (tabID == $(this).attr('href')) {
                        return false;
                    }
                    preTab = $(this);
                });

                $(this).parents('li').remove();
                $(tabID).remove();

                preTab.tab('show');
            });             
        });

        $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
            if ($($(e.target).attr('href')).children()[0].contentWindow.SetParametersDialog == undefined) return;
            $($(e.target).attr('href')).children()[0].contentWindow.SetParametersDialog($(e.target).attr('href'));
        })

    </script>

    <!-- Alertify js -->
    <script src="../plugins/alertify/js/alertify.js"></script>
    </form>
</body>
</html>
