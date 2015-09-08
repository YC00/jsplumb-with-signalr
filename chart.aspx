<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chart.aspx.cs" Inherits="Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.chart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="bootstrap/css/bootstrap.css" rel="stylesheet" type="text/css" />
     <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="bootstrap3/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Chart ID="Chart1" runat="server">
                            <Series>
                                <asp:Series Name="Series_1"></asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
    </div>
         <div>
                        <asp:Table runat="server" ID="table" class="table table-striped">
                            
                             
                        </asp:Table>
                    </div>
    </form>
</body>
</html>
