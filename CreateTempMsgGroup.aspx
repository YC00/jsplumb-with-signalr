<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateTempMsgGroup.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.CreateTempMsgGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>建立臨時推播訊息群組</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellpadding="15" cellspacing="5">
            <tr>
                <td>
                    <asp:Label ID="lbTitle" runat="server" Text="建立臨時推播訊息群組:" Font-Size="XX-Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbGroupName" runat="server" Text="<Font Color='Red'>*</Font>群組名稱:" Font-Size="X-Large"></asp:Label>
                    <asp:TextBox ID="tbGroupName" runat="server" Font-Size="X-Large" Width="230"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbGroupPsw" runat="server" Text="群組密碼:" Font-Size="X-Large"></asp:Label>
                    <asp:TextBox ID="tbGroupPsw" runat="server" Font-Size="X-Large" TextMode="Password" Width="230"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbGroupPswConfirm" runat="server" Text="請再次確認群組密碼:" Font-Size="X-Large"></asp:Label>
                    <asp:TextBox ID="tbGroupPswConfirm" runat="server" Font-Size="X-Large" TextMode="Password" Width="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnPrevious" runat="server" Text="上一步" Font-Size="Medium" 
                        onclick="btnPrevious_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnConfirm" runat="server" Text="確定" Font-Size="Medium" 
                        onclick="btnConfirm_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="取消" Font-Size="Medium" OnClientClick="window.close()"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
