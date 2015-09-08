using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Text;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace Microsoft.AspNet.SignalR.Samples.Hubs
{
    public partial class testbrowsingtool : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            // Creating the series 
            Series series1 = new Series("Series1");

            // Setting the Chart Types
            series1.ChartType = SeriesChartType.Column;
            Chart1.Series["Series_1"].IsXValueIndexed = true;
            // Adding some points
            series1.Points.AddXY("Match", 12);
            series1.Points.AddXY("Partial Match", 1);
            series1.Points.AddXY("Mismatch", 3);
            series1.Points.AddXY("Additional", 2);
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

            if (!IsPostBack)
            {

                loadDropDown();
                loadNodeTemplateToolbar();
                loadLinkTemplateToolbar();
                //for (int i = 0; i < Session.Count; i++)
                //{
                //    var crntSession = Session.Keys[i];
                //    Response.Write(string.Concat(crntSession, "=", Session[crntSession]) + "<br />");
                //}
            }
        }
        public void loadDropDown()
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from ConceptMap_Info", con))
                {

                    try
                    {
                        con.Open();
                        DataTable dt = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            browsingStudentList.DataSource = dt;
                            browsingStudentList.DataValueField = dt.Columns["CID"].ToString();
                            browsingStudentList.DataTextField = dt.Columns["CName"].ToString();
                            browsingStudentList.DataBind();
                            browsingStudentList.Items.Insert(0, "");

                            browsingTeacherList.DataSource = dt;
                            browsingTeacherList.DataValueField = dt.Columns["CID"].ToString();
                            browsingTeacherList.DataTextField = dt.Columns["CName"].ToString();
                            browsingTeacherList.DataBind();
                            browsingTeacherList.Items.Insert(0, "");
                        }
                        con.Close();
                    }
                    catch (SqlException ex)
                    {
                        // output the error to see what's going on
                        //return ex.Message.ToString();
                    }
                }
            }
        }
        public void loadNodeTemplateToolbar()
        {
            //if (HttpContext.Current.Application["nodeTemplate"] != null)
            //{
            //    var nodeTList = (List<NodeManagement.NodeTemplate>)Application["nodeTemplate"];
            //    var js = "<script type='text/javascript'>";
            //    foreach (var nodeTItems in nodeTList)
            //    {
            //        js += "localStorage.setItem('template-" + nodeTItems.templateID + "','" + nodeTItems.templateData + "');";
            //        nodeTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li style=\"\"><div style=\"width:120px;margin-left:-20px;\">" + nodeTItems.templateName + "</div></li><li><div id=\"Div" + nodeTItems.templateID + "\" class=\"toolbar_padding toolbar_blue divtooltip\" style=\"width: 75px; background-color:#" + nodeTItems.color + "\" title=\"" + Server.HtmlEncode(loadNodeTemplate(nodeTItems.templateID)) + "\"></div></li>";
            //    }

            //    js += "inactiveLinkButton();</script>";
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "tmp", js, false);
            //}
            //else
            //{
            //    nodeTemplatePanel.Text += "<li><div id=\"\" class=\"\" style=\"width: 75px;\"><img src=\"images/play.jpg\" id=\"playbtn\" style=\"cursor:pointer;\"/></div></li>";
            //}
        }
        public void loadLinkTemplateToolbar()
        {
            //if (HttpContext.Current.Application["linkTemplate"] != null)
            //{

            //    var linkTList = (List<LinkManagement.NodeTemplate>)Application["linkTemplate"];
            //    var js = "<script type='text/javascript'>";
            //    linkTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li style=\"\"><div style=\"width:120px;margin-left:0px;\">Default</div></li><li><div id=\"Div0\" class=\"divtooltip\" style=\"width: 75px; color: black;\"><button type=\"button\" class=\"btn btn-default btn-lg linkButton active\" data-toggle=\"button\" style=\"padding-bottom: 15px;font-size:40px;border: 1px solid #757575;color:#808080\">→</button></div></li>";
            //    foreach (var linkTItems in linkTList)
            //    {
            //        js += "localStorage.setItem('linktemplate-" + linkTItems.templateID + "','" + linkTItems.templateData + "');";
            //        linkTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li style=\"\"><div style=\"width:120px;margin-left:-20px;\">" + linkTItems.templateName + "</div></li><li><div id=\"Div" + linkTItems.templateID + "\" class=\"divtooltip\" style=\"width: 75px;\" title=\"" + Server.HtmlEncode(loadLinkTemplate(linkTItems.templateID)) + "\"> <button type=\"button\" class=\"btn btn-default btn-lg linkButton\" data-toggle=\"button\" style=\"padding-bottom: 15px;font-size:40px; border: 1px solid #757575;color:#" + linkTItems.color + "\">&rarr;</button></div></li>";
            //    }
            //    js += "</script>";
            //    //Response.Write(js);
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "tmplink", js, false);
            //}
            //else
            //{
            //    linkTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li><div id=\"Div0\" class=\"divtooltip\" style=\"width: 75px; color: black;\"><img src=\"images/refresh.jpg\" id=\"refreshbtn\" style=\"cursor:pointer;\"/></div></li>";
            //}
        }
        public string loadNodeTemplate(string templateID)
        {
            string strQuery = "select FieldName, FieldValue" +
                                  " from ConceptMap_TemplateNode " +
                                  "where " +
                                  "ConceptMap_TemplateNode.TemplateID=" + templateID;

            SqlCommand cmd = new SqlCommand(strQuery);

            SqlConnection con = new SqlConnection(strConnString);
            //建立SQL命令對象
            con.Open();

            cmd = new SqlCommand(strQuery);

            //GridView2.DataSource = GetData(cmd);
            //GridView2.DataBind();

            GridView grdv = new GridView();
            grdv.DataSource = GetData(cmd);
            grdv.CssClass = "table table-bordered";
            grdv.DataBind();
            StringBuilder htmlBody = new StringBuilder();
            StringWriter sw = new StringWriter(htmlBody);
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdv.RenderControl(hw);
            con.Close();
            return htmlBody.ToString();

        }

        public string loadLinkTemplate(string templateID)
        {
            string strQuery = "select FieldName, FieldValue" +
                                  " from ConceptMap_TemplateLinkData " +
                                  "where " +
                                  "ConceptMap_TemplateLinkData.TemplateID=" + templateID;

            SqlCommand cmd = new SqlCommand(strQuery);

            SqlConnection con = new SqlConnection(strConnString);
            //建立SQL命令對象
            con.Open();

            cmd = new SqlCommand(strQuery);

            //GridView2.DataSource = GetData(cmd);
            //GridView2.DataBind();

            GridView grdv = new GridView();
            grdv.DataSource = GetData(cmd);
            grdv.CssClass = "table table-bordered";
            grdv.DataBind();
            StringBuilder htmlBody = new StringBuilder();
            StringWriter sw = new StringWriter(htmlBody);
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdv.RenderControl(hw);
            con.Close();
            return htmlBody.ToString();

        }
        private DataTable GetData(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(strConnString);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            con.Open();
            sda.SelectCommand = cmd;
            sda.Fill(dt);
            con.Close();
            return dt;
        }
        public class NodeValue
        {
            public string NodeID { get; set; }
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
        }
        [WebMethod]
        public static string InsertDefaultNodeValue(List<NodeValue> connections)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                foreach (var c in connections)
                {
                    using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_NodeApplication(NodeID,FieldName,FieldValue) VALUES(@NodeID,@FieldName,@FieldValue)", con))
                    {
                        try
                        {
                            con.Open();

                            cmd.Parameters.AddWithValue("@NodeID", c.NodeID);
                            cmd.Parameters.AddWithValue("@FieldName", c.FieldName);
                            cmd.Parameters.AddWithValue("@FieldValue", c.FieldValue);
                            int i = cmd.ExecuteNonQuery();
                            if (i == 1)
                            {
                                msg = "true";
                            }
                            else
                            {
                                msg = "false";
                            }

                            con.Close();


                        }
                        catch (SqlException ex)
                        {
                            // output the error to see what's going on
                            return ex.Message.ToString();
                        }
                    }
                }
            }
            return msg;
        }
        public class Connections
        {
            public string CID { get; set; }
            public string connectionId { get; set; }
            public string pageSourceId { get; set; }
            public string pageTargetId { get; set; }
            public string label { get; set; }
        }
        [WebMethod]
        public static string NewInsertConnectionData(List<Connections> connections)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                foreach (var c in connections)
                {
                    using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_Link(CID,LinkID,StartNode,EndNode,LinkLabel,CreatedDateTime) VALUES(@CID,@connectionId,@pageSourceId,@pageTargetId,@label,@createdDateTime)", con))
                    {
                        try
                        {
                            con.Open();

                            cmd.Parameters.AddWithValue("@CID", c.CID);
                            cmd.Parameters.AddWithValue("@connectionId", c.connectionId);
                            cmd.Parameters.AddWithValue("@pageSourceId", c.pageSourceId);
                            cmd.Parameters.AddWithValue("@pageTargetId", c.pageTargetId);
                            cmd.Parameters.AddWithValue("@label", c.label);
                            cmd.Parameters.AddWithValue("@createdDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            int i = cmd.ExecuteNonQuery();
                            if (i == 1)
                            {
                                msg = "true";
                            }
                            else
                            {
                                msg = "false";
                            }

                            con.Close();


                        }
                        catch (SqlException ex)
                        {
                            // output the error to see what's going on
                            return ex.Message.ToString();
                        }
                    }
                }
            }
            return msg;
        }
        [WebMethod]
        public static string InsertConnectionData(string CID, string connectionId, string pageSourceId, string pageTargetId, string label)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_Link(CID,LinkID,StartNode,EndNode,LinkLabel,CreatedDateTime) VALUES(@CID,@connectionId,@pageSourceId,@pageTargetId,@label,@createdDateTime)", con))
                {

                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@CID", CID);
                        cmd.Parameters.AddWithValue("@connectionId", connectionId);
                        cmd.Parameters.AddWithValue("@pageSourceId", pageSourceId);
                        cmd.Parameters.AddWithValue("@pageTargetId", pageTargetId);
                        cmd.Parameters.AddWithValue("@label", label);
                        cmd.Parameters.AddWithValue("@createdDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        int i = cmd.ExecuteNonQuery();
                        con.Close();
                        if (i == 1)
                        {
                            msg = "true";
                        }
                        else
                        {
                            msg = "false";
                        }

                    }
                    catch (SqlException ex)
                    {
                        // output the error to see what's going on
                        return ex.Message.ToString();
                    }
                }
            }
            return msg;
        }
        public class Nodes
        {
            public string CID { get; set; }
            public string nodeID { get; set; }
            public string nodeLabel { get; set; }
            public string positionX { get; set; }
            public string positionY { get; set; }
            public string width { get; set; }
            public string height { get; set; }
            public string color { get; set; }
        }

        [WebMethod]
        public static string NewInsertNodeData(List<Nodes> nodes)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                foreach (var n in nodes)
                {
                    using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_Node(CID,NodeID,NodeLabel,Xcoordinate,Ycoordinate,Width,Height,Color,CreatedDateTime) VALUES(@CID,@nodeID,@nodeLabel,@positionX,@positionY,@width,@height,@color,@createdDateTime)", con))
                    {

                        try
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@CID", n.CID);
                            cmd.Parameters.AddWithValue("@nodeID", n.nodeID);
                            cmd.Parameters.AddWithValue("@nodeLabel", n.nodeLabel);
                            cmd.Parameters.AddWithValue("@positionX", n.positionX);
                            cmd.Parameters.AddWithValue("@positionY", n.positionY);
                            cmd.Parameters.AddWithValue("@width", n.width);
                            cmd.Parameters.AddWithValue("@height", n.height);
                            cmd.Parameters.AddWithValue("@color", n.color);
                            cmd.Parameters.AddWithValue("@createdDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            int i = cmd.ExecuteNonQuery();
                            con.Close();
                            if (i == 1)
                            {
                                msg = "true";
                            }
                            else
                            {
                                msg = "false";
                            }

                        }
                        catch (SqlException ex)
                        {
                            // output the error to see what's going on
                            return ex.Message.ToString();
                        }
                    }
                }
            }
            return msg;
        }

        public class NodeFields
        {
            public string nodeID { get; set; }
            public string templateID { get; set; }
            public string fieldName { get; set; }
            public string fieldValue { get; set; }
            public string type { get; set; }
            public string show { get; set; }
        }

        [WebMethod]
        public static string InsertNodeFieldsData(List<NodeFields> nodeFields)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                foreach (var n in nodeFields)
                {
                    using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_NodeFieldsData" +
                        "(NodeID,TemplateID,FieldName,FieldValue,Type,Show,CreatedDateTime) " +
                        "VALUES(@nodeID,@templateID,@fieldName,@fieldValue,@type,@show,@createdDateTime)", con))
                    {

                        try
                        {
                            con.Open();
                            cmd.Parameters.AddWithValue("@nodeID", n.nodeID);
                            cmd.Parameters.AddWithValue("@templateID", n.templateID);
                            cmd.Parameters.AddWithValue("@fieldName", n.fieldName);
                            cmd.Parameters.AddWithValue("@fieldValue", n.fieldValue);
                            cmd.Parameters.AddWithValue("@type", n.type);
                            cmd.Parameters.AddWithValue("@show", n.show);
                            cmd.Parameters.AddWithValue("@createdDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            int i = cmd.ExecuteNonQuery();
                            con.Close();
                            if (i == 1)
                            {
                                msg = "true";
                            }
                            else
                            {
                                msg = "false";
                            }

                        }
                        catch (SqlException ex)
                        {
                            // output the error to see what's going on
                            return ex.Message.ToString();
                        }
                    }
                }
            }
            return msg;
        }

        [WebMethod]
        public static string InsertNodeData(string CID, string nodeID, string nodeLabel, string positionX, string positionY, string width, string height)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_Node(CID,NodeID,NodeLabel,Xcoordinate,Ycoordinate,Width,Height,CreatedDateTime) VALUES(@CID,@nodeID,@nodeLabel,@positionX,@positionY,@width,@height,@createdDateTime)", con))
                {

                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@CID", CID);
                        cmd.Parameters.AddWithValue("@nodeID", nodeID);
                        cmd.Parameters.AddWithValue("@nodeLabel", nodeLabel);
                        cmd.Parameters.AddWithValue("@positionX", positionX);
                        cmd.Parameters.AddWithValue("@positionY", positionY);
                        cmd.Parameters.AddWithValue("@width", width);
                        cmd.Parameters.AddWithValue("@height", height);
                        cmd.Parameters.AddWithValue("@createdDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        int i = cmd.ExecuteNonQuery();
                        con.Close();
                        if (i == 1)
                        {
                            msg = "true";
                        }
                        else
                        {
                            msg = "false";
                        }

                    }
                    catch (SqlException ex)
                    {
                        // output the error to see what's going on
                        return ex.Message.ToString();
                    }
                }
            }
            return msg;
        }

        [WebMethod]
        public static string CreateConceptMap(string CName, string SystemName, string SystemID)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_Info(CName,SystemName,SystemID) VALUES(@CName,@SystemName,@SystemID);SELECT SCOPE_IDENTITY();", con))
                {

                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@CName", CName);
                        cmd.Parameters.AddWithValue("@SystemName", SystemName);
                        cmd.Parameters.AddWithValue("@SystemID", SystemID);
                        msg = cmd.ExecuteScalar().ToString();

                        con.Close();

                    }
                    catch (SqlException ex)
                    {
                        // output the error to see what's going on
                        return ex.Message.ToString();
                    }
                }
            }
            return msg;
        }
        [WebMethod]
        public static void MyMethod(string id1, string id2, string id3, string keyword1, string keyword2, string keyword3)
        {
            //// Singleton instance
            //int i = 1;
            //IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<ShapeShare>();
            //if (keyword1 != "")
            //    _context.Clients.All.keywordShapeCreated(id1, i++.ToString() + ". " + keyword1, "0", "130");
            //if (keyword2 != "")
            //    _context.Clients.All.keywordShapeCreated(id2, i++.ToString() + ". " + keyword2, "10", "140");
            //if (keyword3 != "")
            //    _context.Clients.All.keywordShapeCreated(id3, i.ToString() + ". " + keyword3, "20", "150");
            //_context.Clients.All.shapeCreated("71121213123123137");
            // This method is invoked by a Timer object.

            // return some data here
            //ScriptManager.RegisterStartupScript(pThis.Page, pThis.GetType(), "tmp", "<script type='text/javascript'>shapeShare.server.newKeywordShape(\"77\", \"123\", \"abc\", \"xyz\", \"\", \"0\", \"130\");</script>", false);
            //ScriptManager.RegisterStartupScript("OnLoad", "<script>shapeShare.server.newKeywordShape(\"77\", \"123\", \"abc\", \"xyz\", \"\", \"0\", \"130\");</script>");
            //sendKeyword();
            //return "";
        }
        public static string DataTableToJSON(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];

            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\":\"" + StrDc[i] + i.ToString() + "¾" + "\",";

            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();

            Sb.Append("[");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;

                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().Trim());
                }
                //Sb.AppendFormat("{{{0}}},",TempStr);

                Sb.Append("{" + TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            if (Sb.ToString().Length > 0)
                Sb.Append("]");

            return StripControlChars(Sb.ToString());

        }
        //To strip control characters:

        //A character that does not represent a printable character but //serves to initiate a particular action.

        public static string StripControlChars(string s)
        {
            return Regex.Replace(s, @"[^\x20-\x7F]", "");
        }
        public static void setTemplData(string templateid, string templatedata)
        {
            //IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<ShapeShare>();
            //_context.Clients.All.setTemplateData(templateid, templatedata);
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}