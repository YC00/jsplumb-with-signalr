using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using Microsoft.AspNet.SignalR.Hubs;
using OpenNLP.Tools.SentenceDetect;

namespace Microsoft.AspNet.SignalR.Samples.Hubs.ShapeShare
{
    public class ShapeShare : Hub
    {
        private static ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        private static Random _userNameGenerator = new Random();

        //public IEnumerable<Shape> GetShapes()
        //{
        //    //return _shapes.Values;
        //}



        public void Join(string userName)
        {
            User user = null;
            if (string.IsNullOrWhiteSpace(userName))
            {
                user = new User();
                do
                {
                    user.Name = "User" + _userNameGenerator.Next(1000);
                } while (!_users.TryAdd(user.Name, user));
            }
            else if (!_users.TryGetValue(userName, out user))
            {
                user = new User { Name = userName };
                _users.TryAdd(userName, user);
            }
            Clients.Caller.user = user;
        }

        private String strConnString = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;

        public void SetUserlist(string userName, string connID, string groupID)
        {
            List<string> userlist = new List<string>();
            List<string> connIDList = new List<string>();
            SqlConnection strcon = new SqlConnection(strConnString);
            strcon.Open();
            string InsertStr = "Delete from ConceptMap_tmpuserlist where username='"+userName+"' and groupid='"+groupID+"';Insert Into ConceptMap_tmpuserlist(username, connectionid, groupid) values('"+userName+"','"+connID+"','"+groupID+"');";
            SqlCommand InsertCmd = new SqlCommand(InsertStr, strcon);
            InsertCmd.ExecuteNonQuery();

            //string ADDStr = "select id, title, description, url, image, video from ConceptMap_Keyword where id='" + Request.QueryString["id"] + "'";
            string QueryStr = "select username, connectionid, groupid from ConceptMap_tmpuserlist where groupid='"+groupID+"' order by username";
            SqlCommand Cmd = new SqlCommand(QueryStr, strcon);
            SqlDataReader QueryReader = null;

            QueryReader = Cmd.ExecuteReader();
            while (QueryReader.Read())
            {
                userlist.Add(QueryReader["username"].ToString());
                connIDList.Add(QueryReader["connectionid"].ToString());
            }
            

            //tmpuserlist.Add(new User(userName, connID));
            //foreach (var currentUserlist in tmpuserlist)
            //{
            //    userlist.Add(currentUserlist.Name);
            //    connIDList.Add(currentUserlist.ConnectionId);
            //}
            Clients.Group(groupID).userlistSet(string.Join(",", userlist.ToArray()), string.Join(",", connIDList.ToArray()));
        }
        public void ChangeUserName(string currentUserName, string newUserName)
        {
            User user;
            if (!string.IsNullOrEmpty(newUserName) && _users.TryGetValue(currentUserName, out user))
            {
                user.Name = newUserName;
                User oldUser;
                _users.TryRemove(currentUserName, out oldUser);
                _users.TryAdd(newUserName, user);
                Clients.All.userNameChanged(currentUserName, newUserName);
                Clients.Caller.user = user;
            }
        }

        //public Task CreateShape(string type = "rectangle", string groupName = "")
        //{
        //    string name = Clients.Caller.user["Name"];
        //    var user = _users[name];
        //    var shape = Shape.Create(type);
        //    shape.ChangedBy = user;
        //    _shapes.TryAdd(shape.ID, shape);

        //    if (groupName != "")
        //        return Clients.Group(groupName).shapeAdded(shape);
        //    else
        //        return Clients.All.shapeAdded(shape);
        //}

        public Task MuteEdit(string connid)
        {
            return Clients.Client(connid).editMuted(connid);
        }
        public Task UnlockMuteEdit(string connid)
        {
            return Clients.Client(connid).editMutedUnlock(connid);
        }
        public Task ShowUserStat(string id, string groupID, string msg, string effect)
        {
            if (id != "")
            {
                return Clients.Group(groupID).userStatShowed(id, msg, effect);
            }
            else
            {
                return Clients.Group(groupID).userStatShowed(Context.ConnectionId, msg, effect);
            }
        }

        public Task CreateSelectedText(string nodeID, string label, string groupID)
        {
            return Clients.Group(groupID).selectedTextCreated(nodeID, label);
        }
        public Task NewShape(string newid)
        {
            return Clients.All.shapeCreated(newid);
        }
        public Task NewShape(string newid, string color)
        {
            return Clients.All.shapeCreated(newid, color);
        }
        public Task NewShape(string newid, string color, string templateid)
        {
            return Clients.All.shapeCreated(newid, color, templateid);
        }

        public Task NewShape(string newid, string color, string templateid, string x, string y)
        {
            return Clients.All.shapeCreated(newid, color, templateid, x, y);
        }
        public Task NewShape(string newid, string color, string templateid, string x, string y, string groupID = "")
        {
            if(groupID!="")
                return Clients.Group(groupID).shapeCreated(newid, color, templateid, x, y);
            else
                return Clients.All.shapeCreated(newid, color, templateid, x, y);
        }
        public Task RefreshNodeTemplate(string newid, string color)
        {
            return Clients.All.shapeCreated(newid, color);
        }

        public void SettingTemplateData(string templateid, string templatedata)
        {
            Clients.All.setTemplateData(templateid, templatedata);
        }

        public void SingleSettingTemplateData(string templateid, string templatedata)
        {
            Clients.Caller.setTemplateData(templateid, templatedata);
        }
        public void SettingTemplateData(string templateid, string templatedata, string groupID)
        {
            Clients.Group(groupID).setTemplateData(templateid, templatedata);
        }
        public void NewKeywordShape(string seq, string newid1, string newid2, string newid3, string label, string top, string left)
        {
            string strKeyWord = "SELECT * FROM MeetingSpeechKeywords WHERE Seq = '" + seq + "'";
            SqlCommand nodescmd = new SqlCommand(strKeyWord);
            DataSet nodesds = GetChangHanData(nodescmd);
            DataTable nodesdt = nodesds.Tables[0];
            
            //DataTable dtKeyWord = new DataTable();
            //dtKeyWord = sqlDB.getDataSet(strKeyWord).Tables[0];

            //3­ keywords
            //nodesdt.Rows[0]["cKeySpeakPart1"].ToString()
            //dtKeyWord.Rows[0]["cKeySpeakPart2"].ToString()
            //dtKeyWord.Rows[0]["cKeySpeakPart3"].ToString()
            int i = 1;
            if (nodesdt.Rows[0]["cKeySpeakPart1"].ToString()!="")
            Clients.All.keywordShapeCreated(newid1, i++.ToString() + ". " + nodesdt.Rows[0]["cKeySpeakPart1"].ToString(), top, left);
            if(nodesdt.Rows[0]["cKeySpeakPart2"].ToString()!="")
            Clients.All.keywordShapeCreated(newid2, i++.ToString() + ". " + nodesdt.Rows[0]["cKeySpeakPart2"].ToString(), (Convert.ToInt32(top) + 10), (Convert.ToInt32(left) + 10));
            if (nodesdt.Rows[0]["cKeySpeakPart3"].ToString() != "")
            Clients.All.keywordShapeCreated(newid3, i.ToString() + ". " + nodesdt.Rows[0]["cKeySpeakPart3"].ToString(), (Convert.ToInt32(top) + 20), (Convert.ToInt32(left) + 20));
        }
        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
        public Task linkNode()
        {
            return Clients.All.nodeLinked();
        }
        //public void ChangeShape(string id, int x, int y, int w, int h)
        //{
        //    if (w <= 0 || h <= 0) return;

        //    var shape = FindShape(id);
        //    if (shape == null)
        //    {
        //        return;
        //    }

        //    string name = Clients.Caller.user["Name"];
        //    var user = _users[name];

        //    shape.Width = w;
        //    shape.Height = h;
        //    shape.Location.X = x;
        //    shape.Location.Y = y;
        //    shape.ChangedBy = user;

        //    Task task = Clients.Others.shapeChanged(id,shape);
           
        //    task.Wait();
        //    if (task.Exception != null)
        //    {
        //        throw task.Exception;
        //    }
        //}

