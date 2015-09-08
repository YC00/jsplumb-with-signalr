<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NodeManagement.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.NodeManagement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Node Template</title>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/bootstrap-responsive.css" rel="stylesheet" />
    <link rel="stylesheet" href="Styles/jquery-ui.css" />

    <script type = "text/javascript" src="http://code.jquery.com/jquery-1.11.0.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js" type="text/javascript"></script>
    <script src="Scripts/jscolor.js"></script>
<script type = "text/javascript">
    /*function BlockUI(elementID) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(function () {
            $("#" + elementID).block({
                message: '<table align = "center"><tr><td>' +
                 '<img src="images/loadingAnim.gif"/></td></tr></table>',
                css: {},
                overlayCSS: {
                    backgroundColor: '#000000', opacity: 0.6, border: '3px solid #63B2EB'
                }
            });
        });
        prm.add_endRequest(function () {
            $("#" + elementID).unblock();
        });
    }
    $(document).ready(function () {

        BlockUI("dvGrid");
        $.blockUI.defaults.css = {};
    });*/
    $(document).ready(function () {
    //    $('#copy').click(function () {
    //        $('#GridView1 tr:last').before('<tr><td>10</td><td>Field Name 1</td><td>Field Value 1</td><td><a onclick="return confirm(\'Do you want to delete?\');">刪除</a></td><td><a href="#" style="color:#333333;">編輯</a></td></tr>');
    //        $('#GridView2 tr:last').before('<tr><td>11</td><td>Field Name 2</td><td>Field Value 2</td><td><a onclick="return confirm(\'Do you want to delete?\');">刪除</a></td><td><a href="#" style="color:#333333;">編輯</a></td></tr>');
    //    });
//
        $("#view").click(function () {
            $("#viewdiv").toggle();
        });
    });
    $(function () {
        $("#viewdiv").draggable();
    });
    $(document).mouseup(function (e) {
        var container = $("#viewdiv");

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0) // ... nor a descendant of the container
        {
            container.hide();
        }
    });
</script> 
    <style>
  .custom-combobox {
    position: relative;
    display: inline-block;
  }
  .custom-combobox-toggle {
    position: absolute;
    top: 0;
    bottom: 0;
    margin-left: -1px;
    padding: 0;
    /* support: IE7 */
    *height: 1.7em;
    *top: 0.1em;
  }
  .custom-combobox-input {
    margin: 0;
    padding: 0.3em;
  }
  .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default {
border: 1px solid #d3d3d3;
font-weight: normal;
color: #555555;
}
  </style>
  <script type="text/javascript">
      (function ($) {
          $.widget("custom.combobox", {
              _create: function () {
                  this.wrapper = $("<span>")
                    .addClass("custom-combobox")
                    .insertAfter(this.element);

                  this.element.hide();
                  this._createAutocomplete();
                  this._createShowAllButton();
              },

              _createAutocomplete: function () {
                  var selected = this.element.children(":selected"),
                    value = selected.val() ? selected.text() : "";

                  this.input = $("<input>")
                    .appendTo(this.wrapper)
                    .val(value)
                    .attr("title", "")
                    .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                    .autocomplete({
                        delay: 0,
                        minLength: 0,
                        source: $.proxy(this, "_source")
                    })
                    .tooltip({
                        tooltipClass: "ui-state-highlight"
                    });

                  this._on(this.input, {
                      autocompleteselect: function (event, ui) {
                          ui.item.option.selected = true;
                          this._trigger("select", event, {
                              item: ui.item.option
                          });
                      }
                  });
              },

              _createShowAllButton: function () {
                  var input = this.input,
                    wasOpen = false;

                  $("<a>")
                    .attr("tabIndex", -1)
                    .attr("title", "Show All Items")
                    .tooltip()
                    .appendTo(this.wrapper)
                    .button({
                        icons: {
                            primary: "ui-icon-triangle-1-s"
                        },
                        text: false
                    })
                    .removeClass("ui-corner-all")
                    .addClass("custom-combobox-toggle ui-corner-right")
                    .mousedown(function () {
                        wasOpen = input.autocomplete("widget").is(":visible");
                    })
                    .click(function () {
                        input.focus();

                        // Close if already visible
                        if (wasOpen) {
                            return;
                        }

                        // Pass empty string as value to search for, displaying all results
                        input.autocomplete("search", "");
                    });
              },

              _source: function (request, response) {
                  var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                  response(this.element.children("option").map(function () {
                      var text = $(this).text();
                      if (this.value && (!request.term || matcher.test(text)))
                          return {
                              label: text,
                              value: text,
                              option: this
                          };
                  }));
              },

              _removeIfInvalid: function (event, ui) {

                  // Selected an item, nothing to do
                  if (ui.item) {
                      return;
                  }

                  // Search for a match (case-insensitive)
                  var value = this.input.val(),
                    valueLowerCase = value.toLowerCase(),
                    valid = false;
                  this.element.children("option").each(function () {
                      if ($(this).text().toLowerCase() === valueLowerCase) {
                          this.selected = valid = true;
                          return false;
                      }
                  });

                  // Found a match, nothing to do
                  if (valid) {
                      return;
                  }

                  // Remove invalid value
                  this.input
                    .val("")
                    .attr("title", value + " didn't match any item")
                    .tooltip("open");
                  this.element.val("");
                  this._delay(function () {
                      this.input.tooltip("close").attr("title", "");
                  }, 2500);
                  this.input.data("ui-autocomplete").term = "";
              },

              _destroy: function () {
                  this.wrapper.remove();
                  this.element.show();
              }
          });
      })(jQuery);

      $(function () {
      
          //$(".combobox").combobox();
          //$("#toggle").click(function () {
          //    $("#combobox").toggle();
          //});
      });
      function disp_tab2()
      {
          $("#profilebutton").trigger("click");
      }

      $(function() {
          $("#accordion").accordion({
              collapsible: true
          });
          $('#accordion input[type="checkbox"]').click(function (e) {
              e.stopPropagation();
          });
      });

  </script>
