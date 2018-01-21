namespace DhaliProcurement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _16jan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 50),
                        ContactPerson = c.String(maxLength: 100),
                        Note = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        ClientId = c.Int(),
                        ProjectGroupId = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.ProjectGroups", t => t.ProjectGroupId)
                .Index(t => t.ClientId)
                .Index(t => t.ProjectGroupId);
            
            CreateTable(
                "dbo.ProjectGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectResources",
                c => new
                    {
                        ProjectId = c.Int(nullable: false),
                        CompanyResourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectId, t.CompanyResourceId })
                .ForeignKey("dbo.CompanyResources", t => t.CompanyResourceId, cascadeDelete: false)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: false)
                .Index(t => t.ProjectId)
                .Index(t => t.CompanyResourceId);
            
            CreateTable(
                "dbo.CompanyResources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Position = c.String(maxLength: 100),
                        DOJ = c.DateTime(),
                        DOB = c.DateTime(),
                        Phone = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Address = c.String(maxLength: 100),
                        Status = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectSiteResources",
                c => new
                    {
                        ProjectSiteId = c.Int(nullable: false),
                        CompanyResourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectSiteId, t.CompanyResourceId })
                .ForeignKey("dbo.CompanyResources", t => t.CompanyResourceId, cascadeDelete: false)
                .ForeignKey("dbo.ProjectSites", t => t.ProjectSiteId, cascadeDelete: false)
                .Index(t => t.ProjectSiteId)
                .Index(t => t.CompanyResourceId);
            
            CreateTable(
                "dbo.ProjectSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Location = c.String(maxLength: 100),
                        SiteStatus = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: false)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.CompanyInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 50),
                        Web = c.String(maxLength: 50),
                        Email = c.String(maxLength: 100),
                        ContactPerson = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Size = c.String(maxLength: 50),
                        HSCode = c.String(maxLength: 100),
                        ItemDesc = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Proc_MaterialEntryDet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Proc_MaterialEntryMasId = c.Int(nullable: false),
                        Proc_PurchaseOrderDetId = c.Int(nullable: false),
                        ChallanNo = c.String(nullable: false, maxLength: 50),
                        ChallanDate = c.DateTime(),
                        EntryQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proc_MaterialEntryMas", t => t.Proc_MaterialEntryMasId, cascadeDelete: false)
                .ForeignKey("dbo.Proc_PurchaseOrderDet", t => t.Proc_PurchaseOrderDetId, cascadeDelete: false)
                .Index(t => t.Proc_MaterialEntryMasId)
                .Index(t => t.Proc_PurchaseOrderDetId)
                .Index(t => t.ChallanNo, unique: true, name: "ChallanNo");
            
            CreateTable(
                "dbo.Proc_MaterialEntryMas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EDate = c.DateTime(),
                        ProcProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProcProjects", t => t.ProcProjectId, cascadeDelete: false)
                .Index(t => t.ProcProjectId);
            
            CreateTable(
                "dbo.ProcProjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectSiteId = c.Int(nullable: false),
                        BOQDate = c.DateTime(),
                        BOQNo = c.String(maxLength: 100),
                        NOADate = c.DateTime(),
                        NOANo = c.String(maxLength: 100),
                        ProjectType = c.String(maxLength: 50),
                        Status = c.String(maxLength: 50),
                        Remarks = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectSites", t => t.ProjectSiteId, cascadeDelete: false)
                .Index(t => t.ProjectSiteId);
            
            CreateTable(
                "dbo.Proc_PurchaseOrderDet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Proc_PurchaseOrderMasId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        POQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        POAmt = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.Proc_PurchaseOrderMas", t => t.Proc_PurchaseOrderMasId, cascadeDelete: false)
                .Index(t => t.Proc_PurchaseOrderMasId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Proc_PurchaseOrderMas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PONo = c.String(nullable: false, maxLength: 100),
                        PODate = c.DateTime(nullable: false),
                        VendorId = c.Int(nullable: false),
                        Proc_TenderMasId = c.Int(nullable: false),
                        LeadTime = c.Int(),
                        OrderTo = c.String(maxLength: 100),
                        Attention = c.String(maxLength: 100),
                        AttnCell = c.String(maxLength: 100),
                        AttnEmail = c.String(maxLength: 100),
                        Subject = c.String(maxLength: 100),
                        Content = c.String(maxLength: 800),
                        RecvConcernPerson = c.String(maxLength: 10),
                        RecvConcernPersonCell = c.String(maxLength: 100),
                        POTotalAmt = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proc_TenderMas", t => t.Proc_TenderMasId, cascadeDelete: false)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: false)
                .Index(t => t.VendorId)
                .Index(t => t.Proc_TenderMasId);
            
            CreateTable(
                "dbo.Proc_TenderMas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TNo = c.String(nullable: false, maxLength: 50),
                        TDate = c.DateTime(nullable: false),
                        Specs = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 100),
                        isApproved = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TNo, unique: true, name: "TNo");
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 150),
                        Phone = c.String(maxLength: 50),
                        Web = c.String(maxLength: 50),
                        Email = c.String(maxLength: 100),
                        ContactPerson = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Proc_RequisitionDet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Proc_RequisitionMasId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        ReqQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CStockQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Brand = c.String(maxLength: 50),
                        Size = c.String(maxLength: 50),
                        RequiredDate = c.DateTime(),
                        Remarks = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: false)
                .ForeignKey("dbo.Proc_RequisitionMas", t => t.Proc_RequisitionMasId, cascadeDelete: false)
                .Index(t => t.Proc_RequisitionMasId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Proc_RequisitionMas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcProjectId = c.Int(nullable: false),
                        Rcode = c.String(nullable: false, maxLength: 20),
                        ReqDate = c.DateTime(nullable: false),
                        Remarks = c.String(maxLength: 50),
                        Status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProcProjects", t => t.ProcProjectId, cascadeDelete: true)
                .Index(t => t.ProcProjectId)
                .Index(t => t.Rcode, unique: true, name: "RCodeIndex");
            
            CreateTable(
                "dbo.Proc_TenderDet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Proc_TenderMasId = c.Int(nullable: false),
                        Proc_RequisitionDetId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        TQDate = c.DateTime(),
                        TQNo = c.String(maxLength: 50),
                        TQPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proc_RequisitionDet", t => t.Proc_RequisitionDetId, cascadeDelete: false)
                .ForeignKey("dbo.Proc_TenderMas", t => t.Proc_TenderMasId, cascadeDelete: false)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: false)
                .Index(t => t.Proc_TenderMasId)
                .Index(t => t.Proc_RequisitionDetId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.Proc_VendorPaymentDet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Proc_VendorPaymentMasId = c.Int(nullable: false),
                        Proc_MaterialEntryDetId = c.Int(nullable: false),
                        BillNo = c.String(maxLength: 50),
                        PayAmt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Proc_MaterialEntryDet", t => t.Proc_MaterialEntryDetId, cascadeDelete: false)
                .ForeignKey("dbo.Proc_VendorPaymentMas", t => t.Proc_VendorPaymentMasId, cascadeDelete: false)
                .Index(t => t.Proc_VendorPaymentMasId)
                .Index(t => t.Proc_MaterialEntryDetId);
            
            CreateTable(
                "dbo.Proc_VendorPaymentMas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VPDate = c.DateTime(nullable: false),
                        VendorId = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: false)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.ProcProjectItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcProjectId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        UnitId = c.Int(nullable: false),
                        PQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: false)
                .ForeignKey("dbo.ProcProjects", t => t.ProcProjectId, cascadeDelete: false)
                .ForeignKey("dbo.Units", t => t.UnitId, cascadeDelete: false)
                .Index(t => t.ProcProjectId)
                .Index(t => t.ItemId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 155),
                        Salt = c.String(maxLength: 100),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        Email = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 100),
                        Address = c.String(maxLength: 255),
                        IsActive = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsersRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UsersRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ProcProjectItems", "UnitId", "dbo.Units");
            DropForeignKey("dbo.ProcProjectItems", "ProcProjectId", "dbo.ProcProjects");
            DropForeignKey("dbo.ProcProjectItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Proc_VendorPaymentDet", "Proc_VendorPaymentMasId", "dbo.Proc_VendorPaymentMas");
            DropForeignKey("dbo.Proc_VendorPaymentMas", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.Proc_VendorPaymentDet", "Proc_MaterialEntryDetId", "dbo.Proc_MaterialEntryDet");
            DropForeignKey("dbo.Proc_TenderDet", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.Proc_TenderDet", "Proc_TenderMasId", "dbo.Proc_TenderMas");
            DropForeignKey("dbo.Proc_TenderDet", "Proc_RequisitionDetId", "dbo.Proc_RequisitionDet");
            DropForeignKey("dbo.Proc_RequisitionDet", "Proc_RequisitionMasId", "dbo.Proc_RequisitionMas");
            DropForeignKey("dbo.Proc_RequisitionMas", "ProcProjectId", "dbo.ProcProjects");
            DropForeignKey("dbo.Proc_RequisitionDet", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Proc_MaterialEntryDet", "Proc_PurchaseOrderDetId", "dbo.Proc_PurchaseOrderDet");
            DropForeignKey("dbo.Proc_PurchaseOrderDet", "Proc_PurchaseOrderMasId", "dbo.Proc_PurchaseOrderMas");
            DropForeignKey("dbo.Proc_PurchaseOrderMas", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.Proc_PurchaseOrderMas", "Proc_TenderMasId", "dbo.Proc_TenderMas");
            DropForeignKey("dbo.Proc_PurchaseOrderDet", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Proc_MaterialEntryDet", "Proc_MaterialEntryMasId", "dbo.Proc_MaterialEntryMas");
            DropForeignKey("dbo.Proc_MaterialEntryMas", "ProcProjectId", "dbo.ProcProjects");
            DropForeignKey("dbo.ProcProjects", "ProjectSiteId", "dbo.ProjectSites");
            DropForeignKey("dbo.ProjectResources", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectSiteResources", "ProjectSiteId", "dbo.ProjectSites");
            DropForeignKey("dbo.ProjectSites", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectSiteResources", "CompanyResourceId", "dbo.CompanyResources");
            DropForeignKey("dbo.ProjectResources", "CompanyResourceId", "dbo.CompanyResources");
            DropForeignKey("dbo.Projects", "ProjectGroupId", "dbo.ProjectGroups");
            DropForeignKey("dbo.Projects", "ClientId", "dbo.Clients");
            DropIndex("dbo.UsersRoles", new[] { "RoleId" });
            DropIndex("dbo.UsersRoles", new[] { "UserId" });
            DropIndex("dbo.ProcProjectItems", new[] { "UnitId" });
            DropIndex("dbo.ProcProjectItems", new[] { "ItemId" });
            DropIndex("dbo.ProcProjectItems", new[] { "ProcProjectId" });
            DropIndex("dbo.Proc_VendorPaymentMas", new[] { "VendorId" });
            DropIndex("dbo.Proc_VendorPaymentDet", new[] { "Proc_MaterialEntryDetId" });
            DropIndex("dbo.Proc_VendorPaymentDet", new[] { "Proc_VendorPaymentMasId" });
            DropIndex("dbo.Proc_TenderDet", new[] { "VendorId" });
            DropIndex("dbo.Proc_TenderDet", new[] { "Proc_RequisitionDetId" });
            DropIndex("dbo.Proc_TenderDet", new[] { "Proc_TenderMasId" });
            DropIndex("dbo.Proc_RequisitionMas", "RCodeIndex");
            DropIndex("dbo.Proc_RequisitionMas", new[] { "ProcProjectId" });
            DropIndex("dbo.Proc_RequisitionDet", new[] { "ItemId" });
            DropIndex("dbo.Proc_RequisitionDet", new[] { "Proc_RequisitionMasId" });
            DropIndex("dbo.Proc_TenderMas", "TNo");
            DropIndex("dbo.Proc_PurchaseOrderMas", new[] { "Proc_TenderMasId" });
            DropIndex("dbo.Proc_PurchaseOrderMas", new[] { "VendorId" });
            DropIndex("dbo.Proc_PurchaseOrderDet", new[] { "ItemId" });
            DropIndex("dbo.Proc_PurchaseOrderDet", new[] { "Proc_PurchaseOrderMasId" });
            DropIndex("dbo.ProcProjects", new[] { "ProjectSiteId" });
            DropIndex("dbo.Proc_MaterialEntryMas", new[] { "ProcProjectId" });
            DropIndex("dbo.Proc_MaterialEntryDet", "ChallanNo");
            DropIndex("dbo.Proc_MaterialEntryDet", new[] { "Proc_PurchaseOrderDetId" });
            DropIndex("dbo.Proc_MaterialEntryDet", new[] { "Proc_MaterialEntryMasId" });
            DropIndex("dbo.ProjectSites", new[] { "ProjectId" });
            DropIndex("dbo.ProjectSiteResources", new[] { "CompanyResourceId" });
            DropIndex("dbo.ProjectSiteResources", new[] { "ProjectSiteId" });
            DropIndex("dbo.ProjectResources", new[] { "CompanyResourceId" });
            DropIndex("dbo.ProjectResources", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "ProjectGroupId" });
            DropIndex("dbo.Projects", new[] { "ClientId" });
            DropTable("dbo.UsersRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Units");
            DropTable("dbo.ProcProjectItems");
            DropTable("dbo.Proc_VendorPaymentMas");
            DropTable("dbo.Proc_VendorPaymentDet");
            DropTable("dbo.Proc_TenderDet");
            DropTable("dbo.Proc_RequisitionMas");
            DropTable("dbo.Proc_RequisitionDet");
            DropTable("dbo.Vendors");
            DropTable("dbo.Proc_TenderMas");
            DropTable("dbo.Proc_PurchaseOrderMas");
            DropTable("dbo.Proc_PurchaseOrderDet");
            DropTable("dbo.ProcProjects");
            DropTable("dbo.Proc_MaterialEntryMas");
            DropTable("dbo.Proc_MaterialEntryDet");
            DropTable("dbo.Items");
            DropTable("dbo.CompanyInformations");
            DropTable("dbo.ProjectSites");
            DropTable("dbo.ProjectSiteResources");
            DropTable("dbo.CompanyResources");
            DropTable("dbo.ProjectResources");
            DropTable("dbo.ProjectGroups");
            DropTable("dbo.Projects");
            DropTable("dbo.Clients");
        }
    }
}
