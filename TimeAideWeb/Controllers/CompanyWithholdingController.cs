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
    public class CompanyWithholdingController : TimeAideWebControllers<CompanyWithholding>
    {
        [HttpPost]
        public ActionResult Create(CompanyWithholding companyWithholding)
        {
            if (!companyWithholding.IsLoan)
            {
                ModelState.Remove("CompanyWithholdingLoan.LoanDescription");
                ModelState.Remove("CompanyWithholdingLoan.LoanAmount");
                
                companyWithholding.CompanyWithholdingLoan = null;
            }
            if (ModelState.IsValid)
            {
                db.CompanyWithholding.Add(companyWithholding);
                db.SaveChanges();
                if (companyWithholding.CompanyWithholdingPRPayExport != null)
                    companyWithholding.CompanyWithholdingPRPayExport.CompanyWithholdingId = companyWithholding.Id;
                if (companyWithholding.CompanyWithholdingLoan != null)
                    companyWithholding.CompanyWithholdingLoan.CompanyWithholdingId = companyWithholding.Id;
                if (companyWithholding.CompanyWithholding401K != null)
                    companyWithholding.CompanyWithholding401K.CompanyWithholdingId = companyWithholding.Id;
                db.SaveChanges();
                CompanyWithholdingCompensationExclusionService.UpdateSelectedList(companyWithholding.Id, companyWithholding.SelectedCompensations);
                return Json(companyWithholding);
            }
            return GetErrors();
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyWithholding companyWithholding)
        {
            if (!companyWithholding.IsLoan)
            {
                ModelState.Remove("CompanyWithholdingLoan.LoanDescription");
                ModelState.Remove("CompanyWithholdingLoan.LoanAmount");
                companyWithholding.CompanyWithholdingLoan = null;
            }
            if (ModelState.IsValid)
            {
                companyWithholding.SetUpdated<CompanyWithholding>();
                db.Entry(companyWithholding).State = EntityState.Modified;
                if (companyWithholding.CompanyWithholdingPRPayExport != null)
                {
                    companyWithholding.CompanyWithholdingPRPayExport.CompanyWithholdingId = companyWithholding.Id;
                    if (companyWithholding.CompanyWithholdingPRPayExport.Id > 0)
                        db.Entry(companyWithholding.CompanyWithholdingPRPayExport).State = EntityState.Modified;
                    else if (companyWithholding.CompanyWithholdingPRPayExport.Id == 0)
                        db.CompanyWithholdingPRPayExport.Add(companyWithholding.CompanyWithholdingPRPayExport);
                }
                if (companyWithholding.CompanyWithholdingLoan != null)
                {
                    companyWithholding.CompanyWithholdingLoan.CompanyWithholdingId = companyWithholding.Id;
                    if (companyWithholding.CompanyWithholdingLoan.Id > 0)
                        db.Entry(companyWithholding.CompanyWithholdingLoan).State = EntityState.Modified;
                    else if (companyWithholding.CompanyWithholdingLoan.Id == 0)
                        db.CompanyWithholdingLoan.Add(companyWithholding.CompanyWithholdingLoan);
                }
                if (companyWithholding.CompanyWithholding401K != null)
                {
                    companyWithholding.CompanyWithholding401K.CompanyWithholdingId = companyWithholding.Id;
                    if (companyWithholding.CompanyWithholding401K.Id > 0)
                        db.Entry(companyWithholding.CompanyWithholding401K).State = EntityState.Modified;
                    else if (companyWithholding.CompanyWithholding401K.Id == 0)
                        db.CompanyWithholding401K.Add(companyWithholding.CompanyWithholding401K);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.CompanyWithholding.Include(u => u.)
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