<style>
    .autocomplete
    {
        margin-top: 10px;
    }
    #viewdiv { padding: 0.7em; position:absolute;z-index:3;background-color:white;}
</style>
</head>
<body style ="margin:0;padding:0;">
        <form id="form1" runat="server" class="form-horizontal" role="form">
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header" style="margin-left: 20px;">Node Management</h1>
                </div>
                <!-- /.col-lg-12 -->
            </div>
<!-- Nav tabs -->
<ul class="nav nav-tabs">
  
  <li class="active"><a href="#home" data-toggle="tab">節點Template</a></li>
      <li style="display:none;"><a href="#group" data-toggle="tab">Template Group</a></li>
  <li><a href="#profile" data-toggle="tab" id="profilebutton">新增節點Template</a></li>
</ul>
    <!-- Tab panes -->
<div class="tab-content" style="overflow:hidden;">
     <div class="tab-pane" id="group">
            <div class="container theme-showcase" style="display:none;margin-top:20px;">
               
            </div>
     </div>
  <div class="tab-pane active" id="home">
      <div class="container theme-showcase" style="margin-top:20px;">

          <asp:Panel ID="accordion" runat="server"></asp:Panel><br />
          <div style="float:right;">
              <asp:Button ID="Button4" cssClass="btn" Text="Save as a new template" runat="server" OnClick="ButtonSaveNodeTemplate_Click"/>
              <asp:Button ID="Button3" CssClass="btn btn-success" runat="server" Text="Select" OnClick="ButtonSelectNodeTemplate_Click" />
          </div>
          <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </div>
       <div class="container theme-showcase">
         <asp:gridview id="AuthorsGridView" 
        autogeneratecolumns="true" 
        runat="server" CssClass="table table-bordered" Width="70%">
      </asp:gridview>           <br />
           
           <div>
    </div>
 
    </div>
      </div> 
   
  <div class="tab-pane" id="profile">
      <div class="container theme-showcase" style="margin-top:10px;">
          <div class="panel panel-default">
  <div class="panel-body">

            <asp:Label ID="Label1" runat="server" Text="Template Name : " ></asp:Label>
            <asp:TextBox ID="TextBox1"  runat="server" CssClass="form-control"></asp:TextBox>
  
            <br />
          <asp:Label ID="Label2" runat="server" Text="Node Template : "></asp:Label>
          <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" class="form-control">
          </asp:DropDownList>
            <br />
          <input id="view" type="button" value="View" onclick="ButtonAdd_Click" class="btn" />
          
          <!----view----->
          <div id="viewdiv" style="display: none;"  class="ui-widget-content">
              <asp:GridView ID="GridView3" runat="server"  CssClass="table table-bordered">
                  <Columns>

                        <asp:TemplateField >
                            <ItemTemplate>
                                <asp:CheckBox ID="myCheckBox" runat="server" />
                               
                            </ItemTemplate>
                        </asp:TemplateField>

                  </Columns>

              </asp:GridView>
              
              <asp:Button ID="copy" runat="server"  onclick="ButtonCopy_Click" Text="Copy" CssClass="btn" />
          </div>
          <!-- Modal -->
        
  </div>
