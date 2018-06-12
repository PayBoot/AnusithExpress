namespace AnousithExpress.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnousithLogistic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TbConsolidatedItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Consolidator_Id = c.Int(),
                        Items_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbConsolidateList", t => t.Consolidator_Id)
                .ForeignKey("dbo.TbItems", t => t.Items_Id)
                .Index(t => t.Consolidator_Id)
                .Index(t => t.Items_Id);
            
            CreateTable(
                "dbo.TbConsolidateList",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsolidateNumber = c.String(),
                        ConsolidatedDate = c.DateTime(nullable: false),
                        AmountOfItem = c.Double(nullable: false),
                        Fee = c.Double(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbCustomer", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.TbCustomer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Phonenumber = c.String(),
                        Password = c.String(),
                        Address = c.String(),
                        BCEL_Kip = c.String(),
                        BCEL_Baht = c.String(),
                        BCEL_Dollar = c.String(),
                        isDeleted = c.Boolean(nullable: false),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbStatus", t => t.Status_Id)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.TbStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrackingNumber = c.String(),
                        ItemName = c.String(),
                        ItemValue_Kip = c.Double(),
                        ItemValue_Baht = c.Double(),
                        ItemValue_Dollar = c.Double(),
                        ReceiverName = c.String(),
                        ReceipverPhone = c.String(),
                        ReceiverAddress = c.String(),
                        isDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(storeType: "date"),
                        ConfrimDate = c.DateTime(storeType: "date"),
                        ReceiveDate = c.DateTime(storeType: "date"),
                        SendingDate = c.DateTime(storeType: "date"),
                        Customer_Id = c.Int(),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbCustomer", t => t.Customer_Id)
                .ForeignKey("dbo.TbItemStatus", t => t.Status_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.TbItemStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbItemAllocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Route_Id = c.Int(),
                        Time_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbRoute", t => t.Route_Id)
                .ForeignKey("dbo.TbTime", t => t.Time_Id)
                .Index(t => t.Route_Id)
                .Index(t => t.Time_Id);
            
            CreateTable(
                "dbo.TbRoute",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Route = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbTime",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbItemLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Item_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbItems", t => t.Item_Id)
                .ForeignKey("dbo.TbUser", t => t.User_Id)
                .Index(t => t.Item_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.TbUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Phonenumber = c.String(),
                        Role_Id = c.Int(),
                        Status_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbRole", t => t.Role_Id)
                .ForeignKey("dbo.TbStatus", t => t.Status_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.Status_Id);
            
            CreateTable(
                "dbo.TbRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TbItemLog", "User_Id", "dbo.TbUser");
            DropForeignKey("dbo.TbUser", "Status_Id", "dbo.TbStatus");
            DropForeignKey("dbo.TbUser", "Role_Id", "dbo.TbRole");
            DropForeignKey("dbo.TbItemLog", "Item_Id", "dbo.TbItems");
            DropForeignKey("dbo.TbItemAllocation", "Time_Id", "dbo.TbTime");
            DropForeignKey("dbo.TbItemAllocation", "Route_Id", "dbo.TbRoute");
            DropForeignKey("dbo.TbConsolidatedItems", "Items_Id", "dbo.TbItems");
            DropForeignKey("dbo.TbItems", "Status_Id", "dbo.TbItemStatus");
            DropForeignKey("dbo.TbItems", "Customer_Id", "dbo.TbCustomer");
            DropForeignKey("dbo.TbConsolidatedItems", "Consolidator_Id", "dbo.TbConsolidateList");
            DropForeignKey("dbo.TbConsolidateList", "Customer_Id", "dbo.TbCustomer");
            DropForeignKey("dbo.TbCustomer", "Status_Id", "dbo.TbStatus");
            DropIndex("dbo.TbUser", new[] { "Status_Id" });
            DropIndex("dbo.TbUser", new[] { "Role_Id" });
            DropIndex("dbo.TbItemLog", new[] { "User_Id" });
            DropIndex("dbo.TbItemLog", new[] { "Item_Id" });
            DropIndex("dbo.TbItemAllocation", new[] { "Time_Id" });
            DropIndex("dbo.TbItemAllocation", new[] { "Route_Id" });
            DropIndex("dbo.TbItems", new[] { "Status_Id" });
            DropIndex("dbo.TbItems", new[] { "Customer_Id" });
            DropIndex("dbo.TbCustomer", new[] { "Status_Id" });
            DropIndex("dbo.TbConsolidateList", new[] { "Customer_Id" });
            DropIndex("dbo.TbConsolidatedItems", new[] { "Items_Id" });
            DropIndex("dbo.TbConsolidatedItems", new[] { "Consolidator_Id" });
            DropTable("dbo.TbRole");
            DropTable("dbo.TbUser");
            DropTable("dbo.TbItemLog");
            DropTable("dbo.TbTime");
            DropTable("dbo.TbRoute");
            DropTable("dbo.TbItemAllocation");
            DropTable("dbo.TbItemStatus");
            DropTable("dbo.TbItems");
            DropTable("dbo.TbStatus");
            DropTable("dbo.TbCustomer");
            DropTable("dbo.TbConsolidateList");
            DropTable("dbo.TbConsolidatedItems");
        }
    }
}
