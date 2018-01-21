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
    public class ItemsController : AlertController
    {
        private DCPSContext db = new DCPSContext();


        public ActionResult Index(int? ItemId)
        {
            ViewBag.ItemId = new SelectList(db.Item, "Id", "Name");
            var items = db.Item.ToList();
            if (ItemId != null)
            {
                items = items.Where(x => x.Id == ItemId).ToList();
            }
            return View(items);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Size,HSCode,ItemDesc")] Item item)
        {
            var check = db.Item.SingleOrDefault(x => x.Name == item.Name.Trim());
            if(check == null)
            {
                if (ModelState.IsValid)
                {
                    db.Item.Add(item);
                    db.SaveChanges();
                    //TempData["Message"] = "Item added successfully!";
                    Success(string.Format("Successfully save data !"), true);
                    return RedirectToAction("Index");
                }
                else
                {
                    Danger(string.Format("Item creation failed!"), true);
                    return View();
                }


            }
            else
            {
                Danger(string.Format("Item exists!"), true);
                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemName,Remarks")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = db.Item.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Item.Find(id);
            db.Item.Remove(item);
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


        public JsonResult DeleteItem(int ItemId)
        {

            var checkitem= db.ProcProjectItem.Where(x => x.ItemId == ItemId).ToList();

            if (checkitem.Count == 0)
            {
                bool flag = false;
                try
                {
                    var item = db.Item.Where(x => x.Id == ItemId);
                    db.Item.RemoveRange(item);
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
                        message = "Item deletion successful."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = new
                    {
                        flag = false,
                        message = "Item deletion failed!\nError Occured."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Item deletion failed!\nThis unit has been used!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


    }
}