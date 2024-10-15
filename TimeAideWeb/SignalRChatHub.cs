using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using System.Threading.Tasks;
using System.Text;

namespace SignalRChat
{
    [HubName("signalRChatHub")]
    [Authorize]
    public class SignalRChatHub : Hub
    {
        public void BroadCastMessageAllGroups(String msgFrom, String msg, String GroupName)
        {
            var id = Context.ConnectionId;
            string[] Exceptional = new string[0];
            string name = Context.User.Identity.Name;
            msgFrom = SignalRService.GetUserInformationName(name);
            string initials = "";
            if (string.IsNullOrWhiteSpace(msgFrom))
            {
                initials = string.Empty;
            }
            else
            {
                var splitted = msgFrom?.Split(' ');
                initials = $"{splitted[0][0]}{(splitted.Length > 1 ? splitted[splitted.Length - 1][0] : (char?)null)}";
            }
            Clients.Group(GroupName, Exceptional).receiveMessage(msgFrom, msg);
            //Clients.All.receiveMessage(msgFrom, msg, "");
            /*string[] Exceptional = new string[1];
            Exceptional[0] = id;       
            Clients.AllExcept(Exceptional).receiveMessage(msgFrom, msg);*/
        }

        public void BroadCastMessage(String msgFrom, String msg, string chatConversationId)
        {
            string name = Context.User.Identity.Name;
            msgFrom = SignalRService.GetUserInformationName(name);

            var id = Context.ConnectionId;
            Clients.All.receiveMessage(msgFrom, msg, "", chatConversationId);
            /*string[] Exceptional = new string[1];
            Exceptional[0] = id;       
            Clients.AllExcept(Exceptional).receiveMessage(msgFrom, msg);*/
        }

        [HubMethodName("hubconnect")]
        public void Get_Connect(String username, String userid, String connectionid, String chatConversationId, string clientId, string companyId)
        {
            string count = "";
            string msg = "";
            string list = "";
            try
            {
                string name = Context.User.Identity.Name;
                username = SignalRService.GetUserInformationName(name);
                count = SignalRService.GetCount().ToString();
                msg = updaterec(username, userid, connectionid, chatConversationId, clientId, companyId);
                list = SignalRService.GetUsers(username);
            }
            catch (Exception d)
            {
                msg = "DB Error " + d.Message;
            }
            var id = Context.ConnectionId;

            string[] Exceptional = new string[1];
            Exceptional[0] = id;
            Clients.Caller.receiveMessage("RU", msg, list);
            Clients.AllExcept(Exceptional).receiveMessage("NewConnection", username, count);
        }

        [HubMethodName("groupconnect")]
        public void Get_Connect(String username, String userid, String connectionid, String GroupName, String chatConversationId, string clientId, string companyId)
        {
            string count = "NA";
            string msg = "Welcome to group " + GroupName;
            string list = "";

            var id = Context.ConnectionId;
            Groups.Add(id, GroupName);

            string name = Context.User.Identity.Name;
            username = SignalRService.GetUserInformationName(name);
            count = SignalRService.GetCount().ToString();
            msg = updaterec(username, userid, connectionid, chatConversationId, clientId, companyId);


            string[] Exceptional = new string[1];
            Exceptional[0] = id;

            Clients.Caller.receiveMessage("Group Chat Hub", msg, list);
            //Clients.OthersInGroup(GroupName).receiveMessage("NewConnection", GroupName + " " + username + " " + id, count);
            //Clients.AllExcept(Exceptional).receiveMessage("NewConnection", username + " " + id, count);
        }

