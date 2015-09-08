/// <reference path="jquery-1.4.1-vsdoc.js" />

(function ($) {
    // 初始化端点，把所有带有ep样式元素的父节点设置为源节点
    DragFlow.initEndpoints = function (nextColour) {
        $(".ep").each(function (i, e) {
			
            var p = $(e).parent();
            jsPlumb.makeSource($(e),
            {
                parent: p,
                //anchor:"BottomCenter",
                anchor: "Continuous",
                connector: ["Flowchart", { curviness: 20}],
				cssClass: "aLabel",
                connectorStyle: { strokeStyle: "black", lineWidth: 1 },
                endpoint: "Blank",
                maxConnections: 5,
                onMaxConnections: function (info, e) {
                    alert("Maximum connections (" + info.maxConnections + ") reached");
					
                }
            });
        });
    };
    // 设置元素为连接源节点
    DragFlow.makeSourceById = function (newid) {
		
        jsPlumb.makeSource($("#" + newid + "").children(".ep"),
                            {
                                parent: newid,
                                anchor: "Continuous",
                                connector: ["Flowchart", { curviness: 20}],
                                connectorStyle: { strokeStyle: "black", lineWidth: 1 },
                                endpoint: "Blank",
                                maxConnections: 5,
								cssClass: "aLabel",
                                onMaxConnections: function (info, e) {
                                    alert("Maximum connections (" + info.maxConnections + ") reached");
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
			$("#"+c.id).trigger( "click" );
    }
    // 设置元素右键菜单
    //DragFlow.makecontextmenu = function (newid, isAllElements) {
		
    //    var els = $("#" + newid);
    //    if (isAllElements) {
    //        els = $(".component");
    //    }

    //    els.contextMenu('myMenu1',
    //         {
    //             bindings:
    //              {
    //                  'edit': function (t) {
					  
    //                      // 弹出编辑页面
    //                      $("#itemLabel").val($("#"+t.id+" label").html());
	//					  // alert('Trigger was ' + t.id + '\nAction was Open');
	//					  // alert($('label[for="' + t.id + '"]').html());
    //                      $("#editDialog").dialog({
	//							buttons: {
	//								'確定': function() {
										
	//									$("#"+t.id).html('<label>'+$("#itemLabel").val()+'</label><div class="ep"></div>');
	//									$("#itemLabel").val("");
	//									$(this).dialog('close');
	//									DragFlow.initEndpoints("");
	//								}
	//							}
	//					  });
    //                  },
    //                  'delete': function (t) {
    //                      // 执行删除操作
    //                      $("#"+t.id).remove();
    //                  },
    //                  'save': function (t) {
    //                      // 保存流程操作
    //                      alert('Trigger was ' + t.id + '\nAction was Save');
    //                  }
    //              }
    //         });
    //}
	//DragFlow.makendpointercontextmenu = function (newid, isAllElements) {
		
    //    var els = $("#" + newid);
    //    if (isAllElements) {
    //        els = $(".aLabel");
    //    }

    //    els.contextMenu('myEndpointMenu',
    //         {
    //             bindings:
    //              {
    //                  'editEndpoint': function (t) {
					  
    //                      // 弹出编辑页面
    //                      $("#itemLabel").val($("#"+t.id).html());
	//					  // alert('Trigger was ' + t.id + '\nAction was Open');
	//					  // alert($('label[for="' + t.id + '"]').html());
    //                      $("#editDialog").dialog({
	//							buttons: {
	//								'確定': function() {
										
	//									$("#"+t.id).html($("#itemLabel").val());
	//									$("#itemLabel").val("");
	//									$(this).dialog('close');
										
	//								}
	//							}
	//					  });
    //                  },
    //                  'deleteEndpoint': function (t) {
    //                      // 执行删除操作
    //                    DragFlow.removeEndpoint(t);
    //                  },
    //                  'save': function (t) {
    //                      // 保存流程操作
    //                      alert('Trigger was ' + t.id + '\nAction was Save');
    //                  }
    //              }
    //         });
    //}

	// $( document ).on( "click", ".component", function() {
		
	// });


})(jQuery);