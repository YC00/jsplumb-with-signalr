<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testautocomplete.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.testautocomplete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>jQuery UI Autocomplete - Default functionality</title>  <link rel="stylesheet" href="//code.jquery.com/ui/1.11.0/themes/smoothness/jquery-ui.css">  <script src="//code.jquery.com/jquery-1.10.2.js"></script>  <script src="//code.jquery.com/ui/1.11.0/jquery-ui.js"></script>  <link rel="stylesheet" href="/resources/demos/style.css">  <script>      $(function () {
          var availableTags = [            "ActionScript",            "AppleScript",            "Asp",            "BASIC",            "C",            "C++",            "Clojure",            "COBOL",            "ColdFusion",            "Erlang",            "Fortran",            "Groovy",            "Haskell",            "Java",            "JavaScript",            "Lisp",            "Perl",            "PHP",            "Python",            "Ruby",            "Scala",            "Scheme"          ];          $("#tags").autocomplete({
              source: 'jsonkeyword.aspx',              minLength: 0
          }).focus(function () {
              $(this).autocomplete("search");
          });
      });  </script>
</head>
<body>
    <form id="form1" runat="server">
   <div class="ui-widget">  <label for="tags">Tags: </label>  <input id="tags"></div> 
    </form>
</body>
</html>
