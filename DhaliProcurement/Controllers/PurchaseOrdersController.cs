using DhaliProcurement.Helper;
using DhaliProcurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DhaliProcurement.ViewModel;
using System.Data.Entity;

namespace DhaliProcurement.Controllers
{
    [Authorize]
    public class PurchaseOrdersController : Controller
    {
        private DCPSContext db = new DCPSContext();

        // GET: PurchaseOrders
        public ActionResult Index(int? PurchaseNo)
        {
            var purchaseOrderList = db.Proc_PurchaseOrderMas.ToList();
            ViewBag.PurchaseNo = new SelectList(db.Proc_PurchaseOrderMas, "Id", "PONo");
            if (PurchaseNo != null)
            {
                purchaseOrderList = purchaseOrderList.Where(x => x.Id == PurchaseNo).ToList();
            }

            return View(purchaseOrderList);
        }





        public JsonResult getPurchaseOrderedItems(int PurchaseMasId, int TenderMasId)
        {
            //var counter = 0;
            List<VMPurchaseOrderEdit> check = new List<VMPurchaseOrderEdit>();
            var items = db.Proc_PurchaseOrderDet.Where(x => x.Proc_PurchaseOrderMasId == PurchaseMasId).ToList();


            //var purchaseItemDet = (from purchaseDet in db.Proc_PurchaseOrderDet
            //                       join purchaseMas in db.Proc_PurchaseOrderMas on purchaseDet.Proc_PurchaseOrderMasId equals purchaseMas.Id
            //                       join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
            //                       join reqDet in db.Proc_RequisitionDet on purchaseDet.ItemId equals reqDet.ItemId
            //                       join reqMas in db.Proc_RequisitionMas on reqDet.Proc_RequisitionMasId equals reqMas.Id
            //                       join procItem in db.ProcProjectItem on reqMas.ProcProjectId equals procItem.ProcProjectId
            //                       //join procProj in db.ProcProject on procItem.ProcProjectId equals procProj.Id
            //                       //join projSite in db.ProjectSite on procProj.ProjectSiteId equals projSite.Id
            //                       //join proj in db.Project on projSite.ProjectId equals proj.Id
            //                       join tenderDet in db.Proc_TenderDet on purchaseMas.Proc_TenderMasId equals tenderDet.Proc_TenderMasId
            //                       where purchaseMas.Id == PurchaseMasId && tenderMas.Id== TenderMasId
            //                       select new { purchaseDet, reqDet, procItem, tenderDet }).Distinct().ToList();


            var purchaseItemDet = (from purchaseDet in db.Proc_PurchaseOrderDet
                                   join purchaseMas in db.Proc_PurchaseOrderMas on purchaseDet.Proc_PurchaseOrderMasId equals purchaseMas.Id
                                   where purchaseMas.Id == PurchaseMasId
                                   select new { purchaseDet }).Distinct().ToList();

            //var unit = (from procProjectItem in db.ProcProjectItem
            //            join units in db.Unit on procProjectItem.UnitId equals units.Id
            //            where tenderDet.Proc_TenderMasId == tenderMasId
            //            select units).FirstOrDefault();


            //var price = (from tenderDet in db.Proc_TenderDet
            //             join procReqDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals procReqDet.Id
            //             join procItem in db.ProcProjectItem on procReqDet.ItemId equals procItem.ItemId
            //             where tenderDet.Proc_TenderMasId == tenderMasId
            //             select tenderDet).FirstOrDefault();


            //var size = (from procReqDet in db.Proc_RequisitionDet
            //            join item in db.Item on procReqDet.ItemId equals item.Id
            //            join procReqMas in db.Proc_RequisitionMas on procReqDet.Proc_RequisitionMasId equals procReqMas.Id
            //            join tenderDet in db.Proc_TenderDet on procReqDet.Id equals tenderDet.Proc_RequisitionDetId
            //            where tenderDet.Proc_TenderMasId == tenderMasId
            //            select procReqDet).FirstOrDefault();



            foreach (var i in purchaseItemDet)
            {
                VMPurchaseOrderEdit p = new VMPurchaseOrderEdit();
                p.ProcPurchaseMasterId = i.purchaseDet.Proc_PurchaseOrderMasId;

                //21 Jan 18
                p.PurchaseOrderDetId = i.purchaseDet.Id;

                p.ItemId = i.purchaseDet.ItemId;
                // p.ItemName = i.purchaseDet.Item.Name;
                p.POQuantity = i.purchaseDet.POQty;
                var size = (from procReqDet in db.Proc_RequisitionDet
                            join item in db.Item on procReqDet.ItemId equals item.Id
                            join procReqMas in db.Proc_RequisitionMas on procReqDet.Proc_RequisitionMasId equals procReqMas.Id
                            join tenderDet in db.Proc_TenderDet on procReqDet.Id equals tenderDet.Proc_RequisitionDetId
                            where tenderDet.Proc_TenderMasId == TenderMasId
                            select procReqDet).FirstOrDefault();
                p.Size = size.Size;

                var unit = (from procProjectItem in db.ProcProjectItem
                            join units in db.Unit on procProjectItem.UnitId equals units.Id
                            where procProjectItem.ItemId == i.purchaseDet.ItemId
                            select units).FirstOrDefault();

                p.UnitId = unit.Id;
                //p.UnitName = i.procItem.Unit.Name;

                var price = (from tenderDet in db.Proc_TenderDet
                             join procReqDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals procReqDet.Id
                             join procItem in db.ProcProjectItem on procReqDet.ItemId equals procItem.ItemId
                             where tenderDet.Proc_TenderMasId == TenderMasId
                             select tenderDet).FirstOrDefault();

                p.UnitPrice = price.TQPrice;
                p.TotalPrice = i.purchaseDet.POAmt;
                var itemsName = db.Item.FirstOrDefault(x => x.Id == i.purchaseDet.ItemId);
                p.ItemName = itemsName.Name;
                //var unitsName = unit.Name;
                p.UnitName = unit.Name;

                check.Add(p);
            }

            return Json(check, JsonRequestBehavior.AllowGet);

        }



