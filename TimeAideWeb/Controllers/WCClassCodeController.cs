using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class WCClassCodeController : TimeAideWebControllers<WCClassCode>
    {

        // POST: WCClassCode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WCClassCodeId,ClassName,ClassCode,ClassDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] WCClassCode wCClassCode)
        {
            if (ModelState.IsValid)
            {
                db.WCClassCode.Add(wCClassCode);
                db.SaveChanges();
                return Json(wCClassCode);
            }
            return GetErrors();
        }


        // POST: WCClassCode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassName,ClassCode,ClassDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] WCClassCode wCClassCode)
        {
            if (ModelState.IsValid)
            {
                wCClassCode.SetUpdated<WCClassCode>();
                db.Entry(wCClassCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            var wCClassCode = db.WCClassCode.Include(u => u.PayInformationHistory)                     
                                .FirstOrDefault(c => c.Id == id);
            if (wCClassCode.PayInformationHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            
            return true;
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