</div>
<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h4 class="modal-title" id="myModalLabel">Node Template 1</h4>
      </div>
      <div class="modal-body">
          
      </div>
    </div>
  </div>
</div>
      
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
<div class="panel-group" id="accordion">
  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
          Application General
        </a>
      </h4>
    </div>
 <div id="collapseOne" class="panel-collapse collapse in">
        <div class="panel-body" >
            <asp:gridview ID="Gridview1" runat="server" ShowFooter="true" OnRowDeleting="OnRowDeleting" AutoGenerateColumns="false" CssClass="table table-bordered" OnRowDataBound="OnRowDataBound">
            <Columns>
            <asp:BoundField ItemStyle-Width="5%" DataField="RowNumber" HeaderText="ID" />
            <asp:TemplateField ItemStyle-Width="40%" HeaderText="Field Name">
                <ItemTemplate>
                    <asp:TextBox ID="FieldName1TextBox" class="form-control" runat="server"></asp:TextBox>
                    
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="40%" HeaderText="Field Value">
                <ItemTemplate>
                      <asp:TextBox ID="TextBox1" class="form-control" runat="server"></asp:TextBox>
                    
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Visible">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" /> Show
                </ItemTemplate>
                <FooterStyle HorizontalAlign="Right" />
                
            </asp:TemplateField>
            <asp:TemplateField  ItemStyle-Width="5%" ShowHeader="False">
                <ItemTemplate>
                <asp:Button ID="DeleteButton"
                            Text="Delete"
                            CommandName="Delete" 
                            CssClass="btn btn-danger"
                            runat="server" />
                </ItemTemplate>
                <FooterTemplate>
                 <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" 
                        onclick="ButtonAdd_Click" CssClass="btn"/>
                </FooterTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:gridview>
        </div>
     </div>
    
    </div>

  <div class="panel panel-default">
    <div class="panel-heading">
      <h4 class="panel-title">
        <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">
          Application Specific
        </a>
      </h4>
    </div>
 <div id="collapseTwo" class="panel-collapse collapse in">
        <div class="panel-body" >
            <asp:gridview ID="Gridview2" runat="server" ShowFooter="true" OnRowDeleting="OnRowDeletingSpecific" AutoGenerateColumns="false" CssClass="table table-bordered" OnRowDataBound="OnRowDataBoundSpecific">
            <Columns>
            <asp:BoundField ItemStyle-Width="5%"  DataField="RowNumber" HeaderText="ID" />
            <asp:TemplateField ItemStyle-Width="40%" HeaderText="Field Name">
                <ItemTemplate>
                    <asp:TextBox ID="FieldName2TextBox" class="form-control" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="40%" HeaderText="Field Value">
                <ItemTemplate>
                      <asp:TextBox ID="TextBox2" class="form-control" runat="server"></asp:TextBox>
            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="10%" HeaderText="Visible">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox2" runat="server" /> Show
                </ItemTemplate>
                <FooterStyle HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField  ItemStyle-Width="5%" ShowHeader="False">
                <ItemTemplate>
                <asp:Button ID="DeleteButton2"
                            Text="Delete"
                            CommandName="Delete" 
                            CssClass="btn btn-danger"
                            runat="server" />
                </ItemTemplate>
                <FooterTemplate>
                 <asp:Button ID="ButtonAdd2" runat="server" Text="Add New Row" 
                        onclick="ButtonAdd_Click2" CssClass="btn"/>
                </FooterTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:gridview>
        </div>
      </div>
      </div>
            </div>
        <asp:Button ID="Button1" runat="server"  onclick="ButtonSave_Click"  Text="Submit" CssClass="btn btn-success" />&nbsp;
        <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-warning" />
   
</div>
</div>
             </div>  



        </form>
</body>
</html>
