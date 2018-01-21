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
    public class VendorsController : Controller
    {
        private DCPSContext db = new DCPSContext();

       
        public ActionResult Index(int? ResourceId)
        {
            ViewBag.ResourceId = new SelectList(db.Vendor, "Id", "Name");

            var resources = db.Vendor.ToList();

            if (ResourceId != null)
            {
                resources = resources.Where(x => x.Id == ResourceId).ToList();
            }


            //int pageSize = 20;
            //int pageNumber = (page ?? 1);

            return View(resources);

            //return View(db.CompanyResource.ToList());
        }

        // GET: CompanyResources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor companyResource = db.Vendor.Find(id);
            if (companyResource == null)
            {
                return HttpNotFound();
            }
            return View(companyResource);
        }

        // GET: CompanyResources/Create
        public ActionResult Create()
        {

            var status = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Active", Value = "A" }, new SelectListItem { Text = "Inactive", Value = "I" }, }, "Value", "Text");
            ViewBag.Status = status;
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Phone,Web,Email,ContactPerson")] Vendor companyResource)
        {
            if (ModelState.IsValid)
            {
                db.Vendor.Add(companyResource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var status = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Active", Value = "A" }, new SelectListItem { Text = "Inactive", Value = "I" }, }, "Value", "Text");
            ViewBag.Status = status;
            return View(companyResource);
        }




        // GET: CompanyResources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor companyResource = db.Vendor.Find(id);
            if (companyResource == null)
            {
                return HttpNotFound();
            }

            return View(companyResource);
        }

        // POST: CompanyResources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,Web,Email,ContactPerson")] Vendor companyResource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyResource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyResource);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendor.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: CompanyInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendor.Find(id);
            db.Vendor.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetStaff()
        {
            try
            {
                var staff = new SelectList(db.Vendor.ToList(), "Id", "Name");
                return Json(new { Flag = true, CompanyResources = staff }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Flag = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
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


        public JsonResult DeleteVendor(int VendorId)
        {

            var checkitem = db.Proc_TenderDet.Where(x => x.VendorId == VendorId).ToList();

            if (checkitem.Count == 0)
            {
                bool flag = false;
                try
                {
                    var vendor = db.Vendor.Where(x => x.Id == VendorId);
                    db.Vendor.RemoveRange(vendor);
                    flag = db.SaveChanges() > 0;
                }
                catch
                {

                }

                if (flag)
                {
                    var result = new
                    {
                        flag = true,
                        message = "Vendor deletion successful."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = new
                    {
                        flag = false,
                        message = "Vendor deletion failed!\nError Occured."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Vendor deletion failed!\nThis Vendor has been used!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

    }
}