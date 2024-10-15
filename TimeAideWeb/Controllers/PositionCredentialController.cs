
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class PositionCredentialController : TimeAideWebControllers<PositionCredential>
    {
        public ActionResult IndexByPosition(int? id)
        {
            var model = db.PositionCredential.Where(w => w.PositionId == id).OrderBy(o=>o.IsRequired);
            return PartialView("Index", model);
        }
        public ActionResult CreateEdit(int? id)
        {
            IEnumerable<SelectListItem> credentialItemList = null;
            var positionCredentialList = db.PositionCredential.Where(w => w.PositionId == id && w.DataEntryStatus == 1)
                                              .ToList();
            credentialItemList = db.GetAll<Credential>(SessionHelper.SelectedClientId).
                                      Select(s => new SelectListItem
                                      {
                                          Text = s.CredentialName,
                                          Value = s.Id.ToString()

                                      });

            ViewBag.CredentialItemList = credentialItemList;
            string[] TempData = positionCredentialList.Select(s => s.CredentialId.ToString()).ToArray<string>();
            ViewBag.SelectedPositionCredentials = TempData;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedCredentialIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedCredentialIds.Split(',').ToList();
                List<PositionCredential> credentialAddList = new List<PositionCredential>();
                List<PositionCredential> credentialRemoveList = new List<PositionCredential>();
                var existingCredentialList = db.PositionCredential.Where(w => w.PositionId == id).ToList();

                foreach (var credentialItem in existingCredentialList)
                {
                    var RecCnt = selectedCredentialsList.Where(w => w == credentialItem.CredentialId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        credentialRemoveList.Add(credentialItem);
                    }

                }
                foreach (var selectedCredentialId in selectedCredentialsList)
                {
                    if (selectedCredentialId == "") continue;
                    int credentialId = int.Parse(selectedCredentialId);
                    var recExists = existingCredentialList.Where(w => w.CredentialId == credentialId).Count();
                    if (recExists == 0)
                    {
                        credentialAddList.Add(new PositionCredential() { PositionId = id, CredentialId = credentialId });

                    }
                }

                db.PositionCredential.RemoveRange(credentialRemoveList);
                db.PositionCredential.AddRange(credentialAddList);

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
        public ActionResult EditRequiredCredential(int? id)
        {
            var model = db.PositionCredential.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult EditRequiredCredential(PositionCredential model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var positionCredentialEntity = db.PositionCredential.Find(model.Id);
                if (positionCredentialEntity != null)
                {
                    positionCredentialEntity.IsRequired = model.IsRequired;
                    db.SaveChanges();
                }

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
