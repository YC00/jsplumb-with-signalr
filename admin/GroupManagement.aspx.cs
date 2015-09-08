using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.SignalR.Samples.App_Code;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare.admin
{
    public partial class GroupManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //載入群組TreeView
            LoadGroupTree();
            //載入尚未分組名單
            LoadNonGroup();
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
                            DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(drClassGroup["iClassGroupID"].ToString());
                            if (dtTempGroup.Rows.Count > 0)
                            {
                                foreach (DataRow drTempGroup in dtTempGroup.Rows)
                                {
                                    //定義第三層Node(TempGroup)
                                    TreeNode tnTempGroup = new TreeNode();
                                    tnTempGroup.Value = drTempGroup["iTempGroupID"].ToString() + "_TempGroup_" + drTempGroup["iClassGroupID"].ToString();
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
        //載入尚未分組名單
        protected void LoadNonGroup()
        {
            ///////////////////////////////////////檢查是否有成員尚未加入名單中///////////////////////////////////////
            //檢查課程所有成員是否有分到組
            //建立尚未分組名單
            DataTable dtNonGroupMember = new DataTable();
            dtNonGroupMember.Columns.Add("SchoolName", typeof(String));
            dtNonGroupMember.Columns.Add("ClasslName", typeof(String));
            dtNonGroupMember.Columns.Add("UserID", typeof(String));
            dtNonGroupMember.Columns.Add("UserName", typeof(String));
            dtNonGroupMember.Columns.Add("ClassID", typeof(String));
            //取出課程
            DataTable dtAllClass = clsGroupManagement.ORCS_ClassGroup_SELECT_All();
            for (int i = 0; i < dtAllClass.Rows.Count; i++)
            {
                //取出此課程所有成員
                DataTable dtMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(dtAllClass.Rows[i]["iClassGroupID"].ToString(), "ClassGroup");
                //取出課程所包含的組別
                DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(dtAllClass.Rows[i]["iClassGroupID"].ToString());
                if (dtTempGroup.Rows.Count != 0)
                {
                    //檢查所有成員是否包含在某一組別
                    for (int j = 0; j < dtMember.Rows.Count; j++)
                    {
                        int iCheck = 0; //尚未在組別
                        for (int k = 0; k < dtTempGroup.Rows.Count; k++)
                        {
                            DataTable dtCheckMemberInGroup = clsGroupManagement.ORCS_GroupMember_SELECT_by_UserID_GroupID_Classify(dtMember.Rows[j]["cUserID"].ToString(), dtTempGroup.Rows[k]["iTempGroupID"].ToString(), "TempGroup");
                            if (dtCheckMemberInGroup.Rows.Count != 0)
                            {
                                iCheck = 1;
                            }
                        }
                        if (iCheck == 0)
                        {
                            DataRow row = dtNonGroupMember.NewRow();
                            //取出系所名稱
                            DataTable dtSchoolName = clsGroupManagement.ORCS_SchoolGroup_SELECT_by_SchoolGroupID(dtAllClass.Rows[i]["iSchoolGroupID"].ToString());
                            //取出姓名
                            DataTable dtUserName = clsGroupManagement.ORCS_User_SELECT_by_UserID(dtMember.Rows[j]["cUserID"].ToString());
                            row["SchoolName"] = dtSchoolName.Rows[0]["cSchoolGroupName"];
                            row["ClasslName"] = dtAllClass.Rows[i]["cClassGroupName"];
                            row["UserID"] = dtMember.Rows[j]["cUserID"];
                            row["UserName"] = dtUserName.Rows[0]["cUserName"];
                            row["ClassID"] = dtAllClass.Rows[i]["iClassGroupID"];
                            dtNonGroupMember.Rows.Add(row);
                        }
                    }
                }
            }
            if (dtNonGroupMember.Rows.Count != 0)
            {
                divNonGroup.Visible = true;
                btnAutoGroupForManual.Visible = true;
                gvNonGroup.DataSource = dtNonGroupMember;
                gvNonGroup.DataBind();
            }
            else
            {
                divNonGroup.Visible = false;
                btnAutoGroupForManual.Visible = false;
            }
        }
        //選擇不同TreeView的Node發生之事件
        protected void tvGroup_SelectedNodeChanged(object sender, EventArgs e)
        {
            //將所選擇到的Node存入hfNodeValue
            hfNodeValue.Value = tvGroup.SelectedNode.Value;
            //載入群組人員GridView
            LoadGroupGridView();
        }
        //載入群組人員GridView
        protected void LoadGroupGridView()
        {
            //定義所選擇的NodeID
            string strNodeID = hfNodeValue.Value.Split('_')[0];
            //定義所選擇的群組
            string strGroup = hfNodeValue.Value.Split('_')[1];
            //定義所選擇的Node的ParentGroupID
            string strParentGroupID = hfNodeValue.Value.Split('_')[2];
            //定義所選擇不同的群組所發生之事件
            DataTable dtMember = new DataTable();
            switch (strGroup)
            {
                case "SchoolGroup": // 選擇第一層Node
                    //直接取全部User
                    if (ddlAuthority.SelectedItem.Value == "t")
                        dtMember = clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority("%", "t");
                    else if (ddlAuthority.SelectedItem.Value == "s")
                        dtMember = clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority("%", "s");
                    else
                        dtMember = clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority("%", "a");
                    break;
                case "ClassGroup": // 選擇第二層Node
                    //取得ParentNode的User
                    DataTable dtClassGroupMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(strParentGroupID, "SchoolGroup");
                    //將取得的User放進dtMember
                    if (dtClassGroupMember.Rows.Count > 0)
                    {
                        foreach (DataRow drClassGroupMember in dtClassGroupMember.Rows)
                        {
                            if (ddlAuthority.SelectedItem.Value == "t")
                                dtMember.Merge(clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority(drClassGroupMember["cUserID"].ToString(), "t"));
                            else if (ddlAuthority.SelectedItem.Value == "s")
                                dtMember.Merge(clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority(drClassGroupMember["cUserID"].ToString(), "s"));
                            else
                                dtMember.Merge(clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority(drClassGroupMember["cUserID"].ToString(), "a"));
                        }
                    }
                    break;
                case "TempGroup": // 選擇第三層Node
                    //取得ParentNode的User
                    DataTable dtTempGroupMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(strParentGroupID, "ClassGroup");
                    //將取得的User放進dtMember
                    if (dtTempGroupMember.Rows.Count > 0)
                    {
                        foreach (DataRow drTempGroupMember in dtTempGroupMember.Rows)
                        {
                            if (ddlAuthority.SelectedItem.Value == "t")
                                dtMember.Merge(clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority(drTempGroupMember["cUserID"].ToString(), "t"));
                            else if (ddlAuthority.SelectedItem.Value == "s")
                                dtMember.Merge(clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority(drTempGroupMember["cUserID"].ToString(), "s"));
                            else
                                dtMember.Merge(clsGroupManagement.ORCS_User_SELECT_ID_Name_by_UserID_Authority(drTempGroupMember["cUserID"].ToString(), "a"));
                        }
                    }
                    //載入小組成員
                    Init_MemberList(strNodeID);
                    //載入主席
                    Init_ChairMan(strNodeID);
                    //載入紀錄者
                    Init_RecordMan(strNodeID);
                    break;
            }
            //將取得的User放進GridView裡
            gvGroupMember.DataSource = dtMember;
            //顯示GridView內容
            gvGroupMember.DataBind();
            //若GridView有內容則顯示該有的標題和勾選的人員
            if (gvGroupMember.Rows.Count > 0)
            {
                gvGroupMember.HeaderRow.Cells[1].Text = "帳號"; // GridView第1行標題
                gvGroupMember.HeaderRow.Cells[2].Text = "姓名"; // GridView第2行標題
                for (int i = 0; i < gvGroupMember.Rows.Count; i++)
                {
                    //檢查使用者是否存在人員群組裡，若有則勾選
                    switch (strGroup)
                    {
                        case "SchoolGroup": // 選擇第一層Node
                            if (CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "SchoolGroup") == true)
                                ((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked = true;
                            break;
                        case "ClassGroup": // 選擇第二層Node
                            if (CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "ClassGroup") == true)
                                ((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked = true;
                            break;
                        case "TempGroup": // 選擇第三層Node
                            if (CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "TempGroup") == true)
                                ((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked = true;
                            break;
                    }
                }
            }
        }
        //檢查使用者是否存在人員群組裡，若有則回傳true
        protected Boolean CheckGroupMember(string cUserID, string cGroupID, string cGroupClassify)
        {
            DataTable dtGroupMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_UserID_GroupID_Classify(cUserID, cGroupID, cGroupClassify);
            if (dtGroupMember.Rows.Count > 0)
                return true;
            else
                return false;
        }
        //選擇不同身分發生之事件
        protected void ddlAuthority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hfNodeValue.Value != "")
                LoadGroupGridView();
        }
        //選擇不同模式發生之事件
        protected void ddlEditMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEditMode.SelectedValue.ToString() == "EditMember")
            {
                divGroupMember.Visible = true;
                divEditIdentify.Visible = false;
                ddlAuthority.Visible = true;
                lbAuthority.Visible = true;
            }
            else
            {
                divGroupMember.Visible = false;
                divEditIdentify.Visible = true;
                ddlAuthority.Visible = false;
                lbAuthority.Visible = false;
            }
        }
        //「儲存」按鈕事件
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //若hfNodeValue.Value不為空表示有選到某一個Node才執行儲存的動作
            if (hfNodeValue.Value != "")
            {
                //定義所選擇的NodeID
                string strNodeID = hfNodeValue.Value.Split('_')[0];
                //定義所選擇的群組
                string strGroup = hfNodeValue.Value.Split('_')[1];
                //定義所選擇的Node的ParentGroupID
                string strParentGroupID = hfNodeValue.Value.Split('_')[2];
                //檢查勾選的群組人員
                if (gvGroupMember.Rows.Count > 0)
                {
                    //檢查每一位使用者勾選狀態
                    for (int i = 0; i < gvGroupMember.Rows.Count; i++)
                    {
                        switch (strGroup)
                        {
                            case "SchoolGroup": // 選擇第一層Node
                                //若沒被勾選但資料表有此人員則刪除;若被勾選但資料表沒有此人員則新增
                                if (((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked == false && CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "SchoolGroup") == true)
                                {
                                    //取出第二層人員
                                    DataTable dtClassGroup = clsGroupManagement.ORCS_ClassGroup_SELECT_by_ParentGroupID(strNodeID);
                                    if (dtClassGroup.Rows.Count > 0)
                                    {
                                        foreach (DataRow drClassGroup in dtClassGroup.Rows)
                                        {
                                            //取出第三層人員
                                            DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(drClassGroup["iClassGroupID"].ToString());
                                            if (dtTempGroup.Rows.Count > 0)
                                            {
                                                foreach (DataRow drTempGroup in dtTempGroup.Rows)
                                                {
                                                    //刪除第三層人員
                                                    clsGroupManagement.ORCS_GroupMember_DELETE_by_UserID_GroupID_Classify(gvGroupMember.Rows[i].Cells[1].Text, drTempGroup["iTempGroupID"].ToString(), "TempGroup");
                                                }
                                            }
                                            //刪除第二層人員
                                            clsGroupManagement.ORCS_GroupMember_DELETE_by_UserID_GroupID_Classify(gvGroupMember.Rows[i].Cells[1].Text, drClassGroup["iClassGroupID"].ToString(), "ClassGroup");
                                        }
                                    }
                                    //刪除第一層人員
                                    clsGroupManagement.ORCS_GroupMember_DELETE_by_UserID_GroupID_Classify(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "SchoolGroup");
                                }
                                else if (((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked == true && CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "SchoolGroup") == false)
                                    clsGroupManagement.ORCS_GroupMember_INSERT(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "SchoolGroup");
                                break;
                            case "ClassGroup": // 選擇第二層Node
                                //若沒被勾選但資料表有此人員則刪除;若被勾選但資料表沒有此人員則新增
                                if (((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked == false && CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "ClassGroup") == true)
                                {
                                    //取出第三層人員
                                    DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(strNodeID);
                                    if (dtTempGroup.Rows.Count > 0)
                                    {
                                        foreach (DataRow drTempGroup in dtTempGroup.Rows)
                                        {
                                            //刪除第三層人員
                                            clsGroupManagement.ORCS_GroupMember_DELETE_by_UserID_GroupID_Classify(gvGroupMember.Rows[i].Cells[1].Text, drTempGroup["iTempGroupID"].ToString(), "TempGroup");
                                        }
                                    }
                                    //刪除第二層人員
                                    clsGroupManagement.ORCS_GroupMember_DELETE_by_UserID_GroupID_Classify(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "ClassGroup");
                                }
                                else if (((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked == true && CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "ClassGroup") == false)
                                    clsGroupManagement.ORCS_GroupMember_INSERT(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "ClassGroup");
                                break;
                            case "TempGroup": // 選擇第三層Node
                                //若沒被勾選但資料表有此人員則刪除;若被勾選但資料表沒有此人員則新增
                                if (((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked == false && CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "TempGroup") == true)
                                    clsGroupManagement.ORCS_GroupMember_DELETE_by_UserID_GroupID_Classify(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "TempGroup");
                                else if (((CheckBox)gvGroupMember.Rows[i].FindControl("chkUserSelect")).Checked == true && CheckGroupMember(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "TempGroup") == false)
                                    clsGroupManagement.ORCS_GroupMember_INSERT(gvGroupMember.Rows[i].Cells[1].Text, strNodeID, "TempGroup");
                                break;
                        }
                    }
                }
            }
            LoadNonGroup();
        }
        //「全選」Check按鈕事件
        protected void chkSelectAll_OnCheckedChanged(object sender, EventArgs e)
        {
            //若全選為勾選狀態則將全部人員勾選，否則取消勾選
            if (((CheckBox)sender).Checked == true)
            {
                foreach (GridViewRow gvrGroupMember in gvGroupMember.Rows)
                    ((CheckBox)gvrGroupMember.FindControl("chkUserSelect")).Checked = true;
            }
            else
            {
                foreach (GridViewRow gvrGroupMember in gvGroupMember.Rows)
                    ((CheckBox)gvrGroupMember.FindControl("chkUserSelect")).Checked = false;
            }
        }
        //自動分組按鈕事件
        protected void btnAutoGroup_Click(object sender, EventArgs e)
        {
            if (hfNodeValue.Value != "")
            {
                //定義所選擇的NodeID
                ViewState["NodeID"] = hfNodeValue.Value.Split('_')[0];
                //定義所選擇的群組
                ViewState["Group"] = hfNodeValue.Value.Split('_')[1];
                //定義所選擇的Node的ParentGroupID
                ViewState["ParentGroupID"] = hfNodeValue.Value.Split('_')[2];
                //如果使用者點選課程才會顯示分組視窗
                if (ViewState["Group"].ToString() == "ClassGroup")
                {
                    //計算此課程成員人數
                    DataTable dtTempGroupMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify_Random(ViewState["NodeID"].ToString(), "ClassGroup");
                    lbGroupNum.Text = "此課程人數";
                    lbGroupNum.Text = lbGroupNum.Text + dtTempGroupMember.Rows.Count.ToString() + "人<br />請選擇分組的數目:";
                    divGroupNum.Visible = true;
                    //初始化下拉式選單
                    ddlGroupNum.Items.Clear();
                    for (int i = 1; i < dtTempGroupMember.Rows.Count / 2; i++)
                    {
                        if (dtTempGroupMember.Rows.Count % (i + 1) != 0)
                        {
                            string strDDLText = (i + 1).ToString() + "(一組人數" + (dtTempGroupMember.Rows.Count / (i + 1)).ToString() + "~" + ((dtTempGroupMember.Rows.Count / (i + 1)) + 1).ToString() + "人)";
                            ddlGroupNum.Items.Add(new ListItem(strDDLText, (i + 1).ToString()));
                        }
                        else
                        {
                            string strDDLText = (i + 1).ToString() + "(一組人數" + (dtTempGroupMember.Rows.Count / (i + 1)).ToString() + "人)";
                            ddlGroupNum.Items.Add(new ListItem(strDDLText, (i + 1).ToString()));
                        }
                    }
                }
            }
        }
        //分組數量視窗按下確認按鈕事件
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //取出此課程的所有小組
            DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(ViewState["NodeID"].ToString());
            //刪除ORCS_TempGroup資料表相關資訊
            clsGroupManagement.ORCS_TempGroup_DELETE_by_ParentGroupID(ViewState["NodeID"].ToString());
            //刪除ORCS_GroupMember資料表相關資訊和ORCS_MeetingIdentity資料表相關資訊
            for (int i = 0; i < dtTempGroup.Rows.Count; i++)
            {
                clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(dtTempGroup.Rows[i]["iTempGroupID"].ToString(), "TempGroup");
                clsGroupManagement.ORCS_MeetingIdentity_DELETE_by_GroupID(dtTempGroup.Rows[i]["iTempGroupID"].ToString());
            }
            //關閉分組視窗
            divGroupNum.Visible = false;
            //取出課程總人數
            DataTable dtTempGroupMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify_Random(ViewState["NodeID"].ToString(), "ClassGroup");
            //計算一個群組人數最多幾人
            int iPeopleNumber = ((dtTempGroupMember.Rows.Count) / Convert.ToInt32(ddlGroupNum.SelectedValue)) + 1;
            //有幾組會有最多的情況
            int iMaxNumCond = (dtTempGroupMember.Rows.Count) % Convert.ToInt32(ddlGroupNum.SelectedValue);
            //新建組別
            for (int iGroupNum = 0; iGroupNum < Convert.ToInt32(ddlGroupNum.SelectedValue); iGroupNum++)
            {
                //預設名稱為小組1，小組2...
                clsGroupManagement.ORCS_TempGroup_INSERT("小組" + (iGroupNum + 1).ToString(), ViewState["NodeID"].ToString());
            }
            //取得新建組別ID
            DataTable dtGroupID = clsGroupManagement.ORCS_TempGroup_SELECT_by_iClassGroupID(ViewState["NodeID"].ToString());
            //進行成員分組且選出每一組主席和紀錄者(每一組第一個選出來的為各組主席，每一組第二個選出來的為各組紀錄者)
            int iCheck = 1;
            int iTimes = 0;
            for (int i = 0; i < dtTempGroupMember.Rows.Count; i++)
            {
                if (i / iPeopleNumber < iMaxNumCond)
                {
                    //分組(先處理分組人數較多的狀況)
                    clsGroupManagement.ORCS_GroupMember_INSERT(dtTempGroupMember.Rows[i]["cUserID"].ToString(), (Convert.ToInt32(dtGroupID.Rows[i / iPeopleNumber]["iTempGroupID"].ToString())).ToString(), "TempGroup");
                    //選主席和紀錄者
                    if (i % iPeopleNumber == 0)
                    {
                        //選主席
                        clsGroupManagement.ORCS_MeetingIdentity_INSERT((Convert.ToInt32(dtGroupID.Rows[i / iPeopleNumber]["iTempGroupID"].ToString())).ToString(), dtTempGroupMember.Rows[i]["cUserID"].ToString(), "ChairMan");
                    }
                    else if (i % iPeopleNumber == 1)
                    {
                        //選紀錄者
                        clsGroupManagement.ORCS_MeetingIdentity_INSERT((Convert.ToInt32(dtGroupID.Rows[i / iPeopleNumber]["iTempGroupID"].ToString())).ToString(), dtTempGroupMember.Rows[i]["cUserID"].ToString(), "RecordMan");
                    }
                }
                else
                {
                    //分組(處理分組人數較少的情況)
                    clsGroupManagement.ORCS_GroupMember_INSERT(dtTempGroupMember.Rows[i]["cUserID"].ToString(), (Convert.ToInt32(dtGroupID.Rows[iMaxNumCond + iTimes]["iTempGroupID"].ToString())).ToString(), "TempGroup");
                    //選主席和紀錄者 
                    if (iCheck == 1)
                    {
                        //選主席
                        clsGroupManagement.ORCS_MeetingIdentity_INSERT((Convert.ToInt32(dtGroupID.Rows[iMaxNumCond + iTimes]["iTempGroupID"].ToString())).ToString(), dtTempGroupMember.Rows[i]["cUserID"].ToString(), "ChairMan");
                    }
                    else if (iCheck == 2)
                    {
                        //選主席
                        clsGroupManagement.ORCS_MeetingIdentity_INSERT((Convert.ToInt32(dtGroupID.Rows[iMaxNumCond + iTimes]["iTempGroupID"].ToString())).ToString(), dtTempGroupMember.Rows[i]["cUserID"].ToString(), "RecordMan");
                    }
                    iCheck++;
                    if (iCheck == iPeopleNumber)
                    {
                        iCheck = 1;
                        iTimes++;
                    }
                }
            }
            //載入群組TreeView
            LoadGroupTree();
        }
        //分組數量視窗按下取消按鈕事件
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //關閉分組視窗
            divGroupNum.Visible = false;
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
        //點選匯入成員按鈕事件
        protected void btnExceltoDB_Click(object sender, EventArgs e)
        {
            if (hfNodeValue.Value != "")
            {
                //定義所選擇的NodeID
                ViewState["NodeID"] = hfNodeValue.Value.Split('_')[0];
                //定義所選擇的群組
                ViewState["Group"] = hfNodeValue.Value.Split('_')[1];
                //定義所選擇的Node的ParentGroupID
                ViewState["ParentGroupID"] = hfNodeValue.Value.Split('_')[2];
                if (ViewState["Group"].ToString() == "SchoolGroup" || ViewState["Group"].ToString() == "ClassGroup")
                {
                    divExceltoDB.Visible = true;
                    btnAddMemberByExcel.Visible = false;
                    gvExcelMemberData.Visible = false;
                    lbRemind.Visible = false;
                }
            }
        }
        //上傳Excel檔案
        protected void btnExcelSave_Click(object sender, EventArgs e)
        {
            if (FileUploadExcel.FileName.ToString() == "")
            {
                //未上傳檔案
            }
            else
            {
                string PathName, FileName;
                PathName = FileUploadExcel.FileName.ToString();
                string[] NewPath = PathName.Split('\\');
                FileName = NewPath[NewPath.Length - 1];

                //檔案存放位置
                string SeverPath = "D:\\ORCS\\ExcelMemberFile";

                //判斷檔案是否已在SERVER上
                if (!(FileUploadExcel.PostedFile == null))
                {
                    if (File.Exists(SeverPath + "\\" + FileName))
                    {
                        //lState.Text = "檔案已存在";
                        string[] NewFileName = FileName.Split('.');
                        FileName = NewFileName[0] + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    }
                    FileUploadExcel.PostedFile.SaveAs(SeverPath + "\\" + FileName);
                    //上傳成功
                    //建立Data Table
                    DataTable dtLoadExcel = LoadExcelFile(SeverPath + "\\" + FileName, "Sheet1");
                    Session["UserDataTable"] = dtLoadExcel;
                    //Show在Gridview中供使用者觀看
                    gvExcelMemberData.DataSource = dtLoadExcel;
                    gvExcelMemberData.DataBind();
                }
                if (FileUploadExcel.PostedFile.ContentLength == 0)
                {
                    //上傳失敗
                }
            }
            btnAddMemberByExcel.Visible = true;
            gvExcelMemberData.Visible = true;
            lbRemind.Visible = true;
        }
        //在匯入成員div中的取消按鈕事件
        protected void btnExcelCancel_Click(object sender, EventArgs e)
        {
            divExceltoDB.Visible = false;
        }
        //載入Excel檔案中成員資料
        static public DataTable LoadExcelFile(string strFilePath, string strSheetName)
        {
            System.Data.OleDb.OleDbConnection cn = new System.Data.OleDb.OleDbConnection();
            cn.ConnectionString = "Provider=MicroSoft.Jet.OLEDB.4.0;Data Source = " + strFilePath + "; Extended Properties ='Excel 8.0;'";
            cn.Open();

            System.Data.OleDb.OleDbDataAdapter da;
            da = new System.Data.OleDb.OleDbDataAdapter("Select * from [" + strSheetName + "$]", cn);

            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            finally
            {
                da.Dispose();
            }
            return dt;
        }
        //將成員資料儲存至資料庫中
        protected void ExcelToDB_INSERT()
        {
            //取出Excel成員名單
            DataTable dtLoadExcel = (DataTable)Session["UserDataTable"];
            switch (ViewState["Group"].ToString())
            {
                case "SchoolGroup": // 選擇第一層Node
                    //檢查Excel中新增的課程是否與目前的課程相同，如果沒有則新增此課程，如果有則將原本課程成員清除
                    for (int i = 0; i < dtLoadExcel.Rows.Count; i++)
                    {
                        //取出此系所所有的課程
                        DataTable dtSchoolGroup = clsGroupManagement.ORCS_ClassGroup_SELECT_by_ParentGroupID(ViewState["NodeID"].ToString());
                        int iCheckSameClass = 0; //判別是否有新課程
                        for (int j = 0; j < dtSchoolGroup.Rows.Count; j++)
                        {
                            if (dtLoadExcel.Rows[i]["ClassName"].ToString() == dtSchoolGroup.Rows[j]["cClassGroupName"].ToString())
                            {
                                //非新課程
                                //清除舊成員資訊
                                clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(dtSchoolGroup.Rows[j]["iClassGroupID"].ToString(), "ClassGroup");
                                //刪除此班級下的所有小組成員和小組
                                //刪除小組成員
                                clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(dtSchoolGroup.Rows[j]["iClassGroupID"].ToString(), "TempGroup");
                                //刪除小組
                                clsGroupManagement.ORCS_TempGroup_DELETE_by_ParentGroupID(dtSchoolGroup.Rows[j]["iClassGroupID"].ToString());
                                iCheckSameClass = 1;
                            }
                            else
                            {
                            }
                        }
                        if (iCheckSameClass == 0)
                        {
                            clsGroupManagement.ORCS_ClassGroup_INSERT(dtLoadExcel.Rows[i]["ClassName"].ToString(), ViewState["NodeID"].ToString());
                        }
                    }
                    //取出系所成員
                    DataTable dtSchool = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(ViewState["NodeID"].ToString(), "SchoolGroup");
                    //新增成員
                    for (int i = 0; i < dtLoadExcel.Rows.Count; i++)
                    {
                        int iCheck = 0; //是否包含在系所中
                        for (int j = 0; j < dtSchool.Rows.Count; j++)
                        {
                            if (dtLoadExcel.Rows[i]["ID"].ToString() == dtSchool.Rows[j]["cUserID"].ToString())
                            {
                                iCheck = 1;
                            }
                        }
                        if (iCheck == 0)
                        {
                            //未在系所中，將成員寫入資料庫
                            clsGroupManagement.ORCS_GroupMember_INSERT(dtLoadExcel.Rows[i]["ID"].ToString(), ViewState["NodeID"].ToString(), "SchoolGroup");
                        }
                        DataTable dtClassGroupID = clsGroupManagement.ORCS_ClassGroup_SELECT_by_ParentGroupID_ClassName(ViewState["NodeID"].ToString(), dtLoadExcel.Rows[i]["ClassName"].ToString());
                        clsGroupManagement.ORCS_GroupMember_INSERT(dtLoadExcel.Rows[i]["ID"].ToString(), dtClassGroupID.Rows[0]["iClassGroupID"].ToString(), "ClassGroup");

                    }
                    LoadGroupTree();
                    break;
                case "ClassGroup": // 選擇第二層Node
                    //刪除原有的班級成員
                    clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(ViewState["NodeID"].ToString(), "ClassGroup");
                    //刪除此班級下的所有小組成員和小組
                    //首先，取出此班級所有小組
                    DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(ViewState["NodeID"].ToString());
                    for (int i = 0; i < dtTempGroup.Rows.Count; i++)
                    {
                        //刪除小組成員
                        clsGroupManagement.ORCS_GroupMember_DELETE_by_GroupID_Classify(dtTempGroup.Rows[i]["iTempGroupID"].ToString(), "TempGroup");
                        //刪除小組
                        clsGroupManagement.ORCS_TempGroup_DELETE_by_ParentGroupID(ViewState["NodeID"].ToString());
                    }
                    //取出系所的成員
                    DataTable dtSchoolMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(ViewState["ParentGroupID"].ToString(), "SchoolGroup");
                    //檢查此系所是否包含Excel中的成員，沒有的話，此成員要加入至系所中
                    for (int i = 0; i < dtLoadExcel.Rows.Count; i++)
                    {
                        int iCheck = 0; //是否包含在系所中
                        for (int j = 0; j < dtSchoolMember.Rows.Count; j++)
                        {
                            if (dtLoadExcel.Rows[i]["ID"].ToString() == dtSchoolMember.Rows[j]["cUserID"].ToString())
                            {
                                iCheck = 1;
                            }
                        }
                        if (iCheck == 0)
                        {
                            //未在系所中，將成員寫入資料庫
                            clsGroupManagement.ORCS_GroupMember_INSERT(dtLoadExcel.Rows[i]["ID"].ToString(), ViewState["ParentGroupID"].ToString(), "SchoolGroup");
                        }
                        clsGroupManagement.ORCS_GroupMember_INSERT(dtLoadExcel.Rows[i]["ID"].ToString(), ViewState["NodeID"].ToString(), "ClassGroup");
                    }
                    LoadGroupTree();
                    break;
                case "TempGroup": // 選擇第三層Node

                    break;
            }
        }
        //按下送出事件按鈕(匯入Excel)
        protected void btnAddMemberByExcel_Click(object sender, EventArgs e)
        {
            ExcelToDB_INSERT();
            divExceltoDB.Visible = false;
            LoadGroupGridView();
        }
        //當手動分組時，有尚未分組的成員，則可以利用自動分組快速分配
        protected void btnAutoGroupForManual_Click(object sender, EventArgs e)
        {
            //寫成FUNCTION
            ///////////////////////////////////////檢查是否有成員尚未加入名單中///////////////////////////////////////
            //檢查課程所有成員是否有分到組
            //建立尚未分組名單
            DataTable dtNonGroupMember = new DataTable();
            dtNonGroupMember.Columns.Add("SchoolName", typeof(String));
            dtNonGroupMember.Columns.Add("ClasslName", typeof(String));
            dtNonGroupMember.Columns.Add("UserID", typeof(String));
            dtNonGroupMember.Columns.Add("UserName", typeof(String));
            dtNonGroupMember.Columns.Add("ClassID", typeof(String));
            //取出課程
            DataTable dtAllClass = clsGroupManagement.ORCS_ClassGroup_SELECT_All();
            for (int i = 0; i < dtAllClass.Rows.Count; i++)
            {
                //取出此課程所有成員
                DataTable dtMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(dtAllClass.Rows[i]["iClassGroupID"].ToString(), "ClassGroup");
                //取出課程所包含的組別
                DataTable dtTempGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(dtAllClass.Rows[i]["iClassGroupID"].ToString());
                if (dtTempGroup.Rows.Count != 0)
                {
                    //檢查所有成員是否包含在某一組別
                    for (int j = 0; j < dtMember.Rows.Count; j++)
                    {
                        int iCheck = 0; //尚未在組別
                        for (int k = 0; k < dtTempGroup.Rows.Count; k++)
                        {
                            DataTable dtCheckMemberInGroup = clsGroupManagement.ORCS_GroupMember_SELECT_by_UserID_GroupID_Classify(dtMember.Rows[j]["cUserID"].ToString(), dtTempGroup.Rows[k]["iTempGroupID"].ToString(), "TempGroup");
                            if (dtCheckMemberInGroup.Rows.Count != 0)
                            {
                                iCheck = 1;
                            }
                        }
                        if (iCheck == 0)
                        {
                            DataRow row = dtNonGroupMember.NewRow();
                            //取出系所名稱
                            DataTable dtSchoolName = clsGroupManagement.ORCS_SchoolGroup_SELECT_by_SchoolGroupID(dtAllClass.Rows[i]["iSchoolGroupID"].ToString());
                            //取出姓名
                            DataTable dtUserName = clsGroupManagement.ORCS_User_SELECT_by_UserID(dtMember.Rows[j]["cUserID"].ToString());
                            row["SchoolName"] = dtSchoolName.Rows[0]["cSchoolGroupName"];
                            row["ClasslName"] = dtAllClass.Rows[i]["cClassGroupName"];
                            row["UserID"] = dtMember.Rows[j]["cUserID"];
                            row["UserName"] = dtUserName.Rows[0]["cUserName"];
                            row["ClassID"] = dtAllClass.Rows[i]["iClassGroupID"];
                            dtNonGroupMember.Rows.Add(row);
                        }
                    }
                }
            }
            for (int i = 0; i < dtNonGroupMember.Rows.Count; i++)
            {
                //取出課程所包含的組別
                DataTable dtTempGroupForNonGroup = clsGroupManagement.ORCS_TempGroup_SELECT_by_ParentGroupID(dtNonGroupMember.Rows[i]["ClassID"].ToString());
                ArrayList arrMemberCount = new ArrayList();
                //計算每一個組別的人數
                for (int j = 0; j < dtTempGroupForNonGroup.Rows.Count; j++)
                {
                    DataTable dtMember = clsGroupManagement.ORCS_GroupMember_SELECT_by_GroupID_Classify(dtTempGroupForNonGroup.Rows[j]["iTempGroupID"].ToString(), "TempGroup");
                    arrMemberCount.Add(dtMember.Rows.Count);
                }

                int min_pos = 0;
                for (int k = 0; k < dtTempGroupForNonGroup.Rows.Count; k++)
                {
                    if (Convert.ToInt32(arrMemberCount[k]) < Convert.ToInt32(arrMemberCount[min_pos]))
                    {
                        min_pos = k;
                    }
                }
                //人數最少的組別
                string strMinNumGroupID = dtTempGroupForNonGroup.Rows[min_pos]["iTempGroupID"].ToString();
                //將成員分到此組
                clsGroupManagement.ORCS_GroupMember_INSERT(dtNonGroupMember.Rows[i]["UserID"].ToString(), strMinNumGroupID, "TempGroup");
            }
            //載入尚未分組名單
            LoadNonGroup();
        }
    }
}