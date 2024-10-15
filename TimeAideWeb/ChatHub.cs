using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web
{
    public class ChatHub : Hub
    {
        public void Send(int? id, int? LoginId, string name, string message)
        {
            try
            {
                Clients.All.addNewMessageToPage(name, message);
            }
            catch (Exception ex)
            {
            }
        }
    }
}