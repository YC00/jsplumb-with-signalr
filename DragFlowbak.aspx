<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DragFlowbak.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.DragFlowbak" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/drag_flow_sub.css" rel="stylesheet" type="text/css" />
    <link href="Styles/drag_flow_main.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.8.23/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Styles/jquery.loadmask.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.jsPlumb-1.3.16-all.js" type="text/javascript"></script>
    <script src="Scripts/jquery.contextmenu.r2.packed.js" type="text/javascript"></script>
    <script src="../../Scripts/signalr.samples.js" type="text/javascript"></script>
    <script src="../../Scripts/json2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.signalR.js" type="text/javascript"></script>
    <script src="../../signalr/hubs" type="text/javascript"></script>
    <script src="../../Scripts/jquery.cookie.js" type="text/javascript"></script>
     <script src="ShapeShare.js" type="text/javascript"></script>
    <script src="Scripts/DragFlow.Init.js" type="text/javascript"></script>
    <script src="Scripts/DragFlow.Render.js" type="text/javascript"></script>
    <script src="Scripts/DragFlow.JQuery.js" type="text/javascript"></script>
    <script src="Scripts/DragFlow.Tools.js" type="text/javascript"></script>
    <style>
        #Iframe1{
          height: 830px;
          width: 1000px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dialog").dialog({
                autoOpen: false,
                modal: true,
                height: 850,
                width: 1000,
                open: function (ev, ui) {
                    $('#Iframe1').attr('src', 'http://140.116.72.19/shape/Hubs/ShapeShare/NodeManagement.aspx');
                }
            });

            $('#dialogBtn').click(function () {
                $('#dialog').dialog('open');
            });
            //$('#dialogBtn').trigger("click");
        });
    </script>
</head>
<body data-library="jquery" data-demo-id="demo" style="margin: 0px;">
    <div id="dialog" style="display:none;">
    <iframe id="Iframe1"  frameBorder="0" src=""></iframe>
