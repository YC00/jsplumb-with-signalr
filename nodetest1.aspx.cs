using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Data.SqlClient;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class nodetest1 : System.Web.UI.Page
    {
        const string key = "MyDataSource";
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
                GenerateControls();
            }
        }
        private void BindGridView()
        {
            if (Session[key] == null)
            {
                gridView1.DataSource = GetDataSource();
                gridView1.DataBind();
            }
            else
            {
                gridView1.DataSource = (DataTable)Session[key];
                gridView1.DataBind();
            }

        }
        protected DataTable GetDataSource()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(int)).AutoIncrement = true;
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Age", typeof(int));
                DataColumn[] keys = new DataColumn[2];
                keys[0] = dt.Columns["ID"];
                dt.PrimaryKey = keys;
                dt.Rows.Add("1", "first object", 34);
                dt.Rows.Add("2", "second object", 24);
                dt.Rows.Add("3", "third object", 34);
                dt.Rows.Add("4", "fourth object", 24);
                dt.Rows.Add("5", "fifth object", 34);

                Session[key] = dt;
                return dt;
            }
            catch
            {
                return null;
            }
        }
        protected void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtMain = Session[key] as DataTable;
                //copy the schema of source table
                DataTable dtClone = dtMain.Clone();
                foreach (GridViewRow gv in gridView1.Rows)
                {
                    CheckBox chk = gv.FindControl("chkSelect") as CheckBox;
                    HiddenField hdValue = gv.FindControl("hdValue") as HiddenField;
                    if (chk.Checked)
                    {
                        //get only the rows you want
                        DataRow[] results = dtMain.Select("ID=" + hdValue.Value + "");
                        //populate new destination table
                        foreach (DataRow dr in results)
                        {
                            dtClone.ImportRow(dr);
                        }
                    }
                    gridView2.DataSource = dtClone;
                    gridView2.DataBind();
                }
            }
            catch
            {
                BindGridView();
            }

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
           
        }
        public static CheckBox[] chck = new CheckBox[100];
        public static TextBox[] textb = new TextBox[100];
        public static GridView[] grdv = new GridView[100];
        private void GenerateControls()
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
                strQuery = "select FieldName, FieldValue" +
                                   " from ConceptMap_TemplateNode " +
                                   "where " +
                                   "ConceptMap_TemplateNode.TemplateID=" + myDataReader["TemplateID"].ToString();

                cmd = new SqlCommand(strQuery);

                //GridView2.DataSource = GetData(cmd);
                //GridView2.DataBind();
                chck[i] = new CheckBox();
                textb[i] = new TextBox();
                grdv[i] = new GridView();
                Panel Panel1 = new Panel();
                grdv[i].DataSource = GetData(cmd);
                grdv[i].DataBind();
                Panel1.Style.Add(HtmlTextWriterStyle.ZIndex, "0");
                chck[i].ID = string.Format("chk_1_{0}", i);
                chck[i].Text = myDataReader["TemplateName"].ToString();
                //chck[i].Style.Add(HtmlTextWriterStyle.ZIndex, "99999");
                //chck[i].Checked = true;
                HtmlGenericControl h3Yogesh = new HtmlGenericControl("h3");
                h3Yogesh.InnerText = "Section " + (i + 1).ToString();
                HtmlGenericControl divYogesh = new HtmlGenericControl("div");

                textb[i].Text = "Color";
                textb[i].ID = string.Format("txt_1_{0}", i);
                textb[i].Attributes.Add("class", "color");

                //divYogesh.InnerHtml = "<p>Mauris mauris ante, blandit et, ultrices a, suscipit eget, quam. Integer ut neque. Vivamus nisi metus, molestie vel, gravida in, condimentum sitamet, nunc. Nam a nibh. Donec suscipit eros. Nam mi. Proin viverra leo ut odio. Curabitur malesuada. Vestibulum a velit eu ante scelerisque vulputate. </p>";
                divYogesh.Controls.Add(textb[i]);
                divYogesh.Controls.Add(grdv[i]);
                //accordion.Style.Add(HtmlTextWriterStyle.ZIndex, "99999");
                //divYogesh.Attributes.Add("class", "myClass");
                //divYogesh.Attributes.Add("id", (i + 1).ToString());
                //divYogesh.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#" + i.ToString() + i.ToString() + i.ToString() + i.ToString() + i.ToString() + i.ToString());
                //accordion.Controls.Add(chck[i]);
                //accordion.Controls.Add(chck[i].Controls.Add(h3Yogesh));
                Panel1.Controls.Add(chck[i]);
                //Panel1.Controls.Add(h3Yogesh);
                accordion.Controls.Add(Panel1);
                accordion.Controls.Add(divYogesh);
                i++;
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Session["divCount"] == null)
                Session["divCount"] = 1;
            else
            {
                int divID = (int)Session["divCount"];
                divID++;
                Session["divCount"] = divID;
            }

            HtmlGenericControl divYogesh = new HtmlGenericControl("div");
            divYogesh.Attributes.Add("class", "myClass");
            divYogesh.Attributes.Add("id", Session["divCount"].ToString());

            pnlYogesh.Controls.Add(divYogesh);
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
    }
}