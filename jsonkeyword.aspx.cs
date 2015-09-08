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
    public partial class jsonkeyword : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection strcon = new SqlConnection(strConnString);
            strcon.Open();
            //string ADDStr = "select id, title, description, url, image, video from ConceptMap_Keyword where id='" + Request.QueryString["id"] + "'";
            string keywordStr = "select id, title from ConceptMap_Keyword";
            SqlCommand keywordCmd = new SqlCommand(keywordStr, strcon);
            SqlDataReader keywordReader = null;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("[");
            keywordReader = keywordCmd.ExecuteReader();
            while (keywordReader.Read())
            {
                sb.Append("\"" + keywordReader["title"].ToString() + "\",");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            jsonpanel.Text = sb.ToString();
        }
    }
}