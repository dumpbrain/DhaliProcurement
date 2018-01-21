using DhaliProcurement.Helper;
using DhaliProcurement.Models;
using DhaliProcurement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DhaliProcurement.Controllers
{
    public class DemoController : Controller
    {
        private DCPSContext db = new DCPSContext();

        // GET: Demo
        public ActionResult DemoCreate()
        {
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");

            ViewBag.PrManId = new SelectList(db.CompanyResource, "Id", "Name");
            ViewBag.StEngId = new SelectList(db.CompanyResource, "Id", "Name");

            ViewBag.ItemName = new SelectList(db.Item, "ISLNO", "ItemName");
            ViewBag.Unit = new SelectList(db.Unit, "USLNO", "UnitName");

            var projectTypeStatus = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" }, new SelectListItem { Text = "Non-Government", Value = "Non-Government" }, }, "Value", "Text");
            ViewBag.ProjectType = projectTypeStatus;



            return View();
        }


        [HttpPost]
        public JsonResult GetPId(int ProjectId)
        {
            List<SelectListItem> siteList = new List<SelectListItem>();

            var projects = db.Project.SingleOrDefault(x => x.Id == ProjectId);
            var sDate = NullHelper.DateToString(projects.StartDate);
            var eDate = NullHelper.DateToString(projects.EndDate);

            var sites = db.ProjectSite.Where(x => x.ProjectId == ProjectId).ToList();
            var projectResources = db.ProjectResource.SingleOrDefault(x => x.ProjectId == ProjectId);
            var projectManager = projectResources.CompanyResource.Name;


            foreach (var x in sites)
            {
                siteList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            }

            var result = new
            {
                start = sDate,
                end = eDate,
                manager = projectManager,
                Sites = siteList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSiteEngineer(int SiteId)
        {
            var projectSiteResources = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == SiteId);
            var siteEngineer = NullHelper.ObjectToString(projectSiteResources.CompanyResource.Name);
            var result = new
            {
                siteEngineer = siteEngineer
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SaveProject(int ProjectId,int SiteId)
        {
            var result = new {
                flag = false
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItems(int id)
        {
            var itemName = db.Item.SingleOrDefault(x => x.Id == id);

            return Json(itemName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ItemCreate(List<DemoVM> ResourceDetails)
        {
            var result = new
            {
                flag = false,
                message = "Saving failed"
            };

            foreach (var item in ResourceDetails)
            {
                ProcProjectItem procitem = new ProcProjectItem();
                procitem.Id = item.ProjectId;
                //procitem. = item.ProjectSiteId;

                procitem.ItemId = item.ItemISLNO;
                procitem.UnitId = item.UnitUSLNO;
                procitem.PQuantity = item.PQuantity;
                //procitem.Unit = (Unit)Enum.Parse(typeof(Unit), item.UnitUSLNO.ToString());
                procitem.PCost = item.PCost;
                procitem.Remarks = item.Remarks;

                db.ProcProjectItem.Add(procitem);
                db.SaveChanges();
                
            }

            result = new
            {
                flag = true,
                message = "Project saving successful."
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
