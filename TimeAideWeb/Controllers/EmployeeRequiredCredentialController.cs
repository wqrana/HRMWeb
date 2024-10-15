using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class EmployeeRequiredCredentialController : TimeAideWebControllers<EmployeeRequiredCredential>
    {

        public ActionResult AddEmployeeRequiredCredential(int id)
        {
            var model = new EmployeeRequiredCredentialViewModel();
            model.SelectedUserId = id;
            UserInformation userInformation = db.UserInformation.FirstOrDefault(u => u.Id == id);

            var employeeRequiredCredential = db.EmployeeRequiredCredential.Where(u => u.UserInformationId == id && u.DataEntryStatus == 1).ToList();
            List<SelectListItem> employeeRequiredCredentialItems = new List<SelectListItem>();
            foreach (var team in employeeRequiredCredential)
            {
                employeeRequiredCredentialItems.Add(new SelectListItem { Value = team.Credential.Id.ToString(), Text = team.Credential.CredentialName });
            }
            model.RequiredCredential = new MultiSelectList(employeeRequiredCredentialItems.OrderBy(i => i.Text), "Value", "Text");
            model.RequiredCredentialId = new List<string>();

            var users = db.Credential.Where(u => u.DataEntryStatus == 1).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var team in users)
            {
                var addedU = employeeRequiredCredential.FirstOrDefault(s => s.CredentialId == team.Id);
                if (addedU == null)
                    items.Add(new SelectListItem { Value = team.Id.ToString(), Text = team.CredentialName });
            }
            model.Credentials = new MultiSelectList(items.OrderBy(i => i.Text), "Value", "Text");
            model.CredentialId = new List<string>();

            return PartialView(model);
        }

        // POST: EmployeeSupervisor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddEmployeeRequiredCredential([Bind(Include = "SelectedUserId,RequiredCredential,Credentials,RequiredCredentialId,CredentialId")] EmployeeRequiredCredentialViewModel employeeRequiredCredentialViewModel)
        {
            if (ModelState.IsValid)
            {
                var employeeRequiredCredential = db.EmployeeRequiredCredential.Where(e => e.UserInformationId == employeeRequiredCredentialViewModel.SelectedUserId);

                foreach (var each in employeeRequiredCredential.ToList())
                {
                    EmployeeRequiredCredential eachCredential = db.EmployeeRequiredCredential.Find(each.Id);
                    db.EmployeeRequiredCredential.Remove(eachCredential);
                    //db.SaveChanges();
                }

                if (employeeRequiredCredentialViewModel.RequiredCredentialId != null)
                {
                    foreach (var each in employeeRequiredCredentialViewModel.RequiredCredentialId)
                    {
                        int userId = Convert.ToInt32(each);

                        var employee = employeeRequiredCredential.FirstOrDefault(s => s.UserInformationId == userId);
                        if (employee == null)
                        {
                            db.EmployeeRequiredCredential.Add(new EmployeeRequiredCredential()
                            {
                                UserInformationId = employeeRequiredCredentialViewModel.SelectedUserId,
                                CredentialId = Convert.ToInt32(each)
                            });

                        }
                        else
                        {
                            employee.DataEntryStatus = 1;
                        }
                    }
                }

                db.SaveChanges();
                return Json(employeeRequiredCredentialViewModel);
            }

            return Json(employeeRequiredCredentialViewModel);
        }
        // POST: EmployeeRequiredCredential/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeRequiredCredentialId,UserInformationId,CredentialId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeRequiredCredential employeeRequiredCredential)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeRequiredCredential.Add(employeeRequiredCredential);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employeeRequiredCredential);
        }

        // POST: EmployeeRequiredCredential/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeRequiredCredentialId,UserInformationId,CredentialId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeRequiredCredential employeeRequiredCredential)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeRequiredCredential).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employeeRequiredCredential);
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