</div>
<button id="dialogBtn" style="display:none;"></button>
    <form id="form1" runat="server">
        <div style="width:100%;height:30px;">

            <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem>---</asp:ListItem>
        </asp:DropDownList>
        </div>

     
    <div id="myMenu1" style="position: absolute; z-index: 2000; display: none;">
        <ul>
            <li id="edit">
                <img src="images/edit.gif" />
                設定</li>
            <li id="delete">
              <img src="images/delete.gif" />
                刪除</li>
        </ul>
    </div>
    <div id="myEndpointMenu" style="position: absolute; z-index: 2000; display: none;">
        <ul>
            <li id="editEndpoint">
                <img src="images/edit.gif" />
                編輯</li>
            <li id="deleteEndpoint">
              <img src="images/delete.gif" />
                刪除</li>
        </ul>
    </div>

    <div class="container" style="position: absolute; top: 30px; left: 0px;" id="container">
        <div id="design" class="row-fluid">
            <div class="span2" id="designtool" style="position: absolute;z-index: 100;">
                
                <div class="well sidebar-nav">
                    <ul class="nav nav-list">
                        <li class="nav-header" style="color:Black;">Toolbox</li>
                        <li></li>
                       
                        <li>
                            <div id="toolbar_blue" class="toolbar_padding toolbar_blue " style="width: 75px;">
                            </div>
                        </li>
                        <li>
                            <a href="#" id="update">   <img src="images/save-icon.jpg" width="100" /></a>
                        </li>
                        <!--<li>
                            <a href="#" id="keyword">Add Keyword</a>
                        </li>-->
                    </ul>
                </div>
            </div>
            <div class="span10" id="designpannel" style="width:inherit;float:left;">
              <!--  <div id="window2" class="gradient_green component window">
                    <label for="window2">Testing</label>
                    <div class="ep">
                    </div>
                </div>
                <div id="window6" class="gradient_blue component window">
                    <label for="window6">Testing</label>
                    <div class="ep">
                    </div>
                </div>
             -->
            </div> 
        </div>
    </div>
 
    <script type="text/javascript">
        /// <reference path="../../Scripts/jquery-1.6.4.js" />
        /// <reference path="../../Scripts/jquery-ui-1.8.12.js" />
        /// <reference path="../../Scripts/jQuery.tmpl.js" />
        /// <reference path="../../Scripts/jquery.cookie.js" />
        /// <reference path="../../Scripts/signalR.js" />
        var confirmOnPageExit = function (e) {
            // If we haven't been passed the event get the window.event
            e = e || window.event;

            var message = 'Any text will block the navigation and display a prompt';

            // For IE6-8 and Firefox prior to version 4
            if (e) {
                e.returnValue = message;
            }

            // For Chrome, Safari, IE8+ and Opera 12+
            return message;
        };
        window.onbeforeunload = confirmOnPageExit;

        $(function () {
        
            $(".toolbar_padding").draggable({
                helper: "clone",
                drag: function (event, ui) {
                    //   debugger;
                }
            });

        $(function () {
            $("button")
                .button()
                .click(function (event) {
                    event.preventDefault();
                });
        });

            $('#update').click(function () {
                
                $("#saveDialog").dialog({
                    buttons: {
                        'OK': function () {
                            
                            var cname = $("#conceptMapName").val();
                            //var conceptmapid = 0;
                            //var serializedConceptmap = JSON.stringify(conceptmap);
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "DragFlow.aspx/CreateConceptMap",
                                data: '{ CName: "' + String(cname)+ '", SystemName: "", SystemID: "" }',
                                dataType: "json",
                                beforeSend: function () {
                                    $('#loadingIMG').show();
                                },
                                complete: function () {
                                    $('#loadingIMG').hide();
                                },
                                success: function (data) {
                                    //alert(data.d);
                                    //conceptmapid = parseInt(data.d);
                                    

                                    if (parseInt(data.d)>0) {
                                        var connections = [];
                                        $.each(jsPlumb.getConnections(), function (idx, connection) {
                                            connections.push({
                                                CID: data.d,
                                                connectionId: connection.id,
                                                pageSourceId: connection.sourceId,
                                                pageTargetId: connection.targetId,
                                                label: connection.getOverlay("label").getLabel()
                                            });
                                        });
                                        console.log(JSON.stringify(connections));
                                        /*for (var i = 0; i < connections.length; i++) {
                                            var serializedConnection = JSON.stringify(connections[i]);
                                            $.ajax({
                                                type: "POST",
                                                contentType: "application/json; charset=utf-8",
                                                url: "DragFlow.aspx/NewInsertConnectionData",
                                                data: serializedConnection,
                                                dataType: "json",
                                                success: function (data) {
                                                    var obj = data.d;
                                                    //alert(obj);
                                                    if (obj == 'true') {
                                                        //alert("Successfully");
                                                    }
                                                    if (obj = false) {
                                                        //alert("Failed");
                                                    }
                                                },
                                                error: function (result) {
                                                    //alert("Error");
                                                }
                                            });
                                        }*/
                                        
                                            //var serializedConnection = JSON.stringify(connections);
                                            $.ajax({
                                                type: "POST",
                                                contentType: "application/json; charset=utf-8",
                                                url: "DragFlow.aspx/NewInsertConnectionData",
                                                data: "{connections:" + JSON.stringify(connections) + "}",
                                                dataType: "json",
                                                success: function (data) {
                                                    var obj = data.d;
                                                    //alert(obj);
                                                    if (obj == 'true') {
                                                        //alert("Successfully");
                                                    }
                                                    if (obj = false) {
                                                        //alert("Failed");
                                                    }
                                                },
                                                error: function (result) {
                                                    //alert("Error");
                                                }
                                            });
                                        

                                        //$('#jsonConnection').html(serializedConnection);

                                        var blocks = []
                                        $("#designpannel .component").each(function (idx, elem) {
                                            var $elem = $(elem);
                                            var lbl = $("#" + $elem.attr('id')).find('label');

                                            blocks.push({
                                                CID: data.d,
                                                nodeID: $elem.attr('id'),
                                                nodeLabel: lbl.text(),
                                                positionX: String(parseInt($elem.css("left"), 10)),
                                                positionY: String(parseInt($elem.css("top"), 10)),
                                                width: String(parseInt($elem.css("width"), 10)),
                                                height: String(parseInt($elem.css("height"), 10))
                                            });

                                        });
                                        var savedCheck = "false";
                                        //for (var i = 0; i < blocks.length; i++) {
                                            //var serializedData = JSON.stringify(blocks[i]);
                                            $.ajax({
                                                type: "POST",
                                                contentType: "application/json; charset=utf-8",
                                                url: "DragFlow.aspx/NewInsertNodeData",
                                                data: "{nodes:" +JSON.stringify(blocks)+ "}",
                                                dataType: "json",
                                                success: function (data) {
                                                    var obj = data.d;
                                                    if (obj == 'true') {
                                                        alert("Saved Successfully");

                                                    }
                                                    if (obj = false) {
                                                        //alert("Failed");
                                                    }
                                                },
                                                error: function (result) {
                                                    //alert("Error");
                                                }
                                            });
                                        //}
                                        //if (savedCheck == "true")
                                            
                                        //$('#jsonData').html(serializedData);
                                    }
                                },
                                error: function (xhr, status, error) {
                                    var err = eval("(" + xhr.responseText + ")");
                                    alert(err.Message);
                                }
                            });
                            
                            $(this).dialog('close');
                        }
                    }
                });
                
            });
        });

    </script>
       <span id="jsonData"></span>
    <span id="jsonConnection"></span>
    
<div id="editDialog" title="設定" style="display:none;">
  <p>標籤：<input type="text" id="itemLabel" value="" style="width: 400px;" /></p>
  <p><iframe src=""  style="border:0px #FFFFFF none;" id="myiFrame" name="myiFrame" scrolling="no" frameborder="1" marginheight="0px" marginwidth="0px" height="410px" width="600px"></iframe></p>
</div>
<div id="dialog-confirm" title="刪除" style="display:none;">
  <p>確定?</p>
</div>
<div id="saveDialog" title="儲存概念圖" style="display:none;">
  <p>概念圖名稱：<input type="text" id="conceptMapName" value="" /></p>
</div>
        <input type="hidden" id="templateID" />
        
    </form>
    <div id="loadingIMG" style="display:none"><img src="images/load.gif" />資料處理中，請稍後。</div>
</body>
</html>
