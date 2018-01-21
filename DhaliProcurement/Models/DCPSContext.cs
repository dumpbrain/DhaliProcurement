using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DhaliProcurement.Models
{
    public class DCPSContext : DbContext
    {
         public DCPSContext() : base("DCPS")
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UsersRole> UsersRole { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ProjectGroup> ProjectGroup { get; set; }
        public DbSet<CompanyInformation> CompanyInformation { get; set; }
        public DbSet<CompanyResource> CompanyResource { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectResource> ProjectResource { get; set; }
        public DbSet<ProjectSite> ProjectSite { get; set; }
        public DbSet<ProjectSiteResource> ProjectSiteResource { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<ProcProject> ProcProject { get; set; }
        public DbSet<ProcProjectItem> ProcProjectItem { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Proc_RequisitionMas> Proc_RequisitionMas { get; set; }
        public DbSet<Proc_RequisitionDet> Proc_RequisitionDet { get; set; }
        public DbSet<Proc_TenderMas> Proc_TenderMas { get; set; }
        public DbSet<Proc_TenderDet> Proc_TenderDet { get; set; }
        public DbSet<Proc_MaterialEntryMas> Proc_MaterialEntryMas { get; set; }
        public DbSet<Proc_MaterialEntryDet> Proc_MaterialEntryDet { get; set; }
        public DbSet<Proc_PurchaseOrderMas> Proc_PurchaseOrderMas { get; set; }
        public DbSet<Proc_PurchaseOrderDet> Proc_PurchaseOrderDet { get; set; }
        public DbSet<Proc_VendorPaymentMas> Proc_VendorPaymentMas { get; set; }
        public DbSet<Proc_VendorPaymentDet> Proc_VendorPaymentDet { get; set; }

    }
}
