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
    public class CompanyContributionController : TimeAideWebControllers<CompanyContribution>
    {
        [HttpPost]
        public ActionResult Create(CompanyContribution companyContribution)
        {
            if (ModelState.IsValid)
            {
                db.CompanyContribution.Add(companyContribution);
                db.SaveChanges();
                //companyContribution.SelectedTransactions;
                companyContribution.CompanyContributionPRPayExport.CompanyContributionId = companyContribution.Id;
                //companyContribution.CompanyContributionLoan.CompanyContributionId = companyContribution.Id;
                if (companyContribution.CompanyContribution401K != null)
                    companyContribution.CompanyContribution401K.CompanyContributionId = companyContribution.Id;
                db.SaveChanges();
                CompanyContributionCompensationExclusionService.UpdateSelectedList(companyContribution.Id, companyContribution.SelectedCompensations);
                return Json(companyContribution);
            }
            return GetErrors();
        }

        // POST: Country/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyContribution CompanyContribution)
        {
            if (ModelState.IsValid)
            {
                CompanyContribution.SetUpdated<CompanyContribution>();
                db.Entry(CompanyContribution).State = EntityState.Modified;
                if (CompanyContribution.CompanyContributionPRPayExport != null)
                {
                    CompanyContribution.CompanyContributionPRPayExport.CompanyContributionId = CompanyContribution.Id;
                    if (CompanyContribution.CompanyContributionPRPayExport.Id > 0)
                        db.Entry(CompanyContribution.CompanyContributionPRPayExport).State = EntityState.Modified;
                    else if (CompanyContribution.CompanyContributionPRPayExport.Id == 0)
                        db.CompanyContributionPRPayExport.Add(CompanyContribution.CompanyContributionPRPayExport);
                }
                
                if (CompanyContribution.CompanyContribution401K != null)
                {
                    CompanyContribution.CompanyContribution401K.CompanyContributionId = CompanyContribution.Id;
                    if (CompanyContribution.CompanyContribution401K.Id > 0)
                        db.Entry(CompanyContribution.CompanyContribution401K).State = EntityState.Modified;
                    else if (CompanyContribution.CompanyContribution401K.Id == 0)
                        db.CompanyContribution401K.Add(CompanyContribution.CompanyContribution401K);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.CompanyContribution.Include(u => u.)
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
