using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR.Samples.App_Code;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.admin
{
    public partial class GroupKeywordManagement : System.Web.UI.Page
    {
        DataTable dt;
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGroupTree();
        }

        public void BindGrid(string ORCSGroupID)
        {
            try
            {
                SqlConnection strcon = new SqlConnection(strConnString);
                strcon.Open();
                string ADDStr = "select id, title, description, url, image, video from ConceptMap_Keyword where ORCSGroupID='" + ORCSGroupID + "'";
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
                titleDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[3].Text).ToString();
                descriptionDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);
                urlDetail.Width = 100;
                urlDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[5].Text);
                imageDetail.ImageUrl = "../images/keyword/"+HttpUtility.HtmlDecode(gvrow.Cells[6].Text);
                videoDetail.NavigateUrl = "../images/keyword/" + HttpUtility.HtmlDecode(gvrow.Cells[7].Text);
                videoDetail.Text = HttpUtility.HtmlDecode(gvrow.Cells[7].Text);
                lblResult.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#detailModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DetailModalScript", sb.ToString(), false);
            }
            else if (e.CommandName.Equals("editRecord"))
            {
                editID.Value = GridView1.DataKeys[index].Value.ToString();
                GridViewRow gvrow = GridView1.Rows[index];
                titleEdit.Text = HttpUtility.HtmlDecode(gvrow.Cells[3].Text).ToString();
                descriptionEdit.Text = HttpUtility.HtmlDecode(gvrow.Cells[4].Text);
                urlEdit.Text = HttpUtility.HtmlDecode(gvrow.Cells[5].Text);
                lblResult.Visible = false;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#editModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditModalScript", sb.ToString(), false);

            }
            else if (e.CommandName.Equals("deleteRecord"))
            {
                string code = GridView1.DataKeys[index].Value.ToString();
                hfCode.Value = code;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script type='text/javascript'>");
                sb.Append("$('#deleteModal').modal('show');");
                sb.Append(@"</script>");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "DeleteModalScript", sb.ToString(), false);
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            String savePath = Server.MapPath("../images/keyword/");

            string title = titleEdit.Text;
            string description = descriptionEdit.Text;
            string url = urlEdit.Text;
            string image = "";
            Boolean fileOK = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (imageUpload.FileName.Length > 0)
            {
                String fileExtension =
                System.IO.Path.GetExtension(imageUpload.FileName).ToLower();
                String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
                if (fileOK)
                {
                    try
                    {
                        image = imageUpload.FileName.ToString();
                        imageUpload.SaveAs(savePath + image);
                    }
                    catch (Exception ex)
                    {
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("alert('圖片上傳錯誤');");
                        sb.Append(@"</script>");
                        //Response.Write("<script>alert('File could not be uploaded.');</script>";
                    }
                }
                else
                {
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("alert('圖片檔案格式錯誤');");
                    sb.Append(@"</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
                    //Label1.Text = "Cannot accept files of this type.";
                }

            }
            fileOK = false;
            string video = "";
            if (videoUpload.FileName.Length > 0)
            {
                String fileExtension =
                System.IO.Path.GetExtension(videoUpload.FileName).ToLower();
                String[] allowedExtensions = { ".avi", ".mov", ".mp4", ".mpg", ".wmv" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
                if (fileOK)
                {
                    try
                    {
                        video = videoUpload.FileName;
                        videoUpload.SaveAs(savePath + video);
                    }
                    catch (Exception ex)
                    {
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("alert('影像上傳錯誤');");
                        sb.Append(@"</script>");
                        //Response.Write("<script>alert('File could not be uploaded.');</script>";
                    }
                }
                else
                {
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("alert('影像檔案格式錯誤');");
                    sb.Append(@"</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
                    //Label1.Text = "Cannot accept files of this type.";
                }
            }
            executeUpdate(editID.Value.ToString(), title, description, url, image, video,hfNodeValue.Value);
            BindGrid(hfNodeValue.Value);
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("alert('已更新');");
            sb.Append("$('#editModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "EditHideModalScript", sb.ToString(), false);

        }

        private void executeUpdate(string id, string title, string description, string url, string image, string video, string ORCSGroupID)
        {
            string imgstr = "";
            string videostr = "";
            //string connString = ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString;
            try
            {
                if (image != "")
                    imgstr = ",image = '" + image + "'";
                if (video != "")
                    videostr = ",video ='" + video + "'";
                string strquery = "update ConceptMap_Keyword set title='" + title + "', description='" + description + "',url='" + url + "',ORCSGroupID='" + ORCSGroupID + "' " + imgstr + " " + videostr + " where id='" + id + "'";
                CreateCommand(strquery, strConnString);
            }
            catch (SqlException me)
            {
                System.Console.Write(me.Message);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("$('#addModal').modal('show');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddShowModalScript", sb.ToString(), false);

        }

        protected void btnAddRecord_Click(object sender, EventArgs e)
        {
            String savePath = Server.MapPath("../images/keyword/");

            string title = txtTitle.Text;
            string description = txtDescription.Text;
            string url = txtURL.Text;
            string image = "";
            Boolean fileOK = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (txtImageUpload.FileName.Length > 0)
            {
                String fileExtension =
                System.IO.Path.GetExtension(txtImageUpload.FileName).ToLower();
                String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
                if (fileOK)
                {
                    try
                    {
                        image = txtImageUpload.FileName.ToString();
                        txtImageUpload.SaveAs(savePath + image);
                    }
                    catch (Exception ex)
                    {
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("alert('圖片上傳錯誤');");
                        sb.Append(@"</script>");
                        //Response.Write("<script>alert('File could not be uploaded.');</script>";
                    }
                }
                else
                {
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("alert('圖片檔案格式錯誤');");
                    sb.Append(@"</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
                    //Label1.Text = "Cannot accept files of this type.";
                }

            }
            fileOK = false;
            string video = "";
            if (txtVideoUpload.FileName.Length > 0)
            {
                String fileExtension =
                System.IO.Path.GetExtension(txtVideoUpload.FileName).ToLower();
                String[] allowedExtensions = { ".avi", ".mov", ".mp4", ".mpg", ".wmv" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
                if (fileOK)
                {
                    try
                    {
                        video = txtVideoUpload.FileName;
                        txtVideoUpload.SaveAs(savePath + video);
                    }
                    catch (Exception ex)
                    {
                        sb.Append(@"<script type='text/javascript'>");
                        sb.Append("alert('影像上傳錯誤');");
                        sb.Append(@"</script>");
                        //Response.Write("<script>alert('File could not be uploaded.');</script>";
                    }
                }
                else
                {
                    sb.Append(@"<script type='text/javascript'>");
                    sb.Append("alert('影像檔案格式錯誤');");
                    sb.Append(@"</script>");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);
                    //Label1.Text = "Cannot accept files of this type.";
                }
            }

            //int population = Convert.ToInt32(txtTotalPopulation.Text);
            //int indyear = Convert.ToInt32(txtIndYear.Text);
            executeAdd(title, description, url, image, video, hfNodeValue.Value);
            BindGrid(hfNodeValue.Value);
            

            sb.Append(@"<script type='text/javascript'>");
            sb.Append("alert('已新增');");
            sb.Append("$('#addModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddHideModalScript", sb.ToString(), false);


        }

        private void executeAdd(string title, string description, string url, string image, string video, string ORCSGroupID)
        {
            //string connString = ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString;
            try
            {
                string strquery = "insert into ConceptMap_Keyword (title,description,url,image,video,ORCSGroupID) values " +
                                    "('" + title + "','" + description + "','" + url + "','" + image + "','" + video + "','" + ORCSGroupID + "')";
                CreateCommand(strquery, strConnString);
            }
            catch (SqlException me)
            {
                System.Console.Write(me.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string code = hfCode.Value;
            executeDelete(code);
            //Response.Write(code);
            BindGrid(hfNodeValue.Value);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            //sb.Append("alert('已刪除');");
            sb.Append("$('#deleteModal').modal('hide');");
            sb.Append(@"</script>");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "delHideModalScript", sb.ToString(), false);


        }

        private void executeDelete(string code)
        {
            //string connString = ConfigurationManager.ConnectionStrings["MySqlConnString"].ConnectionString;
            try
            {
                string strquery = "delete from ConceptMap_Keyword where id=" + code;
                CreateCommand(strquery, strConnString);
            }
            catch (SqlException me)
            {
                System.Console.Write(me.Message);
            }

        }
        private static void CreateCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        //載入群組TreeView
        protected void LoadGroupTree()
        {
            //先清空TreeView(避免重複出現)
            tvGroup.Nodes.Clear();
            //定義TreeRoot
            TreeNode tnRoot = new TreeNode();
            tnRoot.Value = "1_Root_NULL";
            tnRoot.Text = "國立成功大學";
            tnRoot.SelectAction = TreeNodeSelectAction.None;
            //取得最上層Node(SchoolGroup)
            DataTable dtSchoolGroup = clsGroupManagement.ORCS_SchoolGroup_SELECT_by_SchoolGroupID("%");
            if (dtSchoolGroup.Rows.Count > 0)
            {
                foreach (DataRow drSchoolGroup in dtSchoolGroup.Rows)
                {
                    //定義最上層Node(SchoolGroup)
                    TreeNode tnSchoolGroup = new TreeNode();
                    
                    tnSchoolGroup.Value = drSchoolGroup["iSchoolGroupID"].ToString() + "_SchoolGroup_NULL";
                    tnSchoolGroup.Text = drSchoolGroup["cSchoolGroupName"].ToString();
                    //取得第二層Node(ClassGroup)
                    DataTable dtClassGroup = clsGroupManagement.ORCS_ClassGroup_SELECT_by_ParentGroupID(drSchoolGroup["iSchoolGroupID"].ToString());
                    if (dtClassGroup.Rows.Count > 0)
                    {
                        foreach (DataRow drClassGroup in dtClassGroup.Rows)
                        {
                            //定義第二層Node(ClassGroup)
                            TreeNode tnClassGroup = new TreeNode();
                            tnClassGroup.Value = drClassGroup["iClassGroupID"].ToString() + "_ClassGroup_" + drClassGroup["iSchoolGroupID"].ToString();
                            tnClassGroup.Text = drClassGroup["cClassGroupName"].ToString();
                            //取得第三層Node(TempGroup)
                            //DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(drClassGroup["iClassGroupID"].ToString());
                            //if (dtTempGroup.Rows.Count > 0)
                            //{
                            //    foreach (DataRow drTempGroup in dtTempGroup.Rows)
                            //    {
                            //        //定義第三層Node(TempGroup)
                            //        TreeNode tnTempGroup = new TreeNode();
                            //        tnTempGroup.Value = drTempGroup["iTempGroupID"].ToString() + "_TempGroup_" + drTempGroup["iClassGroupID"].ToString();
                            //        tnTempGroup.Text = drTempGroup["cTempGroupName"].ToString();
                            //        //將第三層Node加入第二層Node底下
                            //        tnClassGroup.ChildNodes.Add(tnTempGroup);
                            //    }
                            //}
                            ////將第二層Node加入最上層Node底下
                            tnSchoolGroup.ChildNodes.Add(tnClassGroup);
                        }
                    }
                    //將最上層Node加入TreeRoot底下
                    tnRoot.ChildNodes.Add(tnSchoolGroup);
                }
            }
            
            //將TreeNode加入TreeView裡
            tvGroup.Nodes.Add(tnRoot);
            //展開所有Node
            tvGroup.ExpandAll();
        }
        protected void tvGroup_SelectedNodeChanged(object sender, EventArgs e)
        {
            ////將所選擇到的Node存入hfNodeValue
            hfNodeValue.Value = tvGroup.SelectedNode.Value;

            BindGrid(hfNodeValue.Value);
            //SqlCommand cmd = new SqlCommand(strQuery);

            ////GridView2.DataSource = GetData(cmd);
            ////GridView2.DataBind();

            //DataTable selectedTemplate = GetData(cmd);
            //// Declare an object variable. 
            //if (selectedTemplate.Rows.Count > 0)
            //{
            //    foreach (DataRow row in selectedTemplate.Rows) // Loop over the rows.
            //    {
            //        for (int x = 0; x < accordion.Controls.Count / 2; x++)
            //        {
            //            CheckBox chk = (CheckBox)accordion.FindControl("chk_1_" + x);
            //            TextBox txt = (TextBox)accordion.FindControl("txt_1_" + x);

            //            string templateID = chk.Attributes["value"] != null ? chk.Attributes["value"].ToString() : "";
            //            if (templateID == row["TemplateID"].ToString())
            //            {
            //                chk.Checked = true;
            //                txt.Text = row["TemplateColor"].ToString();
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    for (int x = 0; x < accordion.Controls.Count / 2; x++)
            //    {
            //        CheckBox chk = (CheckBox)accordion.FindControl("chk_1_" + x);
            //        TextBox txt = (TextBox)accordion.FindControl("txt_1_" + x);

            //        chk.Checked = false;
            //        txt.Text = "FFFFFF";

            //    }
            //}

        }
    }
}