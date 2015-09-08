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
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class NodeManagement : System.Web.UI.Page
    {
        const string key = "MyDataSource";
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
                //String queryString = "SELECT TemplateID, TemplateName FROM ConceptMap_Template";
                //DataSet ds = GetData(queryString);
                //ddlEmployee.DataSource = ds;
                //ddlEmployee.DataBind();
                SetInitialRow();
                bindTemplate();
                //bindTemplateGroup();
                SetDropDown1();
                GenerateControls();
                //bindGeneralNode();
            }
            else 
            {
                GenerateControls();
            }
            
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            //lblSelectedValue.Text = cboCountry.SelectedValue;

        }
        private void BindData()
        {
            string strQuery = "";
            if (Request.QueryString["id"] != "")
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication where NodeID = '" + Request.QueryString["id"].ToString() + "'";
            else
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication";

            SqlCommand cmd = new SqlCommand(strQuery);
            //GridView1.DataSource = GetData(cmd);
            //GridView1.DataBind();
            if (Request.QueryString["id"] != "")
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication where NodeID = '" + Request.QueryString["id"].ToString() + "'";
            else
                strQuery = "select NodeApplicationID, FieldName, FieldValue, OptionName, OptionValue" +
                               " from ConceptMap_NodeApplication";

            cmd = new SqlCommand(strQuery);
            //GridView2.DataSource = GetData(cmd);
            //GridView2.DataBind();
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
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            //GridView1.PageIndex = e.NewPageIndex;
            //GridView1.DataBind();
        }

        protected void AddNewCustomer(object sender, EventArgs e)
        {
            //string CustomerID = ((TextBox)GridView1.FooterRow.FindControl("txtCustomerID")).Text;
            //string Name = ((TextBox)GridView1.FooterRow.FindControl("txtContactName")).Text;
            //string Company = ((TextBox)GridView1.FooterRow.FindControl("txtCompany")).Text;
            //SqlConnection con = new SqlConnection(strConnString);
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "insert into ConceptMap_NodeApplication(NodeApplicationID, FieldName, FieldValue) " +
            //"values(@NodeApplicationID, @FieldName, @FieldValue);" +
            // "select NodeApplicationID, FieldName, FieldValue from ConceptMap_NodeApplication";
            //cmd.Parameters.Add("@NodeApplicationID", SqlDbType.VarChar).Value = CustomerID;
            //cmd.Parameters.Add("@FieldName", SqlDbType.VarChar).Value = Name;
            //cmd.Parameters.Add("@FieldValue", SqlDbType.VarChar).Value = Company;
            //GridView1.DataSource = GetData(cmd);
            //GridView1.DataBind();
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
            //GridView1.DataSource = GetData(cmd);
            //GridView1.DataBind();
        }
        protected void EditCustomer(object sender, GridViewEditEventArgs e)
        {
            //GridView1.EditIndex = e.NewEditIndex;
            BindData();
        }
        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            //GridView1.EditIndex = -1;
            BindData();
        }
        protected void UpdateCustomer(object sender, GridViewUpdateEventArgs e)
        {
            //string CustomerID = ((Label)GridView1.Rows[e.RowIndex].FindControl("lblCustomerID")).Text;
            //string Name = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtContactName")).Text;
            //string Company = ((TextBox)GridView1.Rows[e.RowIndex].FindControl("txtCompany")).Text;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "update customers set ContactName=@ContactName,CompanyName=@CompanyName " +
            // "where CustomerID=@CustomerID;" +
            // "select CustomerID,ContactName,CompanyName from customers";
            //cmd.Parameters.Add("@CustomerID", SqlDbType.VarChar).Value = CustomerID;
            //cmd.Parameters.Add("@ContactName", SqlDbType.VarChar).Value = Name;
            //cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = Company;
            //GridView1.EditIndex = -1;
            //GridView1.DataSource = GetData(cmd);
            //GridView1.DataBind();
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
        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DataRow dr = null;
            DataRow dr2 = null;
            dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt.Columns.Add(new DataColumn("Column3", typeof(string)));
            
            dt2.Columns.Add(new DataColumn("RowNumber", typeof(string)));
            dt2.Columns.Add(new DataColumn("Column1", typeof(string)));
            dt2.Columns.Add(new DataColumn("Column2", typeof(string)));
            dt2.Columns.Add(new DataColumn("Column3", typeof(string)));
            
            dr = dt.NewRow();
            dr["RowNumber"] = 1;
            dr["Column1"] = "123";
            dr["Column2"] = "abc";
            dr["Column3"] = string.Empty;
            
            dr2 = dt2.NewRow();
            dr2["RowNumber"] = 1;
            dr2["Column1"] = string.Empty;
            dr2["Column2"] = string.Empty;
            dr2["Column3"] = string.Empty;
            
            dt.Rows.Add(dr);
            dt2.Rows.Add(dr2);
            
            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;
            ViewState["CurrentTable2"] = dt2;
            
            Gridview1.DataSource = dt;
            Gridview1.DataBind();

            Gridview2.DataSource = dt2;
            Gridview2.DataBind();
        }
        private void AddNewRowToGrid()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("FieldName1TextBox");
                        TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox1");
                        //DropDownList box3 = (DropDownList)Gridview1.Rows[rowIndex].Cells[3].FindControl("combobox");
                        //TextBox box3 = ;

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        //dtCurrentTable.Rows[i - 1]["Column3"] = box3.SelectedValue;

                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();
                }
                //Session[key] = dtCurrentTable;
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
        }

        private void AddNewRowToGrid2()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable2"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable2"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)Gridview2.Rows[rowIndex].Cells[1].FindControl("FieldName2TextBox");
                        TextBox box2 = (TextBox)Gridview2.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                        //DropDownList box3 = (DropDownList)Gridview2.Rows[rowIndex].Cells[3].FindControl("combobox2");
                        //TextBox box3 = ;

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;

                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        //dtCurrentTable.Rows[i - 1]["Column3"] = box3.SelectedValue;

                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable2"] = dtCurrentTable;

                    Gridview2.DataSource = dtCurrentTable;
                    Gridview2.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData2();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
        }
        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("FieldName1TextBox");
                        TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox1");
                        //DropDownList box3 = (DropDownList)Gridview1.Rows[rowIndex].Cells[3].FindControl("combobox");

                        box1.Text = dt.Rows[i]["Column1"].ToString();
                        box2.Text = dt.Rows[i]["Column2"].ToString();
                        //box3.Text = dt.Rows[i]["Column3"].ToString();

                        rowIndex++;
                    }
                }
            }
        }

        private void SetPreviousData2()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable2"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable2"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox box1 = (TextBox)Gridview2.Rows[rowIndex].Cells[1].FindControl("FieldName2TextBox");
                        TextBox box2 = (TextBox)Gridview2.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                        //DropDownList box3 = (DropDownList)Gridview2.Rows[rowIndex].Cells[3].FindControl("combobox2");

                       
                        box1.Text = dt.Rows[i]["Column1"].ToString();
                        box2.Text = dt.Rows[i]["Column2"].ToString();
                        //box3.Text = dt.Rows[i]["Column3"].ToString();

                        rowIndex++;
                    }
                }
            }
        }
        private void CopyNewRowToGeneralGrid()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                int num = 0;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("FieldName1TextBox");
                        TextBox box2 = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("TextBox1");
                        //DropDownList box3 = (DropDownList)Gridview1.Rows[rowIndex].Cells[3].FindControl("combobox");
                        //TextBox box3 = ;

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        num = i + 1;
                        dtCurrentTable.Rows[i - 1]["RowNumber"] = i;
                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        //dtCurrentTable.Rows[i - 1]["Column3"] = box3.SelectedValue;

                        rowIndex++;
                    }
                    //drCurrentRow["combobox"] = "abc123";
                    //drCurrentRow[""] = "abc123";
                    //dtCurrentTable.Rows.Add(drCurrentRow);
                    
                    foreach (GridViewRow gv in GridView3.Rows)
                    {
                        CheckBox chk = gv.FindControl("myCheckBox") as CheckBox;
                        //HiddenField hdValue = gv.FindControl("hdValue") as HiddenField;
                        if (chk.Checked)
                        {
                            if (gv.Cells[1].Text == "General")
                            {
                                dtCurrentTable.Rows.Add(num++, gv.Cells[2].Text, gv.Cells[3].Text, "");
                            }
                            //get only the rows you want
                            //DataRow[] results = dtMain.Select("ID=" + hdValue.Value + "");
                            //populate new destination table
                            //foreach (DataRow dr in results)
                            //{
                                
                            //}
                        }
                        //gridView2.DataSource = dtClone;
                        //gridView2.DataBind();
                    }
                    
                    ViewState["CurrentTable"] = dtCurrentTable;

                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();
                }
                //Session[key] = dtCurrentTable;
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
        }
        private void CopyNewRowToSpecificGrid()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable2"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable2"];
                DataRow drCurrentRow = null;
                int num = 0;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox box1 = (TextBox)Gridview2.Rows[rowIndex].Cells[1].FindControl("FieldName2TextBox");
                        TextBox box2 = (TextBox)Gridview2.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                        //DropDownList box3 = (DropDownList)Gridview1.Rows[rowIndex].Cells[3].FindControl("combobox");
                        //TextBox box3 = ;

                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNumber"] = i + 1;
                        num = i + 1;
                        dtCurrentTable.Rows[i - 1]["RowNumber"] = i;
                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        //dtCurrentTable.Rows[i - 1]["Column3"] = box3.SelectedValue;

                        rowIndex++;
                    }
                    //drCurrentRow["combobox"] = "abc123";
                    //drCurrentRow[""] = "abc123";
                    //dtCurrentTable.Rows.Add(drCurrentRow);

                    foreach (GridViewRow gv in GridView3.Rows)
                    {
                        CheckBox chk = gv.FindControl("myCheckBox") as CheckBox;
                        //HiddenField hdValue = gv.FindControl("hdValue") as HiddenField;
                        if (chk.Checked)
                        {
                            if (gv.Cells[1].Text == "Specific")
                            {
                                dtCurrentTable.Rows.Add(num++, gv.Cells[2].Text, gv.Cells[3].Text, "");
                            }
                            //get only the rows you want
                            //DataRow[] results = dtMain.Select("ID=" + hdValue.Value + "");
                            //populate new destination table
                            //foreach (DataRow dr in results)
                            //{

                            //}
                        }
                        //gridView2.DataSource = dtClone;
                        //gridView2.DataBind();
                    }

                    ViewState["CurrentTable2"] = dtCurrentTable;

                    Gridview2.DataSource = dtCurrentTable;
                    Gridview2.DataBind();
                }
                //Session[key] = dtCurrentTable;
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData2();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
        }
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
            //AddNewRowToGrid2();
           
        }

        protected void ButtonAdd_Click2(object sender, EventArgs e)
        {
         
            AddNewRowToGrid2();
        }

        protected void ButtonCopy_Click(object sender, EventArgs e)
        {
            //AddNewRowToGrid();
            //AddNewRowToGrid2();
            CopyNewRowToGeneralGrid();
            CopyNewRowToSpecificGrid();
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string StrQuery;
            try
            {
                using (SqlConnection conn = new SqlConnection(strConnString))
                {
                    using (SqlCommand comm = new SqlCommand())
                    {
                        comm.Connection = conn;
                        conn.Open();
                        StrQuery = @"INSERT INTO ConceptMap_Template(TemplateName) VALUES ('"
                               + TextBox1.Text + "');SELECT SCOPE_IDENTITY ();";
                        comm.CommandText = StrQuery;
                        int myID = Convert.ToInt32(comm.ExecuteScalar());
                        int check = 0;
                        //comm.ExecuteNonQuery();
                        for (int i = 0; i < Gridview1.Rows.Count; i++)
                        {
                            TextBox box1 = (TextBox)Gridview1.Rows[i].Cells[1].FindControl("FieldName1TextBox");
                            TextBox box2 = (TextBox)Gridview1.Rows[i].Cells[2].FindControl("TextBox1");
                            CheckBox box3 = (CheckBox)Gridview1.Rows[i].Cells[3].FindControl("CheckBox1");

                            if (box3.Checked == true)
                                check = 1;
                            else
                                check = 0;
                            StrQuery = @"INSERT INTO ConceptMap_TemplateNode(TemplateID, FieldName, FieldValue, Type, Show) VALUES ("
                                + myID + ", '"
                                + box1.Text + "', '"
                                + box2.Text + "', 'General',"
                                + check + ");";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();
                        }
                        for (int i = 0; i < Gridview2.Rows.Count; i++)
                        {
                            TextBox box1 = (TextBox)Gridview2.Rows[i].Cells[1].FindControl("FieldName2TextBox");
                            TextBox box2 = (TextBox)Gridview2.Rows[i].Cells[2].FindControl("TextBox2");
                            CheckBox box3 = (CheckBox)Gridview2.Rows[i].Cells[3].FindControl("CheckBox2");
                            if (box3.Checked == true)
                                check = 1;
                            else
                                check = 0;
                            StrQuery = @"INSERT INTO ConceptMap_TemplateNode(TemplateID, FieldName, FieldValue, Type, Show) VALUES ("
                                + myID + ", '"
                                + box1.Text + "', '"
                                + box2.Text + "', 'Specific',"
                                + check + ");";
                            comm.CommandText = StrQuery;
                            comm.ExecuteNonQuery();
                        }
                        
                    }
                }
                //GenerateControls();
            }
            catch 
            { }
        }
        protected void bindTemplate()
        {
            SqlConnection strcon1 = new SqlConnection(strConnString);
            strcon1.Open();
            string ADDStr = "SELECT TemplateID, TemplateName FROM ConceptMap_Template";
            SqlCommand ADDCmd = new SqlCommand(ADDStr, strcon1);
            
            DataTable table = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(ADDCmd);

            adapter.Fill(table);
           
            DropDownList2.DataSource = table;
            DropDownList2.DataValueField = "TemplateID"; //The Value of the DropDownList, to get it you should call ddlDepartments.SelectedValue;
            DropDownList2.DataTextField = "TemplateName"; //The Name shown of the DropDownList.
            DropDownList2.DataBind();
            //DropDownList2.Items.Insert(0, new ListItem("Select Node Template", string.Empty));

        }
        //protected void bindTemplateGroup()
        //{
        //    SqlConnection strcon1 = new SqlConnection(strConnString);
        //    strcon1.Open();
        //    string ADDStr = "SELECT TemplateGroupID, TemplateGroupName FROM ConceptMap_TemplateGroup";
        //    SqlCommand ADDCmd = new SqlCommand(ADDStr, strcon1);

        //    DataTable table = new DataTable();

        //    SqlDataAdapter adapter = new SqlDataAdapter(ADDCmd);

        //    adapter.Fill(table);

        //    nodetemplateselectlist.DataSource = table;
        //    nodetemplateselectlist.DataValueField = "TemplateGroupID"; //The Value of the DropDownList, to get it you should call ddlDepartments.SelectedValue;
        //    nodetemplateselectlist.DataTextField = "TemplateGroupName"; //The Name shown of the DropDownList.
        //    nodetemplateselectlist.DataBind();
        //    using (SqlConnection Cn = new SqlConnection(strConnString))
        //    {
        //        using (SqlCommand Cmd = new SqlCommand("SELECT TemplateGroupValueID, TemplateGroupID, TemplateID FROM ConceptMap_TemplateGroupValue WHERE (TemplateGroupID = 1)", Cn))
        //        {
        //            Cn.Open();

        //            //Cmd.Parameters.AddWithValue("@templateid", int.Parse(DropDownList2.SelectedValue));
        //            SqlDataReader Dr = Cmd.ExecuteReader();
        //            //讀取結果
        //            List<string> id = new List<string>();
        //            while (Dr.Read())
        //            {
        //                 id.Add(Dr["TemplateID"].ToString()); 
        //            }
        //            Dr.Close();
        //            string idlist = string.Join(",", id.ToArray());

        //            string strSQL = "SELECT FieldName, FieldValue FROM ConceptMap_TemplateNode WHERE (TemplateID IN (" + idlist + ")) ORDER BY TemplateID, Type";
        //            SqlConnection con = new SqlConnection(strConnString);
        //            SqlCommand myCommand = new SqlCommand(strSQL, Cn);
        //            SqlDataReader TemplateList = Cmd.ExecuteReader();
        //            if (TemplateList.HasRows)
        //            {
        //                NodeTemplateListView.DataSource = TemplateList;
        //                NodeTemplateListView.DataBind();
        //            }
                    
                    
        //            TemplateList.Close();
        //            Cn.Close();
        //        }

        //    }
        //    //DropDownList2.Items.Insert(0, new ListItem("Select Node Template", string.Empty));

        //}
        
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            

            
            SqlConnection strcon1 = new SqlConnection(strConnString);
            strcon1.Open();
            string ADDStr = "SELECT TemplateNodeID, TemplateID, FieldName, FieldValue FROM ConceptMap_TemplateNode where Type='General' order by FieldName";
            SqlCommand ADDCmd = new SqlCommand(ADDStr, strcon1);
            DataTable table = new DataTable();


            SqlDataAdapter adapter = new SqlDataAdapter(ADDCmd);

            adapter.Fill(table);
            string item = e.Row.Cells[0].Text;
            foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
            {
                if (button.CommandName == "Delete")
                {
                    button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                }
            }
            //DropDownList combobox = (DropDownList)e.Row.FindControl("combobox");
            //ddl.DataSource = someList;//the source of your dropdown
            //ddl.DataBind();

            //DropDownList combobox = (e.Row.FindControl("combobox") as DropDownList);
            //combobox.DataSource = table;
            //combobox.DataValueField = "FieldName"; //The Value of the DropDownList, to get it you should call ddlDepartments.SelectedValue;
            //combobox.DataTextField = "FieldName"; //The Name shown of the DropDownList.
            
            //combobox.DataBind();
            //combobox.Items.Insert(0, new ListItem(""));
            strcon1.Close();
            }
            
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["CurrentTable"] as DataTable;
            dt.Rows[index].Delete();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["RowNumber"] = i+1;
            }
            ViewState["CurrentTable"] = dt;

            Gridview1.DataSource = dt;
            Gridview1.DataBind();
            //ViewState["CurrentTable"] = dt;
            //BindGridView();
            SetPreviousData();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
            
        }
        protected void OnRowDataBoundSpecific(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SqlConnection strcon1 = new SqlConnection(strConnString);
                strcon1.Open();
                string ADDStr = "SELECT TemplateNodeID, TemplateID, FieldName, FieldValue FROM ConceptMap_TemplateNode where Type='Specific' order by FieldName";
                SqlCommand ADDCmd = new SqlCommand(ADDStr, strcon1);
                DataTable table = new DataTable();


                SqlDataAdapter adapter = new SqlDataAdapter(ADDCmd);

                adapter.Fill(table);

                string item = e.Row.Cells[0].Text;
                foreach (Button button in e.Row.Cells[2].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
                //DropDownList combobox = (DropDownList)e.Row.FindControl("combobox");
                //ddl.DataSource = someList;//the source of your dropdown
                //ddl.DataBind();

                //DropDownList combobox2 = (e.Row.FindControl("combobox2") as DropDownList);
                //combobox2.DataSource = table;
                //combobox2.DataValueField = "FieldName"; //The Value of the DropDownList, to get it you should call ddlDepartments.SelectedValue;
                //combobox2.DataTextField = "FieldName"; //The Name shown of the DropDownList.

                //combobox2.DataBind();
                //combobox2.Items.Insert(0, new ListItem(""));
                strcon1.Close();
            }
        }
        protected void OnRowDeletingSpecific(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["CurrentTable2"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable2"] = dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["RowNumber"] = i + 1;
            }
            Gridview2.DataSource = dt;
            Gridview2.DataBind();
            //ViewState["CurrentTable"] = dt;
            //BindGridView();
            SetPreviousData2();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection Cn = new SqlConnection(strConnString))
            {
                using (SqlCommand Cmd = new SqlCommand("select Type, FieldName,FieldValue from ConceptMap_TemplateNode where TemplateID = @templateid order by Type, FieldName", Cn))
                {
                    Cn.Open();

                    Cmd.Parameters.AddWithValue("@templateid", int.Parse(DropDownList2.SelectedValue));
                    SqlDataReader Dr = Cmd.ExecuteReader();
                    if (Dr.HasRows)
                    {
                        GridView3.DataSource = Dr;
                        GridView3.DataBind();
                    }
                    Dr.Close();

                    Cn.Close();
                }

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
        }
        protected void DropDownListNodeTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection Cn = new SqlConnection(strConnString))
            {
                using (SqlCommand Cmd = new SqlCommand("select Type, FieldName,FieldValue from ConceptMap_TemplateNode where TemplateID = @templateid order by Type, FieldName", Cn))
                {
                    Cn.Open();

                    Cmd.Parameters.AddWithValue("@templateid", int.Parse(DropDownList2.SelectedValue));
                    SqlDataReader Dr = Cmd.ExecuteReader();
                    if (Dr.HasRows)
                    {
                        GridView3.DataSource = Dr;
                        GridView3.DataBind();
                    }
                    Dr.Close();

                    Cn.Close();
                }

            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>disp_tab2();</script>", false);
        }
        private void SetDropDown1()
        {
            using (SqlConnection Cn = new SqlConnection(strConnString))
            {
                using (SqlCommand Cmd = new SqlCommand("SELECT Type, FieldName, FieldValue FROM ConceptMap_TemplateNode WHERE (TemplateID = (SELECT     TOP (1) TemplateID FROM ConceptMap_TemplateNode AS ConceptMap_TemplateNode_1 ORDER BY TemplateID)) ORDER BY Type, FieldName", Cn))
                {
                    Cn.Open();

                   
                    SqlDataReader Dr = Cmd.ExecuteReader();
                    if (Dr.HasRows)
                    {
                        
                        GridView3.DataSource = Dr;
                        GridView3.DataBind();
                    }
                    Dr.Close();

                    Cn.Close();
                }

            }

        }
        private void BindGridView()
        {
            if (ViewState["CurrentTable"] != null)
            {
                Gridview1.DataSource = ViewState["CurrentTable"];
                Gridview1.DataBind();
            }
            else
            {
                Gridview1.DataSource = (DataTable)Session[key];
                Gridview1.DataBind();
            }

        }
        public static CheckBox chck;
        public static TextBox[] textb = new TextBox[100];
        public static GridView[] grdv = new GridView[100];
        protected void GenerateControls()
        {
            string strQuery = "";

            SqlCommand cmd = new SqlCommand(strQuery);

            String strSQL = @"select TemplateID, TemplateName" +
                                   " from ConceptMap_Template";
            SqlConnection con = new SqlConnection(strConnString);
            //建立SQL命令對象
            SqlCommand myCommand = new SqlCommand(strSQL, con);
            con.Open();

            //得到Data結果集
            SqlDataReader myDataReader = myCommand.ExecuteReader();



            //讀取結果
            int i = 0;
            while (myDataReader.Read())
            {
                strQuery = "select FieldName, FieldValue,Type" +
                                   " from ConceptMap_TemplateNode " +
                                   "where " +
                                   "ConceptMap_TemplateNode.TemplateID=" + myDataReader["TemplateID"].ToString() +""+
                                   "and Show = 1";

                cmd = new SqlCommand(strQuery);

                //GridView2.DataSource = GetData(cmd);
                //GridView2.DataBind();
                chck = new CheckBox();
                textb[i] = new TextBox();
                grdv[i] = new GridView();
                Panel Panel1 = new Panel();
                Label label = new Label();
                grdv[i].ID = string.Format("grd_1_{0}", i);
                grdv[i].DataSource = GetData(cmd);
                grdv[i].CssClass = "table table-bordered";
                grdv[i].DataBind();
                Panel1.Style.Add(HtmlTextWriterStyle.ZIndex, "0");

                chck.ID = string.Format("chk_1_{0}", i);
                chck.Text = myDataReader["TemplateName"].ToString();
                chck.Attributes.Add("value", myDataReader["TemplateID"].ToString());

                //chck.AutoPostBack = true;
                //chck.CheckedChanged += CheckedChanged;

                //((CheckBox)chck).CheckedChanged += new System.EventHandler(this.ClickMe);
                //((CheckBox)chck).AutoPostBack = true;
                //chck.AutoPostBack = true;
                //chck.CheckedChanged += new EventHandler(chkDynamic_CheckedChanged);

                //chck[i].Style.Add(HtmlTextWriterStyle.ZIndex, "99999");
                //chck[i].Checked = true;
                HtmlGenericControl h3Yogesh = new HtmlGenericControl("h3");
                h3Yogesh.InnerText = "Section " + (i + 1).ToString();
                HtmlGenericControl divYogesh = new HtmlGenericControl("div");

                
                textb[i].ID = string.Format("txt_1_{0}", i);
                textb[i].Attributes.Add("class", "color form-control");
                //divYogesh.InnerHtml = "<p>Mauris mauris ante, blandit et, ultrices a, suscipit eget, quam. Integer ut neque. Vivamus nisi metus, molestie vel, gravida in, condimentum sitamet, nunc. Nam a nibh. Donec suscipit eros. Nam mi. Proin viverra leo ut odio. Curabitur malesuada. Vestibulum a velit eu ante scelerisque vulputate. </p>";
                
                label.Text = "Color: ";
                divYogesh.Controls.Add(label);
                divYogesh.Controls.Add(textb[i]);
                divYogesh.Controls.Add(grdv[i]);
                //divYogesh.Attributes.Add("height", "100%");
                //accordion.Style.Add(HtmlTextWriterStyle.ZIndex, "99999");
                //divYogesh.Attributes.Add("class", "myClass");
                //divYogesh.Attributes.Add("id", (i + 1).ToString());
                //divYogesh.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#" + i.ToString() + i.ToString() + i.ToString() + i.ToString() + i.ToString() + i.ToString());
                //accordion.Controls.Add(chck[i]);
                //accordion.Controls.Add(chck[i].Controls.Add(h3Yogesh));
                Panel1.Controls.Add(chck);
                //Panel1.Attributes.Add("height", "100%");
                //Panel1.Controls.Add(h3Yogesh);
                
                accordion.Controls.Add(Panel1);
                accordion.Controls.Add(divYogesh);
                i++;
            }
            myDataReader.Close();
            con.Close();
        }
        private void CheckedChanged(Object sender, EventArgs e)     {       }

        public class NodeTemplate
        {
            public string color { get; set; }
            public string templateID { get; set; }
            public string templateName { get; set; }
            public string templateData { get; set; }
            public string templateType { get; set; }
            public string templateShow { get; set; }
            public NodeTemplate(string color, string templateID)
            {
	            this.color = color;
                this.templateID = templateID;
            }
            public NodeTemplate(string color, string templateID, string templateName)
            {
                this.color = color;
                this.templateID = templateID;
                this.templateName = templateName;
            }
            public NodeTemplate(string color, string templateID, string templateName, string templateData)
            {
                this.color = color;
                this.templateID = templateID;
                this.templateName = templateName;
                this.templateData = templateData;
            }

            public NodeTemplate(string color, string templateID, string templateName, string templateData, string templateType)
            {
                this.color = color;
                this.templateID = templateID;
                this.templateName = templateName;
                this.templateData = templateData;
                this.templateType = templateType;
            }

            public NodeTemplate(string color, string templateID, string templateName, string templateData, string templateType, string templateShow)
            {
                this.color = color;
                this.templateID = templateID;
                this.templateName = templateName;
                this.templateData = templateData;
                this.templateType = templateType;
                this.templateShow = templateShow;
            }
        }
        protected void ButtonSelectNodeTemplate_Click(object sender, EventArgs e)
        {
            //int idx = 0;
            //CheckBox label = (CheckBox)accordion.FindControl("chk_1_0");
            //label.Text = "After Click I am changed!";
            
            List<NodeTemplate> nTemplate = new List <NodeTemplate>();
            NodeTemplate nodeTmp;

            IHubContext _nodetemplate = GlobalHost.ConnectionManager.GetHubContext<ShapeShare>();
            _nodetemplate.Clients.All.emptyNodeItems();
            for (int x = 0; x < accordion.Controls.Count/2; x++)
            //foreach (var control in accordion.Controls)
            {
             //   if (control is CheckBox)
                //((CheckBox)control).Text = "After Click I am changed!"
                CheckBox chk = (CheckBox)accordion.FindControl("chk_1_"+x);
                TextBox txt = (TextBox)accordion.FindControl("txt_1_"+x);
                GridView grd = (GridView)accordion.FindControl("grd_1_"+x);
                //control.Text = "After Click I am changed!";
                //Control ctl = accordion.Controls[x];
                if (((CheckBox)chk).Checked)
                {
                    var chkValue = chk.Attributes["value"] != null ? chk.Attributes["value"].ToString() : "";

                    nodeTmp = new NodeTemplate(txt.Text, chkValue.ToString(), chk.Text, DataTableToJSON((DataTable)grd.DataSource));
                    _nodetemplate.Clients.All.addNodeItems(chkValue.ToString(), txt.Text, chk.Text);
                    _nodetemplate.Clients.All.setTemplateData("template-"+chkValue.ToString(), DataTableToJSON((DataTable)grd.DataSource));
                    nTemplate.Add(nodeTmp);
                     //coding here
                    //Literal1.Text += ((TextBox)txt).Text;
                }
                //if (ctl is TextBox)             
                //{
                //   Literal1.Text = ((TextBox)ctl).ID;
                //}             
                //else if (ctl is CheckBox)
                //{                 
                //    Response.Write(" Checked : "
                //        + ((CheckBox)ctl).Checked.ToString() + " | "
                //        + Request.Form["chk" + idx.ToString()] + " "); 
                //    idx++;             
                //}         
            }
            //Session["nodeTemplate"] = nTemplate;
            if(nTemplate.Count>0)
                Application["nodeTemplate"] = nTemplate;
            //Literal1.Text = accordion.Controls.Count.ToString();

            //foreach (var control in form1.Controls)
            //{
            //    if (control is CheckBox)
            //    {
            //        if (((CheckBox)control).Checked)
            //        {
            //            //update
            //            Literal1.Text += ((CheckBox)control).Text;
            //        }
            //        else
            //        {
            //            //update another
            //        }
            //    }
            //}
        }

        public static string DataTableToJSON(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];

            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {

                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\":\"" + StrDc[i] + i.ToString() + "¾" + "\",";

            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();

            Sb.Append("[");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {

                string TempStr = HeadStr;

                for (int j = 0; j < Dt.Columns.Count; j++)
                {

                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().Trim());
                }
                //Sb.AppendFormat("{{{0}}},",TempStr);

                Sb.Append("{" + TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));

            if (Sb.ToString().Length > 0)
                Sb.Append("]");

            return StripControlChars(Sb.ToString());

        }
        //To strip control characters:

        //A character that does not represent a printable character but //serves to initiate a particular action.

        public static string StripControlChars(string s)
        {
            return Regex.Replace(s, @"[^\x20-\x7F]", "");
        }
        protected void ButtonSaveNodeTemplate_Click(object sender, EventArgs e)
        {
            //int idx = 0;
            //CheckBox label = (CheckBox)accordion.FindControl("chk_1_0");
            //label.Text = "After Click I am changed!";

            List<NodeTemplate> nTemplate = new List<NodeTemplate>();
            NodeTemplate nodeTmp;

            IHubContext _nodetemplate = GlobalHost.ConnectionManager.GetHubContext<ShapeShare>();
            _nodetemplate.Clients.All.emptyNodeItems();
            for (int x = 0; x < accordion.Controls.Count / 2; x++)
            //foreach (var control in accordion.Controls)
            {
                //   if (control is CheckBox)
                //((CheckBox)control).Text = "After Click I am changed!"
                CheckBox chk = (CheckBox)accordion.FindControl("chk_1_" + x);
                TextBox txt = (TextBox)accordion.FindControl("txt_1_" + x);
                //control.Text = "After Click I am changed!";
                //Control ctl = accordion.Controls[x];
                if (((CheckBox)chk).Checked)
                {
                    var chkValue = chk.Attributes["value"] != null ? chk.Attributes["value"].ToString() : "";

                    nodeTmp = new NodeTemplate(txt.Text, chkValue.ToString());

                    _nodetemplate.Clients.All.addNodeItems(chkValue.ToString(), txt.Text, chk.Text);

                    nTemplate.Add(nodeTmp);
                    //coding here
                    //Literal1.Text += ((TextBox)txt).Text;
                }
                //if (ctl is TextBox)             
                //{
                //   Literal1.Text = ((TextBox)ctl).ID;
                //}             
                //else if (ctl is CheckBox)
                //{                 
                //    Response.Write(" Checked : "
                //        + ((CheckBox)ctl).Checked.ToString() + " | "
                //        + Request.Form["chk" + idx.ToString()] + " "); 
                //    idx++;             
                //}         
            }
            //Session["nodeTemplate"] = nTemplate;
            Application["nodeTemplate"] = nTemplate;
            //Literal1.Text = accordion.Controls.Count.ToString();

            //foreach (var control in form1.Controls)
            //{
            //    if (control is CheckBox)
            //    {
            //        if (((CheckBox)control).Checked)
            //        {
            //            //update
            //            Literal1.Text += ((CheckBox)control).Text;
            //        }
            //        else
            //        {
            //            //update another
            //        }
            //    }
            //}
        }
    }
}