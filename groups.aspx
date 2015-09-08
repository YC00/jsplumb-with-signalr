<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="groups.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.groups" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="bootstrap3/css/bootstrap.css" rel="stylesheet" type="text/css" />
     <script src="Scripts/jquery.min.js" type="text/javascript"></script>
     <script src="bootstrap3/js/bootstrap.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#profile').popover();
        });
    </script>
</head>
<body>

    <form id="form1" runat="server">
        <ul class="nav nav-pills" style="float:right;">
                            <!--<li><a id="notifications" class="" title="Notifications" href="#" target="_blank"><i class="glyphicon glyphicon-comment"></i><span id="notification-unread-count" class="badge badge-info" style="display: none;"></span></a></li>-->
                            <li><a id="profile" href="#" data-placement="bottom"  <asp:Literal ID="UserLabelTop" runat="server"></asp:Literal>><i class="glyphicon glyphicon-user"></i></a></li>
                            <asp:Literal ID="adminmenu" runat="server"></asp:Literal>
                            <!--<li><a class="help" aria-haspopup="true" href="#" title="Display Help"><i class="glyphicon glyphicon-question-sign"></i></a></li>
                            <li><a class="logout" href="#" title="Sign Out"><i class="glyphicon glyphicon-log-out"></i></a></li>-->
                        </ul>
        <h1>概念圖系統</h1>
               <hr />
        <h4>
           <asp:Label ID="UserLabel" runat="server" Text=""></asp:Label>
        </h4>
        <hr />
        <asp:ListView ID="ListView1" runat="server">
            <LayoutTemplate> 
                <div class="list-group">
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                 <a href="DragFlow.aspx?userid=<%# Eval("userid") %>&groupID=<%# Eval("iTempGroupID") %>&chairman=<%# Eval("chairman") %>&name=<%# Eval("username") %>" class="list-group-item">
                    <span class="badge"><%# Eval("total") %></span>
                    <%# Eval("title") %>
                 </a>
            </ItemTemplate>
        </asp:ListView>
    </form>
</body>
</html>
