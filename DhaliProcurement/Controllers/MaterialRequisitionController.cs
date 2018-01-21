using DhaliProcurement.Helper;
using DhaliProcurement.Models;
using DhaliProcurement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DhaliProcurement.Controllers
{
    [Authorize]
    public class MaterialRequisitionController : Controller
    {
        private DCPSContext db = new DCPSContext();

        public ActionResult Index(int? ReqId)
        {
            ViewBag.ReqId = new SelectList(db.Proc_RequisitionMas, "Id", "RCode");
            var RequisitionList = db.Proc_RequisitionMas.ToList();

            if (ReqId != null)
            {
                RequisitionList = RequisitionList.Where(x => x.Id == ReqId).ToList();
                ViewBag.TenderNo = new SelectList(db.Proc_TenderMas.Where(y => y.Id == ReqId), "Id", "RCode");
                return View(RequisitionList);
            }

            else
            {
                return View(RequisitionList);
            }

            //List<Proc_RequisitionMas> requisitionList = db.Proc_RequisitionMas.ToList();

            //return View(requisitionList);
        }

        public ActionResult Create()
        {
            //var procprojects = db.ProcProject.ToList();
            var procprojects = (from procProject in db.ProcProject
                                join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                                join project in db.Project on site.ProjectId equals project.Id
                                where site.Id == procProject.ProjectSiteId
                                select new { site, project }).Distinct().ToList();

            List<ProjectSite> sites = new List<ProjectSite>();
            foreach (var i in procprojects) {
                //var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.ProjectSiteId);
                var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.site.Id);
                sites.Add(site);
            }

            List<Project> projects = new List<Project>();
            foreach (var i in sites)
            {
                var proj = db.Project.FirstOrDefault(x => x.Id == i.ProjectId);
                projects.Add(proj);
            }

            //ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            //ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");
            ViewBag.ProjectId = new SelectList(projects.Distinct(), "Id", "Name");

            //ViewBag.ProjectId = new SelectList(projects, "Id", "Name");
            ViewBag.SiteId = new SelectList(sites, "Id", "Name");

            ViewBag.PrManId = new SelectList(db.CompanyResource, "Id", "Name");
            ViewBag.StEngId = new SelectList(db.CompanyResource, "Id", "Name");
            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.Unit = new SelectList(db.Unit, "Id", "Name");


            var projectTypeStatus = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" }, new SelectListItem { Text = "Non-Government", Value = "Non-Government" }, }, "Value", "Text");
            ViewBag.ProjectType = projectTypeStatus;
            return View();
        }

        public ActionResult Edit(int RequisitionMasId)
        {

            ViewBag.RequisitionMasId = RequisitionMasId;

            var newProj = (from procRequisitionMas in db.Proc_RequisitionMas
                           join procProject in db.ProcProject on procRequisitionMas.ProcProjectId equals procProject.Id
                           join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                           join project in db.Project on site.ProjectId equals project.Id
                           where procRequisitionMas.Id == RequisitionMasId
                           select project).FirstOrDefault();

            ViewBag.ProjectId = newProj.Id;
            ViewBag.ProjectName = newProj.Name;
            var projectResources = db.ProjectResource.SingleOrDefault(x => x.ProjectId == newProj.Id);
            ViewBag.ProjectManager = NullHelper.ObjectToString(projectResources.CompanyResource.Name);

            var newSite = (from procRequisitionMas in db.Proc_RequisitionMas
                           join procProject in db.ProcProject on procRequisitionMas.ProcProjectId equals procProject.Id
                           join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                           where procRequisitionMas.Id == RequisitionMasId
                           select site).FirstOrDefault();

            ViewBag.SiteId = newSite.Id;
            ViewBag.SiteName = newSite.Name;


            var projectSiteResources = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == newSite.Id);
            ViewBag.SiteEngineer = NullHelper.ObjectToString(projectSiteResources.CompanyResource.Name);

            var master = db.Proc_RequisitionMas.SingleOrDefault(x => x.Id == RequisitionMasId);

            List<SelectListItem> ItemList = new List<SelectListItem>();

            var items = (from procProjectItem in db.ProcProjectItem
                         join procproject in db.ProcProject on procProjectItem.ProcProjectId equals procproject.Id
                         join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                         join project in db.Project on site.ProjectId equals project.Id
                         where project.Id == newProj.Id && site.Id == newSite.Id
                         select procProjectItem).ToList();

            foreach (var x in items)
            {
                var itemName = db.Item.SingleOrDefault(m => m.Id == x.ItemId);
                ItemList.Add(new SelectListItem { Text = itemName.Name, Value = x.ItemId.ToString() });
            }

            ViewBag.ItemName = ItemList;
            //ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.Unit = new SelectList(db.Unit, "Id", "Name");
            var Req = db.Proc_RequisitionMas.SingleOrDefault(x => x.Id == RequisitionMasId);

            VMRequisitionMasterDetail vm = new VMRequisitionMasterDetail();
            vm.requisitionMaster = db.Proc_RequisitionMas.SingleOrDefault(x => x.Id == RequisitionMasId);
            var Details = db.Proc_RequisitionDet.Where(x => x.Proc_RequisitionMasId == RequisitionMasId).ToList();
            List<Proc_RequisitionDet> dt = new List<Proc_RequisitionDet>();
            foreach (var detail in Details)
            {
                dt.Add(detail);
            }
            vm.requisitionDetail = dt;
            return View(vm);
        }

        public ActionResult Details(int RequisitionMasId)
        {
            ViewBag.RequisitionMasId = RequisitionMasId;
            var newProj = (from procRequisitionMas in db.Proc_RequisitionMas
                           join procProject in db.ProcProject on procRequisitionMas.ProcProjectId equals procProject.Id
                           join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                           join project in db.Project on site.ProjectId equals project.Id
                           where procRequisitionMas.Id == RequisitionMasId
                           select project).SingleOrDefault();

            //foreach (var i in newProj)
            //{
            //    ViewBag.ProjectId = i.Id;
            //    ViewBag.ProjectName = i.Name;
            //}
            ViewBag.ProjectId = newProj.Id;
            ViewBag.ProjectName = newProj.Name;
            var projectResources = db.ProjectResource.SingleOrDefault(x => x.ProjectId == newProj.Id);
            ViewBag.ProjectManager = NullHelper.ObjectToString(projectResources.CompanyResource.Name);

            var newSite = (from procRequisitionMas in db.Proc_RequisitionMas
                           join procProject in db.ProcProject on procRequisitionMas.ProcProjectId equals procProject.Id
                           join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                           where procRequisitionMas.Id == RequisitionMasId
                           select site).SingleOrDefault();

            //foreach (var i in newSite)
            //{
            //    ViewBag.SiteId = i.Id;
            //    ViewBag.SiteName = i.Name;
            //}

            ViewBag.SiteId = newSite.Id;
            ViewBag.SiteName = newSite.Name;

            var projectSiteResources = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == newSite.Id);
            ViewBag.SiteEngineer = NullHelper.ObjectToString(projectSiteResources.CompanyResource.Name);

            var master = db.Proc_RequisitionMas.SingleOrDefault(x => x.Id == RequisitionMasId);


            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.Unit = new SelectList(db.Unit, "Id", "Name");
            var Req = db.Proc_RequisitionMas.SingleOrDefault(x => x.Id == RequisitionMasId);

            VMRequisitionMasterDetail vm = new VMRequisitionMasterDetail();
            vm.requisitionMaster = db.Proc_RequisitionMas.SingleOrDefault(x => x.Id == RequisitionMasId);
            var Details = db.Proc_RequisitionDet.Where(x => x.Proc_RequisitionMasId == RequisitionMasId).ToList();
            List<Proc_RequisitionDet> dt = new List<Proc_RequisitionDet>();
            foreach (var detail in Details)
            {
                dt.Add(detail);
            }
            vm.requisitionDetail = dt;
            return View(vm);
        }


        [HttpPost]
        public JsonResult GetSites(int ProjectId)
        {

            //var procsites = (from procProject in db.ProcProject
            //            join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
            //            join project in db.Project on site.ProjectId equals project.Id
            //            where project.Id == ProjectId
            //            select project).ToList();

            var procsites = (from procProject in db.ProcProject
                             join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                             join project in db.Project on site.ProjectId equals project.Id
                             where project.Id == ProjectId
                             select site).ToList();

            List<SelectListItem> siteList = new List<SelectListItem>();
            //List<ProjectSite> siteList = new List<ProjectSite>();
            foreach (var i in procsites)
            {
                //var site = db.ProjectSite.FirstOrDefault(x => x.ProjectId == i.Id);
                var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.Id);
                siteList.Add(new SelectListItem { Text = site.Name, Value = site.Id.ToString() });
                //siteList.Add(site);
            }
            //List < SelectListItem > siteList = new List<SelectListItem>();

            //var projects = db.Project.SingleOrDefault(x => x.Id == ProjectId);

            //var sites = db.ProjectSite.Where(x => x.ProjectId == ProjectId).ToList();
            var projectResources = db.ProjectResource.SingleOrDefault(x => x.ProjectId == ProjectId);
            var projectManager = projectResources.CompanyResource.Name;


            //foreach (var x in sites)
            //{
            //    siteList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            //}

            var result = new
            {
                manager = projectManager,
                Sites = siteList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSiteEngineer(int SiteId, int ProjectId)
        {
            List<SelectListItem> ItemList = new List<SelectListItem>();

            var items = (from procProjectItem in db.ProcProjectItem
                         join procproject in db.ProcProject on procProjectItem.ProcProjectId equals procproject.Id
                         join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                         join project in db.Project on site.ProjectId equals project.Id
                         where project.Id == ProjectId && site.Id == SiteId
                         select procProjectItem).ToList();

            foreach (var x in items)
            {
                var itemName = db.Item.SingleOrDefault(m => m.Id == x.ItemId);
                ItemList.Add(new SelectListItem { Text = itemName.Name, Value = x.ItemId.ToString() });
            }

            var projectSiteResources = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == SiteId);
            var siteEngineer = NullHelper.ObjectToString(projectSiteResources.CompanyResource.Name);
            var result = new
            {
                siteEngineer = siteEngineer,
                Items = ItemList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetUnit(int itemId, int projectId, int siteId)
        {
            //var unit = db.Unit.SingleOrDefault(x => x.Id == itemId);
            var unit = (from procProjectItem in db.ProcProjectItem
                        join units in db.Unit on procProjectItem.UnitId equals units.Id
                        where procProjectItem.ItemId == itemId
                        select units).FirstOrDefault();

            var totalRequired = (from procProjectItem in db.ProcProjectItem
                                 join procproject in db.ProcProject on procProjectItem.ProcProjectId equals procproject.Id
                                 join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                                 join project in db.Project on site.ProjectId equals project.Id
                                 where project.Id == projectId && site.Id == siteId && procProjectItem.ItemId == itemId
                                 select procProjectItem.PQuantity).FirstOrDefault();

            //if (totalRequired == null)
            //{
            //    totalRequired.PQuantity = 0;
            //}
            if (totalRequired == null)
            {
                totalRequired = 0;
            }


            //var totalReceived = (from entryDet in db.Proc_MaterialEntryDet
            //                     join purchaseDet in db.Proc_PurchaseOrderDet on entryDet.Proc_PurchaseOrderDetId equals purchaseDet.Id
            //                     where purchaseDet.ItemId == itemId
            //                     select entryDet).FirstOrDefault();



            //if (totalReceived == null)
            //{
            //    totalReceived.EntryQty = 0;
            //}

            var result = new
            {
                flag = true,
                UnitName = unit.Name,
                UnitId = unit.Id,
                TotalRequired = totalRequired
                //TotalReceived = totalReceived.EntryQty
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateRequisition(IEnumerable<VMRequisitionItem> RequisitionItems, int ProjectId, int SiteId, DateTime RequisitionDate, string ReqNo, string remarks)
        {
            var result = new
            {
                flag = false,
                message = "Requisition saving error !"
            };
            var flag = false;
            var RequisitionId = 0; ;
            //if (RequisitionId == null)
            //{
            //    RequisitionId = 0;
            //}


            var planList = db.Proc_RequisitionMas.Where(x => x.Rcode.Trim() == ReqNo.Trim()).ToList();
            if (planList.Count == 0)
            {
                flag = false;
                Proc_RequisitionMas master = new Proc_RequisitionMas();

                //master.ProcProjectId = ProjectId;
                var newProj = (from procProject in db.ProcProject
                               join site in db.ProjectSite on procProject.ProjectSiteId equals SiteId
                               join project in db.Project on site.ProjectId equals ProjectId
                               where project.Id == ProjectId
                               select procProject);

                foreach (var i in newProj)
                {
                    master.ProcProjectId = i.Id;
                }

                master.ReqDate = RequisitionDate;
                master.Rcode = ReqNo;
                master.Remarks = remarks;
                master.Status = "N";

                db.Proc_RequisitionMas.Add(master);

                flag = db.SaveChanges() > 0;
                var getReqId = db.Proc_RequisitionMas.SingleOrDefault(x => x.Rcode == ReqNo);
                RequisitionId = getReqId.Id;


                foreach (var item in RequisitionItems)
                {
                    Proc_RequisitionDet detail = new Proc_RequisitionDet();
                    detail.ItemId = item.ItemId;
                    detail.Proc_RequisitionMasId = RequisitionId;
                    detail.ReqQty = item.ReqQty;
                    detail.CStockQty = item.CStockQty;
                    detail.Brand = item.Brand;
                    detail.Size = item.Size;
                    detail.RequiredDate = item.RequiredDate;
                    detail.Remarks = item.ItemRemarks;

                    db.Proc_RequisitionDet.Add(detail);
                    db.SaveChanges();
                }
            }
            //else
            //{
            //    var Proc_RequisitionMasId = db.Proc_RequisitionMas.Where(x => x.Rcode.Trim() == ReqNo.Trim()).SingleOrDefault();
            //    var master = db.Proc_RequisitionMas.Find(Proc_RequisitionMasId.Id);
            //    //var prevProj = (from procProject in db.ProcProject
            //    //               join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
            //    //               join project in db.Project on site.ProjectId equals project.Id
            //    //               where project.Id == ProjectId
            //    //               select project).SingleOrDefault();

            //    //master.ProcProjectId = prevProj.Id;
            //    master.ReqDate = RequisitionDate;
            //    master.Rcode = ReqNo;
            //    master.Remarks = remarks;
            //    //master.Status = "A";

            //    db.Entry(master).State = EntityState.Modified;
            //    //db.Proc_RequisitionMas.Add(master);

            //    flag = db.SaveChanges() > 0;
            //    var getReqId = db.Proc_RequisitionMas.SingleOrDefault(x => x.Rcode == ReqNo);
            //    RequisitionId = getReqId.Id;

            //    foreach (var item in RequisitionItems)
            //    {
            //        var check = db.Proc_RequisitionDet.SingleOrDefault(x => x.ProcRequisitionMasId == RequisitionId && x.ItemId == item.ItemId);
            //        if (check == null) {
            //            Proc_RequisitionDet detail = new Proc_RequisitionDet();
            //            detail.ItemId = item.ItemId;
            //            detail.ProcRequisitionMasId = RequisitionId;
            //            detail.ReqQty = item.ReqQty;
            //            detail.CStockQty = item.CStockQty;
            //            detail.Brand = item.Brand;
            //            detail.Size = item.Size;
            //            detail.RequiredDate = item.RequiredDate;
            //            detail.Remarks = item.ItemRemarks;
            //            db.Entry(detail).State = EntityState.Added;
            //            //db.Proc_RequisitionDet.Add(detail);
            //            db.SaveChanges();
            //        }
            //        else
            //        {

            //            var Proc_RequisitionDet_Id = db.Proc_RequisitionDet.SingleOrDefault(x => x.ProcRequisitionMasId == RequisitionId && x.ItemId == item.ItemId);
            //            var getItem = db.Proc_RequisitionDet.Find(Proc_RequisitionDet_Id.Id);
            //            getItem.CStockQty = item.CStockQty;
            //            getItem.Brand = item.Brand;
            //            getItem.Size = item.Size;
            //            getItem.RequiredDate = item.RequiredDate;
            //            getItem.Remarks = item.ItemRemarks;

            //            db.Entry(getItem).State = EntityState.Modified;
            //            db.SaveChanges();
            //        }

            //  }
            //}


            if (flag == true)
            {
                result = new
                {
                    flag = true,
                    message = "Save Successful!"
                };
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EditRequisition(IEnumerable<VMRequisitionItem> RequisitionItems, int ProjectId, int SiteId, DateTime RequisitionDate, string ReqNo, string remarks, int RequisitionMasId)
        {
            var result = new
            {
                flag = false,
                message = "Requisition saving error !"
            };
            var flag = false;

            var master = db.Proc_RequisitionMas.Find(RequisitionMasId);
            master.ReqDate = RequisitionDate;
            master.Rcode = ReqNo;
            master.Remarks = remarks;
            master.Status = "N";
            db.Entry(master).State = EntityState.Modified;
            flag = db.SaveChanges() > 0;

            if (flag)
            {
                foreach (var item in RequisitionItems)
                {
                    var check = db.Proc_RequisitionDet.SingleOrDefault(x => x.Proc_RequisitionMasId == RequisitionMasId && x.ItemId == item.ItemId);
                    if (check == null)
                    {
                        Proc_RequisitionDet detail = new Proc_RequisitionDet();
                        detail.ItemId = item.ItemId;
                        detail.Proc_RequisitionMasId = RequisitionMasId;
                        detail.ReqQty = item.ReqQty;
                        detail.CStockQty = item.CStockQty;
                        detail.Brand = item.Brand;
                        detail.Size = item.Size;
                        detail.RequiredDate = item.RequiredDate;
                        detail.Remarks = item.ItemRemarks;
                        db.Entry(detail).State = EntityState.Added;
                        flag =db.SaveChanges()>0;
                    }
                    else
                    {

                        var Proc_RequisitionDet_Id = db.Proc_RequisitionDet.SingleOrDefault(x => x.Proc_RequisitionMasId == RequisitionMasId && x.ItemId == item.ItemId);
                        var getItem = db.Proc_RequisitionDet.Find(Proc_RequisitionDet_Id.Id);
                        getItem.CStockQty = item.CStockQty;
                        getItem.ReqQty = item.ReqQty;
                        getItem.Brand = item.Brand;
                        getItem.Size = item.Size;
                        getItem.RequiredDate = item.RequiredDate;
                        getItem.Remarks = item.ItemRemarks;

                        db.Entry(getItem).State = EntityState.Modified;
                        flag = db.SaveChanges() > 0;
                    }

                }
            }
            if (flag == true)
            {
                result = new
                {
                    flag = true,
                    message = "Save Successful!"
                };
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePlan(int ProcRequisitionMId)
        {

            var RequisitionCount = (from requisitionMas in db.Proc_RequisitionMas
                               join requisitionDet in db.Proc_RequisitionDet on requisitionMas.Id equals requisitionDet.Proc_RequisitionMasId
                               join tenderDet in db.Proc_TenderDet on requisitionDet.Id equals tenderDet.Proc_RequisitionDetId                              
                               where requisitionMas.Id == ProcRequisitionMId
                               select requisitionMas).Distinct().Count();

            if (RequisitionCount == 0)
            {
                bool flag = false;
            try
            {
                var itemsToDeletePlan = db.Proc_RequisitionDet.Where(x => x.Proc_RequisitionMasId == ProcRequisitionMId);
                db.Proc_RequisitionDet.RemoveRange(itemsToDeletePlan);
                db.SaveChanges();

                var itemsToDeleteTask = db.Proc_RequisitionMas.Where(x => x.Id == ProcRequisitionMId);
                db.Proc_RequisitionMas.RemoveRange(itemsToDeleteTask);

                //var itemsToDeletePlan = db.Proc_RequisitionDet.Where(x => x.Proc_RequisitionMasId== ProcRequisitionMId);
                //db.Proc_RequisitionDet.RemoveRange(itemsToDeletePlan);

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
                    message = "Requisition deletion successful."
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Requisition deletion failed!\nError Occured."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Requisition deletion failed!\nDelete Tender first."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }


        public JsonResult DeleteEditItem(int ProcRequisitionMId, int itemId)
        {
            bool flag = false;
            try
            {
                //var itemsToDeleteTask = db.Proc_RequisitionMas.Where(x => x.Id == ProcRequisitionMId);
                //db.Proc_RequisitionMas.RemoveRange(itemsToDeleteTask);

                var itemsDelete = db.Proc_RequisitionDet.SingleOrDefault(x => x.Proc_RequisitionMasId == ProcRequisitionMId && x.ItemId == itemId);
                db.Proc_RequisitionDet.Remove(itemsDelete);

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
                    message = "Requisition deletion successful."
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Requisition deletion failed!\nError Occured."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult ApproveRequisition(IEnumerable<VMRequisitionItem> RequisitionItems, int RequisitionMasId, string Status)
        {
            var flag = false;
            var result = new
            {
                flag = false,
                message = "Requisition approve error !"
            };

            var master = db.Proc_RequisitionMas.Find(RequisitionMasId);
            master.Status = Status;
            
            db.Entry(master).State = EntityState.Modified;
            flag = db.SaveChanges() > 0;
            if (flag)
            {
                foreach (var item in RequisitionItems)
                {
                    var check = db.Proc_RequisitionDet.SingleOrDefault(x => x.Proc_RequisitionMasId == RequisitionMasId && x.ItemId == item.ItemId);
                    if (check == null)
                    {
                        Proc_RequisitionDet detail = new Proc_RequisitionDet();
                        detail.ItemId = item.ItemId;
                        detail.Proc_RequisitionMasId = RequisitionMasId;
                        detail.ReqQty = item.ReqQty;
                        detail.CStockQty = item.CStockQty;
                        detail.Brand = item.Brand;
                        detail.Size = item.Size;
                        detail.RequiredDate = item.RequiredDate;
                        detail.Remarks = item.ItemRemarks;
                        db.Entry(detail).State = EntityState.Added;
                        flag = db.SaveChanges() > 0;
                    }
                    else
                    {

                        var Proc_RequisitionDet_Id = db.Proc_RequisitionDet.SingleOrDefault(x => x.Proc_RequisitionMasId == RequisitionMasId && x.ItemId == item.ItemId);
                        var getItem = db.Proc_RequisitionDet.Find(Proc_RequisitionDet_Id.Id);
                        getItem.CStockQty = item.CStockQty;
                        getItem.ReqQty = item.ReqQty;
                        getItem.Brand = item.Brand;
                        getItem.Size = item.Size;
                        getItem.RequiredDate = item.RequiredDate;
                        getItem.Remarks = item.ItemRemarks;

                        db.Entry(getItem).State = EntityState.Modified;
                        flag = db.SaveChanges() > 0;
                    }

                }
            }
            if (flag == true)
            {
                result = new
                {
                    flag = true,
                    message = "Approve Successful!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRequisitionDetailItem(int RequisitionDetailId)
        {
            var flag = false;
            var result = new
            {
                flag = false,
                message = "Delete error !"
            };

            var data = db.Proc_RequisitionDet.Where(x => x.Id == RequisitionDetailId).FirstOrDefault();
            db.Proc_RequisitionDet.Remove(data);
            flag = db.SaveChanges() > 0;
            //return RedirectToAction("Edit", "Projects", new { id = projectId });
            if (flag == true)
            {
                result = new
                {
                    flag = true,
                    message = "Delete Successful Successful!"
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}