        public ActionResult Edit(int purchaseOrderId, int tenderMasId)
        {
            //var procPurchaseMasterId = db.Proc_PurchaseOrderMas.FirstOrDefault(x => x.Id == purchaseOrderId && x.Proc_TenderMasId == tenderMasId);
            var procPurchaseMasterId = db.Proc_PurchaseOrderMas.FirstOrDefault(x => x.Id == purchaseOrderId);

            ViewBag.ProcPurchaseMasterId = procPurchaseMasterId.Id;

            //   var procPurchaseDetId = db.Proc_PurchaseOrderDet.FirstOrDefault(x => x.Proc_PurchaseOrderMasId == purchaseOrderId);
            //   ViewBag.ProcPurchaseDetId = procPurchaseDetId.Id;


            ViewBag.PurchaseMasId = purchaseOrderId;
            ViewBag.TenderMasId = tenderMasId;


            var purchaseItemDet = (from purchaseDet in db.Proc_PurchaseOrderDet
                                   join purchaseMas in db.Proc_PurchaseOrderMas on purchaseDet.Proc_PurchaseOrderMasId equals purchaseMas.Id
                                   join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
                                   join reqDet in db.Proc_RequisitionDet on purchaseDet.ItemId equals reqDet.ItemId
                                   join reqMas in db.Proc_RequisitionMas on reqDet.Proc_RequisitionMasId equals reqMas.Id
                                   join procItem in db.ProcProjectItem on reqMas.ProcProjectId equals procItem.ProcProjectId
                                   join procProj in db.ProcProject on procItem.ProcProjectId equals procProj.Id
                                   join projSite in db.ProjectSite on procProj.ProjectSiteId equals projSite.Id
                                   join proj in db.Project on projSite.ProjectId equals proj.Id
                                   where purchaseMas.Id == purchaseOrderId
                                   select reqMas).FirstOrDefault();




            if (purchaseItemDet != null)
            {
                ViewBag.ProjectName = purchaseItemDet.ProcProject.ProjectSite.Project.Name;
                ViewBag.SiteName = purchaseItemDet.ProcProject.ProjectSite.Name;
            }

            //ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");


            var vendorAndTenderId = (from purchaseOrderMas in db.Proc_PurchaseOrderMas
                                     join vendors in db.Vendor on purchaseOrderMas.VendorId equals vendors.Id
                                     where purchaseOrderMas.Id == purchaseOrderId
                                     select purchaseOrderMas).FirstOrDefault();
            ViewBag.VendorId = new SelectList(db.Vendor, "Id", "Name", vendorAndTenderId.VendorId);




            ViewBag.TenderId = new SelectList(db.Proc_TenderMas.Where(x => x.Id == tenderMasId), "Id", "TNo", vendorAndTenderId.Proc_TenderMasId);



                ViewBag.PurchaseAddress = purchaseItemDet.ProcProject.ProjectSite.Location;

            var items = (from tenderMas in db.Proc_TenderMas
                         join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                         join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                         join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                         join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                         join procProjectItem in db.ProcProjectItem on procProject.Id equals procProjectItem.ProcProjectId
                         join item in db.Item on procProjectItem.ItemId equals item.Id
                         where tenderDet.VendorId == vendorAndTenderId.VendorId && requisitionDet.ItemId == item.Id
                         select item).Distinct().ToList();

            List<SelectListItem> itemList = new List<SelectListItem>();
            foreach (var x in items)
            {
                //var itemName = db.Proc_TenderMas.SingleOrDefault(m => m.Id == x.Id);
                itemList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            }

            //ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.ItemName = itemList.Distinct();
            ViewBag.UnitName = new SelectList(db.Unit, "Id", "Name");

            var puchaseOrders = db.Proc_PurchaseOrderMas.FirstOrDefault(x => x.Id == purchaseOrderId);

            if (puchaseOrders != null)
            {
                ViewBag.PONo = NullHelper.ObjectToString(puchaseOrders.PONo);
                ViewBag.PODate = NullHelper.DateToString(puchaseOrders.PODate);
                ViewBag.VendorAttention = NullHelper.ObjectToString(puchaseOrders.Attention);
                ViewBag.AttnManager = NullHelper.ObjectToString(puchaseOrders.OrderTo);
                ViewBag.AttnCell = NullHelper.ObjectToString(puchaseOrders.AttnCell);
                ViewBag.Email = NullHelper.ObjectToString(puchaseOrders.AttnEmail);
                ViewBag.Subject = NullHelper.ObjectToString(puchaseOrders.Subject);
                ViewBag.LeadTime = NullHelper.ObjectToString(puchaseOrders.LeadTime);
                ViewBag.Content = NullHelper.ObjectToString(puchaseOrders.Content);
                ViewBag.RcvConcernPerson = NullHelper.ObjectToString(puchaseOrders.RecvConcernPerson);
                ViewBag.RcvConcernPersonCell = NullHelper.ObjectToString(puchaseOrders.RecvConcernPersonCell);
            }


            return View();

        }




