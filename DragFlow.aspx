<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DragFlow.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.DragFlow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Concept Map</title>
    <link href="Styles/drag_flow_sub.css" rel="stylesheet" type="text/css" />
    <link href="Styles/drag_flow_main.css" rel="stylesheet" type="text/css" />
    <link href="http://code.jquery.com/ui/1.8.23/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="Styles/jquery.loadmask.css" rel="stylesheet" type="text/css" />
    <link href="Styles/tooltipster.css" rel="stylesheet" type="text/css" />
    <link href="Styles/fixedMenu_style1.css" rel="stylesheet" type="text/css"/>
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="bootstrap3/js/bootstrap.min.js"></script>
    <script src="Scripts/jquery.ui.touch-punch.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.jsPlumb-1.3.16-all.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tooltipster.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.contextmenu.r2.packed.js" type="text/javascript"></script>
    <script src="Scripts/jquery.fixedMenu.js" type="text/javascript"></script>
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

        #accordion .ui-accordion-content { overflow: hidden; }
        #accordion .ui-accordion-header
        {
            padding-left: 30px;
        } 
        #pollSlider-button{
            position: absolute;
            right: -20px;
            top:0;
            background-color: #eee;
            font-size: 6px;
            padding: 3px;
            cursor: pointer;
        }
        #users {
        width: 100%;
        float: left;
        margin-left: 0px;
        padding-left: 0px;
        overflow: auto;
        font-size: small;
        white-space: nowrap;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            //$("#pollSlider-button").toggle(
            //function () {
            //    $('.well').hide('slow');
            //},
            //function () {
            //    $('.well').show('slow');
            //});

           
            $("#pollSlider-button").click(function () {
                if ($(".well").is(":hidden")) {
                    $(".well").show("slide", { direction: "left" });
                    $("#pollSlider-button").css("left", "150px");
                    $("#pollSlider-button").css("width", "20px");
                    $("#pollSlider-button").html("&lt;&lt;");
                }
                else {
                    $(".well").hide("slide", { direction: "left" });
                    $("#pollSlider-button").css("left", "0");
                    $("#pollSlider-button").css("width", "20px");
                    $("#pollSlider-button").html("&gt;&gt;");
                }
            });

            $("#dialog").dialog({
                autoOpen: false,
                modal: true,
                height: 850,
                width: 1000,
                open: function (ev, ui) {
                    $('#Iframe1').attr('src', 'http://140.116.72.19/shape/Hubs/ShapeShare/NodeManagement.aspx');
                }
            });

            

            //$("#radio").buttonset();

            $('#dialogBtn').click(function () {
                $('#dialog').dialog('open');
            });

            $('#userlist').draggable();
           
            
            //$('.linkButton').click(function () {
                //var $target = $(this);
                //console.log(!$target.hasClass('active'));
                //if (!$target.find('active')) {
                    ///alert($('.linkButton .active'));
                //    $('.linkButton').removeClass('active');
                //}
                //$('.linkButton').removeClass('active');
                //alert($target);
            //});
            $('.divtooltip').tooltipster({

                position: 'right',
                contentAsHTML: true
            });

            //$('.linkButton').click(function () {
                //Removing `data-toggle` from all elements
                //$('.linkButton').removeClass('active');
                //Adding `data-toggle` on clicked element
                //$(this).addClass('active');
            //});
            
            //$('#dialogBtn').trigger("click");
            $('.filemenu').fixedMenu();

            
            
        });
       
        function inactiveLinkButton() {
            $('.linkButton').on("click", function () {
                $('.linkButton').removeClass('active');
            });
        }
        $.fn.togglepanels = function () {
            return this.each(function () {
                $(this).addClass("ui-accordion ui-accordion-icons ui-widget ui-helper-reset")
              .find("h3")
                .addClass("ui-accordion-header ui-helper-reset ui-state-default ui-corner-top ui-corner-bottom")
                .hover(function () { $(this).toggleClass("ui-state-hover"); })
                .prepend('<span class="ui-icon ui-icon-triangle-1-e"></span>')
                .click(function () {
                    $(this)
                      .toggleClass("ui-accordion-header-active ui-state-active ui-state-default ui-corner-bottom")
                      .find("> .ui-icon").toggleClass("ui-icon-triangle-1-e ui-icon-triangle-1-s").end()
                      .next().slideToggle();
                    return false;
                })
                .next()
                  .addClass("ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom")
                  .hide();
            });
        };
        function openchatwindow() {
            $("#chatDialog").dialog({
                autoResize: true,
                width: 650
            });
        }
        
    </script>
