using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAide.Web.Models;

namespace TimeAide.Services
{
    public class CompanyContributionCompensationExclusionService
    {
        public static void UpdateSelectedList(int id, string selectedIds)
        {
            TimeAideContext db = new TimeAideContext();
            if (!string.IsNullOrEmpty(selectedIds))
            {
                var selectedList = selectedIds.Split(',').ToList();
                List<CompanyContributionCompensationExclusion> addList = new List<CompanyContributionCompensationExclusion>();
                List<CompanyContributionCompensationExclusion> removeList = new List<CompanyContributionCompensationExclusion>();
                var existingList = db.CompanyContributionCompensationExclusion.Where(w => w.CompanyContributionId == id).ToList();

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
                        addList.Add(new CompanyContributionCompensationExclusion() { CompanyContributionId = id, CompanyCompensationId = recordId });

                    }
                }

                db.CompanyContributionCompensationExclusion.RemoveRange(removeList);
                db.CompanyContributionCompensationExclusion.AddRange(addList);

                db.SaveChanges();
            }
        }
    }
}
