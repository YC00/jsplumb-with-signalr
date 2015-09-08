/// <reference path="../../Scripts/jquery-1.6.4.js" />
/// <reference path="../../Scripts/jquery-ui-1.8.12.js" />
/// <reference path="../../Scripts/jQuery.tmpl.js" />
/// <reference path="../../Scripts/jquery.cookie.js" />
/// <reference path="../../Scripts/signalR.js" />
$(function () {
    'use strict';

    var shapeShare = $.connection.DragFlow;

    // 设置toolbar中所有元素可拖拽
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
    // designpannel设计面板中的填充事件，通过它编辑元素
    $("#designpannel").droppable({
        drop: function (event, ui) {

            //                    var $movingDiv = $(ui.helper[0]);
            var moving_div_id = ui.draggable[0].id;
            switch (moving_div_id) {
                case "toolbar_blue":
                    // 建立新元素
                    // 生成新元素id号，生成机制唯一，id号作为主键
                    var newid = $.fn.NewGUID();
                    // 弹出元素编辑界面

                    //shapeShare.server.createShape(newid);
                    
                    //            switch (this.className.toLowerCase()) {
                    //                case "square":
                    //                    shapeShare.createShape("square");
                    //                    break;
                    //                case "picture":
                    //                    shapeShare.createShape("picture");
                    //                    break;
                    //            }

                    return false;


                    break;
                case "toolbar_yellow":
                    // 建立新元素
                    // 生成新元素id号，生成机制唯一，id号作为主键
                    var newid = $.fn.NewGUID();
                    // 弹出元素编辑界面
                    $(this).append('<div id="' + newid + '" class="gradient_yellow component window" style="top:60px;left:30px;"><label>Node Label</label><div class="ep"> </div></div>');

                    // 设置元素为连接源节点
                    DragFlow.makeSourceById(newid);
                    // 设置元素为可拖拽
                    jsPlumb.draggable(newid);
                    // 设置连接目标节点
                    DragFlow.makeTargetById(newid);
                    // 设置新元素的右键菜单
                    DragFlow.makecontextmenu(newid, false);
                    break;
            }
        }
    });

    var blocks = []
    $("#designpannel .component").each(function (idx, elem) {
        var $elem = $(elem);
        blocks.push({
            blockId: $elem.attr('id'),
            positionX: parseInt($elem.css("left"), 10),
            positionY: parseInt($elem.css("top"), 10)
        });
    });

    var serializedData = JSON.stringify(blocks);


    //$('#jsonData').html(serializedData);



    $('#update').click(function () {

        var connections = [];
        $.each(jsPlumb.getConnections(), function (idx, connection) {
            connections.push({
                connectionId: connection.id,
                pageSourceId: connection.sourceId,
                pageTargetId: connection.targetId
            });
        });

        var serializedConnection = JSON.stringify(connections);

        $('#jsonConnection').html(serializedConnection);
    });

    shapeShare.client.shapeAdded = function (newid) {
        $(this).append('<div id="' + newid + '" class="gradient_blue component window" style="top:60px;left:30px;">  <label>Node Label</label><div class="ep"> </div></div>');
        // 设置元素为连接源节点
        DragFlow.makeSourceById(newid);
        // 设置元素为可拖拽
        jsPlumb.draggable(newid);
        // 设置连接目标节点
        DragFlow.makeTargetById(newid);
        // 设置新元素的右键菜单
        DragFlow.makecontextmenu(newid, false);
       
    };
});
