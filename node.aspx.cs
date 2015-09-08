using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class node : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindData();
                // Declare the query string.
                //String queryString =
                //  "SELECT FieldName, FieldValue FROM ConceptMap_NodeApplication where NodeID='0ADFEFB1-18A5-1DC3-6B58-C2A3203BBBF6'";

                // Run the query and bind the resulting DataSet
                // to the GridView control.
                //DataSet ds = GetData(queryString);
                //if (ds.Tables.Count > 0)
                //{
                //    AuthorsGridView.DataSource = ds;
                //    AuthorsGridView.DataBind();
                //}
               // else
                //{
                    //Message.Text = "Unable to connect to the database.";
                //}
                String queryString = "SELECT TemplateID, TemplateName FROM ConceptMap_Template";
                DataSet ds = GetData(queryString);
                ddlEmployee.DataSource = ds;
                ddlEmployee.DataBind();
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            //lblSelectedValue.Text = cboCountry.SelectedValue;

        }
        private void BindData()
        {
            string strQuery = "";
            if(Request.QueryString["id"]!="")
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication where NodeID = '" + Request.QueryString["id"].ToString() + "'";
            else
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication";
            
            SqlCommand cmd = new SqlCommand(strQuery);
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();
            if (Request.QueryString["id"] != "")
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication where NodeID = '" + Request.QueryString["id"].ToString() + "'";
            else
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication";

            cmd = new SqlCommand(strQuery);
            GridView2.DataSource = GetData(cmd);
            GridView2.DataBind();
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
            return dt;
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataBind();
        }

        protected void AddNewCustomer(object sender, EventArgs e)
        {
            string CustomerID = ((TextBox)GridView1.FooterRow.FindControl("txtCustomerID")).Text;
            string Name = ((TextBox)GridView1.FooterRow.FindControl("txtContactName")).Text;
            string Company = ((TextBox)GridView1.FooterRow.FindControl("txtCompany")).Text;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into ConceptMap_NodeApplication(NodeApplicationID, FieldName, FieldValue) " +
            "values(@NodeApplicationID, @FieldName, @FieldValue);" +
             "select NodeApplicationID, FieldName, FieldValue from ConceptMap_NodeApplication";
            cmd.Parameters.Add("@NodeApplicationID", SqlDbType.VarChar).Value = CustomerID;
            cmd.Parameters.Add("@FieldName", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@FieldValue", SqlDbType.VarChar).Value = Company;
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();
        }

        protected void DeleteCustomer(object sender, EventArgs e)
        {
            LinkButton lnkRemove = (LinkButton)sender;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from ConceptMap_NodeApplication where " +
            "NodeApplicationID=@NodeApplicationID;" +
             "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue from ConceptMap_NodeApplication";
            cmd.Parameters.Add("@NodeApplicationID", SqlDbType.VarChar).Value = lnkRemove.CommandArgument;
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();
        }
        protected void EditCustomer(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }
        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData();
        }
        protected void UpdateCustomer(object sender, GridViewUpdateEventArgs e)
        {
            string CustomerID = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblCustomerID")).Text;
            string Name = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtContactName")).Text;
            string Company = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtCompany")).Text;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update customers set ContactName=@ContactName,CompanyName=@CompanyName " +
             "where CustomerID=@CustomerID;" +
             "select CustomerID,ContactName,CompanyName from customers";
            cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar).Value = CustomerID;
            cmd.Parameters.Add("@ContactName", SqlDbType.VarChar).Value = Name;
            cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = Company;
            GridView1.EditIndex = -1;
            GridView1.DataSource = GetData(cmd);
            GridView1.DataBind();
        }
        DataSet GetData(String queryString)
        {

            // Retrieve the connection string stored in the Web.config file.
            String connectionString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;

            DataSet ds = new DataSet();

            try
            {
                // Connect to the database and run the query.
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

                // Fill the DataSet.
                adapter.Fill(ds);

            }
            catch (Exception ex)
            {

                // The connection failed. Display an error message.
                //Message.Text = "Unable to connect to the database.";

            }

            return ds;

        }
        public class Template
        {
            public int TemplateId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        //public List<Employee> GetEmployees([Control]int? ddlEmployee)
        //{
        //    List<Employee> employeeList = new List<Employee>();
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        employeeList.Add(new Employee
        //        {
        //            EmployeeId = i,
        //            FirstName = string.Format("First{0}", i),
        //            LastName = string.Format("Last{0}", i)
        //        });
        //    }
        //    return employeeList.Where(e => e.EmployeeId == ddlEmployee).ToList();
        //}
        
    }
}