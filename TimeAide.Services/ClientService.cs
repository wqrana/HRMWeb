using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class ClientService
    {
        public static List<Client> GetClients()
        {
            TimeAideContext db = new TimeAideContext();
            return db.GetAll<Client>(1).ToList();
        }
        public static string SelectedClientName
        {
            get
            {
                TimeAideContext db = new TimeAideContext();
                var client = db.Client.FirstOrDefault(c => c.Id == SessionHelper.SelectedClientId);
                return client.ClientName;
            }
        }
        public static string GetClientName(int clientId)
        {
            TimeAideContext db = new TimeAideContext();
            var client = db.Client.FirstOrDefault(c => c.Id == clientId);
            if (client != null)
                return client.ClientName;
            else
                return "";
        }

    }
}
