using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class UserSessionLogEventService
    {
        public static List<UserSessionLogEvent> UserSessionLogEvent { get; set; }
        private static UserSessionLogEvent _DefaultUserSessionLogEvent { get; set; }
        public static UserSessionLogEvent CurrentCompanyUserSessionLogEvent 
        {
            get
            {
                if (UserSessionLogEvent != null)
                {
                    var selected = UserSessionLogEvent.FirstOrDefault(u => u.CompanyId == SessionHelper.SelectedCompanyId);
                    if(selected==null)
                        selected = UserSessionLogEvent.FirstOrDefault(u => u.ClientId == SessionHelper.SelectedClientId);
                    if (selected == null)
                        selected = UserSessionLogEvent.FirstOrDefault(u => !u.ClientId.HasValue && !u.CompanyId.HasValue);
                    if (selected == null)
                        return GetDefault();
                    return selected;
                }
                else
                {
                    return GetDefault();
                }
            }
        }

        private static UserSessionLogEvent GetDefault()
        {
            if (_DefaultUserSessionLogEvent == null)
            {
                _DefaultUserSessionLogEvent = new UserSessionLogEvent()
                {
                    ClientId = null,
                    CompanyId = null,
                    LogAddSave = true,
                    LogAddOpen = true,
                    LogDeleteOpen = true,
                    LogDeleteSave = true,
                    LogEditOpen = true,
                    LogEditSave = true,
                    LogListing = true,
                    LogView = true
                };
            }
            return _DefaultUserSessionLogEvent;
        }
    }
}
