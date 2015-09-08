<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newnode.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.jqueryget" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
    <script src="//code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>
<script>
    function getParameterByName(name) //courtesy Artem
    {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
    (function ($) {
        $.fn.NewGUID = function () {
            var guid = (G() + G() + "-" + G() + "-" + G() + "-" +G() + "-" + G() + G() + G()).toUpperCase();
            return guid;
        };
        function G() {

            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1)

        };

    })(jQuery);
    $(document).ready(function () {
        var id1 = $.fn.NewGUID();
        var id2 = $.fn.NewGUID();
        var id3 = $.fn.NewGUID();
        //function sendkeyword() {
        var keyword1 = getParameterByName("keyword1");
        var keyword2 = getParameterByName("keyword2");
        var keyword3 = getParameterByName("keyword3");
        var groupID = getParameterByName("groupID");
        //var chairman = getParameterByName("chairman");
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "http://140.116.72.19/shape/Hubs/ShapeShare/DragFlow.aspx/MyMethod",
            data: '{ "id1": "' + String(id1) + '","id2": "' + String(id2) + '","id3": "' + String(id3) + '","keyword1": "' + String(keyword1) + '", "keyword2": "' + String(keyword2) + '", "keyword3": "' + String(keyword3) + '", "groupid": "' + groupID + '" }',
            dataType: "json",
            success: function (response) {
                //alert(response);
            },
            error: function(response){
                //alert(response);
            }
        });
    });
   // }
    //sendkeyword("123","123","123");
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
