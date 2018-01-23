using DhaliProcurement.Helper;
using DhaliProcurement.Models;
using DhaliProcurement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DhaliProcurement.Controllers
{
    [Authorize]
    public class MaterialsEntryController : Controller
    {
        private DCPSContext db = new DCPSContext();
        public ActionResult Index(int? ProjectId, int? SiteId)
        {
            var MarterialsEntry = db.Proc_MaterialEntryMas.ToList();

            var procprojects = db.ProcProject.ToList();
            List<ProjectSite> sites = new List<ProjectSite>();
            foreach (var i in procprojects)
            {
                var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.ProjectSiteId);
                sites.Add(site);
            }

            List<Project> projects = new List<Project>();
            foreach (var i in sites)
            {
                var proj = db.Project.FirstOrDefault(x => x.Id == i.ProjectId);
                projects.Add(proj);
            }

            ViewBag.ProjectId = new SelectList(projects, "Id", "Name");
            ViewBag.SiteId = new SelectList(sites, "Id", "Name");

            if (ProjectId != null && SiteId != null)
            {

                MarterialsEntry = MarterialsEntry.Where(x => x.ProcProject.ProjectSiteId == SiteId && x.ProcProject.ProjectSite.ProjectId == ProjectId).ToList();
                ViewBag.SiteId = new SelectList(db.ProjectSite.Where(y => y.ProjectId == ProjectId), "Id", "Name");
                return View(MarterialsEntry);

            }
            else if (ProjectId != null)
            {
                MarterialsEntry = MarterialsEntry.Where(x => x.ProcProject.ProjectSite.ProjectId == ProjectId).ToList();
                ViewBag.SiteId = new SelectList(db.ProjectSite.Where(y => y.ProjectId == ProjectId), "Id", "Name");
                return View(MarterialsEntry);
            }

            else
            {
                return View(MarterialsEntry.ToList());
            }

        }

        public ActionResult Create()
        {
            //ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            //ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");

            var EntryProject = (from purchaseMas in db.Proc_PurchaseOrderMas
                        join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
                        join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                        join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                        join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                        join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                        join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                        join project in db.Project on site.ProjectId equals project.Id
                        select project).ToList();

            var procprojects = db.ProcProject.ToList();
            List<ProjectSite> sites = new List<ProjectSite>();
            foreach (var i in procprojects)
            {
                var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.ProjectSiteId);
                sites.Add(site);
            }

            List<Project> projects = new List<Project>();

            foreach (var i in sites)
            {
                var proj = db.Project.FirstOrDefault(x => x.Id == i.ProjectId);
                projects.Add(proj);
            }

            //var requisitionProjects = from requisitionMas in db.Proc_RequisitionMas
            //                          join procproject in db.ProcProject on requisitionMas.ProcProjectId equals procproject.Id
            //                          //join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
            //                          //join project in db.Project on site.ProjectId equals project.Id
            //                          where requisitionMas.ProcProjectId == procproject.Id
            //                          select procproject;

            ViewBag.ProjectId = new SelectList(EntryProject.Distinct(), "Id", "Name");
            //ViewBag.ProjectId = new SelectList(projects.Distinct(), "Id", "Name");
            ViewBag.SiteId = new SelectList(sites, "Id", "Name");

            List<SelectListItem> ItemList = new List<SelectListItem>();
            var items = (from requisitionDet in db.Proc_RequisitionDet
                         join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                         join procproject in db.ProcProject on requisitionMas.ProcProjectId equals procproject.Id
                         join procProjectItem in db.ProcProjectItem on procproject.Id equals procProjectItem.ProcProjectId
                         join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                         join project in db.Project on site.ProjectId equals project.Id
                         where requisitionDet.ItemId == procProjectItem.ItemId
                         select procProjectItem).ToList();

            foreach (var x in items)
            {
                var itemName = db.Item.SingleOrDefault(m => m.Id == x.ItemId);
                ItemList.Add(new SelectListItem { Text = itemName.Name, Value = x.ItemId.ToString() });
            }
            //var items = from requisitionDet in db.Proc_RequisitionDet

            ViewBag.ItemName = ItemList;

            //ViewBag.ItemName = new SelectList(itemName, "Id", "Name");
            ViewBag.ReqNo = new SelectList(db.Proc_RequisitionMas, "Id", "RCode");
            ViewBag.PONo = new SelectList(db.Proc_PurchaseOrderMas, "PONo", "PONo");
            //ViewBag.VendorId = new SelectList(db.Vendor, "Id", "Name");
            return View();
        }

        public ActionResult Details(int MaterialEntryMasId)
        {
            ViewBag.MaterialEntryMasId = MaterialEntryMasId;
            var id = (from entryMas in db.Proc_MaterialEntryMas
                      join procProject in db.ProcProject on entryMas.ProcProjectId equals procProject.Id
                      join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                      join project in db.Project on site.ProjectId equals project.Id
                      where entryMas.Id == MaterialEntryMasId
                      select new { site, project }).FirstOrDefault();

            ViewBag.ProjectId = id.project.Id;
            ViewBag.ProjectName = id.project.Name;

            ViewBag.SiteId = id.site.Id;
            ViewBag.SiteName = id.site.Name;

            VMMaterialsEntryMasterDetail vm = new VMMaterialsEntryMasterDetail();
            vm.proc_MaterialEntryMas = db.Proc_MaterialEntryMas.SingleOrDefault(x => x.Id == MaterialEntryMasId);
            List<Proc_MaterialEntryDet> tenderList = new List<Proc_MaterialEntryDet>();
            var TenderDetails = db.Proc_MaterialEntryDet.Where(x => x.Proc_MaterialEntryMasId == MaterialEntryMasId).ToList();
            foreach (var item in TenderDetails)
            {
                tenderList.Add(item);
            }
            vm.proc_MaterialEntryDet = tenderList;
            return View(vm);
        }


        public ActionResult Edit(int MaterialEntryMasId)
        {
            ViewBag.MaterialEntryMasId = MaterialEntryMasId;
            var id = (from entryMas in db.Proc_MaterialEntryMas
                     join procProject in db.ProcProject on entryMas.ProcProjectId equals procProject.Id
                     join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                     join project in db.Project on site.ProjectId equals project.Id
                     where entryMas.Id == MaterialEntryMasId
                     select new { site, project }).FirstOrDefault();

            ViewBag.ProjectId = id.project.Id;
            ViewBag.ProjectName = id.project.Name;

            ViewBag.SiteId = id.site.Id;
            ViewBag.SiteName = id.site.Name;

            List<SelectListItem> ItemList = new List<SelectListItem>();

            var items = (from procProjectItem in db.ProcProjectItem
                         join procproject in db.ProcProject on procProjectItem.ProcProjectId equals procproject.Id
                         join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                         join project in db.Project on site.ProjectId equals project.Id
                         where project.Id == id.project.Id && site.Id == id.site.Id
                         select procProjectItem).ToList();


            foreach (var x in items)
            {
                var itemName = db.Item.SingleOrDefault(m => m.Id == x.ItemId);
                ItemList.Add(new SelectListItem { Text = itemName.Name, Value = x.ItemId.ToString() });
            }


            ViewBag.ItemName = ItemList;
            ViewBag.PONo = new SelectList(db.Proc_PurchaseOrderMas, "PONo", "PONo");
            ViewBag.ReqNo = new SelectList(db.Proc_RequisitionMas, "Id", "RCode");
            ViewBag.MaterialEntryMasId = MaterialEntryMasId;

            VMMaterialsEntryMasterDetail vm = new VMMaterialsEntryMasterDetail();
            vm.proc_MaterialEntryMas = db.Proc_MaterialEntryMas.SingleOrDefault(x => x.Id == MaterialEntryMasId);
            List<Proc_MaterialEntryDet> tenderList = new List<Proc_MaterialEntryDet>();
            var TenderDetails = db.Proc_MaterialEntryDet.Where(x => x.Proc_MaterialEntryMasId == MaterialEntryMasId).ToList();
            foreach (var item in TenderDetails)
            {
                tenderList.Add(item);
            }
            vm.proc_MaterialEntryDet = tenderList;
            return View(vm);
        }





        [HttpPost]
        public JsonResult GetSites(int ProjectId)
        {
            List<SelectListItem> siteList = new List<SelectListItem>();

            //var projects = db.Project.SingleOrDefault(x => x.Id == ProjectId);

            var projects = (from purchaseMas in db.Proc_PurchaseOrderMas
                                join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
                                join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Id
                                join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                                join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                                join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                                join project in db.Project on site.ProjectId equals project.Id
                                select project).ToList();

            var sites = db.ProjectSite.Where(x => x.ProjectId == ProjectId).ToList();
            var projectResources = db.ProjectResource.SingleOrDefault(x => x.ProjectId == ProjectId);
            var projectManager = projectResources.CompanyResource.Name;


            foreach (var x in sites)
            {
                siteList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            }

            var result = new
            {
                manager = projectManager,
                Sites = siteList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItemList(int SiteId, int ProjectId)
        {
            List<SelectListItem> ItemList = new List<SelectListItem>();

            var items = (from purchaseMas in db.Proc_PurchaseOrderMas
                         join purchaseDet in db.Proc_PurchaseOrderDet on purchaseMas.Id equals purchaseDet.Proc_PurchaseOrderMasId
                         join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
                         join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                         join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                         join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id                        
                         join procproject in db.ProcProject on requisitionMas.ProcProjectId equals procproject.Id
                         join procProjectItem in db.ProcProjectItem on procproject.Id equals procProjectItem.ProcProjectId
                         join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                         join project in db.Project on site.ProjectId equals project.Id
                         where project.Id == ProjectId && site.Id == SiteId && purchaseDet.ItemId == procProjectItem.ItemId
                         select procProjectItem).Distinct().ToList();

            //var items = (from procProjectItem in db.ProcProjectItem
            //             join procproject in db.ProcProject on procProjectItem.ProcProjectId equals procproject.Id
            //             join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
            //             join project in db.Project on site.ProjectId equals project.Id
            //             where project.Id == ProjectId && site.Id == SiteId
            //             select procProjectItem).ToList();


            foreach (var x in items)
            {
                var itemName = db.Item.SingleOrDefault(m => m.Id == x.ItemId);
                ItemList.Add(new SelectListItem { Text = itemName.Name, Value = x.ItemId.ToString() });
            }


            var result = new
            {
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
                                 select procProjectItem).FirstOrDefault();
            if (totalRequired == null)
            {
                totalRequired.PQuantity = 0;
            }

            List<SelectListItem> ReqList = new List<SelectListItem>();
            //var requisition = from requisitionDet in db.Proc_RequisitionDet
            //                  join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
            //                  join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
            //                  join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
            //                  where requisitionDet.ItemId == itemId && site.Id== siteId
            //                  select requisitionMas;

            var requisition = (from purchaseMas in db.Proc_PurchaseOrderMas
                              join purchaseDet in db.Proc_PurchaseOrderDet on purchaseMas.Id equals purchaseDet.Proc_PurchaseOrderMasId
                              join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
                              join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                              join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                              join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                              join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                              join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                              where requisitionDet.ItemId == itemId && site.Id == siteId
                              select requisitionMas).Distinct();

            foreach (var x in requisition)
            {
                ReqList.Add(new SelectListItem { Text = x.Rcode, Value = x.Rcode.ToString() });
            }

            var result = new
            {
                flag = true,
                UnitName = unit.Name,
                UnitId = unit.Id,
                ReqList = ReqList,
                TotalRequired = totalRequired.PQuantity
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetVendor(int itemId, int projectId, int siteId, string ReqNo)
        //{
        //    var vendor = (from requisitionDet in db.Proc_RequisitionDet
        //                      join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
        //                      join tenderDet in db.Proc_TenderDet on requisitionDet.Id equals tenderDet.Proc_RequisitionDetId
        //                      where requisitionMas.Rcode == ReqNo && requisitionDet.ItemId == itemId
        //                  select tenderDet).FirstOrDefault();

        //    var totalMaterial = (from total in db.ProcProjectItem
        //                         join procProject in db.ProcProject on total.ProcProjectId equals procProject.Id
        //                         where procProject.ProjectSiteId == siteId && total.ItemId == itemId
        //                         select total).FirstOrDefault();

        //    var requiredQty = (from requisitionDet in db.Proc_RequisitionDet
        //                       join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
        //                       where requisitionMas.Rcode == ReqNo && requisitionDet.ItemId == itemId
        //                       select requisitionDet).FirstOrDefault();

        //    var unitPrice = (from tenderDet in db.Proc_TenderDet
        //                     join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
        //                     join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
        //                     where requisitionMas.Rcode == ReqNo && requisitionDet.ItemId == itemId
        //                     select tenderDet).FirstOrDefault();

        //    var result = new
        //    {
        //        VendorId = vendor.VendorId,
        //        vendorName = db.Vendor.SingleOrDefault(x=>x.Id == vendor.VendorId).Name,
        //        TotalMaterial = totalMaterial.PQuantity,
        //        RemainingQty = requiredQty.ReqQty,
        //        UnitPrice = unitPrice.TQPrice
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetVendor(int itemId, int projectId, int siteId, string ReqNo, string PONo)
        {
            //var vendors = (from purchaseDet in db.Proc_PurchaseOrderDet
            //              join purchaseMas in db.Proc_PurchaseOrderMas on purchaseDet.Proc_PurchaseOrderMasId equals purchaseMas.Id
            //              join entryDet in db.Proc_MaterialEntryDet on purchaseDet.Id equals entryDet.Proc_PurchaseOrderDetId 
            //              join vendor in db.Vendor on purchaseMas.VendorId equals vendor.Id
            //              where purchaseMas.PONo == PONo && purchaseMas.VendorId == vendor.Id
            //              select vendor).Distinct().FirstOrDefault();

            
            var vendors = (from purchaseDet in db.Proc_PurchaseOrderDet
                           join purchaseMas in db.Proc_PurchaseOrderMas on purchaseDet.Proc_PurchaseOrderMasId equals purchaseMas.Id
                           //join entryDet in db.Proc_MaterialEntryDet on purchaseDet.Id equals entryDet.Proc_PurchaseOrderDetId
                           join vendor in db.Vendor on purchaseMas.VendorId equals vendor.Id
                           where purchaseMas.PONo == PONo
                           select vendor).Distinct().FirstOrDefault();

            var result = new
            {
                VendorId = vendors.Id,
                vendorName = db.Vendor.SingleOrDefault(x => x.Id == vendors.Id).Name
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPO(int itemId, int projectId, int siteId, string ReqNo)
        {
            var PO = (from requisitionDet in db.Proc_RequisitionDet
                          join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                          join tenderDet in db.Proc_TenderDet on requisitionDet.Id equals tenderDet.Proc_RequisitionDetId
                          join tenderMas in db.Proc_TenderMas on tenderDet.Proc_TenderMasId equals tenderMas.Id
                          join purchaseMas in db.Proc_PurchaseOrderMas on tenderMas.Id equals purchaseMas.Proc_TenderMasId
                          join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                          join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                      where requisitionMas.Rcode == ReqNo && requisitionDet.ItemId == itemId && site.Id == siteId
                      select purchaseMas).Distinct().ToList();

            var totalMaterial = (from total in db.ProcProjectItem
                                 join procProject in db.ProcProject on total.ProcProjectId equals procProject.Id
                                 where procProject.ProjectSiteId == siteId && total.ItemId == itemId
                                 select total).FirstOrDefault();

            var requiredQty = (from requisitionDet in db.Proc_RequisitionDet
                               join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                               where requisitionMas.Rcode == ReqNo && requisitionDet.ItemId == itemId
                               select requisitionDet).FirstOrDefault();

            var unitPrice = (from tenderDet in db.Proc_TenderDet
                             join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                             join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                             where requisitionMas.Rcode == ReqNo && requisitionDet.ItemId == itemId
                             select tenderDet).FirstOrDefault();


            List<SelectListItem> POList = new List<SelectListItem>();

            foreach (var x in PO)
            {
                POList.Add(new SelectListItem { Text = x.PONo, Value = x.PONo.ToString() });
            }

            var result = new
            {
                TotalMaterial = totalMaterial.PQuantity,
                RemainingQty = requiredQty.ReqQty,
                UnitPrice = unitPrice.TQPrice,
                POs = POList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateMaterialsEntry(IEnumerable<VMMaterialsEntry> RequisitionItems, int ProjectId, int SiteId, string EDate)
        {
            var result = new
            {
                flag = false,
                message = "Requisition saving error !"
            };
            var flag = false;
            var EntryDate =  DateTime.ParseExact(EDate, "dd/mm/yyyy",  CultureInfo.CurrentCulture);
            var planList = (from procProject in db.ProcProject
                            join site in db.ProjectSite on procProject.ProjectSiteId equals SiteId
                            join project in db.Project on site.ProjectId equals ProjectId
                            join materialEntry in db.Proc_MaterialEntryMas on procProject.Id equals materialEntry.ProcProjectId
                            where project.Id == ProjectId && site.Id== SiteId && materialEntry.EDate== EntryDate
                            select procProject).ToList();

            if (planList.Count == 0)
            {
                flag = false;
                Proc_MaterialEntryMas master = new Proc_MaterialEntryMas();

                var newProj = (from procProject in db.ProcProject
                               join site in db.ProjectSite on procProject.ProjectSiteId equals SiteId
                               join project in db.Project on site.ProjectId equals ProjectId
                               where project.Id == ProjectId
                               select procProject);

                foreach (var i in newProj)
                {
                    master.ProcProjectId = i.Id;
                }

                master.EDate = EntryDate;
                db.Proc_MaterialEntryMas.Add(master);

                flag = db.SaveChanges() > 0;

               var EntryMasId = master.Id;


                foreach (var item in RequisitionItems)
                {
                    Proc_MaterialEntryDet detail = new Proc_MaterialEntryDet();
                    detail.Proc_MaterialEntryMasId = EntryMasId;
                    var purchaseOrderMasId = db.Proc_PurchaseOrderMas.FirstOrDefault(x=>x.PONo==item.PONo).Id;
                    detail.Proc_PurchaseOrderDetId = db.Proc_PurchaseOrderDet.FirstOrDefault(x=>x.Proc_PurchaseOrderMasId == purchaseOrderMasId && x.ItemId==item.ItemId).Id; 
                    detail.ChallanNo = item.ChallanNo;
                    if (item.ChallanDate == null) {
                        detail.ChallanDate = null;
                    }
                    else
                    {
                        //detail.ChallanDate = DateTime.ParseExact(item.ChallanDate, "dd/mm/yyyy", CultureInfo.CurrentCulture);
                        detail.ChallanDate = DateTime.ParseExact(item.ChallanDate, "dd/mm/yyyy", CultureInfo.CurrentCulture);
                    }
                    
                    detail.EntryQty = item.EntryQty;
                    detail.Status = item.Status;

                    db.Proc_MaterialEntryDet.Add(detail);
                    db.SaveChanges();
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

        public JsonResult DeleteMaterialEntry(int MaterialEntryMasId)
        {
            //var EntryCount = (from entryMas in db.Proc_MaterialEntryMas
            //                       //join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
            //                   join paymentDet in db.Proc_PurchaseOrderMas on entryMas.Id equals paymentDet.Proc_EntryDetId
            //                   where entryMas.Id == MaterialEntryMasId
            //                  select entryMas).Distinct().Count();
            //if (EntryCount == 0)
            //{
            bool flag = false;
            try
            {
                var itemsToDeleteTask = db.Proc_MaterialEntryMas.Where(x => x.Id == MaterialEntryMasId);
                db.Proc_MaterialEntryMas.RemoveRange(itemsToDeleteTask);

                var itemsToDeletePlan = db.Proc_MaterialEntryDet.Where(x => x.Proc_MaterialEntryMasId == MaterialEntryMasId);
                db.Proc_MaterialEntryDet.RemoveRange(itemsToDeletePlan);

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
                    message = "Material Entry deletion successful."
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Material Entry deletion failed!\nError Occured."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //}
            //else
            //{
            //    var result = new
            //    {
            //        flag = false,
            //        message = "Tender deletion failed!\nDelete purchase order first."
            //    };
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}

        }

        public JsonResult getEditMaterialEntry(int MaterialEntryMasId)
        {
            bool flag = false;

            var check = db.Proc_MaterialEntryDet.Where(x => x.Proc_MaterialEntryMasId == MaterialEntryMasId).Count();
            if (check > 0)
            {
                flag = true;
                var data = db.Proc_MaterialEntryDet.Where(x => x.Proc_MaterialEntryMasId == MaterialEntryMasId).ToList();

                List<VMEditMaterialsEntryItem> detailsList = new List<VMEditMaterialsEntryItem>();
                foreach (var i in data)
                {
                    VMEditMaterialsEntryItem vm = new VMEditMaterialsEntryItem();

                    vm.Proc_MaterialEntryDetId = i.Id;
                    //var projectId = (from tenderDet in db.Proc_TenderDet
                    //                 join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                    //                 join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                    //                 join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                    //                 join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                    //                 join project in db.Project on site.ProjectId equals project.Id
                    //                 where tenderDet.Proc_TenderMasId == i.Proc_TenderMasId
                    //                 select project).FirstOrDefault();
                    //vm.ProjectId = projectId.Id;
                    //vm.ProjectName = projectId.Name;

                    //var siteId = (from tenderDet in db.Proc_TenderDet
                    //              join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                    //              join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                    //              join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                    //              join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                    //              where tenderDet.Proc_TenderMasId == TenderId
                    //              select site).FirstOrDefault();
                    //vm.SiteId = siteId.Id;
                    //vm.SitetName = siteId.Name;

                    vm.ChallanNo = i.ChallanNo;

                    //var date = NullHelper.DateToString(i.ChallanDate);
                    if (i.ChallanDate == null)
                    {
                        vm.ChallanDate = "";
                    }
                    else
                    {
                        vm.ChallanDate = NullHelper.DateToString(i.ChallanDate);
                    }
                    //vm.ChallanDate = i.ChallanDate;
                    vm.EntryQty = i.EntryQty;
                    vm.Status = i.Status;

                    var ItemId = (from entryDet in db.Proc_MaterialEntryDet
                                  join entryMas in db.Proc_MaterialEntryMas on entryDet.Proc_MaterialEntryMasId equals entryMas.Id
                                  join procProject in db.ProcProject on entryMas.ProcProjectId equals procProject.Id
                                  join procProjectItem in db.ProcProjectItem on procProject.Id equals procProjectItem.ProcProjectId
                                  join items in db.Item on procProjectItem.ItemId equals items.Id
                                  where entryDet.Proc_MaterialEntryMasId == MaterialEntryMasId
                                  select items).FirstOrDefault();
                    vm.ItemId = ItemId.Id;
                    vm.ItemName = ItemId.Name;

                    var requisitionId = (from entryDet in db.Proc_MaterialEntryDet
                                         join entryMas in db.Proc_MaterialEntryMas on entryDet.Proc_MaterialEntryMasId equals entryMas.Id
                                         join procProject in db.ProcProject on entryMas.ProcProjectId equals procProject.Id
                                         join procProjectItem in db.ProcProjectItem on procProject.Id equals procProjectItem.ProcProjectId
                                         join requisitionMas in db.Proc_RequisitionMas on procProject.Id equals requisitionMas.ProcProjectId
                                         where entryDet.Proc_MaterialEntryMasId == MaterialEntryMasId && procProjectItem.ItemId == ItemId.Id
                                         select requisitionMas).FirstOrDefault();

                    vm.RCode = requisitionId.Rcode;

                    var PONo = (from purchaseOrderMas in db.Proc_PurchaseOrderMas
                                join purchaseOrderDet in db.Proc_PurchaseOrderDet on purchaseOrderMas.Id equals purchaseOrderDet.Proc_PurchaseOrderMasId
                                join entryDet in db.Proc_MaterialEntryDet on purchaseOrderDet.Id equals entryDet.Proc_PurchaseOrderDetId
                                where entryDet.Proc_MaterialEntryMas.Id == MaterialEntryMasId
                                select purchaseOrderMas).FirstOrDefault();

                    vm.PONo = PONo.PONo;
                    vm.PurchaseOrderDetId = db.Proc_PurchaseOrderDet.FirstOrDefault(x => x.Proc_PurchaseOrderMasId == PONo.Id).Id;


                    //var unit = db.Unit.FirstOrDefault(x => x.Id == ItemId.Id);
                    var unit = (from procProjectItem in db.ProcProjectItem
                                join units in db.Unit on procProjectItem.UnitId equals units.Id
                                where procProjectItem.ItemId == ItemId.Id
                                select units).FirstOrDefault();
                    vm.UnitId = unit.Id;
                    vm.UnitName = unit.Name;

                    var vendors = (from purchaseDet in db.Proc_PurchaseOrderDet
                                   join purchaseMas in db.Proc_PurchaseOrderMas on purchaseDet.Proc_PurchaseOrderMasId equals purchaseMas.Id
                                   //join entryDet in db.Proc_MaterialEntryDet on purchaseDet.Id equals entryDet.Proc_PurchaseOrderDetId
                                   join vendor in db.Vendor on purchaseMas.VendorId equals vendor.Id
                                   where purchaseMas.PONo == PONo.PONo
                                   select vendor).Distinct().FirstOrDefault();

                    //var vendor = (from requisitionDet in db.Proc_RequisitionDet
                    //              join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                    //              join tenderDet in db.Proc_TenderDet on requisitionDet.Id equals tenderDet.Proc_RequisitionDetId
                    //              where requisitionMas.Rcode == requisitionId.Rcode && requisitionDet.ItemId == ItemId.Id
                    //              select tenderDet).FirstOrDefault();

                    vm.VendorId = vendors.Id;
                    vm.VendorName = db.Vendor.SingleOrDefault(x => x.Id == vendors.Id).Name;
                    var totalMaterial = (from total in db.ProcProjectItem
                                         join procProject in db.ProcProject on total.ProcProjectId equals procProject.Id
                                         where  total.ItemId == ItemId.Id
                                         select total).FirstOrDefault();

                    var requiredQty = (from requisitionDet in db.Proc_RequisitionDet
                                       join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                       where requisitionMas.Rcode == requisitionId.Rcode && requisitionDet.ItemId == ItemId.Id
                                       select requisitionDet).FirstOrDefault();

                    var unitPrice = (from tenderDet in db.Proc_TenderDet
                                     join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                                     join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                     where requisitionMas.Rcode == requisitionId.Rcode && requisitionDet.ItemId == ItemId.Id

                                     select tenderDet).FirstOrDefault();
                    vm.TotalMaterial = totalMaterial.PQuantity;
                    vm.RemainingQty = requiredQty.ReqQty;
                    vm.UnitPrice = unitPrice.TQPrice;
                    vm.Status = i.Status;
                    detailsList.Add(vm);
                }

                var result = new
                {
                    flag = flag,
                    List = detailsList
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new
                {
                    flag = flag
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult EditEntry(IEnumerable<VMMaterialsEntry> EntryItems,int EntryMasId, int ProcProjectId, int ProjectId,string TNo, int SiteId ,string EDate)
        {
            var result = new
            {
                flag = false,
                message = "Entry saving error !"
            };
            var flag = false;

            var master = db.Proc_MaterialEntryMas.Find(EntryMasId);
            master.EDate = DateTime.ParseExact(EDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            //master.EDate = EDate;
            db.Entry(master).State = EntityState.Modified;
            flag = db.SaveChanges() > 0;
            //var TenderMasId = master.Id;

            if (flag)
            {
                foreach (var item in EntryItems)
                {
                    var check = db.Proc_MaterialEntryDet.FirstOrDefault(x => x.Proc_MaterialEntryMasId == EntryMasId && x.Proc_PurchaseOrderDet.ItemId==item.ItemId && x.ChallanNo == item.ChallanNo);
                    //var check = data.SingleOrDefault(x=>x.ChallanNo==item.ChallanNo && x.Proc_PurchaseOrderDet.ItemId==item.ItemId);

                    if (check == null)
                    {
                        Proc_MaterialEntryDet detail = new Proc_MaterialEntryDet();
                        detail.Proc_MaterialEntryMasId = EntryMasId;
                        detail.ChallanNo = item.ChallanNo;
                        if (item.ChallanDate == null)
                        {
                            detail.ChallanDate = null;
                        }
                        else
                        {
                            detail.ChallanDate = DateTime.ParseExact(item.ChallanDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        }
                        //detail.ChallanDate = item.ChallanDate;
                        detail.EntryQty = item.EntryQty;
                        detail.Status = item.Status;

                        var PurchaseOrderMasId = db.Proc_PurchaseOrderMas.FirstOrDefault(x => x.PONo == item.PONo);
                        var PurchaseOrderDetId = db.Proc_PurchaseOrderDet.FirstOrDefault(y => y.Proc_PurchaseOrderMasId == PurchaseOrderMasId.Id && y.ItemId == item.ItemId);

                        detail.Proc_PurchaseOrderDetId = PurchaseOrderDetId.Id;
                        db.Entry(detail).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    else
                    {

                        var Proc_EntryDet_Id = db.Proc_MaterialEntryDet.FirstOrDefault(x => x.Proc_MaterialEntryMasId == EntryMasId && x.Proc_PurchaseOrderDet.ItemId == item.ItemId);
                        var getItem = db.Proc_MaterialEntryDet.Find(Proc_EntryDet_Id.Id);

                        getItem.ChallanNo = item.ChallanNo;
                        if (item.ChallanDate == null)
                        {
                            getItem.ChallanDate = null;
                        }
                        else
                        {
                            getItem.ChallanDate = DateTime.ParseExact(item.ChallanDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        }
                        //getItem.ChallanDate = item.ChallanDate;
                        getItem.EntryQty = item.EntryQty;
                        getItem.Status = item.Status;

                        db.Entry(getItem).State = EntityState.Modified;
                        db.SaveChanges();
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

        public ActionResult DeleteEntryDetailItem(int EntryDetailId)
        {
            var flag = false;
            var result = new
            {
                flag = false,
                message = "Delete error !"
            };

            var data = db.Proc_MaterialEntryDet.Where(x => x.Id == EntryDetailId).FirstOrDefault();
            db.Proc_MaterialEntryDet.Remove(data);
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