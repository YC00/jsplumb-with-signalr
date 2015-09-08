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
    public partial class searchnodepath : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Call recursive method with two parameters.
            //
            //int count = 0;
            //int total = Recursive(5, ref count);
            //
            // Write the result from the method calls and also the call count.
            //

            string rfQuery = "SELECT * FROM ConceptMap_RootLeafPath WHERE CID = '400'";
            SqlCommand rfcmd = new SqlCommand(rfQuery);
            DataTable rftable = GetDataTable(rfcmd);
            if (rftable.Rows.Count > 0)
            {
                foreach (DataRow rfrow in rftable.Rows) // Loop over the rows.
                {
                    string cid = rfrow["CID"].ToString();
                    string snode = rfrow["StartNode"].ToString();
                    string enode = rfrow["EndNode"].ToString();
                    findfullpath(cid, snode, enode);
                    //findfullpath1(cid, snode, enode);
                    string nodeQuery = "SELECT nodepath FROM ConceptMap_Path WHERE CID = '" + cid + "' AND StartNode = '" + snode + "' AND EndNode='" + enode + "'";
                    SqlCommand nodecmd = new SqlCommand(nodeQuery);
                    DataTable nodetable = GetDataTable(nodecmd);



                    if (nodetable.Rows.Count > 0)
                    {
                        foreach (DataRow row in nodetable.Rows) // Loop over the rows.
                        {
                            string[] tmppath = row["nodepath"].ToString().Split(';');
                            if (tmppath[0] == snode && tmppath[tmppath.Length - 1] == enode)
                            {
                                string delQuery = "Delete from ConceptMap_Path where CID = '"+cid+"' AND StartNode = '"+snode+"' AND EndNode='"+enode+"'";
                                CreateCommand(delQuery, strConnString);
                                //Response.Write(row["nodepath"].ToString());
                                string insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode, nodepath) values('"+cid+"','"+snode+"', '"+enode+"','" + row["nodepath"] + "')";
                                CreateCommand(insQuery, strConnString);
                            }

                        }
                    }
                
                }
            }
          
            
            //Response.Write(count.ToString() + "<br />");
            //Response.Write(fullpath("462"));
        }

        public void findfullpath1(string CID, string startnode, string endnode)
        {
            string tmpath ="";
            string tmpenode = startnode+";";
            do{
            string nodeQuery = "SELECT EndNode FROM ConceptMap_Link WHERE CID = '" + CID + "' AND StartNode = '" + startnode + "'";
            SqlCommand nodecmd = new SqlCommand(nodeQuery);
            DataTable nodetable = GetDataTable(nodecmd);
            string tmppath = startnode + ";";
            
            if (nodetable.Rows.Count > 0)
            {
                foreach (DataRow row in nodetable.Rows) // Loop over the rows.
                {
                    tmpenode = row["EndNode"].ToString();
                    tmpath += row["EndNode"].ToString()+";";
                }
            }
            }while(endnode!=tmpenode);
            Response.Write(tmpath+"<br />");
        }

        public void findfullpath(string CID, string startnode, string endnode)
        {
            string nodeQuery = "SELECT EndNode FROM ConceptMap_Link WHERE CID = '" + CID + "' AND StartNode = '" + startnode + "'";
            SqlCommand nodecmd = new SqlCommand(nodeQuery);
            DataTable nodetable = GetDataTable(nodecmd);
            string tmppath = startnode + ";";
            string insQuery = "";
            
            if (nodetable.Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow row in nodetable.Rows) // Loop over the rows.
                {
                    insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode, nodepath) values('" + CID + "','" + startnode + "', '" + endnode + "','" + tmppath + row["EndNode"].ToString() + "')";
                    CreateCommand(insQuery, strConnString);

                    if (nodetable.Rows.Count <= 1)
                        tmppath += row["EndNode"].ToString() + ";";
                    //else
                    //{
                    //    if (count == 0)
                    //    {
                    //        tmppath += row["EndNode"].ToString() + ";";
                    //        count++;
                    //    }
                    //    else
                    //    {
                    //        string[] tmparr = tmppath.Split(';');
                    //        List<string> lst = tmparr.OfType<string>().ToList(); // this isn't going to be fast.
                    //        lst.RemoveAt(tmparr.Length - 1);
                    //        tmppath += row["EndNode"].ToString() + ";";

                    //    }
                    //}
                    if (row["EndNode"].ToString() == endnode)
                        break;
                    
                    string nodeQuery1 = "SELECT EndNode FROM ConceptMap_Link WHERE StartNode = '" + row["EndNode"].ToString() + "'";
                    SqlCommand nodecmd1 = new SqlCommand(nodeQuery1);
                    DataTable nodetable1 = GetDataTable(nodecmd1);

                    if (nodetable1.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in nodetable1.Rows) // Loop over the rows.
                        {

                            insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode, nodepath) values('" + CID + "','" + startnode + "', '" + endnode + "','" + tmppath + row1["EndNode"].ToString() + "')";
                            
                            CreateCommand(insQuery, strConnString);

                            if (nodetable1.Rows.Count <= 1)
                                tmppath += row1["EndNode"].ToString() + ";";
                    
                            if (row1["EndNode"].ToString() == endnode)
                                break;
                           
                            //Response.Write(tmppath + "<br />");
                            string nodeQuery2 = "SELECT EndNode FROM ConceptMap_Link WHERE StartNode = '" + row1["EndNode"].ToString() + "'";
                            SqlCommand nodecmd2 = new SqlCommand(nodeQuery2);
                            DataTable nodetable2 = GetDataTable(nodecmd2);

                            if (nodetable2.Rows.Count > 0)
                            {

                                foreach (DataRow row2 in nodetable2.Rows) // Loop over the rows.
                                {
                                    insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode, nodepath) values('" + CID + "','" + startnode + "', '" + endnode + "','" + tmppath + row2["EndNode"].ToString() + "')";
                                    CreateCommand(insQuery, strConnString);

                                    if (nodetable2.Rows.Count <= 1)
                                        tmppath += row2["EndNode"].ToString() + ";";
                                    //Response.Write(tmppath + "<br />");

                                    if (row2["EndNode"].ToString() == endnode)
                                        break;

                                }
                            }
                            else
                            {
                                insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode, nodepath) values('" + CID + "','" + startnode + "', '" + endnode + "','" + tmppath + row1["EndNode"].ToString() + "')";
                                CreateCommand(insQuery, strConnString);
                                tmppath = "";
                            }
                        }
                        //tmppath = "";
                    }
                    else
                    {
                        insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode, nodepath) values('" + CID + "','" + startnode + "', '" + endnode + "','" + tmppath + row["EndNode"].ToString() + "')";
                        CreateCommand(insQuery, strConnString);
                        tmppath = "";
                    }
                }
            }
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
        static int Recursive(int value, ref int count)
        {
            count++;
            if (value >= 10)
            {
                // throw new Exception("End");
                return value;
            }
            return Recursive(value + 1, ref count);
        }
        public string recursivepath(string startnode, string endnode) 
        {
            string nodeQuery = "SELECT EndNode FROM ConceptMap_Link WHERE StartNode = '"+startnode+"'";
            SqlCommand nodecmd = new SqlCommand(nodeQuery);
            DataTable nodetable = GetDataTable(nodecmd);
            string tmpendnode = "";
            if (nodetable.Rows.Count > 0)
            {
                foreach (DataRow row in nodetable.Rows) // Loop over the rows.
                {
                    if (row["EndNode"].ToString()==endnode)
                        tmpendnode = row["EndNode"].ToString();
                }
            }
            return recursivepath(startnode +";"+ tmpendnode, endnode);
        }
        public string fullpath(string CID)
        {
            string pathQuery = "SELECT StartNode,EndNode FROM ConceptMap_Link WHERE CID = '"+CID+"'";
            SqlCommand pathcmd = new SqlCommand(pathQuery);
            DataTable pathtable = GetDataTable(pathcmd);
            List<string> fullpath = new List<string>();
            string startnode = "";
            string tmpendnode = "";
            string endnode = "";
            if (pathtable.Rows.Count > 0)
            {
                foreach (DataRow row in pathtable.Rows) // Loop over the rows.
                {
                    startnode = row["StartNode"].ToString();
                    fullpath.Add(startnode);
                    endnode = row["EndNode"].ToString();
                    do{
                        string pathQuery1 = "SELECT EndNode FROM ConceptMap_Link WHERE StartNode = '" + startnode + "'";
                        SqlCommand pathcmd1 = new SqlCommand(pathQuery1);
                        DataTable pathtable1 = GetDataTable(pathcmd1);
                        if (pathtable1.Rows.Count > 0)
                        {
                            foreach (DataRow row1 in pathtable1.Rows) // Loop over the rows.
                            { 
                                
                            }
                        }
                    }while(tmpendnode!=endnode);
                    
                }
            }
            
            
            string strfullpath = "";
            
            for (int i = 0; i < fullpath.Count; i++) // Loop through List with for
            {
                strfullpath += fullpath + ";";
            }

            strfullpath = strfullpath.Remove(strfullpath.Length - 1);
            return strfullpath;
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
    }
}