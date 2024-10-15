using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ClientController : TimeAideWebControllers<Client>
    {

        // POST: Client/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClientName,Address1,Address2,CityId,StateId,ZipCode,CountryId,Phone,Fax,Email,WebSite,ContactName,PayrollName,PayrollAddress1,PayrollAddress2,PayrollCountryId,PayrollCityId,PayrollStateId,PayrollZipCode,EIN,PayrollFax,PayrollContactName,PayrollContactTitle,PayrollContactPhone,PayrollEmail,CompanyStartDate,SICCode,NAICSCode,SeguroChoferilAccount,DepartamentoDelTrabajoAccount,DepartamentoDelTrabajoRate,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.CreatedBy = SessionHelper.LoginId;
                client.CreatedDate = DateTime.Now;
                db.Client.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClientName,Address1,Address2,CityId,StateId,ZipCode,CountryId,Phone,Fax,Email,WebSite,ContactName,PayrollName,PayrollAddress1,PayrollAddress2,PayrollCountryId,PayrollCityId,PayrollStateId,PayrollZipCode,EIN,PayrollFax,PayrollContactName,PayrollContactTitle,PayrollContactPhone,PayrollEmail,CompanyStartDate,SICCode,NAICSCode,SeguroChoferilAccount,DepartamentoDelTrabajoAccount,DepartamentoDelTrabajoRate,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.ModifiedBy = SessionHelper.LoginId;
                client.ModifiedDate = DateTime.Now;
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }
        [HttpGet]
        public JsonResult CreateEdit(int id)
        {
            dynamic model=null;
            if (id == 0)
            {
                model = new Client();
            }
            else
            {
                model = db.Client
                          .Where(w=>w.Id == id)
                          .Select(s=> new
                          {
                              Id = s.Id,
                              ClientName = s.ClientName,
                              Address1 = s.Address1,
                              Address2 = s.Address2,
                              CityId= s.CityId,
                              StateId = s.StateId,
                              ZipCode = s.ZipCode,
                              CountryId = s.CountryId,
                              Phone = s.Phone,
                              Fax = s.Fax,
                              Email = s.Email,
                              WebSite = s.WebSite,
                              ContactName = s.ContactName,
                              PayrollName= s.PayrollName,
                              PayrollAddress1= s.PayrollAddress1,
                              PayrollAddress2= s.PayrollAddress2,
                              PayrollCountryId= s.PayrollCountryId,
                              PayrollCityId= s.PayrollCityId,
                              PayrollStateId= s.PayrollStateId,
                              PayrollZipCode= s.PayrollZipCode,
                              EIN= s.EIN,
                              PayrollFax= s.PayrollFax,
                              PayrollContactName= s.PayrollContactName,
                              PayrollContactTitle= s.PayrollContactTitle,
                              PayrollContactPhone= s.PayrollContactPhone,
                              PayrollEmail= s.PayrollEmail,
                              CompanyStartDate= s.CompanyStartDate,
                              SICCode= s.SICCode,
                              NAICSCode= s.NAICSCode,
                              SeguroChoferilAccount= s.SeguroChoferilAccount,
                              DepartamentoDelTrabajoAccount= s.DepartamentoDelTrabajoAccount,
                              DepartamentoDelTrabajoRate= s.DepartamentoDelTrabajoRate,
                              IsTimeAideWindow =s.IsTimeAideWindow,
                              DBServerName=s.DBServerName,
                              DBName=s.DBName,
                              DBUser=s.DBUser,
                              DBPassword =s.DBPassword,
                              CreatedBy = s.CreatedBy,
                              CreatedDate= s.CreatedDate
                          })
                          .FirstOrDefault();
            }
            JsonResult jsonResult = new JsonResult()
            {
                Data = model,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
            return jsonResult;
        }
        [HttpPost]
        public JsonResult CreateEdit([Bind(Include = "Id,ClientName,Address1,Address2,CityId,StateId,ZipCode,CountryId,Phone,Fax,Email,WebSite,ContactName,PayrollName,PayrollAddress1,PayrollAddress2,PayrollCountryId,PayrollCityId,PayrollStateId,PayrollZipCode,EIN,PayrollFax,PayrollContactName,PayrollContactTitle,PayrollContactPhone,PayrollEmail,CompanyStartDate,SICCode,NAICSCode,SeguroChoferilAccount,DepartamentoDelTrabajoAccount,DepartamentoDelTrabajoRate,IsTimeAideWindow,DBServerName,DBName,DBUser,DBPassword,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] Client model)
        {
            string status = "Success";
            string errorMessage = "";

           if(ModelState.IsValid)
            {
                try
                {
                    var isExist=db.Client.Where(w => w.DataEntryStatus == 1 &&
                                    w.Id != model.Id &&
                                    w.ClientName.ToLower().Contains(model.ClientName.ToLower())).Count();
                    if (isExist == 0)
                    {
                        if (model.Id == 0)
                        {
                            model.CreatedBy = SessionHelper.LoginId;
                            model.CreatedDate = DateTime.Now;
                            db.Client.Add(model);
                            db.SaveChanges();
                            errorMessage = "Successfully Added!";
                        }
                        else
                        {

                            model.ModifiedBy = SessionHelper.LoginId;
                            model.ModifiedDate = DateTime.Now;
                            var updateEntry = db.Entry(model);
                            updateEntry.State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        status = "Error";
                        errorMessage = "Client Name is already Exist";
                    }
                }
                catch( Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    errorMessage = ex.Message;
                }
            }
            else
            {
                status = "Error";
                errorMessage = GetModelError(ViewData);
            }
           
            return Json(new {status= status, errorMessage = errorMessage });
        }
        public JsonResult ValidateTAWindowConnection([Bind(Include = "Id,DBServerName,DBName,DBUser,DBPassword")] Client model)
        {
            string status = "Success";
            string message = "Successfully Tested!";

           
                try
                {
                    var connStrDb = string.Format("data source={0};initial catalog={1};User ID={2};Password={3};MultipleActiveResultSets=True;App=EntityFramework", model.DBServerName, model.DBName, model.DBUser, model.DBPassword);
                    var isValidated =DataHelper.ValidateTAWindowConnection(connStrDb);
                    if (!isValidated) throw new Exception("Not Validated");
                }
                catch (Exception ex)
                {
                    
                    status = "Error";
                    message = "Connection is not Validated.";
                }                  

            return Json(new { status = status, message = message });
        }
        public override ActionResult Details(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                Client model = db.Client
                                            .Where(w => w.Id == id)
                                            .Include(i => i.Country)
                                            .Include(i => i.State)
                                            .Include(i => i.City)
                                            .Include(i => i.PayrollCountry)
                                            .Include(i => i.PayrollState)
                                            .Include(i => i.PayrollCity)
                                            .FirstOrDefault();


                if (model == null)
                {
                    return PartialView();
                }
              
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Client", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
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
