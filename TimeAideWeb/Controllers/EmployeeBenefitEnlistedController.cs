using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;
namespace TimeAide.Web.Controllers
{
    public class EmployeeBenefitEnlistedController : TimeAideWebControllers<EmployeeBenefitEnlisted>
    {
        

        public override ActionResult Index()
        {
            // ViewBag.InsuranceStatusId = new SelectList(db.InsuranceStatus.Where(w => w.DataEntryStatus == 1), "Id", "InsuranceStatusName");

            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
            IEnumerable<SelectListItem> EnlistedBenefitItemList = null;
             var EnlistedTempList = db.EmployeeBenefitEnlisted.Where(w => w.UserInformationId == id && w.DataEntryStatus==1).ToList();
            EnlistedBenefitItemList = db.Benefit.Where(w=>w.DataEntryStatus==1).
                                      Select(s=> new SelectListItem
                                      {
                                          Text = s.BenefitName,
                                          Value = s.Id.ToString(),
                                         
                                      });
            //ViewBag.InsuranceStatusId = new SelectList(db.InsuranceStatus.Where(w => w.DataEntryStatus == 1), "Id", "InsuranceStatusName", model.InsuranceStatusId);
            ViewBag.EnlistedBenefitItemList = EnlistedBenefitItemList;
            string[] TempData = EnlistedTempList.Select(s => s.BenefitId.ToString()).ToArray<string>();
            ViewBag.SelectedBenefitItemList = TempData;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string enlistedBenefitids)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
           try
            {
                var selectedEnlistedBenefitList = enlistedBenefitids.Split(',').ToList();
                List<EmployeeBenefitEnlisted> enlistedBenefitAddList = new List<EmployeeBenefitEnlisted>();
                List<EmployeeBenefitEnlisted> enlistedBenefitRemoveList = new List<EmployeeBenefitEnlisted>();
                var existingEnlistedBenefitList = db.EmployeeBenefitEnlisted.Where(w => w.UserInformationId == id).ToList();
                foreach (var enlistedBenefitItem in existingEnlistedBenefitList)
                {
                    var RecCnt = selectedEnlistedBenefitList.Where(w => w == enlistedBenefitItem.BenefitId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        enlistedBenefitRemoveList.Add(enlistedBenefitItem);
                    }

                }
                foreach (var selectedEnlistedId in selectedEnlistedBenefitList)
                {
                    if (selectedEnlistedId == "") continue;
                    int benefitId = int.Parse(selectedEnlistedId);
                    var recExists = existingEnlistedBenefitList.Where(w => w.BenefitId == benefitId).Count();
                    if (recExists == 0)
                    {
                        enlistedBenefitAddList.Add(new EmployeeBenefitEnlisted() { UserInformationId = id, BenefitId = benefitId, CreatedBy = 1, DataEntryStatus = 1, CreatedDate = DateTime.Now });

                    }
                }

                db.EmployeeBenefitEnlisted.RemoveRange(enlistedBenefitRemoveList);
                db.EmployeeBenefitEnlisted.AddRange(enlistedBenefitAddList);                                           
                
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

    }
}