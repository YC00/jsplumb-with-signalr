using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class chart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int match = 0;
                int partialmatch = 0;
                int mismatch = 0;
                int additional = 0;
                if (Request.QueryString["m"] != "")
                    match = Int32.Parse(Request.QueryString["m"]);
                if (Request.QueryString["pm"] != "")
                    partialmatch = Int32.Parse(Request.QueryString["pm"]);
                if (Request.QueryString["mm"] != "")
                    mismatch = Int32.Parse(Request.QueryString["mm"]);
                if (Request.QueryString["ad"] != "")
                    additional = Int32.Parse(Request.QueryString["ad"]);

                int total = match + partialmatch + mismatch + additional;

                // Creating the series 
                Series series1 = new Series("Series1");

                // Setting the Chart Types
                series1.ChartType = SeriesChartType.Column;
                Chart1.Series["Series_1"].IsXValueIndexed = true;
                // Adding some points
                series1.Points.AddXY("Match", match);
                series1.Points.AddXY("Partial Match", partialmatch);
                series1.Points.AddXY("Mismatch", mismatch);
                series1.Points.AddXY("Additional", additional);
                series1.Points[0].Color = System.Drawing.Color.Green;
                series1.Points[1].Color = System.Drawing.Color.LightGreen;
                series1.Points[2].Color = System.Drawing.Color.Red;
                series1.Points[3].Color = System.Drawing.Color.Yellow;
                Chart1.Series.Add(series1);

                // Series visual
                series1.YValueMembers = "Frequency";
                series1.XValueMember = "RoundedValue";
                series1.BorderWidth = 1;
                series1.ShadowOffset = 0;
                series1.Color = System.Drawing.Color.Red;
                series1.IsXValueIndexed = true;

                Chart1.Height = 300;
                Chart1.Width = 600;
                // Setting the X Axis
                Chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;
                Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
                Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Double.NaN;
                Chart1.ChartAreas["ChartArea1"].AxisX.Title = "";
                Chart1.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("Sans Serif", 10, FontStyle.Bold);

                // Setting the Y Axis
                Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 2;
                Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Double.NaN;
                Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Numbers";
                Chart1.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("Sans Serif", 10, FontStyle.Bold);

                TableRow row0 = new TableRow();
                TableCell rcell1 = new TableCell();
                TableCell rcell2 = new TableCell();
                TableCell rcell3 = new TableCell();

                rcell1.Text = "Categories";
                row0.Cells.Add(rcell1);
                rcell2.Text = "Numbers";
                row0.Cells.Add(rcell2);
                rcell3.Text = "Rates";
                row0.Cells.Add(rcell3);
                table.Rows.Add(row0);

                TableRow row1 = new TableRow();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TableCell cell3 = new TableCell();

                cell1.Text = "Match";
                row1.Cells.Add(cell1);
                cell2.Text = match.ToString();
                row1.Cells.Add(cell2);
                int percentComplete = 0;
                if (match > 0)
                {
                    percentComplete = (int)Math.Round((double)(100 * match) / total);
                    cell3.Text = percentComplete.ToString() + "%";
                }
                else
                cell3.Text = "0%";
                row1.Cells.Add(cell3);
                table.Rows.Add(row1);

                TableRow row2 = new TableRow();
                TableCell pmcell1 = new TableCell();
                TableCell pmcell2 = new TableCell();
                TableCell pmcell3 = new TableCell();

                pmcell1.Text = "Partial Match";
                row2.Cells.Add(pmcell1);
                pmcell2.Text = partialmatch.ToString();
                row2.Cells.Add(pmcell2);
                if (partialmatch > 0)
                {
                    percentComplete = (int)Math.Round((double)(100 * partialmatch) / total);
                    pmcell3.Text = percentComplete.ToString() + "%";
                }
                else
                    pmcell3.Text = "0%";
                row2.Cells.Add(pmcell3);
                table.Rows.Add(row2);

                TableRow row3 = new TableRow();
                TableCell mmcell1 = new TableCell();
                TableCell mmcell2 = new TableCell();
                TableCell mmcell3 = new TableCell();


                mmcell1.Text = "Mismatch";
                row3.Cells.Add(mmcell1);
                mmcell2.Text = mismatch.ToString();
                row3.Cells.Add(mmcell2);
                if (mismatch > 0)
                {
                    percentComplete = (int)Math.Round((double)(100 * mismatch) / total);
                    mmcell3.Text = percentComplete.ToString() + "%";
                }else
                mmcell3.Text = "0%";
                row3.Cells.Add(mmcell3);
                table.Rows.Add(row3);

                TableRow row4 = new TableRow();
                TableCell acell1 = new TableCell();
                TableCell acell2 = new TableCell();
                TableCell acell3 = new TableCell();


                acell1.Text = "Additional";
                row4.Cells.Add(acell1);
                acell2.Text = additional.ToString();
                row4.Cells.Add(acell2);
                if (additional > 0)
                {
                    percentComplete = (int)Math.Round((double)(100 * additional) / total);
                    acell3.Text = percentComplete.ToString() + "%";
                }
                else
                acell3.Text = "0%";
                row4.Cells.Add(acell3);
                table.Rows.Add(row4);
            }
        }
    }
}