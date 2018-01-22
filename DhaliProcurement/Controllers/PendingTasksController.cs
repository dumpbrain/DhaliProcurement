using DhaliProcurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DhaliProcurement.Controllers
{
    [Authorize(Roles = "Management")]
    public class PendingTasksController : Controller
    {
        private DCPSContext db = new DCPSContext();

        public ActionResult PendingRequisition()
        {
            var data = db.Proc_RequisitionMas.Where(y => y.Status.Trim() != "A").ToList();

            return View(data);

        }

        public ActionResult PendingTender()
        {
            var data = db.Proc_TenderMas.Where(y => y.isApproved.Trim() != "A").ToList();

            return View(data);

        }

        public ActionResult PendingPurchase()
        {
            var data = db.Proc_RequisitionMas.Where(y => y.Status.Trim() != "A").ToList();

            return View(data);

        }

    }
}