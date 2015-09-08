/// <reference path="jquery-1.4.1-vsdoc.js" />
/*
*  DragFlow初始化类，主要包括：
*  1、初始化流程默认显示样式；
*  2、解析流程步骤及步骤关系；
*  3、设置所有元素的可拖拽性、目标节点、源节点等
*  4、设置右键菜单；
*  Author:limq
*  date:2012.12.08
*/
(function () {

    window.DragFlow = {

        init: function () {
            // jsplum默认样式
            jsPlumb.importDefaults({
                DragOptions: { cursor: "pointer", zIndex: 2000 },
                HoverClass: "connector-hover",
                HoverPaintStyle: { strokeStyle: "#7ec3d9" }
            //    PaintStyle: {
            //        lineWidth: 2,
            //        strokeStyle: "black"
            //    },
            //    Overlays: [["PlainArrow", { location: 1, width: 10, length: 12}]],
			//	ConnectionOverlays : [
			//		[ "Arrow", { 
			//			location:1,
			//			id:"arrow",
	        //            length:14,
	        //            foldback:0.8
			//		} ]
	        //        , ["Label", {
	        //            label: "label", id: "label", cssClass: "aLabel",
	        //            events: {
	        //                dblclick: function (labelOverlay, originalEvent) {
	        //                    alert(labelOverlay.component.getOverlay("label").getLabel());
	        //                }
	        //            }
	        //        }]
			//	]
				 
            });
            jsPlumb.Defaults.Overlays = [
                ["PlainArrow", {
                    location: 1,
                    id: "arrow",
                    width: 10,
                    length: 12
                }]
            ];
            //var connectorStrokeColor = "rgba(50, 50, 200, 1)",
			//	connectorHighlightStrokeColor = "rgba(180, 180, 200, 1)",
			//	hoverPaintStyle = { strokeStyle: "#7ec3d9" }; 		// hover paint style is merged on normal style, so you 
            // don't necessarily need to specify a lineWidth

            //            var connection1 = jsPlumb.connect({
            //                source: "window1",
            //                target: "window2",
            //                connector: "Flowchart",
            //                cssClass: "c1",
            //                endpoint: "Blank",
            //                endpointClass: "c1Endpoint",
            //                anchor: "Continuous",
            //                paintStyle: {
            //                    lineWidth: 1,
            //                    strokeStyle: "black"
            //                },

            //                endpointStyle: { fillStyle: "#a7b04b" },
            //                hoverPaintStyle: hoverPaintStyle,
            //                overlays: [
            //			   				["PlainArrow", {
            //			   				    cssClass: "l1arrow",
            //			   				    location: 1, width: 10, length: 12,
            //			   				    events: {
            //			   				        "click": function (arrow, evt) {
            //			   				            alert("clicked on arrow for connection " + arrow.component.id);
            //			   				        },
            //			   				        id: "myArrow"
            //			   				    }
            //			   				}]
            //			]
            //            });

            //            var connectiontest = jsPlumb.connect({
            //                source: "window_big1",
            //                target: "window3",
            //                connector: "Flowchart",
            //                cssClass: "c1",
            //                endpoint: "Blank",
            //                endpointClass: "c1Endpoint",
            //                anchors: [[1, 0.12, 0, 0.1], "LeftMiddle"],
            //                overlays: [["PlainArrow", { location: 1, width: 10, length: 12}]],
            //                paintStyle: {
            //                    lineWidth: 1,
            //                    strokeStyle: "black"
            //                },
            //                endpointStyle: { fillStyle: "#a7b04b" },
            //                hoverPaintStyle: hoverPaintStyle
            //            });

            //            var stateMachineConnector = {
            //                connector: "Flowchart",
            //                paintStyle: { lineWidth: 1, strokeStyle: "black" },
            //                hoverPaintStyle: { strokeStyle: "#dbe300" },
            //                endpoint: "Blank",
            //                anchor: "Continuous",
            //                overlays: [["PlainArrow", { location: 1, width: 10, length: 12}]]
            //            };

            //            jsPlumb.connect({
            //                source: "window3",
            //                target: "window4"
            //            }, stateMachineConnector);

            //            jsPlumb.connect({
            //                source: "window4",
            //                target: "window_big2",
            //                anchors: ["TopCenter", "LeftMiddle"]
            //            }, stateMachineConnector);

            //            jsPlumb.connect({
            //                source: "window5",
            //                target: "window6"
            //            }, stateMachineConnector);
            //            jsPlumb.connect({
            //                source: "window7",
            //                target: "window8"
            //            }, stateMachineConnector);
            //            jsPlumb.connect({
            //                source: "window9",
            //                target: "window10"
            //            }, stateMachineConnector);
            //            jsPlumb.connect({
            //                source: "window4",
            //                target: "window7",
            //                anchors: ["BottomCenter", "LeftMiddle"]
            //            }, stateMachineConnector);
            //            jsPlumb.connect({
            //                source: "window4",
            //                target: "window9",
            //                anchors: ["BottomCenter", "LeftMiddle"]
            //            }, stateMachineConnector);

            // double click on any connection 
            //            jsPlumb.bind("dblclick", function (connection, originalEvent) { alert("double click on connection from " + connection.sourceId + " to " + connection.targetId); });
            // single click on any endpoint
            //            jsPlumb.bind("endpointClick", function (endpoint, originalEvent) { alert("click on endpoint on element " + endpoint.elementId); });
            // context menu (right click) on any component.
            
            //20140114
            
			
           
            // 链接双击事件，在此事件中加入删除链接的逻辑
            //jsPlumb.bind("dblclick", function (c) {
				//console.log(c);
            //    jsPlumb.detach(c);
            //});
			
			
            // hand off to the library specific demo code here.  not my ideal, but to write common code
            // is less helpful for everyone, because all developers just like to copy stuff, right?
            // make each ".ep" div a source and give it some parameters to work with.  here we tell it
            // to use a Continuous anchor and the StateMachine connectors, and also we give it the
            // connector's paint style.  note that in this demo the strokeStyle is dynamically generated,
            // which prevents us from just setting a jsPlumb.Defaults.PaintStyle.  but that is what i
            // would recommend you do.
            DragFlow.initEndpoints("");
            // 设置所有节点为连接目标节点
            //jsPlumb.makeTarget(jsPlumb.getSelector(".component"), {
            //    dropOptions: { hoverClass: "dragHover" },
            //    anchor: "Continuous",
            //    endpoint: "Blank"
            //    //anchor:"TopCenter"			
            //});

            // make all .window divs draggable
            jsPlumb.draggable(jsPlumb.getSelector(".component"));
			//var conn1 = jsPlumb.connect({ source:"window6", target:"window2" });
            //            $(jsPlumb.getSelector(".window")).draggable(
            //            {
            //                start: function (event, ui)
            //                {

            //                },
            //                drag: function ()
            //                {


            //                },
            //                stop: function (event, ui)
            //                {
            //                    var $movingDiv = $(ui.helper[0]);
            //                    var realtop = $movingDiv.position().top;
            //                    //                    alert(realtop);
            //                    // 重新画
            //                    jsPlumb.repaintEverything();
            //                }
            //            }
            //        );

			
            // 设置所有节点的右键菜单
			//jsPlumb.setAutomaticRepaint(true);
            //DragFlow.makecontextmenu("", true);
            //DragFlow.makendpointercontextmenu("", true);
			

        }
		
    }; 

})();