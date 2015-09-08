<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrowsingTool.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.BrowsingTool" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

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
    <script>
        var availableTags = [];
    </script>
    <script src="shapeshare.js" type="text/javascript"></script>
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
        /*#pathaccordion .ui-accordion-content { overflow: hidden; }
        #pathaccordion .ui-accordion-header
        {
            padding-left: 30px;
        }*/ 
        #pollSlider-button{
            position: absolute;
            right: -20px;
            top:0;
            background-color: #eee;
            font-size: 6px;
            padding: 3px;
            cursor: pointer;
            display: none;
        }
        #overlay {
            background: black;
            opacity: 0.1;
            display:none;
            width:100%; height:100%;
            position:absolute; top:0; left:0; z-index:5;
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

            $("#colorpanel").dialog({
                autoOpen: true,
                modal: false,
                width: 800,
                height: 160,
                position: { my: 'right bottom', at: 'right bottom', of: window }
            });

            //$("#answerpanel").dialog({
            //    autoOpen: false,
            //    modal: false,
            //    width: 800,
            //    height: 600,
            //    position: { my: 'right top', at: 'right top', of: window }
            //}).parent().attr('id', 'answerpaneldialog');;


            $("#statpanel").dialog({
                autoOpen: false,
                modal: false,
                width: 600,
                height: 530,
                position: { my: 'right bottom', at: 'right bottom', of: window }
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
            $('#pathaccordion').dialog({
                autoOpen: true,
                modal: false,
                width: 800,
                height: 400,
                position: { my: 'left bottom', at: 'left bottom', of: window }
            });
            //$("#radio").buttonset();

            $('#dialogBtn').click(function () {
                $('#dialog').dialog('open');
            });
            
            $('#userlist').draggable();

            $(".component").on("click", function () { $(this).css("background", "#FF7373"); });
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
            $(function () {
                $("#slider-vertical").slider({
                    orientation: "vertical",
                    range: "min",
                    min: 100,
                    max: 300,
                    value: 0,
                    slide: function (event, ui) {
                        $("#designpannel").css("zoom",String(ui.value)+"%");
                        $("#amount").html(String(ui.value) + "%");
                    }
                });
                $("#amount").val($("#slider-vertical").slider("value"));

                $("#highlight-vertical").slider({
                    orientation: "vertical",
                    range: "min",
                    min: 1,
                    max: 10,
                    value: 0,
                    slide: function (event, ui) {
                        //$("#designpannel").css("zoom", String(ui.value) + "%");
                        $("#highlightamount").html(String(ui.value));
                        if (ui.value > 1) {
                            $('#overlay').show();
                            $('#overlay').css('opacity', parseFloat(ui.value / 10).toFixed(1));
                        } else {
                            $('#overlay').hide();
                        }
                    }
                });
                $("#transparent-vertical").slider({
                    orientation: "vertical",
                    range: "min",
                    min: 6,
                    max: 10,
                    value: 10,
                    slide: function (event, ui) {
                        //$("#designpannel").css("zoom", String(ui.value) + "%");
                        //$("#highlightamount").html(String(ui.value));
                        if (ui.value > 1) {
                            //$('#designpannelt').show();
                            $('#answerpaneldialog').css('opacity', parseFloat(ui.value / 10).toFixed(1));
                        } else {
                            $('#answerpaneldialog').css('opacity', parseFloat(ui.value / 10).toFixed(1));
                        }
                    }
                });
                $('#designpannelt').draggable();
                $('#designpannel').draggable();
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
        function opencolorpanel() {
            $("#colorpanel").dialog('open');
        }

        function openstatpanel() {
            $("#statpanel").dialog('open');
        }
        function openanswerpanel() {
            $("#answerpanel").dialog('open');
        }
        function openpathpanel() {
            $("#pathaccordion").dialog('open');
        }
        function inactiveLinkButton() {
            $('.linkButton').on("click", function () {
                $('.linkButton').removeClass('active');
            });
        }
        function paintlink(color) {
            $('path').attr('stroke',color);
            $('path').attr('fill',color);
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

    </script>
</head>
<body data-library="jquery" data-demo-id="demo" style="margin: 0px;">
   
        <form id="form1" runat="server">
             <div class="filemenu">
        <ul style="position: absolute;z-index: 10000;">
            <li>
                <a href="#">File</a>
                
                <ul>
                    <!--<li><a href="#" onclick="javascript:$('#designpannel').empty();">New</a></li>-->
                    <li><a href="#" id="openconceptmap">Open</a></li>
                    <!--<li><a href="#" id="update">Save</a></li>-->
                </ul>
            </li>
            <li>
                <a href="#">View</a>
                
                <ul>
                    <li style="width:100px;"><a href="#" onclick="javascript:opencolorpanel();">顏色對照</a></li>
                    <li style="width:100px;"><a href="#" onclick="javascript:openstatpanel();">概念圖統計表</a></li>
                    <!--<li style="width:100px;"><a href="#" onclick="javascript:openanswerpanel();">概念圖標準答案</a></li>-->
                    <li style="width:100px;"><a href="#" onclick="javascript:openpathpanel();">Match Table</a></li>
                </ul>
            </li>
        </ul>
        </div>
        <!-- Nav tabs -->
<ul class="nav nav-tabs">
  <li class="active"><a href="#home" id="homebtn" data-toggle="tab">學生概念圖</a></li>
  <!--<li><a href="#profile" id="profilebtn" data-toggle="tab">概念圖標準答案</a></li>-->
</ul>

<!-- Tab panes -->
<div class="tab-content">
  <div class="tab-pane active" id="home"> <div class="filemenu" style="top:0;">
        <ul>
            <li>

            </li>
            <li>
                &nbsp;起始節點: <select id="startnode"></select> &nbsp;&nbsp;&nbsp;結束節點: <select id="endnode"></select> <input type="button" class="btn" id="selectsenode" value="select" style="margin-top: -10px;"/>
             </li>
        </ul>
        </div>

     
    <div id="myMenu1" style="position: absolute; z-index: 2000; display: none;">
       
    </div>
    <div id="myEndpointMenu" style="position: absolute; z-index: 2000; display: none;">
       
    </div>

    <div class="container" style="position: absolute; top: 120px; left: 0px;" id="container">
        <div id="design" class="row-fluid">
            <div class="span2" id="designtool" style="position: absolute;z-index: 499;">
                
                <div class="well sidebar-nav" style=" padding: 0px;position: relative;">
                     <div id="accordion" style="padding:0;">
  <div class="group">
    <h3>Play</h3>
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
    <h3>Refresh</h3>
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
<div class="group">
    <h3>Zoom</h3>
    <div>
         <div style="height: 35px;"><span id="amount">100%</span> </div>
       <div id="slider-vertical" style="height:150px;"></div>
         
    </div>
  </div>
<div class="group" style="">
    <h3>Highlight</h3>
    <div>
         <div style="height: 35px;"><span id="highlightamount">0</span> </div>
       <div id="highlight-vertical" style="height:150px"></div>
         
    </div>
  </div>

</div>
                    



</div>
             <!--  <div style="text-align: center;">
          <a href="#" id="update">   <img src="images/save-icon.jpg" width="100" /></a>
    </div>        -->       
                     
                </div>
               <div id="pollSlider-button">&lt;&lt;</div> 
            </div>
            <div class="span10" id="designpannel" style="width:inherit;float:left;">  </div> 

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
          
         <div class="span10" id="designpannelt" style="width:auto;float:left;"></div>     
        </div>
    </div>
 <!-- <div class="tab-pane" id="profile">
  </div>-->

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
                opacity: 0.7,
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

                //$("#pathaccordion")
                //  .accordion({
                //      header: "> div > h3",
                //      collapsible: true,
                //      active: true,
                //      autoHeight: false,
                //      disabled: true
                //  })
                //  .sortable({
                //      axis: "y",
                //      handle: "h3",
                //      stop: function (event, ui) {
                //          // IE doesn't register the blur when sorting
                //          // so trigger focusout handlers to remove .ui-state-focus
                //          ui.item.children("h3").triggerHandler("focusout");
                //      }
                //  });
                //$('#pathaccordion div h3.ui-accordion-header').click(function () {
                //    $(this).next().slideToggle();
                //});
                //$('#pathaccordion div h3:first-child').trigger('click');

            });

            

            $("#tabs").tabs();

            $('#update').click(function () {

                $("#saveDialog").dialog({
                    buttons: {
                        'OK': {
                            text: 'OK',
                            class: 'btn',
                            click: function () {

                                var cname = $("#conceptMapName").val();
                                //var conceptmapid = 0;
                                //var serializedConceptmap = JSON.stringify(conceptmap);
                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "DragFlow.aspx/CreateConceptMap",
                                    data: '{ CName: "' + String(cname) + '", SystemName: "", SystemID: "" }',
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
                                            var connections = [];
                                            $.each(jsPlumb.getConnections(), function (idx, connection) {
                                                connections.push({
                                                    CID: data.d,
                                                    connectionId: connection.id,
                                                    pageSourceId: connection.sourceId,
                                                    pageTargetId: connection.targetId,
                                                    label: connection.overlays[1].getLabel()
                                                    //label: connection.getOverlay("label").getLabel()
                                                });
                                                console.log(connection.overlays[1].getLabel());
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
                                                    color: $elem.css('backgroundColor')
                                                });
                                                //var templateFieldsData = parseNodeTemplateJson(String($elem.attr('id')), String($elem.attr("template-id")), templateData);
                                                //console.log(String($elem.attr("template-id")));
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
                    }
                });

            });
        });

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
<div id="browsingopenDialog" title="開啟概念圖" style="display:none;">
  <p>學生概念圖:<asp:DropDownList ID="browsingStudentList" Width="420px" runat="server">
            <asp:ListItem>---</asp:ListItem>
        </asp:DropDownList></p>
  <p>標準答案概念圖:<asp:DropDownList ID="browsingTeacherList" Width="420px" runat="server">
            <asp:ListItem>---</asp:ListItem>
        </asp:DropDownList></p>
</div>
<div id="saveDialog" title="儲存概念圖" style="display:none;">
  <p>概念圖名稱：<input type="text" id="conceptMapName" value="" /></p>
</div>
        <input type="hidden" id="templateID" />
        <input type="hidden" id="linktemplateID" />
        <input type="hidden" id="linktemplateColor" />
        <input type="hidden" id="groupID" value="0" />
        <input type="hidden" id="userID" value="" />
        <input type="hidden" id="chairman" value="0" />
            <input type="hidden" id="match" value="0" />
            <input type="hidden" id="partialmatch" value="0" />
            <input type="hidden" id="mismatch" value="0" />
            <input type="hidden" id="additional" value="0" />
     
                <div id="pathaccordion" title="Match Table" style="padding:0;">
                      <div id="tabs">
  <ul>
    <li><a href="#tabs-1">Path Comparison Table</a></li>
    <li><a href="#tabs-2">Node Match Table</a></li>
   <!-- <li><a href="#tabs-3">Link Match Table</a></li>-->
  </ul>
  <div id="tabs-1">
     <div>
         <h5>Root-leaf match table</h5>
                        <table id="pathMatchTable" class="table table-bordered">
                            <tr>
                                <th>ID</th>
                                <th width="30%">(root,leaf)</th>
                                <th width="60%">Path Name</th>
                            </tr>
                        </table>
    </div>
    <!--<div>
         <h5>Root-leaf un-match table</h5>
                        <table id="pathUnmatchTable" class="table table-bordered">
                            <tr>
                                <th>ID</th>
                                <th width="30%">(root,leaf)</th>
                                <th width="60%">Path Name</th>
                            </tr>
                        </table>
    </div>-->
  </div>
  <div id="tabs-2">
    <div>
                        <table id="nodeMatchTable" class="table table-bordered">
                            <tr>
                                <th width="25%">Match</th>
                                <th width="25%">Partial Match</th>
                                <th width="25%">Mismatch</th>
                                <th width="25%">Additional</th>
                            </tr>
                            <tr>
                                <td><div id="mtable"></div></td>
                                <td><div id="pmtable"></div></td>
                                <td><div id="mmtable"></div></td>
                                <td><div id="addtable"></div></td>
                            </tr>
                        </table>
                    </div>
  </div>
    <div id="tabs-3" style="display:none;">
    <div>
                        <table id="linkMatchTable" class="table table-bordered">
                            <tr>
                                <th width="33%">Match</th>
                                <th width="33%">Mismatch</th>
                                <th width="33%">Additional</th>
                            </tr>
                            <tr>
                                <td>help to, answer, represent, includes, needed to, are is, neccessary for, may be, constructed, show, in, especially with, add to</td>
                                <td>is longer in, is higher in, is shorter, is lower</td>
                                <td>increase, falls, reduce</td>
                            </tr>
                        </table>
                    </div>
  </div>
</div>
                   
              
                  


                </div>
           
            <div style="position:fixed;left:0;bottom:0;">
                <div id="statpanel" title="概念圖統計表" style="padding:0;">
                  <div class="group">
                    <div>
                        <iframe src="chart.aspx?m=0&pm=0&mm=0&ad=0" name="chartframe" id="chartframe" scrolling="no" frameborder="no" align="center" height = "480px" width = "580px"></iframe>
                    </div>
                   
                  </div>
                  


                </div>
            </div>
    
    <div id="loadingIMG" style="display:none"><img src="images/load.gif" />資料處理中，請稍後。</div>
    <div id="colorpanel" title="顏色意義對照表">
            <p>
            <table class="colortable">
            <tr>
            <td>節點顏色意義對照</td>
            <td> <div class="colordiv green">Match</div>
            </td>
            <td> <div class="colordiv lightgreen">Partial Match</div>
            </td>
            <td> <div class="colordiv red">Mismatch</div>
            </td>
            <td> <div class="colordiv yellow">Additional</div>
            </td>
            </tr>

            <tr>
            <td>標籤顏色意義對照</td>
            <td> <div class="colordiv green">Match</div>
            </td>
            <td> <div class="colordiv lightgreen">Partial Match</div>
            </td>
            <td> <div class="colordiv red">Mismatch</div>
            </td>
            <td> <div class="colordiv lightblue">Head not match</div>
            </td>
            <td> <div class="colordiv purple">Tail not match</div>
            </td>
            <td> <div class="colordiv orange">Both not match</div>
            </td>
            </tr>
            </table>
            </p>
        </div>
    <div id="answerpanel" title="概念圖標準答案" style="display:none;">
        <div id="answeraccordion" style="padding:0;">
<div class="group" style="">
    <div style="padding-bottom:10px;">
        <h6>透明度</h6>
    </div>
    <div>
       <div id="transparent-vertical" style="height:150px"></div>
         
    </div>
  </div>

</div>
            
        </div>
    <div id="overlay"></div>

      
            </form>
</body>
</html>
