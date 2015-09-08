/// <reference path="../../Scripts/jquery-1.6.4.js" />
/// <reference path="../../Scripts/jquery-ui-1.8.12.js" />
/// <reference path="../../Scripts/jQuery.tmpl.js" />
/// <reference path="../../Scripts/jquery.cookie.js" />
/// <reference path="../../Scripts/signalR.js" />

$(function () {
    'use strict';

    var shapeShare = $.connection.shapeShare;

    (function ($) {
        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        $.getParameterByName = getParameterByName;
    })(jQuery);


    function changeShape(el) {
        //console.log(el);
        shapeShare.server.moveShape($(el).attr('id'), el.offsetLeft, el.offsetTop || 0, el.clientWidth, el.clientHeight);
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
                drag: function (event, ui) {

                    changeShape(this);
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

    shapeShare.client.shapeCreated = function (newid) {
        $("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" style="top:0px;left:150px;position:absolute;"><label>Node Label</label><div class="ep"> </div></div>');
        DragFlow.makeSourceById(newid);

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
        $("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" style="top:0px;left:150px;position:absolute;background-color:' + color + '"><label>Node Label</label><div class="ep"> </div></div>');
        DragFlow.makeSourceById(newid);

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
        $("#designpannel").append('<div id="' + newid + '" template-id="' + templateid + '" class="gradient_blue component window" style="top:0px;left:150px;position:absolute;background-color:' + color + '"><label>Node Label</label><div class="ep"> </div></div>');
        DragFlow.makeSourceById(newid);

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
        //console.log(x + " " + y);
        $("#designpannel").append('<div id="' + newid + '" template-id="' + templateid + '" class="gradient_blue component window" style="top:' + (y - 80) + 'px;left:' + (x - 80) + 'px;position:absolute;background-color:' + color + '"><label>Node Label</label><div class="ep"> </div></div>');
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
                //25console.log(event);
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
        $("#designpannel").append('<div id="' + newid + '" class="gradient_blue component window" style="top:' + top + 'px;left:' + left + 'px;position:absolute;"><label>' + label + '</label><div class="ep"> </div></div>');
        DragFlow.makeSourceById(newid);

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

    //shapeShare.client.mask = function () {
    //    $("body").mask("Loading...");
    //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); });

    //};
    //shapeShare.client.unmask = function () {
    //    $("body").unmask();
    //$("#designpannel div[id^='childdiv']").each(function (el) { $(el).empty(); });

    //};
    shapeShare.client.shapeLoaded = function (nodeID, nodeLabel, xCoordinate, yCoordinate, width, height) {
        $("#designpannel").append('<div id="' + nodeID + '" class="gradient_blue component window" style="top:' + yCoordinate + 'px;left:' + xCoordinate + 'px;width:' + width + 'px;height:' + height + 'px;position:absolute;"><label>' + nodeLabel + '</label><div class="ep"> </div></div>');
        DragFlow.makeSourceById(nodeID);

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

    shapeShare.client.shapeLoaded = function (nodeID, nodeLabel, templateId, color, xCoordinate, yCoordinate, width, height) {
        $("#designpannel").append('<div id="' + nodeID + '" template-id="' + templateId + '" class="gradient_blue component window" style="top:' + yCoordinate + 'px;left:' + xCoordinate + 'px;width:' + width + 'px;height:' + height + 'px;position:absolute;background-color:' + color + '"><label>' + nodeLabel + '</label><div class="ep"> </div></div>');
        DragFlow.makeSourceById(nodeID);

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
    shapeShare.client.linkLoaded = function (linkID, linkLabel, startNode, endNode) {
        //jsPlumb.unbind("jsPlumbConnection");
        jsPlumb.bind("ready", function () {
            var conn = jsPlumb.connect({
                source: startNode,
                target: endNode,
                //paintStyle: { strokeStyle: "#808080", lineWidth: 2 },
                //connector: ["Flowchart", { curviness: 20 }],
                //connectorStyle: { lineWidth: 2,strokeStyle: "#808080" },
                overlays: [
            ["Label", { label: linkLabel, id: "label", cssClass: "aLabel", }]
                ]
            });
        });
        var connid = new Array();
        $("._jsPlumb_overlay").each(function (i, obj) {
            connid.push($(obj).attr("id"));
        });

        //shapeShare.server.changeConnID(connid, $.connection.hub.id);

        //jsPlumb.bind("jsPlumbConnection", function (conn) {
        //    shapeShare.server.connectNode(startNode, endNode, $.connection.hub.id);

        //});
        //bindconnection();
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

        shapeShare.server.changeConnID(connid, clientid);
        //jsPlumb.bind("jsPlumbConnection", function (conn) {
        //    shapeShare.server.connectNode(sourceid, targetid, clientid);
        //});
    };
    shapeShare.client.nodeConnected = function (sourceid, targetid, clientid, linkcolor) {
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
            target: targetid
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

        //shapeShare.server.changeConnID(connid, clientid);

        //jsPlumb.bind("jsPlumbConnection", function (conn) {
        //    shapeShare.server.connectNode(sourceid, targetid, clientid, color);
        //bindconnection();
        //});
        //conn.setParameter("idname","123");
        //$("#jsPlumb_1_11").attr("id","jsPlumb_1_17");
        //var els = $(".aLabel");
        //alert(els.id);
        //});

        jsPlumb.bind("jsPlumbConnection", function (conn, e) {

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

            conn.connection.setParameter("templateid", templateid);
            console.log(conn.connection);
            conn.connection.setPaintStyle({ strokeStyle: color, lineWidth: 2, });

            //shapeShare.server.connectNode(conn.sourceId, conn.targetId, $.connection.hub.id, color);

        });

        //console.log(conn.getParameter("templateid"));
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
            if (id === connection.overlays[1].canvas.id) {
                //console.log(id + " " + connection.overlays[1].canvas.id)
                connection.overlays[1].setLabel(value);
                //alert(connection.getParameter("templateid"));
            }
            //console.log(connection.overlays[1].canvas.id);813
        });
        jsPlumb.repaintEverything();
        //});
    };

    $('#send').click(function () {

        //shapeShare.server.joinGroup($('#group').val());

        alert("Joined group " + $('#group').val() + " !");

    });
    $('#toolbar')
        .delegate("menu[label=add] a", "click", function (e) {
            e.preventDefault();

            //shapeShare.server.createShape(this.className.toLowerCase(), $('#group').val());

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

            //shapeShare.server.deleteAllShapes();

            return false;
        });
    $(document).keypress(function (e) {
        if (e.which == 100) {

            //shapeShare.server.deleteAllShapes();

            // enter pressed
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
        pre.innerHTML = out;
        document.body.appendChild(pre)
    }
    $(".toolbar_padding").click(function (event, ui) {
        var newid = $.fn.NewGUID();
        // 弹出元素编辑界面

        var templateid = $(this).attr('id').replace("Div", "");
        var color = $(this).css('backgroundColor');
        //mouse pointer start
        //var event = event || window.event; // IE-ism
        //var x = event.pageX;
        //var y = event.pageY;

        //shapeShare.server.newShape(newid, color, templateid, '210', '80');

    });
    $("#designpannel").droppable({
        drop: function (event, ui) {

            //                    var $movingDiv = $(ui.helper[0]);
            var moving_div = ui.draggable[0];
            var moving_div_templateid = moving_div.id;
            var moving_div_classname = ui.draggable[0].className;
            var color = moving_div.style.backgroundColor;
            //var str = "How are you doing today?";
            var res = moving_div_classname.split(" ");
            //dump(ui.draggable[0].style.backgroundColor);

            switch (res[0]) {
                case "toolbar_blue":
                    // 建立新元素
                    // 生成新元素id号，生成机制唯一，id号作为主键
                    var newid = $.fn.NewGUID();
                    // 弹出元素编辑界面

                    //shapeShare.server.newShape(newid);

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

                    //shapeShare.server.newShape(newid, color, templateid, x, y);

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
                                endpoint: "Blank",
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

                                  //shapeShare.server.settingTemplateData(t.id, localStorage["template-" + $("#" + t.id).attr("template-id")]);

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

                                          
                                          //console.log(jsonTempl);
                                          //console.log(jsonValue);

                                          //shapeShare.server.settingTemplateData(t.id, JSON.stringify(jsonTempl));
                                          //shapeShare.server.editNode(t.id, $("#itemLabel").val());

                                          $(this).dialog('close');
                                      }
                                  }
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

        els.contextMenu('myEndpointMenu',
             {
                 bindings:
                  {
                      'editEndpoint': function (t) {

                          //t.id
                          $("#linkLabel").val($("#" + t.id).html());
                          var tid = t.id;
                          var templatelinkid = "";
                          var linkid = "";
                          $.each(jsPlumb.getConnections(), function (idx, connection) {
                              //label.getOverlay("label").setLabel($("#itemLabel").val());
                              //$("#" + $elem.attr('id')).find('label')
                              //console.log(id + connection.overlays[1].canvas.id);
                              if (t.id === connection.overlays[1].canvas.id) {
                                  //console.log(id + " " + connection.overlays[1].canvas.id)
                                  //connection.overlays[1].setLabel(value);
                                  //console.log(connection.overlays[1].canvas.id);
                                  templatelinkid = connection.getParameter("templateid");
                                  //linkid = connection.getId();
                                  console.log(templatelinkid);
                              }
                              //console.log(connection.overlays[1].canvas.id);813
                          });
                          //console.log(templatelinkid);
                          //初始化link data
                          var templateLinkData = localStorage["linktemplate-" + templatelinkid];
                          console.log(templateLinkData);
                          if (typeof templateLinkData !== 'undefined' && templateLinkData !== null) {
                              if (localStorage[t.id] == null) {

                                  //shapeShare.server.settingTemplateData(t.id, localStorage["linktemplate-" + templatelinkid]);

                                  $('#tempLinkField').html(appendTempField(localStorage["linktemplate-" + templatelinkid]));
                              } else {
                                  $('#tempLinkField').html(appendTempField(localStorage[t.id]));
                                  console.log(appendTempField(localStorage[t.id]));
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

                                          //shapeShare.server.settingTemplateData(tid, JSON.stringify(jsonTempl));
                                          //shapeShare.server.editEP(tid, $("#linkLabel").val());

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
                      },
                      'deleteEndpoint': function (t) {
                          // 执行删除操作
                          //if (confirm('Are you sure that you want to permanently delete the selected element?')) {
                          //DragFlow.removeEndpoint(t);
                          //} 
                          ConfirmDeleteEP(t);
                      },
                      'save': function (t) {
                          // 保存流程操作
                          alert('Trigger was ' + t.id + '\nAction was Save');
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
                    shapeShare.server.deleteShape(courseID);
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
                    shapeShare.server.deleteConnection(courseID.id);
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
        conn.connection.setParameter("templateid", templateid);
        conn.connection.setPaintStyle({ strokeStyle: color, lineWidth: 2 });
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

        //shapeShare.server.connectNode(conn.sourceId, conn.targetId, $.connection.hub.id, color);

        //shapeShare.server.connectNode(conn.sourceId, conn.targetId);
        //alert(conn.sourceId+" "+conn.targetId);
    });

    $('.linkButton').on("click", function () {
        $('.linkButton').removeClass('active');
        $('#linktemplateID').val($(this).parent().attr('id').replace("Div", ""));
        $('#linktemplateColor').val($(this).css('color'));

        //shapeShare.server.setLinkTemplateIDcolor($(this).parent().attr('id').replace("Div", ""), $(this).css('color'));

        //changeConnectionColor($(this).css('color'));
        //var linkcolor = $(this).css('color');

        //    console.log($elem.attr('id') + " " + linkcolor);
        //    jsPlumb.makeSource($("#" + $elem.attr('id') + "").children(".ep"),
        //                 {
        //                     parent: $elem.attr('id'),
        //                     anchor: "Continuous",
        //                     connector: ["Flowchart", { curviness: 20 }],
        //                     connectorStyle: { strokeStyle: linkcolor, lineWidth: 2 },
        //                     endpoint: "Blank",
        //                     maxConnections: 5,
        //                     cssClass: "aLabel",
        //                     onMaxConnections: function (info, e) {
        //                         alert("Maximum connections (" + (info.maxConnections) + ") reached");
        //                     }
        //                 });
        ////    var $elem = $(elem);
        ////    console.log($elem);
        ////    DragFlow.makeSourceById($elem.attr('id'), $(this).css('color'));
        //console.log(rgb2hex($('#linktemplateColor').val()));
    });

    $("#DropDownList1").change(function () {
        $("#DropDownList1 option:selected").each(function () {
            //alert($(this).val());

            shapeShare.server.emptyPanel();
            shapeShare.server.loadConceptMap($(this).val());

        });
    });
    $.connection.hub.logging = true;
    $.connection.hub.start({ transport: activeTransport }, function () {
        shapeShare.server.join($.cookie("userName"))
                         .done(function () {
                             $.cookie("userName", shapeShare.state.user.Name, { expires: 30 });
                             $("#user").val(shapeShare.state.user.Name);
                             //$("#templateID").val("abc123");

                             //load();
                         });
    });

});