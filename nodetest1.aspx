<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nodetest1.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.nodetest1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css">
	<script src="//code.jquery.com/jquery-1.10.2.js"></script>
	<script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script src="Scripts/jscolor.js"></script>
    <style>
        .myClass {
        width: 300px;   
        height: 300px;
        margin: auto;   /*版面居中對齊*/
        font-family: Arial, Helvetica, sans-serif;
        border: 1px solid #fff; /*網頁做外框的設定*/
        }

    </style>
    <script>
     $(function() {
         $("#accordion").accordion();
         $('#accordion input[type="checkbox"]').click(function (e) {
             e.stopPropagation();
         });
      });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gridView1" runat="server">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                        <asp:HiddenField ID="hdValue" runat="server" Value='<%#Eval("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:Button ID="btnMove" runat="server" Text="Move" OnClick="btnMove_Click" />
    </div>
    <div>
        <asp:GridView ID="gridView2" runat="server">
        </asp:GridView>
    </div>
        <asp:Panel ID="pnlYogesh" runat="server"></asp:Panel>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    
<div id="accordion" runat="server">

</div>
    </form>
</body>
</html>
