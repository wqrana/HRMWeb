using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class CompanyWithholdingCompensationExclusionService
    {
        public static void UpdateSelectedList(int id, string selectedIds)
        {
            TimeAideContext db = new TimeAideContext();
            if (!string.IsNullOrEmpty(selectedIds))
            {
                var selectedList = selectedIds.Split(',').ToList();
                List<CompanyWithholdingCompensationExclusion> addList = new List<CompanyWithholdingCompensationExclusion>();
                List<CompanyWithholdingCompensationExclusion> removeList = new List<CompanyWithholdingCompensationExclusion>();
                var existingList = db.CompanyWithholdingCompensationExclusion.Where(w => w.CompanyWithholdingId == id).ToList();

                foreach (var each in existingList)
                {
                    var RecCnt = selectedList.Where(w => w == each.CompanyCompensationId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        removeList.Add(each);
                    }

                }
                foreach (var eachSelectedId in selectedList)
                {
                    if (eachSelectedId == "") continue;
                    int recordId = int.Parse(eachSelectedId);
                    var recExists = existingList.Where(w => w.CompanyCompensationId == recordId).Count();
                    if (recExists == 0)
                    {
                        addList.Add(new CompanyWithholdingCompensationExclusion() { CompanyWithholdingId = id, CompanyCompensationId = recordId });

                    }
                }

                db.CompanyWithholdingCompensationExclusion.RemoveRange(removeList);
                db.CompanyWithholdingCompensationExclusion.AddRange(addList);

                db.SaveChanges();
            }
        }
    }
}
