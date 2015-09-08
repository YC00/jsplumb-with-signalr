<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectkeyword.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.selectkeyword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="bootstrap3/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="bootstrap3/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:ScriptManager runat="server" ID="ScriptManager1" />
           <!-- Placing GridView in UpdatePanel-->
            <asp:UpdatePanel ID="upCrudGrid" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" Width="900px" HorizontalAlign="Center"
                        OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="false" AllowPaging="true"
                        DataKeyNames="id" CssClass="table table-hover table-bordered">
                        <Columns>
                            <asp:TemplateField>
                            <ItemTemplate>
                                <input name="keywordid" type="radio" value='<%# Eval("id") %>' />
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:ButtonField CommandName="detail" ControlStyle-CssClass="btn btn-info"
                                ButtonType="Button" Text="查看" HeaderText="詳細資料">
                                <ControlStyle CssClass="btn btn-info"></ControlStyle>
                            </asp:ButtonField>
                            <asp:BoundField DataField="title" HeaderText="Title" />
                            <asp:BoundField DataField="description" HeaderText="Description" />
                            <asp:BoundField DataField="url" HeaderText="URL" />
                            <asp:BoundField DataField="image" HeaderText="Image" />
                            <asp:BoundField DataField="video" HeaderText="Video" />
                        </Columns>
                    </asp:GridView>
                    
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
           <!-- Detail Modal Starts here-->
            <div id="detailModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                         <div class="modal-dialog">
                      <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalLabel">詳細資料</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="modal-body">
                     
                            <table class="table table-bordered table-hover">
                                <tr>
                                    <td>Title : </td><td>
                                        <asp:Label ID="titleDetail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Description :  </td><td>
                                        <asp:Label ID="descriptionDetail" CssClass="h4" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>URL: </td><td>
                                        <asp:Label ID="urlDetail" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Image: </td><td>
                                        <asp:Image ID="imageDetail" CssClass="img-thumbnail" Width="200" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Video: </td><td>
                                        <asp:HyperLink ID="videoDetail" Target="_blank" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                           
                        </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                    </div>
                </div>
                           </div>
                              </div>
            </div>
            <!-- Detail Modal Ends here -->
    </div>
    </form>
</body>
</html>
