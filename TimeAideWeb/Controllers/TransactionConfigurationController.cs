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
    public class TransactionConfigurationController : TimeAideWebControllers<TransactionConfiguration>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(TransactionConfiguration transactionConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.TransactionConfiguration.Add(transactionConfiguration);
                db.SaveChanges();
                return Json(transactionConfiguration);
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(TransactionConfiguration transactionConfiguration)
        {
            if (ModelState.IsValid)
            {
                transactionConfiguration.SetUpdated<TransactionConfiguration>();
                db.Entry(transactionConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EmployeeType.Include(u => u.UserInformations)
                                            .Include(u => u.EmploymentHistory)
                                            .Include(u => u.SupervisorEmployeeType)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.SupervisorEmployeeType.Where(t => t.DataEntryStatus == 1).Count() > 0)
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
