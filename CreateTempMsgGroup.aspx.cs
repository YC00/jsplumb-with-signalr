using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public partial class CreateTempMsgGroup : System.Web.UI.Page
    {
        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //「上一步」按鈕事件
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            Page.RegisterClientScriptBlock("WindowOpen", "<script>window.open('../PushMessage/MessageModeChoose.aspx','MessageModeChoose', 'width=450, height=320, scrollbars=yes');window.close();</script>");
        }
        //「確定」按鈕事件
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (tbGroupName.Text != "") //判斷群組名稱是否為空
                if (tbGroupPsw.Text == tbGroupPswConfirm.Text) //判斷兩次輸入的密碼是否相同
                {
                    //將建立的群組存入PushMessage_TempGroup資料表
                    //clsHintsDB HintsDB = new clsHintsDB();
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(strConnString))
                        {
                            using (SqlCommand comm = new SqlCommand())
                            {
                                comm.Connection = conn;
                                conn.Open();
                                string strTempGroupID = "TempGroup_" + DateTime.Now.ToString("yyyyMMddHHmmss");//定義臨時推播訊息群組ID
                                string strSQL = "INSERT INTO PushMessage_TempGroup (cMsgGroupID, cMsgGroupName, cMsgGroupPsw, cUserID) VALUES ('" + strTempGroupID + "','" + tbGroupName.Text + "','" + tbGroupPsw.Text + "','demoUser01')";
                                comm.CommandText = strSQL;
                                comm.ExecuteNonQuery();
                                //將人員加入PushMessage_TempGroupMember資料表
                                strSQL = "INSERT INTO PushMessage_TempGroupMember (cMsgGroupID, cUserID) VALUES ('" + strTempGroupID + "','demoUser01')";
                                //HintsDB.ExecuteNonQuery(strSQL);
                                comm.CommandText = strSQL;
                                comm.ExecuteNonQuery();
                                //儲存完後回到訊息模式選擇頁面
                                Page.RegisterClientScriptBlock("WindowOpen", "<script>window.open('../PushMessage/MessageModeChoose.aspx','MessageModeChoose', 'width=450, height=320, scrollbars=yes');window.close();</script>");
                            }
                        }
                    }catch { }
                }
                else
                    Page.RegisterClientScriptBlock("alert", "<script>alert('兩次輸入的密碼不同，請重新輸入密碼')</script>");
            else
                Page.RegisterClientScriptBlock("alert", "<script>alert('請輸入群組名稱')</script>");
            
                
        }
    }
}