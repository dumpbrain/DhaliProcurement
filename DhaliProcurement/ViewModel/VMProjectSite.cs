using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DhaliProcurement.Models;

namespace DhaliProcurement.ViewModel
{
    public class VMProjectSite
    {
        // public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public int ProjectSiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteLocation { get; set; }
        // public int RName { get; set; }
        // public string Remarks { get; set; }
        // public DateTime StartDate { get; set; }
        // public DateTime EndDate { get; set; }
        public int SiteEngineerId { get; set; }
        public string SiteEngineer { get; set; }

    }
}