        //[HubMethodName("privatemessage")]
        public void Send_PrivateMessage(String msgFrom, String msg, String touserid, string chatConversationId)
        {
            try
            {
                string name = Context.User.Identity.Name;
                msgFrom = SignalRService.GetUserInformationName(name);
                var toLoginEmail = SignalRService.GetUserByUserInformationId(touserid);
                var toConnectionId = SignalRService.GetUserConnectionId(toLoginEmail);
                var id = Context.ConnectionId;
                Clients.Caller.receiveMessage(msgFrom, msg, toConnectionId);
                Clients.Client(toConnectionId).receiveMessage(msgFrom, msg, id, chatConversationId);
            }
            catch (Exception ex)
            {
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            //string username = Context.QueryString["username"].ToString();
            string clientId = Context.ConnectionId;
            string data = clientId;
            string count = "";
            try
            {
                count = SignalRService.GetCount().ToString();
            }
            catch (Exception d)
            {
                count = d.Message;
            }
            Clients.Caller.receiveMessage("ChatHub", data, count);
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            string username = SignalRService.GetUserInformationName(name);
            string count = "";
            string msg = "";

            string clientId = Context.ConnectionId;
            SignalRService.DeleteRecord(clientId);

            try
            {
                count = SignalRService.GetCount().ToString();
            }
            catch (Exception d)
            {
                msg = "DB Error " + d.Message;
            }
            string[] Exceptional = new string[1];
            Exceptional[0] = clientId;
            Clients.AllExcept(Exceptional).receiveMessage("NewConnection", username + " is offline", count);

            return base.OnDisconnected(stopCalled);
        }

        public string updaterec(string username, string userid, string connectionid, String chatConversationId, string clientId, string companyId)
        {
            try
            {
                string name = Context.User.Identity.Name;
                SignalRService.UpdateChateMessage(username, userid, connectionid, chatConversationId, clientId, companyId, name);
                return "saved";
            }
            catch (Exception d)
            {
                return d.Message;
            }
        }

        public void Create_Group(string GroupName)
        {

        }

        private string GetClientId()
        {
            string clientId = "";
            if (Context.QueryString["clientId"] != null)
            {
                // clientId passed from application 
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.ConnectionId;
            }

            return clientId;
        }
    }

