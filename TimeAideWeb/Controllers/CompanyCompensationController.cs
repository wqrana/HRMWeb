using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class CompanyCompensationController : TimeAideWebControllers<CompanyCompensation>
    {
        [HttpPost]
        public ActionResult Create(CompanyCompensation companyCompensation)
        {
            if (ModelState.IsValid)
            {
                db.CompanyCompensation.Add(companyCompensation);
                db.SaveChanges();
                //companyCompensation.SelectedTransactions;
                companyCompensation.CompanyCompensationPRPayExport.CompanyCompensationId = companyCompensation.Id;
                db.SaveChanges();
                CompanyCompensationService.UpdateCompensationTransaction(companyCompensation.Id, companyCompensation.SelectedTransactions);
                return Json(companyCompensation);
            }
            return GetErrors();
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyCompensation CompanyCompensation)
        {
            if (ModelState.IsValid)
            {
                CompanyCompensation.SetUpdated<CompanyCompensation>();
                db.Entry(CompanyCompensation).State = EntityState.Modified;
                if (CompanyCompensation.CompanyCompensationPRPayExport != null)
                {
                    CompanyCompensation.CompanyCompensationPRPayExport.CompanyCompensationId = CompanyCompensation.Id;
                    if (CompanyCompensation.CompanyCompensationPRPayExport.Id > 0)
                        db.Entry(CompanyCompensation.CompanyCompensationPRPayExport).State = EntityState.Modified;
                    else if (CompanyCompensation.CompanyCompensationPRPayExport.Id == 0)
                        db.CompanyCompensationPRPayExport.Add(CompanyCompensation.CompanyCompensationPRPayExport);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.CompanyCompensation.Include(u => u.)
            //                          .Include(u => u.MailingCountryUserContactInformation)
            //                          .Include(u => u.HomeCountryUserContactInformation)
            //             .FirstOrDefault(c => c.Id == id);
            //if (entity.States.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.MailingCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.HomeCountryUserContactInformation.Where(t => t.DataEntryStatus == 1).Count() > 0)
            //    return false;
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
