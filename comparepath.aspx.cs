using System;
using System.Collections;
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
    public partial class comparepath : System.Web.UI.Page
    {
        
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(getStartEndNode("398"));
            //Response.Write(getStartEndNode("397"));
            listAllComparePath("400", "463");
        }
        public void listAllComparePath(string studentCID, string teacherCID) 
        {
            List<string> sPath = getStartToEndNodePath(studentCID);
            List<string> tPath = getStartToEndNodePath(teacherCID);

            List<string> sFullPath = getfullpathText(sPath, studentCID);
            List<string> tFullPath = getfullpathText(tPath, teacherCID);

            DataTable dt1 = new DataTable();
            dt1.Clear();
            dt1.Columns.Add("cAnnotationNum");

            DataTable dt2 = new DataTable();
            dt2.Clear();
            dt2.Columns.Add("cAnnotationNum");

            List<string> answer = new List<string>();
            var list = new List<KeyValuePair<int, string>>();

            for (int i = 0; i < sFullPath.Count; i++) // Loop through List with for
            {

                string[] tmptnode = sFullPath[i].Split(',');
                foreach (string tnodestr in tmptnode)
                    dt1.Rows.Add(tnodestr);


                for (int j = 0; j < tFullPath.Count; j++) // Loop through List with for
                {

                    string[] tmpsnode = tFullPath[j].Split(',');
                    foreach (string snodestr in tmpsnode)
                        dt2.Rows.Add(snodestr);

                    for (int k = 0; k < dt1.Rows.Count; k++)
                    {
                        //stdanswer.Text += dt1.Rows[k]["cAnnotationNum"].ToString() + " ";
                    }
                    //stdanswer.Text += "<br />";
                    int[,] arr = EditDistance(dt1, dt2);

                    int rowLength = arr.GetLength(0);
                    int colLength = arr.GetLength(1);

                    string tmpans = CreatStudentAnswerTable(dt2, dt1);


                    string[] tmparr = tmpans.Split('@');
                    list.Add(new KeyValuePair<int, string>(int.Parse(tmparr[0]), tmparr[1]));

                    dt2.Clear();
                }
                list.Sort(Compare1);
                list.Reverse();
                    Label1.Text = list[0].Value + "<br />";
                dt1.Clear();
                //dt1.Clear();

                //break;
            }
        }

            public int Compare1(KeyValuePair<int, string> a, KeyValuePair<int, string> b)
            {
                return a.Key.CompareTo(b.Key);
            }
            public List<string> getfullpathText(List<string> nodepath, string CID){

                    //string pathquery = "SELECT Top 1 nodepath FROM ConceptMap_Path WHERE StartNode = '" + startnode + "' AND EndNode = '" + endnode + "'";
                    //SqlCommand pathcmd = new SqlCommand(pathquery);
                    //DataSet pathds = GetData(pathcmd);
                    //DataRow nodepath = pathds.Tables[0].Rows[0];
                    //SingleHighlightNodePath(nodepath["nodepath"].ToString());

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
                    string linklabel = "";
                    string linkpathstr = "";
                    for (int i = 0; i < nodepath.Count; i++)
                    {
                        string[] fullpath = nodepath[i].Split(';');
                        for (int j = 0; j < fullpath.Length-1; j++)
                        {
                            if (j == 0)
                            linkpathstr += nodelabel[fullpath[j]].ToString() + ",";
                            using (var connection = new SqlConnection(strConnString))
                            {
                                connection.Open();
                                var countCommand = new SqlCommand(
                                    "select LinkLabel from ConceptMap_Link where StartNode = '" + fullpath[j] + "' and EndNode = '" + fullpath[j + 1] + "'",
                                    connection);

                                linklabel = countCommand.ExecuteScalar().ToString();
                                connection.Close();
                                linkpathstr += linklabel + ",";
                                linkpathstr += nodelabel[fullpath[j + 1]].ToString() + ",";
                            }
                        }
                        linkpathstr = linkpathstr.Remove(linkpathstr.Length - 1);
                        linkpath.Add(linkpathstr);
                        linkpathstr = "";
                    }
                    return linkpath;
            }

            public void getfullpathID(string CID, string startnode, string endnode)
            {

            }
            

           

            //for (int i = 0; i < rowLength; i++)
            //{
            //    for (int j = 0; j < colLength; j++)
            //    {
            //        Response.Write(string.Format("{0}, ", arr[i, j]));
            //    }
            //    Response.Write(string.Format("<br />"));
            //    //Console.Write(Environment.NewLine + Environment.NewLine);
            //}
            //Response.Write(string.Format("<br />"));
            //Response.Write(string.Format("<br />"));
            //string[] shortestp = getShortestPath(arr);
            //for (int k = 0; k < shortestp.Length; k++)
            //{
            //    Response.Write(string.Format("{0}, ", shortestp[k]));
            //    Response.Write(string.Format("<br />"));
            //}
            //Panel1.Controls.Add(CreatStudentAnswerTable(dt1, dt2));
           
            //Console.ReadLine();
        
        public List<string> getStartToEndNodePath(string CID)
        {
                string pathQuery = "SELECT DISTINCT LinkID, LinkLabel, StartNode, EndNode, CID, Color, CreatedDateTime FROM ConceptMap_Link WHERE (CID = '" + CID + "') ORDER BY CID";

                SqlCommand pathcmd = new SqlCommand(pathQuery);

                DataTable roofLeaf = GetDataTable(pathcmd);

                List<string> startnode = new List<string>();
                List<string> endnode = new List<string>();
                
                //List<startendnode> senode = new List<startendnode>();

                if (roofLeaf.Rows.Count > 0)
                {

                    foreach (DataRow row in roofLeaf.Rows) // Loop over the rows.
                    {
                        startnode.Add(row["StartNode"].ToString());
                        endnode.Add(row["EndNode"].ToString());
                        //senode.Add(new startendnode(row["StartNode"].ToString(), row["EndNode"].ToString()));
                        //endnode.Add();
                    }

                }

                List<startendnode> senode = new List<startendnode>();

                string rootLeafPathQuery = ""; 
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
                                string rootLeafQuery = "select nodepath from ConceptMap_RootLeafPath where CID = '" + CID + "' AND StartNode = '" + snode + "' AND EndNode='" + enode + "'";

                                SqlCommand fullpathcmd = new SqlCommand(rootLeafQuery);

                                DataTable fullpathtable = GetDataTable(fullpathcmd);

                                if (fullpathtable.Rows.Count == 0)
                                {
                                    string insQuery = "insert into ConceptMap_RootLeafPath(CID, StartNode, EndNode) values('" + CID + "','" + snode + "', '" + enode + "')";
                                    CreateCommand(insQuery, strConnString);
                                }
                                    
                            }
                        });
                    }
                });

                

                
                string fullpathQuery = "";
                List<string> pathstr = new List<string>();
                foreach (var senodes in senode)
                {
                    
                    //using (var connection = new SqlConnection(strConnString))
                    //{
                    //    connection.Open();
                    //    var countCommand = new SqlCommand(
                    //        "select nodepath from ConceptMap_path where StartNode = '" + senodes.ssnode + "' and EndNode = '" + senodes.eenode + "'",
                    //        connection);
                    fullpathQuery = "select nodepath from ConceptMap_path where StartNode = '" + senodes.ssnode + "' and EndNode='"+senodes.eenode+"'";

                    SqlCommand fullpathcmd = new SqlCommand(fullpathQuery);

                    DataTable fullpathtable = GetDataTable(fullpathcmd);

                    if (fullpathtable.Rows.Count > 0)
                    {
                        foreach (DataRow row in fullpathtable.Rows) // Loop over the rows.
                        {
                            pathstr.Add(row["nodepath"].ToString());
                        }
                    }
                    string tmppath = "";
                    string rootleafquery = "SELECT nodepath FROM ConceptMap_RootLeafPath WHERE CID = '" + CID + "' AND StartNode = '" + senodes.ssnode + "' AND EndNode = '" + senodes.eenode + "'";
                    SqlCommand rootleafcmd = new SqlCommand(rootleafquery);
                    DataTable pathdt = GetDataTable(rootleafcmd);
                    int totalRows = pathdt.Rows.Count;
                    //nodepath["nodepath"].ToString();
                    

                    if (totalRows <= 0)
                    {
                        foreach (var pathValue in pathstr)
                        {
                            tmppath += pathValue + ";";
                            
                        }
                        tmppath = tmppath.Remove(tmppath.Length - 1);
                        string delimeter = ";";
                        rootLeafPathQuery = "insert into ConceptMap_RootLeafPath(CID, nodepath, linkpath,StartNode,EndNode) values('" + CID + "','"+tmppath+"','','" + senodes.ssnode + "','" + senodes.eenode + "');";
                        CreateCommand(rootLeafPathQuery, strConnString);
                    }
                    //    pathstr = countCommand.ExecuteScalar().ToString();
                    //    connection.Close();
                    //}

                    
                    //string[] nodepath = pathstr.Split(';');
                    //return pathstr;
                    //string linklabel = "";
                    //string linkpathstr = "";
                    //for (int i = 0; i < nodepath.Length-1; i++)
                    //{
                    //    if(i==0)
                    //        linkpathstr = nodelabel[nodepath[0]].ToString() +",";
                    //    using (var connection = new SqlConnection(strConnString))
                    //    {
                    //        connection.Open();
                    //        var countCommand = new SqlCommand(
                    //            "select LinkLabel from ConceptMap_Link where StartNode = '" + nodepath[i] + "' and EndNode = '" + nodepath[i + 1] + "'",
                    //            connection);

                    //        linklabel = countCommand.ExecuteScalar().ToString();
                    //        connection.Close();
                    //        linkpathstr += linklabel + ",";
                    //        linkpathstr += nodelabel[nodepath[i + 1]].ToString() + ",";
                    //    }
                    //}
                    //linkpathstr = linkpathstr.Remove(linkpathstr.Length - 1);
                    //linkpath.Add(linkpathstr);
                 }
                return pathstr;
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
        protected string CreatStudentAnswerTable(DataTable dtStandardStep, DataTable dtStudentStep)
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

            string tmpAns = "";
            int mark = 0;
            int point = 0;
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

                        Label lbStep = new Label();
                        lbStep.Text = dtStudentStep.Rows[i]["cAnnotationNum"].ToString();
                        tmpAns += "<span style=\"background-color:yellow;font-weight:bold;text-decoration: line-through;\"> " + lbStep.Text + " </span>";
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

                        Label lbStepOpterating = new Label();
                        lbStepOpterating.Text = dtStandardStep.Rows[Convert.ToInt32(tempNew[0]) - 1]["cAnnotationNum"].ToString();
                        tmpAns += "<span style=\"background-color:red;font-weight:bold;\"> " + lbStepOpterating.Text + " </span>";

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
                        Label lbStep = new Label();
                        lbStep.Text = dtStudentStep.Rows[i]["cAnnotationNum"].ToString();
                        tmpAns += "<span style=\"background-color:green;color:white;font-weight:bold;\"> " + lbStep.Text + " </span>";
                        //TableCell tcOperating = new TableCell();
                        //tcOperating.Attributes["Width"] = "50%";
                        //tcOperating.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";

                        //tcStep.Controls.Add(lbStep);

                        //trData.Controls.Add(tcStep);
                        //trData.Controls.Add(tcOperating);
                        //tbStudentStep.Controls.Add(trData);
                        mark++;
                        z = z + 1;
                        point = point + 1;//插入新的步驟後，指針往下加1
                    }
                }
                else
                {
                }
                
            }
            //SortedList<int, string> st = new SortedList<int, string>();

            //st.Add(mark, tmpAns);

            //Label1.Text += tmpAns + "<br />";

            return mark+"@"+tmpAns;
            //if (point <= dtStudentStep.Rows.Count)
            //{
               
            //}





            //return tbStudentStep;

        }
    }
}