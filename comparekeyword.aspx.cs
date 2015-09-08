using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenNLP.Tools.SentenceDetect;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class comparekeyword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string[] arr_T = {"ABC"};
            //string[] arr_S = {"DEF"};
            //string[,] arr_compare = CompareKeywords.KeywordCompare1(arr_T, arr_S, false);

            //int rowLength = arr_compare.GetLength(0);
            //int colLength = arr_compare.GetLength(1);

            //for (int i = 0; i < rowLength; i++)
            //{
            //    for (int j = 0; j < colLength; j++)
            //    {
            //        Literal1.Text += string.Format("{0} <br />", arr_compare[i, j]);
            //    }
            //}
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string[] arr_T = TextBox1.Text.ToString().Split(' ');
            //string[] arr_S = TextBox2.Text.ToString().Split(' ');
            //string[,] arr_compare = CompareKeywords.KeywordCompare1(arr_T, arr_S, false);

            //int rowLength = arr_compare.GetLength(0);
            //int colLength = arr_compare.GetLength(1);
            Literal1.Text = compareword(TextBox1.Text.ToString(), TextBox2.Text.ToString());
            //for (int i = 0; i < rowLength; i++)
            //{
            //    for (int j = 0; j < colLength; j++)
            //    {
            //        Literal1.Text += string.Format("{0}<br />", arr_compare[i, j]);
            //    }
            //}
        }
        public string compareword(string Tkeyword, string Skeyword)
        {
            string[] arr_T = Tkeyword.ToString().Split(' ');
            string[] arr_S = Skeyword.ToString().Split(' ');

            string[,] arr_compare = CompareKeywords.KeywordCompare1(arr_T, arr_S, false);

            int answer = 0;

            int rowLength = arr_compare.GetLength(0);
            int colLength = arr_compare.GetLength(1);
            //return arr_compare[rowLength - 1, colLength - 1].ToString();
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    string[] tmparr = { };
                    try
                    {
                        Response.Write(arr_compare[i, j].ToString()+"<br />");
                        tmparr = arr_compare[i, j].ToString().Split('$');
                        answer += Convert.ToInt32(tmparr[0]);
                    }
                    catch { }
                    
                }
            }
            if (answer == arr_T.Length)
                return "match";
            else if (answer < arr_T.Length && answer>0)
                return "partial match";
            else
                return "mismatch";
        }
    }
}