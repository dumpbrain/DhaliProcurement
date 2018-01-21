using DhaliProcurement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DhaliProcurement.Controllers
{
    [Authorize]
    public class UnitsController : Controller
    {


        private DCPSContext db = new DCPSContext();


        public ActionResult Index()
        {
            return View(db.Unit.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit units = db.Unit.Find(id);
            if (units == null)
            {
                return HttpNotFound();
            }
            return View(units);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Unit units)
        {
            if (ModelState.IsValid)
            {
                db.Unit.Add(units);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(units);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit units = db.Unit.Find(id);
            if (units == null)
            {
                return HttpNotFound();
            }
            return View(units);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Unit units)
        {
            if (ModelState.IsValid)
            {
                db.Entry(units).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(units);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Unit units = db.Unit.Find(id);
            if (units == null)
            {
                return HttpNotFound();
            }
            return View(units);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit units = db.Unit.Find(id);
            db.Unit.Remove(units);
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

        public JsonResult DeleteUnit(int UnitId)
        {

            var checkUnit = db.ProcProjectItem.Where(x => x.UnitId == UnitId).ToList();

            if (checkUnit.Count == 0)
            {
                bool flag = false;
                try
                {
                    var unit = db.Unit.Where(x => x.Id == UnitId);
                    db.Unit.RemoveRange(unit);
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
                        message = "Unit deletion successful."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = new
                    {
                        flag = false,
                        message = "Unit deletion failed!\nError Occured."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Unit deletion failed!\nThis unit has been used!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

    }
}