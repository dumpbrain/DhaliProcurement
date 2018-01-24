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
    public class TenderQuotationController : Controller
    {
        private DCPSContext db = new DCPSContext();

        public ActionResult Index(int? TenderId)
        {
            ViewBag.TenderId = new SelectList(db.Proc_TenderMas, "Id", "TNo");
            var TenderList = db.Proc_TenderMas.ToList();

            if (TenderId != null)
            {
                TenderList = TenderList.Where(x => x.Id == TenderId).ToList();
                ViewBag.TenderNo = new SelectList(db.Proc_TenderMas.Where(y => y.Id == TenderId), "Id", "TNo");
                return View(TenderList);
            }

            else
            {
                return View(TenderList);
            }



        }

        public ActionResult Create()
        {
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
            var requisitionProjects = (from requisitionMas in db.Proc_RequisitionMas
                                       join procproject in db.ProcProject on requisitionMas.ProcProjectId equals procproject.Id
                                       //join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                                       //join project in db.Project on site.ProjectId equals project.Id
                                       where requisitionMas.ProcProjectId == procproject.Id && requisitionMas.Status == "A"
                                       select procproject).ToList();

            List<ProjectSite> sites = new List<ProjectSite>();

            foreach (var i in requisitionProjects)
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
            //ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            //ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");
            ViewBag.ProjectId = new SelectList(projects.Distinct(), "Id", "Name");
            ViewBag.SiteId = new SelectList(sites, "Id", "Name");

            ViewBag.RCode = new SelectList(db.Proc_RequisitionMas, "Id", "Rcode");
            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendor, "Id", "Name");
            return View();

        }

        public ActionResult Edit(int TenderId)
        {

            ViewBag.TenderDetPrimaryKey = db.Proc_TenderDet.Max(x => x.Id);
            ViewBag.TenderId = TenderId;
            VMTenderMasterDetail vm = new VMTenderMasterDetail();
            vm.proc_TenderMas = db.Proc_TenderMas.SingleOrDefault(x => x.Id == TenderId);
            List<Proc_TenderDet> tenderList = new List<Proc_TenderDet>();
            var TenderDetails = db.Proc_TenderDet.Where(x => x.Proc_TenderMasId == TenderId).ToList();
            foreach (var item in TenderDetails)
            {
                tenderList.Add(item);
            }
            vm.proc_TenderDet = tenderList;

            //ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            //ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");
            var requisitionProjects = (from requisitionMas in db.Proc_RequisitionMas
                                       join procproject in db.ProcProject on requisitionMas.ProcProjectId equals procproject.Id
                                       where requisitionMas.ProcProjectId == procproject.Id && requisitionMas.Status == "A"
                                       select procproject).ToList();

            List<ProjectSite> sites = new List<ProjectSite>();

            foreach (var i in requisitionProjects)
            {
                var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.ProjectSiteId);
                sites.Add(site);
            }

            List<Project> projects = new List<Project>();

            foreach (var i in sites.Distinct())
            {
                var proj = db.Project.FirstOrDefault(x => x.Id == i.ProjectId);
                projects.Add(proj);
            }
            ViewBag.ProjectId = new SelectList(projects.Distinct(), "Id", "Name");
            ViewBag.SiteId = new SelectList(sites, "Id", "Name");

            ViewBag.RCode = new SelectList(db.Proc_RequisitionMas, "Id", "Rcode");
            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendor, "Id", "Name");
            return View(vm);
        }

        public ActionResult Details(int TenderId)
        {
            ViewBag.TenderId = TenderId;
            VMTenderMasterDetail vm = new VMTenderMasterDetail();
            vm.proc_TenderMas = db.Proc_TenderMas.SingleOrDefault(x => x.Id == TenderId);
            List<Proc_TenderDet> tenderList = new List<Proc_TenderDet>();
            var TenderDetails = db.Proc_TenderDet.Where(x => x.Proc_TenderMasId == TenderId).ToList();
            foreach (var item in TenderDetails)
            {
                tenderList.Add(item);
            }
            vm.proc_TenderDet = tenderList;

            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");
            ViewBag.RCode = new SelectList(db.Proc_RequisitionMas, "Id", "Rcode");
            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendor, "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        public JsonResult GetSites(int ProjectId)
        {
            //List<SelectListItem> siteList = new List<SelectListItem>();

            //var projects = db.Project.SingleOrDefault(x => x.Id == ProjectId);
            //var sites = db.ProjectSite.Where(x => x.ProjectId == ProjectId).ToList();

            //foreach (var x in sites)
            //{
            //    siteList.Add(new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
            //}

            var requisitionProjects = (from requisitionMas in db.Proc_RequisitionMas
                                       join procproject in db.ProcProject on requisitionMas.ProcProjectId equals procproject.Id
                                       join site in db.ProjectSite on procproject.ProjectSiteId equals site.Id
                                       join project in db.Project on site.ProjectId equals project.Id
                                       //where requisitionMas.ProcProjectId == procproject.Id
                                       where project.Id == ProjectId && requisitionMas.Status == "A"
                                       select site).Distinct().ToList();

            List<SelectListItem> siteList = new List<SelectListItem>();

            foreach (var i in requisitionProjects)
            {
                var site = db.ProjectSite.FirstOrDefault(x => x.Id == i.Id);
                siteList.Add(new SelectListItem { Text = i.Name, Value = i.Id.ToString() });
            }

            var result = new
            {
                Sites = siteList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetReqNo(int SiteId, int ProjectId)
        {
            List<SelectListItem> ReqList = new List<SelectListItem>();

            var requisitions = (from requisitionDet in db.Proc_RequisitionDet
                                join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                                join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                                join project in db.Project on site.ProjectId equals project.Id
                                where project.Id == ProjectId && site.Id == SiteId && requisitionMas.Status == "A"
                                select requisitionMas).ToList().Distinct();

            foreach (var x in requisitions)
            {
                ReqList.Add(new SelectListItem { Text = x.Rcode, Value = x.Rcode.ToString() });
            }

            var result = new
            {
                Items = ReqList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetReqitem(string Rcode)
        {
            List<SelectListItem> ItemList = new List<SelectListItem>();

            var items = (from requisitionDet in db.Proc_RequisitionDet
                         join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                         join reqItem in db.Item on requisitionDet.ItemId equals reqItem.Id
                         where requisitionMas.Rcode == Rcode
                         select reqItem).ToList().Distinct();

            foreach (var x in items)
            {
                var itemName = db.Item.SingleOrDefault(m => m.Id == x.Id);
                ItemList.Add(new SelectListItem { Text = itemName.Name, Value = x.Id.ToString() });
            }

            var ReqmasterId = db.Proc_RequisitionMas.SingleOrDefault(x => x.Rcode == Rcode);
            var PRequisitionDetId = db.Proc_RequisitionDet.FirstOrDefault(x => x.Proc_RequisitionMasId == ReqmasterId.Id);

            var result = new
            {
                Items = ItemList,
                Proc_RequisitionDetId = PRequisitionDetId.Id
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetUnit(int itemId, string Rcode)
        {
            //var unit = db.Unit.SingleOrDefault(x => x.Id == itemId);
            var unit = (from procProjectItem in db.ProcProjectItem
                        join units in db.Unit on procProjectItem.UnitId equals units.Id
                        where procProjectItem.ItemId == itemId
                        select units).FirstOrDefault();

            var items = (from requisitionDet in db.Proc_RequisitionDet
                         join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                         join reqItem in db.Item on requisitionDet.ItemId equals reqItem.Id
                         where requisitionMas.Rcode == Rcode && requisitionDet.ItemId == itemId
                         select requisitionDet).FirstOrDefault();

            var result = new
            {
                flag = true,
                UnitName = unit.Name,
                UnitId = unit.Id,
                Qty = items.ReqQty,
                Proc_RequisitionDetId = items.Id
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult CreateTenderQuotation(IEnumerable<VMTenderItem> TenderItems, string TNo, string TDate, string Specs, string Remarks)
        {
            var result = new
            {
                flag = false,
                message = "Requisition saving error !"
            };
            var flag = false;
            var TenderDate = DateTime.ParseExact(TDate, "dd/mm/yyyy", CultureInfo.CurrentCulture);
            var tenderList = db.Proc_TenderMas.Where(x => x.TNo.Trim() == TNo.Trim()).ToList();
            if (tenderList.Count == 0)
            {
                flag = false;
                Proc_TenderMas master = new Proc_TenderMas();

                master.TNo = TNo;
                master.TDate = TenderDate;
                master.Specs = Specs;
                master.Remarks = Remarks;
                master.isApproved = "N";
                db.Proc_TenderMas.Add(master);

                flag = db.SaveChanges() > 0;
                //var getReqId = db.Proc_RequisitionMas.SingleOrDefault(x => x.Rcode == ReqNo);
                var TenderMasterId = master.Id;

                foreach (var item in TenderItems)
                {
                    Proc_TenderDet detail = new Proc_TenderDet();
                    detail.Proc_TenderMasId = TenderMasterId;
                    detail.Proc_RequisitionDetId = item.Proc_RequisitionDetId;
                    detail.VendorId = item.VendorId;
                    //detail.TQDate = item.TQDate;
                    if (item.TQDate == null)
                    {
                        detail.TQDate = null;
                    }
                    else
                    {
                        //detail.ChallanDate = DateTime.ParseExact(item.ChallanDate, "dd/mm/yyyy", CultureInfo.CurrentCulture);
                        detail.TQDate = DateTime.ParseExact(item.TQDate, "dd/mm/yyyy", CultureInfo.CurrentCulture);
                    }
                    detail.TQNo = item.TQNo;
                    detail.TQPrice = item.TQPrice;
                    detail.Status = item.Status;

                    db.Proc_TenderDet.Add(detail);
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

        public JsonResult DeleteTenderQuotation(int TenderQuotarionMasId)
        {
            var TenderCount = (from tenderMas in db.Proc_TenderMas
                                   //join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                               join purchaseMas in db.Proc_PurchaseOrderMas on tenderMas.Id equals purchaseMas.Proc_TenderMasId
                               where tenderMas.Id == TenderQuotarionMasId
                               select tenderMas).Distinct().Count();


            if (TenderCount == 0)
            {
                bool flag = false;
                try
                {

                    var itemsToDeletePlan = db.Proc_TenderDet.Where(x => x.Proc_TenderMasId == TenderQuotarionMasId);
                    db.Proc_TenderDet.RemoveRange(itemsToDeletePlan);
                    db.SaveChanges();

                    var itemsToDeleteTask = db.Proc_TenderMas.Where(x => x.Id == TenderQuotarionMasId);
                    db.Proc_TenderMas.RemoveRange(itemsToDeleteTask);


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
                        message = "Tender deletion successful."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = new
                    {
                        flag = false,
                        message = "Tender deletion failed!\nError Occured."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Tender deletion failed!\nDelete purchase order first."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult getEditTender(int TenderId)
        {

            bool flag = false;

            var check = db.Proc_TenderDet.Where(x => x.Proc_TenderMasId == TenderId).Count();
            if (check > 0)
            {
                flag = true;
                var data = db.Proc_TenderDet.Where(x => x.Proc_TenderMasId == TenderId).ToList();

                List<VMEditTenderItem> detailsList = new List<VMEditTenderItem>();
                foreach (var i in data)
                {
                    VMEditTenderItem vm = new VMEditTenderItem();
                    vm.TenderDetailId = i.Id;

                    var projectId = (from tenderDet in db.Proc_TenderDet
                                     join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                                     join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                     join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                                     join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                                     join project in db.Project on site.ProjectId equals project.Id
                                     where tenderDet.Proc_TenderMasId == i.Proc_TenderMasId
                                     select project).FirstOrDefault();
                    vm.ProjectId = projectId.Id;
                    vm.ProjectName = projectId.Name;

                    var siteId = (from tenderDet in db.Proc_TenderDet
                                  join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                                  join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                  join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                                  join site in db.ProjectSite on procProject.ProjectSiteId equals site.Id
                                  where tenderDet.Proc_TenderMasId == TenderId
                                  select site).FirstOrDefault();
                    vm.SiteId = siteId.Id;
                    vm.SitetName = siteId.Name;

                    var requisitionId = (from tenderDet in db.Proc_TenderDet
                                         join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                                         join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                         where tenderDet.Proc_TenderMasId == TenderId
                                         select requisitionMas).FirstOrDefault();

                    vm.RCode = requisitionId.Rcode;

                    var ItemId = (from tenderDet in db.Proc_TenderDet
                                  join requisitionDet in db.Proc_RequisitionDet on tenderDet.Proc_RequisitionDetId equals requisitionDet.Id
                                  join requisitionMas in db.Proc_RequisitionMas on requisitionDet.Proc_RequisitionMasId equals requisitionMas.Id
                                  join procProject in db.ProcProject on requisitionMas.ProcProjectId equals procProject.Id
                                  join procProjectItem in db.ProcProjectItem on procProject.Id equals procProjectItem.ProcProjectId
                                  join items in db.Item on procProjectItem.ItemId equals items.Id
                                  where tenderDet.Proc_TenderMasId == TenderId && requisitionDet.Id == i.Proc_RequisitionDetId && items.Id == requisitionDet.ItemId
                                  select items).FirstOrDefault();
                    vm.ItemId = ItemId.Id;
                    vm.ItemName = ItemId.Name;


                    var quantity = db.Proc_RequisitionDet.FirstOrDefault(x => x.ItemId == ItemId.Id).ReqQty;
                    vm.Qty = quantity;

                    //var unit = db.Unit.FirstOrDefault(x => x.Id == ItemId.Id);
                    var unit = (from procProjectItem in db.ProcProjectItem
                                join units in db.Unit on procProjectItem.UnitId equals units.Id
                                where procProjectItem.ItemId == ItemId.Id
                                select units).FirstOrDefault();

                    vm.UnitId = unit.Id;
                    vm.UnitName = unit.Name;
                    vm.Proc_RequisitionDetId = i.Proc_RequisitionDetId;
                    vm.VendorId = i.VendorId;
                    vm.VendorName = db.Vendor.SingleOrDefault(x => x.Id == i.VendorId).Name;
                    vm.TQNo = i.TQNo;

                    if (i.TQDate == null)
                    {
                        vm.TQDate = "";
                    }
                    else
                    {
                        vm.TQDate = NullHelper.DateToString(i.TQDate);
                    }
                    //vm.TQDate = i.TQDate;
                    vm.TQPrice = i.TQPrice;
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


        public JsonResult EditTender(IEnumerable<VMTenderItem> TenderItems, int?[] DeleteItems, int TenderId, string TNo, string TDate, string Specs, string TenderRemarks)
        {
            var result = new
            {
                flag = false,
                message = "Tender saving error !"
            };

            if (DeleteItems != null)
            {
                foreach (var i in DeleteItems)
                {
                    var delteItem = db.Proc_TenderDet.Find(i);
                    db.Proc_TenderDet.Remove(delteItem);
                    db.SaveChanges();

                }
            }



            var flag = false;
            var master = db.Proc_TenderMas.Find(TenderId);
            master.TNo = TNo;
            master.TDate = DateTime.ParseExact(TDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            master.Specs = Specs;
            master.Remarks = TenderRemarks;
            master.isApproved = "N";
            db.Entry(master).State = EntityState.Modified;
            flag = db.SaveChanges() > 0;
            var TenderMasId = master.Id;

            if (flag)
            {
                foreach (var item in TenderItems)
                {
                    var check = db.Proc_TenderDet.FirstOrDefault(x => x.Proc_TenderMasId == TenderMasId && x.Proc_RequisitionDetId == item.Proc_RequisitionDetId && x.VendorId == item.VendorId);
                    //var check = db.Proc_TenderDet.FirstOrDefault(x => x.Id==item.TenderDetailId);
                    if (check == null)
                    {
                        Proc_TenderDet detail = new Proc_TenderDet();
                        detail.Proc_TenderMasId = TenderMasId;
                        detail.Proc_RequisitionDetId = item.Proc_RequisitionDetId;
                        detail.VendorId = item.VendorId;
                        if (item.TQDate == null)
                        {
                            detail.TQDate = null;
                        }
                        else
                        {
                            detail.TQDate = DateTime.ParseExact(item.TQDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        }
                        //detail.TQDate = item.TQDate;
                        detail.TQNo = item.TQNo;
                        detail.TQPrice = item.TQPrice;
                        detail.Status = item.Status;
                        db.Entry(detail).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    else
                    {

                        //var Proc_TenderDet_Id = db.Proc_TenderDet.FirstOrDefault(x => x.Proc_RequisitionDetId == item.Proc_RequisitionDetId && x.Proc_TenderMasId == TenderMasId);
                        //var getItem = db.Proc_TenderDet.Find(Proc_TenderDet_Id.Id);
                        var getItem = db.Proc_TenderDet.Find(item.TenderDetailId);
                        if (item.TQDate == null)
                        {
                            getItem.TQDate = null;
                        }
                        else
                        {
                            getItem.TQDate = DateTime.ParseExact(item.TQDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        }
                        //getItem.TQDate = item.TQDate;
                        getItem.TQNo = item.TQNo;
                        getItem.TQPrice = item.TQPrice;
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

        public ActionResult PendingTender()
        {
            var data = db.Proc_TenderMas.Where(y => y.isApproved.Trim() != "A").ToList();

            //var data = (from status in db.ProjectSiteStatus
            //            join siteTask in db.ProjectSitePlanTask on status.ProjectSitePlanTaskId equals siteTask.Id
            //            join plan in db.ProjectSitePlan on siteTask.ProjectSitePlanId equals plan.Id
            //            join site in db.ProjectSite on plan.ProjectSiteId equals site.Id
            //            join project in db.Project on site.ProjectId equals project.Id
            //            where status.IsAuth != true
            //            select (new VMPendingStatus
            //            {
            //                ProjectName = project.Name,
            //                SiteName = site.Name,
            //                InputBy = db.User.Where(x => x.Id == status.InsertedBy).FirstOrDefault().UserName,
            //                PlanId = plan.Id,
            //                StatusDate = status.SiteStatusDate
            //            })).Distinct();

            //new VMPlanTaskDispPrevStat
            //{
            //    ConId = x.
            //});

            return View(data);
            //return View(data);
        }

        public JsonResult ApproveTender(IEnumerable<VMTenderItem> TenderItems, int TenderMasId, string isApproved) {
            var flag = false;
            var result = new
            {
                flag = false,
                message = "Tender approve error !"
            };

            var master = db.Proc_TenderMas.Find(TenderMasId);
            master.isApproved = isApproved;
            db.Entry(master).State = EntityState.Modified;
            flag = db.SaveChanges() > 0;

            //var TMasId = master.Id;

            if (flag)
            {
                foreach (var item in TenderItems)
                {
                    var check = db.Proc_TenderDet.FirstOrDefault(x => x.Proc_TenderMasId == TenderMasId && x.Proc_RequisitionDetId == item.Proc_RequisitionDetId && x.VendorId == item.VendorId);
                    //var check = db.Proc_TenderDet.FirstOrDefault(x => x.Id==item.TenderDetailId);
                    if (check == null)
                    {
                        Proc_TenderDet detail = new Proc_TenderDet();
                        detail.Proc_TenderMasId = TenderMasId;
                        detail.Proc_RequisitionDetId = item.Proc_RequisitionDetId;
                        detail.VendorId = item.VendorId;
                        if (item.TQDate == null)
                        {
                            detail.TQDate = null;
                        }
                        else
                        {
                            detail.TQDate = DateTime.ParseExact(item.TQDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        }
                        //detail.TQDate = item.TQDate;
                        detail.TQNo = item.TQNo;
                        detail.TQPrice = item.TQPrice;
                        detail.Status = item.Status;
                        db.Entry(detail).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    else
                    {

                        var Proc_TenderDet_Id = db.Proc_TenderDet.FirstOrDefault(x => x.Proc_RequisitionDetId == item.Proc_RequisitionDetId && x.Proc_TenderMasId == TenderMasId && x.VendorId == item.VendorId);
                        var getItem = db.Proc_TenderDet.Find(Proc_TenderDet_Id.Id);
                        if (item.TQDate == null)
                        {
                            getItem.TQDate = null;
                        }
                        else
                        {
                            getItem.TQDate = DateTime.ParseExact(item.TQDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        }
                        //getItem.TQDate = item.TQDate;
                        getItem.TQNo = item.TQNo;
                        getItem.TQPrice = item.TQPrice;
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
                    message = "Approve Successful!"
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult DeleteTenderDetailItem(int TenderDetailId)
        //{
        //    var flag = false;
        //    var result = new
        //    {
        //        flag = false,
        //        message = "Delete error !"
        //    };

        //    var TenderMasId = db.Proc_TenderDet.SingleOrDefault(x => x.Id == TenderDetailId).Proc_TenderMasId;
        //    var check = db.Proc_PurchaseOrderDet.Where(x => x.Proc_PurchaseOrderMas.Proc_TenderMasId == TenderMasId).ToList();
        //    if (check.Count == 0)
        //    {
        //        var data = db.Proc_TenderDet.Where(x => x.Id == TenderDetailId).FirstOrDefault();
        //        db.Proc_TenderDet.Remove(data);
        //        flag = db.SaveChanges() > 0;
        //        //return RedirectToAction("Edit", "Projects", new { id = projectId });
        //        if (flag == true)
        //        {
        //            result = new
        //            {
        //                flag = true,
        //                message = "Delete Successful Successful!"
        //            };
        //        }

        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        result = new
        //        {
        //            flag = false,
        //            message = "This tender has been used in purchase order. Please delete PO first!"
        //        };

        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }

        //}



        public ActionResult DeleteTenderDetailItem(int TenderDetailId)
        {

            var result = new
            {
                flag = false,
                message = "Delete error !"
            };

            var check = (from purchaseMas in db.Proc_PurchaseOrderMas
                               join purchaseDet in db.Proc_PurchaseOrderDet on purchaseMas.Id equals purchaseDet.Proc_PurchaseOrderMasId
                               join tenderMas in db.Proc_TenderMas on purchaseMas.Proc_TenderMasId equals tenderMas.Id
                               join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                               where tenderDet.Id == TenderDetailId && tenderDet.VendorId == purchaseMas.VendorId
                               select tenderDet).Distinct().ToList();

            if (check.Count == 0)
            {
                result = new
                {
                    flag = true,
                    message = "Delete Successful!"
                };

            }
            else
            {
                result = new
                {
                    flag = false,
                    message = "Delete Failed! This item has been used in purchase order!"
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
    
        
    }
}