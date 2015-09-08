<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResourceView.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.ResourceView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://code.jquery.com/ui/1.8.23/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <!-- Latest compiled and minified CSS -->
<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css">

<!-- Optional theme -->
<link rel="stylesheet" href="//netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap-theme.min.css">

<!-- Latest compiled and minified JavaScript -->
<script src="//netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
           <div class="panel panel-default">
      <div class="panel-heading">
          <asp:Label ID="title" runat="server" Text="Label"></asp:Label></div>
      <div class="panel-body">
          <asp:Literal ID="description" runat="server"></asp:Literal><br />
          <asp:Label ID="urlLabel" runat="server" Text="參考網址: " Visible="false"></asp:Label>
          <asp:HyperLink ID="url" Target="_blank" runat="server"></asp:HyperLink>
          <span></span>
          
      </div>
    </div>
     <asp:Literal ID="image" runat="server"></asp:Literal>  
     <asp:Literal ID="video" runat="server"></asp:Literal>
         
          
      


    </form>
</body>
</html>
