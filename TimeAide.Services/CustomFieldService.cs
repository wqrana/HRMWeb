using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class CustomFieldService
    {
        public static List<CustomField> GetCustomFields(bool isForEmployee)
        {
            TimeAideContext db = new TimeAideContext();
            if (isForEmployee)
               return db.CustomField.Include("CustomFieldType").Where(t => t.DataEntryStatus == 1 && t.ClientId == SessionHelper.SelectedClientId).ToList();
            //return db.GetAll<CustomField>(SessionHelper.SelectedClientId).Where(d => (!d.DocumentRequiredById.HasValue || d.DocumentRequiredById == 1 || d.DocumentRequiredById == 3)).ToList();
            else
                return db.GetAll<CustomField>(SessionHelper.SelectedClientId).Where(d => d.DocumentRequiredById == 2 || d.DocumentRequiredById == 3).ToList();
        }
    }
}
