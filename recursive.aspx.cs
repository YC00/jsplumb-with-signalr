using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class recursive : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;

        public class link { 
            public string startnode { get; set; }
            public string endnode { get; set; }

            public link(string startNodeItem, string endNodeItem)
            {
                startnode = startNodeItem;
                endnode = endNodeItem;
            }
           
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> rootleaf = getRootLeafNode("470");

            List<string> root = rootleaf["root"];
            List<string> leaf = rootleaf["leaf"];

            for (int i = 0; i < root.Count; i++)
            {
                Response.Write("root : " + root[i].ToString() + "</br>");
            }

            for (int j = 0; j < leaf.Count; j++)
            {
                Response.Write("leaf : " + leaf[j].ToString() + "</br>");
            }
        }
        
        public List<string> getStartNode(string CID)
        {
            List<string> startnode = new List<string>();

            string QueryStr = "select startnode from ConceptMap_Link where CID='"+CID+"'";
            SqlCommand Cmd = new SqlCommand(QueryStr);
            DataTable dt = GetDataTable(Cmd);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    startnode.Add(row["StartNode"].ToString());
                }
            }
            return startnode;
        }

        public List<string> getEndNode(string CID)
        {
            List<string> endnode = new List<string>();

            string QueryStr = "select endnode from ConceptMap_Link where CID='"+CID+"'";
            SqlCommand Cmd = new SqlCommand(QueryStr);
            DataTable dt = GetDataTable(Cmd);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows) // Loop over the rows.
                {
                    endnode.Add(row["EndNode"].ToString());
                }
            }
            return endnode;
        }

        public Dictionary<string, List<string>> getRootLeafNode(string CID)
        {
            Dictionary<string, List<string>> rootleaf = new Dictionary<string, List<string>>();

            List<string> startnode = getStartNode(CID);
            List<string> endnode = getEndNode(CID);

            List<string> root = new List<string>();
            List<string> leaf = new List<string>();

            string pathQuery = "SELECT LinkID, LinkLabel, StartNode, EndNode, CID, Color, CreatedDateTime FROM ConceptMap_Link WHERE (CID = '" + CID + "')";

            SqlCommand pathcmd = new SqlCommand(pathQuery);

            DataTable roofLeaf = GetDataTable(pathcmd);


            //找root, leaf節點
            startnode.ForEach(delegate(String snode)
            {
                DataRow[] sresult = roofLeaf.Select("EndNode='" + snode + "'");
                if (sresult.Length == 0) //起始節點
                {
                    endnode.ForEach(delegate(String enode)
                    {
                        DataRow[] eresult = roofLeaf.Select("StartNode='" + enode + "'");

                        if (eresult.Length == 0) //節點末端，comment這段可找全部
                        {
                            root.Add(snode);
                            leaf.Add(enode);
                        }
                    });
                }
            });
            rootleaf.Add("root", root);
            rootleaf.Add("leaf", leaf);

            return rootleaf;
        }

        public string findpath(string CID, List<string> root, List<string> leaf) 
        {
            Stack<string> path = new Stack<string>();

            string pathQuery = "SELECT LinkID, LinkLabel, StartNode, EndNode, CID, Color, CreatedDateTime FROM ConceptMap_Link WHERE (CID = '" + CID + "')";

            SqlCommand pathcmd = new SqlCommand(pathQuery);

            DataTable roofLeaf = GetDataTable(pathcmd);
            
            //找root, leaf節點
            
            leaf.ForEach(delegate(String lnode)
            {
                DataRow[] sresult = roofLeaf.Select("EndNode='" + lnode + "'");

                if (sresult.Length > 0) //起始節點
                {
                    root.ForEach(delegate(String rnode)
                    {
                        DataRow[] eresult = roofLeaf.Select("StartNode='" + rnode + "'");

                        if (eresult.Length == 0) //節點末端，comment這段可找全部
                        {
                            //root.Add(lnode);
                            //leaf.Add(lnode);
                        }
                    });
                }
            });

            return "";
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
    }
}