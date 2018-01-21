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
    public class CompanyResourcesController : AlertController
    {
        private DCPSContext db = new DCPSContext();

        // GET: CompanyResources
        public ActionResult Index(int? ResourceId)
        {
            ViewBag.ResourceId = new SelectList(db.CompanyResource, "Id", "Name");

            var resources = db.CompanyResource.Include(p => p.ProjectResources).Include(p => p.ProjectSiteResources).ToList();

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
            CompanyResource companyResource = db.CompanyResource.Find(id);
            if (companyResource == null)
            {
                return HttpNotFound();
            }
            return View(companyResource);
        }


        
         public ActionResult DetailsCompanyResources(int? projectId, int? companyresourceId)
        {
            if (projectId == null || companyresourceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyResource companyResource = db.CompanyResource.Find(projectId, companyresourceId);
            if (companyResource == null)
            {
                return HttpNotFound();
            }
            return View(companyResource);
        }

        // GET: CompanyResources/Create
        public ActionResult Create()
        {

            var status = new SelectList(new List<SelectListItem>{new SelectListItem{Text="Active",Value="A"},new SelectListItem {Text="Inactive",Value="I"},},"Value","Text");
            ViewBag.Status = status;

            //var enumDataStaffStatus = from VMCompanyResources.StaffStatus e in Enum.GetValues(typeof(VMCompanyResources.StaffStatus))
            //                         select new
            //                         {
            //                             Id = (int)e,
            //                             Name = e.ToString()
            //                         };
            //ViewBag.StaffStatus = new SelectList(enumDataStaffStatus, "Id", "Name");



            return View();
        }

        // POST: CompanyResources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Position,DOJ,Phone,Address,Status")] CompanyResource companyResource)
        {

            var isAjaxRequest = Request.IsAjaxRequest();

            ModelState["Status"].Errors.Clear();

            if (ModelState.IsValid)
            {
                db.CompanyResource.Add(companyResource);
                db.SaveChanges();

                if (isAjaxRequest)
                {
                    var staff = new SelectList(db.CompanyResource.ToList(), "Id", "Name");
                    return Json(new { Flag = true, CompanyResources = staff }, JsonRequestBehavior.AllowGet);
                }

                Success(string.Format("Successfully save data !"), true);
                return RedirectToAction("Index");
            }
            if (!isAjaxRequest) return View(companyResource);
            return Json(null, JsonRequestBehavior.AllowGet);

        }




        //added by Nabid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResourceCreate([Bind(Include = "Id,Name,Position,DOJ,Phone,Address,Status")] CompanyResource companyResource)
        {

            var isAjaxRequest = Request.IsAjaxRequest();

            ModelState["Status"].Errors.Clear();

            if (ModelState.IsValid)
            {
                db.CompanyResource.Add(companyResource);
                db.SaveChanges();

                if (isAjaxRequest)
                {
                    var staff = new SelectList(db.CompanyResource.ToList(), "Id", "Name");
                    return Json(new { Flag = true, CompanyResources = staff }, JsonRequestBehavior.AllowGet);
                }

                Success(string.Format("Successfully save data !"), true);
                return RedirectToAction("Index");
            }
            if (!isAjaxRequest) return View(companyResource);
            return Json(null, JsonRequestBehavior.AllowGet);

        }


        //end






        // GET: CompanyResources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyResource companyResource = db.CompanyResource.Find(id);
            if (companyResource == null)
            {
                return HttpNotFound();
            }
            var status = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Active", Value = "A" }, new SelectListItem { Text = "Inactive", Value = "I" }, }, "Value", "Text", companyResource.Status);
            ViewBag.Status = status;
            return View(companyResource);
        }

        // POST: CompanyResources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Position,DOJ,Phone,Address,Status")] CompanyResource companyResource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyResource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var status = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Active", Value = "A" }, new SelectListItem { Text = "Inactive", Value = "I" }, }, "Value", "Text", companyResource.Status);
            ViewBag.Status = status;
            return View(companyResource);
        }

        // GET: CompanyResources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CompanyResource companyResource = db.CompanyResource.Find(id);
            if (companyResource == null)
            {
                return HttpNotFound();
            }

            ViewBag.ExistsProjectResource = false;
            ViewBag.ExistsSiteResource = false;

            var projectResourceList = db.ProjectResource.Where(x => x.CompanyResourceId == id).ToList();
            if (projectResourceList.Count != 0)
            {
                ViewBag.ExistsProjectResource = true;
                //ViewBag.Count = projectList.Count;
            }

            var siteResourceList = db.ProjectSiteResource.Where(x => x.CompanyResourceId == id).ToList();
            if (siteResourceList.Count != 0)
            {
                ViewBag.ExistsSiteResource = true;
                //ViewBag.Count = projectList.Count;
            }

            return View(companyResource);
        }

        // POST: CompanyResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            var projectResourceList = db.ProjectResource.Where(x => x.CompanyResourceId == id).ToList();
            if (projectResourceList.Count != 0)
            {
                return RedirectToAction("Delete", new { id = id });
            }

            var siteResourceList = db.ProjectSiteResource.Where(x => x.CompanyResourceId == id).ToList();
            if (siteResourceList.Count != 0)
            {
                return RedirectToAction("Delete", new { id = id });
            }

            CompanyResource companyResource = db.CompanyResource.Find(id);
            db.CompanyResource.Remove(companyResource);
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        public JsonResult GetStaff()
        {
            try
            {
                var staff = new SelectList(db.CompanyResource.ToList(), "Id", "Name");
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


        public JsonResult DeleteCompanyResource(int ResourceId)
        {

            var checkProject= db.ProjectResource.Where(x => x.CompanyResourceId == ResourceId).ToList();
            var checkSite = db.ProjectSiteResource.Where(x => x.CompanyResourceId == ResourceId).ToList();

            if (checkProject.Count == 0  && checkSite.Count==0)
            {
                bool flag = false;
                try
                {
                    var resource = db.CompanyResource.Where(x => x.Id == ResourceId);
                    db.CompanyResource.RemoveRange(resource);
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
                        message = "Employee deletion successful."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = new
                    {
                        flag = false,
                        message = "Employee deletion failed!\nError Occured."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Employee deletion failed!\nThis Employee has been used!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
    }
}