</head>
<body data-library="jquery" data-demo-id="demo" style="margin: 0px;"  onbeforeunload="savetmpconceptmap()">
        <form id="form1" runat="server">
        <div class="filemenu">
        <ul>
            <li>
                <a href="#">File</a>
                
                <ul>
                    <li><a href="#" onclick="javascript:$('#designpannel').empty();">New</a></li>
                    <li><a href="#" id="openconceptmap">Open</a></li>
                    <li><a href="#" id="update">Save</a></li>
                </ul>
            </li>
            <li>
                <a href="#">View</a>
                
                <ul>
                    <li><a href="#" id="userlistbtn">使用者列表</a></li>
                   <li><a href="#" onclick="javascript:openchatwindow();">對話視窗</a></li>
                </ul>
            </li>
           <!-- <li>
                <a href="#">Edit<span class="arrow"></span></a>
                <ul>
                    <li><a href="http://www.ajaxshake.com/en/JS/12131/galleries.html">Galleries</a></li>
                    <li><a href="http://www.ajaxshake.com/en/JS/1191/menus.html">DropDown Menus</a></li>
                    <li><a href="http://www.ajaxshake.com/en/JS/12581/image-slider.html">Content Slider</a></li>
                    <li><a href="http://www.ajaxshake.com/en/JS/12261/lightbox.html">LightBox</a></li>
                </ul>
            </li>
            <li>
                <a href="#">View<span class="arrow"></span></a>
                <ul>
                    <li><a href="http://www.ajaxshake.com">www.ajaxshake.com</a></li>
                    <li><a href="http://www.solvingequations.net">www.solvingequations.net</a></li>
                    <li><a href="http://www.tutorialjquery.com">www.tutorialjQuery.com</a></li>
                    <li><a href="http://www.jqueryload.com">www.jqueryload.com</a></li>
                </ul>
            </li>
            <li>
                <a href="#">Insert<span class="arrow"></span></a>
                <ul>
                    <li><a href="http://www.twitter.com/ajaxshake">Follow us on Twitter</a></li>
                    <li><a href="http://www.facebook.com/ajaxshake">Facebook</a></li>
                    <li><a href="http://feeds.feedburner.com/ajaxshake">Rss</a></li>
                    <li><a href="mailto:info@ajaxshake.com">e-mail</a></li>
                </ul>
            </li>
            
            <li>
                <a href="#">Help<span class="arrow"></span></a>
                <ul>
                    <li><a href="http://www.twitter.com/ajaxshake">Follow us on Twitter</a></li>
                    <li><a href="http://www.facebook.com/ajaxshake">Facebook</a></li>
                    <li><a href="http://feeds.feedburner.com/ajaxshake">Rss</a></li>
                    <li><a href="mailto:info@ajaxshake.com">e-mail</a></li>
                </ul>
            </li>-->
        </ul>
    </div>
    <hr style="margin:0;"/>
    <div id="dialog" style="display:none;">
    <iframe id="Iframe1"  frameBorder="0" src=""></iframe>
   
</div>
<button id="dialogBtn" style="display:none;"></button>


       
        <div style="width:100%;height:30px;">

            
        </div>

     
    <div id="myMenu1" style="position: absolute; z-index: 2000; display: none;">
        <ul>
            <li id="edit">
                <img src="images/edit.gif" />
                設定</li>
            <li id="resource">
                <img src="images/photos.png" />
                參考資源</li>
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
            <div class="span2" id="designtool" style="position: absolute;z-index: 499;">
                
                <div class="well sidebar-nav" style=" padding: 0px;position: relative;">
                     <div id="accordion" style="padding:0;">
  <div class="group">
    <h3>Node</h3>
    <div>
        <ul class="nav nav-list" style="padding:0;width:100%;">
                        <li class="nav-header" style="color:Black;"></li>
                        <li></li>
                       
                        <asp:Literal ID="nodeTemplatePanel" runat="server"></asp:Literal>
                        <li>
                          
                        </li>
                        <!--<li>
                            <a href="#" id="keyword">Add Keyword</a>
                        </li>-->
                    </ul>  
    </div>
  </div>
  <div class="group">
    <h3>Link</h3>
    <div>
        <ul class="nav nav-list link" style="padding:0;width:100%;">
                        <li class="nav-header" style="color:Black;"></li>
                        <li></li>
                       
                        <asp:Literal ID="linkTemplatePanel" runat="server"></asp:Literal>
                        <li>
                        
                        </li>
                        <!--<li>
                            <a href="#" id="keyword">Add Keyword</a>
                        </li>-->
                    </ul>  
    </div>
  </div>



