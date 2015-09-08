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
    public partial class rootleaf : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            string pathQuery = "SELECT LinkID, LinkLabel, StartNode, EndNode, CID, Color, CreatedDateTime FROM ConceptMap_Link WHERE (CID = '346') ORDER BY CID";

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
                if (sresult.Length == 0)
                {
                    endnode.ForEach(delegate(String enode)
                    {
                        DataRow[] eresult = roofLeaf.Select("StartNode='" + enode + "'");
                        if (eresult.Length == 0)
                        {
                            senode.Add(new startendnode(snode,enode));
                            string insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode) values('346','" + snode + "', '" + enode + "')";

                            CreateCommand(insQuery, strConnString);
                        }
                    });
                }
            });

            Stack<string> nodepath = new Stack<string>();
            
            foreach (var senodes in senode)
            {
                nodepath.Clear();
                nodepath.Push(senodes.eenode);
                
                string tmp = "";

                using (var connection = new SqlConnection(strConnString))
                {
                    connection.Open();
                    var countCommand = new SqlCommand(
                        "WITH ConceptMap_Link_BOM(StartNode, EndNode, LEVEL, SortCol) AS (SELECT StartNode, EndNode, 0, CONVERT(nvarchar(128), StartNode) FROM ConceptMap_Link WHERE EndNode = N'" + senodes.eenode + "' UNION ALL SELECT P.StartNode, P.EndNode, B. LEVEL + 1, CONVERT(nvarchar(128), B.SortCol + ';' + CONVERT(nvarchar(128), P.StartNode)) FROM ConceptMap_Link P, ConceptMap_Link_BOM B WHERE P.EndNode = B.StartNode) SELECT TOP 1 SortCol FROM ConceptMap_Link_BOM ORDER BY LEVEL DESC;", // this GROUP BY is superfluous, returns no rows
                        connection);


                    tmp = countCommand.ExecuteScalar().ToString();
                }

                string[] path = tmp.Split(';');
                for (int i = 0; i < path.Length; i++)
                {
                    nodepath.Push(path[i]);
                }
                string concat = String.Join(";", nodepath.ToArray());

                string updQuery = "update ConceptMap_Path set nodepath='" + concat + "' where StartNode='" + senodes.ssnode + "' and EndNode='" + senodes.eenode + "'";

                CreateCommand(updQuery, strConnString);
            } 
        }

        public class startendnode {
            public string ssnode { get; set; }
            public string eenode { get; set; }
            public startendnode(string snode, string enode) 
            {
                this.ssnode = snode;
                this.eenode = enode;
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