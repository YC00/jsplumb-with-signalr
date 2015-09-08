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
using System.Collections;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class BrowsingTool : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //// Creating the series 
            //Series series1 = new Series("Series1");

            //// Setting the Chart Types
            //series1.ChartType = SeriesChartType.Column;
            //Chart1.Series["Series_1"].IsXValueIndexed = true;
            //// Adding some points
            //series1.Points.AddXY("Match", 12);
            //series1.Points.AddXY("Partial Match", 1);
            //series1.Points.AddXY("Mismatch", 3);
            //series1.Points.AddXY("Additional", 2);
            //series1.Points[0].Color = System.Drawing.Color.Green;
            //series1.Points[1].Color = System.Drawing.Color.LightGreen;
            //series1.Points[2].Color = System.Drawing.Color.Red;
            //series1.Points[3].Color = System.Drawing.Color.Yellow;
            //Chart1.Series.Add(series1);

            //// Series visual
            //series1.YValueMembers = "Frequency";
            //series1.XValueMember = "RoundedValue";
            //series1.BorderWidth = 1;
            //series1.ShadowOffset = 0;
            //series1.Color = System.Drawing.Color.Red;
            //series1.IsXValueIndexed = true;

            //Chart1.Height = 300;
            //Chart1.Width = 600;
            //// Setting the X Axis
            //Chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;
            //Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            //Chart1.ChartAreas["ChartArea1"].AxisX.Maximum = Double.NaN;
            //Chart1.ChartAreas["ChartArea1"].AxisX.Title = "";
            //Chart1.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("Sans Serif", 10, FontStyle.Bold);

            //// Setting the Y Axis
            //Chart1.ChartAreas["ChartArea1"].AxisY.Interval = 2;
            //Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Double.NaN;
            //Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Numbers";
            //Chart1.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("Sans Serif", 10, FontStyle.Bold);

            if (!IsPostBack)
            {

                loadDropDown();
                loadNodeTemplateToolbar();
                loadLinkTemplateToolbar();
                //loadNodeMatchTable("1.", "Influenza Virus, viral proteins", "<span>Influenza Virus is spherical with 8 RNA segments with code for 10 viral proteins</span><br /><span style=\"background-color:green;color:white;\">Influenza Virus is spherical </span><span style=\"background-color:red;color:white;\">and ssRNA virus</span><span style=\"background-color:green;color:white;\"> with 8 RNA segments with code for 10 viral proteins</span>");
                //for (int i = 0; i < Session.Count; i++)
                //{
                //    var crntSession = Session.Keys[i];
                //    Response.Write(string.Concat(crntSession, "=", Session[crntSession]) + "<br />");
                //}
            }
        }

        public void loadNodeMatchTable(string cell1data, string cell2data, string cell3data)
        {
            TableRow row = new TableRow();
 
            TableCell cell1 = new TableCell();
            cell1.Text = cell1data;
            row.Cells.Add(cell1); 
 
 
            TableCell cell2 = new TableCell();
            cell2.Text = cell2data;
            row.Cells.Add(cell2);
 
 
            TableCell cell3 = new TableCell();
            cell3.Text = cell3data;
            row.Cells.Add(cell3);
 
            //產生表格
            //nodeMatchTable.Rows.Add(row);
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
            if (HttpContext.Current.Application["nodeTemplate"] != null)
            {
                var nodeTList = (List<NodeManagement.NodeTemplate>)Application["nodeTemplate"];
                var js = "<script type='text/javascript'>";
                foreach (var nodeTItems in nodeTList)
                {
                    js += "localStorage.setItem('template-" + nodeTItems.templateID + "','" + nodeTItems.templateData + "');";
                    nodeTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li style=\"\"><div style=\"width:120px;margin-left:-20px;\">" + nodeTItems.templateName + "</div></li><li><div id=\"Div" + nodeTItems.templateID + "\" class=\"toolbar_padding toolbar_blue divtooltip\" style=\"width: 75px; background-color:#" + nodeTItems.color + "\" title=\"" + Server.HtmlEncode(loadNodeTemplate(nodeTItems.templateID)) + "\"></div></li>";
                }

                js += "inactiveLinkButton();</script>";
                ScriptManager.RegisterStartupScript(Page, GetType(), "tmp", js, false);
            }
            else
            {
                nodeTemplatePanel.Text += "<li><div id=\"\" class=\"\" style=\"width: 75px;\"><img src=\"images/play.jpg\" id=\"playbtn\" style=\"cursor:pointer;\"/></div></li>";
            }
        }
        public void loadLinkTemplateToolbar()
        {
            if (HttpContext.Current.Application["linkTemplate"] != null)
            {

                var linkTList = (List<LinkManagement.NodeTemplate>)Application["linkTemplate"];
                var js = "<script type='text/javascript'>";
                linkTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li style=\"\"><div style=\"width:120px;margin-left:0px;\">Default</div></li><li><div id=\"Div0\" class=\"divtooltip\" style=\"width: 75px; color: black;\"><button type=\"button\" class=\"btn btn-default btn-lg linkButton active\" data-toggle=\"button\" style=\"padding-bottom: 15px;font-size:40px;border: 1px solid #757575;color:#808080\">→</button></div></li>";
                foreach (var linkTItems in linkTList)
                {
                    js += "localStorage.setItem('linktemplate-" + linkTItems.templateID + "','" + linkTItems.templateData + "');";
                    linkTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li style=\"\"><div style=\"width:120px;margin-left:-20px;\">" + linkTItems.templateName + "</div></li><li><div id=\"Div" + linkTItems.templateID + "\" class=\"divtooltip\" style=\"width: 75px;\" title=\"" + Server.HtmlEncode(loadLinkTemplate(linkTItems.templateID)) + "\"> <button type=\"button\" class=\"btn btn-default btn-lg linkButton\" data-toggle=\"button\" style=\"padding-bottom: 15px;font-size:40px; border: 1px solid #757575;color:#" + linkTItems.color + "\">&rarr;</button></div></li>";
                }
                js += "</script>";
                //Response.Write(js);
                ScriptManager.RegisterStartupScript(Page, GetType(), "tmplink", js, false);
            }
            else
            {
                linkTemplatePanel.Text += "<li style=\"height: 10px;\"></li><li><div id=\"Div0\" class=\"divtooltip\" style=\"width: 75px; color: black;\"><img src=\"images/refresh.jpg\" id=\"refreshbtn\" style=\"cursor:pointer;\"/></div></li>";
            }
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
            // Singleton instance
            int i = 1;
            IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<ShapeShare>();
            if (keyword1 != "")
                _context.Clients.All.keywordShapeCreated(id1, i++.ToString() + ". " + keyword1, "0", "130");
            if (keyword2 != "")
                _context.Clients.All.keywordShapeCreated(id2, i++.ToString() + ". " + keyword2, "10", "140");
            if (keyword3 != "")
                _context.Clients.All.keywordShapeCreated(id3, i.ToString() + ". " + keyword3, "20", "150");
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
            IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<ShapeShare>();
            _context.Clients.All.setTemplateData(templateid, templatedata);
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public List<string> getStartToEndNodePath(string CID)
        {
            string pathQuery = "SELECT LinkID, LinkLabel, StartNode, EndNode, CID, Color, CreatedDateTime FROM ConceptMap_Link WHERE (CID = '" + CID + "') ORDER BY CID";

            SqlCommand pathcmd = new SqlCommand(pathQuery);

            DataTable roofLeaf = GetDataTable(pathcmd);

            List<string> startnode = new List<string>();
            List<string> endnode = new List<string>();

            if (roofLeaf.Rows.Count > 0)
            {

                foreach (DataRow row in roofLeaf.Rows) // Loop over the rows.
                {
                    startnode.Add(row["StartNode"].ToString());
                    endnode.Add(row["EndNode"].ToString());
                }

            }

            List<startendnode> senode = new List<startendnode>();

            startnode.ForEach(delegate(String snode)
            {
                DataRow[] sresult = roofLeaf.Select("EndNode='" + snode + "'");
                if (sresult.Length == 0) //起始節點
                {
                    endnode.ForEach(delegate(String enode)
                    {
                        DataRow[] eresult = roofLeaf.Select("StartNode='" + enode + "'");

                        if (eresult.Length == 0) //節點末端
                        {
                            senode.Add(new startendnode(snode, enode));
                        }
                    });
                }
            });

            Hashtable nodelabel = new Hashtable();
            string nodeQuery = "SELECT * FROM ConceptMap_Node WHERE (CID = '" + CID + "')";

            SqlCommand nodecmd = new SqlCommand(nodeQuery);

            DataTable nodetable = GetDataTable(nodecmd);

            if (nodetable.Rows.Count > 0)
            {
                foreach (DataRow row in nodetable.Rows) // Loop over the rows.
                {
                    nodelabel.Add(row["NodeID"].ToString(), row["NodeLabel"].ToString());
                }
            }

            List<string> linkpath = new List<string>();
            foreach (var senodes in senode)
            {
                string pathstr;
                using (var connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var countCommand = new SqlCommand(
                        "select nodepath from ConceptMap_path where StartNode = '" + senodes.ssnode + "' and EndNode = '" + senodes.eenode + "'",
                        connection);

                    pathstr = countCommand.ExecuteScalar().ToString();
                    connection.Close();
                }

                string linklabel = "";
                string[] nodepath = pathstr.Split(';');

                linkpath.Add(nodelabel[nodepath[0]].ToString());
                for (int i = 0; i < nodepath.Length - 1; i++)
                {
                    using (var connection = new SqlConnection(strConnString))
                    {
                        connection.Open();
                        var countCommand = new SqlCommand(
                            "select LinkLabel from ConceptMap_Link where StartNode = '" + nodepath[i] + "' and EndNode = '" + nodepath[i + 1] + "'",
                            connection);

                        linklabel = countCommand.ExecuteScalar().ToString();
                        connection.Close();
                        linkpath.Add(linklabel);
                        linkpath.Add(nodelabel[nodepath[i + 1]].ToString());
                    }
                }
            }
            return linkpath;
        }
        private static void CreateCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
        private DataTable GetDataTable(SqlCommand cmd)
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
        public class startendnode
        {
            public string ssnode { get; set; }
            public string eenode { get; set; }
            public startendnode(string snode, string enode)
            {
                this.ssnode = snode;
                this.eenode = enode;
            }
        }
        //EditDistance演算法，用來計算學生答案跟標準答案的相似度，及需要幾個步驟方能將學生答案改為標準答案(無權重)
        public int[,] EditDistance(DataTable dtStandardAns, DataTable dtStudentAns)
        {
            int n = dtStandardAns.Rows.Count;
            int m = dtStudentAns.Rows.Count;
            //Response.Write("m="+m);
            //Response.Write("n=" + n);
            int[,] D = new int[n + 1, m + 1]; //用來存比較結果的陣列
            int[] W = new int[m];
            //沒有加上權重的方法(各個權重皆為1)
            //給定預設值D[0,0]=0 , D[1,0]=1...D[n,0]=n , D[0,1]=1...D[0,m]=m
            D[0, 0] = 0;
            for (int i = 1; i <= n; i++)
            {
                D[i, 0] = i;
            }
            for (int j = 1; j <= m; j++)
            {
                D[0, j] = j;
            }
            //判斷哪一個操作為最佳(最小值)
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    D[i, j] = Math.Min((Math.Min((D[i - 1, j] + 1), (D[i, j - 1] + 1))), D[i - 1, j - 1] + substituteCost(dtStandardAns.Rows[i - 1]["cAnnotationNum"].ToString(), dtStudentAns.Rows[j - 1]["cAnnotationNum"].ToString()));
                }
            }

            return D;
        }


        //得到利用Edit Distance演算法求得的最小步驟路徑(無權重)
        //public string[] getShortestPath(int[,] EditDistanceMatrix)
        //{
        //    string[] shortestPath = new string[EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5];
        //    int x = 0, y = 0, z = 1;

        //    shortestPath[0] = "0,0";
        //    while (x != EditDistanceMatrix.GetLength(0) - 1 || y != EditDistanceMatrix.GetLength(1) - 1)
        //    {
        //        if (x == EditDistanceMatrix.GetLength(0) - 1)
        //        {
        //            y = y + 1;
        //            shortestPath[z] = x.ToString() + "," + y.ToString();
        //            z = z + 1;
        //        }
        //        else if (y == EditDistanceMatrix.GetLength(1) - 1)
        //        {
        //            x = x + 1;
        //            shortestPath[z] = x.ToString() + "," + y.ToString();
        //            z = z + 1;
        //        }

        //        else
        //        {
        //            //上方的點為最小值
        //            if ((EditDistanceMatrix[x, y + 1] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1])))
        //            {
        //                //判斷下一步
        //                if (x == y)//目前點在對角線
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x > y)//目前點在對角線下方
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x < y)//目前點在對角線上方
        //                {
        //                    x = x + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }
        //            }

        //            //右方的值為最小
        //            else if ((EditDistanceMatrix[x + 1, y] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1])))
        //            {
        //                //判斷下一步
        //                if (x == y)//目前點在對角線
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x > y)//目前點在對角線下方
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x < y)//目前點在對角線上方
        //                {
        //                    x = x + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }
        //            }

        //            //斜對角的值最小，表示不需要做動作
        //            else if (EditDistanceMatrix[x + 1, y + 1] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1]))
        //            {
        //                x = x + 1;
        //                y = y + 1;
        //                shortestPath[z] = x.ToString() + "," + y.ToString();
        //                z = z + 1;
        //            }
        //        }


        //    }

        //    /*for (int w = 0; w < EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5; w++)
        //    {
        //        Response.Write(shortestPath[w]);
        //    }*/

        //    return shortestPath;
        //}
        public string[] getShortestPath(int[,] EditDistanceMatrix)
        {
            string[] shortestPath = new string[EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5];
            string[] temp = new string[EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5];

            int x = EditDistanceMatrix.GetLength(0) - 1, y = EditDistanceMatrix.GetLength(1) - 1, z = 1;

            temp[0] = (EditDistanceMatrix.GetLength(0) - 1).ToString() + "," + (EditDistanceMatrix.GetLength(1) - 1).ToString();
            shortestPath[0] = "0,0";

            while (x != 0 || y != 0)
            {
                if (x == 0)
                {
                    y = y - 1;
                    temp[z] = x.ToString() + "," + y.ToString();
                    z = z + 1;
                }
                else if (y == 0)
                {
                    x = x - 1;
                    temp[z] = x.ToString() + "," + y.ToString();
                    z = z + 1;
                }

                else
                {
                    //斜下方的值為最小且等於目前的值，將斜下方的座標存入
                    if (EditDistanceMatrix[x, y] == EditDistanceMatrix[x - 1, y - 1] && EditDistanceMatrix[x - 1, y - 1] < EditDistanceMatrix[x - 1, y] && EditDistanceMatrix[x - 1, y - 1] < EditDistanceMatrix[x, y - 1])
                    {
                        x = x - 1;
                        y = y - 1;
                        temp[z] = x.ToString() + "," + y.ToString();
                        z = z + 1;
                    }

                    //左方的值為最小
                    else if (EditDistanceMatrix[x - 1, y] == Math.Min(EditDistanceMatrix[x - 1, y], EditDistanceMatrix[x, y - 1]))
                    {
                        x = x - 1;
                        temp[z] = x.ToString() + "," + y.ToString();
                        z = z + 1;
                    }

                    //下方的值為最小
                    else if (EditDistanceMatrix[x, y - 1] == Math.Min(EditDistanceMatrix[x - 1, y], EditDistanceMatrix[x, y - 1]))
                    {
                        y = y - 1;
                        temp[z] = x.ToString() + "," + y.ToString();
                        z = z + 1;
                    }
                }
            }


            int t = temp.Length - 1;
            int s = 0;

            while (t > -1)
            {
                if (temp[t] != null)
                {
                    shortestPath[s] = temp[t];
                    s = s + 1;
                    t = t - 1;
                }
                else
                    t = t - 1;
            }



            return shortestPath;
        }

        //比較兩步驟是否一樣
        public int substituteCost(String a, String b)
        {
            if (a == b)
                return 0;
            else
                return 2;

        }
        //創建學生的作答步驟表格
        protected void CreatStudentAnswerTable(DataTable dtStandardStep, DataTable dtStudentStep)
        {

            //Table tbStudentStep = new Table();
            //tbStudentStep.ID = "tbStudentStep";
            //tbStudentStep.Attributes["Width"] = "95%";
            //tbStudentStep.Attributes["style"] = "border:solid 1px black;border-collaspse:collapse;font-size: large;";

            //TableRow trTittle = new TableRow();
            //trTittle.ID = "trStudentTittle";

            //TableCell tcStepTitle = new TableCell();
            //tcStepTitle.ID = "tcStudentStep";
            //tcStepTitle.Text = "Step Order";
            ////tcStepTitle.ColumnSpan = 2;
            //tcStepTitle.Attributes["Width"] = "50%";
            //tcStepTitle.Attributes["style"] = "border:solid 1px black;border-collaspse:collapse;border-style: groove; border-width: thin; background-color: #999999; color: #FFFFFF; font-weight: 900;font-size: large;";
            ////tcTittle.Font.Bold = true;
            //TableCell tcMissStep = new TableCell();
            //tcMissStep.ID = "tcMissStep";
            //tcMissStep.Text = "Missing Step";
            //tcMissStep.Attributes["Width"] = "50%";
            //tcMissStep.Attributes["style"] = "border:solid 1px black;border-collaspse:collapse;border-style: groove; border-width: thin; background-color: #999999; color: #FFFFFF; font-weight: 900;font-size: large;";

            //trTittle.Controls.Add(tcStepTitle);
            //trTittle.Controls.Add(tcMissStep);
            //tbStudentStep.Controls.Add(trTittle);

            string[] shortestPath = new string[dtStandardStep.Rows.Count + dtStudentStep.Rows.Count + 7];
            shortestPath = getShortestPath(EditDistance(dtStandardStep, dtStudentStep));
            int z = 0;//shortestPath陣列的指標




            //int n = dtStandardStep.Rows.Count - numOfOr;
            //string[,] standardDetailStep = new string[n, 2];//存放標準步驟細節步驟以及權重
            //int addrPoint = 0;//記錄現在在標準步驟的陣列放到第幾個


            int point = 0;
            Label lbStep = new Label();
            lbStep.Text = "";
            //開始建立學生步驟的表格內容
            for (int i = 0; i < dtStudentStep.Rows.Count + dtStandardStep.Rows.Count; i++)
            {

                if (shortestPath[z + 1] != null)
                {
                    //將坐標字串以','來切割，用來判斷刪除、插入等等動作
                    string[] tempNew = new string[2];
                    tempNew[0] = shortestPath[z + 1].Split(',')[0];
                    tempNew[1] = shortestPath[z + 1].Split(',')[1];

                    string[] tempOld = new string[2];
                    tempOld[0] = shortestPath[z].Split(',')[0];
                    tempOld[1] = shortestPath[z].Split(',')[1];

                    //該步驟需要刪除
                    if (Convert.ToInt32(tempNew[0]) - Convert.ToInt32(tempOld[0]) == 0 && Convert.ToInt32(tempNew[1]) - Convert.ToInt32(tempOld[1]) == 1)
                    {
                        //TableRow trData = new TableRow();
                        ////trData.ID = "trStudentData_" + dtStep.Rows[i]["strRecordID"].ToString();

                        //TableCell tcStep = new TableCell();
                        ////tcStep.ID = "tcStudentStep_" + dtStep.Rows[i]["strRecordID"].ToString();
                        //tcStep.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC;color: #0000FF; font-weight: 900;text-decoration: line-through;";
                        //tcStep.Attributes["Width"] = "50%";

                        
                        lbStep.Text += "<span style=\"background-color:yellow;font-weight:bold;text-decoration: line-through;\">" + dtStudentStep.Rows[i]["cAnnotationNum"].ToString() + " </span>";
                        
                        //Label1.Text += "<span style=\"background-color:yellow;font-weight:bold;text-decoration: line-through;\">" + lbStep.Text + " </span>";
                        
                        //TableCell tcOperating = new TableCell();
                        //tcOperating.Attributes["Width"] = "50%";
                        //tcOperating.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";

                        //tcStep.Controls.Add(lbStep);
                        //trData.Controls.Add(tcStep);
                        //trData.Controls.Add(tcOperating);
                        //tbStudentStep.Controls.Add(trData);
                        z = z + 1;//指往下一個
                        //dtStudentStep.RemoveAt(point);
                    }

                    //插入缺少的步驟(在上面已經有將詳細步驟合併,所以這邊直接插入陣列的內容)
                    else if (Convert.ToInt32(tempNew[0]) - Convert.ToInt32(tempOld[0]) == 1 && Convert.ToInt32(tempNew[1]) - Convert.ToInt32(tempOld[1]) == 0)
                    {
                        //TableRow trData = new TableRow();

                        //TableCell tcStep = new TableCell();
                        //tcStep.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";
                        //tcStep.Attributes["Width"] = "50%";

                        //TableCell tcOperating = new TableCell();
                        //tcOperating.Attributes["Width"] = "50%";
                        //tcOperating.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#FF0000;font-weight: 900;";

                        //Label lbStepOpterating = new Label();
                        //lbStepOpterating.Text = dtStandardStep.Rows[Convert.ToInt32(tempNew[0]) - 1]["cAnnotationNum"].ToString();

                        lbStep.Text += "<span style=\"background-color:red;font-weight:bold;\">" + dtStandardStep.Rows[Convert.ToInt32(tempNew[0]) - 1]["cAnnotationNum"].ToString() + " </span>";

                        //Label1.Text += "<span style=\"background-color:red;font-weight:bold;\">" + lbStepOpterating.Text + " </span>";

                        //tcOperating.Controls.Add(lbStepOpterating);
                        //trData.Controls.Add(tcStep);
                        //trData.Controls.Add(tcOperating);
                        //tbStudentStep.Controls.Add(trData);
                        z = z + 1;
                        i = i - 1;
                        point = point + 1;//插入新的步驟後，指針往下加1
                    }

                    else if (Convert.ToInt32(tempNew[0]) - Convert.ToInt32(tempOld[0]) == 1 && Convert.ToInt32(tempNew[1]) - Convert.ToInt32(tempOld[1]) == 1)
                    {
                        //TableRow trData = new TableRow();
                        ////trData.ID = "trStudentData_" + dtStep.Rows[i]["strRecordID"].ToString();

                        //TableCell tcStep = new TableCell();
                        ////tcStep.ID = "tcStudentStep_" + dtStep.Rows[i]["strRecordID"].ToString();
                        //tcStep.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";
                        
                        //Label lbStep = new Label();
                        lbStep.Text = "<span style=\"background-color:green;color:white;font-weight:bold;\">" + dtStudentStep.Rows[i]["cAnnotationNum"].ToString() + " </span>";
                        
                        
                        //Label1.Text += "<span style=\"background-color:green;color:white;font-weight:bold;\">" + lbStep.Text + " </span>";
                        
                        //TableCell tcOperating = new TableCell();
                        //tcOperating.Attributes["Width"] = "50%";
                        //tcOperating.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";

                        //tcStep.Controls.Add(lbStep);

                        //trData.Controls.Add(tcStep);
                        //trData.Controls.Add(tcOperating);
                        //tbStudentStep.Controls.Add(trData);
                        z = z + 1;
                        point = point + 1;//插入新的步驟後，指針往下加1
                    }
                }
                else
                {
                }

            }





            //return tbStudentStep;

        }
    }
}