</div>
             <!--  <div style="text-align: center;">
          <a href="#" id="update">   <img src="images/save-icon.jpg" width="100" /></a>
    </div>        -->       
                     
                </div>
               <div id="pollSlider-button">&lt;&lt;</div> 
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
        //window.onbeforeunload = confirmOnPageExit;

        function parseNodeTemplateJson(nodeid, templateid, obj) {
            var nodeFieldsData = JSON.parse(obj);
            var nodefields = [];
            //var fieldGroup = "<table class='table borderless'><tr><th>Field Name</th><th>Field Value</th></tr>";
            for (var key in nodeFieldsData) {
                if (nodeFieldsData.hasOwnProperty(key)) {
                    //fieldGroup += "<tr><td>" + objData[key]["FieldName"] + "</td><td> " + "<input type=\"hidden\" name=\"fieldName[]\" value=\"" + objData[key]["FieldName"] + "\"><input type=\"text\" name=\"fieldValue[]\" value=\"" + objData[key]["FieldValue"] + "\"/></td></tr>";
                    nodefields.push({
                        nodeID: String(nodeid),
                        templateID: String(templateid),
                        fieldName: String(nodeFieldsData[key]["FieldName"]),
                        fieldValue: String(nodeFieldsData[key]["FieldValue"]),
                        type: String(nodeFieldsData[key]["Type"]),
                        show: '1'
                        //label: connection.getOverlay("label").getLabel()
                    });
                    //objData[key]["FieldName"]
                    //objData[key]["FieldValue"]
                    //alert(objData[key]["FieldName"] + " = " + objData[key]["FieldValue"]);
                }
            }
            //fieldGroup += "</table>";
            return nodefields;
        }

        $(function () {
        
            $(".toolbar_padding").draggable({
                opacity: 0.8,
                helper: "clone",
                appendTo: '#designpannel',
                drag: function (event, ui) {
                    //   debugger;
                }
            });

            $('#resourceclick').click(function () { $("#resourcedialog").dialog({ width: 370 }); });

        $(function () {
            $("button")
                .button()
                .click(function (event) {
                    event.preventDefault();
                });
        });
        $(function () {
            $("#accordion")
              .accordion({
                  header: "> div > h3",
                  collapsible: true,
                  active: true,
                  autoHeight: false,
                  disabled: true
              })
              .sortable({
                  axis: "y",
                  handle: "h3",
                  stop: function (event, ui) {
                      // IE doesn't register the blur when sorting
                      // so trigger focusout handlers to remove .ui-state-focus
                      ui.item.children("h3").triggerHandler("focusout");
                  }
              });
            $('#accordion div h3.ui-accordion-header').click(function () {
                $(this).next().slideToggle();
            });
            $('#accordion div h3:first-child').trigger('click');
        });
        
        $('#openconceptmap').click(function () {

            $("#openDialog").dialog({
                width: '400'
            });
        });
        $('#update').click(function () {

            $("#saveDialog").dialog({
                width: '450',
                buttons: {
                    'OK': {
                        text: 'OK',
                        class: 'btn',
                        click: function () {

                            if ($('#conceptMapName').val() == '')
                                alert("請輸入概念圖名稱！");
                            else{
                                var cname = $("#conceptMapName").val();
                            //var conceptmapid = 0;
                            //var serializedConceptmap = JSON.stringify(conceptmap);
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "DragFlow.aspx/CreateConceptMap",
                                data: '{ CName: "' + String(cname) + '", SystemName: "", SystemID: "", GroupID: "' + $("#groupID").val() + '" }',
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
                                    //alert(data.d);
                                    $('#savedcmid').val(data.d);

                                    if (parseInt(data.d) > 0) {
                                        var connections = [];
                                        $.each(jsPlumb.getConnections(), function (idx, connection) {

                                            var templateData = "";
                                            if (localStorage[connection.id + "_" + $("#groupID").val()] == null)
                                                templateData = String(localStorage["linktemplate-" + connection.getParameter("templateid")]);
                                            else
                                                templateData = String(localStorage[connection.id + "_" + $("#groupID").val()]);
                                            var label = connection.getOverlay("label");
                                            connections.push({
                                                CID: data.d,
                                                //connectionId: connection.id,
                                                connectionId: connection.getParameter("linkid"),
                                                pageSourceId: connection.sourceId,
                                                pageTargetId: connection.targetId,
                                                label: label.getLabel(),
                                                color: connection.paintStyleInUse["strokeStyle"],
                                                createdatetime: connection.getParameter("createdatetime")
                                                //label: connection.getOverlay("label").getLabel()
                                            });
                                            console.log(connections);
                                            //console.log(templateData);
                                            if (templateData != 'undefined') {
                                                var linkFieldsData = JSON.parse(templateData);
                                                var linkfields = [];
                                                //var fieldGroup = "<table class='table borderless'><tr><th>Field Name</th><th>Field Value</th></tr>";
                                                for (var key in linkFieldsData) {
                                                    if (linkFieldsData.hasOwnProperty(key)) {
                                                        //fieldGroup += "<tr><td>" + objData[key]["FieldName"] + "</td><td> " + "<input type=\"hidden\" name=\"fieldName[]\" value=\"" + objData[key]["FieldName"] + "\"><input type=\"text\" name=\"fieldValue[]\" value=\"" + objData[key]["FieldValue"] + "\"/></td></tr>";
                                                        linkfields.push({
                                                            CID: String(data.d),
                                                            nodeID: String(connection.id),
                                                            templateID: String(connection.getParameter("templateid")),
                                                            startNode: String(connection.sourceId),
                                                            endNode: String(connection.targetId),
                                                            fieldName: String(linkFieldsData[key]["FieldName"]),
                                                            fieldValue: String(linkFieldsData[key]["FieldValue"]),
                                                            type: String(linkFieldsData[key]["Type"]),
                                                            show: '1'
                                                            //label: connection.getOverlay("label").getLabel()
                                                        });
                                                        //objData[key]["FieldName"]
                                                        //objData[key]["FieldValue"]
                                                        //alert(objData[key]["FieldName"] + " = " + objData[key]["FieldValue"]);
                                                    }
                                                }
                                                //console.log(linkfields);
                                                $.ajax({
                                                    type: "POST",
                                                    contentType: "application/json; charset=utf-8",
                                                    url: "DragFlow.aspx/InsertLinkFieldsData",
                                                    data: "{linkFields:" + JSON.stringify(linkfields) + "}",
                                                    dataType: "json",
                                                    success: function (data) {
                                                        var obj = data.d;
                                                        if (obj == 'true') {
                                                            //alert("Saved Successfully");


                                                        }
                                                        if (obj = false) {
                                                            alert("Save Link Field Failed");
                                                        }
                                                    },
                                                    error: function (result) {
                                                        //console.log(result);
                                                        alert("Save Link Field Error");
                                                    }
                                                });
                                                console.log(linkfields);
                                            }

                                        });

                                        //console.log(JSON.stringify(connections));
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
                                                    alert("Save Connection Failed");
                                                }
                                            },
                                            error: function (result) {
                                                alert("Save Connection Error");
                                            }
                                        });

                                        //$('#jsonConnection').html(serializedConnection);

                                        var blocks = []
                                        $("#designpannel .component").each(function (idx, elem) {
                                            var $elem = $(elem);
                                            var lbl = $("#" + $elem.attr('id')).find('label');
                                            var templateData = "";
                                            if (localStorage[$elem.attr('id')] == null)
                                                templateData = String(localStorage["template-" + $elem.attr("template-id")]);
                                            else
                                                templateData = String(localStorage[$elem.attr('id')]);
                                            //console.log(templateData);
                                            blocks.push({
                                                CID: data.d,
                                                nodeID: $elem.attr('id'),
                                                nodeLabel: lbl.text(),
                                                positionX: String(parseInt($elem.css("left"), 10)),
                                                positionY: String(parseInt($elem.css("top"), 10)),
                                                width: String(parseInt($elem.css("width"), 10)),
                                                height: String(parseInt($elem.css("height"), 10)),
                                                color: $elem.css('backgroundColor'),
                                                createdDateTime: $elem.attr('ctime')
                                            });
                                            //var templateFieldsData = parseNodeTemplateJson(String($elem.attr('id')), String($elem.attr("template-id")), templateData);
                                            //console.log(String($elem.attr("template-id")));
                                            if (templateData != 'undefined') {
                                                var nodeFieldsData = JSON.parse(templateData);
                                                var nodefields = [];

                                                //var fieldGroup = "<table class='table borderless'><tr><th>Field Name</th><th>Field Value</th></tr>";
                                                for (var key in nodeFieldsData) {

                                                    if (nodeFieldsData.hasOwnProperty(key)) {
                                                        //fieldGroup += "<tr><td>" + objData[key]["FieldName"] + "</td><td> " + "<input type=\"hidden\" name=\"fieldName[]\" value=\"" + objData[key]["FieldName"] + "\"><input type=\"text\" name=\"fieldValue[]\" value=\"" + objData[key]["FieldValue"] + "\"/></td></tr>";
                                                        nodefields.push({
                                                            nodeID: String($elem.attr('id')),


                                                            templateID: String($elem.attr("template-id")),
                                                            fieldName: String(nodeFieldsData[key]["FieldName"]),
                                                            fieldValue: String(nodeFieldsData[key]["FieldValue"]),
                                                            type: String(nodeFieldsData[key]["Type"]),
                                                            show: '1'

                                                            //label: connection.getOverlay("label").getLabel()
                                                        });
                                                        //objData[key]["FieldName"]
                                                        //objData[key]["FieldValue"]



                                                        //alert(objData[key]["FieldName"] + " = " + objData[key]["FieldValue"]);
                                                    }
                                                }
                                                //console.log(nodefields);
                                                $.ajax({
                                                    type: "POST",
                                                    contentType: "application/json; charset=utf-8",
                                                    url: "DragFlow.aspx/InsertNodeFieldsData",
                                                    data: "{nodeFields:" + JSON.stringify(nodefields) + "}",
                                                    dataType: "json",
                                                    success: function (data) {
                                                        var obj = data.d;
                                                        if (obj == 'true') {
                                                            //alert("Saved Successfully");

                                                        }
                                                        if (obj = false) {
                                                            alert("Save Node Field Failed");
                                                        }
                                                    },
                                                    error: function (result) {
                                                        console.log(result);
                                                        alert("Save Node Field Error");
                                                    }
                                                });
                                            }
                                        });
                                        var savedCheck = "false";
                                        //for (var i = 0; i < blocks.length; i++) {
                                        //var serializedData = JSON.stringify(blocks[i]);
                                        $.ajax({
                                            type: "POST",
                                            contentType: "application/json; charset=utf-8",
                                            url: "DragFlow.aspx/NewInsertNodeData",
                                            data: "{nodes:" + JSON.stringify(blocks) + "}",
                                            dataType: "json",
                                            success: function (data) {
                                                var obj = data.d;
                                                if (obj == 'true') {
                                                    alert("Saved Successfully");

                                                    var CMName = $('#conceptMapName').val();
                                                    var savedcmid = $('#savedcmid').val();
                                                    $('#DropDownList1').append($('<option/>', {
                                                        value: savedcmid,
                                                        text: CMName
                                                    }));
                                                }
                                                if (obj = false) {
                                                    alert("Save Node Failed");

                                                }
                                            },
                                            error: function (result) {
                                                alert("Save Node Error");
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
                    }
                }

            });


        });
        });
        //$(window).parent().on('beforeunload', function () {
        //    if ($('#chairman').val() == '1') {
        //        savetmpconceptmap();
        //    }
        //    return "確定要離開嗎？"; // you can make this dynamic, ofcourse...
        $(window).on('beforeunload', function () {
            //    if ($('#chairman').val() == '1') {
            //        savetmpconceptmap();
            //    }
            return "確定要離開嗎？"; // you can make this dynamic, ofcourse...

        });
        //});
        function savetmpconceptmap() {
            //alert("");
                var now = new Date();
                var cname = String(now.format("yyyy/MM/dd h:mm tt"))+" Concept Map";
                //var conceptmapid = 0;
                //var serializedConceptmap = JSON.stringify(conceptmap);
                $.ajax({
                    type: "POST",


                    contentType: "application/json; charset=utf-8",
                    url: "DragFlow.aspx/CreateConceptMap",
                    data: '{ CName: "' + String(cname) + '", SystemName: "", SystemID: "", GroupID: "' + $("#groupID").val() + '" }',
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


                        if (parseInt(data.d) > 0) {
                            alert(data.d);
                            $('#savedcmid').val(data.d);
                            var connections = [];
                            $.each(jsPlumb.getConnections(), function (idx, connection) {

                                var templateData = "";
                                if (localStorage[connection.id + "_" + $("#groupID").val()] == null)
                                    templateData = String(localStorage["linktemplate-" + connection.getParameter("templateid")]);
                                else
                                    templateData = String(localStorage[connection.id + "_" + $("#groupID").val()]);
                                var label = connection.getOverlay("label");
                                connections.push({
                                    CID: data.d,
                                    //connectionId: connection.id,
                                    connectionId: connection.getParameter("linkid"),
                                    pageSourceId: connection.sourceId,
                                    pageTargetId: connection.targetId,
                                    label: label.getLabel(),
                                    color: connection.paintStyleInUse["strokeStyle"],
                                    createdatetime: connection.getParameter("createdatetime")
                                    //label: connection.getOverlay("label").getLabel()
                                });

                                //console.log(templateData);
                                if (templateData != 'undefined') {
                                    var linkFieldsData = JSON.parse(templateData);
                                    var linkfields = [];
                                    //var fieldGroup = "<table class='table borderless'><tr><th>Field Name</th><th>Field Value</th></tr>";
                                    for (var key in linkFieldsData) {
                                        if (linkFieldsData.hasOwnProperty(key)) {
                                            //fieldGroup += "<tr><td>" + objData[key]["FieldName"] + "</td><td> " + "<input type=\"hidden\" name=\"fieldName[]\" value=\"" + objData[key]["FieldName"] + "\"><input type=\"text\" name=\"fieldValue[]\" value=\"" + objData[key]["FieldValue"] + "\"/></td></tr>";
                                            linkfields.push({
                                                CID: String(data.d),
                                                nodeID: String(connection.id),
                                                templateID: String(connection.getParameter("templateid")),
                                                startNode: String(connection.sourceId),
                                                endNode: String(connection.targetId),
                                                fieldName: String(linkFieldsData[key]["FieldName"]),
                                                fieldValue: String(linkFieldsData[key]["FieldValue"]),
                                                type: String(linkFieldsData[key]["Type"]),
                                                show: '1'
                                                //label: connection.getOverlay("label").getLabel()
                                            });
                                            //objData[key]["FieldName"]
                                            //objData[key]["FieldValue"]
                                            //alert(objData[key]["FieldName"] + " = " + objData[key]["FieldValue"]);
                                        }
                                    }
                                    //console.log(linkfields);
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: "DragFlow.aspx/InsertLinkFieldsData",
                                        data: "{linkFields:" + JSON.stringify(linkfields) + "}",
                                        dataType: "json",
                                        success: function (data) {
                                            var obj = data.d;
                                            if (obj == 'true') {
                                                //alert("Saved Successfully");


                                            }
                                            if (obj = false) {
                                                alert("Failed");
                                            }
                                        },
                                        error: function (result) {
                                            //console.log(result);
                                            alert("Error");
                                        }
                                    });
                                }

                            });

                            //console.log(JSON.stringify(connections));
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
                                var templateData = "";
                                if (localStorage[$elem.attr('id')] == null)
                                    templateData = String(localStorage["template-" + $elem.attr("template-id")]);
                                else
                                    templateData = String(localStorage[$elem.attr('id')]);
                                //console.log(templateData);
                                blocks.push({
                                    CID: data.d,
                                    nodeID: $elem.attr('id'),
                                    nodeLabel: lbl.text(),
                                    positionX: String(parseInt($elem.css("left"), 10)),
                                    positionY: String(parseInt($elem.css("top"), 10)),
                                    width: String(parseInt($elem.css("width"), 10)),
                                    height: String(parseInt($elem.css("height"), 10)),
                                    color: $elem.css('backgroundColor'),
                                    createdDateTime: $elem.attr('ctime')
                                });
                                //var templateFieldsData = parseNodeTemplateJson(String($elem.attr('id')), String($elem.attr("template-id")), templateData);
                                //console.log(String($elem.attr("template-id")));
                                if (templateData != 'undefined') {
                                    var nodeFieldsData = JSON.parse(templateData);
                                    var nodefields = [];

                                    //var fieldGroup = "<table class='table borderless'><tr><th>Field Name</th><th>Field Value</th></tr>";
                                    for (var key in nodeFieldsData) {

                                        if (nodeFieldsData.hasOwnProperty(key)) {
                                            //fieldGroup += "<tr><td>" + objData[key]["FieldName"] + "</td><td> " + "<input type=\"hidden\" name=\"fieldName[]\" value=\"" + objData[key]["FieldName"] + "\"><input type=\"text\" name=\"fieldValue[]\" value=\"" + objData[key]["FieldValue"] + "\"/></td></tr>";
                                            nodefields.push({
                                                nodeID: String($elem.attr('id')),


                                                templateID: String($elem.attr("template-id")),
                                                fieldName: String(nodeFieldsData[key]["FieldName"]),
                                                fieldValue: String(nodeFieldsData[key]["FieldValue"]),
                                                type: String(nodeFieldsData[key]["Type"]),
                                                show: '1'

                                                //label: connection.getOverlay("label").getLabel()
                                            });
                                            //objData[key]["FieldName"]
                                            //objData[key]["FieldValue"]



                                            //alert(objData[key]["FieldName"] + " = " + objData[key]["FieldValue"]);
                                        }
                                    }
                                    //console.log(nodefields);
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: "DragFlow.aspx/InsertNodeFieldsData",
                                        data: "{nodeFields:" + JSON.stringify(nodefields) + "}",
                                        dataType: "json",
                                        success: function (data) {
                                            var obj = data.d;
                                            if (obj == 'true') {
                                                //alert("Saved Successfully");

                                            }
                                            if (obj = false) {
                                                alert("Failed");
                                            }
                                        },
                                        error: function (result) {
                                            console.log(result);
                                            alert("Error");
                                        }
                                    });
                                }
                            });
                            var savedCheck = "false";
                            //for (var i = 0; i < blocks.length; i++) {
                            //var serializedData = JSON.stringify(blocks[i]);
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "DragFlow.aspx/NewInsertNodeData",
                                data: "{nodes:" + JSON.stringify(blocks) + "}",
                                dataType: "json",
                                success: function (data) {
                                    var obj = data.d;
                                    if (obj == 'true') {
                                        alert("Saved Successfully");

                                        var CMName = $('#conceptMapName').val();
                                        var savedcmid = $('#savedcmid').val();
                                        $('#DropDownList1').append($('<option/>', {
                                            value: savedcmid,
                                            text: CMName
                                        }));
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
    </script>
       <span id="jsonData"></span>
    <span id="jsonConnection"></span>

  <!-- <div class="ui-widget-content well well-sm" id="userlist" style="diaplay:none;padding-left:19px;padding-right:19px;padding-top:0px;padding-bottom:0;">
      <h3 style="text-decoration:underline;color:#e0e0e0;">使用者</h3>
      <div class="panel-body">

      
          <table style="width: 50%;">
              <tr>
                  <td>立祥</td>
                  <td><span style="color:#e0e0e0;">(教師)</span></td>
              </tr>
              <tr>
                  <td>楊書睿</td>
                  <td><span style="color:#e0e0e0;">(學生)</span></td>
              </tr>
              <tr>
                  <td>孫稟睿</td>
                  <td><span style="color:#e0e0e0;">(學生)</span></td>
              </tr>
              <tr>
                  <td>胡愷育</td>
                  <td><span style="color:#e0e0e0;">(學生)</span></td>
              </tr>
          </table>
      </div>
   </div>-->

<div id="editDialog" title="設定" style="display:none;">

  <p>標籤：<input type="text" id="itemLabel" style="width:400px;" /></p>
  <!--<p><iframe src=""  style="border:0px #FFFFFF none;" id="myiFrame" name="myiFrame" scrolling="no" frameborder="1" marginheight="0px" marginwidth="0px" height="410px" width="600px"></iframe></p>-->
     <div id="tempField"></div>
</div>
<div id="editLabelDialog" title="設定" style="display:none;">
  <p>標籤：<input type="text" id="linkLabel" style="width:400px;"/></p>
  <!--<p><iframe src=""  style="border:0px #FFFFFF none;" id="myiFrame" name="myiFrame" scrolling="no" frameborder="1" marginheight="0px" marginwidth="0px" height="410px" width="600px"></iframe></p>-->
    <div id="tempLinkField"></div>
</div>
<div id="dialog-confirm" title="刪除" style="display:none;">
  <p>確定?</p>
</div>
<div id="openDialog" title="開啟概念圖" style="display:none;">
  <p><asp:DropDownList ID="DropDownList1" Width="370" runat="server">
            <asp:ListItem>---</asp:ListItem>
        </asp:DropDownList></p>
</div>

<div id="saveDialog" title="儲存概念圖" style="display:none;">
  <p>概念圖名稱：<input type="text" id="conceptMapName" value="" style="width:400px;" /></p>
</div>
<div id="resourceDialog" title="參考資源" style="display:none;">
   <button type="button" class="btn" id="setresource" style="padding-bottom:10px;">設定參考資源</button>
    <iframe id="resourceIframe" width="100%" height="100%" style="margin-top:10px;" scrolling="no" frameborder="0" src=""></iframe>
     
</div>
<div id="userListDialog" title="使用者列表" style="display:none;">
       <ul id="users">
        </ul>
</div>        <!-- resource dialog start -->
<div id="sResourceDialog" title="設定參考資源" style="display:none;">

                    <asp:GridView ID="GridView1" runat="server" Width="900px" HorizontalAlign="Center"
                        OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="false" AllowPaging="true"
                        DataKeyNames="id" CssClass="table table-hover table-bordered">
                        <Columns>
                            <asp:TemplateField>
                            <ItemTemplate>
                                <input name="keywordid" type="radio" value='<%# Eval("id") %>' />
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="title" HeaderText="Title" />
                            <asp:BoundField DataField="description" HeaderText="Description" />
                            <asp:BoundField DataField="url" HeaderText="URL" />
                            <asp:HyperLinkField HeaderText="Image" DataNavigateUrlFields="image" DataNavigateUrlFormatString="images/keyword/{0}" Target="_blank" Text="image" />
                            <asp:HyperLinkField HeaderText="Video" DataNavigateUrlFields="video" DataNavigateUrlFormatString="images/keyword/{0}" Target="_blank" Text="Video" />
                        </Columns>
                    </asp:GridView>
       
           <!-- Detail Modal Starts here-->
            <div id="detailModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                         <div class="modal-dialog">
                      <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h3 id="myModalLabel">詳細資料</h3>
                </div>
                <div class="modal-body">
          
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
                    
                    <div class="modal-footer">
                        <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Close</button>
                    </div>
                </div>
                           </div>
                              </div>
            </div>
</div>
             <!-- resource dialog end -->

<div id="chatDialog" title="對話視窗" style="display:none;-webkit-user-select: none;
-khtml-user-select: none;
-moz-user-select: none;
-o-user-select: none;
user-select: none;">
 
       <div id="btndiv"><button type="button" id="copytext">Create Node</button>&nbsp;將對話內容反白點擊此按鈕即可新增節點</div>

    <textarea id="chatwindow" style="height:400px;width:100%;overflow: auto;cursor:text;"></textarea>
    <input type="text" id="chatinput"  style="width:100%;" />
</div>
     <div class="alert alert-warning alert-dismissible"  id="alertlock" role="alert" style="width:50%;margin:0 auto;display:none;margin-top: -60px;">
  <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only"></span></button>
  <strong>注意！</strong> 編輯操作功能暫時被鎖定了
</div>
            <div class="alert alert-success alert-dismissible" id="alertunlock" role="alert" style="width:50%;margin:0 auto;display:none;margin-top: -60px;">
  <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only"></span></button>
   編輯操作功能已解鎖！
</div>
            <div class="alert alert-success alert-dismissible" id="alertmsg" role="alert" style="width:50%;margin:0 auto;display:none;margin-top: -60px;">
  <button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">&times;</span><span class="sr-only"></span></button>
        <span id="movemsg"></span>
</div>
        <input type="hidden" id="templateID" />
        <input type="hidden" id="linktemplateID" />
        <input type="hidden" id="linktemplateColor" />
        <input type="hidden" id="groupID" value="0" />
        <input type="hidden" id="userID" value="" />
        <input type="hidden" id="username" value="" />
        <input type="hidden" id="chairman" value="0" />
        <input type="hidden" id="tmplinkid" value="0" />
        <input type="hidden" id="tmptpllinkid" value="0" />
        <input type="hidden" id="savedcmid" value="0" />
        <asp:Literal ID="autocompletedata" runat="server"></asp:Literal>
    
    <div id="loadingIMG" style="display:none"><img src="images/load.gif" />資料處理中，請稍後。</div>
    <div style="display:none;top:0;left:0;position:absolute;width:100%;height:100%;opacity:0;z-index:1000;background:#fff;" id="student"></div>
</form>
</body>
</html>
