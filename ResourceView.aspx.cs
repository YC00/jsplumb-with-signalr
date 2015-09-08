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
    public partial class ResourceView : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection strcon = new SqlConnection(strConnString);
            strcon.Open();
            string ADDStr = "select id, title, description, url, image, video from ConceptMap_Keyword where id='" +  Request.QueryString["id"] + "'";
            SqlCommand ADDCmd = new SqlCommand(ADDStr, strcon);

            DataTable table = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(ADDCmd);

            adapter.Fill(table);
            title.Text = table.Rows[0]["title"].ToString();
            description.Text = table.Rows[0]["description"].ToString();
            if (table.Rows[0]["url"].ToString() != "")
            {
                url.Text = table.Rows[0]["title"].ToString();
                urlLabel.Visible = true;
                url.NavigateUrl = table.Rows[0]["url"].ToString();
            }
            if (table.Rows[0]["image"].ToString() != "")
            {
                image.Text = "<div class=\"panel panel-default\"><div class=\"panel-body\"><img src=\"images/keyword/" + table.Rows[0]["image"].ToString() + "\" width=\"320\" /></div></div>";
                //imageValue.ImageUrl = "images/keyword/" + table.Rows[0]["image"].ToString();
            }
            if (table.Rows[0]["video"].ToString()!="")
            video.Text = "<div class=\"panel panel-default\"><div class=\"panel-body\"><video width=\"320\" height=\"240\"  style=\"margin:0 auto;\" controls><source src=\"images/keyword/" + table.Rows[0]["video"].ToString() + "\" type=\"video/" + table.Rows[0]["video"].ToString().Split('.').Last() + "\">Your browser does not support the video tag.</video></div></div>";
        }
    }
}