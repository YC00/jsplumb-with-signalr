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

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class DragFlowbak : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadDropDown();
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
                            DropDownList1.DataSource = dt;
                            DropDownList1.DataValueField = dt.Columns["CID"].ToString();
                            DropDownList1.DataTextField = dt.Columns["CName"].ToString();
                            DropDownList1.DataBind();
                            DropDownList1.Items.Insert(0, "-");
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
                    using (SqlCommand cmd = new SqlCommand("insert into ConceptMap_Node(CID,NodeID,NodeLabel,Xcoordinate,Ycoordinate,Width,Height,CreatedDateTime) VALUES(@CID,@nodeID,@nodeLabel,@positionX,@positionY,@width,@height,@createdDateTime)", con))
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
            _context.Clients.All.keywordShapeCreated(id1, i++.ToString() + ". " + keyword1, "0", "130");
            _context.Clients.All.keywordShapeCreated(id2, i++.ToString() + ". " + keyword2, "10", "140");
            _context.Clients.All.keywordShapeCreated(id3, i.ToString() + ". " + keyword3, "20", "150");
            //_context.Clients.All.shapeCreated("71121213123123137");
            // This method is invoked by a Timer object.
            
            // return some data here
            //ScriptManager.RegisterStartupScript(pThis.Page, pThis.GetType(), "tmp", "<script type='text/javascript'>shapeShare.server.newKeywordShape(\"77\", \"123\", \"abc\", \"xyz\", \"\", \"0\", \"130\");</script>", false);
            //ScriptManager.RegisterStartupScript("OnLoad", "<script>shapeShare.server.newKeywordShape(\"77\", \"123\", \"abc\", \"xyz\", \"\", \"0\", \"130\");</script>");
            //sendKeyword();
            //return "";
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}