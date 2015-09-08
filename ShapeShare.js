/// <reference path="../../Scripts/jquery-1.6.4.js" /> 
/// <reference path="../../Scripts/jquery-ui-1.8.12.js" /> 
/// <reference path="../../Scripts/jQuery.tmpl.js" /> 
/// <reference path="../../Scripts/jquery.cookie.js" /> 
/// <reference path="../../Scripts/signalR.js" /> 

$(function () {
    'use strict';

    var nodeQueue = [];
    var nodePlayDupQueue = [];
    var nodeRefreshDupQueue = [];
    var linkQueue = [];
    var linkPlayDupQueue = [];
    var linkRefreshDupQueue = [];
    

    var shapeShare = $.connection.shapeShare;

    window.onload = function () {
        if ($('#chairman').val() == '0') {
            $('#btndiv').hide();
            $('#lock').hide();
            $('#unlock').hide();
        }
    };
    
    (function ($) {
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        $.getParameterByName = getParameterByName;
        
    })(jQuery);

    function getCurDateTime() {
        var d = new Date().toISOString().substr(0, 19).replace('T', ' ');
        //var curr_date = (d.getDate() < 10 ? '0' : '') + d.getDate();
        //var curr_month = (d.getMonth() < 10 ? '0' : '') + d.getMonth();
        //var curr_year = d.getFullYear();
        //var curr_hour = (d.getHours() < 10 ? '0' : '') + d.getHours();
        //var curr_minute = (d.getMinutes() < 10 ? '0' : '') + d.getMinutes();
        //var curr_second = (d.getSeconds() < 10 ? '0' : '') + d.getSeconds();
        //return curr_year + "-" + curr_month
        //+ "-" + curr_date + " " + curr_hour + ":" + curr_minute + ":" + curr_second;
        return d;
    }
    console.log(getCurDateTime());
    function changeShape(el) {
        //console.log(el); 
        shapeShare.server.moveShape($(el).attr('id'), el.offsetLeft, el.offsetTop || 0, el.clientWidth, el.clientHeight, $('#groupID').val());
        
        //alert(el.clientWidth); 
    }
    function resizeShape(el) {
        shapeShare.server.resizableShape($(el).attr('id'), el.clientWidth, el.clientHeight);
        //alert(el.clientWidth); 
    }
    function makeShape(shape) {
        /// <returns type="jQuery" /> 
        var el;
        switch (shape.Type) {
            case "picture":
                el = $("<div><img src='" + shape.Src + "' /></div>");
                break;
            case "circle":
                el = $("<div />");
                break;
            case "square":
                el = $("<div />");
            case "rectangle":
            default:
                el = $("<div>Node Label</div>");
                break;
        }
        el
            .css({
                width: shape.Width,
                height: shape.Height,
                left: shape.Location.X,
                top: shape.Location.Y
            })
            .addClass("shape")
            .addClass(shape.Type)
            .attr("id", "s-" + shape.ID)
            .data("shapeId", shape.ID)
        //.append("<div class='user-info' id='u-" + shape.ID + "'>" + (shapeShare.state.user.Name === shape.ChangedBy.Name ? "" : "changed by " + shape.ChangedBy.Name) + "</div>"); 
        .append("<div class='user-info' id='u-" + shape.ID + "'></div>");
        return el;
    }
    shapeShare.client.shapeAdded = function (shape) {
        makeShape(shape)
            .hide()
            .appendTo("#shapes")
            .draggable({
                containment: "parent",
                start: function () {
                    shapeShare.server.ShowEditStat($('#groupID').val(), "節點移動中...");
                },
                drag: function (event, ui) {
                    changeShape(this);
                },
                stop: function () {
                    shapeShare.server.HideEditStat($('#groupID').val());
                }
            })
            .resizable({
                handles: "all",
                containment: "parent",
                aspectRatio:
                    shape.Type === "picture" ||
                    shape.Type === "square" ||
                    shape.Type === "circle",
                minHeight: 50,
                minWidth: 50,
                autoHide: true,
                resize: function () {
                    changeShape(this);
                }
            })
            .fadeIn();
    };
    shapeShare.client.selectedTextCreated = function (nodeID, label) {
        
                
        $("#designpannel").append('<div id="' + nodeID + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:0px;left:150px;position:absolute;"><label>' + label + '</label><div class="ep"> </div></div>');
        var linkcolor = "";
        if ($('#linktemplateColor').val() != '') {
            linkcolor = $('#linktemplateColor').val();
        }
        else {
            linkcolor = "#808080";
        }
        DragFlow.makeSourceById(nodeID, linkcolor);

        // 设置元素为可拖拽 
        jsPlumb.draggable(nodeID, {
            containment: "parent",
            drag: function (event, ui) {
                //console.log(event); 
                changeShape(this);

            }
        });
        // 设置连接目标节点 
        DragFlow.makeTargetById(nodeID);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(nodeID, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };
    shapeShare.client.shapeCreated = function (newid) {
        

        $("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:0px;left:150px;position:absolute;"><label>Node Label</label><div class="ep"> </div></div>');
        var linkcolor = "";
        if ($('#linktemplateColor').val() != '') {
            linkcolor = $('#linktemplateColor').val();
        }
        else {
            linkcolor = "#808080";
        }
        DragFlow.makeSourceById(newid, linkcolor);

        // 设置元素为可拖拽 
        jsPlumb.draggable(newid, {
            containment: "parent",
            drag: function (event, ui) {
                //console.log(event); 
                changeShape(this);

            }
        });
        // 设置连接目标节点 
        DragFlow.makeTargetById(newid);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(newid, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };

    shapeShare.client.shapeCreated = function (newid, color) {
        

        $("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:0px;left:150px;position:absolute;background-color:' + color + '"><label>Node Label</label><div class="ep"> </div></div>');
        var linkcolor = "";
        if ($('#linktemplateColor').val() != '') {
            linkcolor = $('#linktemplateColor').val();
        }
        else {
            linkcolor = "#808080";
        }
        DragFlow.makeSourceById(newid, linkcolor);

        // 设置元素为可拖拽 
        jsPlumb.draggable(newid, {
            containment: "parent",
            drag: function () {
                changeShape(this);

            }
        });
        // 设置连接目标节点 
        DragFlow.makeTargetById(newid);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(newid, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };

    shapeShare.client.shapeCreated = function (newid, color, templateid) {
        //console.log(x + " " + y); 
        

        $("#designpannel").append('<div id="' + newid + '" template-id="' + templateid + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:0px;left:150px;position:absolute;background-color:' + color + '"><label>Node Label</label><div class="ep"> </div></div>');
        var linkcolor = "";
        if ($('#linktemplateColor').val() != '') {
            linkcolor = $('#linktemplateColor').val();
        }
        else {
            linkcolor = "#808080";
        }
        DragFlow.makeSourceById(newid, linkcolor);

        // 设置元素为可拖拽 
        jsPlumb.draggable(newid, {
            containment: "parent",
            drag: function () {
                changeShape(this);

            }
        });
        // 设置连接目标节点 
        DragFlow.makeTargetById(newid);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(newid, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };
    shapeShare.client.shapeCreated = function (newid, color, templateid, x, y) {
        console.log(x + " " + y); 
        

        $("#designpannel").append('<div id="' + newid + '" template-id="' + templateid + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:' + (y - 80) + 'px;left:' + (x - 80) + 'px;position:absolute;background-color:' + color + '"><label>Node Label</label><div class="ep"> </div></div>');
        var linkcolor = "";
        if ($('#linktemplateColor').val() != '') {
            linkcolor = $('#linktemplateColor').val();
        }
        else {
            linkcolor = "#808080";
        }
        DragFlow.makeSourceById(newid, linkcolor);

        // 设置元素为可拖拽 
        jsPlumb.draggable(newid, {
            containment: "parent",
            drag: function (event, ui) {
                changeShape(this);
            }
        });
        // 设置连接目标节点 
        DragFlow.makeTargetById(newid);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(newid, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };
    shapeShare.client.keywordShapeCreated = function (newid, label, top, left) {
        

        $("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:' + top + 'px;left:' + left + 'px;position:absolute;"><label>' + label + '</label><div class="ep"> </div></div>');
        //DragFlow.makeSourceById(newid);
        var linkcolor = "";
        if ($('#linktemplateColor').val() != '') {
            linkcolor = $('#linktemplateColor').val();
        }
        else {
            linkcolor = "#808080";
        }
        DragFlow.makeSourceById(newid, linkcolor);

        // 设置元素为可拖拽 
        jsPlumb.draggable(newid, {
            containment: "parent",
            drag: function () {
                changeShape(this);

            }
        });
        // 设置连接目标节点 
        DragFlow.makeTargetById(newid);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(newid, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };
    shapeShare.client.userlistSet = function (userid, connid) {
        var useridlist = new Array();
        var connidlist = new Array();
        
        useridlist = userid.split(",");
        connidlist = connid.split(",");
        $('#users').html('');
        for (var i = 0; i < useridlist.length; i++) {
            $('#users').append("<li id=\"" + connidlist[i] + "\"><input type=\"checkbox\" name=\"userlistradio\" value=\"" + connidlist[i] + "\" />&nbsp;<img src=\"images/avatar.png\"/> " + useridlist[i] + "<span id=\"stat_" + connidlist[i] + "\"></span></li>");
            //Do something
        }
        if($('#chairman').val()=="0")
            $('input:checkbox[name=userlistradio]').hide();
        //$('#users').append("<button id=\"muteEdit\" class=\"btn\">鎖定<button><button id=\"unlockMuteEdit\" class=\"btn\">解鎖<button></li>");
        //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); }); 

    };
   
        //$('#muteEdit').on("click", function () {
            
        //});
        //$('#unlockMuteEdit').on("click", function () {
        //    alert($('input:radio[name=userlistradio]:checked').val());
        //});
        //$('').click(function () {
            //console.debug(ShapeShare.server.muteEdit((this).attr('connid')));
        //    alert($('input:radio[name=userlistradio]:checked').val());
        //});
        ///$('#unlockMuteEdit').click(function (event) {
            //ShapeShare.server.unlockMuteEdit((this).attr('connid'));
        //    alert();
        //});

    shapeShare.client.editMuted = function (connid) {
        $('#student').show();
        $('#alertlock').fadeIn();
        
    };
    shapeShare.client.editMutedUnlock = function (connid) {
        $('#student').hide();
        $('#alertlock').hide();
        $('#alertunlock').fadeIn().delay(2000).fadeOut();
       
    };
    shapeShare.client.userStatShowed = function (id, msg, effect) {
        //console.log(msg);
        $('#stat_' + id).html(msg);
    };
    shapeShare.client.panelEmpty = function () {
        $("#designpannel").empty();
        //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); }); 

    };
    shapeShare.client.addNodeItems = function (templateid, color) {
        //$("#designpannel").empty(); 
        $('.well ul').find(' > li:nth-last-child(1)').before('<li></li><li><div id="Div' + String(templateid).trim() + '" class="toolbar_padding toolbar_blue" style="width: 75px; background-color:#' + String(color).trim() + '"></div></li>');
        $("#Div" + String(templateid)).draggable({ opacity: 0.7, helper: "clone", drag: function (event, ui) { } });
        //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); }); 

    };
    shapeShare.client.addNodeItems = function (templateid, color, title) {
        //$("#designpannel").empty(); 
        $('.well ul').find(' > li:nth-last-child(1)').before('<li style=\"height: 10px;\"></li><li style=\"width:100px;margin-left:-10px;\">' + String(title) + '</li><li><div id="Div' + String(templateid).trim() + '" class="toolbar_padding toolbar_blue" style="width: 75px; background-color:#' + String(color).trim() + '"></div></li>');
        $("#Div" + String(templateid)).draggable({ opacity: 0.7, helper: "clone", drag: function (event, ui) { } });
        //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); }); 

    };
    shapeShare.client.setTemplateData = function (templateid, templatedata) {
        localStorage.setItem(templateid, templatedata);
    };

    shapeShare.client.addLinkItems = function (templateid, color, title) {
        //$("#designpannel").empty(); 
        $('.well ul.link').find(' > li:nth-last-child(1)').before('<li style=\"height: 10px;\"></li><li style=\"\"><div style=\"width:120px;margin-left:-20px;\">' + String(title) + '</div></li><li><div id="Div' + String(templateid).trim() + '" class="divtooltip" style="width: 75px; "><button type=\"button\" class=\"btn btn-default btn-lg linkButton\" data-toggle=\"button\" style=\"padding-bottom: 15px;font-size:40px;color:#' + String(color).trim() + '\">&rarr;</button></div></li>');

        $('.linkButton').on("click", function () {
            $('.linkButton').removeClass('active');
        });
        //removeOtherButtons(); 
        //$("#Div" + String(templateid)).draggable({ helper: "clone", drag: function (event, ui) { } }); 
        //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); }); 

    };

    shapeShare.client.emptyNodeItems = function () {
        //$('.well ul').empty(); 
        //$('.well ul').append('<li class="nav-header" style="color:Black;">Toolbox</li>'); 
        //$('.well ul').append('<li><div id="toolbar_blue" class="toolbar_padding toolbar_blue ui-draggable" style="width: 75px;"></div></li>'); 
        //$('.well ul').append('<li><a href="#" id="update"><img src="images/save-icon.jpg" width="100"></a></li>'); 
        $('.well ul li:not(:first):not(:last)').remove();
    };
    shapeShare.client.setSChartVal = function (m, pm, ad) {
        //$('.well ul').empty(); 
        //$('.well ul').append('<li class="nav-header" style="color:Black;">Toolbox</li>'); 
        //$('.well ul').append('<li><div id="toolbar_blue" class="toolbar_padding toolbar_blue ui-draggable" style="width: 75px;"></div></li>'); 
        //$('.well ul').append('<li><a href="#" id="update"><img src="images/save-icon.jpg" width="100"></a></li>'); 
        //$('#chartframe').attr('src', 'chart.aspx?m=' + m + '&pm=' + pm + '&mm=' + mm + '&ad=' + ad);
        $('#match').val(m);
        $('#partialmatch').val(pm);
        $('#additional').val(ad);
    };

    shapeShare.client.setTChartVal = function (mm) {
        //$('.well ul').empty(); 
        //$('.well ul').append('<li class="nav-header" style="color:Black;">Toolbox</li>'); 
        //$('.well ul').append('<li><div id="toolbar_blue" class="toolbar_padding toolbar_blue ui-draggable" style="width: 75px;"></div></li>'); 
        //$('.well ul').append('<li><a href="#" id="update"><img src="images/save-icon.jpg" width="100"></a></li>'); 
        //$('#chartframe').attr('src', 'chart.aspx?m=' + m + '&pm=' + pm + '&mm=' + mm + '&ad=' + ad);
        $('#mismatch').val(mm);
    };
    shapeShare.client.browsingChartLoaded = function () {
        //$('.well ul').empty(); 
        //$('.well ul').append('<li class="nav-header" style="color:Black;">Toolbox</li>'); 
        //$('.well ul').append('<li><div id="toolbar_blue" class="toolbar_padding toolbar_blue ui-draggable" style="width: 75px;"></div></li>'); 
        //$('.well ul').append('<li><a href="#" id="update"><img src="images/save-icon.jpg" width="100"></a></li>'); 
        $('#chartframe').attr('src', 'chart.aspx?m=' + $('#match').val() + '&pm=' + $('#partialmatch').val() + '&mm=' + $('#mismatch').val() + '&ad=' + $('#additional').val());
    };
    //shapeShare.client.browsingChartLoaded = function (m, pm, mm, ad) {
    //    //$('.well ul').empty(); 
    //    //$('.well ul').append('<li class="nav-header" style="color:Black;">Toolbox</li>'); 
    //    //$('.well ul').append('<li><div id="toolbar_blue" class="toolbar_padding toolbar_blue ui-draggable" style="width: 75px;"></div></li>'); 
    //    //$('.well ul').append('<li><a href="#" id="update"><img src="images/save-icon.jpg" width="100"></a></li>'); 
    //    $('#chartframe').attr('src', 'chart.aspx?m=' + m + '&pm=' + pm + '&mm=' + mm + '&ad=' + ad);
    //};
    //shapeShare.client.mask = function () { 
    //    $("body").mask("Loading..."); 
    //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); }); 

    //}; 
    //shapeShare.client.unmask = function () { 
    //    $("body").unmask(); 
    //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); }); 

    //}; 
    shapeShare.client.shapeLoaded = function (nodeID, nodeLabel, xCoordinate, yCoordinate, width, height) {
        

        $("#designpannel").append('<div id="' + nodeID + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:' + yCoordinate + 'px;left:' + xCoordinate + 'px;width:' + width + 'px;height:' + height + 'px;position:absolute;"><label>' + nodeLabel + '</label><div class="ep"> </div></div>');
        DragFlow.makeSourceById(nodeID);

        // 设置元素为可拖拽 
        jsPlumb.draggable(nodeID, {
            containment: "parent",
            drag: function () {
                changeShape(this);

            }
        });
        // 设置连接目标节点 shaper
        DragFlow.makeTargetById(nodeID);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(nodeID, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };

    shapeShare.client.shapeLoaded = function (nodeID, nodeLabel, templateId, color, xCoordinate, yCoordinate, width, height) {
        

        $("#designpannel").append('<div id="' + nodeID + '" template-id="' + templateId + '" class="gradient_blue component window" ctime="' + getCurDateTime() + '" style="top:' + yCoordinate + 'px;left:' + xCoordinate + 'px;width:' + width + 'px;height:' + height + 'px;position:absolute;background-color:' + color + '"><label>' + nodeLabel + '</label><div class="ep"> </div></div>');
        //var linkcolor = ""; 
        //if ($("#linktemplateColor").val()!='') 
        //    DragFlow.makeSourceById(nodeID, $("#linktemplateColor").val()); 
        //else 
        DragFlow.makeSourceById(nodeID, "#808080");
        // 设置元素为可拖拽 
        jsPlumb.draggable(nodeID, {
            containment: "parent",
            drag: function () {
                changeShape(this);

            }
        });
        // 设置连接目标节点 
        DragFlow.makeTargetById(nodeID);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(nodeID, false);
        // 设置元素为连接源节点 
        $(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });
    };
    shapeShare.client.browsingShapeLoaded = function (nodeID, nodeLabel, templateId, color, xCoordinate, yCoordinate, width, height, panel, createdatetime) {


        $("#" + panel).append('<div id="' + nodeID + '" template-id="' + templateId + '" class="gradient_blue component window" ctime="' + createdatetime + '" style="top:' + yCoordinate + 'px;left:' + xCoordinate + 'px;width:' + width + 'px;height:' + height + 'px;position:absolute;background-color:' + color + '"><label>' + nodeLabel + '</label><div class="ep"> </div></div>');
        //var linkcolor = ""; 
        //if ($("#linktemplateColor").val()!='') 
        //    DragFlow.makeSourceById(nodeID, $("#linktemplateColor").val()); 
        //else 
        DragFlow.makeSourceById(nodeID, "#808080");
        // 设置元素为可拖拽 
        
        /*jsPlumb.draggable(nodeID, {
            containment: "parent",
            drag: function () {
                changeShape(this);

            }
        });*/

        // 设置连接目标节点 
        DragFlow.makeTargetById(nodeID);
        // 设置新元素的右键菜单 
        DragFlow.makecontextmenu(nodeID, false);
        // 设置元素为连接源节点 

        /*$(".component").resizable({
            aspectRatio: false,
            resize: function () {

                resizeShape(this);
                jsPlumb.repaintEverything();
            }
        });*/
        $('#' + nodeID + ' .ep').hide();

        if (panel == 'designpannel') {
            nodeQueue.push({
                nID: nodeID,
                color: color,
                datetime: createdatetime
            });
        }
    };
    shapeShare.client.nodeLinked = function () {

    };

    shapeShare.client.shapeChanged = function (id, shape) {
        //console.log(shape.ID); 
        $("#" + id).css({
            top: shape.Location.Y,
            left: shape.Location.X,
            width: shape.Width,
            height: shape.Height
        });

        //$("#u-" + shape.ID) 
        //  .text("changed by " + shape.ChangedBy.Name); 
    };
    shapeShare.client.shapeMoved = function (id, x, y, w, h) {
        //console.log(shape.ID); 
        //console.log(x+" "+y); 
        $("#" + id).css({
            top: y,
            left: x,
            //width: w, 
            //height: h 
        });
        
        //jsPlumb.draggable(jsPlumb.getSelector(".component")); 
        jsPlumb.repaintEverything();
        //shapeShare.server.showUserStat("", $('#groupID').val(), " 編輯中<img src=\"images/action.gif\" />", "fade");
        //$("#u-" + shape.ID) 
        //  .text("changed by " + shape.ChangedBy.Name); 
    };
    shapeShare.client.shapeResized = function (id, w, h) {
        //console.log(shape.ID); 
        $("#" + id).css({
            //top: y, 
            //left: x, 
            width: w,
            height: h
        });
        jsPlumb.repaintEverything();
        //jsPlumb.draggable(jsPlumb.getSelector(".component")); 
        //$("#u-" + shape.ID) 
        //  .text("changed by " + shape.ChangedBy.Name); 
    };
    shapeShare.client.shapeDeleted = function (id) {
        $("#" + id).fadeOut(250, function () {
            $("#" + id).remove();
        });
        jsPlumb.repaintEverything();
    };

    //shapeShare.client.shapesDeleted = function (shapes) { 
    //    $.each(shapes, function () { 
    //        shapeShare.client.shapeDeleted(this.id); 
    //    }); 
    //}; 
    //shapeShare.client.editEP = function (id, value) { 
    //    //$("#" + id).fadeOut(250, function () { 
    //    //  $("#" + id).remove(); 
    //    //$("#" + id).html(value); 
    //    $.each(jsPlumb.getConnections(), function (idx, connection) { 
    //        //label.getOverlay("label").setLabel($("#itemLabel").val()); 
    //        //$("#" + $elem.attr('id')).find('label') 
    //        //console.log(tid) 
    //        if (id === connection.overlays[1].canvas.id) { 
    //            console.log(tid + " " + connection.overlays[1].canvas.id) 
    //            connection.overlays[1].setLabel(value); 
    //        } 
    //        //console.log(connection.overlays[1].canvas.id); 
    //    }); 
    //    jsPlumb.repaintEverything(); 
    //    //}); 
    //}; 
    shapeShare.client.linkLoaded = function (linkID, linkLabel, linkColor, startNode, endNode) {
        //alert(linkLabel); 
        jsPlumb.makeSource($("#" + startNode + "").children(".ep"),
                           {
                               parent: startNode,
                               anchor: "Continuous",
                               connector: ["Straight", { curviness: 20 }],
                               //connectorStyle: { strokeStyle: linkColor, lineWidth: 2 }, 
                               connectorStyle: { strokeStyle: linkColor, lineWidth: 2 },
                               paintStyle: { fillStyle: linkColor, outlineColor: linkColor, outlineWidth: 2 },
                               endpoint: "Blank",
                               maxConnections: 100,
                               cssClass: "aLabel",
                               onMaxConnections: function (info, e) {
                                   alert("Maximum connections (" + (info.maxConnections) + ") reached");
                               }
                           });

        jsPlumb.unbind("jsPlumbConnection");
        //jsPlumb.bind("ready", function () {
            var conn = jsPlumb.connect({
                source: startNode,
                target: endNode,
                paintStyle: { fillStyle: "#808080", outlineColor: linkColor, outlineWidth: 2 },
                //connector: ["Straight", { curviness: 20 }], 
                connectorStyle: { strokeStyle: "#808080", lineWidth: 2 },
                overlays: [["PlainArrow", {
                    location: 1,
                    id: "arrow",
                    width: 10,
                    length: 12
                }], ["Label", {
                    label: linkLabel,
                    id: "label",
                    cssClass: "aLabel"
                }]
                ]
            });
        //});
        //jsPlumb.bind("jsPlumbConnection", function (conn) { 
        //    shapeShare.server.connectNode(startNode, endNode, $.connection.hub.id); 

        //}); 

        //var connid = new Array(); 
        //$("._jsPlumb_overlay").each(function (i, obj) { 
        //    connid.push($(obj).attr("id")); 
        //}); 
        //shapeShare.server.changeConnID(connid, $.connection.hub.id); 
      
        //bindconnection(); 
    };

    shapeShare.client.linkLoaded = function (linkID, linkLabel, linkColor, templateID, startNode, endNode) {
        //alert(linkLabel); 

        // jsPlumb.makeSource($("#" + startNode + "").children(".ep"),
       //                    {
       //                        parent: startNode,
       //                        anchor: "Continuous",
       //                        connector: ["Straight", { curviness: 20 }],
       //                        //connectorStyle: { strokeStyle: linkColor, lineWidth: 2 }, 
       //                        connectorStyle: { strokeStyle: linkColor, lineWidth: 2 },
       //                        paintStyle: { fillStyle: linkColor, outlineColor: linkColor, outlineWidth: 2 },
       //                        endpoint: "Blank",
       //                        maxConnections: 100,
       //                        cssClass: "aLabel",
       //                        onMaxConnections: function (info, e) {
       //                            alert("Maximum connections (" + (info.maxConnections) + ") reached");
       //                        }
       //                    });

       // jsPlumb.unbind("jsPlumbConnection");
       //// jsPlumb.bind("ready", function () {
       //     var conn = jsPlumb.connect({
       //         source: startNode,
       //         target: endNode,
       //         paintStyle: { fillStyle: "#808080", outlineColor: linkColor, outlineWidth: 2 },
       //         //connector: ["Straight", { curviness: 20 }], 
       //         connectorStyle: { strokeStyle: "#808080", lineWidth: 2 },
       //         overlays: [
       //     ["Label", { label: linkLabel, id: "label", cssClass: "aLabel", }]
       //         ]
       //     });
       //     conn.setParameter("templateid", templateID);

        jsPlumb.makeSource($("#" + startNode + "").children(".ep"),
                           {
                               parent: startNode,
                               anchor: "Continuous",
                               connector: ["Straight", { curviness: 20 }],
                               connectorStyle: { strokeStyle: linkColor, lineWidth: 2 },
                               endpoint: "Blank",
                               maxConnections: 100,
                               cssClass: "aLabel",
                               onMaxConnections: function (info, e) {
                                   alert("Maximum connections (" + (info.maxConnections) + ") reached");
                               }
                           });
        jsPlumb.unbind("jsPlumbConnection");
        var conn = jsPlumb.connect({
            source: startNode,
            target: endNode,
            overlays: [["PlainArrow", {
                location: 1,
                id: "arrow",
                width: 10,
                length: 12
            }], ["Label", {
                label: linkLabel,
                id: "label",
                cssClass: "aLabel"
            }]
            ]
        });
        conn.setParameter("linkid", linkID);
        conn.setParameter("templateid", templateID);

        var datetime = getCurDateTime();
        conn.setParameter("createdatetime", datetime);

        //});
        jsPlumb.bind("jsPlumbConnection", function (conn) { 
            //shapeShare.server.connectNode(startNode, endNode, $.connection.hub.id); 
            var color = "";
            if ($('#linktemplateColor').val() != '') {
                color = $('#linktemplateColor').val();
            }
            else {
                color = "#808080";
            }
            var templateid = "";
            if ($('#linktemplateID').val() != '') {
                templateid = $('#linktemplateID').val();
            }
            else {
                templateid = "0";
            }
            //conn. 
            //jsPlumb.registerConnectionTypes({ 
            //    "foo": { connectorStyle: { strokeStyle: color, lineWidth: 2 } } 
            //}); 
            //conn.connection.setType("foo"); 
            //console.log(e); 
            //conn.getConnections().strokeStyle.color = color; 
            //e.preventDefault(); 
            //conn.connection.getOverlay("label").setLabel("label"); 
            //alert(jsPlumb._getId(conn.connection.getOverlay("label"))); 
            //jsPlumb.unbind("jsPlumbConnection"); 
            //console.log(conn.connection.id); 
            //$('input[id^="linktemplateColor"]').each(function (input) { 
            //    var value = $('input[id^="linktemplateColor"]').val(); 
            //    var id = $('input[id^="linktemplateColor"]').attr('id'); 
            //    alert('id: ' + id + ' value:' + value); 
            //}); 
            //conn.endpoints.setPaintStyle({ fillStyle: color }); 
            //console.log(conn.connection); 
            //conn.connection.setPaintStyle({ fillStyle: color }); 
            //console.log($("input[id*='linktemplateColor']").val()); 
            //conn.connection.addClass("123"); 
            //conn.connection.setLabel("label");
            //conn.connection.addOverlay(["Label", {label: "label",id: "label",cssClass: "aLabel"}]);
            conn.connection.setParameter("templateid", templateid);
            var newid = $.fn.NewGUID();
            conn.connection.setParameter("linkid", newid);
            var datetime = getCurDateTime();
            conn.connection.setParameter("createdatetime", datetime);
            //conn.connection.setPaintStyle({ strokeStyle: color, lineWidth: 2 });
            //jsPlumb.select({ source: conn.sourceId, target: conn.targetId }).getPaintStyle().strokeStyle = color; 
            //conn.connection.repaint(); 
            /*jsPlumb.makeSource($("#" + conn.sourceId + "").children(".ep"), 
                              { 
                                  parent: conn.sourceId, 
                                  anchor: "Continuous", 
                                  connector: ["Flowchart", { curviness: 20 }], 
                                  connectorStyle: { strokeStyle: color, lineWidth: 2 }, 
                                  endpoint: "Blank", 
                                  maxConnections: 5, 
                                  cssClass: "aLabel", 
                                  onMaxConnections: function (info, e) { 
                                      alert("Maximum connections (" + (info.maxConnections) + ") reached"); 
                                  } 
                              });*/
            shapeShare.server.connectNode(conn.sourceId, conn.targetId, $.connection.hub.id, color, "", newid, datetime, $('#groupID').val());

        }); 

        //var connid = new Array(); 
        //$("._jsPlumb_overlay").each(function (i, obj) { 
        //    connid.push($(obj).attr("id")); 
        //}); 
        //shapeShare.server.changeConnID(connid, $.connection.hub.id); 
      
        //bindconnection(); 
    };
    shapeShare.client.browsingLinkLoaded = function (linkID, linkLabel, linkColor, templateID, startNode, endNode, panel, createdatetime) {
        //alert(linkLabel); 
        //console.log(linkID);
        // jsPlumb.makeSource($("#" + startNode + "").children(".ep"),
        //                    {
        //                        parent: startNode,
        //                        anchor: "Continuous",
        //                        connector: ["Straight", { curviness: 20 }],
        //                        //connectorStyle: { strokeStyle: linkColor, lineWidth: 2 }, 
        //                        connectorStyle: { strokeStyle: linkColor, lineWidth: 2 },
        //                        paintStyle: { fillStyle: linkColor, outlineColor: linkColor, outlineWidth: 2 },
        //                        endpoint: "Blank",
        //                        maxConnections: 100,
        //                        cssClass: "aLabel",
        //                        onMaxConnections: function (info, e) {
        //                            alert("Maximum connections (" + (info.maxConnections) + ") reached");
        //                        }
        //                    });

        // jsPlumb.unbind("jsPlumbConnection");
        //// jsPlumb.bind("ready", function () {
        //     var conn = jsPlumb.connect({
        //         source: startNode,
        //         target: endNode,
        //         paintStyle: { fillStyle: "#808080", outlineColor: linkColor, outlineWidth: 2 },
        //         //connector: ["Straight", { curviness: 20 }], 
        //         connectorStyle: { strokeStyle: "#808080", lineWidth: 2 },
        //         overlays: [
        //     ["Label", { label: linkLabel, id: "label", cssClass: "aLabel", }]
        //         ]
        //     });
        //     conn.setParameter("templateid", templateID);

        jsPlumb.makeSource($("#" + startNode + "").children(".ep"),
                           {
                               parent: startNode,
                               anchor: "Continuous",
                               connector: ["Straight", { curviness: 20 }],
                               connectorStyle: { strokeStyle: linkColor, lineWidth: 2 },
                               endpoint: "Blank",
                               maxConnections: 100,
                               cssClass: "aLabel",
                               onMaxConnections: function (info, e) {
                                   alert("Maximum connections (" + (info.maxConnections) + ") reached");
                               }
                           });
        jsPlumb.unbind("jsPlumbConnection");
        var conn = jsPlumb.connect({
            source: startNode,
            target: endNode,
            overlays: [["PlainArrow", {
                            location: 1,
                            id: "arrow",
                            width: 10,
                            length: 12
                       }], ["Label", {
                           label: linkLabel,
                           id: "label",
                           cssClass: "aLabel"
                        }]
            ]
        });
        conn.setParameter("linkid", linkID);
        conn.setParameter("templateid", templateID);
        if (panel == 'designpannel') {
            linkQueue.push({
                lID: linkID,
                color: linkColor,
                datetime: createdatetime
            });
        }
        //});
        //jsPlumb.bind("jsPlumbConnection", function (conn) { 
        //    shapeShare.server.connectNode(startNode, endNode, $.connection.hub.id); 

        //}); 

        //var connid = new Array(); 
        //$("._jsPlumb_overlay").each(function (i, obj) { 
        //    connid.push($(obj).attr("id")); 
        //}); 
        //shapeShare.server.changeConnID(connid, $.connection.hub.id); 

        //bindconnection(); 
    };
    shapeShare.client.nodeConnected = function (sourceid, targetid, clientid, linkcolor, linkLabel) {
        //alert(linklabel);
        
        jsPlumb.makeSource($("#" + sourceid + "").children(".ep"),
                           {
                               parent: sourceid,
                               anchor: "Continuous",
                               connector: ["Straight", { curviness: 20 }],
                               connectorStyle: { strokeStyle: linkcolor, lineWidth: 2 },
                               endpoint: "Blank",
                               maxConnections: 100,
                               cssClass: "aLabel",
                               onMaxConnections: function (info, e) {
                                   alert("Maximum connections (" + (info.maxConnections) + ") reached");
                               }
                           });
        jsPlumb.unbind("jsPlumbConnection");
        var conn = jsPlumb.connect({
            source: sourceid,
            target: targetid,
            overlays: [["PlainArrow", {
                location: 1,
                id: "arrow",
                width: 10,
                length: 12
            }], ["Label", {
                label: linkLabel,
                id: "label",
                cssClass: "aLabel"
            }]
            ]
        });
        var color = "";
        var templateid = "";

        if ($('#linktemplateColor').val() != '') {
            color = $('#linktemplateColor').val();
        }
        else {
            color = "#808080";
        }

        if ($('#linktemplateID').val() != '') {
            templateid = $('#linktemplateID').val();
        }
        else {
            templateid = "0";
        }
        conn.setParameter("templateid", templateid);

        var newid = $.fn.NewGUID();
        conn.setParameter("linkid", newid);
        conn.setParameter("createdatetime", getCurDateTime());
        console.log(conn.getParameter("linkid"));
        console.log(conn.getParameter("createdatetime"));
    };
    

    shapeShare.client.nodeConnected = function (sourceid, targetid, clientid) {
        jsPlumb.unbind("jsPlumbConnection");
        var conn = jsPlumb.connect({
            source: sourceid,
            target: targetid
        });
        var connid = new Array();
        $("._jsPlumb_overlay").each(function (i, obj) {

            connid.push($(obj).attr("id"));
        });

        shapeShare.server.changeConnID(connid, clientid, $('groupID').val());
        //jsPlumb.bind("jsPlumbConnection", function (conn) { 
        //    shapeShare.server.connectNode(sourceid, targetid, clientid); 
        //}); 
    };
    shapeShare.client.nodeConnected = function (sourceid, targetid, clientid, linkcolor) {
        //alert("");
        jsPlumb.makeSource($("#" + sourceid + "").children(".ep"),
                          {
                              parent: sourceid,
                              anchor: "Continuous",
                              connector: ["Straight", { curviness: 20 }],
                              connectorStyle: { strokeStyle: linkcolor, lineWidth: 2 },
                              endpoint: "Blank",
                              maxConnections: 100,
                              cssClass: "aLabel",
                              onMaxConnections: function (info, e) {
                                  alert("Maximum connections (" + (info.maxConnections) + ") reached");
                              }
                          });
        jsPlumb.unbind("jsPlumbConnection");
        
        var color = "";
        var templateid = "";
        //線段顏色
        if ($('#linktemplateColor').val() != '') {
            color = $('#linktemplateColor').val();
        }
        else {
            color = "#808080";
        }

        //templateID
        if ($('#linktemplateID').val() != '') {
            templateid = $('#linktemplateID').val();
        }
        else {
            templateid = "0";
        }

        //var newid = $.fn.NewGUID();
        //conn.setParameter("templateid", templateid);
        var conn = jsPlumb.connect({
            source: sourceid,
            target: targetid,
            overlays: [["PlainArrow", {
                    location: 1,
                    id: "arrow",
                    width: 10,
                    length: 12
                }], ["Label", {
                    label: linkLabel,
                    id: "label",
                    cssClass: "aLabel"
                }]
            ],
            parameters: {
                "templateid": templateid,
                "linkid": newid,
                "createdatetime": createdatetime
            }
        });
        //console.log(sourceid); 
        //console.log(conn); 
        // jsPlumb.bind("jsPlumbConnection", function (conn) { 
        //conn.connection.setPaintStyle({ strokeStyle: nextColour() }); 
        //conn.connection.getOverlay("label").setLabel("label"); 
        //alert(jsPlumb._getId(conn.connection.getOverlay("label"))); 
        //shapeShare.server.connectNode(conn.sourceId, conn.targetId, $.connection.hub.id); 
        //shapeShare.server.connectNode(conn.sourceId, conn.targetId); 
        //alert(conn.sourceId+" "+conn.targetId); 
        //}); 
        var connid = new Array();
        $("._jsPlumb_overlay").each(function (i, obj) {
            //alert($(obj).attr("id")); 
            connid.push($(obj).attr("id"));
        });

        //conn.connection.setParameter("templateid", templateid); 
        shapeShare.server.changeConnID(connid, clientid, $('#groupID').val());
        //jsPlumb.bind("jsPlumbConnection", function (conn) { 
        //    shapeShare.server.connectNode(sourceid, targetid, clientid, color); 
        //bindconnection(); 
        //}); 
        //conn.setParameter("idname","123"); 
        //$("#jsPlumb_1_11").attr("id","jsPlumb_1_17"); 
        //var els = $(".aLabel"); 
        //alert(els.id); 
        //}); 


        //2014/07/18
        //jsPlumb.bind("jsPlumbConnection", function (conn, e) {

        //    var color = "";
        //    var templateid = "";

        //    if ($('#linktemplateColor').val() != '') {
        //        color = $('#linktemplateColor').val();
        //    }
        //    else {
        //        color = "#808080";
        //    }

        //    if ($('#linktemplateID').val() != '') {
        //        templateid = $('#linktemplateID').val();
        //    }
        //    else {
        //        templateid = "0";
        //    }

        //    conn.connection.setParameter("templateid", templateid);

        //    var newid = $.fn.NewGUID();
        //    conn.setParameter("linkid", newid);
        //    conn.setParameter("createdatetime", getCurDateTime());
        //    console.log(conn.getParameter("linkid"));
        //    console.log(conn.getParameter("createdatetime"));

        //    //console.log(conn.connection);
        //    conn.connection.setPaintStyle({ strokeStyle: color, lineWidth: 2, });

        //    shapeShare.server.connectNode(conn.sourceId, conn.targetId, $.connection.hub.id, color, "", $('#groupID').val());

        //});
        //2014/07/18


        //console.log(conn.getParameter("templateid")); 
    };
    shapeShare.client.nodeConnected = function (sourceid, targetid, clientid, linkcolor, newid, createdatetime) {
        //alert("");
        jsPlumb.makeSource($("#" + sourceid + "").children(".ep"),
                          {
                              parent: sourceid,
                              anchor: "Continuous",
                              connector: ["Straight", { curviness: 20 }],
                              connectorStyle: { strokeStyle: linkcolor, lineWidth: 2 },
                              endpoint: "Blank",
                              maxConnections: 100,
                              cssClass: "aLabel",
                              onMaxConnections: function (info, e) {
                                  alert("Maximum connections (" + (info.maxConnections) + ") reached");
                              }
                          });
        jsPlumb.unbind("jsPlumbConnection");

        var color = "";
        var templateid = "";
        //線段顏色
        if ($('#linktemplateColor').val() != '') {
            color = $('#linktemplateColor').val();
        }
        else {
            color = "#808080";
        }

        //templateID
        if ($('#linktemplateID').val() != '') {
            templateid = $('#linktemplateID').val();
        }
        else {
            templateid = "0";
        }

        var conn = jsPlumb.connect({
            source: sourceid,
            target: targetid,
            paintStyle: { fillStyle: "#808080", outlineColor: linkcolor, outlineWidth: 2 },
            //connector: ["Straight", { curviness: 20 }], 
            connectorStyle: { strokeStyle: "#808080", lineWidth: 2 },
            overlays: [["PlainArrow", {
                location: 1,
                id: "arrow",
                width: 10,
                length: 12
            }], ["Label", {
                label: "label",
                id: "label",
                cssClass: "aLabel"
            }]
            ],
            parameters: {
                "templateid": templateid,
                "linkid": newid,
                "createdatetime": createdatetime
            }
        });
     
        var connid = new Array();
        $("._jsPlumb_overlay").each(function (i, obj) {
            connid.push($(obj).attr("id"));
        });

        shapeShare.server.changeConnID(connid, clientid, $('#groupID').val());
       
    };
    shapeShare.client.connIDChanged = function (connid) {
        //$("#" + id).fadeOut(250, function () { 
        //  $("#" + id).remove(); 
        //$("#" + id).html(value); 

        var i = 0;
        $("._jsPlumb_overlay").each(function (i, obj) {
            $(obj).attr("id", connid[i++]);
            //shapeShare.server.changeConnID($(obj).attr("id"), clientid); 
        });

        //console.log(connid); 
        //}); 
    };

    //node編輯 
    shapeShare.client.nodeEdit = function (id, value) {
        //$("#" + id).fadeOut(250, function () { 
        //  $("#" + id).remove(); 
        //$("#" + id).html(value); 
        var lbl = $("#" + id).find('label');
        lbl.text(value);
        //$("#" + id).html('<label>' + value + '</label><div class="ep"></div>'); 
        $("#itemLabel").val("");
        DragFlow.initEndpoints("");

        //}); 
    };

    //node編輯 
    shapeShare.client.linkTemplateIDcolorSet = function (templateid, color) {
        //console.log(templateid, color); 
        $('#linktemplateColor').val(color);
        $('#linktemplateID').val(templateid);

        jsPlumb.bind("jsPlumbConnection", function (conn, e) {

            var color = "";
            if ($('#linktemplateColor').val() != '') {
                color = $('#linktemplateColor').val();
            }
            else {
                color = "#808080";
            }
            var templateid = "";
            if ($('#linktemplateID').val() != '') {
                templateid = $('#linktemplateID').val();
            }
            else {
                templateid = "0";
            }
            conn.connection.addOverlay(["Label", { label: "label", id: "label", cssClass: "aLabel" }]);
            conn.connection.setParameter("templateid", templateid);
            var newid = $.fn.NewGUID();
            conn.connection.setParameter("linkid", newid);
            var datetime = getCurDateTime();
            conn.connection.setParameter("createdatetime", datetime);
            conn.connection.setPaintStyle({ strokeStyle: color, lineWidth: 2 });

        });
    };
    //link編輯 
    shapeShare.client.epEdit = function (id, value) {
        //$("#" + id).fadeOut(250, function () { 
        //  $("#" + id).remove(); 
        //$("#" + id).html(value); 

        //$("#" + id).html(value); 
        //component.getOverlay("label").setLabel(value); 
        //$("#linkLabel").val(""); 
        //DragFlow.initEndpoints(""); 
        $.each(jsPlumb.getConnections(), function (idx, connection) {
            //label.getOverlay("label").setLabel($("#itemLabel").val()); 
            //$("#" + $elem.attr('id')).find('label') 
            //console.log(id + connection.overlays[1].canvas.id); 
            //if (id === connection.overlays[1].canvas.id) {
            if (id === connection.getParameter("linkid")) {
                //console.log(id + " " + connection.overlays[1].canvas.id) 

                connection.removeOverlay("label");
                connection.addOverlay(["Label", {
                    label: value,
                    id: "label",
                    cssClass: "aLabel"
                }]
                );

                //connection.overlays[1].setLabel("")
                //connection.setLabel(value);
                //alert(connection.getParameter("templateid")); 
            }
            //console.log(connection.overlays[1].canvas.id);813 
        });
        jsPlumb.repaintEverything();
        //}); 
    };

    $('#send').click(function () {
        shapeShare.server.joinGroup($('#group').val());
        alert("Joined group " + $('#group').val() + " !");

    });
    $('#copytext').click(function () {
        var selectedText = copySelection();
        if (selectedText != '') {
            var newid = $.fn.NewGUID();
            shapeShare.server.createSelectedText(newid, selectedText, $("#groupID").val());
        }
        else
            alert('請反白對話內容');
    });
    $('#toolbar')
        .delegate("menu[label=add] a", "click", function (e) {
            e.preventDefault();

            shapeShare.server.createShape(this.className.toLowerCase(), $('#group').val());

            //            switch (this.className.toLowerCase()) { 
            //                case "square": 
            //                    shapeShare.createShape("square"); 
            //                    break; 
            //                case "picture": 
            //                    shapeShare.createShape("picture"); 
            //                    break; 
            //            } 

            return false;
        })
        .delegate("a.delete", "click", function (e) {
            e.preventDefault();
            shapeShare.server.deleteAllShapes();
            return false;
        });
    $(document).keypress(function (e) {
        if (e.which == 100) {
            shapeShare.server.deleteAllShapes();
            // enter pressed 
        }
    });
    $('#chatinput').keypress(function (e) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13' && $('#chatinput').val()!='') {
            shapeShare.server.sendChatMessage($('#chatinput').val(), $("#userID").val(), $("#groupID").val());
            $('#chatinput').val("");
        } else if (keycode == '13' && $('#chatinput').val() == '')
        {
            alert('請輸入訊息');
        }

    });


    $("#user").change(function () {
        shapeShare.server.changeUserName(shapeShare.state.user.Name, $(this).val()).done(function () {
            $.cookie("userName", shapeShare.state.user.Name, { expires: 30 })
            $("#user").val(shapeShare.state.user.Name);
        });
    });

    $("#keyword").click(function () {


        //例:获得名为deliverQty的参数值   
        //var animal = $.getParameterByName("keyword2"); 
        //alert(animal); 
        //var newid = $.fn.NewGUID(); 
        // 弹出元素编辑界面 
        //shapeShare.server.newKeywordShape(newid, $.getParameterByName("keyword1"), "60", "30"); 
        //newid = $.fn.NewGUID(); 
        //shapeShare.server.newKeywordShape(newid, $.getParameterByName("keyword2"), "70", "40"); 
        //newid = $.fn.NewGUID(); 
        // shapeShare.server.newKeywordShape(newid, $.getParameterByName("keyword3"), "80", "50"); 
    });
    
    

    function dump(obj) {
        var out = '';
        for (var i in obj) {
            out += i + ": " + obj[i] + "\n";
        }

        alert(out);

        // or, if you wanted to avoid alerts... 

        var pre = document.createElement('pre');
        pre.innerHTML = out; shape
        document.body.appendChild(pre)
    }
    $(".toolbar_padding").click(function (event, ui) {
        var newid = $.fn.NewGUID();
        // 弹出元素编辑界面 

        var templateid = $(this).attr('id').replace("Div", "");
        var color = $(this).css('backgroundColor');
        if (color == "")
            color = "#ffffff";
        //mouse pointer start 
        //var event = event || window.event; // IE-ism 
        //var x = event.pageX; 
        //var y = event.pageY; 
        shapeShare.server.newShape(newid, color, templateid, '210', '80', $('#groupID').val());
    });
    $("#designpannel").droppable({
        drop: function (event, ui) {

            //                    var $movingDiv = $(ui.helper[0]); 
            var moving_div = ui.draggable[0];
            var moving_div_templateid = moving_div.id;
            var moving_div_classname = ui.draggable[0].className;
            var color = moving_div.style.backgroundColor;
            if (color == "")
                color = "#ffffff";
            //var str = "How are you doing today?"; 
            var res = moving_div_classname.split(" ");
            //dump(ui.draggable[0].style.backgroundColor); 

            switch (res[0]) {
                case "toolbar_blue":
                    // 建立新元素 
                    // 生成新元素id号，生成机制唯一，id号作为主键 
                    var newid = $.fn.NewGUID();
                    // 弹出元素编辑界面 
                    shapeShare.server.newShape(newid);

                    //            switch (this.className.toLowerCase()) { 
                    //                case "square": 
                    //                    shapeShare.createShape("square"); 
                    //                    break; 
                    //                case "picture": 
                    //                    shapeShare.createShape("picture"); 
                    //                    break; 
                    //            } 
                    //$("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" style="top:60px;left:30px;">  <p class="p_title">Node Label</p><p class="p_content"></p><div class="ep"> </div></div>'); 

                    return false;
                    break;
                case "toolbar_padding":
                    // 建立新元素 
                    // 生成新元素id号，生成机制唯一，id号作为主键 
                    var newid = $.fn.NewGUID();
                    // 弹出元素编辑界面 

                    var templateid = moving_div_templateid.replace("Div", "");
                    //mouse pointer start 
                    //var event = event || window.event; // IE-ism 
                    var x = event.pageX;
                    var y = event.pageY;
                    shapeShare.server.newShape(newid, color, templateid, x, y, $('#groupID').val());
                    //mouse pointer end 


                    //            switch (this.className.toLowerCase()) { 
                    //                case "square": 
                    //                    shapeShare.createShape("square"); 
                    //                    break; 
                    //                case "picture": 
                    //                    shapeShare.createShape("picture"); 
                    //                    break; 
                    //            } 
                    //$("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" style="top:60px;left:30px;">  <p class="p_title">Node Label</p><p class="p_content"></p><div class="ep"> </div></div>'); 

                    return false;
                    break;
            }
        }
    });

    DragFlow.initEndpoints = function (nextColour) {
        $(".ep").each(function (i, e) {
            var p = $(e).parent();
            jsPlumb.makeSource($(e),
            {
                parent: p,
                //anchor:"BottomCenter", 
                anchor: "Continuous",
                connector: ["Straight", { curviness: 20 }],
                cssClass: "aLabel",
                connectorStyle: { strokeStyle: "#5c96bc", lineWidth: 2 },
                endpoint: "Blank",
                maxConnections: 100,
                onMaxConnections: function (info, e) {
                    alert("Maximum connections (" + info.maxConnections + ") reached");
                    //DragFlow.makendpointercontextmenu("", true); 
                }
            });
        });
    };

    // 设置元素为连接源节 Link Color 
    DragFlow.makeSourceById = function (newid) {

        jsPlumb.makeSource($("#" + newid + "").children(".ep"),
                            {
                                parent: newid,
                                anchor: "Continuous",
                                connector: ["Straight", { curviness: 20 }],
                                connectorStyle: { strokeStyle: "#808080", lineWidth: 2 },
                                endpoint: {},
                                maxConnections: 100,
                                cssClass: "aLabel",
                                onMaxConnections: function (info, e) {
                                    alert("Maximum connections (" + (info.maxConnections) + ") reached");
                                }
                            });
    }
    // 设置元素为连接源节 Link Color 
    DragFlow.makeSourceById = function (newid, color) {

        jsPlumb.makeSource($("#" + newid + "").children(".ep"),
                            {
                                parent: newid,
                                anchor: "Continuous",
                                connector: ["Straight", { curviness: 20 }],
                                connectorStyle: { strokeStyle: color, lineWidth: 2 },
                                endpoint: "Blank",
                                maxConnections: 100,
                                cssClass: "aLabel",
                                connectorOverlays:[ 
                                    ["PlainArrow", {width: 10,length: 12, location: 1, id: "arrow"
                                    }],
                                    [ "Label", { label:"label", id:"label", cssClass:"aLabel" } ]
                                ],
                                onMaxConnections: function (info, e) {
                                    alert("Maximum connections (" + (info.maxConnections) + ") reached");
                                }
                            });
    }
    // 设置连接目标节点 
    DragFlow.makeTargetById = function (newid) {
        jsPlumb.makeTarget(newid, {
            dropOptions: { hoverClass: "dragHover" },
            anchor: "Continuous",
            endpoint: "Blank"
            //anchor:"TopCenter"             
        });
    }
    DragFlow.removeEndpoint = function (c) {
        jsPlumb.bind("click", function (c) {
            jsPlumb.detach(c);
        });
        $("#" + id).trigger("click");
    }

    shapeShare.client.connectionDeleted = function (id) {
        //alert(id); 
        jsPlumb.bind("click", function (c) {
            jsPlumb.detach(c);
        });
        $("#" + id).trigger("click");
        jsPlumb.unbind("click");
        jsPlumb.repaintEverything();
        //$("#" + id).trigger("click"); 
    };

    shapeShare.client.appendMessage = function (message) {
        //alert(id); 
        //$("#" + id).trigger("click"); 
        $('#chatwindow').append("" + message + "\n<br />");
        openchatwindow();
        
    };

    shapeShare.client.matchtableload = function (message) {
        //alert(id); 
        //$("#" + id).trigger("click"); 
        $('#mtable').append("(" + message + ") ");        
    };
    shapeShare.client.pmatchtableload = function (message) {
        //alert(id); 
        //$("#" + id).trigger("click"); 
        $('#pmtable').append("(" + message + ") ");
    };
    shapeShare.client.mmatchtableload = function (message) {
        //alert(id); 
        //$("#" + id).trigger("click"); 
        $('#mmtable').append("(" + message + ") ");
    };
    shapeShare.client.addtableload = function (message) {
        //alert(id); 
        //$("#" + id).trigger("click"); 
        $('#addtable').append("(" + message + ") ");
    };
    shapeShare.client.startNodeDropdownLoaded = function (value, text) {
        $('#startnode').append($('<option/>', {
            value: value,
            text: text
        }));
    };
    shapeShare.client.resourceSet = function (val, id) {
        $('#setresource').attr('value', id);
        $('#' + $('#setresource').attr('value')).attr('keyword-id', val);
        $('#' + $('#setresource').attr('value') + ' label').addClass('noderesource');
        $('#' + $('#setresource').attr('value') + ' label').attr('keyword-id', val);
        fireresourcewindow();
    };
    shapeShare.client.endNodeDropdownLoaded = function (value, text) {
        $('#endnode').append($('<option/>', {
            value: value,
            text: text
        }));
    };
    $('#selectsenode').click(function () {
        $('#refreshbtn').trigger('click');
        shapeShare.server.loadNodePath($("#startnode option:selected").val(), $("#endnode option:selected").val(), $("#browsingStudentList option:selected").val());
    });
    shapeShare.client.singleNodePathLoaded = function (nodepath) {
        //console.log(nodepath);
        //$('#refreshbtn').trigger('click');
        $('._jsPlumb_overlay').css("z-index", 5);
        var nodepathdata = nodepath.split(";");
        
         for (var i = 0; i < nodepathdata.length; i++) {
            $('#' + nodepathdata[i]).css('background-color', '#F5A9A9');
            $('#' + nodepathdata[i]).css('z-index', '6');
        }
        for (var j = 0; j < nodepathdata.length-1; j++) {
            var conn = jsPlumb.select({ source: nodepathdata[j], target: nodepathdata[j + 1] }).setPaintStyle({
                strokeStyle: "#F5A9A9",
                lineWidth: 2
            }).each(function (c) {
                $(c.canvas).css("z-index", 6);
                $(c.canvas).next().css("z-index", 6);
            });
            
            //console.log(jsPlumb.select({ source: nodepathdata[j], target: nodepathdata[j + 1] }).addClass("highligted"));
        }
        //
    };
    shapeShare.client.comparePathListed = function (id, rootleaf, value) {
        //console.log(nodepath);
        $('#pathMatchTable tr:last').after('<tr><td>' + id + '</td><td>' + rootleaf + '</td><td>' + value + '</td></tr>');
        
        //var nodepathdata = nodepath.split(";");
        
        //for (var i = 0; i < nodepathdata.length; i++) {
        //    $('#' + nodepathdata[i]).css('background-color', '#F5A9A9');
        //    $('#' + nodepathdata[i]).css('z-index', '6');
        //}
       
    };
    // Node Mouse Right Click 
    DragFlow.makecontextmenu = function (newid, isAllElements) {

        var els = $("#" + newid);
        if (isAllElements) {
            els = $(".component");
        }

        els.contextMenu('myMenu1',
             {
                 bindings:
                  {
                      'edit': function (t) {

                          // 弹出编辑页面 
                          $("#itemLabel").val($("#" + t.id + " label").html());
                          // alert('Trigger was ' + t.id + '\nAction was Open'); 
                          // alert($('label[for="' + t.id + '"]').html()); 
                          
                          var connections = [];
                          connections.push({
                              NodeID: t.id,
                              FieldName: "Field Name",
                              FieldValue: "Field Value"
                          });

                          //$('#testspan').html(localStorage["DFCC7DEB-2171-25ED-2320-39B38B9EC8F4"]); 
                          //printJson(localStorage["template-"+$("#" + t.id).attr("template-id")]); 
                          var templateData = localStorage["template-" + $("#" + t.id).attr("template-id")];
                          //var nodeTempData = localStorage[t.id]; 
                          //console.log(templateData); 
                          if (typeof templateData !== 'undefined' && templateData !== null) {
                              if (localStorage[t.id] == null) {
                                  shapeShare.server.settingTemplateData(t.id, localStorage["template-" + $("#" + t.id).attr("template-id")]);
                                  $('#tempField').html(appendTempField(localStorage["template-" + $("#" + t.id).attr("template-id")]));
                              } else {
                                  $('#tempField').html(appendTempField(localStorage[t.id]));
                              }
                          }



                          //$.ajax({ 
                          //      type: "POST", 
                          //      contentType: "application/json; charset=utf-8", 
                          //      url: "DragFlow.aspx/InsertDefaultNodeValue", 
                          //      data: "{connections:" + JSON.stringify(connections) + "}", 
                          //      dataType: "json", 
                          //      success: function (data) { 
                          //          var obj = data.d; 
                          //          //alert(obj); 
                          //          if (obj == 'true') { 
                          //              //alert("Successfully"); 
                          //              $("#myiFrame").attr("src", "node.aspx?id=" + t.id); 
                          //          } 
                          //          if (obj = false) { 
                          //              alert("Error"); 
                          //          } 
                          //      }, 
                          //      error: function (result) { 
                          //          //alert("Error"); 
                          //      } 
                          //  }); 
                          var jsonTempl = [];

                          //var jsonValue = new Array(); 
                          $("#editDialog").dialog({
                              autoResize: true,
                              width: 650,
                              //height: 600, 
                              buttons: {

                                  'OK': {
                                      text: 'OK',
                                      class: 'btn',
                                      click: function () {

                                          //$('input[name="fieldName[]"]').each(function () { 
                                          //jsonName.push($(this).val()); 
                                          //alert($(this).val()); 
                                          //alert($(this).prev().val()); 
                                          //}); 
                                          $('input[name="fieldValue[]"]').each(function () {
                                              //alert($(this).prev().val()); 
                                              jsonTempl.push({
                                                  FieldName: $(this).prev().val(),
                                                  FieldValue: $(this).val(),
                                                  Type: $(this).next().val()
                                              });
                                              //alert($(this).val()); 

                                              //jsonValue.push((this).val()); 
                                          });
                                          shapeShare.server.settingTemplateData(t.id, JSON.stringify(jsonTempl));
                                          //console.log(jsonTempl); 
                                          //console.log(jsonValue); 
                                          shapeShare.server.editNode(t.id, $("#itemLabel").val(), $('#groupID').val());
                                          $(this).dialog('close');
                                      }
                                  }
                              }
                          });
                      },
                      'resource': function (t) {
                          // 执行删除操作 
                          $("#resourceDialog").dialog({
                              autoResize: true,
                              width: 400,
                              height: 800,
                              open: function (ev, ui) {
                                  var keywordid = $('#' + t.id).attr('keyword-id');

                                  if (typeof keywordid !== typeof undefined && keywordid !== false) {
                                      $('#resourceIframe').attr('src', 'ResourceView.aspx?id=' + keywordid);
                                  } else {
                                      $("#resourceDialog").css('height', '100px');
                                      $('#resourceIframe').attr('src', '');
                                  }
                                  $('#setresource').attr('value', t.id);


                              }
                          });
                          
                      },
                      'delete': function (t) {
                          // 执行删除操作 

                          ConfirmDelete(t.id);
                      },
                      'save': function (t) {
                          // 保存流程操作 
                          alert('Trigger was ' + t.id + '\nAction was Save');
                      }
                  }
             });
    }
    
    
    DragFlow.makendpointercontextmenu = function (newid, component, isAllElements) {

        //var els = $("#" + newid); 
        //if (isAllElements) { 
        //    els = $(".aLabel"); 
        //} 

        //els.contextMenu('myEndpointMenu', 
        //     { 
        //         bindings: 
        //          { 
        //              'editEndpoint': function (t) { 

        //                  // 弹出编辑页面 
        //                  $("#itemLabel").val($("#" + t.id).html()); 
        //                  // alert('Trigger was ' + t.id + '\nAction was Open'); 
        //                  // alert($('label[for="' + t.id + '"]').html()); 
        //                  $("#editDialog").dialog({ 
        //                      buttons: { 
        //                          'OK': function () { 
        //                              //console.log(t); 
        //                              //var $elem = $(elem); 
        //                              //var lbl = $("#" + $elem.attr('id')).find('label'); 
        //                              //$.each(jsPlumb.getConnections(), function (idx, connection) { 
        //                              //label.getOverlay("label").setLabel($("#itemLabel").val()); 
        //                                  //$("#" + $elem.attr('id')).find('label') 
        //                              //}); 

        //                              component.getOverlay("label").setLabel($("#itemLabel").val()); 
        //                              //component.getOverlay("label").hide(); 
        //                              //shapeShare.server.editEP(t.id, $("#itemLabel").val()); 
        //                              //shapeShare.client.editNewEP(component, $("#itemLabel").val()); 

        //                              //shapeShare.server.editEP(t.id, $("#itemLabel").val()); 
        //                              $(this).dialog('close'); 
        //                          } 
        //                      } 
        //                  }); 
        //              }, 
        //              'deleteEndpoint': function (t) { 
        //                  // 执行删除操作 
        //                  //if (confirm('Are you sure that you want to permanently delete the selected element?')) { 
        //                      //DragFlow.removeEndpoint(t); 
        //                  //}  
        //                  ConfirmDeleteEP(t); 
        //              }, 
        //              'save': function (t) { 
        //                  // 保存流程操作 
        //                  alert('Trigger was ' + t.id + '\nAction was Save'); 
        //              } 
        //          } 
        //     }); 
    }
    //jsPlumb.bind("beforeDrop", function (connection) { alert(connection); }); 
    //link label right mouse click 
    jsPlumb.bind("contextmenu", function (component, originalEvent) {
        //alert("context menu on component " + component.id); 
        //DragFlow.makendpointercontextmenu("", component, true); 
        var els = $(".aLabel");
        $('#tmplinkid').val(component.getParameter("linkid"));
        $('#tmptpllinkid').val(component.getParameter("templateid"));
        els.contextMenu('myEndpointMenu',
             {
                 bindings:
                  {
                      'editEndpoint': function (t) {
                          console.log( $('#tmplinkid').val());
                          //t.id 
                          $("#linkLabel").val($("#" + t.id).html());
                          var tid = "";
                          var templatelinkid = "";
                          var linkid = "";
                          //$.each(jsPlumb.getConnections(), function (idx, connection) {
                              //label.getOverlay("label").setLabel($("#itemLabel").val()); 
                              //$("#" + $elem.attr('id')).find('label') 
                              //console.log(id + connection.overlays[1].canvas.id); 
                              //if (t.id === connection.overlays[1].canvas.id) {
                                  //console.log(id + " " + connection.overlays[1].canvas.id) 
                                  //connection.overlays[1].setLabel(value); 
                                  //console.log(connection.overlays[1].canvas.id); 
                                  
                                  //tid = connection.getParameter("linkid");
                                  //linkid = connection.getId(); 
                                  //console.log(templatelinkid); 
                              //}
                              //console.log(connection.overlays[1].canvas.id);813 
                          //});
                          templatelinkid = $('#tmptpllinkid').val();
                          tid = $('#tmplinkid').val();
                          //console.log(templatelinkid); 
                          //初始化link data 
                          var templateLinkData = localStorage["linktemplate-" + templatelinkid];
                          //console.log(templateLinkData); 
                          if (typeof templateLinkData !== 'undefined' && templateLinkData !== null) {
                              if (localStorage[t.id + "_" + $("#groupID").val()] == null) {
                                  shapeShare.server.settingTemplateData(t.id + "_" + $("#groupID").val(), localStorage["linktemplate-" + templatelinkid]);
                                  $('#tempLinkField').html(appendTempField(localStorage["linktemplate-" + templatelinkid]));
                              } else {
                                  $('#tempLinkField').html(appendTempField(localStorage[t.id + "_" + $("#groupID").val()]));
                                  //console.log(appendTempField(localStorage[t.id])); 
                              }

                          }


                          //console.log($("#" + t.id).html()); 
                          // alert('Trigger was ' + t.id + '\nAction was Open'); 
                          // alert($('label[for="' + t.id + '"]').html()); 
                          var jsonTempl = [];
                          $("#editLabelDialog").dialog({
                              autoResize: true,
                              width: 650,

                              buttons: {
                                  'OK': {
                                      text: 'OK',
                                      class: 'btn',
                                      click: function (t) {
                                          //console.log(t); 
                                          //var $elem = $(elem); 
                                          //var lbl = $("#" + $elem.attr('id')).find('label'); 
                                          //console.log($("#linkLabel").val()); 

                                          $('input[name="fieldValue[]"]').each(function () {
                                              //alert($(this).prev().val()); 

                                              jsonTempl.push({
                                                  FieldName: $(this).prev().val(),
                                                  FieldValue: $(this).val(),
                                                  Type: $(this).next().val()
                                              });

                                              //alert($(this).val()); 

                                              //jsonValue.push((this).val()); 
                                          });

                                          shapeShare.server.settingTemplateData(tid, JSON.stringify(jsonTempl));
                                          shapeShare.server.editEP(tid, $("#linkLabel").val(), $("#groupID").val());
                                          //console.log(tid); 
                                          //console.log(jsPlumb.getConnections()); 
                                          //console.log(originalEvent); 

                                          //console.log(component.overlays[1].getLabel()); 
                                          //console.log(t.id); 
                                          //console.log(component.overlays[1].getLabel()); 
                                          //component.getOverlay("label").hide(); 
                                          //component.getOverlay("label").setLabel($("#itemLabel").val()); 
                                          //shapeShare.server.editEP(t.id, $("#linkLabel").val()); 

                                          //shapeShare.client.editNewEP(component, $("#itemLabel").val()); 

                                          //shapeShare.server.editEP(t.id, $("#itemLabel").val()); 
                                          $(this).dialog('close');
                                      }
                                  }
                              }
                          });
                          originalEvent.preventDefault();
                      },
                      'deleteEndpoint': function (t) {
                          // 执行删除操作 
                          //if (confirm('Are you sure that you want to permanently delete the selected element?')) { 
                          //DragFlow.removeEndpoint(t); 
                          //}  
                          ConfirmDeleteEP(t);
                          originalEvent.preventDefault();
                      },
                      'save': function (t) {
                          // 保存流程操作 
                          alert('Trigger was ' + t.id + '\nAction was Save');
                          originalEvent.preventDefault();
                      }
                  }
             });
        //alert("context menu on component " + component.id); 
        //console.log(component); 
        originalEvent.preventDefault();
        return false;
    });
    function ConfirmDelete(courseID) {
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 150,
            modal: true,
            buttons: {
                "OK": function () {
                    $(this).dialog("close");
                    shapeShare.server.deleteShape(courseID, $('#groupID').val());
                },
                Cancel: function () {
                    $(this).dialog("close");
                    return false;
                }
            }
        });
        return false; //The actual submission of the form happens in the click handler for the delete button 
    }
    function ConfirmDeleteEP(courseID) {
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 150,
            modal: true,
            buttons: {
                "OK": function () {
                    $(this).dialog("close");
                    shapeShare.server.deleteConnection(courseID.id, $('#groupID').val());
                    //alert(courseID.id); 
                    //DragFlow.removeEndpoint(courseID); 
                    //shapeShare.client.connectionDeleted(courseID); 
                },
                Cancel: function () {
                    $(this).dialog("close");
                    return false;
                }
            }
        });
        return false; //The actual submission of the form happens in the click handler for the delete button 
    }
    function printJson(obj) {
        var objData = JSON.parse(obj);
        for (var key in objData) {
            if (objData.hasOwnProperty(key)) {
                alert(objData[key]["FieldName"] + " = " + objData[key]["FieldValue"]);
            }
        }
    }
    function appendTempField(obj) {
        var objData = JSON.parse(obj);
        var fieldGroup = "<table class='table borderless'><tr><th>Field Name</th><th>Field Value</th></tr>";
        for (var key in objData) {
            if (objData.hasOwnProperty(key)) {
                fieldGroup += "<tr><td>" + objData[key]["FieldName"] + " [" + objData[key]["Type"].charAt(0) + "]</td><td> " + "<input type=\"hidden\" name=\"fieldName[]\" value=\"" + objData[key]["FieldName"] + "\"><input type=\"text\" name=\"fieldValue[]\" style=\"width:100%;\" value=\"" + objData[key]["FieldValue"] + "\"/><input type=\"hidden\" name=\"type[]\" value=\"" + objData[key]["Type"] + "\"></td></tr>";
                //alert(objData[key]["FieldName"] + " = " + objData[key]["FieldValue"]); 
            }
        }
        fieldGroup += "</table>";
        return fieldGroup;
    }
    //function load() { 
    //shapeShare.server.getShapes().done(function (shapes) { 
    //    $.each(shapes, function () { 
    //        shapeShare.client.shapeAdded(this); 
    //    }); 
    //}); 
    // 
    //----------------------傳值給昌韓meeting system start 
    /*if ($.getParameterByName("keyword1") != "" || $.getParameterByName("keyword2") != "" || $.getParameterByName("keyword3") != "" || $.getParameterByName("seq") != "") { 
        var seq = $.getParameterByName("seq"); 
        var newid1 = $.fn.NewGUID(); 
        var newid2 = $.fn.NewGUID(); 
        var newid3 = $.fn.NewGUID(); 

        shapeShare.server.newKeywordShape(seq,newid1, newid2, newid3, "", "0", "130");*/
    //----------------------傳值給昌韓meeting system end 
    //    if ($.getParameterByName("keyword1") != "") { 
    //        var newid = $.fn.NewGUID(); 
    //        shapeShare.server.newKeywordShape(newid, $.getParameterByName("keyword1"), "0", "130"); 
    //    } 
    //    if ($.getParameterByName("keyword2") != "") { 
    //        var newid = $.fn.NewGUID(); 
    //        shapeShare.server.newKeywordShape(newid, $.getParameterByName("keyword2"), "10", "140"); 
    //    } 
    //    if ($.getParameterByName("keyword3") != "") { 
    //        var newid = $.fn.NewGUID(); 
    //        shapeShare.server.newKeywordShape(newid, $.getParameterByName("keyword3"), "20", "150"); 
    //   } 
    //    window.open('', '_self', ''); 
    //    window.close(); 
    //} 
    //} 

    //jsPlumb.bind("connection", function (info) { 
    //info.connection.getOverlay("label").setLabel("label"); 
    //shapeShare.server.connectNode(info.sourceId, info.targetId); 
    //alert(info.sourceId + " " + info.targetId); 
    //}); 
    // 链接创建成功事件，在此事件中可以加入增加链接的逻辑 



    jsPlumb.bind("jsPlumbConnection", function (conn, e) {

        var color = "";
        if ($('#linktemplateColor').val() != '') {
            color = $('#linktemplateColor').val();
        }
        else {
            color = "#808080";
        }
        var templateid = "";
        if ($('#linktemplateID').val() != '') {
            templateid = $('#linktemplateID').val();
        }
        else {
            templateid = "0";
        }
        //conn. 
        //jsPlumb.registerConnectionTypes({ 
        //    "foo": { connectorStyle: { strokeStyle: color, lineWidth: 2 } } 
        //}); 
        //conn.connection.setType("foo"); 
        //console.log(e); 
        //conn.getConnections().strokeStyle.color = color; 
        //e.preventDefault(); 
        //conn.connection.getOverlay("label").setLabel("label"); 
        //alert(jsPlumb._getId(conn.connection.getOverlay("label"))); 
        //jsPlumb.unbind("jsPlumbConnection"); 
        //console.log(conn.connection.id); 
        //$('input[id^="linktemplateColor"]').each(function (input) { 
        //    var value = $('input[id^="linktemplateColor"]').val(); 
        //    var id = $('input[id^="linktemplateColor"]').attr('id'); 
        //    alert('id: ' + id + ' value:' + value); 
        //}); 
        //conn.endpoints.setPaintStyle({ fillStyle: color }); 
        //console.log(conn.connection); 
        //conn.connection.setPaintStyle({ fillStyle: color }); 
        //console.log($("input[id*='linktemplateColor']").val()); 
        //conn.connection.addClass("123"); 
        //conn.connection.setLabel("label");
        //conn.connection.addOverlay(["Label", {label: "label",id: "label",cssClass: "aLabel"}]);
        conn.connection.setParameter("templateid", templateid);
        var newid = $.fn.NewGUID();
        conn.connection.setParameter("linkid", newid);
        var datetime = getCurDateTime();
        conn.connection.setParameter("createdatetime", datetime);



        //conn.connection.setPaintStyle({ strokeStyle: color, lineWidth: 2 });
        //jsPlumb.select({ source: conn.sourceId, target: conn.targetId }).getPaintStyle().strokeStyle = color; 
        //conn.connection.repaint(); 
        /*jsPlumb.makeSource($("#" + conn.sourceId + "").children(".ep"), 
                          { 
                              parent: conn.sourceId, 
                              anchor: "Continuous", 
                              connector: ["Flowchart", { curviness: 20 }], 
                              connectorStyle: { strokeStyle: color, lineWidth: 2 }, 
                              endpoint: "Blank", 
                              maxConnections: 5, 
                              cssClass: "aLabel", 
                              onMaxConnections: function (info, e) { 
                                  alert("Maximum connections (" + (info.maxConnections) + ") reached"); 
                              } 
                          });*/
        shapeShare.server.connectNode(conn.sourceId, conn.targetId, $.connection.hub.id, color, "", newid, datetime, $('#groupID').val());
        //shapeShare.server.connectNode(conn.sourceId, conn.targetId); 
        //alert(conn.sourceId+" "+conn.targetId); 
    });

    $('.linkButton').on("click", function () {
        $('.linkButton').removeClass('active');
        $('#linktemplateID').val($(this).parent().attr('id').replace("Div", ""));
        $('#linktemplateColor').val($(this).css('color'));
        shapeShare.server.setLinkTemplateIDcolor($(this).parent().attr('id').replace("Div", ""), $(this).css('color'));
        //jsPlumb.repaintEverything(); 
    });

    $("#DropDownList1").change(function () {
        $("#DropDownList1 option:selected").each(function () {
            //alert($(this).val()); 
            shapeShare.server.emptyPanel();
            shapeShare.server.loadConceptMap($(this).val(), $('#groupID').val());
            $('#openDialog').dialog('close');
        });
    });

    $('#openconceptmap').click(function () {

        $("#browsingopenDialog").dialog({
            autoResize: true,
            autoOpen: true,
            width: 450,
            modal: false,
            buttons: {
                'OK': {
                    text: 'OK',
                    'class': 'btn',
                    click: function () {
                        if ($("#browsingStudentList option:selected").val() != '' && $("#browsingTeacherList option:selected").val() != '')
                        {
                            if ($("#browsingStudentList option:selected").val() != $("#browsingTeacherList option:selected").val())
                            {
                                //$('#loadingIMG').show();
                                $("#designpannel").empty();
                                $("#designpannelt").empty();
                                $('#answerpanel').dialog('open');
                                shapeShare.server.browsingLoadSConceptMap($("#browsingStudentList option:selected").val(), $("#browsingTeacherList option:selected").val(), "designpannel");
                                shapeShare.server.browsingLoadTConceptMap($("#browsingTeacherList option:selected").val(), $("#browsingStudentList option:selected").val(), "designpannelt");
                                jsPlumb.repaintEverything();

                                
                                if ($('#startnode').has('option').length > 0)
                                {
                                    $('#startnode').empty().append('<option selected="selected">-</option>');
                                }
                                if ($('#endnode').has('option').length > 0)
                                {
                                    $('#endnode').empty().append('<option selected="selected">-</option>');
                                }
                                shapeShare.server.generateNodePath($("#browsingStudentList option:selected").val(), "student");
                                shapeShare.server.generateNodePath($("#browsingTeacherList option:selected").val(), "teacher");
                                shapeShare.server.generateComparePathList($("#browsingStudentList option:selected").val(), $("#browsingTeacherList option:selected").val());
                                $(this).dialog('close');
                                //$('#loadingIMG').hide();
                                //shapeShare.server.loadStartNodeDropdown($("#browsingStudentList option:selected").val());
                                //shapeShare.server.loadEndNodeDropdown($("#browsingStudentList option:selected").val());

                            }
                            else
                            {
                                alert("請重新選擇不相同的概念圖");
                            }
                        }
                        else
                        {
                            alert("請選擇任兩張概念圖");
                        }
                        //$('#' + $('#setresource').attr('value')).wrapInner('<a href="#" class="noderesource" keyword-id="' + val + '" style="cursor:pointer;" />');

                        
                    }
                }
            }
        });
    });
    //切換tab重整畫面
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        jsPlumb.repaintEverything();
    })

    $("#itemLabel").autocomplete({
        source: availableTags,        minLength: 0
    }).focus(function () {
        $(this).autocomplete("search");
    });
    function fireresourcewindow() {
        $(".noderesource").on("click", function () {
            var keywordid = $(this).attr('keyword-id');
            if (typeof keywordid !== typeof undefined && keywordid !== false) {
                $('#resourceIframe').attr('src', 'ResourceView.aspx?id=' + keywordid);
            } else {
                $("#resourceDialog").css('height', '100px');
                $('#resourceIframe').attr('src', '');
            }
            $("#resourceDialog").dialog({
                autoResize: true,
                width: 400,
                height: 800
            });
        });
    }
    function isEmpty(obj) {

        // null and undefined are "empty"
        if (obj == null) return true;

        // Assume if it has a length property with a non-zero value
        // that that property is correct.
        if (obj.length > 0) return false;
        if (obj.length === 0) return true;

        // Otherwise, does it have any properties of its own?
        // Note that this doesn't handle
        // toString and valueOf enumeration bugs in IE < 9
        for (var key in obj) {
            if (hasOwnProperty.call(obj, key)) return false;
        }

        return true;
    }
    function copySelection() {
        var textComponent = document.getElementById('chatwindow');
        var selectedText;
        // IE version
        if (document.selection != undefined) {
            textComponent.focus();
            var sel = document.selection.createRange();
            selectedText = sel.text;
        }
            // Mozilla version
        else if (textComponent.selectionStart != undefined) {
            var startPos = textComponent.selectionStart;
            var endPos = textComponent.selectionEnd;
            selectedText = textComponent.value.substring(startPos, endPos)
        }
        return selectedText;
    }
    $('#playbtn').click(function () {
        if (isEmpty(nodePlayDupQueue) && isEmpty(linkPlayDupQueue)) {
            nodePlayDupQueue = JSON.parse(JSON.stringify(nodeQueue));
            linkPlayDupQueue = JSON.parse(JSON.stringify(linkQueue));
        }
        //console.log(nodePlayDupQueue);
        var tmpnode = "";
        var tmplink = "";
        if (!isEmpty(nodePlayDupQueue))
            tmpnode = nodePlayDupQueue.shift();
        if (!isEmpty(linkPlayDupQueue))
            tmplink = linkPlayDupQueue.shift();

        var tmpQueue = [];
        //console.log(nodePlayDupQueue);
        if (tmpnode.datetime <= tmplink.datetime) {
            //console.log(tmpnode);
            $('#' + tmpnode.nID).css('background-color', '#00FFFF');
            linkPlayDupQueue.unshift(tmplink);
        } else
        {
            $.each(jsPlumb.getConnections(), function (idx, connection) {
                if (connection.getParameter("linkid") == tmplink.lID) {
                    connection.setPaintStyle({
                        strokeStyle: "Cyan",
                        lineWidth: 2
                    });
                }
            });
            nodePlayDupQueue.unshift(tmpnode);
        }
        
    });


    $('#refreshbtn').click(function () {
        nodeRefreshDupQueue = JSON.parse(JSON.stringify(nodeQueue));
        linkRefreshDupQueue = JSON.parse(JSON.stringify(linkQueue));
        
        //var nodeidval = nodePlayDupQueue.shift();
        for (var key in nodeRefreshDupQueue) {
            var obj = nodeRefreshDupQueue[key];
            $('#' + obj.nID).css('background-color', obj.color);
            //console.log(obj.nID);
            //for (var prop in obj) {
                // important check that this is objects own property 
                // not from prototype prop inherited
                //if (obj.hasOwnProperty(prop)) {
                    //alert(prop + " = " + obj[prop]);
                //}
            //}
        }
        for (var key in linkRefreshDupQueue) {
            var obj = linkRefreshDupQueue[key];
            $.each(jsPlumb.getConnections(), function (idx, connection) {
                //console.log(connection.getParameter("linkid"));
                if (connection.getParameter("linkid") == obj.lID) {
                    connection.setPaintStyle({
                        strokeStyle: obj.color,
                        lineWidth: 2
                    });
                }
            });
        }
        nodePlayDupQueue = [];
        linkPlayDupQueue = [];

        $('#overlay').hide();
        $('#highlight-vertical').slider('value', 0);
        $('#highlightamount').html('1');
        $("#designpannel .component").each(function (idx, elem) {
            $(this).css('z-index', '4');
        });
        
        //$('#' + nodeidval.nID).css('background-color', '#0000FF'); $('#' + nodeidval.nID).css('border-color', '#0000FF');
    });


    //$('.noderesource').click(function (e) {
       // $('#resourceIframe').attr('src', 'ResourceView.aspx?id='+e.data.id);
       /* $("#resourceDialog").dialog({
            autoResize: true,
            width: 400,
            height: 800,
            open: function (ev, ui) {
                var keywordid = $('#' + t.id).attr('keyword-id');

                if (typeof keywordid !== typeof undefined && keywordid !== false) {
                    $('#resourceIframe').attr('src', 'ResourceView.aspx?id=' + keywordid);
                } else {
                    $("#resourceDialog").css('height', '100px');
                    $('#resourceIframe').attr('src', '');
                }
                $('#setresource').attr('value', t.id);


            }
        });*/
        //alert('');
    //});
    $('#setresource').click(function () {
        $('#sResourceDialog').dialog({
            autoResize: true,
            autoOpen: true,
            width: 650,
            modal:false,
            buttons: {
                'OK': {
                    text: 'OK',
                    'class': 'btn',
                    click: function () {

                        var val = $('input:radio[name=keywordid]:checked').val();
                        shapeShare.server.setResource(val,  $('#setresource').attr('value'), $('#groupID').val());
                        
                        //$('#' + $('#setresource').attr('value')).wrapInner('<a href="#" class="noderesource" keyword-id="' + val + '" style="cursor:pointer;" />');
                        
                        $(this).dialog('close');
                    }
                }
            }
        });
    });
    // jqueryui defaults
    $.extend($.ui.dialog.prototype.options, { 
        create: function() {
            var $this = $(this);

            // focus first button and bind enter to it
            $this.parent().find('.ui-dialog-buttonpane button:first').focus();
            $this.keypress(function(e) {
                if( e.keyCode == $.ui.keyCode.ENTER ) {
                    $this.parent().find('.ui-dialog-buttonpane button:first').click();
                    return false;
                }
            });
        } 
    });

    $("#userListDialog").dialog({
        autoOpen: true,
        modal: false,
        buttons: {
            "lockbtn": {
                text: "  鎖定  ",
                id: "lock",
                click: function () {
                    var usercheckbox = $('input:checkbox[name=userlistradio]');
                    for (var i = 0; i < usercheckbox.length; i++)
                    {
                        if (usercheckbox[i].checked){
                            shapeShare.server.muteEdit(usercheckbox[i].value);
                            shapeShare.server.showUserStat(usercheckbox[i].value, $('#groupID').val(), " <img src=\"images/lock.png\" />", "");
                        }
                    }
                }
            },
            "unlockbtn": {
                text: "  解鎖  ",
                id: "unlock",
                click: function () {
                    var usercheckbox = $('input:checkbox[name=userlistradio]');
                    for (var i = 0; i < usercheckbox.length; i++)
                    {
                        if (usercheckbox[i].checked){
                            shapeShare.server.unlockMuteEdit(usercheckbox[i].value);
                            shapeShare.server.showUserStat(usercheckbox[i].value, $('#groupID').val(), "", "");
                        }
                    }
                }
            }
        },
        position: { my: "right top", at: "right top", of: window }

    });

    $("#userlistbtn").click(function () {
        $("#userListDialog").dialog("open");
    });

    function setCookie(cName, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = cName + "=" + c_value;
    }
    $.connection.hub.logging = true;
    $.connection.hub.start({ transport: activeTransport }, function () {
        shapeShare.server.join($.cookie("userName"))
                         .done(function () {
                             $.cookie("userName", shapeShare.state.user.Name, { expires: 30 });
                             $("#user").val(shapeShare.state.user.Name);
                             //$("#templateID").val("abc123"); 
                             var myClientId = $.connection.hub.id;
                             setCookie("srconnectionid", myClientId);
                             var groupid = $.getParameterByName("groupID")
                             if (groupid != "") {
                                 $('#groupID').val(groupid);
                                 shapeShare.server.joinGroup(groupid);
                                 shapeShare.server.setUserlist($("#username").val(), $.connection.hub.id, groupid);
                                 //shapeShare.server.refreshList();
                             }
                             else {
                                 shapeShare.server.joinGroup("0");
                                 shapeShare.server.setUserlist($("#username").val(), $.connection.hub.id, "0");
                                 //shapeShare.server.refreshList();
                             }
                             //load(); 
                         });
    });


});