        public void MoveShape(string id, int x, int y, int w, int h)
        {
            if (w <= 0 || h <= 0) return;

            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //string name = Clients.Caller.user["Name"];
            //var user = _users[name];

            //shape.Width = w;
            //shape.Height = h;
            //shape.Location.X = x;
            //shape.Location.Y = y;
            //shape.ChangedBy = user;

            
            //task.Wait();
            //if (task.Exception != null)
            //{
            //    throw task.Exception;
            //}
        }
        public void MoveShape(string id, int x, int y, int w, int h, string groupID="")
        {
            if (w <= 0 || h <= 0) return;

            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //string name = Clients.Caller.user["Name"];
            //var user = _users[name];

            //shape.Width = w;
            //shape.Height = h;
            //shape.Location.X = x;
            //shape.Location.Y = y;
            //shape.ChangedBy = user;
            if (groupID != "")
            {
                //Clients.Group(groupID).userStatShowed(Context.ConnectionId, " 編輯中<img src=\"images/action.gif\" />", "fade");
                Clients.OthersInGroup(groupID).shapeMoved(id, x, y, w, h);
                //Clients.OthersInGroup(groupID).userStatShowed(Context.ConnectionId,"移動中","");
            }
            else
            {
                Clients.Others.shapeMoved(id, x, y, w, h);
            }
            //task.Wait();
            //if (task.Exception != null)
            //{
            //    throw task.Exception;
            //}
        }
        public void ResizableShape(string id, int w, int h)
        {
            if (w <= 0 || h <= 0) return;

            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //string name = Clients.Caller.user["Name"];
            //var user = _users[name];

            //shape.Width = w;
            //shape.Height = h;
            //shape.Location.X = x;
            //shape.Location.Y = y;
            //shape.ChangedBy = user;

            Clients.Others.shapeResized(id, (w-32), (h-32));

            //task.Wait();
            //if (task.Exception != null)
            //{
            //    throw task.Exception;
            //}
        }
        public void DeleteShape(string id, string groupID="")
        {
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);
            if(groupID!="")
                Clients.Group(groupID).shapeDeleted(id);
            else
                Clients.All.shapeDeleted(id);
        }
        public void deleteConnection(string id, string groupID="")
        {
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);
            if(groupID!="")
                Clients.Group(groupID).connectionDeleted(id);
            else
                Clients.All.connectionDeleted(id);
        }
        public void EditEP(string id,string value, string groupID="")
        {
            if(groupID!="")
                Clients.Group(groupID).epEdit(id, value);
            else
                Clients.All.epEdit(id, value);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void EditNode(string id, string value, string groupID="")
        {
            if(groupID!="")
                Clients.Group(groupID).nodeEdit(id, value);
            else
                Clients.All.nodeEdit(id, value);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }

        public void ConnectNode(string sourceid, string targetid, string clientid)
        {
            Clients.Others.nodeConnected(sourceid, targetid, clientid);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void ConnectNode(string sourceid, string targetid, string clientid, string color, string linklabel)
        {
            Clients.All.nodeConnected(sourceid, targetid, clientid, color, linklabel);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void ConnectNode(string sourceid, string targetid, string clientid, string color, string linklabel,string groupID="")
        {
            if(groupID!="")
                Clients.OthersInGroup(groupID).nodeConnected(sourceid, targetid, clientid, color);
            else
                Clients.Others.nodeConnected(sourceid, targetid, clientid, color);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void ConnectNode(string sourceid, string targetid, string clientid, string color, string linklabel, string linkid, string datetime, string groupID = "")
        {
            if (groupID != "")
                Clients.OthersInGroup(groupID).nodeConnected(sourceid, targetid, clientid, color, linkid, datetime);
            else
                Clients.Others.nodeConnected(sourceid, targetid, clientid, color, linkid, datetime);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void SetLinkTemplateIDcolor(string templateid, string color)
        {
            Clients.All.linkTemplateIDcolorSet(templateid, color);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void SetLinkTemplateIDcolor(string templateid, string color, string groupID)
        {
            Clients.All.linkTemplateIDcolorSet(templateid, color);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void changeConnID(List<String> connid, string clientid)
        {
            //Clients.Client(clientid).connIDChanged(connid);
            Clients.Others.connIDChanged(connid);
            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }

        public void sendChatMessage(string message, string userID, string groupID)
        {
            if (userID!="")
                Clients.Group(groupID).appendMessage("" + userID.ToString() + ": " + message + "");
            else
                Clients.Group(groupID).appendMessage("" + Context.ConnectionId.ToString() + ": " + message + "");
        }
        public void changeConnID(List<String> connid, string clientid, string groupID)
        {
            //Clients.Client(clientid).connIDChanged(connid);
            if(groupID!="")
                Clients.OthersInGroup(groupID).connIDChanged(connid);
            else
                Clients.Others.connIDChanged(connid);

            //var shape = FindShape(id);
            //if (shape == null)
            //{
            //    return;
            //}

            //Shape ignored;
            //_shapes.TryRemove(id, out ignored);

            //Clients.All.shapeDeleted(id);
        }
        public void DeleteAllShapes()
        {
            //var shapes = _shapes.Select(s => new { id = s.Value.ID }).ToList();

            //_shapes.Clear();

            //Clients.All.shapesDeleted(shapes);
        }

        //private Shape FindShape(string id)
        //{
        //    //return _shapes[id];
        //}
        public Task LoadShape(string nodeID, string nodeLabel, string xCoordinate, string yCoordinate, string width, string height)
        {
            return Clients.All.shapeLoaded(nodeID, nodeLabel, xCoordinate, yCoordinate, width, height);
        }
        public Task LoadShape(string nodeID, string nodeLabel, string color, string xCoordinate, string yCoordinate, string width, string height)
        {
            return Clients.All.shapeLoaded(nodeID, nodeLabel, color, xCoordinate, yCoordinate, width, height);
        }
        public Task LoadShape(string nodeID, string nodeLabel, string templateId, string color, string xCoordinate, string yCoordinate, string width, string height)
        {
            return Clients.All.shapeLoaded(nodeID, nodeLabel, templateId, color, xCoordinate, yCoordinate, width, height);
        }
        public Task SingleLoadShape(string nodeID, string nodeLabel, string templateId, string color, string xCoordinate, string yCoordinate, string width, string height, string panel, string datetime)
        {
            return Clients.Caller.browsingShapeLoaded(nodeID, nodeLabel, templateId, color, xCoordinate, yCoordinate, width, height, panel, datetime);
        }
        public Task SSetChartValue(int match, int pmatch, int additional)
        {
            return Clients.Caller.setSChartVal(match, pmatch, additional);
        }
        public Task TSetChartValue(int mmatch)
        {
            return Clients.Caller.setTChartVal(mmatch);
        }
        public Task SingleLoadChart()
        {
            return Clients.Caller.browsingChartLoaded();
        }
        public Task SingleLoadChart(int match, int pmatch, int mmatch, int additional)
        {
            return Clients.Caller.browsingChartLoaded(match, pmatch, mmatch, additional);
        }
        public Task LoadShape(string nodeID, string nodeLabel, string templateId , string color, string xCoordinate, string yCoordinate, string width, string height, string groupID)
        {
            return Clients.Group(groupID).shapeLoaded(nodeID, nodeLabel, templateId, color, xCoordinate, yCoordinate, width, height);
        }
        public Task LoadLink(string linkID, string linkLabel, string linkColor, string startNode, string endNode)
        {
            return Clients.All.linkLoaded(linkID, linkLabel, linkColor, startNode, endNode);
        }
        public Task LoadLink(string linkID, string linkLabel, string linkColor, string templateID, string startNode, string endNode)
        {
            return Clients.All.linkLoaded(linkID, linkLabel, linkColor, templateID, startNode, endNode);
        }
        public Task SingleLoadLink(string linkID, string linkLabel, string linkColor, string templateID, string startNode, string endNode, string panel, string datetime)
        {
            return Clients.Caller.browsingLinkLoaded(linkID, linkLabel, linkColor, templateID, startNode, endNode, panel, datetime);
        }
        public Task LoadLink(string linkID, string linkLabel, string linkColor, string templateID, string startNode, string endNode, string groupID)
        {
            return Clients.Group(groupID).linkLoaded(linkID, linkLabel, linkColor, templateID, startNode, endNode);
        }
        public Task LoadField(string fieldName, string fieldValue)
        {
            return Clients.All.fieldLoaded(fieldName, fieldValue);
        }

        public Task ListComparePath(string id, string rootleaf, string Value)
        {
            return Clients.Caller.comparePathListed(id, rootleaf, Value);
        }
        public void EmptyPanel()
        {
            Clients.All.panelEmpty();
        }
        public void EmptyPanel(string groupID)
        {
            Clients.Group(groupID).panelEmpty();
        }
        public Task SingleLoadStartNodeDropdown(string StartNode, string NodeLabel)
        {
            return Clients.Caller.startNodeDropdownLoaded(StartNode, NodeLabel);
        }
        public Task SingleLoadEndNodeDropdown(string EndNode, string NodeLabel)
        {
            return Clients.Caller.endNodeDropdownLoaded(EndNode, NodeLabel);
        }
        public Task SetResource(string val, string id, string groupid)
        {
            return Clients.Group(groupid).resourceSet(val, id);
        }
        public Task StartLoadingImage(string groupid)
        {
            return Clients.Group(groupid).loadingImageStarted();
        }
        public Task StartLoadingImage()
        {
            return Clients.Caller.loadingImageStarted();
        }
        public Task StopLoadingImage(string groupid)
        {
            return Clients.Group(groupid).loadingImageStopped();
        }
        public Task StopLoadingImage()
        {
            return Clients.Caller.loadingImageStopped();
        }
        public Task loadmatchtable(string msg)
        {
            return Clients.Caller.matchtableload(msg);
        }
        public Task loadpmatchtable(string msg)
        {
            return Clients.Caller.pmatchtableload(msg);
        }
        public Task loadmmatchtable(string msg)
        {
            return Clients.Caller.mmatchtableload(msg);
        }
        public Task loadaddtable(string msg)
        {
            return Clients.Caller.addtableload(msg);
        }
        public void loadStartNodeDropdown(string CID)
        {
            //Clients.All.mask();
            //load nodes

            string spathquery = "SELECT DISTINCT ConceptMap_Path.StartNode, ConceptMap_Node.NodeLabel FROM ConceptMap_Path INNER JOIN ConceptMap_Node ON ConceptMap_Path.StartNode = ConceptMap_Node.NodeID WHERE (ConceptMap_Path.CID = '" + CID + "')";
            SqlCommand spathcmd = new SqlCommand(spathquery);
            DataSet spathds = GetData(spathcmd);

            foreach (DataRow spathdata in spathds.Tables[0].Rows)
            {
                SingleLoadStartNodeDropdown(spathdata["StartNode"].ToString(), spathdata["NodeLabel"].ToString());
            }
            //Clients.All.unmask();
        }
        public void loadEndNodeDropdown(string CID)
        {
            //Clients.All.mask();
            //load nodes

            //string epathquery = "SELECT DISTINCT ConceptMap_Path.EndNode, ConceptMap_Node.NodeLabel FROM ConceptMap_Path INNER JOIN ConceptMap_Node ON ConceptMap_Path.EndNode = ConceptMap_Node.NodeID WHERE (ConceptMap_Path.CID = '" + CID + "')";
            string epathquery = "SELECT DISTINCT ConceptMap_Node.NodeID, ConceptMap_Node.NodeLabel FROM ConceptMap_Node WHERE (CID = '" + CID + "') AND (NodeID IN (SELECT EndNode FROM ConceptMap_Path WHERE (CID = '" + CID + "')))";
            SqlCommand epathcmd = new SqlCommand(epathquery);
            DataSet epathds = GetData(epathcmd);

            foreach (DataRow epathdata in epathds.Tables[0].Rows)
            {
                SingleLoadEndNodeDropdown(epathdata["NodeID"].ToString(), epathdata["NodeLabel"].ToString());
            }
            //Clients.All.unmask();
        }
        private DataTable GetDataTable(SqlCommand cmd)
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
        private static void CreateCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void generateNodePath(string CID, string role)
        {
            string checkPathQuery = "SELECT DISTINCT ConceptMap_Path.StartNode, ConceptMap_Node.NodeLabel FROM ConceptMap_Path INNER JOIN ConceptMap_Node ON ConceptMap_Path.StartNode = ConceptMap_Node.NodeID WHERE (ConceptMap_Path.CID = '" + CID + "')";
            SqlCommand checkpathcmd = new SqlCommand(checkPathQuery);
            DataTable checkpathdt = GetDataTable(checkpathcmd);

            if (checkpathdt.Rows.Count <= 0)
            {
                Graph myGraph = new Graph();
 
                List<string> node = new List<string>();

                string nodeQuery = "SELECT NodeID, CID, UserID, NodeLabel, Xcoordinate, Ycoordinate, Width, Height, ParentNode, [Level], Color, CreatedDateTime FROM ConceptMap_Node WHERE (CID = '" + CID + "')";

                SqlCommand nodecmd = new SqlCommand(nodeQuery);

                DataTable nodedt = GetDataTable(nodecmd);

                if (nodedt.Rows.Count > 0)
                {
                    foreach (DataRow row in nodedt.Rows) // Loop over the Nodes Data.
                    {
                        node.Add(row["NodeID"].ToString());
                    }
                }
                myGraph.addVertexs(String.Join(",", node.ToArray()));
                myGraph.compeleteAddVertexs();

                string pathQuery = "SELECT LinkID, LinkLabel, StartNode, EndNode, CID, Color, CreatedDateTime FROM ConceptMap_Link WHERE (CID = '" + CID + "')";

                SqlCommand pathcmd = new SqlCommand(pathQuery);

                DataTable roofLeaf = GetDataTable(pathcmd);

                List<string> startnode = new List<string>();
                List<string> endnode = new List<string>();

                if (roofLeaf.Rows.Count > 0)
                {
                    foreach (DataRow row in roofLeaf.Rows) // Loop over the Links Data.
                    {
                        startnode.Add(row["StartNode"].ToString());
                        endnode.Add(row["EndNode"].ToString());
                        myGraph.addEdge(row["StartNode"].ToString(), row["EndNode"].ToString());
                    }
                }

                List<startendnode> senode = new List<startendnode>();

                //找root, leaf節點
                startnode.ForEach(delegate(String snode)
                {
                    DataRow[] sresult = roofLeaf.Select("EndNode='" + snode + "'");
                    if (sresult.Length == 0) //起始節點
                    {
                        endnode.ForEach(delegate(String enode)
                        {
                            DataRow[] eresult = roofLeaf.Select("StartNode='" + enode + "'");
                            
                            if (eresult.Length == 0) //節點末端，comment這段可找全部
                            {
                                senode.Add(new startendnode(snode, enode));

                                string seQuery = "SELECT * FROM ConceptMap_Path WHERE (ConceptMap_Path.CID = '" + CID + "') AND StartNode='" + snode + "' AND EndNode='" + enode + "'";
                                SqlCommand secmd = new SqlCommand(seQuery);
                                DataTable sedt = GetDataTable(secmd);
                                if (sedt.Rows.Count <= 0)
                                {
                                     string insQuery = "insert into ConceptMap_Path(CID, StartNode, EndNode) values('" + CID + "','" + snode + "', '" + enode + "')";
                                     CreateCommand(insQuery, strConnString);
			                    }
                            }
                        });
                    }
                });

                List<string> nodepath = new List<string>();
                
                string seQuery1 = "SELECT * FROM ConceptMap_Path WHERE (ConceptMap_Path.CID = '" + CID + "')";
                SqlCommand secmd1 = new SqlCommand(seQuery1);
                DataTable sedt1 = GetDataTable(secmd1);
                if (sedt1.Rows.Count > 0)
                {
                    foreach (DataRow row in sedt1.Rows) // Loop over the Links Data.
                    {
                        myGraph.searchAllPathInTwoVertex(row["StartNode"].ToString(), row["EndNode"].ToString());
                        List<string> tmp = myGraph.returnAllPath();

                        string delQuery = "delete from ConceptMap_Path where CID ='" + CID + "' AND StartNode='" + row["StartNode"].ToString() + "' AND EndNode='" + row["EndNode"].ToString() + "'";
                        CreateCommand(delQuery, strConnString);
                        
                        foreach (string tmp_path in tmp)
                        {   
                            string insQuery = "insert into ConceptMap_Path(CID,StartNode,EndNode,nodepath) values('" + CID + "','" + row["StartNode"].ToString() + "','" + row["EndNode"].ToString() + "','" + tmp_path + "')";
                            CreateCommand(insQuery, strConnString);
                        }
                    }
                }
                //foreach (var senodes in senode)
                //{
                //    nodepath.Clear();
                //    //nodepath.Push(senodes.eenode);

                //    //using (var connection = new SqlConnection(strConnString))
                //    //{
                //    //    connection.Open();
                //    //    var countCommand = new SqlCommand(
                //    //        "WITH ConceptMap_Link_BOM(StartNode, EndNode, LEVEL, SortCol) AS (SELECT StartNode, EndNode, 0, CONVERT(nvarchar(128), StartNode) FROM ConceptMap_Link WHERE EndNode = N'" + senodes.eenode + "' UNION ALL SELECT P.StartNode, P.EndNode, B. LEVEL + 1, CONVERT(nvarchar(128), B.SortCol + ';' + CONVERT(nvarchar(128), P.StartNode)) FROM ConceptMap_Link P, ConceptMap_Link_BOM B WHERE P.EndNode = B.StartNode) SELECT TOP 1 SortCol FROM ConceptMap_Link_BOM ORDER BY LEVEL DESC;", // this GROUP BY is superfluous, returns no rows
                //    //        connection);

                //    //    tmp = countCommand.ExecuteScalar().ToString();
                //    //}

                //    //string[] path = tmp.Split(';');
                //    //for (int i = 0; i < path.Length; i++)
                //    //{
                //    //    nodepath.Push(path[i]);
                //    //}
                    
                //    myGraph.searchAllPathInTwoVertex(senodes.ssnode, senodes.eenode);
                //    List<string> tmp = myGraph.returnAllPath();
                //    foreach (string tmp_path in tmp)
                //    {
                //        string updQuery = "update ConceptMap_Path set nodepath='" + tmp_path + "' where StartNode='" + senodes.ssnode + "' and EndNode='" + senodes.eenode + "'";

                //        CreateCommand(updQuery, strConnString);
                //    }


                //    //string concat = String.Join(";", nodepath.ToArray());
                    
                    
                //}
            }
            if (role == "student")
            {
                loadStartNodeDropdown(CID);
                loadEndNodeDropdown(CID);
            }
        }

        public Task SingleHighlightNodePath(string nodepath)
        {
            return Clients.Caller.singleNodePathLoaded(nodepath);
        }
        public Task SingleHighlightLink(string startnode, string endnode)
        {
            return Clients.Caller.singleNodePathLoaded(startnode, endnode);
        }
        public void loadNodePath(string startnode, string endnode, string CID)
        {
            //Clients.All.mask();
            //load nodes

            string pathquery = "SELECT nodepath FROM ConceptMap_Path WHERE CID = '"+CID+"' AND StartNode = '" + startnode + "' AND EndNode = '"+endnode+"'";
            SqlCommand pathcmd = new SqlCommand(pathquery);
            DataSet pathds = GetData(pathcmd);
            DataTable nodepath = pathds.Tables[0];
            foreach (DataRow nodesitem in nodepath.Rows)
            {
                SingleHighlightNodePath(nodesitem["nodepath"].ToString());
            }
            //string[] nodepatharr = nodepath["nodepath"].ToString().Split(';');
            
            //for(int i = 0; i < nodepatharr.Length-1; i++)
            //{
            //    SingleHighlightLink(nodepatharr[i], nodepatharr[i + 1]);
            //}
            //Clients.All.unmask();
        }
        public string comparekeyword(string Tkeyword, string Skeyword) 
        {
            string[] arr_T = Tkeyword.ToString().Split(' ');
            string[] arr_S = Skeyword.ToString().Split(' ');

            string[,] arr_compare = CompareKeywords.KeywordCompare1(arr_T, arr_S, false);

            int answer = 0;

            int rowLength = arr_compare.GetLength(0);
            int colLength = arr_compare.GetLength(1);
            //return arr_compare[rowLength - 1, colLength - 1].ToString();
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    string[] tmparr = { };
                    try
                    {
                        tmparr = arr_compare[i, j].ToString().Split('$');
                        answer += Convert.ToInt32(tmparr[0]);
                    }
                    catch { }
                }
            }
            if (answer == arr_T.Length)
                return "match";
            else if (answer < arr_T.Length && answer > 0)
                return "partial match";
            else
                return "mismatch";
        }
        public class tListItem{
            public string linklabel { get; set; }
            public string startnode { get; set; }
            public string endnode { get; set; }

            public tListItem()
            {
            }

            public tListItem(string linkLabelItem, string startNodeItem, string endNodeItem)
            {
                linklabel = linkLabelItem;
                startnode = startNodeItem;
                endnode = endNodeItem;
            }
        }
        public void browsingLoadSConceptMap(string CID, string tCID, string panel)
        {
            //Clients.All.mask();
            //load nodes
            //StartLoadingImage();
            List<string> tAnswer = new List<string>();
            List<tListItem> tListAnswer = new List<tListItem>();
            
            int m = 0;
            int pmatch = 0;
            int mmatch = 0;
            int additional = 0;

            string tNodesquery = "SELECT DISTINCT ConceptMap_Node.NodeID, ConceptMap_Node.CID, ConceptMap_Node.UserID, ConceptMap_Node.NodeLabel, ConceptMap_Node.Xcoordinate, ConceptMap_Node.Ycoordinate, ConceptMap_Node.Width, ConceptMap_Node.Height, ConceptMap_Node.ParentNode, ConceptMap_Node.[Level], ConceptMap_Node.Color, ConceptMap_Node.CreatedDateTime,TemplateID FROM ConceptMap_Node LEFT OUTER JOIN ConceptMap_NodeFieldsData ON ConceptMap_Node.NodeID = ConceptMap_NodeFieldsData.NodeID where CID=" + tCID + " order by ConceptMap_Node.CreatedDateTime";
            SqlCommand tNodescmd = new SqlCommand(tNodesquery);
            DataSet tNodesds = GetData(tNodescmd);
            //DataTable tNodesdt = tNodesds.Tables[0];
            foreach (DataRow tNodesitem in tNodesds.Tables[0].Rows)
            { 
                tAnswer.Add(tNodesitem["NodeLabel"].ToString());
            }

            string nodesquery = "SELECT DISTINCT ConceptMap_Node.NodeID, ConceptMap_Node.CID, ConceptMap_Node.UserID, ConceptMap_Node.NodeLabel, ConceptMap_Node.Xcoordinate, ConceptMap_Node.Ycoordinate, ConceptMap_Node.Width, ConceptMap_Node.Height, ConceptMap_Node.ParentNode, ConceptMap_Node.[Level], ConceptMap_Node.Color, ConceptMap_Node.CreatedDateTime,TemplateID FROM ConceptMap_Node LEFT OUTER JOIN ConceptMap_NodeFieldsData ON ConceptMap_Node.NodeID = ConceptMap_NodeFieldsData.NodeID where CID=" + CID + " order by ConceptMap_Node.CreatedDateTime";
            SqlCommand nodescmd = new SqlCommand(nodesquery);
            DataSet nodesds = GetData(nodescmd);
            //DataTable nodesdt = nodesds.Tables[0];
            foreach (DataRow nodesitem in nodesds.Tables[0].Rows)
            {

                if(panel=="designpannel"){
                    string color = "";
                    //bool match = tAnswer.Contains(nodesitem["NodeLabel"].ToString(), StringComparer.OrdinalIgnoreCase);
                    //if (match)
                    //{
                    //    color = "#54C571";
                    //    break;
                    //}
                    List<int> match = new List<int>();
                    foreach (string keyword in tAnswer)
                    {
                       
                        //if (comparekeyword(keyword, nodesitem["NodeLabel"].ToString()) == "match")
                        if (String.Equals(keyword, nodesitem["NodeLabel"].ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            match.Add(2);
                            //color = "#54C571";
                            //break;
                        }
                        else if (comparekeyword(keyword, nodesitem["NodeLabel"].ToString()) == "partial match")
                        {
                            match.Add(1);
                         
                            //color = "#C3FDB8";
                            //break;
                        }
                        else if(nodesitem["NodeLabel"].ToString()=="storage device")
                        {
                            match.Add(1);

                            //color = "#C3FDB8";
                            //break;
                        }
                        else if (comparekeyword(keyword, nodesitem["NodeLabel"].ToString()) == "mismatch")
                        {
                            match.Add(0);
                            //color = "#FF0000";
                        }
                    }
                    match.Sort();
                    match.Reverse();
                    int matchresult = match.First();

                    if (matchresult == 2)
                    {
                        loadmatchtable(nodesitem["NodeLabel"].ToString());
                        color = "#54C571";
                        m++;
                    }
                    else if (matchresult == 1)
                    {
                        loadpmatchtable(nodesitem["NodeLabel"].ToString());
                        color = "#C3FDB8";
                        pmatch++;
                    }
                    else
                    {
                        loadaddtable(nodesitem["NodeLabel"].ToString());
                        color = "#FFFF00";
                        additional++;
                    }
                    SingleLoadShape(nodesitem["NodeID"].ToString(), nodesitem["NodeLabel"].ToString(), nodesitem["TemplateID"].ToString(), color, nodesitem["Xcoordinate"].ToString(), nodesitem["Ycoordinate"].ToString(), nodesitem["Width"].ToString(), nodesitem["Height"].ToString(), panel, nodesitem["CreatedDateTime"].ToString());
                }
                else{
                    SingleLoadShape(nodesitem["NodeID"].ToString(), nodesitem["NodeLabel"].ToString(), nodesitem["TemplateID"].ToString(), nodesitem["Color"].ToString(), nodesitem["Xcoordinate"].ToString(), nodesitem["Ycoordinate"].ToString(), nodesitem["Width"].ToString(), nodesitem["Height"].ToString(), panel, nodesitem["CreatedDateTime"].ToString());
                }
                
                string nodefieldsquery = "SELECT FieldName,FieldValue,Type FROM ConceptMap_NodeFieldsData where NodeID='" + nodesitem["NodeID"].ToString() + "'";
                SqlCommand nodefieldscmd = new SqlCommand(nodefieldsquery);
                DataSet nodefieldsds = GetData(nodefieldscmd);
                if (nodefieldsds.Tables[0].Rows.Count > 0)
                {
                    DataTable nodefieldsdt = nodefieldsds.Tables[0];
                    SingleSettingTemplateData(nodesitem["NodeID"].ToString(), NodeManagement.DataTableToJSON(nodefieldsdt));
                }
            }
            SSetChartValue(m, pmatch, additional);




            //load links
            string linksquery = "SELECT DISTINCT " +
                      "ConceptMap_Link.LinkID, ConceptMap_Link.LinkLabel, ConceptMap_Link.StartNode, ConceptMap_Link.EndNode, ConceptMap_Link.CID, ConceptMap_Link.Color, " +
                      "(SELECT DISTINCT NodeLabel FROM ConceptMap_Node WHERE (NodeID = ConceptMap_Link.StartNode) AND (CID = ConceptMap_Link.CID)) AS StartNodeLabel, (SELECT DISTINCT NodeLabel FROM ConceptMap_Node AS ConceptMap_Node_1 WHERE (NodeID = ConceptMap_Link.EndNode) AND (CID = ConceptMap_Link.CID)) AS EndNodeLabel, " +
                      "ConceptMap_Link.CreatedDateTime, ConceptMap_LinkFieldsData.TemplateID " +
                      "FROM ConceptMap_Link LEFT OUTER JOIN ConceptMap_LinkFieldsData ON ConceptMap_Link.CID = ConceptMap_LinkFieldsData.CID " +
                      "WHERE (ConceptMap_Link.CID = '" + CID + "') ORDER BY ConceptMap_Link.CreatedDateTime";
            
            SqlCommand linkscmd = new SqlCommand(linksquery);
            DataSet linksds = GetData(linkscmd);
            //DataTable linksdt = linksds.Tables[0];

            string tLinksquery = "SELECT StartNode, EndNode, LinkID, CID, LinkLabel, Color, CreatedDateTime From ConceptMap_Link WHERE (CID = '"+tCID+"') ORDER BY CreatedDateTime";
            SqlCommand tLinkscmd = new SqlCommand(tLinksquery);
            DataSet tLinksds = GetData(tLinkscmd);
            //DataTable tNodesdt = tNodesds.Tables[0];
            string startnode = "";
            string endnode = "";
            foreach (DataRow tLinksitem in tLinksds.Tables[0].Rows)
            {
                string snquery = "select NodeLabel from ConceptMap_Node where NodeID='" + tLinksitem["StartNode"].ToString() + "'";
                SqlCommand sncmd = new SqlCommand(snquery);
                DataSet snsds = GetData(sncmd);
               
                foreach (DataRow snlabel in snsds.Tables[0].Rows)
                {
                    startnode = snlabel["NodeLabel"].ToString();
                }
                string enquery = "select NodeLabel from ConceptMap_Node where NodeID='" + tLinksitem["EndNode"].ToString() + "'";
                SqlCommand encmd = new SqlCommand(enquery);
                DataSet ends = GetData(encmd);
                foreach (DataRow enlabel in ends.Tables[0].Rows)
                {
                    endnode = enlabel["NodeLabel"].ToString();
                }

                tListAnswer.Add(new tListItem(tLinksitem["LinkLabel"].ToString(), startnode, endnode));
            }

            foreach (DataRow linksitem in linksds.Tables[0].Rows)
            {
                if (panel == "designpannel")
                {
                    string color = "";
                    //bool match = tAnswer.Contains(nodesitem["NodeLabel"].ToString(), StringComparer.OrdinalIgnoreCase);
                    //if (match)
                    //{
                    //    color = "#54C571";
                    //    break;
                    //}
                    List<int> match = new List<int>();
                    foreach (var keyword in tListAnswer)
                    {

                        if (String.Equals(keyword.linklabel, linksitem["LinkLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(5); //match
                            else if (!String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(3); //head not match
                            else if (String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && !String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(2); //tail not match    
                            else if (!String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && !String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(1); //both not match
                            //    //color = "#54C571";
                            //break;
                        }
                        else if (comparekeyword(keyword.linklabel, linksitem["LinkLabel"].ToString()) == "partial match")
                        {
                            match.Add(4);
                            //color = "#C3FDB8";
                            //break;
                        }
                        else if (comparekeyword(keyword.linklabel, linksitem["LinkLabel"].ToString()) == "mismatch")
                        {
                            match.Add(0);
                            //color = "#FF0000";
                        }
                    }
                    match.Sort();
                    match.Reverse();
                    int matchresult = match.First();

                    if (matchresult == 5)
                        color = "#54C571";
                    else if (matchresult == 4)
                        color = "#C3FDB8";
                    else if (matchresult == 3)
                        color = "#82CAFF";
                    else if (matchresult == 2)
                        color = "#D462FF";
                    else if (matchresult == 1)
                        color = "#FFA62F";
                    else if (matchresult == 0)
                        color = "#FF0000";
                    match.Clear();
                    SingleLoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), color, linksitem["TemplateID"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), panel, linksitem["CreatedDateTime"].ToString());
                }
                else
                {
                    if (linksitem["TemplateID"].ToString() != "")
                        SingleLoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), linksitem["TemplateID"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), panel, linksitem["CreatedDateTime"].ToString());
                    else
                        SingleLoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), "0", linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), panel, linksitem["CreatedDateTime"].ToString());
                }
                //ConnectNode(linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), "", "", linksitem["LinkLabel"].ToString());
            }
            //StopLoadingImage();
            //Clients.All.unmask();
        }
        public void browsingLoadTConceptMap(string CID, string sCID, string panel)
        {
            //Clients.All.mask();
            //load nodes
            List<string> tAnswer = new List<string>();
            List<tListItem> tListAnswer = new List<tListItem>();

            int m = 0;
            int pmatch = 0;
            int mmatch = 0;
            int additional = 0;

            string tNodesquery = "SELECT DISTINCT ConceptMap_Node.NodeID, ConceptMap_Node.CID, ConceptMap_Node.UserID, ConceptMap_Node.NodeLabel, ConceptMap_Node.Xcoordinate, ConceptMap_Node.Ycoordinate, ConceptMap_Node.Width, ConceptMap_Node.Height, ConceptMap_Node.ParentNode, ConceptMap_Node.[Level], ConceptMap_Node.Color, ConceptMap_Node.CreatedDateTime,TemplateID FROM ConceptMap_Node LEFT OUTER JOIN ConceptMap_NodeFieldsData ON ConceptMap_Node.NodeID = ConceptMap_NodeFieldsData.NodeID where CID=" + sCID + " order by ConceptMap_Node.CreatedDateTime";
            SqlCommand tNodescmd = new SqlCommand(tNodesquery);
            DataSet tNodesds = GetData(tNodescmd);
            //DataTable tNodesdt = tNodesds.Tables[0];
            foreach (DataRow tNodesitem in tNodesds.Tables[0].Rows)
            {
                tAnswer.Add(tNodesitem["NodeLabel"].ToString());
            }

            string nodesquery = "SELECT DISTINCT ConceptMap_Node.NodeID, ConceptMap_Node.CID, ConceptMap_Node.UserID, ConceptMap_Node.NodeLabel, ConceptMap_Node.Xcoordinate, ConceptMap_Node.Ycoordinate, ConceptMap_Node.Width, ConceptMap_Node.Height, ConceptMap_Node.ParentNode, ConceptMap_Node.[Level], ConceptMap_Node.Color, ConceptMap_Node.CreatedDateTime,TemplateID FROM ConceptMap_Node LEFT OUTER JOIN ConceptMap_NodeFieldsData ON ConceptMap_Node.NodeID = ConceptMap_NodeFieldsData.NodeID where CID=" + CID + " order by ConceptMap_Node.CreatedDateTime";
            SqlCommand nodescmd = new SqlCommand(nodesquery);
            DataSet nodesds = GetData(nodescmd);
            DataTable nodesdt = nodesds.Tables[0];
            foreach (DataRow nodesitem in nodesds.Tables[0].Rows)
            {
                if (panel == "designpannelt")
                {
                    string color = "";
                    //bool match = tAnswer.Contains(nodesitem["NodeLabel"].ToString(), StringComparer.OrdinalIgnoreCase);
                    //if (match)
                    //{
                    //    color = "#54C571";
                    //    break;
                    //}
                    List<int> match = new List<int>();
                    foreach (string keyword in tAnswer)
                    {

                        //if (comparekeyword(keyword, nodesitem["NodeLabel"].ToString()) == "match")
                        if (String.Equals(keyword, nodesitem["NodeLabel"].ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            match.Add(2);
                            //color = "#54C571";
                            //break;
                        }
                        else if (comparekeyword(keyword, nodesitem["NodeLabel"].ToString()) == "partial match")
                        {
                            match.Add(1);

                            //color = "#C3FDB8";
                            //break;
                        }
                        else if (comparekeyword(keyword, nodesitem["NodeLabel"].ToString()) == "mismatch")
                        {
                            match.Add(0);
                            //color = "#FF0000";
                        }
                    }
                    match.Sort();
                    match.Reverse();
                    int matchresult = match.First();

                    if (matchresult == 2)
                    {
                        //loadmatchtable(nodesitem["NodeLabel"].ToString());
                        color = "#54C571";
                        m++;
                    }
                    else if (matchresult == 1)
                    {
                        //loadpmatchtable(nodesitem["NodeLabel"].ToString());
                        color = "#C3FDB8";
                        pmatch++;
                    }
                    else
                    {
                        loadmmatchtable(nodesitem["NodeLabel"].ToString());
                        color = "#FF0000";
                        mmatch++;
                    }
                    SingleLoadShape(nodesitem["NodeID"].ToString(), nodesitem["NodeLabel"].ToString(), nodesitem["TemplateID"].ToString(), color, nodesitem["Xcoordinate"].ToString(), nodesitem["Ycoordinate"].ToString(), nodesitem["Width"].ToString(), nodesitem["Height"].ToString(), panel, nodesitem["CreatedDateTime"].ToString());
                }
                else
                {
                    SingleLoadShape(nodesitem["NodeID"].ToString(), nodesitem["NodeLabel"].ToString(), nodesitem["TemplateID"].ToString(), nodesitem["Color"].ToString(), nodesitem["Xcoordinate"].ToString(), nodesitem["Ycoordinate"].ToString(), nodesitem["Width"].ToString(), nodesitem["Height"].ToString(), panel, nodesitem["CreatedDateTime"].ToString());
                }
                //SingleLoadShape(nodesitem["NodeID"].ToString(), nodesitem["NodeLabel"].ToString(), nodesitem["TemplateID"].ToString(), nodesitem["Color"].ToString(), nodesitem["Xcoordinate"].ToString(), nodesitem["Ycoordinate"].ToString(), nodesitem["Width"].ToString(), nodesitem["Height"].ToString(), panel, nodesitem["CreatedDateTime"].ToString());
                
                string nodefieldsquery = "SELECT FieldName,FieldValue,Type FROM ConceptMap_NodeFieldsData where NodeID='" + nodesitem["NodeID"].ToString() + "' and Show=1";
                SqlCommand nodefieldscmd = new SqlCommand(nodefieldsquery);
                DataSet nodefieldsds = GetData(nodefieldscmd);
                if (nodefieldsds.Tables[0].Rows.Count > 0)
                {
                    DataTable nodefieldsdt = nodefieldsds.Tables[0];
                    SingleSettingTemplateData(nodesitem["NodeID"].ToString(), NodeManagement.DataTableToJSON(nodefieldsdt));
                }
            }

            //load links
            string linksquery = "SELECT DISTINCT " +
                      "ConceptMap_Link.LinkID, ConceptMap_Link.LinkLabel, ConceptMap_Link.StartNode, ConceptMap_Link.EndNode, ConceptMap_Link.CID, ConceptMap_Link.Color, " +
                      "(SELECT DISTINCT NodeLabel FROM ConceptMap_Node WHERE (NodeID = ConceptMap_Link.StartNode) AND (CID = ConceptMap_Link.CID)) AS StartNodeLabel, (SELECT DISTINCT NodeLabel FROM ConceptMap_Node AS ConceptMap_Node_1 WHERE (NodeID = ConceptMap_Link.EndNode) AND (CID = ConceptMap_Link.CID)) AS EndNodeLabel, " +
                      "ConceptMap_Link.CreatedDateTime, ConceptMap_LinkFieldsData.TemplateID " +
                      "FROM ConceptMap_Link LEFT OUTER JOIN ConceptMap_LinkFieldsData ON ConceptMap_Link.CID = ConceptMap_LinkFieldsData.CID " +
                      "WHERE (ConceptMap_Link.CID = '" + CID + "')";
            SqlCommand linkscmd = new SqlCommand(linksquery);
            DataSet linksds = GetData(linkscmd);
            DataTable linksdt = linksds.Tables[0];

            string tLinksquery = "SELECT StartNode, EndNode, LinkID, CID, LinkLabel, Color, CreatedDateTime From ConceptMap_Link WHERE (CID = '" + sCID + "') ORDER BY CreatedDateTime";
            SqlCommand tLinkscmd = new SqlCommand(tLinksquery);
            DataSet tLinksds = GetData(tLinkscmd);
            //DataTable tNodesdt = tNodesds.Tables[0];
            string startnode = "";
            string endnode = "";
            foreach (DataRow tLinksitem in tLinksds.Tables[0].Rows)
            {
                string snquery = "select NodeLabel from ConceptMap_Node where NodeID='" + tLinksitem["StartNode"].ToString() + "'";
                SqlCommand sncmd = new SqlCommand(snquery);
                DataSet snsds = GetData(sncmd);

                foreach (DataRow snlabel in snsds.Tables[0].Rows)
                {
                    startnode = snlabel["NodeLabel"].ToString();
                }
                string enquery = "select NodeLabel from ConceptMap_Node where NodeID='" + tLinksitem["EndNode"].ToString() + "'";
                SqlCommand encmd = new SqlCommand(enquery);
                DataSet ends = GetData(encmd);
                foreach (DataRow enlabel in ends.Tables[0].Rows)
                {
                    endnode = enlabel["NodeLabel"].ToString();
                }

                tListAnswer.Add(new tListItem(tLinksitem["LinkLabel"].ToString(), startnode, endnode));
            }

            foreach (DataRow linksitem in linksds.Tables[0].Rows)
            {
                if (panel == "designpannelt")
                {
                    string color = "";
                    //bool match = tAnswer.Contains(nodesitem["NodeLabel"].ToString(), StringComparer.OrdinalIgnoreCase);
                    //if (match)
                    //{
                    //    color = "#54C571";
                    //    break;
                    //}
                    List<int> match = new List<int>();
                    foreach (var keyword in tListAnswer)
                    {

                        if (String.Equals(keyword.linklabel, linksitem["LinkLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(5); //match
                            else if (!String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(3); //head not match
                            else if (String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && !String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(2); //tail not match    
                            else if (!String.Equals(keyword.startnode, linksitem["StartNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase) && !String.Equals(keyword.endnode, linksitem["EndNodeLabel"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                match.Add(1); //both not match
                            //    //color = "#54C571";
                            //break;
                        }
                        else if (comparekeyword(keyword.linklabel, linksitem["LinkLabel"].ToString()) == "partial match")
                        {
                            match.Add(4);
                            //color = "#C3FDB8";
                            //break;
                        }
                        else if (comparekeyword(keyword.linklabel, linksitem["LinkLabel"].ToString()) == "mismatch")
                        {
                            match.Add(0);
                            //color = "#FF0000";
                        }
                    }
                    match.Sort();
                    match.Reverse();
                    int matchresult = match.First();

                    if (matchresult == 5)
                        color = "#54C571";
                    else if (matchresult == 4)
                        color = "#C3FDB8";
                    else if (matchresult == 3)
                        color = "#82CAFF";
                    else if (matchresult == 2)
                        color = "#D462FF";
                    else if (matchresult == 1)
                        color = "#FFA62F";
                    else if (matchresult == 0)
                        color = "#FF0000";
                    match.Clear();
                    SingleLoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), color, linksitem["TemplateID"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), panel, linksitem["CreatedDateTime"].ToString());
                }
                else
                {
                    if (linksitem["TemplateID"].ToString() != "")
                        SingleLoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), linksitem["TemplateID"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), panel, linksitem["CreatedDateTime"].ToString());
                    else
                        SingleLoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), "0", linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), panel, linksitem["CreatedDateTime"].ToString());
                }
                //ConnectNode(linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), "", "", linksitem["LinkLabel"].ToString());
            }

            TSetChartValue(mmatch);
            SingleLoadChart();
            //Clients.All.unmask();
        }
        public void loadConceptMap(string CID) 
        {
            //Clients.All.mask();
            //load nodes
            string nodesquery = "SELECT DISTINCT ConceptMap_Node.NodeID, ConceptMap_Node.CID, ConceptMap_Node.UserID, ConceptMap_Node.NodeLabel, ConceptMap_Node.Xcoordinate, ConceptMap_Node.Ycoordinate, ConceptMap_Node.Width, ConceptMap_Node.Height, ConceptMap_Node.ParentNode, ConceptMap_Node.[Level], ConceptMap_Node.Color, ConceptMap_Node.CreatedDateTime,TemplateID FROM ConceptMap_Node LEFT OUTER JOIN ConceptMap_NodeFieldsData ON ConceptMap_Node.NodeID = ConceptMap_NodeFieldsData.NodeID where CID=" + CID + "";
            SqlCommand nodescmd = new SqlCommand(nodesquery);
            DataSet nodesds = GetData(nodescmd);
            DataTable nodesdt = nodesds.Tables[0];
            foreach (DataRow nodesitem in nodesds.Tables[0].Rows)
            {
                LoadShape(nodesitem["NodeID"].ToString(), nodesitem["NodeLabel"].ToString(), nodesitem["TemplateID"].ToString(), nodesitem["Color"].ToString(), nodesitem["Xcoordinate"].ToString(), nodesitem["Ycoordinate"].ToString(), nodesitem["Width"].ToString(), nodesitem["Height"].ToString());
                    string nodefieldsquery = "SELECT FieldName,FieldValue,Type FROM ConceptMap_NodeFieldsData where NodeID='" + nodesitem["NodeID"].ToString() + "' and Show=1";
                    SqlCommand nodefieldscmd = new SqlCommand(nodefieldsquery);
                    DataSet nodefieldsds = GetData(nodefieldscmd);
                    if (nodefieldsds.Tables[0].Rows.Count > 0)
                    {
                        DataTable nodefieldsdt = nodefieldsds.Tables[0];
                        SettingTemplateData(nodesitem["NodeID"].ToString(), NodeManagement.DataTableToJSON(nodefieldsdt));
                    }
            }

            //load links
            string linksquery = "SELECT DISTINCT "+
                      "ConceptMap_Link.LinkID, ConceptMap_Link.LinkLabel, ConceptMap_Link.StartNode, ConceptMap_Link.EndNode, ConceptMap_Link.CID, ConceptMap_Link.Color, "+
                      "ConceptMap_Link.CreatedDateTime, ConceptMap_LinkFieldsData.TemplateID "+
                      "FROM ConceptMap_Link LEFT OUTER JOIN ConceptMap_LinkFieldsData ON ConceptMap_Link.CID = ConceptMap_LinkFieldsData.CID "+
                      "WHERE (ConceptMap_Link.CID = '"+CID+"')";
            SqlCommand linkscmd = new SqlCommand(linksquery);
            DataSet linksds = GetData(linkscmd);
            DataTable linksdt = linksds.Tables[0];
            
            foreach (DataRow linksitem in linksds.Tables[0].Rows)
            {
                if (linksitem["TemplateID"].ToString()!="")
                    LoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), linksitem["TemplateID"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString());
                else
                    LoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), "0", linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString());
                //ConnectNode(linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), "", "", linksitem["LinkLabel"].ToString());
            }
            //Clients.All.unmask();
        }
        public void loadConceptMap(string CID, string groupID)
        {
            //Clients.All.mask();
            //load nodes
            StartLoadingImage(groupID);
            string nodesquery = "SELECT DISTINCT ConceptMap_Node.NodeID, ConceptMap_Node.CID, ConceptMap_Node.UserID, ConceptMap_Node.NodeLabel, ConceptMap_Node.Xcoordinate, ConceptMap_Node.Ycoordinate, ConceptMap_Node.Width, ConceptMap_Node.Height, ConceptMap_Node.ParentNode, ConceptMap_Node.[Level], ConceptMap_Node.Color, ConceptMap_Node.CreatedDateTime,TemplateID FROM ConceptMap_Node LEFT OUTER JOIN ConceptMap_NodeFieldsData ON ConceptMap_Node.NodeID = ConceptMap_NodeFieldsData.NodeID where CID=" + CID + "";
            SqlCommand nodescmd = new SqlCommand(nodesquery);
            DataSet nodesds = GetData(nodescmd);
            DataTable nodesdt = nodesds.Tables[0];
            foreach (DataRow nodesitem in nodesds.Tables[0].Rows)
            {
                LoadShape(nodesitem["NodeID"].ToString(), nodesitem["NodeLabel"].ToString(), nodesitem["TemplateID"].ToString(), nodesitem["Color"].ToString(), nodesitem["Xcoordinate"].ToString(), nodesitem["Ycoordinate"].ToString(), nodesitem["Width"].ToString(), nodesitem["Height"].ToString(),groupID);
                string nodefieldsquery = "SELECT FieldName,FieldValue,Type FROM ConceptMap_NodeFieldsData where NodeID='" + nodesitem["NodeID"].ToString() + "' and Show=1";
                SqlCommand nodefieldscmd = new SqlCommand(nodefieldsquery);
                DataSet nodefieldsds = GetData(nodefieldscmd);
                if (nodefieldsds.Tables[0].Rows.Count > 0)
                {
                    DataTable nodefieldsdt = nodefieldsds.Tables[0];
                    SettingTemplateData(nodesitem["NodeID"].ToString(), NodeManagement.DataTableToJSON(nodefieldsdt), groupID);
                }
            }

            //load links
            //string linksquery = "SELECT DISTINCT ConceptMap_Link.*,ConceptMap_LinkFieldsData.TemplateID " +
            //                    "FROM ConceptMap_Link INNER JOIN ConceptMap_LinkFieldsData ON " +
            //                    "ConceptMap_Link.CID = ConceptMap_LinkFieldsData.CID AND ConceptMap_Link.LinkID = ConceptMap_LinkFieldsData.NodeID " +
            //                    "AND ConceptMap_Link.CID='" + CID + "'";
            string linksquery = "SELECT DISTINCT " +
                        "ConceptMap_Link.LinkID, ConceptMap_Link.LinkLabel, ConceptMap_Link.StartNode, ConceptMap_Link.EndNode, ConceptMap_Link.CID, ConceptMap_Link.Color, " +
                        "ConceptMap_Link.CreatedDateTime, ConceptMap_LinkFieldsData.TemplateID " +
                        "FROM ConceptMap_Link LEFT OUTER JOIN ConceptMap_LinkFieldsData ON ConceptMap_Link.CID = ConceptMap_LinkFieldsData.CID " +
                        "WHERE (ConceptMap_Link.CID = '" + CID + "')";
            SqlCommand linkscmd = new SqlCommand(linksquery);
            DataSet linksds = GetData(linkscmd);
            DataTable linksdt = linksds.Tables[0];
            foreach (DataRow linksitem in linksds.Tables[0].Rows)
            {
                if (linksitem["TemplateID"].ToString() != "")
                    LoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), linksitem["TemplateID"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), groupID);
                else
                    LoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), "0", linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), groupID);
               
                //LoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["Color"].ToString(), linksitem["TemplateID"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), groupID);
                //ConnectNode(linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString(), "", "", linksitem["LinkLabel"].ToString());
            }
            StopLoadingImage(groupID);
            //Clients.All.unmask();
        }
        public void loadTemplateField(string TID)
        {
            //Clients.All.mask();
            //load nodes
            string templatequery = "SELECT TemplateNodeID, TemplateID, FieldName, FieldValue, Type, Show " 
                                +"FROM ConceptMap_TemplateNode "
                                +"WHERE     (TemplateID = " + TID + ") "
                                +"ORDER BY Type, FieldName";
            SqlCommand templatecmd = new SqlCommand(templatequery);
            DataSet templateds = GetData(templatecmd);
            DataTable templatedt = templateds.Tables[0];
            foreach (DataRow templateitem in templateds.Tables[0].Rows)
            {

                LoadField(templateitem["FieldName"].ToString(), templateitem["FieldValue"].ToString());
            }

            //load links
            //string linksquery = "SELECT * FROM ConceptMap_Link where CID=" + TID + "";
            //SqlCommand linkscmd = new SqlCommand(linksquery);
            //DataSet linksds = GetData(linkscmd);
            //DataTable linksdt = linksds.Tables[0];
            //foreach (DataRow linksitem in linksds.Tables[0].Rows)
            //{
            //    LoadLink(linksitem["LinkID"].ToString(), linksitem["LinkLabel"].ToString(), linksitem["StartNode"].ToString(), linksitem["EndNode"].ToString());
            //}
            //Clients.All.unmask();
        }
        private static DataSet GetData(SqlCommand cmd)
        {
            string constr = ConfigurationManager.ConnectionStrings["NewVersionHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }
        private static DataSet GetChangHanData(SqlCommand cmd)
        {
            string constr = ConfigurationManager.ConnectionStrings["ChangHanHintsDBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }
        public void generateComparePathList(string studentCID, string teacherCID)
        {
            string prepathQuery = "SELECT rootleaf,path FROM ConceptMap_PathCompareResult WHERE (sCID = '" + studentCID + "') AND tCID = '" + teacherCID + "' ORDER BY id";

            SqlCommand prepathcmd = new SqlCommand(prepathQuery);

            DataTable prepathdt = GetDataTable(prepathcmd);

            if (prepathdt.Rows.Count > 0)
            {
                int count = 1;
                foreach (DataRow row in prepathdt.Rows) // Loop over the rows.
                {
                    ListComparePath((count++).ToString(), row["rootleaf"].ToString(), row["path"].ToString());
                }
            }else{
            List<string> sPath = getStartToEndNodePath(studentCID);
            List<string> tPath = getStartToEndNodePath(teacherCID);

            List<string> sFullPath = getfullpathText(sPath, studentCID);
            List<string> tFullPath = getfullpathText(tPath, teacherCID);

            DataTable dt1 = new DataTable();
            dt1.Clear();
            dt1.Columns.Add("cAnnotationNum");

            DataTable dt2 = new DataTable();
            dt2.Clear();
            dt2.Columns.Add("cAnnotationNum");

            List<string> answer = new List<string>();
            List<string> finalans = new List<string>();
            List<string> rootvalue = new List<string>();
            List<string> leafvalue = new List<string>();

            var list = new List<KeyValuePair<int, string>>();
            var rootleaf = new List<KeyValuePair<int, string>>();


            for (int i = 0; i < sFullPath.Count; i++) // Loop through List with for
            {

                string[] tmptnode = sFullPath[i].Split(',');
                foreach (string tnodestr in tmptnode)
                    dt1.Rows.Add(tnodestr);


                for (int j = 0; j < tFullPath.Count; j++) // Loop through List with for
                {

                    string[] tmpsnode = tFullPath[j].Split(',');
                    foreach (string snodestr in tmpsnode)
                    {

                        dt2.Rows.Add(snodestr);
                    }

                    for (int k = 0; k < dt1.Rows.Count; k++)
                    {
                        //stdanswer.Text += dt1.Rows[k]["cAnnotationNum"].ToString() + " ";
                    }
                    //stdanswer.Text += "<br />";
                    int[,] arr = EditDistance(dt1, dt2);

                    int rowLength = arr.GetLength(0);
                    int colLength = arr.GetLength(1);

                    string tmpans = CreatStudentAnswerTable(dt2, dt1);


                    string[] tmparr = tmpans.Split('@');
                    list.Add(new KeyValuePair<int, string>(int.Parse(tmparr[0]), tmparr[1]));
                    //rootleaf.Add(new KeyValuePair<int, string>(int.Parse(tmparr[0]), tmparr[1]));

                    dt2.Clear();
                }
                list.Sort(Compare1);
                list.Reverse();

                //rootleaf.Sort(Compare1);
                //rootleaf.Reverse();

                //Label1.Text = list[0].Value;
                //ListComparePath((i+1).ToString(), list[0].Value);
                foreach (var value in list)
                {
                    finalans.Add(value.Value);
                }
                //foreach (var value in rootleaf)
                //{
                //    rootvalue.Add(value.Value);
                //}
                //finalans.Add(list[0].Value);
                dt1.Clear();
                //dt1.Clear();

                //break;
         
            }
            var distinct = (from item in finalans orderby item select item).Distinct();
            //var distinctroot = (from item in rootvalue orderby item select item).Distinct();
            //var distinctleaf = (from item in leafvalue orderby item select item).Distinct().ToList();
            int count = 1;
            foreach (string value in distinct)
            {
                string[] rf_n_ans = value.Split('|') ;
                //ListComparePath((count++).ToString(), "(" + distinctroot[index].ToString() + "," + leaf[index++].Value.ToString() + ")", value);

                string insQuery = "insert into ConceptMap_PathCompareResult(sCID, tCID, rootleaf, path) values('" + studentCID + "','" + teacherCID + "','" + rf_n_ans[0] + "', '" + rf_n_ans[1] + "')";
                CreateCommand(insQuery, strConnString);

                ListComparePath((count++).ToString(), rf_n_ans[0], rf_n_ans[1]);
            }
            //foreach (string value in distinctroot)
            //{
            //    //ListComparePath((count++).ToString(), "(" + distinctroot[index].ToString() + "," + leaf[index++].Value.ToString() + ")", value);
            //    ListComparePath((count++).ToString(), "", value);
            //}
            }
        }

        public int Compare1(KeyValuePair<int, string> a, KeyValuePair<int, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }
        public List<string> getfullpathText(List<string> nodepath, string CID)
        {

            //string pathquery = "SELECT Top 1 nodepath FROM ConceptMap_Path WHERE StartNode = '" + startnode + "' AND EndNode = '" + endnode + "'";
            //SqlCommand pathcmd = new SqlCommand(pathquery);
            //DataSet pathds = GetData(pathcmd);
            //DataRow nodepath = pathds.Tables[0].Rows[0];
            //SingleHighlightNodePath(nodepath["nodepath"].ToString());

            Hashtable nodelabel = new Hashtable();
            string nodeQuery = "SELECT * FROM ConceptMap_Node WHERE (CID = '" + CID + "')";

            SqlCommand nodecmd = new SqlCommand(nodeQuery);

            DataTable nodetable = GetDataTable(nodecmd);

            if (nodetable.Rows.Count > 0)
            {
                foreach (DataRow row in nodetable.Rows) // Loop over the rows.
                {
                    nodelabel.Add(row["NodeID"].ToString(), row["NodeLabel"].ToString());
                }
            }

            List<string> linkpath = new List<string>();
            string linklabel = "";
            string linkpathstr = "";
            for (int i = 0; i < nodepath.Count; i++)
            {
                string[] fullpath = nodepath[i].Split(';');
                for (int j = 0; j < fullpath.Length - 1; j++)
                {
                    if (j == 0)
                        linkpathstr += nodelabel[fullpath[j]].ToString() + ",";
                    using (var connection = new SqlConnection(strConnString))
                    {
                        connection.Open();
                        var countCommand = new SqlCommand(
                            "select LinkLabel from ConceptMap_Link where StartNode = '" + fullpath[j] + "' and EndNode = '" + fullpath[j + 1] + "'",
                            connection);

                        linklabel = countCommand.ExecuteScalar().ToString();
                        connection.Close();
                        linkpathstr += linklabel + ",";
                        linkpathstr += nodelabel[fullpath[j + 1]].ToString() + ",";
                    }
                }
                linkpathstr = linkpathstr.Remove(linkpathstr.Length - 1);
                linkpath.Add(linkpathstr);
                linkpathstr = "";
            }
            return linkpath;
        }







        //for (int i = 0; i < rowLength; i++)
        //{
        //    for (int j = 0; j < colLength; j++)
        //    {
        //        Response.Write(string.Format("{0}, ", arr[i, j]));
        //    }
        //    Response.Write(string.Format("<br />"));
        //    //Console.Write(Environment.NewLine + Environment.NewLine);
        //}
        //Response.Write(string.Format("<br />"));
        //Response.Write(string.Format("<br />"));
        //string[] shortestp = getShortestPath(arr);
        //for (int k = 0; k < shortestp.Length; k++)
        //{
        //    Response.Write(string.Format("{0}, ", shortestp[k]));
        //    Response.Write(string.Format("<br />"));
        //}
        //Panel1.Controls.Add(CreatStudentAnswerTable(dt1, dt2));

        //Console.ReadLine();

        public List<string> getStartToEndNodePath(string CID)
        {
            List<string> pathstr = new List<string>();

            string prepathQuery = "SELECT nodepath FROM ConceptMap_Path WHERE (CID = '" + CID + "') ORDER BY CID";

            SqlCommand prepathcmd = new SqlCommand(prepathQuery);

            DataTable prepathdt = GetDataTable(prepathcmd);

            if (prepathdt.Rows.Count > 0)
            {
                foreach (DataRow row in prepathdt.Rows) // Loop over the rows.
                {
                    pathstr.Add(row["nodepath"].ToString());
                }
            }
            else
            {

                string pathQuery = "SELECT DISTINCT LinkID, LinkLabel, StartNode, EndNode, CID, Color, CreatedDateTime FROM ConceptMap_Link WHERE (CID = '" + CID + "') ORDER BY CID";

                SqlCommand pathcmd = new SqlCommand(pathQuery);

                DataTable roofLeaf = GetDataTable(pathcmd);

                List<string> startnode = new List<string>();
                List<string> endnode = new List<string>();

                //List<startendnode> senode = new List<startendnode>();

                if (roofLeaf.Rows.Count > 0)
                {

                    foreach (DataRow row in roofLeaf.Rows) // Loop over the rows.
                    {
                        startnode.Add(row["StartNode"].ToString());
                        endnode.Add(row["EndNode"].ToString());
                        //senode.Add(new startendnode(row["StartNode"].ToString(), row["EndNode"].ToString()));
                        //endnode.Add();
                    }

                }

                List<startendnode> senode = new List<startendnode>();

                startnode.ForEach(delegate(String snode)
                {
                    DataRow[] sresult = roofLeaf.Select("EndNode='" + snode + "'");
                    if (sresult.Length == 0) //起始節點
                    {
                        endnode.ForEach(delegate(String enode)
                        {
                            DataRow[] eresult = roofLeaf.Select("StartNode='" + enode + "'");

                            if (eresult.Length == 0) //節點末端
                            {
                                senode.Add(new startendnode(snode, enode));
                            }
                        });
                    }
                });




                string fullpathQuery = "";

                foreach (var senodes in senode)
                {

                    //using (var connection = new SqlConnection(strConnString))
                    //{
                    //    connection.Open();
                    //    var countCommand = new SqlCommand(
                    //        "select nodepath from ConceptMap_path where StartNode = '" + senodes.ssnode + "' and EndNode = '" + senodes.eenode + "'",
                    //        connection);
                    fullpathQuery = "select nodepath from ConceptMap_path where StartNode = '" + senodes.ssnode + "' and EndNode='" + senodes.eenode + "'";

                    SqlCommand fullpathcmd = new SqlCommand(fullpathQuery);

                    DataTable fullpathtable = GetDataTable(fullpathcmd);

                    if (fullpathtable.Rows.Count > 0)
                    {
                        foreach (DataRow row in fullpathtable.Rows) // Loop over the rows.
                        {
                            pathstr.Add(row["nodepath"].ToString());
                        }
                    }
                    //    pathstr = countCommand.ExecuteScalar().ToString();
                    //    connection.Close();
                    //}


                    //string[] nodepath = pathstr.Split(';');
                    //return pathstr;
                    //string linklabel = "";
                    //string linkpathstr = "";
                    //for (int i = 0; i < nodepath.Length-1; i++)
                    //{
                    //    if(i==0)
                    //        linkpathstr = nodelabel[nodepath[0]].ToString() +",";
                    //    using (var connection = new SqlConnection(strConnString))
                    //    {
                    //        connection.Open();
                    //        var countCommand = new SqlCommand(
                    //            "select LinkLabel from ConceptMap_Link where StartNode = '" + nodepath[i] + "' and EndNode = '" + nodepath[i + 1] + "'",
                    //            connection);

                    //        linklabel = countCommand.ExecuteScalar().ToString();
                    //        connection.Close();
                    //        linkpathstr += linklabel + ",";
                    //        linkpathstr += nodelabel[nodepath[i + 1]].ToString() + ",";
                    //    }
                    //}
                    //linkpathstr = linkpathstr.Remove(linkpathstr.Length - 1);
                    //linkpath.Add(linkpathstr);
                }
            }
            return pathstr;
        }
        public class startendnode
        {
            public string ssnode { get; set; }
            public string eenode { get; set; }
            public startendnode(string snode, string enode)
            {
                this.ssnode = snode;
                this.eenode = enode;
            }
        }
        //EditDistance演算法，用來計算學生答案跟標準答案的相似度，及需要幾個步驟方能將學生答案改為標準答案(無權重)
        public int[,] EditDistance(DataTable dtStandardAns, DataTable dtStudentAns)
        {
            int n = dtStandardAns.Rows.Count;
            int m = dtStudentAns.Rows.Count;
            //Response.Write("m="+m);
            //Response.Write("n=" + n);
            int[,] D = new int[n + 1, m + 1]; //用來存比較結果的陣列
            int[] W = new int[m];
            //沒有加上權重的方法(各個權重皆為1)
            //給定預設值D[0,0]=0 , D[1,0]=1...D[n,0]=n , D[0,1]=1...D[0,m]=m
            D[0, 0] = 0;
            for (int i = 1; i <= n; i++)
            {
                D[i, 0] = i;
            }
            for (int j = 1; j <= m; j++)
            {
                D[0, j] = j;
            }
            //判斷哪一個操作為最佳(最小值)
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    D[i, j] = Math.Min((Math.Min((D[i - 1, j] + 1), (D[i, j - 1] + 1))), D[i - 1, j - 1] + substituteCost(dtStandardAns.Rows[i - 1]["cAnnotationNum"].ToString(), dtStudentAns.Rows[j - 1]["cAnnotationNum"].ToString()));
                }
            }

            return D;
        }


        //得到利用Edit Distance演算法求得的最小步驟路徑(無權重)
        //public string[] getShortestPath(int[,] EditDistanceMatrix)
        //{
        //    string[] shortestPath = new string[EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5];
        //    int x = 0, y = 0, z = 1;

        //    shortestPath[0] = "0,0";
        //    while (x != EditDistanceMatrix.GetLength(0) - 1 || y != EditDistanceMatrix.GetLength(1) - 1)
        //    {
        //        if (x == EditDistanceMatrix.GetLength(0) - 1)
        //        {
        //            y = y + 1;
        //            shortestPath[z] = x.ToString() + "," + y.ToString();
        //            z = z + 1;
        //        }
        //        else if (y == EditDistanceMatrix.GetLength(1) - 1)
        //        {
        //            x = x + 1;
        //            shortestPath[z] = x.ToString() + "," + y.ToString();
        //            z = z + 1;
        //        }

        //        else
        //        {
        //            //上方的點為最小值
        //            if ((EditDistanceMatrix[x, y + 1] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1])))
        //            {
        //                //判斷下一步
        //                if (x == y)//目前點在對角線
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x > y)//目前點在對角線下方
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x < y)//目前點在對角線上方
        //                {
        //                    x = x + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }
        //            }

        //            //右方的值為最小
        //            else if ((EditDistanceMatrix[x + 1, y] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1])))
        //            {
        //                //判斷下一步
        //                if (x == y)//目前點在對角線
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x > y)//目前點在對角線下方
        //                {
        //                    y = y + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }

        //                else if (x < y)//目前點在對角線上方
        //                {
        //                    x = x + 1;
        //                    shortestPath[z] = x.ToString() + "," + y.ToString();
        //                    z = z + 1;
        //                }
        //            }

        //            //斜對角的值最小，表示不需要做動作
        //            else if (EditDistanceMatrix[x + 1, y + 1] == Math.Min((Math.Min(EditDistanceMatrix[x + 1, y], EditDistanceMatrix[x, y + 1])), EditDistanceMatrix[x + 1, y + 1]))
        //            {
        //                x = x + 1;
        //                y = y + 1;
        //                shortestPath[z] = x.ToString() + "," + y.ToString();
        //                z = z + 1;
        //            }
        //        }


        //    }

        //    /*for (int w = 0; w < EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5; w++)
        //    {
        //        Response.Write(shortestPath[w]);
        //    }*/

        //    return shortestPath;
        //}
        public string[] getShortestPath(int[,] EditDistanceMatrix)
        {
            string[] shortestPath = new string[EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5];
            string[] temp = new string[EditDistanceMatrix.GetLength(0) + EditDistanceMatrix.GetLength(1) + 5];

            int x = EditDistanceMatrix.GetLength(0) - 1, y = EditDistanceMatrix.GetLength(1) - 1, z = 1;

            temp[0] = (EditDistanceMatrix.GetLength(0) - 1).ToString() + "," + (EditDistanceMatrix.GetLength(1) - 1).ToString();
            shortestPath[0] = "0,0";

            while (x != 0 || y != 0)
            {
                if (x == 0)
                {
                    y = y - 1;
                    temp[z] = x.ToString() + "," + y.ToString();
                    z = z + 1;
                }
                else if (y == 0)
                {
                    x = x - 1;
                    temp[z] = x.ToString() + "," + y.ToString();
                    z = z + 1;
                }

                else
                {
                    //斜下方的值為最小且等於目前的值，將斜下方的座標存入
                    if (EditDistanceMatrix[x, y] == EditDistanceMatrix[x - 1, y - 1] && EditDistanceMatrix[x - 1, y - 1] < EditDistanceMatrix[x - 1, y] && EditDistanceMatrix[x - 1, y - 1] < EditDistanceMatrix[x, y - 1])
                    {
                        x = x - 1;
                        y = y - 1;
                        temp[z] = x.ToString() + "," + y.ToString();
                        z = z + 1;
                    }

                    //左方的值為最小
                    else if (EditDistanceMatrix[x - 1, y] == Math.Min(EditDistanceMatrix[x - 1, y], EditDistanceMatrix[x, y - 1]))
                    {
                        x = x - 1;
                        temp[z] = x.ToString() + "," + y.ToString();
                        z = z + 1;
                    }

                    //下方的值為最小
                    else if (EditDistanceMatrix[x, y - 1] == Math.Min(EditDistanceMatrix[x - 1, y], EditDistanceMatrix[x, y - 1]))
                    {
                        y = y - 1;
                        temp[z] = x.ToString() + "," + y.ToString();
                        z = z + 1;
                    }
                }
            }


            int t = temp.Length - 1;
            int s = 0;

            while (t > -1)
            {
                if (temp[t] != null)
                {
                    shortestPath[s] = temp[t];
                    s = s + 1;
                    t = t - 1;
                }
                else
                    t = t - 1;
            }



            return shortestPath;
        }

        //比較兩步驟是否一樣
        public int substituteCost(String a, String b)
        {
            if (a == b)
                return 0;
            else
                return 2;

        }
        //創建學生的作答步驟表格
        protected string CreatStudentAnswerTable(DataTable dtStandardStep, DataTable dtStudentStep)
        {

            //Table tbStudentStep = new Table();
            //tbStudentStep.ID = "tbStudentStep";
            //tbStudentStep.Attributes["Width"] = "95%";
            //tbStudentStep.Attributes["style"] = "border:solid 1px black;border-collaspse:collapse;font-size: large;";

            //TableRow trTittle = new TableRow();
            //trTittle.ID = "trStudentTittle";

            //TableCell tcStepTitle = new TableCell();
            //tcStepTitle.ID = "tcStudentStep";
            //tcStepTitle.Text = "Step Order";
            ////tcStepTitle.ColumnSpan = 2;
            //tcStepTitle.Attributes["Width"] = "50%";
            //tcStepTitle.Attributes["style"] = "border:solid 1px black;border-collaspse:collapse;border-style: groove; border-width: thin; background-color: #999999; color: #FFFFFF; font-weight: 900;font-size: large;";
            ////tcTittle.Font.Bold = true;
            //TableCell tcMissStep = new TableCell();
            //tcMissStep.ID = "tcMissStep";
            //tcMissStep.Text = "Missing Step";
            //tcMissStep.Attributes["Width"] = "50%";
            //tcMissStep.Attributes["style"] = "border:solid 1px black;border-collaspse:collapse;border-style: groove; border-width: thin; background-color: #999999; color: #FFFFFF; font-weight: 900;font-size: large;";

            //trTittle.Controls.Add(tcStepTitle);
            //trTittle.Controls.Add(tcMissStep);
            //tbStudentStep.Controls.Add(trTittle);

            string[] shortestPath = new string[dtStandardStep.Rows.Count + dtStudentStep.Rows.Count + 7];
            shortestPath = getShortestPath(EditDistance(dtStandardStep, dtStudentStep));
            int z = 0;//shortestPath陣列的指標




            //int n = dtStandardStep.Rows.Count - numOfOr;
            //string[,] standardDetailStep = new string[n, 2];//存放標準步驟細節步驟以及權重
            //int addrPoint = 0;//記錄現在在標準步驟的陣列放到第幾個

            string tmpAns = "";
            int mark = 0;
            int point = 0;
            string rootleaf = "";

            //開始建立學生步驟的表格內容
            for (int i = 0; i < dtStudentStep.Rows.Count + dtStandardStep.Rows.Count; i++)
            {

                rootleaf = "(" + dtStudentStep.Rows[0]["cAnnotationNum"].ToString() + "," + dtStudentStep.Rows[dtStudentStep.Rows.Count - 1]["cAnnotationNum"].ToString() + ")";

                if (shortestPath[z + 1] != null)
                {
                    //將坐標字串以','來切割，用來判斷刪除、插入等等動作
                    string[] tempNew = new string[2];
                    tempNew[0] = shortestPath[z + 1].Split(',')[0];
                    tempNew[1] = shortestPath[z + 1].Split(',')[1];

                    string[] tempOld = new string[2];
                    tempOld[0] = shortestPath[z].Split(',')[0];
                    tempOld[1] = shortestPath[z].Split(',')[1];

                    //該步驟需要刪除
                    if (Convert.ToInt32(tempNew[0]) - Convert.ToInt32(tempOld[0]) == 0 && Convert.ToInt32(tempNew[1]) - Convert.ToInt32(tempOld[1]) == 1)
                    {
                        //TableRow trData = new TableRow();
                        ////trData.ID = "trStudentData_" + dtStep.Rows[i]["strRecordID"].ToString();

                        //TableCell tcStep = new TableCell();
                        ////tcStep.ID = "tcStudentStep_" + dtStep.Rows[i]["strRecordID"].ToString();
                        //tcStep.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC;color: #0000FF; font-weight: 900;text-decoration: line-through;";
                        //tcStep.Attributes["Width"] = "50%";

                        Label lbStep = new Label();
                        lbStep.Text = dtStudentStep.Rows[i]["cAnnotationNum"].ToString();
                        tmpAns += "<span style=\"background-color:yellow;font-weight:bold;text-decoration: line-through;\"> " + lbStep.Text + " </span>";
                        //TableCell tcOperating = new TableCell();
                        //tcOperating.Attributes["Width"] = "50%";
                        //tcOperating.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";

                        //tcStep.Controls.Add(lbStep);
                        //trData.Controls.Add(tcStep);
                        //trData.Controls.Add(tcOperating);
                        //tbStudentStep.Controls.Add(trData);
                        z = z + 1;//指往下一個
                        //dtStudentStep.RemoveAt(point);
                    }

                    //插入缺少的步驟(在上面已經有將詳細步驟合併,所以這邊直接插入陣列的內容)
                    else if (Convert.ToInt32(tempNew[0]) - Convert.ToInt32(tempOld[0]) == 1 && Convert.ToInt32(tempNew[1]) - Convert.ToInt32(tempOld[1]) == 0)
                    {
                        //TableRow trData = new TableRow();

                        //TableCell tcStep = new TableCell();
                        //tcStep.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";
                        //tcStep.Attributes["Width"] = "50%";

                        //TableCell tcOperating = new TableCell();
                        //tcOperating.Attributes["Width"] = "50%";
                        //tcOperating.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#FF0000;font-weight: 900;";

                        Label lbStepOpterating = new Label();
                        lbStepOpterating.Text = dtStandardStep.Rows[Convert.ToInt32(tempNew[0]) - 1]["cAnnotationNum"].ToString();
                        tmpAns += "<span style=\"background-color:red;font-weight:bold;\"> " + lbStepOpterating.Text + " </span>";

                        //tcOperating.Controls.Add(lbStepOpterating);
                        //trData.Controls.Add(tcStep);
                        //trData.Controls.Add(tcOperating);
                        //tbStudentStep.Controls.Add(trData);
                        z = z + 1;
                        i = i - 1;
                        point = point + 1;//插入新的步驟後，指針往下加1
                    }

                    else if (Convert.ToInt32(tempNew[0]) - Convert.ToInt32(tempOld[0]) == 1 && Convert.ToInt32(tempNew[1]) - Convert.ToInt32(tempOld[1]) == 1)
                    {
                        //TableRow trData = new TableRow();
                        ////trData.ID = "trStudentData_" + dtStep.Rows[i]["strRecordID"].ToString();

                        //TableCell tcStep = new TableCell();
                        ////tcStep.ID = "tcStudentStep_" + dtStep.Rows[i]["strRecordID"].ToString();
                        //tcStep.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";
                        Label lbStep = new Label();
                        lbStep.Text = dtStudentStep.Rows[i]["cAnnotationNum"].ToString();
                        tmpAns += "<span style=\"background-color:green;color:white;font-weight:bold;\"> " + lbStep.Text + " </span>";
                        //TableCell tcOperating = new TableCell();
                        //tcOperating.Attributes["Width"] = "50%";
                        //tcOperating.Attributes["style"] = "border:solid 1px black;,border-collaspse:collapse;background-color:#FFFFCC; color:#000000;font-size: large;";

                        //tcStep.Controls.Add(lbStep);

                        //trData.Controls.Add(tcStep);
                        //trData.Controls.Add(tcOperating);
                        //tbStudentStep.Controls.Add(trData);
                        mark++;
                        z = z + 1;
                        point = point + 1;//插入新的步驟後，指針往下加1
                    }
                }
                else
                {
                }

            }
            //SortedList<int, string> st = new SortedList<int, string>();

            //st.Add(mark, tmpAns);

            //Label1.Text += tmpAns + "<br />";

            return mark + "@" + rootleaf + "|" + tmpAns;
            //if (point <= dtStudentStep.Rows.Count)
            //{

            //}





            //return tbStudentStep;

        }

        public override Task OnDisconnected()
        {
            List<string> userlist = new List<string>();
            List<string> connIDList = new List<string>();
            SqlConnection strcon = new SqlConnection(strConnString);
            strcon.Open();
            //"Delete from ConceptMap_tmpuserlist where connectionid='" + Context.ConnectionId + "';";
            string GroupStr = "Select groupid from ConceptMap_tmpuserlist where connectionid = '"+Context.ConnectionId+"'";
            SqlCommand GroupCmd = new SqlCommand(GroupStr, strcon);
            string groupID = GroupCmd.ExecuteScalar().ToString();

            string DeleteStr = "Delete from ConceptMap_tmpuserlist where connectionid='" + Context.ConnectionId + "'";
            SqlCommand DeleteCmd = new SqlCommand(DeleteStr, strcon);
            DeleteCmd.ExecuteNonQuery();

            //string ADDStr = "select id, title, description, url, image, video from ConceptMap_Keyword where id='" + Request.QueryString["id"] + "'";
            string QueryStr = "select username, connectionid, groupid from ConceptMap_tmpuserlist where groupid='" + groupID + "' order by username";
            SqlCommand Cmd = new SqlCommand(QueryStr, strcon);
            SqlDataReader QueryReader = null;

            QueryReader = Cmd.ExecuteReader();
            while (QueryReader.Read())
            {
                userlist.Add(QueryReader["username"].ToString());
                connIDList.Add(QueryReader["connectionid"].ToString());
            }


            //tmpuserlist.Add(new User(userName, connID));
            //foreach (var currentUserlist in tmpuserlist)
            //{
            //    userlist.Add(currentUserlist.Name);
            //    connIDList.Add(currentUserlist.ConnectionId);
            //}
            Clients.Group(groupID).userlistSet(string.Join(",", userlist.ToArray()), string.Join(",", connIDList.ToArray()));
            //ChatUser user = _users.Values.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            //if (user != null)
            //{
            //    ChatUser ignoredUser;
            //    _users.TryRemove(user.Name, out ignoredUser);

            //    // Leave all rooms
            //    HashSet<string> rooms;
            //    if (_userRooms.TryGetValue(user.Name, out rooms))
            //    {
            //        foreach (var room in rooms)
            //        {
            //            Clients.Group(room).leave(user);
            //            ChatRoom chatRoom = _rooms[room];
            //            chatRoom.Users.Remove(user.Name);
            //        }
            //    }

            //    //HashSet<string> ignoredRoom;
            //    //_userRooms.TryRemove(user.Name, out ignoredRoom);
            //}

            return null;
        }
        public bool Join()
        {
            // Check the user id cookie
            Cookie userIdCookie;

            if (!Context.RequestCookies.TryGetValue("userid", out userIdCookie))
            {
                return false;
            }

            User user = _users.Values.FirstOrDefault(u => u.Id == userIdCookie.Value);

            if (user != null)
            {
                // Update the users's client id mapping
                user.ConnectionId = Context.ConnectionId;
                
                // Set some client state
                Clients.Caller.id = user.Id;
                Clients.Caller.name = user.Name;
                

                // Leave all rooms
                //HashSet<string> rooms;
                //if (_userRooms.TryGetValue(user.Name, out rooms))
                //{
                //    foreach (var room in rooms)
                //    {
                //        Clients.Group(room).leave(user);
                //        ChatRoom chatRoom = _rooms[room];
                //        chatRoom.Users.Remove(user.Name);
                //    }
                //}

                //_userRooms[user.Name] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Add this user to the list of users
                Clients.Caller.addUser(user);
                return true;
            }

            return false;
        }
    }
    public class product
    {
        public string name;
        public string img1;
        public string descr;

    }
    [Serializable]
    public class User
    {
        public string ConnectionId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public User()
        {
        }

        public User(string name, string ConnectionId)
        {
            Name = name;
            Id = Guid.NewGuid().ToString("d");
        }
    }
    
    public abstract class Shape
    {
        public string ID { get; private set; }
        public Point Location { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public string Type { get { return GetType().Name.ToLower(); } }
        public User ChangedBy { get; set; }

        public Shape()
        {
            ID = Guid.NewGuid().ToString("d");
            Location = new Point(20, 20);
        }

        public static Shape Create(string type)
        {
            switch (type)
            {
                case "picture":
                    return new Picture();
                case "circle":
                    return new Circle();
                case "square":
                    return new Square();
                case "rectangle":
                default:
                    return new Rectangle();
            }
        }
    }

    public abstract class WidthHeightFixed : Shape
    {
        public override int Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
                base.Width = value;
            }
        }

        public override int Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                base.Height = value;
            }
        }
    }

    public class Rectangle : Shape
    {
        public Rectangle()
            : base()
        {
            Width = 160;
            Height = 100;
        }
    }

    public class Square : WidthHeightFixed
    {
        public Square()
            : base()
        {
            Width = 100;
        }
    }

    public class Circle : WidthHeightFixed
    {
        public int Radius
        {
            get { return base.Width / 2; }
            set
            {
                base.Height = value * 2;
                base.Width = value * 2;
            }
        }

        public Circle()
            : base()
        {
            Width = 100;
        }
    }

    public class Picture : Shape
    {
        public string Src { get; set; }

        public Picture()
            : base()
        {
            Src = "http://www.w3.org/html/logo/badge/html5-badge-h-css3-semantics.png";
            Width = 165;
            Height = 64;
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}