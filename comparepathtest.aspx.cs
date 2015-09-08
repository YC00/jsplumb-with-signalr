using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class comparepathtest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            dt1.Clear();
            dt1.Columns.Add("cAnnotationNum");
            dt1.Rows.Add("Computer");
            dt1.Rows.Add("composed of");
            dt1.Rows.Add("software");
            dt1.Rows.Add("composed of CPU");

            DataTable dt2 = new DataTable();
            dt2.Clear();
            dt2.Columns.Add("cAnnotationNum");
            dt2.Rows.Add("Computer");
            dt2.Rows.Add("composed of");
            dt2.Rows.Add("hardware");
            dt2.Rows.Add("composed of CPU");

            CreatStudentAnswerTable(dt2, dt1);
        }
        //EditDistance演算法，用來計算學生答案跟標準答案的相似度，及需要幾個步驟方能將學生答案改為標準答案(無權重)
        public int[,] EditDistance(DataTable dtStandardAns, DataTable dtStudentAns)
        {
            int n = dtStandardAns.Rows.Count;
            int m = dtStudentAns.Rows.Count;
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
            Response.Write(D[n,m].ToString());
            return D;
        }


        //得到利用Edit Distance演算法求得的最小步驟路徑(無權重)
        public string[] getShortestPath(int[,] EditDistanceMatrix)
        {
            string[] shortestPath = new string[EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5];
            int x = 0, y = 0, z = 1;

            shortestPath[0] = "0,0";
            while (x != EditDistanceMatrix.GetLength(0) - 1 || y != EditDistanceMatrix.GetLength(1) - 1)
            {
                if (x == EditDistanceMatrix.GetLength(0) - 1)
                {
                    y = y + 1;
                    shortestPath[z] = x.ToString() + "," + y.ToString();
                    z = z + 1;
                }
                else if (y == EditDistanceMatrix.GetLength(1) - 1)
                {
                    x = x + 1;
                    shortestPath[z] = x.ToString() + "," + y.ToString();
                    z = z + 1;
                }

                else
                {
                    //上方的點為最小值
                    if ((EditDistanceMatrix[x, y + 1] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1])))
                    {
                        //判斷下一步
                        if (x == y)//目前點在對角線
                        {
                            y = y + 1;
                            shortestPath[z] = x.ToString() + "," + y.ToString();
                            z = z + 1;
                        }

                        else if (x > y)//目前點在對角線下方
                        {
                            y = y + 1;
                            shortestPath[z] = x.ToString() + "," + y.ToString();
                            z = z + 1;
                        }

                        else if (x < y)//目前點在對角線上方
                        {
                            x = x + 1;
                            shortestPath[z] = x.ToString() + "," + y.ToString();
                            z = z + 1;
                        }
                    }

                    //右方的值為最小
                    else if ((EditDistanceMatrix[x + 1, y] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1])))
                    {
                        //判斷下一步
                        if (x == y)//目前點在對角線
                        {
                            y = y + 1;
                            shortestPath[z] = x.ToString() + "," + y.ToString();
                            z = z + 1;
                        }

                        else if (x > y)//目前點在對角線下方
                        {
                            y = y + 1;
                            shortestPath[z] = x.ToString() + "," + y.ToString();
                            z = z + 1;
                        }

                        else if (x < y)//目前點在對角線上方
                        {
                            x = x + 1;
                            shortestPath[z] = x.ToString() + "," + y.ToString();
                            z = z + 1;
                        }
                    }

                    //斜對角的值最小，表示不需要做動作
                    else if (EditDistanceMatrix[x + 1, y + 1] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1]))
                    {
                        x = x + 1;
                        y = y + 1;
                        shortestPath[z] = x.ToString() + "," + y.ToString();
                        z = z + 1;
                    }
                }


            }

            /*for (int w = 0; w < EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5; w++)
            {
                Response.Write(shortestPath[w]);
            }*/

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
            Response.Write(tmpAns);
            //SortedList<int, string> st = new SortedList<int, string>();

            //st.Add(mark, tmpAns);

            //Label1.Text += tmpAns + "<br />";

            //return mark+"@"+tmpAns;
            //if (point <= dtStudentStep.Rows.Count)
            //{

            //}





            //return tbStudentStep;

        }
    }
}