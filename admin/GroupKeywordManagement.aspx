<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupKeywordManagement.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.admin.GroupKeywordManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../bootstrap3/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../bootstrap3/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 20%; float:left">
       <asp:TreeView ID="tvGroup" runat="server" ImageSet="Arrows"
                        onselectednodechanged="tvGroup_SelectedNodeChanged">
                        <ParentNodeStyle Font-Bold="False" />
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="Red" 
                            HorizontalPadding="0px" VerticalPadding="0px" />
                        <NodeStyle Font-Names="Tahoma" Font-Size="24pt" ForeColor="Black" 
                            HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                    </asp:TreeView>
    </div>
        <div style="width: 70%; margin-right: 5%; margin-left: 5%; text-align: left; float:right;">
          <div style="text-align:left;padding-top:10px;padding-bottom:10px;">
              <asp:Button ID="btnAdd" runat="server" Text="新增記錄" CssClass="btn btn-info" OnClick="btnAdd_Click" />
              </div>
            <asp:ScriptManager runat="server" ID="ScriptManager1" />
           <!-- Placing GridView in UpdatePanel-->
            <asp:UpdatePanel ID="upCrudGrid" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" Width="100%" HorizontalAlign="Center"
                        OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="false" AllowPaging="true"
                        DataKeyNames="id" CssClass="table table-hover table-bordered" RowStyle-Wrap="true">
                        <HeaderStyle Width="10%" />
                        <RowStyle Width="10%" />
                        <FooterStyle Width="10%" />
                        <Columns>
                            <asp:ButtonField CommandName="detail" ControlStyle-CssClass="btn btn-info"
                                ButtonType="Button" Text="查看" HeaderText="詳細資料">
                                <ControlStyle CssClass="btn btn-info"></ControlStyle>
                            </asp:ButtonField>
                            <asp:ButtonField CommandName="editRecord" ControlStyle-CssClass="btn btn-info"
                                ButtonType="Button" Text="編輯" HeaderText="編輯記錄">
                                <ControlStyle CssClass="btn btn-info"></ControlStyle>
                            </asp:ButtonField>
                            <asp:ButtonField CommandName="deleteRecord" ControlStyle-CssClass="btn btn-info"
                                ButtonType="Button" Text="刪除" HeaderText="刪除記錄">
                                <ControlStyle CssClass="btn btn-info"></ControlStyle>
                            </asp:ButtonField>
                            <asp:BoundField DataField="title" HeaderText="Title" />
                            <asp:BoundField DataField="description" HeaderText="Description" />
                            <asp:BoundField   HeaderStyle-Width="10%" ItemStyle-Width="10%"
            FooterStyle-Width="10%" DataField="url" HeaderText="URL" />
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
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
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
            <!-- Edit Modal Starts here -->
            <div id="editModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                      <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="editModalLabel">編輯記錄</h3>
                </div>
                   
                <asp:UpdatePanel ID="upEdit" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                                <asp:HiddenField ID="editID" runat="server" />
                            <table class="table table-bordered table-hover">
                                <tr>
                                    <td>Title : </td><td>
                                        <asp:TextBox ID="titleEdit" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Description :  </td><td>
                                        <asp:TextBox ID="descriptionEdit" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>URL: </td><td>
                                        <asp:TextBox ID="urlEdit" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Image: </td><td>
                                        <asp:FileUpload ID="imageUpload" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Video: </td><td>
                                         <asp:FileUpload ID="videoUpload" runat="server" />
                                    </td>
                                </tr>
                            </table>
                           
                        </div>
                        <div class="modal-footer">
                            <asp:Label ID="lblResult" Visible="false" runat="server"></asp:Label>
                            <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-info" OnClick="btnSave_Click" />
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                        <asp:PostBackTrigger ControlID="btnSave" />
                    </Triggers>
                </asp:UpdatePanel>
                           </div>
                     </div>
            </div>
            <!-- Edit Modal Ends here -->
            <!-- Add Record Modal Starts here-->
            <div id="addModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" aria-hidden="true">
             <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="addModalLabel">新增記錄</h3>
                </div>
                <asp:UpdatePanel ID="upAdd" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <table class="table table-bordered table-hover">
                                <tr>
                                    <td>Title : 
                                </td>
                                    <td><asp:TextBox ID="txtTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Description : 
                                     </td>
                                    <td>   <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>URL :
                                     </td>
                                    <td>   <asp:TextBox ID="txtURL" CssClass="form-control" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Image :</td>
                                    <td>   
                                     <asp:FileUpload ID="txtImageUpload" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Video:
                                     </td>
                                    <td>   <asp:FileUpload ID="txtVideoUpload" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="modal-footer">                          
                            <asp:Button ID="btnAddRecord" runat="server" Text="Add" CssClass="btn btn-info" OnClick="btnAddRecord_Click" />
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnAddRecord" />
                    </Triggers>
                </asp:UpdatePanel>
                    </div>
                 </div>
            </div>
            <!--Add Record Modal Ends here-->
            <!-- Delete Record Modal Starts here-->
            <div id="deleteModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="delModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="delModalLabel">刪除記錄</h3>
                </div>
                <asp:UpdatePanel ID="upDel" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            確定刪除?
                            <asp:HiddenField ID="hfCode" runat="server" />
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-info" OnClick="btnDelete_Click" />
                            <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cancel</button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                     </div>
                     </div>
            </div>
            <!--Delete Record Modal Ends here -->
        </div>
                 <asp:HiddenField ID="hfNodeValue" runat="server" Value="" />
        <asp:Label ID="lblUserName" runat="server" />

    </form>
</body>
</html>