        public ActionResult Details(int purchaseOrderId, int tenderMasId)
        {
            var procPurchaseMasterId = db.Proc_PurchaseOrderMas.FirstOrDefault(x => x.Id == purchaseOrderId && x.Proc_TenderMasId == tenderMasId);
            ViewBag.ProcPurchaseMasterId = procPurchaseMasterId.Id;

            var procPurchaseDetId = db.Proc_PurchaseOrderDet.FirstOrDefault(x => x.Proc_PurchaseOrderMasId == purchaseOrderId);
            ViewBag.ProcPurchaseDetId = procPurchaseDetId.Id;


            ViewBag.PurchaseMasId = purchaseOrderId;
            ViewBag.TenderMasId = tenderMasId;


            var purchaseItemDet = (from purchaseDet in db.Proc_PurchaseOrderDet
                                   join purchaseMas in db.Proc_PurchaseOrderMas on purchaseDet.Proc_PurchaseOrderMasId equals purchaseMas.Id
                                   join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
                                   join reqDet in db.Proc_RequisitionDet on purchaseDet.ItemId equals reqDet.ItemId
                                   join reqMas in db.Proc_RequisitionMas on reqDet.Proc_RequisitionMasId equals reqMas.Id
                                   join procItem in db.ProcProjectItem on reqMas.ProcProjectId equals procItem.ProcProjectId
                                   join procProj in db.ProcProject on procItem.ProcProjectId equals procProj.Id
                                   join projSite in db.ProjectSite on procProj.ProjectSiteId equals projSite.Id
                                   join proj in db.Project on projSite.ProjectId equals proj.Id
                                   where purchaseMas.Id == purchaseOrderId
                                   select reqMas).FirstOrDefault();




            if (purchaseItemDet != null)
            {
                ViewBag.ProjectName = purchaseItemDet.ProcProject.ProjectSite.Project.Name;
                ViewBag.SiteName = purchaseItemDet.ProcProject.ProjectSite.Name;
            }

            //ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");


            var vendorAndTenderId = (from purchaseOrderMas in db.Proc_PurchaseOrderMas
                                     join vendors in db.Vendor on purchaseOrderMas.VendorId equals vendors.Id
                                     where purchaseOrderMas.Id == purchaseOrderId
                                     select purchaseOrderMas).FirstOrDefault();
            ViewBag.VendorId = new SelectList(db.Vendor, "Id", "Name", vendorAndTenderId.VendorId);




            ViewBag.TenderId = new SelectList(db.Proc_TenderMas.Where(x => x.Id == tenderMasId), "Id", "TNo", vendorAndTenderId.Proc_TenderMasId);


            ViewBag.PurchaseAddress = purchaseItemDet.ProcProject.ProjectSite.Location;

            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.UnitName = new SelectList(db.Unit, "Id", "Name");

            var puchaseOrders = db.Proc_PurchaseOrderMas.FirstOrDefault(x => x.Id == purchaseOrderId);

            if (puchaseOrders != null)
            {
                ViewBag.PONo = NullHelper.ObjectToString(puchaseOrders.PONo);
                ViewBag.PODate = NullHelper.DateToString(puchaseOrders.PODate);
                ViewBag.VendorAttention = NullHelper.ObjectToString(puchaseOrders.Attention);
                ViewBag.AttnManager = NullHelper.ObjectToString(puchaseOrders.OrderTo);
                ViewBag.AttnCell = NullHelper.ObjectToString(puchaseOrders.AttnCell);
                ViewBag.Email = NullHelper.ObjectToString(puchaseOrders.AttnEmail);
                ViewBag.Subject = NullHelper.ObjectToString(puchaseOrders.Subject);
                ViewBag.LeadTime = NullHelper.ObjectToString(puchaseOrders.LeadTime);
                ViewBag.Content = NullHelper.ObjectToString(puchaseOrders.Content);
                ViewBag.RcvConcernPerson = NullHelper.ObjectToString(puchaseOrders.RecvConcernPerson);
                ViewBag.RcvConcernPersonCell = NullHelper.ObjectToString(puchaseOrders.RecvConcernPersonCell);
            }


            return View();

        }




