using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class PayScaleLevelController : TimeAideWebControllers<PayScaleLevel>
    {

        public ActionResult IndexByPayScale(int? id)
        {
            List<PayScaleLevel> model=null;
            var payscale = db.PayScale.FirstOrDefault(w => w.Id == (id ?? 0));
            ViewBag.PayScaleId = id??0;
            if (id.HasValue)
            {
                ViewBag.PayScaleName = payscale.PayScaleName;
                model = payscale.PayScaleLevel.OrderBy(o => o.PayScaleLevelRate).ToList();
            }
            return PartialView("Index", model);
        }


        public virtual ActionResult CreateByPayScale(int? id)
        {
            try
            {
                AllowAdd();
                //AddDropDowns();
                ViewBag.Label = ViewBag.Label + " - Add";
                var model = new PayScaleLevel();
                model.PayScaleId = id ?? 0;
                return PartialView("Create", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        // GET: PayScaleLevel/Details/5

        // POST: PayScaleLevel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "PayScaleId,PayScaleLevelRate")] PayScaleLevel payScaleLevel)
        {
            ViewBag.PayScaleId = payScaleLevel.PayScaleId;
            if (ModelState.IsValid)
            {
                db.PayScaleLevel.Add(payScaleLevel);
                db.SaveChanges();
                return Json(payScaleLevel);
            }

            return GetErrors();
        }

        // POST: PayScaleLevel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,PayScaleId,PayScaleLevelRate,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] PayScaleLevel payScaleLevel)
        {
            ViewBag.PayScaleId = payScaleLevel.PayScaleId;
            if (ModelState.IsValid)
            {
                db.Entry(payScaleLevel).State = EntityState.Modified;
                db.SaveChanges();
                return Json(payScaleLevel);
            }

            return GetErrors();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
