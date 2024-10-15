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
    public class PositionTrainingController : TimeAideWebControllers<PositionTraining>
    {
        public ActionResult IndexByPosition(int? id)
        {
            var model = db.PositionTraining.Where(w => w.PositionId == id);
            return PartialView("Index", model);
        }
        public ActionResult CreateEdit(int? id)
        {
            IEnumerable<SelectListItem> trainingItemList = null;
            var positionTrainingList = db.PositionTraining.Where(w => w.PositionId == id && w.DataEntryStatus == 1)
                                              .ToList();
            trainingItemList = db.GetAll<Training>(SessionHelper.SelectedClientId).
                                      Select(s => new SelectListItem
                                      {
                                          Text = s.TrainingName,
                                          Value = s.Id.ToString()

                                      });

            ViewBag.TrainingItemList = trainingItemList;
            string[] TempData = positionTrainingList.Select(s => s.TrainingId.ToString()).ToArray<string>();
            ViewBag.SelectedPositionTrainings = TempData;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedTrainingIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedTrainingsList = selectedTrainingIds.Split(',').ToList();
                List<PositionTraining> trainingAddList = new List<PositionTraining>();
                List<PositionTraining> trainingRemoveList = new List<PositionTraining>();
                var existingTrainingList = db.PositionTraining.Where(w => w.PositionId == id).ToList();

                foreach (var trainingItem in existingTrainingList)
                {
                    var RecCnt = selectedTrainingsList.Where(w => w == trainingItem.TrainingId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        trainingRemoveList.Add(trainingItem);
                    }

                }
                foreach (var selectedTrainingId in selectedTrainingsList)
                {
                    if (selectedTrainingId == "") continue;
                    int trainingId = int.Parse(selectedTrainingId);
                    var recExists = existingTrainingList.Where(w => w.TrainingId == trainingId).Count();
                    if (recExists == 0)
                    {
                        trainingAddList.Add(new PositionTraining() { PositionId = id, TrainingId = trainingId });

                    }
                }

                db.PositionTraining.RemoveRange(trainingRemoveList);
                db.PositionTraining.AddRange(trainingAddList);

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