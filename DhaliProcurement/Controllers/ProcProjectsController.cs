using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DhaliProcurement.Models;
using DhaliProcurement.ViewModel;
using DhaliProcurement.Helper;
using System.Net;
using System.Data.Entity;

namespace DhaliProcurement.Controllers
{
    [Authorize]
    public class ProcProjectsController : Controller
    {

        private DCPSContext db = new DCPSContext();


        public ActionResult Index(int? ProjectId, int? SiteId)
        {

            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");

            var projectsSites = db.ProcProject.ToList();


            if (ProjectId != null && SiteId != null)
            {

                projectsSites = projectsSites.Where(x => x.Id == ProjectId && x.ProjectSiteId == SiteId).ToList();
                ViewBag.SiteId = new SelectList(db.ProjectSite.Where(y => y.ProjectId == ProjectId), "Id", "Name");
                return View(projectsSites);

            }
            else if (ProjectId != null)
            {
                projectsSites = projectsSites.Where(x => x.Id == ProjectId).ToList();
                ViewBag.SiteId = new SelectList(db.ProjectSite.Where(y => y.ProjectId == ProjectId), "Id", "Name");
                return View(projectsSites);
            }

            else
            {
                return View(projectsSites.ToList());
            }
            //return View();
        }


        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");

            // ViewBag.PrManId = new SelectList(db.CompanyResource, "Id", "Name");
            // ViewBag.StEngId = new SelectList(db.CompanyResource, "Id", "Name");

            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.Unit = new SelectList(db.Unit, "Id", "Name");

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
        public JsonResult SaveProject(int ProjectId, int SiteId)
        {
            var result = new
            {
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
        public JsonResult ItemCreate(List<DemoVM> ResourceDetails, int ProjectSiteId, DateTime? BOQDate, string BOQNo, DateTime? NOADate, string NOANo, string ProjectType, string ProjectStatus, string ProjectRemarks)
        {
            var result = new
            {
                flag = false,
                message = "Saving failed"
            };

            ProcProject procProject = new ProcProject();
            //procProject.Id = ProjectId;
            procProject.ProjectSiteId = ProjectSiteId;


            procProject.BOQDate = BOQDate;
            procProject.BOQNo = BOQNo;
            procProject.NOADate = NOADate;
            procProject.NOANo = NOANo;
            procProject.Status = ProjectStatus;
            procProject.Remarks = ProjectRemarks;
            procProject.ProjectType = ProjectType;

            db.ProcProject.Add(procProject);
            db.SaveChanges();

            foreach (var item in ResourceDetails)
            {

                ProcProjectItem procitem = new ProcProjectItem();

                //  var checkeingDuplicate= db.ProcProjectItem.Where(x => x.ProjectId == item.ProjectId && x.ProjectSiteId == item.ProjectSiteId && x.ItemISLNO == item.ItemISLNO);

                var procProjId = db.ProcProject.SingleOrDefault(x => x.ProjectSiteId == ProjectSiteId);
                procitem.ProcProjectId = procProjId.Id;
                //  procitem.ProjectSiteId = item.ProjectSiteId;

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



        [HttpPost]
        public JsonResult DeleteProjectItem(int id, int ProcProjectId)
        {
            var result = new
            {
                flag = false,
                message = "Error occured !!"
            };

            try
            {

                var procItemList = db.ProcProjectItem.SingleOrDefault(x => x.ItemId == id && x.ProcProjectId == ProcProjectId);
                if (procItemList != null)
                {
                    ProcProjectItem procItem = db.ProcProjectItem.Find(procItemList.Id);
                    db.ProcProjectItem.Remove(procItem);
                    db.SaveChanges();

                    result = new
                    {
                        flag = true,
                        message = "Delete successful !!"
                    };
                }
                else
                {
                    result = new
                    {
                        flag = false,
                        message = "Delete failed!"
                    };
                }

            }
            catch (Exception ex)
            {
                result = new
                {
                    flag = false,
                    message = "Delete failed !!\nError Occured."
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateProcProject(List<DemoVM> ResourceDetails, int ProcProjectId, int ProjectSiteId, DateTime? BOQDate, string BOQNo, DateTime? NOADate, string NOANo, string ProjectType, string ProjectStatus, string ProjectRemarks)
        {
            var result = new
            {
                flag = false,
                message = "Saving failed"
            };

            var existProcProject = db.ProcProject.Where(x => x.Id == ProcProjectId && x.ProjectSiteId == ProjectSiteId).ToList();

            if (existProcProject.Count != 0)
            {
                //try
                //{
                var flag = false;
                var checkdata = db.ProcProject.Where(x => x.Id == ProcProjectId && x.ProjectSiteId == ProjectSiteId).SingleOrDefault();

                if (checkdata != null)
                {
                    checkdata.Id = ProcProjectId;
                    checkdata.ProjectSiteId = ProjectSiteId;
                    checkdata.BOQDate = BOQDate;
                    checkdata.NOADate = NOADate;
                    checkdata.BOQNo = BOQNo;
                    checkdata.NOANo = NOANo;
                    checkdata.Status = ProjectStatus;
                    //if (ProjectRemarks == null)
                    //{
                    //    checkdata.Remarks = "";
                    //}
                    //else
                    //{
                    // checkdata.Remarks = ProjectRemarks;
                    //}

                    checkdata.Remarks = ProjectRemarks;
                    checkdata.ProjectType = ProjectType;

                    db.Entry(checkdata).State = EntityState.Modified;

                    flag = db.SaveChanges() > 0;
                    if (flag == true)
                    {
                        if (ResourceDetails != null)
                        {
                            try
                            {
                                foreach (var item in ResourceDetails)
                                {
                                    ProcProjectItem procProjectItem = new ProcProjectItem();
                                    procProjectItem.ProcProjectId = item.ProcProjectId;
                                    //  procProjectItem.ProjectSiteId = item.ProjectSiteId;
                                    procProjectItem.ItemId = item.ItemISLNO;
                                    procProjectItem.UnitId = item.UnitUSLNO;
                                    procProjectItem.PQuantity = item.PQuantity;
                                    procProjectItem.PCost = item.PCost;
                                    procProjectItem.Remarks = item.Remarks;
                                    //db.ProcProjectItem.Add(procProjectItem);

                                    var checkingProjectItems = db.ProcProjectItem.FirstOrDefault(x => x.ProcProjectId == item.ProcProjectId /*&& x.ProjectSiteId == item.ProjectSiteId*/ && x.ItemId == item.ItemISLNO);

                                    flag = false;
                                    if (checkingProjectItems == null)
                                    {
                                        db.Entry(procProjectItem).State = EntityState.Added;
                                    }
                                    else
                                    {
                                        procProjectItem = db.ProcProjectItem.Find(checkingProjectItems.Id);
                                        procProjectItem.PQuantity = item.PQuantity;
                                        procProjectItem.PCost = item.PCost;
                                        procProjectItem.PCost = item.PCost;
                                        procProjectItem.Remarks = item.Remarks;
                                        db.Entry(procProjectItem).State = EntityState.Modified;
                                    }

                                    //db.Entry(procProjectItem).State = procProjectItem.ProjectId == item.ProjectId && procProjectItem.ProjectSiteId==item.ProjectSiteId ?
                                    //                            EntityState.Added :
                                    //                            EntityState.Modified;
                                    db.SaveChanges();
                                }

                                result = new
                                {
                                    flag = true,
                                    message = "Edit saving successful !"
                                };

                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                //   }
                //catch (Exception ex)
                //{

                //}
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = new
                {
                    flag = false,
                    message = "Procurement project and Project Items already exist!"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }






        public ActionResult Details(int? projectId, int? siteId)
        {
            //here projectId comes from index which is from ProcProject model i.e here projectId is ProcProjectId

            ViewBag.PId = projectId;
            ViewBag.SId = siteId;


            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");

            ViewBag.ItemName = new SelectList(db.Item, "ISLNO", "ItemName");
            ViewBag.Unit = new SelectList(db.Unit, "USLNO", "UnitName");

            //var projectTypeStatus = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" }, new SelectListItem { Text = "Non-Government", Value = "Non-Government" }, }, "Value", "Text");
            //ViewBag.ProjectType = projectTypeStatus;


            //ViewBag.PrManId = new SelectList(db.CompanyResource, "Id", "Name");
            //ViewBag.StEngId = new SelectList(db.CompanyResource, "Id", "Name");

            //var projects = db.Project.SingleOrDefault(x => x.Id == projectId);
            //ViewBag.StartDate = NullHelper.DateToString(projects.StartDate);
            //ViewBag.EndDate = NullHelper.DateToString(projects.EndDate);

            var projectIdFinder = db.ProjectSite.SingleOrDefault(x => x.Id == siteId);

            var projects = db.Project.SingleOrDefault(x => x.Id == projectIdFinder.ProjectId);
            ViewBag.StartDate = NullHelper.DateToString(projects.StartDate);
            ViewBag.EndDate = NullHelper.DateToString(projects.EndDate);


            var projectRes = db.ProjectResource.SingleOrDefault(x => x.ProjectId == projectIdFinder.ProjectId);
            ViewBag.PrMan = NullHelper.ObjectToString(projectRes.CompanyResource.Name);

            var projectSiteRes = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == siteId);
            ViewBag.StEng = NullHelper.ObjectToString(projectSiteRes.CompanyResource.Name);


            var procProjectId = db.ProcProject.FirstOrDefault(x => x.Id == projectId && x.ProjectSiteId == siteId);
            ViewBag.ProcProjectId = procProjectId.Id;

            ViewBag.ProcProjectIdDetailsForm = procProjectId.ProjectSite.Project.Name;

            var procProjects = db.ProcProject.SingleOrDefault(x => x.Id == projectId && x.ProjectSiteId == siteId);

            var projectType = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" },
                new SelectListItem { Text = "Non-Government", Value = "Non-Government" }, }, "Value", "Text", procProjects.ProjectType);
            ViewBag.ProjectType = projectType;
            ViewBag.BOQDate = NullHelper.DateToString(procProjects.BOQDate);
            ViewBag.NOADate = NullHelper.DateToString(procProjects.NOADate);
            ViewBag.BOQNo = NullHelper.ObjectToString(procProjects.BOQNo);
            ViewBag.NOANo = NullHelper.ObjectToString(procProjects.NOANo);
            ViewBag.PType = NullHelper.ObjectToString(procProjects.ProjectType);
            ViewBag.PStatus = NullHelper.ObjectToString(procProjects.Status);
            ViewBag.PRemarks = NullHelper.ObjectToString(procProjects.Remarks);



            //var projectRes = db.ProjectResource.SingleOrDefault(x => x.ProjectId == projectId);
            //ViewBag.PrMan = NullHelper.ObjectToString(projectRes.CompanyResource.Name);

            //var projectSiteRes = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == siteId);
            //ViewBag.StEng = NullHelper.ObjectToString(projectSiteRes.CompanyResource.Name);

            VMProjectItem vmProjectItem = new VMProjectItem();
            vmProjectItem.ProcProjectItem = db.ProcProjectItem.Where(x => x.ProcProjectId == projectId /*&& x.ProjectSiteId == siteId*/).ToList();


            if (projectId == null || siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProcProject procProject = db.ProcProject.Where(x => x.Id == projectId && x.ProjectSiteId == siteId).FirstOrDefault();
            Project project = db.Project.Where(x => x.Id == projectId).FirstOrDefault();
            ProcProjectItem procProjectItem = db.ProcProjectItem.Where(x => x.ProcProjectId == projectId /*&& x.ProjectSiteId == siteId*/).FirstOrDefault();


            if (procProject == null)
            {
                return HttpNotFound();
            }

            return View(vmProjectItem);


            //if (projectId == null || siteId==null )
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //ProcProject procProject = db.ProcProject.Find(projectId,siteId);
            //if (procProject == null || siteId==null)
            //{
            //    return HttpNotFound();
            //}
            //return View(procProject);
        }




        public ActionResult Edit(int? projectId, int? siteId)
        {
            ViewBag.PId = projectId;
            ViewBag.SId = siteId;

            var procProjectId = db.ProcProject.FirstOrDefault(x => x.Id == projectId && x.ProjectSiteId == siteId);
            ViewBag.ProcProjectId = procProjectId.Id;

            ViewBag.ProcProjectIdEditForm = procProjectId.ProjectSite.Project.Name;



            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");



            ViewBag.ItemName = new SelectList(db.Item, "Id", "Name");
            ViewBag.Unit = new SelectList(db.Unit, "Id", "Name");

            var projectTypeStatus = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" }, new SelectListItem { Text = "Non-Government", Value = "Non-Government" }, }, "Value", "Text");
            ViewBag.ProjectType = projectTypeStatus;

            ViewBag.PrManId = new SelectList(db.CompanyResource, "Id", "Name");
            ViewBag.StEngId = new SelectList(db.CompanyResource, "Id", "Name");

            var projectIdFinder = db.ProjectSite.SingleOrDefault(x => x.Id == siteId);

            var projects = db.Project.SingleOrDefault(x => x.Id == projectIdFinder.ProjectId);
            ViewBag.StartDate = NullHelper.DateToString(projects.StartDate);
            ViewBag.EndDate = NullHelper.DateToString(projects.EndDate);

            var procProjects = db.ProcProject.SingleOrDefault(x => x.Id == projectId && x.ProjectSiteId == siteId);
            ViewBag.BOQDate = NullHelper.DateToString(procProjects.BOQDate);
            ViewBag.NOADate = NullHelper.DateToString(procProjects.NOADate);
            ViewBag.BOQNo = NullHelper.ObjectToString(procProjects.BOQNo);
            ViewBag.NOANo = NullHelper.ObjectToString(procProjects.NOANo);
            //ViewBag.PType = NullHelper.ObjectToString(procProjects.ProjectType);
            var projectType = new SelectList(new List<SelectListItem> { new SelectListItem { Text = "Government", Value = "Government" },
                new SelectListItem { Text = "Non-Government", Value = "Non-Government" }, }, "Value", "Text", procProjects.ProjectType);
            ViewBag.ProjectType = projectType;
            ViewBag.PStatus = NullHelper.ObjectToString(procProjects.Status);
            ViewBag.PRemarks = NullHelper.ObjectToString(procProjects.Remarks);



            var projectRes = db.ProjectResource.SingleOrDefault(x => x.ProjectId == projectIdFinder.ProjectId);
            ViewBag.PrMan = NullHelper.ObjectToString(projectRes.CompanyResource.Name);

            var projectSiteRes = db.ProjectSiteResource.SingleOrDefault(x => x.ProjectSiteId == siteId);
            ViewBag.StEng = NullHelper.ObjectToString(projectSiteRes.CompanyResource.Name);

            VMProjectItem vmProjectItem = new VMProjectItem();
            vmProjectItem.ProcProjectItem = db.ProcProjectItem.Where(x => x.ProcProjectId == projectId /*&& x.ProjectSiteId == siteId*/).ToList();


            if (projectId == null || siteId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProcProject procProject = db.ProcProject.Where(x => x.Id == projectId && x.ProjectSiteId == siteId).FirstOrDefault();
            Project project = db.Project.Where(x => x.Id == projectId).FirstOrDefault();
            ProcProjectItem procProjectItem = db.ProcProjectItem.Where(x => x.ProcProjectId == projectId /*&& x.ProjectSiteId == siteId*/).FirstOrDefault();


            if (procProject == null)
            {
                return HttpNotFound();
            }

            return View(vmProjectItem);
        }



        public JsonResult getProjectItems(int ProjectId, int ProjectSiteId)
        {
            var counter = 0;
            List<ItemViewModel> check = new List<ItemViewModel>();
            var items = db.ProcProjectItem.Where(x => x.ProcProjectId == ProjectId /*&& x.ProjectSiteId == ProjectSiteId*/).ToList();
            foreach (var i in items)
            {
                ItemViewModel p = new ItemViewModel();
                p.ProjectId = i.ProcProjectId;
                //   p.ProjectSiteId = i.;         
                p.ItemISLNO = i.ItemId;
                p.PQuantity = i.PQuantity;
                p.UnitUSLNO = i.UnitId;
                p.PCost = i.PCost;
                if (i.Remarks == null)
                {
                    p.Remarks = "";
                }
                else
                {
                    p.Remarks = i.Remarks;
                }

                var itemsName = db.ProcProjectItem.FirstOrDefault(x => x.ItemId == i.ItemId);
                p.ItemName = itemsName.Item.Name;
                var unitsName = db.ProcProjectItem.FirstOrDefault(x => x.UnitId == i.UnitId);
                p.UnitName = unitsName.Unit.Name;
                check.Add(p);
            }

            return Json(check, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            if (ModelState.IsValid)
            {
                //db.Entry(procurementProject).State = EntityState.Modified;
                //db.Entry(projectItem).State = EntityState.Modified;
                //db.Entry(projectResource).State = EntityState.Modified;
                //db.Entry(projectSiteResources).State = EntityState.Modified;
                //db.Entry(items).State = EntityState.Modified;
                //db.Entry(units).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Project, "Id", "Name");
            ViewBag.SiteId = new SelectList(db.ProjectSite, "Id", "Name");

            return View();
        }



        public JsonResult DeleteProcProjects(int procProjectId)
        {

            var ProcProjectCount = (from procProjectmas in db.ProcProject
                                        //join tenderDet in db.Proc_TenderDet on tenderMas.Id equals tenderDet.Proc_TenderMasId
                                    join requisitionMas in db.Proc_RequisitionMas on procProjectmas.Id equals requisitionMas.ProcProjectId
                                    where procProjectmas.Id == procProjectId
                                    select procProjectmas).Distinct().Count();


            if (ProcProjectCount == 0)
            {
                bool flag = false;
                try
                {
                    var itemsToDeleteTask = db.ProcProjectItem.Where(x => x.ProcProjectId == procProjectId);
                    db.ProcProjectItem.RemoveRange(itemsToDeleteTask);
                    db.SaveChanges();

                    var itemsToDeletePlan = db.ProcProject.Where(x => x.Id == procProjectId);
                    db.ProcProject.RemoveRange(itemsToDeletePlan);

                    //var procProjectDelete = db.ProcProject.Where(x => x.Id == Pid);
                    //db.ProcProject.RemoveRange(procProjectDelete);


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
                        message = "Project deletion successful."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    var result = new
                    {
                        flag = false,
                        message = "Project deletion failed!\nError Occured."
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }


            }
            else
            {
                var result = new
                {
                    flag = false,
                    message = "Project deletion failed!\nDelete requisition first."
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteTDetailItem(int DetailId)
        {

            var result = new
            {
                flag = false,
                message = "Delete error !"
            };

            var check = (from procProject in db.ProcProject
                         join procProjectItems in db.ProcProjectItem on procProject.Id equals procProjectItems.ProcProjectId
                         join requisitioMas in db.Proc_RequisitionMas on procProject.Id equals requisitioMas.ProcProjectId
                         join requisitionDet in db.Proc_RequisitionDet on requisitioMas.Id equals requisitionDet.Proc_RequisitionMasId
                         where procProjectItems.ItemId== DetailId && requisitionDet.Proc_RequisitionMas.ProcProjectId == procProject.Id
                         select procProjectItems).Distinct().ToList();

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

