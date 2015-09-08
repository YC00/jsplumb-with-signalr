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
    public partial class groups : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            bindTemplate();
            loadUsername();
        }
        protected void bindTemplate()
        {
            SqlConnection strcon1 = new SqlConnection(strConnString);
            strcon1.Open();
            string sql = "SELECT iTempGroupID, cTempGroupName as title, '" + Request.QueryString["UserID"] + "' as userid, (select cUserName from ORCS_User where cUserID='" + Request.QueryString["UserID"] + "') as username, (SELECT REPLACE(REPLACE(cAuthority,'s','0'),'t','1') FROM ORCS_User WHERE (cUserID = '" + Request.QueryString["UserID"] + "')) as chairman, (SELECT COUNT(*) FROM ORCS_GroupMember WHERE (iGroupID = (SELECT iGroupID FROM ORCS_GroupMember AS ORCS_GroupMember_1 WHERE (cUserID = '" + Request.QueryString["UserID"] + "') AND (cGroupClassify = 'TempGroup'))) AND (cGroupClassify = 'TempGroup')) AS total from ORCS_TempGroup where iTempGroupID = (SELECT iGroupID FROM ORCS_GroupMember where cUserID='" + Request.QueryString["UserID"] + "' and cGroupClassify='TempGroup')";
            //Response.Write(sql);
            //SqlCommand cmd = new SqlCommand("SELECT userid,username,email,city FROM USERS where username=@username and password=@password", con);


            SqlCommand cmd = new SqlCommand(sql, strcon1);

            DataTable table = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(table);

            ListView1.DataSource = table;
            ListView1.DataBind();

            //DropDownList2.DataValueField = "TemplateID"; //The Value of the DropDownList, to get it you should call ddlDepartments.SelectedValue;
            //DropDownList2.DataTextField = "TemplateName"; //The Name shown of the DropDownList.
            //DropDownList2.DataBind();
            //DropDownList2.Items.Insert(0, new ListItem("Select Node Template", string.Empty));

        }
        protected void loadUsername() 
        {
            string strQuery = "select cUserID, cUserName, cAuthority" +
                               " from ORCS_User " +
                               "where " +
                               "cUserID='" + Request.QueryString["UserID"] + "'";
            
            SqlCommand cmd = new SqlCommand(strQuery);

            //GridView2.DataSource = GetData(cmd);
            //GridView2.DataBind();

            DataTable DT = GetData(cmd);
            // Declare an object variable. 

            adminmenu.Text = "<li><a id=\"admin\" href=\"Browsingtool.aspx\" data-placement=\"bottom\" ><i class=\"fa fa-wrench fa-fw\"></i>概念圖瀏覽工具</a></li>";

            if (DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows) // Loop over the rows.
                {
                    UserLabel.Text = row["cUserID"].ToString() + " " + row["cUserName"].ToString();
                    UserLabelTop.Text = "data-content=\"" + row["cUserID"].ToString() + " " + row["cUserName"].ToString() + "\"";
                    if(row["cAuthority"].ToString()=="t")
                        adminmenu.Text = "<li><a id=\"admin\" href=\"admin/index.aspx\" data-placement=\"bottom\" ><i class=\"fa fa-wrench fa-fw\"></i>管理介面</a></li>";
                    
                }
            }
        }
        private DataTable GetData(SqlCommand cmd)
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