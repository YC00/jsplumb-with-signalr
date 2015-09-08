using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.AspNet.SignalR.Samples.App_Code;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.admin
{
    public partial class EditGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //載入群組TreeView
            LoadGroupTree();
            //加入刪除節點警告視窗
            btnDeleteNode.Attributes.Add("onclick", "return confirm('確定要刪除此節點')");
        }
        //載入群組TreeView
        protected void LoadGroupTree()
        {
            //先清空TreeView(避免重複出現)
            tvGroup.Nodes.Clear();
            //定義TreeRoot
            TreeNode tnRoot = new TreeNode();
            tnRoot.Value = "1_Root";
            tnRoot.Text = "國立成功大學";
            //取得最上層Node(SchoolGroup)
            DataTable dtSchoolGroup = clsGroupManagement.ORCS_SchoolGroup_SELECT_by_SchoolGroupID("%");
            if (dtSchoolGroup.Rows.Count > 0)
            {
                foreach (DataRow drSchoolGroup in dtSchoolGroup.Rows)
                {
                    //定義最上層Node(SchoolGroup)
                    TreeNode tnSchoolGroup = new TreeNode();
                    tnSchoolGroup.Value = drSchoolGroup["iSchoolGroupID"].ToString() + "_SchoolGroup";
                    tnSchoolGroup.Text = drSchoolGroup["cSchoolGroupName"].ToString();
                    //取得第二層Node(ClassGroup)
                    DataTable dtClassGroup = clsGroupManagement.ORCS_ClassGroup_SELECT_by_ParentGroupID(drSchoolGroup["iSchoolGroupID"].ToString());
                    if (dtClassGroup.Rows.Count > 0)
                    {
                        foreach (DataRow drClassGroup in dtClassGroup.Rows)
                        {
                            //定義第二層Node(ClassGroup)
                            TreeNode tnClassGroup = new TreeNode();
                            tnClassGroup.Value = drClassGroup["iClassGroupID"].ToString() + "_ClassGroup";
                            tnClassGroup.Text = drClassGroup["cClassGroupName"].ToString();
                            //取得第三層Node(TempGroup)
                            DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(drClassGroup["iClassGroupID"].ToString());
                            if (dtTempGroup.Rows.Count > 0)
                            {
                                foreach (DataRow drTempGroup in dtTempGroup.Rows)
                                {
                                    //定義第三層Node(TempGroup)
                                    TreeNode tnTempGroup = new TreeNode();
                                    tnTempGroup.Value = drTempGroup["iTempGroupID"].ToString() + "_TempGroup";
                                    tnTempGroup.Text = drTempGroup["cTempGroupName"].ToString();
                                    //將第三層Node加入第二層Node底下
                                    tnClassGroup.ChildNodes.Add(tnTempGroup);
                                }
                            }
                            //將第二層Node加入最上層Node底下
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
        //選擇不同TreeView的Node發生之事件
        protected void tvGroup_SelectedNodeChanged(object sender, EventArgs e)
        {
            //將所選擇到的Node存入hfNodeValue，供其他按鈕事件使用
            hfNodeValue.Value = tvGroup.SelectedNode.Value;
            //定義所選擇的NodeID
            string strNodeID = tvGroup.SelectedNode.Value.Split('_')[0];
            //定義所選擇的群組
            string strGroup = tvGroup.SelectedNode.Value.Split('_')[1];
            //定義所選擇到的Node名稱
            string strNodeName = tvGroup.SelectedNode.Text;
            //定義所選擇不同的群組所發生之事件
            switch (strGroup)
            {
                case "Root": // 選擇RootNode
                    tbName.Enabled = false;
                    tbChildName.Enabled = true;
                    btnEditName.Enabled = false;
                    btnAddChild.Enabled = true;
                    btnDeleteNode.Enabled = false;
                    tbName.Text = strNodeName;
                    break;
                case "SchoolGroup": // 選擇第一層Node
                    tbName.Enabled = true;
                    tbChildName.Enabled = true;
                    btnEditName.Enabled = true;
                    btnAddChild.Enabled = true;
                    btnDeleteNode.Enabled = true;
                    tbName.Text = strNodeName;
                    break;
                case "ClassGroup": // 選擇第二層Node
                    tbName.Enabled = true;
                    tbChildName.Enabled = true;
                    btnEditName.Enabled = true;
                    btnAddChild.Enabled = true;
                    btnDeleteNode.Enabled = true;
                    tbName.Text = strNodeName;
                    break;
                case "TempGroup": // 選擇第三層Node
                    tbName.Enabled = true;
                    tbChildName.Enabled = false;
                    btnEditName.Enabled = true;
                    btnAddChild.Enabled = false;
                    btnDeleteNode.Enabled = true;
                    tbName.Text = strNodeName;
                    //載入小組成員
                    Init_MemberList(strNodeID);
                    //載入主席
                    Init_ChairMan(strNodeID);
                    //載入紀錄者
                    Init_RecordMan(strNodeID);
                    break;
            }
        }
        //「修改名稱」事件
        protected void btnEditName_Click(object sender, EventArgs e)
        {
            //定義所選擇的NodeID
            string strNodeID = hfNodeValue.Value.Split('_')[0];
            //定義所選擇的群組
            string strGroup = hfNodeValue.Value.Split('_')[1];
            //定義所選擇到的Node名稱
            string strNodeName = tbName.Text;
            //若欄位為空則不能增加
            if (strNodeName == "")
            {
                Response.Write("<script>alert('名稱不能為空')</script>");
            }
            else
            {
                //定義所選擇不同的群組所發生之事件
                switch (strGroup)
                {
                    case "SchoolGroup": // 選擇第一層Node
                        clsGroupManagement.ORCS_SchoolGroup_UPDATE(strNodeID, strNodeName);
                        break;
                    case "ClassGroup": // 選擇第二層Node
                        clsGroupManagement.ORCS_ClassGroup_UPDATE(strNodeID, strNodeName);
                        break;
                    case "TempGroup": // 選擇第三層Node
                        clsGroupManagement.ORCS_TempGroup_UPDATE(strNodeID, strNodeName);
                        break;
                }
            }
            //清空名稱欄位
            tbName.Text = "";
            //載入群組TreeView
            LoadGroupTree();
        }
        //「增加」事件
        protected void btnAddChild_Click(object sender, EventArgs e)
        {
            //定義所選擇的NodeID
            string strNodeID = hfNodeValue.Value.Split('_')[0];
            //定義所選擇的群組
            string strGroup = hfNodeValue.Value.Split('_')[1];
            //定義所選擇到的Node名稱
            string strChildNodeName = tbChildName.Text;
            //若欄位為空則不能增加
            if (strChildNodeName == "")
            {
                Response.Write("<script>alert('子節點名稱不能為空')</script>");
            }
            else
            {
                //定義所選擇不同的群組所發生之事件
                switch (strGroup)
                {
                    case "Root": // 選擇RootNode
                        clsGroupManagement.ORCS_SchoolGroup_INSERT(strChildNodeName);
                        break;
                    case "SchoolGroup": // 選擇第一層Node
                        clsGroupManagement.ORCS_ClassGroup_INSERT(strChildNodeName, strNodeID);
                        break;
                    case "ClassGroup": // 選擇第二層Node
                        clsGroupManagement.ORCS_TempGroup_INSERT(strChildNodeName, strNodeID);
                        break;
                }
            }
            //清空子節點名稱欄位
            tbChildName.Text = "";
            //載入群組TreeView
            LoadGroupTree();
        }
        //「刪除節點」事件
        protected void btnDeleteNode_Click(object sender, EventArgs e)
        {
            //定義所選擇的NodeID
            string strNodeID = hfNodeValue.Value.Split('_')[0];
            //定義所選擇的群組
            string strGroup = hfNodeValue.Value.Split('_')[1];
            //定義所選擇不同的群組所發生之事件
            switch (strGroup)
            {
                case "SchoolGroup": // 選擇第一層Node
                    //找出子群組並刪除
                    DataTable dtClassGroup = clsGroupManagement.ORCS_ClassGroup_SELECT_by_ParentGroupID(strNodeID);
                    if (dtClassGroup.Rows.Count > 0)
                    {
                        //刪除群組人員
                        foreach (DataRow drClassGroup in dtClassGroup.Rows)
                        {
                            DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(drClassGroup["iClassGroupID"].ToString());
                            if (dtTempGroup.Rows.Count > 0)
                            {
                                foreach (DataRow drTempGroup in dtTempGroup.Rows)
                                {
                                    //刪除第三層人員
                                    clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(drTempGroup["iTempGroupID"].ToString(), "TempGroup");
                                }
                            }
                            //刪除第二層人員
                            clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(drClassGroup["iClassGroupID"].ToString(), "ClassGroup");
                        }
                        //刪除第一層人員
                        clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(strNodeID, "SchoolGroup");
                        //刪除第三層Node
                        clsGroupManagement.ORCS_TempGroup_DELETE_by_ParentGroupID(dtClassGroup.Rows[0]["iClassGroupID"].ToString());
                    }
                    //刪除第二層Node
                    clsGroupManagement.ORCS_ClassGroup_DELETE_by_ParentGroupID(strNodeID);
                    //刪除第一層Node
                    clsGroupManagement.ORCS_SchoolGroup_DELETE_by_SchoolGroupID(strNodeID);
                    break;
                case "ClassGroup": // 選擇第二層Node
                    //刪除群組人員
                    DataTable dtTempGroup2 = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(strNodeID);
                    if (dtTempGroup2.Rows.Count > 0)
                    {
                        foreach (DataRow drTempGroup2 in dtTempGroup2.Rows)
                        {
                            //刪除第三層人員
                            clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(drTempGroup2["iTempGroupID"].ToString(), "TempGroup");
                        }
                    }
                    //刪除第二層人員
                    clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(strNodeID, "ClassGroup");
                    //刪除第三層Node
                    clsGroupManagement.ORCS_TempGroup_DELETE_by_ParentGroupID(strNodeID);
                    //刪除第二層Node
                    clsGroupManagement.ORCS_ClassGroup_DELETE_by_ClassGroupID(strNodeID);
                    break;
                case "TempGroup": // 選擇第三層Node
                    //刪除第三層人員
                    clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(strNodeID, "TempGroup");
                    //刪除第三層Node
                    clsGroupManagement.ORCS_TempGroup_DELETE_by_TempGroupID(strNodeID);
                    break;
            }
            //清空TextBox欄位
            tbName.Text = "";
            tbChildName.Text = "";
            //載入群組TreeView
            LoadGroupTree();
        }
        //載入小組成員
        protected void Init_MemberList(string strGroupID)
        {
            MemberList.Items.Clear();
            //取得小組成員
            DataTable dtMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(strGroupID, "TempGroup");
            for (int i = 0; i < dtMember.Rows.Count; i++)
            {
                //轉帳號_全名
                DataTable dtName = clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID(dtMember.Rows[i]["cUserID"].ToString());
                //檢查是否為主席和紀錄者
                DataTable dtChairMan = clsGroupManagement.ORCS_MeetingIdentity_SELECT_by_GroupID_IdentityType(strGroupID, "ChairMan");
                DataTable dtRecordMan = clsGroupManagement.ORCS_MeetingIdentity_SELECT_by_GroupID_IdentityType(strGroupID, "RecordMan");
                if (dtChairMan.Rows.Count != 0 && dtRecordMan.Rows.Count != 0)
                {
                    if (dtMember.Rows[i]["cUserID"].ToString() != dtChairMan.Rows[0]["cUserID"].ToString() && dtMember.Rows[i]["cUserID"].ToString() != dtRecordMan.Rows[0]["cUserID"].ToString())
                    {
                        MemberList.Items.Add(dtMember.Rows[i]["cUserID"].ToString() + "_" + dtName.Rows[0]["cUserName"].ToString());
                    }
                }
                else
                {
                    MemberList.Items.Add(dtMember.Rows[i]["cUserID"].ToString() + "_" + dtName.Rows[0]["cUserName"].ToString());
                }
            }
        }
        //載入主席
        protected void Init_ChairMan(string strGroupID)
        {
            ChairmanList.Items.Clear();
            //取得主席
            DataTable dtChairMan = clsGroupManagement.ORCS_MeetingIdentity_SELECT_by_GroupID_IdentityType(strGroupID, "ChairMan");
            if (dtChairMan.Rows.Count != 0)
            {
                //轉帳號_全名
                DataTable dtName = clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID(dtChairMan.Rows[0]["cUserID"].ToString());
                ChairmanList.Items.Add(dtChairMan.Rows[0]["cUserID"].ToString() + "_" + dtName.Rows[0]["cUserName"].ToString());
            }
        }
        //載入紀錄者
        protected void Init_RecordMan(string strGroupID)
        {
            RecordmanList.Items.Clear();
            //取得紀錄者
            DataTable dtRecordMan = clsGroupManagement.ORCS_MeetingIdentity_SELECT_by_GroupID_IdentityType(strGroupID, "RecordMan");
            if (dtRecordMan.Rows.Count != 0)
            {
                //轉帳號_全名
                DataTable dtName = clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID(dtRecordMan.Rows[0]["cUserID"].ToString());
                RecordmanList.Items.Add(dtRecordMan.Rows[0]["cUserID"].ToString() + "_" + dtName.Rows[0]["cUserName"].ToString());
            }
        }
        //加入主席
        protected void btnAddChairMan_Click(object sender, EventArgs e)
        {
            //更換主席
            if (MemberList.SelectedIndex != -1)
            {
                if (ChairmanList.Items.Count == 0)
                {
                    ChairmanList.Items.Add(MemberList.SelectedItem.Text);
                    MemberList.Items.RemoveAt(MemberList.SelectedIndex);
                }
                else
                {
                    MemberList.Items.Add(ChairmanList.Items[0].Text);
                    ChairmanList.Items.RemoveAt(0);
                    ChairmanList.Items.Add(MemberList.SelectedItem.Text);
                    MemberList.Items.RemoveAt(MemberList.SelectedIndex);
                }
            }
        }
        //加入紀錄者
        protected void btnAddRecordMan_Click(object sender, EventArgs e)
        {
            //更換紀錄者
            if (MemberList.SelectedIndex != -1)
            {
                if (RecordmanList.Items.Count == 0)
                {
                    RecordmanList.Items.Add(MemberList.SelectedItem.Text);
                    MemberList.Items.RemoveAt(MemberList.SelectedIndex);
                }
                else
                {
                    MemberList.Items.Add(RecordmanList.Items[0].Text);
                    RecordmanList.Items.RemoveAt(0);
                    RecordmanList.Items.Add(MemberList.SelectedItem.Text);
                    MemberList.Items.RemoveAt(MemberList.SelectedIndex);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //定義所選擇的NodeID
            string strNodeID = hfNodeValue.Value.Split('_')[0];
            //定義所選擇的群組
            string strGroup = hfNodeValue.Value.Split('_')[1];
            //定義所選擇到的Node名稱
            string strChildNodeName = tbChildName.Text;
            //刪除ORCS_MeetingIdentity資料表主席和紀錄者資訊
            clsGroupManagement.ORCS_MeetingIdentity_DELETE_by_GroupID(strNodeID);
            //儲存新增的主席資訊
            if (ChairmanList.Items.Count != 0)
            {
                clsGroupManagement.ORCS_MeetingIdentity_INSERT(strNodeID, ChairmanList.Items[0].Text.Split('_')[0].ToString(), "ChairMan");
            }
            if (RecordmanList.Items.Count != 0)
            {
                clsGroupManagement.ORCS_MeetingIdentity_INSERT(strNodeID, RecordmanList.Items[0].Text.Split('_')[0].ToString(), "RecordMan");
            }
        }
    }
}