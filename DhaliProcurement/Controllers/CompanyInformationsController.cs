using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DhaliProcurement.Models;

namespace DhaliProcurement.Controllers
{
    [Authorize]
    public class CompanyInformationsController : Controller
    {
        //
        // GET: /CompanyInformations/
        private DCPSContext db = new DCPSContext();

        // GET: CompanyInformations
        public ActionResult Index()
        {
            return View(db.CompanyInformation.ToList());
        }

        // GET: CompanyInformations/Details/5
        public ActionResult Details()
        {

            var companyInformation = db.CompanyInformation.FirstOrDefault();
            if (companyInformation == null)
            {
                ViewBag.Count = false;
            }
            return View(companyInformation);
        }

        // GET: CompanyInformations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Phone,Web,Email,ContactPerson")] CompanyInformation companyInformation)
        {
            if (ModelState.IsValid)
            {
                db.CompanyInformation.Add(companyInformation);
                db.SaveChanges();
                return RedirectToAction("Details");
            }

            return View(companyInformation);
        }

        // GET: CompanyInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInformation companyInformation = db.CompanyInformation.Find(id);
            if (companyInformation == null)
            {
                return HttpNotFound();
            }
            return View(companyInformation);
        }

        // POST: CompanyInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,Web,Email,ContactPerson")] CompanyInformation companyInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(companyInformation);
        }

        // GET: CompanyInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInformation companyInformation = db.CompanyInformation.Find(id);
            if (companyInformation == null)
            {
                return HttpNotFound();
            }
            return View(companyInformation);
        }

        // POST: CompanyInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyInformation companyInformation = db.CompanyInformation.Find(id);
            db.CompanyInformation.Remove(companyInformation);
            db.SaveChanges();
            return RedirectToAction("Details");
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