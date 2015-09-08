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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["UserID"] != "" && Request.QueryString["LoginFrom"] == "HINTS")
            {
                string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT cAuthority FROM ORCS_User WHERE cUserID = @cUserID"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@cUserID", Request.QueryString["UserID"]);
                        //cmd.Parameters.AddWithValue("@Password", Login.Password);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            if (rdr.HasRows)
                            {
                                rdr.Read(); // get the first row
                                if(rdr.GetString(0)=="x")
                                    Response.Redirect("admin/index.aspx"); 
                                else
                                    Response.Redirect("groups.aspx?UserID=" + Request.QueryString["UserID"]); 
                            }
                        }
                        con.Close();
                    }
                }
            

                
            }
        }
        protected void ValidateUser(object sender, EventArgs e)
        {
            int userId = 0;
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT cUserId FROM ConceptMap_User WHERE cUsername = @Username AND [cPassword] = @Password"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", Login.UserName);
                    cmd.Parameters.AddWithValue("@Password", Login.Password);
                    cmd.Connection = con;
                    con.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
                switch (userId)
                {
                    case -1:
                        Login.FailureText = "使用者帳號或密碼錯誤";
                        break;
                    case -2:
                        Login.FailureText = "Account has not been activated.";
                        break;
                    default:
                        FormsAuthentication.RedirectFromLoginPage(Login.UserName, Login.RememberMeSet);
                        break;
                }
            }
        }
    }
}