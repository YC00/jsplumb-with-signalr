<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>Concept Map Admin Panel</title>

    <!-- Core CSS - Include with every page -->
    <link href="../bootstrap3/css/bootstrap.min.css" rel="stylesheet">
    <link href="../bootstrap3/font-awesome/css/font-awesome.css" rel="stylesheet">

    <!-- SB Admin CSS - Include with every page -->
    <link href="../bootstrap3/css/sb-admin.css" rel="stylesheet">
</head>
<body>
    <form id="form2" runat="server">
      <div class="container">
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">概念圖系統</h3>
                    </div>
                    <div class="panel-body">
                        <form role="form">
                            <fieldset>
                                <div class="form-group">
                                    <asp:Login ID="Login1" runat="server" OnAuthenticate="ValidateUser1"></asp:Login>
                                </div>
                            </fieldset>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <!-- Core Scripts - Include with every page -->
    <script src="../Scripts/jquery.min.js"></script>
    <script src="../bootstrap3/js/bootstrap.min.js"></script>
    <script src="../bootstrap3/js/plugins/metisMenu/jquery.metisMenu.js"></script>

    <!-- SB Admin Scripts - Include with every page -->
    <script src="../bootstrap3/js/sb-admin.js"></script>
    <script>
    $(document).ready(function () {
        $('#Login1_UserName').addClass('form-control');
        $('#Login1_Password').addClass('form-control');
        $('#Login1_LoginButton').addClass('btn btn-lg btn-primary btn-block');
        $('table').addClass('table-condensed');
        $('#Login1_LoginButton').click(function () {
            if ($('#Login1_UserName').val() == "" || $('#Login_Password').val() == "")
                alert("請輸入使用者名稱及密碼");
        });
    });   
    </script>
</body>
</html>
