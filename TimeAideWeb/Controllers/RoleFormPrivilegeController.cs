using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class RoleFormPrivilegeController : TimeAideWebControllers<RoleFormPrivilege>
    {
        //public ActionResult CreateMatrix_()
        //{
        //    List<RoleFormPrivilegeViewModel> roleTypeFormPrivileges = new List<RoleFormPrivilegeViewModel>();
        //    var forms = db.Form;
        //    var privilege = db.Privilege.Include(p => p.Company).Include(p => p.Client);
        //    foreach (var eachForm in forms)
        //    {
        //        foreach (var eachPrivilege in privilege)
        //        {
        //            roleTypeFormPrivileges.Add(new RoleFormPrivilegeViewModel { Form = eachForm, Privilege = eachPrivilege, IsAssigned = false });
        //        }
        //    }
        //    ViewBag.RoleId = new SelectList(db.Role.Where(u => u.DataEntryStatus == 1), "Id", "Name");
        //    return View(roleTypeFormPrivileges);
        //}

        public ActionResult CreateMatrix()
        {
            ViewBag.RoleId = new SelectList(db.Role.Where(u => u.DataEntryStatus == 1), "Id", "Name");
            var model = new RoleFormPrivilegeMatrixViewModel1();
            //model.ModuleId = 1;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMatrix([Bind(Include = "RoleId,ModuleId,FormName,RoleFormPrivileges")] RoleFormPrivilegeMatrixViewModel1 model)
        {
            model.Role = RoleService.GetRole(model.RoleId);
            if (ModelState.IsValid)
            {
                //if (model.Role.RoleTypeId == 4 && model.ModuleId != 1)
                //{
                //    model.RoleFormPrivileges = new List<RoleFormPrivilegeViewModel1>();
                //    return View(model);
                //}
                if (Request.Form["CreateMatrix"] == "Save All")
                {
                    GetMatrixSave(model);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.Clear();

                    (new RoleFormPrivilegeService()).GetMatrixFromDB(model);
                    model.Modules = ModuleService.GetModules();
                    
                }
                ViewBag.RoleId = new SelectList(db.Role.Where(u => u.DataEntryStatus == 1), "Id", "Name", model.RoleId);
                if (model.ModuleId.HasValue && model.ModuleId.Value > 0)
                    model.Module = db.Find<Module>(model.ModuleId ?? 0, 1);
                //model.RoleFormPrivileges = roleTypeFormPrivileges;
            }
            model.RoleFormPrivileges=model.RoleFormPrivileges.OrderBy(o => o.Form.Module.ModuleName).ThenBy(o => o.Form.FormName).ToList();
            return View(model);
        }

        private void GetMatrixSave(RoleFormPrivilegeMatrixViewModel1 model)
        {
            var roleFormPrivileges = db.RoleFormPrivilege.Where(p => p.RoleId == model.RoleId);
            foreach (var each in model.RoleFormPrivileges)
            {
                var itemInDbs = roleFormPrivileges.Where(p => p.FormId == each.FormId);
                if (itemInDbs == null || itemInDbs.Count() == 0)
                {
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 1, DataEntryStatus = each.AllowAddInt });
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 2, DataEntryStatus = each.AllowEditInt });
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 3, DataEntryStatus = each.AllowDeleteInt });
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 4, DataEntryStatus = each.AllowViewInt });
                    //db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = 1, ClientId = 1, PrivilegeId = 5, DataEntryStatus = each.IsRestrictedInt });
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 5, DataEntryStatus = each.AllowChangeHistoryInt });
                }
                else
                {

                    var itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 1);
                    if (itemInDb == null)
                        db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 1, DataEntryStatus = each.AllowAddInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowAddInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 2);
                    if (itemInDb == null)
                        db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 2, DataEntryStatus = each.AllowEditInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowEditInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 3);
                    if (itemInDb == null)
                        db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 3, DataEntryStatus = each.AllowDeleteInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowDeleteInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 4);
                    if (itemInDb == null)
                        db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 4, DataEntryStatus = each.AllowViewInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowViewInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 5);
                    if (itemInDb == null)
                        db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = each.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 5, DataEntryStatus = each.AllowChangeHistoryInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowChangeHistoryInt;

                }
            }
        }
        [HttpPost]
        public ActionResult SaveRoleFormPrivilege(RoleFormPrivilegeViewModel1 model)
        {
            try
            {
                var itemInDb = db.RoleFormPrivilege.FirstOrDefault(p => p.RoleId == model.RoleId && p.FormId == model.FormId && p.PrivilegeId == 1);
                if (itemInDb == null)
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = model.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 1, DataEntryStatus = model.AllowAddInt });
                else
                    itemInDb.DataEntryStatus = model.AllowAddInt;

                itemInDb = db.RoleFormPrivilege.FirstOrDefault(p => p.RoleId == model.RoleId && p.FormId == model.FormId && p.PrivilegeId == 2);
                if (itemInDb == null)
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = model.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 2, DataEntryStatus = model.AllowEditInt });
                else
                    itemInDb.DataEntryStatus = model.AllowEditInt;

                itemInDb = db.RoleFormPrivilege.FirstOrDefault(p => p.RoleId == model.RoleId && p.FormId == model.FormId && p.PrivilegeId == 3);
                if (itemInDb == null)
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = model.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 3, DataEntryStatus = model.AllowDeleteInt });
                else
                    itemInDb.DataEntryStatus = model.AllowDeleteInt;

                itemInDb = db.RoleFormPrivilege.FirstOrDefault(p => p.RoleId == model.RoleId && p.FormId == model.FormId && p.PrivilegeId == 4);
                if (itemInDb == null)
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = model.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 4, DataEntryStatus = model.AllowViewInt });
                else
                    itemInDb.DataEntryStatus = model.AllowViewInt;

                itemInDb = db.RoleFormPrivilege.FirstOrDefault(p => p.RoleId == model.RoleId && p.FormId == model.FormId && p.PrivilegeId == 5);
                if (itemInDb == null)
                    db.RoleFormPrivilege.Add(new RoleFormPrivilege() { FormId = model.FormId, RoleId = model.RoleId, CompanyId = SessionHelper.SelectedCompanyId, ClientId = SessionHelper.SelectedClientId, PrivilegeId = 5, DataEntryStatus = model.AllowChangeHistoryInt });
                else
                    itemInDb.DataEntryStatus = model.AllowChangeHistoryInt;
                db.SaveChanges();
                return Json("Success");
            }
            catch (Exception)
            {
                return Json("Failed");
            }
            
        }
        [HttpPost]
        public JsonResult GetAutocompleteFormNames(string Prefix)
        {
            IList<string> Name = (from N in (new TimeAideContext()).Form
                                  where (string.IsNullOrEmpty(Prefix) || N.FormName.ToLower().Contains(Prefix.ToLower())) && N.DataEntryStatus==1
                                  select N.FormName).ToList();
            return Json(Name);
        }
        //// GET: RoleFormPrivilege
        //public ActionResult Index()
        //{
        //    var roleFormPrivilege = db.RoleFormPrivilege.Include(r => r.Company).Include(r => r.Form).Include(r => r.Client).Include(r => r.Privilege).Include(r => r.Role);
        //    return View(roleFormPrivilege.ToList());
        //}

        //// GET: RoleFormPrivilege/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoleFormPrivilege roleFormPrivilege = db.RoleFormPrivilege.Find(id);
        //    if (roleFormPrivilege == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(roleFormPrivilege);
        //}

        // GET: RoleFormPrivilege/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyId = new SelectList(db.Company.Where(u => u.DataEntryStatus == 1), "Id", "CompanyName");
        //    ViewBag.FormId = new SelectList(db.Form.Where(u => u.DataEntryStatus == 1), "Id", "FormName");
        //    ViewBag.ClientId = new SelectList(db.Client.Where(u => u.DataEntryStatus == 1), "ClientId", "ClientName");
        //    ViewBag.PrivilegeId = new SelectList(db.Privilege.Where(u => u.DataEntryStatus == 1), "Id", "Name");
        //    ViewBag.RoleId = new SelectList(db.Role.Where(u => u.DataEntryStatus == 1), "Id", "Name");
        //    ViewBag.CreatedBy = new SelectList(db.UserInformation.Where(u => u.DataEntryStatus == 1), "UserId", "IdNumber");
        //    ViewBag.ModifiedBy = new SelectList(db.UserInformation.Where(u => u.DataEntryStatus == 1), "UserId", "IdNumber");
        //    return View();
        //}

        // POST: RoleFormPrivilege/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RolePrivilegeId,RoleId,PrivilegeId,FormId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] RoleFormPrivilege roleFormPrivilege)
        {
            if (ModelState.IsValid)
            {
                db.RoleFormPrivilege.Add(roleFormPrivilege);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Company, "Id", "CompanyName", roleFormPrivilege.CompanyId);
            ViewBag.FormId = new SelectList(db.Form, "Id", "FormName", roleFormPrivilege.FormId);
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "ClientName", roleFormPrivilege.ClientId);
            ViewBag.PrivilegeId = new SelectList(db.Privilege, "Id", "Name", roleFormPrivilege.PrivilegeId);
            ViewBag.RoleId = new SelectList(db.Role, "Id", "Name", roleFormPrivilege.RoleId);
            ViewBag.CreatedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.ModifiedBy);
            return View(roleFormPrivilege);
        }

        // GET: RoleFormPrivilege/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleFormPrivilege roleFormPrivilege = db.RoleFormPrivilege.Find(id);
            if (roleFormPrivilege == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Company, "Id", "CompanyName", roleFormPrivilege.CompanyId);
            ViewBag.FormId = new SelectList(db.Form, "Id", "FormName", roleFormPrivilege.FormId);
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "ClientName", roleFormPrivilege.ClientId);
            ViewBag.PrivilegeId = new SelectList(db.Privilege, "Id", "Name", roleFormPrivilege.PrivilegeId);
            ViewBag.RoleId = new SelectList(db.Role, "Id", "Name", roleFormPrivilege.RoleId);
            ViewBag.CreatedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.ModifiedBy);
            return View(roleFormPrivilege);
        }

        // POST: RoleFormPrivilege/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RolePrivilegeId,RoleId,PrivilegeId,FormId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] RoleFormPrivilege roleFormPrivilege)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleFormPrivilege).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Company, "Id", "CompanyName", roleFormPrivilege.CompanyId);
            ViewBag.FormId = new SelectList(db.Form, "Id", "FormName", roleFormPrivilege.FormId);
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "ClientName", roleFormPrivilege.ClientId);
            ViewBag.PrivilegeId = new SelectList(db.Privilege, "Id", "Name", roleFormPrivilege.PrivilegeId);
            ViewBag.RoleId = new SelectList(db.Role, "Id", "Name", roleFormPrivilege.RoleId);
            ViewBag.CreatedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.ModifiedBy);
            return View(roleFormPrivilege);
        }

        // GET: RoleFormPrivilege/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleFormPrivilege roleFormPrivilege = db.RoleFormPrivilege.Find(id);
            if (roleFormPrivilege == null)
            {
                return HttpNotFound();
            }
            return View(roleFormPrivilege);
        }

        // POST: RoleFormPrivilege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            RoleFormPrivilege roleFormPrivilege = db.RoleFormPrivilege.Find(id);
            db.RoleFormPrivilege.Remove(roleFormPrivilege);
            db.SaveChanges();
            return RedirectToAction("Index");
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