        [HttpPost]
        public JsonResult GetPId(int ProjectId)
        {
            List<SelectListItem> siteList = new List<SelectListItem>();

            // var projects = db.Project.SingleOrDefault(x => x.Id == ProjectId);
            // var sDate = NullHelper.DateToString(projects.StartDate);
            // var eDate = NullHelper.DateToString(projects.EndDate);

            var tenderProjects = (from tendarMas in db.Proc_TenderMas
                                  join tenderDet in db.Proc_TenderDet on tendarMas.Id equals tenderDet.Proc_TenderMasId
                                  join reqDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals reqDet.Id
                                  join reqMas in db.Proc_RequisitionMas on reqDet.Proc_RequisitionMasId equals reqMas.Id
                                  join procProj in db.ProcProject on reqMas.ProcProjectId equals procProj.Id
                                  join Site in db.ProjectSite on procProj.ProjectSiteId equals Site.Id
                                  where Site.ProjectId == ProjectId && tendarMas.isApproved=="A"
                                  select Site).Distinct().ToList();

            //   var sites = db.ProjectSite.Where(x => x.ProjectId == ProjectId).ToList();


            var projectResources = db.ProjectResource.SingleOrDefault(x => x.ProjectId == ProjectId);
            var projectManager = projectResources.CompanyResource.Name;


            foreach (var x in tenderProjects.Distinct())
            {
                siteList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            }


            var result = new
            {
                //    start = sDate,
                //   end = eDate,
                manager = projectManager,
                Sites = siteList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetVendorId(int TenderId)
        {
            List<SelectListItem> vendorList = new List<SelectListItem>();

            // var projects = db.Project.SingleOrDefault(x => x.Id == ProjectId);
            // var sDate = NullHelper.DateToString(projects.StartDate);
            // var eDate = NullHelper.DateToString(projects.EndDate);


            //getting vendor under one tender
            var getVendorUnderTender = (from tenderMas in db.Proc_TenderMas
                                        join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                                        join vendor in db.Vendor on tenderDet.VendorId equals vendor.Id
                                        where tenderDet.VendorId == vendor.Id && tenderMas.Id == TenderId && tenderDet.Status == "A"
                                        select vendor).ToList();



            foreach (var x in getVendorUnderTender.Distinct())
            {
                vendorList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            }


            var result = new
            {
                Vendors = vendorList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTender(int SiteId)
        {
            var tenders = (from tenderMas in db.Proc_TenderMas
                           join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                           join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                           join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                           join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                           join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                           where site.Id == SiteId
                           select tenderMas).ToList();

            var result = new
            {
                TenderList = tenders
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItems(int VendorId)
        {
            var items = (  from tenderMas in db.Proc_TenderMas
                           join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                           join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                           join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                           join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                           join procProjectItem in db.ProcProjectItem on procProject.Id equals procProjectItem.ProcProjectId
                           join item in db.Item on procProjectItem.ItemId equals item.Id
                           where tenderDet.VendorId == VendorId
                           select item).Distinct().ToList();
            List<SelectListItem> itemList = new List<SelectListItem>();
            foreach (var x in items)
            {
                //var itemName = db.Proc_TenderMas.SingleOrDefault(m => m.Id == x.Id);
                itemList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            }
            var result = new
            {
                ItemList = itemList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSiteLocation(int SiteId)
        {
            var tenders = (from tenderMas in db.Proc_TenderMas
                           join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                           join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                           join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                           join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                           join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                           where site.Id == SiteId && tenderMas.isApproved=="A"
                           select tenderMas).Distinct().ToList();

            List<SelectListItem> tenderList = new List<SelectListItem>();
            foreach (var x in tenders)
            {
               
                tenderList.Add(new SelectListItem { Text = x.TNo, Value = x.Id.ToString() });
            }


            var projectSiteLocations = db.ProjectSite.SingleOrDefault(x => x.Id == SiteId);
            var siteAddress = NullHelper.ObjectToString(projectSiteLocations.Location);

            var projectSiteResources = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == SiteId);
            var siteEngineer = NullHelper.ObjectToString(projectSiteResources.CompanyResource.Name);

            var result = new
            {
                TenderList = tenderList,
                sitesLoc = siteAddress,
                siteEngineer = siteEngineer
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetVendorContactPerson(int VendorId)
        {
            var vendorContactPerson = db.Vendor.SingleOrDefault(x => x.Id == VendorId);
            var vContactPerson = NullHelper.ObjectToString(vendorContactPerson.ContactPerson);

            var items = (from tenderMas in db.Proc_TenderMas
                         join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                         join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                         join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                         join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                         join procProjectItem in db.ProcProjectItem on procProject.Id equals procProjectItem.ProcProjectId
                         join item in db.Item on procProjectItem.ItemId equals item.Id
                         where tenderDet.VendorId == VendorId && requisitionDet.ItemId == item.Id
                         select item).Distinct().ToList();
            List<SelectListItem> itemList = new List<SelectListItem>();
            foreach (var x in items)
            {
                //var itemName = db.Proc_TenderMas.SingleOrDefault(m => m.Id == x.Id);
                itemList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            }

            var result = new
            {
                ItemList = itemList,
                vContactPerson = vContactPerson
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public JsonResult GetSelectedItemInfo(int ItemId)
        {

            var unit = (from procProjectItem in db.ProcProjectItem
                        join units in db.Unit on procProjectItem.UnitId equals units.Id
                        where procProjectItem.ItemId == ItemId
                        select units).FirstOrDefault();



            var price = (from tenderDet in db.Proc_TenderDet
                         join procReqDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals procReqDet.Id
                         join procItem in db.ProcProjectItem on procReqDet.ItemId equals procItem.ItemId
                         where procItem.ItemId == ItemId
                         select tenderDet).FirstOrDefault();

            //old worked
            //var size = (from procReqDet in db.Proc_RequisitionDet
            //            join items in db.Item on procReqDet.ItemId equals items.Id
            //            where procReqDet.ItemId == ItemId
            //            select procReqDet).FirstOrDefault();

            var size = (from procReqDet in db.Proc_RequisitionDet
                        join items in db.Item on procReqDet.ItemId equals items.Id
                        join procItem in db.ProcProjectItem on items.Id equals procItem.ItemId
                        where procReqDet.ItemId == ItemId
                        select procReqDet).FirstOrDefault();


            var result = new
            {
                flag = true,
                Unit = unit.Name,
                UnitId = unit.Id,
                Size = size.Size,
                UnitPrice = price.TQPrice
            };

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Create()
        {
            //ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            //ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");
            //var procprojects = db.ProcProject.ToList();
            //List<ProjectSite> sites = new List<ProjectSite>();
            //foreach (var i in procprojects)
            //{
            //    var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.ProjectSiteId);
            //    sites.Add(site);
            //}

            //List<Project> projects = new List<Project>();
            //foreach (var i in sites)
            //{
            //    var proj = db.Project.FirstOrDefault(x => x.Id == i.ProjectId);
            //    projects.Add(proj);
            //}


            //var tenderProjects = (from tendarMas in db.Proc_TenderMas
            //                      join tenderDet in db.Proc_TenderDet on tendarMas.Id equals tenderDet.Proc_TenderMasId
            //                      join reqDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals reqDet.Id
            //                      join procItem in db.ProcProjectItem on reqDet.ItemId equals procItem.ItemId
            //                      join procProj in db.ProcProject on procItem.ProcProjectId equals procProj.Id
            //                      join projSite in db.ProjectSite on procProj.ProjectSiteId equals projSite.Id
            //                      join proj in db.Project on projSite.ProjectId equals proj.Id
            //                      where procProj.ProjectSite.ProjectId == proj.Id && tendarMas.isApproved=="A"
            //                      select projSite).ToList();

            var tenderProjects = (from tendarMas in db.Proc_TenderMas
                                  join tenderDet in db.Proc_TenderDet on tendarMas.Id equals tenderDet.Proc_TenderMasId
                                  join reqDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals reqDet.Id
                                  join reqMas in db.Proc_RequisitionMas on reqDet.Proc_RequisitionMasId equals reqMas.Id
                                  join procProj in db.ProcProject on reqMas.ProcProjectId equals procProj.Id
                                  join Site in db.ProjectSite on procProj.ProjectSiteId equals Site.Id
                                  join proj in db.Project on Site.ProjectId equals proj.Id
                                  where procProj.ProjectSiteId == Site.Id && tendarMas.isApproved == "A"
                                  select Site).ToList();

            List<ProjectSite> sites = new List<ProjectSite>();
            foreach (var i in tenderProjects)
            {
                //var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.ProjectSiteId);
                var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.Id);
                sites.Add(site);
            }

            List<Project> projects = new List<Project>();
            foreach (var i in sites)
            {
                var proj = db.Project.FirstOrDefault(x => x.Id == i.ProjectId);
                projects.Add(proj);
            }



            //var projList = tenderProjects.ProjectSite.Id;
            //var siteList = tenderProjects.ProjectSiteId;

            ViewBag.ProjectId = new SelectList(projects.Distinct(), "Id", "Name");
            ViewBag.SiteId = new SelectList(sites, "Id", "Name");

            ViewBag.VendorId = new SelectList(db.Vendor, "Id", "Name");

            var tenderIdUnderProjects = (from tendarMas in db.Proc_TenderMas
                                         join tenderDet in db.Proc_TenderDet on tendarMas.Id equals tenderDet.Proc_TenderMasId
                                         join reqDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals reqDet.Id
                                         join procItem in db.ProcProjectItem on reqDet.ItemId equals procItem.ItemId
                                         join procProj in db.ProcProject on procItem.ProcProjectId equals procProj.Id
                                         join projSite in db.ProjectSite on procProj.ProjectSiteId equals projSite.Id
                                         join proj in db.Project on projSite.ProjectId equals proj.Id
                                         where procProj.ProjectSite.ProjectId == proj.Id && tendarMas.isApproved == "A"
                                         select tendarMas).ToList();

            List<Proc_TenderMas> sitesTender = new List<Proc_TenderMas>();

            foreach (var i in tenderIdUnderProjects)
            {
                //var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.ProjectSiteId);
                var siteTender = db.Proc_TenderMas.FirstOrDefault(x => x.Id == i.Id);
                sitesTender.Add(siteTender);
            }



            ViewBag.TenderId = new SelectList(sitesTender.Distinct(), "Id", "TNo");

            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.UnitName = new SelectList(db.Unit, "Id", "Name");


            return View();
        }


        [HttpPost]
        public JsonResult PurchaseCreateOrder(IEnumerable<VMPurchaseOrder> AddedDetItems, string PONo, DateTime PODate, int VendorId, int TenderId, string VContactPerson, string ProjectManager, string AttnCell, string Email, string Subject, int LeadTime, string Content, string RcvConcenPerson, string RcvPersonCell, decimal POTotalAmt)
        {
            var result = new
            {
                flag = false,
                message = "Purchase order saving error !"
            };
            var flag = false;




            var orderList = db.Proc_PurchaseOrderMas.Where(x => x.PONo.Trim() == PONo.Trim()).ToList();
            if (orderList.Count == 0)
            {
                flag = false;
                Proc_PurchaseOrderMas master = new Proc_PurchaseOrderMas();
                master.Proc_TenderMasId = TenderId;
                master.PONo = PONo;
                master.PODate = PODate;
                master.VendorId = VendorId;
                master.LeadTime = LeadTime;
                master.OrderTo = ProjectManager;
                master.Attention = VContactPerson;
                master.AttnCell = AttnCell;
                master.AttnEmail = Email;
                master.Subject = Subject;
                master.Content = Content;
                master.RecvConcernPerson = RcvConcenPerson;
                master.RecvConcernPersonCell = RcvPersonCell;
                master.POTotalAmt = POTotalAmt;

                db.Proc_PurchaseOrderMas.Add(master);
                //db.SaveChanges();
                flag = db.SaveChanges() > 0;

                var PurchaseOrderId = master.Id;
                foreach (var item in AddedDetItems)
                {
                    Proc_PurchaseOrderDet detail = new Proc_PurchaseOrderDet();


                    detail.Proc_PurchaseOrderMasId = PurchaseOrderId;
                    detail.ItemId = item.ItemId;
                    detail.POQty = item.POQuantity;
                    detail.POAmt = item.TotalPrice;


                    db.Proc_PurchaseOrderDet.Add(detail);
                    //db.SaveChanges();
                    flag = db.SaveChanges() > 0;
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




        public JsonResult PurchaseUpdate(IEnumerable<VMPurchaseOrderEdit> AddedDetItems, int ProcPurchaseMasterId, int ProcPurchaseDetId, string PONo, DateTime PODate, int VendorId, int TenderId, string VendorAttention, string AttnManager, string AttnCell, string Email, string Subject, int LeadTime, string Content, string RcvConcernPerson, string RcvConcernPersonCell, decimal POTotalAmt)
        {
            var result = new
            {
                flag = false,
                message = "Updating error !"
            };
            var flag = false;

            var master = db.Proc_PurchaseOrderMas.Find(ProcPurchaseMasterId);
            master.Proc_TenderMasId = TenderId;
            master.PONo = PONo;
            master.PODate = PODate;
            master.VendorId = VendorId;
            master.OrderTo = VendorAttention;
            master.Attention = AttnManager;
            master.AttnCell = AttnCell;
            master.AttnEmail = Email;
            master.Subject = Subject;
            master.LeadTime = LeadTime;
            master.Content = Content;
            master.RecvConcernPerson = RcvConcernPerson;
            master.RecvConcernPersonCell = RcvConcernPersonCell;
            master.POTotalAmt = POTotalAmt;

            db.Entry(master).State = EntityState.Modified;
            flag = db.SaveChanges() > 0;
            var PurchaseMasId = master.Id;

            if (flag)
            {
                foreach (var item in AddedDetItems)
                {
                    var check = db.Proc_PurchaseOrderDet.FirstOrDefault(x => x.Proc_PurchaseOrderMasId == PurchaseMasId /*&& x.Id== ProcPurchaseDetId*/);
                    if (check == null)
                    {
                        Proc_PurchaseOrderDet detail = new Proc_PurchaseOrderDet();
                        detail.ItemId = item.ItemId;
                        detail.Proc_PurchaseOrderMasId = item.ProcPurchaseMasterId;
                        detail.POAmt = item.TotalPrice;
                        detail.POQty = item.POQuantity;
                        db.Entry(detail).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    else
                    {
                        var Proc_PurchaseDet_Id = db.Proc_PurchaseOrderDet.FirstOrDefault(x => x.Proc_PurchaseOrderMasId == PurchaseMasId);
                        var getItem = db.Proc_PurchaseOrderDet.Find(Proc_PurchaseDet_Id.Id);
                        getItem.POAmt = item.TotalPrice;
                        getItem.POQty = item.POQuantity;
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














        public JsonResult DeletePurchaseOrders(int PurchaseOrderId)
        {
            bool flag = false;
            try
            {
                var itemsToDeleteDetails = db.Proc_PurchaseOrderDet.Where(x => x.Proc_PurchaseOrderMasId == PurchaseOrderId);
                db.Proc_PurchaseOrderDet.RemoveRange(itemsToDeleteDetails);
                db.SaveChanges();
                var itemsToDeleteMaster = db.Proc_PurchaseOrderMas.Where(x => x.Id == PurchaseOrderId);
                db.Proc_PurchaseOrderMas.RemoveRange(itemsToDeleteMaster);


                flag = db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

            }

            if (flag)
            {
                var result = new
                {
                    flag = true,
                    message = "Purchase Order deletion successful."
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Purchase Order deletion failed!\nError Occured."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }





        //21jan18
        public ActionResult DeletePurchaseDetailItem(int PurchaseOrderDetId)
        {
            var flag = false;
            var result = new
            {
                flag = false,
                message = "Delete error !"
            };

            // Proc_PurchaseOrderDet data = db.Proc_PurchaseOrderDet.Find(PurchaseOrderDetId);
            var checkPurchaseOrderDet = db.Proc_MaterialEntryDet.Where(x => x.Proc_PurchaseOrderDetId == PurchaseOrderDetId).FirstOrDefault();

            if (checkPurchaseOrderDet == null)
            {
                var data = db.Proc_PurchaseOrderDet.Where(x => x.Id == PurchaseOrderDetId).FirstOrDefault();
                db.Proc_PurchaseOrderDet.Remove(data);
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

            else
            {
                result = new
                {
                    flag = false,
                    message = "Please delete Material Entry first!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }
        //21jan end






    }
}