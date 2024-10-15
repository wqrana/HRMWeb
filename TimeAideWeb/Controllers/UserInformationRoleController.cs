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
    public class UserInformationRoleController : TimeAideWebControllers<UserInformationRole>
    {


        // POST: UserInformationRole/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoleId,UserInformationId,IsDeleted,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] UserInformationRole userInformationRole)
        {
            if (ModelState.IsValid)
            {
                db.UserInformationRole.Add(userInformationRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Company, "Id", "CompanyName", userInformationRole.CompanyId);
            ViewBag.ClientId = new SelectList(db.Client, "Id", "ClientName", userInformationRole.ClientId);
            ViewBag.RoleId = new SelectList(db.Role, "Id", "Name", userInformationRole.RoleId);
            ViewBag.UserID = new SelectList(db.UserInformation, "UserId", "FullName", userInformationRole.UserInformationId);
            return View(userInformationRole);
        }

        // POST: UserInformationRole/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleId,UserInformationId,IsDeleted,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] UserInformationRole userInformationRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInformationRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Company, "Id", "CompanyName", userInformationRole.CompanyId);
            ViewBag.ClientId = new SelectList(db.Client, "Id", "ClientName", userInformationRole.ClientId);
            ViewBag.RoleId = new SelectList(db.Role, "Id", "Name", userInformationRole.RoleId);
            ViewBag.UserID = new SelectList(db.UserInformation, "UserId", "FullName", userInformationRole.UserInformationId);

            return View(userInformationRole);
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