    public class User
    {
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public int ClientId { get; set; }
        public Dictionary<string, string> ConnectionIds { get; set; }
        public DateTime LastActivityTime { get; set; }
    }
    //[Authorize]
    public class AuthHub : Hub
    {
        public AuthHub()
        {
        }
        private static readonly ConcurrentDictionary<string, User> ActiveUsers = new ConcurrentDictionary<string, User>(StringComparer.InvariantCultureIgnoreCase);
        public IEnumerable<string> GetConnectedUsers(int companyId)
        {

            return ActiveUsers.Where(x =>
            {

                lock (x.Value.ConnectionIds)
                {

                    return x.Value.CompanyId == companyId && !x.Value.ConnectionIds.ContainsKey(Context.ConnectionId.ToLower());
                }

            }).Select(x => x.Key);
        }
        public override Task OnConnected()
        {
            if (Context.User != null)
            {
                string userName = Context.User.Identity.Name;
                string webBrowserName = System.Web.HttpContext.Current.Request.Browser.Browser;
                int companyId = 0;
                int clientId = 0;
                var userInformation = SignalRService.GetUserInformation(userName);
                if (userInformation != null && userInformation.Rows.Count > 0)
                {
                    companyId = Convert.ToInt32(userInformation.Rows[0]["CompanyId"]);
                    clientId = Convert.ToInt32(userInformation.Rows[0]["ClientId"]);
                }

                //SignalRService.GetApplicationConfiguration("SessionTimeOut", clientId, companyId);

                string connectionId = Context.ConnectionId;

                var user = ActiveUsers.GetOrAdd(userName, _ => new User
                {
                    Name = userName,
                    ConnectionIds = new Dictionary<string, string>(),
                    LastActivityTime = DateTime.Now,
                    CompanyId = companyId,
                    ClientId = clientId
                });

                lock (user.ConnectionIds)
                {

                    user.ConnectionIds.Add(connectionId.ToLower(), webBrowserName.ToLower());

                }
            }
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {

            if (Context.User != null)
            {
                string userName = Context.User.Identity.Name;
                string connectionId = Context.ConnectionId;

                User user;
                ActiveUsers.TryGetValue(userName, out user);

                if (user != null)
                {

                    lock (user.ConnectionIds)
                    {
                        var conn = user.ConnectionIds[connectionId.ToLower()];
                        user.ConnectionIds.Remove(conn);

                        if (!user.ConnectionIds.Any())
                        {

                            User removedUser;
                            ActiveUsers.TryRemove(userName, out removedUser);

                        }
                    }
                }
            }
            return base.OnDisconnected(stopCalled);
        }
        private User GetUser(string username)
        {

            User user;
            ActiveUsers.TryGetValue(username, out user);

            return user;
        }
        public void forceLogOut(string loginUser,string browserName, string eventName)
        {

            User receiver;
            if (ActiveUsers.TryGetValue(loginUser, out receiver))
            {
                User sender = GetUser(loginUser);

                List<KeyValuePair<string, string>> allReceivers;
                lock (receiver.ConnectionIds)
                {
                    allReceivers = receiver.ConnectionIds.Where(c => string.IsNullOrEmpty(browserName) || c.Value == browserName.ToLower()).ToList();
                }

                foreach (var cid in allReceivers)
                {
                    //receiver.ConnectionIds.Remove(cid);
                    Clients.Client(cid.Key).Signout();
                }
            }
        }
        public void forceRedirect(string loginUser, string browserName)
        {

            User receiver;
            if (ActiveUsers.TryGetValue(loginUser, out receiver))
            {
                //string userString = loginUser;
                //if (Context != null && Context.User != null && Context.User.Identity != null)
                //{
                //    userString = Context.User.Identity.Name;
                //                 Context.User.Identity.Name
                //}

                User sender = GetUser(loginUser);

                List<KeyValuePair<string, string>> allReceivers;
                lock (receiver.ConnectionIds)
                {
                    allReceivers = receiver.ConnectionIds.Where(c => string.IsNullOrEmpty(browserName) || c.Value != browserName.ToLower()).ToList();
                }

                foreach (var cid in allReceivers)
                {
                    //receiver.ConnectionIds.Remove(cid);
                    Clients.Client(cid.Key).Redirect(loginUser);
                }
            }
        }
        public void forceRedirect1(string loginUser, string browserName)
        {

            User receiver;
            if (ActiveUsers.TryGetValue(loginUser, out receiver))
            {
                //string userString = loginUser;
                //if (Context != null && Context.User != null && Context.User.Identity != null)
                //{
                //    userString = Context.User.Identity.Name;
                //                 Context.User.Identity.Name
                //}

                User sender = GetUser(loginUser);

                List<KeyValuePair<string, string>> allReceivers;
                lock (receiver.ConnectionIds)
                {
                    allReceivers = receiver.ConnectionIds.Where(c => string.IsNullOrEmpty(browserName) || c.Value == browserName.ToLower()).ToList();
                }

                foreach (var cid in allReceivers)
                {
                    //receiver.ConnectionIds.Remove(cid);
                    Clients.Client(cid.Key).Redirect(loginUser);
                }
            }
        }
        public void updateUserActivity(bool isActive, string browserName)
        {
            try
            {
                if (Context.User != null)
                {
                    User sender = GetUser(Context.User.Identity.Name);
                    if (sender != null)
                    {
                        if (isActive)
                        {
                            sender.LastActivityTime = DateTime.Now;
                            SessionUpdate(sender.Name + "#" + isActive.ToString());
                        }
                        else
                        {
                            Double minutes = ((TimeSpan)(DateTime.Now - sender.LastActivityTime)).TotalMinutes;
                            if (minutes >= SignalRService.CompanySessionTimeout[sender.CompanyId])
                            {
                                forceLogOut(Context.User.Identity.Name, browserName, "Timeout");
                                SessionUpdate(sender.Name + "#" + "Logout");
                            }
                            else
                                SessionUpdate(sender.Name + "#" + isActive.ToString());
                        }
                    }
                    else
                        SessionUpdate("No User" + "#" + isActive.ToString());

                }
                else
                    SessionUpdate("No User" + "#" + isActive.ToString());
            }
            catch (Exception ex)
            {
            }
        }
        public void SessionUpdate(string detail)
        {
            var id = Context.ConnectionId;

            //string name = Context.User.Identity.Name;
            //string username = SignalRService.GetUserInformationName(name);
            StringBuilder details = new StringBuilder();
            details.Append(detail + "#");
            //details.Append(username + "#");
            //details.Append(name + "#");
            SignalRService.UpdateSessionLog(details.ToString());

        }
    }
}