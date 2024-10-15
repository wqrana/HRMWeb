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
    public class DocumentService
    {
        public static List<Document> GetDocuments(bool isEmployeeDocuments)
        {
            TimeAideContext db = new TimeAideContext();
            if (isEmployeeDocuments)
                return db.GetAll<Document>(SessionHelper.SelectedClientId).Where(d => (!d.DocumentRequiredById.HasValue || d.DocumentRequiredById == 1 || d.DocumentRequiredById == 3)).ToList();
            else
                return db.GetAll<Document>(SessionHelper.SelectedClientId).Where(d => d.DocumentRequiredById == 2 || d.DocumentRequiredById == 3).ToList();
        }
    }
}
