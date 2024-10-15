using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Controllers;
using TimeAide.Web.Models;

namespace WebApplication4.Controllers
{
    public class PositionController_old : TimeAideWebControllers<Position>
    {
        // POST: Position/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PositionName,PositionDescription,PositionCode,DefaultPayScaleId,DefaultEEOCategoryId")] Position position)
        {
            if (ModelState.IsValid)
            {
                db.Position.Add(position);
                db.SaveChanges();
                return Json(position);
            }
            return GetErrors();
        }

        // POST: Position/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PositionName,PositionDescription,PositionCode,DefaultPayScaleId,DefaultEEOCategoryId")] Position position)
        {
            if (ModelState.IsValid)
            {
                position.SetUpdated<Position>();
                db.Entry(position).State = EntityState.Modified;
                db.SaveChanges();
                return Json(position);
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
