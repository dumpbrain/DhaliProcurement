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
    public class ClientsController : AlertController
    {
        private DCPSContext db = new DCPSContext();

        // GET: Clients
        public ActionResult Index(int? ClientId)
        {

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            var clients = db.Client.Include(p => p.Projects).ToList();

            if (ClientId != null)
            {
                clients = clients.Where(x => x.Id == ClientId).ToList();
            }


            //int pageSize = 20;
            //int pageNumber = (page ?? 1);

            return View(clients);



            //  return View(db.Client.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Phone,ContactPerson,Note")] Client client)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Client.Add(client);
            //    db.SaveChanges();
            //    Success(string.Format("Successfully save data !"), true);
            //    return RedirectToAction("Index");
            //}

            //return View(client);


            var isAjaxRequest = Request.IsAjaxRequest();
            if (ModelState.IsValid)
            {
                db.Client.Add(client);
                db.SaveChanges();

                if (isAjaxRequest)
                {
                    var clients = new SelectList(db.Client.ToList(), "Id", "Name");
                    return Json(new { Flag = true, Clients = clients }, JsonRequestBehavior.AllowGet);
                }

                Success(string.Format("Successfully save data !"), true);
                return RedirectToAction("Index");
            }
            if (!isAjaxRequest) return View(client);
            return Json(null, JsonRequestBehavior.AllowGet);




        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,ContactPerson,Note")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            ViewBag.Exists = false;

            var projectList = db.Project.Where(x => x.ClientId == id).ToList();
            if (projectList.Count != 0)
            {
                ViewBag.Exists = true;
                ViewBag.Count = projectList.Count;
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var projectList = db.Project.Where(x => x.ClientId == id).ToList();
            if (projectList.Count != 0)
            {
                return RedirectToAction("Delete", new { id = id });
            }

            Client client = db.Client.Find(id);
            db.Client.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public JsonResult GetClients()
        {
            try
            {
                var clients = new SelectList(db.Client.ToList(), "Id", "Name");
                return Json(new { Flag = true, Clients = clients }, JsonRequestBehavior.AllowGet);
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

        public JsonResult DeleteClient(int ClientId)
        {

            //var checkClient = db.ProcProjectItem.Where(x => x.UnitId == UnitId).ToList();

            //if (checkUnit.Count == 0)
            //{
            bool flag = false;
            //    try
            //    {
            var client = db.Client.Where(x => x.Id == ClientId);
                    db.Client.RemoveRange(client);
                    flag = db.SaveChanges() > 0;
            if (flag)
            {
                var result = new
                {
                    flag = true,
                    message = "Client deletion successful."
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Client deletion failed!\nError Occured."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //    }
            //    catch
            //    {

            //    }

            //    if (flag)
            //    {
            //        var result = new
            //        {
            //            flag = true,
            //            message = "Unit deletion successful."
            //        };
            //        return Json(result, JsonRequestBehavior.AllowGet);

            //    }
            //    else
            //    {
            //        var result = new
            //        {
            //            flag = false,
            //            message = "Unit deletion failed!\nError Occured."
            //        };
            //        return Json(result, JsonRequestBehavior.AllowGet);
            //    }

            //}
            //else
            //{
            //    var result = new
            //    {
            //        flag = false,
            //        message = "Unit deletion failed!\nThis unit has been used!"
            //    };
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}

        }





    }
}