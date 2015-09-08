<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGroup.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.admin.EditGroup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center" width="90%" cellpadding="10">
        <tr>
            <!--群組TreeView-->
            <td valign="top" width="350" style="border: thin solid #000000;">
                <asp:TreeView ID="tvGroup" runat="server" ImageSet="Arrows" 
                    onselectednodechanged="tvGroup_SelectedNodeChanged">
                    <ParentNodeStyle Font-Bold="False" />
                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                    <SelectedNodeStyle Font-Underline="True" ForeColor="Red" 
                        HorizontalPadding="0px" VerticalPadding="0px" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="24pt" ForeColor="Black" 
                        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                </asp:TreeView>
            </td>
            <!--群組修改-->
            <td valign="top">
                <table style="border-color: #000000; border-style: double;" align="center" width="70%">
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Label ID="lbNodeData" runat="server" Text="節點資料" Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbName" runat="server" Text="名稱" Font-Size="Large"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbName" runat="server" Font-Size="Large" Width="180px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnEditName" runat="server" Text="修改名稱" Font-Size="Medium" Enabled="false"
                                onclick="btnEditName_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbChildName" runat="server" Text="新增子節點名稱" Font-Size="Large"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbChildName" runat="server" Font-Size="Large" Width="180px" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnAddChild" runat="server" Text="增加" Font-Size="Medium" Enabled="false"
                                onclick="btnAddChild_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Button ID="btnDeleteNode" runat="server" Text="刪除節點" Font-Size="Medium" Enabled="false"
                                onclick="btnDeleteNode_Click" style="height: 33px" />
                        </td>
                    </tr>
                </table>
                <br /><br />
                <table style="border-color: #000000; border-style: double;" align="right" width="100%">
                    <tr>
                        <td>
                            <asp:ListBox ID="MemberList" runat="server" Height="500px" Width="300px" Font-Size="Larger"></asp:ListBox>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <table style="border-color: #000000; border-style: double;">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lbChairman" runat="server" Text="選擇主席" Font-Size="Larger"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddChairMan" runat="server" Text=">>" Width="100px" 
                                                        onclick="btnAddChairMan_Click" /> 
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="ChairmanList" runat="server" Height="55px" Width="300px" Font-Size="Larger"></asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="border-color: #000000; border-style: double;">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lbRecordMan" runat="server" Text="選擇紀錄者" Font-Size="Larger"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAddRecordMan" runat="server" Text=">>" Width="100px" onclick="btnAddRecordMan_Click"/> 
                                                </td>
                                                <td>
                                                    <asp:ListBox ID="RecordmanList" runat="server" Height="55px" Width="300px" Font-Size="Larger"></asp:ListBox>
                                                </td>
                                        </table>
                                    </td>
                                </tr>
                                <tr  align="right">
                                    <td>
                                         <table> 
                                            <tr align="right">
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" Text="儲存" Width="100px" 
                                                        Font-Size="Larger" onclick="btnSave_Click"/> 
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr> 
                </table>
            </td>
        </tr>
    </table>
    <table align="right">
        <asp:Button ID="btnBack" runat="server" CssClass="ORCS_button_control" Text="返回群組管理" PostBackUrl="GroupManagement.aspx" />
    </table>
    <asp:HiddenField ID="hfNodeValue" runat="server" />
    </form>
</body>
</html>
