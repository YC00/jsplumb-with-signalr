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
    public partial class selectkeyword : System.Web.UI.Page
    {
        DataTable dt;
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            BindResourceGrid();
        }
        public void BindResourceGrid()
        {
            try
            {
                SqlConnection strcon = new SqlConnection(strConnString);
                strcon.Open();
                string ADDStr = "select * from ConceptMap_Keyword";
                SqlCommand ADDCmd = new SqlCommand(ADDStr, strcon);

                DataTable table = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(ADDCmd);

                adapter.Fill(table);
                strcon.Close();
                dt = table;
                GridView1.DataSource = table;
                GridView1.DataBind();

                //Fetch data from mysql database
                //string connString = ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString;
                //MySqlConnection conn = new MySqlConnection(connString);
                //conn.Open();
                //string cmd = "select * from tblCountry limit 10";
                //MySqlDataAdapter dAdapter = new MySqlDataAdapter(cmd, conn);
                //DataSet ds = new DataSet();
                //dAdapter.Fill(ds);
                //dt = ds.Tables[0];
                ////Bind the fetched data to gridview
                //GridView1.DataSource = dt;
                //GridView1.DataBind();

            }
            catch (SqlException ex)
            {
                System.Console.Error.Write(ex.Message);

            }

        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName.Equals("detail"))
            {
                //int index = (int)GridView1.DataKeys[index].Value;
                GridViewRow gvrow = GridView1.Rows[index];
                
                titleDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[2].Text).ToString();
                descriptionDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[3].Text);
                urlDetail.Width = 100;
                urlDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);
                imageDetail.ImageUrl = "images/keyword/" + HttpUtility.HtmlDecode(gvrow.Cells[5].Text);
                videoDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[6].Text);
                videoDetail.NavigateUrl = "images/keyword/" + HttpUtility.HtmlDecode(gvrow.Cells[6].Text);
                
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript", sb.ToString(), false);
           }

        }

    }
}