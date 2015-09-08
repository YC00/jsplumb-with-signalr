using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class login : System.Web.UI.Page
    {
        protected void ValidateUser1(object sender, EventArgs e)
        {
            int userId = 0;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT count(cUserId) FROM ORCS_User WHERE cUsername = @Username AND [cPassword] = @Password"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", Login1.UserName);
                    cmd.Parameters.AddWithValue("@Password", Login1.Password);
                    cmd.Connection = con;
                    con.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
                switch (userId)
                {
                    case -1:
                        Login1.FailureText = "使用者帳號或密碼錯誤";
                        break;
                    case -2:
                        Login1.FailureText = "Account has not been activated.";
                        break;
                    default:
                        FormsAuthentication.SetAuthCookie(Login1.UserName, true);
                        Response.Redirect("index.aspx"); 
                        break;
                }
            }
        }
    }
}