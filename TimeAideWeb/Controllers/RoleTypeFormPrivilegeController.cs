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
    public class RoleTypeFormPrivilegeController : TimeAideWebBaseControllers
    {
        private TimeAideContext db = new TimeAideContext();

        public ActionResult CreateMatrix()
        {
            List<RoleTypeFormPrivilegeViewModel> rol_eTypeFormPrivileges = new List<RoleTypeFormPrivilegeViewModel>();
            var forms = db.Form;
            var privilege = db.Privilege.Include(p => p.Company).Include(p => p.Client);
            foreach (var eachForm in forms)
            {
                foreach (var eachPrivilege in privilege)
                {
                    rol_eTypeFormPrivileges.Add(new RoleTypeFormPrivilegeViewModel { Form = eachForm, Privilege = eachPrivilege, IsAssigned = false });
                }
            }
            ViewBag.RoleTypeID = new SelectList(db.RoleType.Where(u => u.DataEntryStatus == 1), "Id", "RoleTypeName");
            return View(rol_eTypeFormPrivileges);
        }

        public ActionResult CreateMatrix2()
        {
            ViewBag.RoleTypeID = new SelectList(db.RoleType.Where(u => u.DataEntryStatus == 1), "Id", "RoleTypeName");
            var model = new RoleTypeFormPrivilegeMatrixViewModel1();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMatrix2([Bind(Include = "RoleTypeID,RoleTypeFormPrivileges")]RoleTypeFormPrivilegeMatrixViewModel1 model)
        {
            if (Request.Form["CreateMatrix2"] == "Create Matrix")
                GetMatrixSave(model);
            else
                GetMatrixFromDB(model);
            db.SaveChanges();
            ViewBag.RoleTypeID = new SelectList(db.RoleType.Where(u => u.DataEntryStatus == 1), "Id", "RoleTypeName", model.RoleTypeID);
            //model.RoleTypeFormPrivileges = rol_eTypeFormPrivileges;
            return View(model);
        }

        private void GetMatrixFromDB(RoleTypeFormPrivilegeMatrixViewModel1 model)
        {
            var tempRoleTypeId = model.RoleTypeID;
            if (Request.Form["LoadData"] == "Load")
                ModelState.Clear();
            model.RoleTypeID = tempRoleTypeId;
            model.RoleTypeFormPrivileges = new List<RoleTypeFormPrivilegeViewModel1>();
            var roleFormPrivileges = db.RoleTypeFormPrivilege.Where(p => p.RoleTypeId == model.RoleTypeID);
            var forms = db.Form;
            foreach (var eachForm in forms)
            {
                var itemInDbs = roleFormPrivileges.Where(p => p.FormId == eachForm.Id && p.RoleTypeId == model.RoleTypeID);
                if (itemInDbs == null || itemInDbs.Count() == 0)
                {
                    model.RoleTypeFormPrivileges.Add(new RoleTypeFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm.Id, RoleTypeID = model.RoleTypeID, AllowAdd = false, AllowEdit = false, AllowDelete = false, AllowView = false, IsRestricted = false });
                    //model.RoleTypeFormPrivileges.Add(new RoleTypeFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm.FormId, RoleTypeID = model.RoleTypeID,   });
                    //model.RoleTypeFormPrivileges.Add(new RoleTypeFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm.FormId, RoleTypeID = model.RoleTypeID,  });
                    //model.RoleTypeFormPrivileges.Add(new RoleTypeFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm.FormId, RoleTypeID = model.RoleTypeID, });
                    //model.RoleTypeFormPrivileges.Add(new RoleTypeFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm.FormId, RoleTypeID = model.RoleTypeID,  });
                }
                else
                {
                    var newItem = new RoleTypeFormPrivilegeViewModel1() { Form = eachForm, FormId = eachForm.Id, RoleTypeID = model.RoleTypeID };
                    model.RoleTypeFormPrivileges.Add(newItem);
                    var itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 1);
                    if (itemInDb == null)
                        newItem.AllowAdd = false;
                    else
                        newItem.AllowAddInt = itemInDb.DataEntryStatus;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 2);
                    if (itemInDb == null)
                        newItem.AllowEdit = false;
                    else
                        newItem.AllowEditInt = itemInDb.DataEntryStatus;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 3);
                    if (itemInDb == null)
                        newItem.AllowDelete = false;
                    else
                        newItem.AllowDeleteInt = itemInDb.DataEntryStatus;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 4);
                    if (itemInDb == null)
                        newItem.AllowView = false;
                    else
                        newItem.AllowViewInt = itemInDb.DataEntryStatus;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 5);
                    if (itemInDb == null)
                        newItem.IsRestricted = false;
                    else
                        newItem.IsRestrictedInt = itemInDb.DataEntryStatus;

                }
            }
            //model.RoleTypeFormPrivileges = rol_eTypeFormPrivileges;
        }

        private void GetMatrixSave(RoleTypeFormPrivilegeMatrixViewModel1 model)
        {
            var roleTypeFormPrivilege = db.RoleTypeFormPrivilege.Where(p => p.RoleTypeId == model.RoleTypeID);
            foreach (var each in model.RoleTypeFormPrivileges)
            {
                var itemInDbs = roleTypeFormPrivilege.Where(p => p.FormId == each.FormId);
                if (itemInDbs == null || itemInDbs.Count() > 0)
                {
                    db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleId, CompanyId = 1, ClientId = 1, PrivilegeId = 1, DataEntryStatus = each.AllowAddInt });
                    db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 2, DataEntryStatus = each.AllowEditInt });
                    db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 3, DataEntryStatus = each.AllowDeleteInt });
                    db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 4, DataEntryStatus = each.AllowViewInt });
                    db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 5, DataEntryStatus = each.IsRestrictedInt });
                }
                else
                {

                    var itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 1);
                    if (itemInDb == null)
                        db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 1, DataEntryStatus = each.AllowAddInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowAddInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 2);
                    if (itemInDb == null)
                        db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 2, DataEntryStatus = each.AllowEditInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowEditInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 3);
                    if (itemInDb == null)
                        db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 3, DataEntryStatus = each.AllowDeleteInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowDeleteInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 4);
                    if (itemInDb == null)
                        db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 4, DataEntryStatus = each.AllowViewInt });
                    else
                        itemInDb.DataEntryStatus = each.AllowViewInt;

                    itemInDb = itemInDbs.FirstOrDefault(p => p.PrivilegeId == 5);
                    if (itemInDb == null)
                        db.RoleTypeFormPrivilege.Add(new RoleTypeFormPrivilege() { FormId = each.FormId, RoleTypeId = model.RoleTypeID, CompanyId = 1, ClientId = 1, PrivilegeId = 5, DataEntryStatus = each.IsRestrictedInt });
                    else
                        itemInDb.DataEntryStatus = each.IsRestrictedInt;

                }
            }
        }

        // GET: RoleTypeFormPrivilege
        public ActionResult Index()
        {
            var roleFormPrivilege = db.RoleTypeFormPrivilege.Include(r => r.Company).Include(r => r.Form).Include(r => r.Client).Include(r => r.Privilege);
            return View(roleFormPrivilege.ToList());
        }

        // GET: RoleTypeFormPrivilege/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTypeFormPrivilege roleFormPrivilege = db.RoleTypeFormPrivilege.Find(id);
            if (roleFormPrivilege == null)
            {
                return HttpNotFound();
            }
            return View(roleFormPrivilege);
        }

        // GET: RoleTypeFormPrivilege/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Company.Where(u => u.DataEntryStatus == 1), "Id", "CompanyName");
            ViewBag.FormId = new SelectList(db.Form.Where(u => u.DataEntryStatus == 1), "Id", "FormName");
            ViewBag.ClientId = new SelectList(db.Client.Where(u => u.DataEntryStatus == 1), "ClientId", "ClientName");
            ViewBag.PrivilegeId = new SelectList(db.Privilege.Where(u => u.DataEntryStatus == 1), "Id", "Name");
            ViewBag.RoleTypeId = new SelectList(db.RoleType.Where(u => u.DataEntryStatus == 1), "Id", "RoleTypeName");
            ViewBag.CreatedBy = new SelectList(db.UserInformation.Where(u => u.DataEntryStatus == 1), "UserId", "IdNumber");
            ViewBag.ModifiedBy = new SelectList(db.UserInformation.Where(u => u.DataEntryStatus == 1), "UserId", "IdNumber");
            return View();
        }

        // POST: RoleTypeFormPrivilege/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleTypePrivilegeId,RoleTypeID,PrivilegeId,FormId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] RoleTypeFormPrivilege roleFormPrivilege)
        {
            if (ModelState.IsValid)
            {
                db.RoleTypeFormPrivilege.Add(roleFormPrivilege);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Company, "Id", "CompanyName", roleFormPrivilege.CompanyId);
            ViewBag.FormId = new SelectList(db.Form, "Id", "FormName", roleFormPrivilege.FormId);
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "ClientName", roleFormPrivilege.ClientId);
            ViewBag.PrivilegeId = new SelectList(db.Privilege, "Id", "Name", roleFormPrivilege.PrivilegeId);
            ViewBag.RoleTypeID = new SelectList(db.RoleType, "Id", "Name", roleFormPrivilege.RoleTypeId);
            ViewBag.CreatedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.ModifiedBy);
            return View(roleFormPrivilege);
        }

        // GET: RoleTypeFormPrivilege/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTypeFormPrivilege roleFormPrivilege = db.RoleTypeFormPrivilege.Find(id);
            if (roleFormPrivilege == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Company, "Id", "CompanyName", roleFormPrivilege.CompanyId);
            ViewBag.FormId = new SelectList(db.Form, "Id", "FormName", roleFormPrivilege.FormId);
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "ClientName", roleFormPrivilege.ClientId);
            ViewBag.PrivilegeId = new SelectList(db.Privilege, "Id", "Name", roleFormPrivilege.PrivilegeId);
            ViewBag.RoleTypeID = new SelectList(db.RoleType, "Id", "Name", roleFormPrivilege.RoleTypeId);
            ViewBag.CreatedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.ModifiedBy);
            return View(roleFormPrivilege);
        }

        // POST: RoleTypeFormPrivilege/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleTypePrivilegeId,RoleTypeID,PrivilegeId,FormId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] RoleTypeFormPrivilege roleFormPrivilege)
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
            ViewBag.RoleTypeID = new SelectList(db.RoleType, "Id", "Name", roleFormPrivilege.RoleTypeId);
            ViewBag.CreatedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.CreatedBy);
            ViewBag.ModifiedBy = new SelectList(db.UserInformation, "UserId", "IdNumber", roleFormPrivilege.ModifiedBy);
            return View(roleFormPrivilege);
        }

        // GET: RoleTypeFormPrivilege/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTypeFormPrivilege roleFormPrivilege = db.RoleTypeFormPrivilege.Find(id);
            if (roleFormPrivilege == null)
            {
                return HttpNotFound();
            }
            return View(roleFormPrivilege);
        }

        // POST: RoleTypeFormPrivilege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            RoleTypeFormPrivilege roleFormPrivilege = db.RoleTypeFormPrivilege.Find(id);
            db.RoleTypeFormPrivilege.Remove(roleFormPrivilege);
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
