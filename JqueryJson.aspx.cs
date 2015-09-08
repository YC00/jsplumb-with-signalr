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

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class JqueryJson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            //using (var l_oConnection = new SqlConnection(constr))
            //{
            //    try
            //    {
            //        l_oConnection.Open();
            //        Label1.Text = "Success";
            //    }
            //    catch (SqlException)
            //    {
            //        Label1.Text = "Failed";
            //    } 
            //}
        }
        [WebMethod]
        public static string InsertData(string username, string subj, string desc)
        {
            string msg = string.Empty;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("insert into TEMP_User(Name,Subject,Description) VALUES(@name,@subject,@desc)", con))
                {

                    try
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@name", username);
                        cmd.Parameters.AddWithValue("@subject", subj);
                        cmd.Parameters.AddWithValue("@desc", desc);
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

    }
}