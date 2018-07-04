namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial : DbMigration
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
                        IncomingBalanceInKip = c.Double(nullable: false),
                        IncomingBalanceInBaht = c.Double(nullable: false),
                        IncomingBalanceInDollar = c.Double(nullable: false),
                        BalanceToTransfer = c.Double(nullable: false),
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
                        Name = c.String(),
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
                        Descripttion = c.String(),
                        CreatedDate = c.DateTime(storeType: "date"),
                        ConfrimDate = c.DateTime(storeType: "date"),
                        ReceiveDate = c.DateTime(storeType: "date"),
                        SendingDate = c.DateTime(storeType: "date"),
                        SentDate = c.DateTime(storeType: "date"),
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
                "dbo.TbItemsPickUpAllocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateToPickUp = c.DateTime(nullable: false),
                        DeliveryMan_Id = c.Int(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbUser", t => t.DeliveryMan_Id)
                .ForeignKey("dbo.TbItems", t => t.Item_Id)
                .Index(t => t.DeliveryMan_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.TbUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Phonenumber = c.String(),
                        isDelete = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.TbItemSentAllocation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateToDeliver = c.DateTime(nullable: false),
                        DeliveryMan_Id = c.Int(),
                        Item_Id = c.Int(),
                        Route_Id = c.Int(),
                        Time_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbUser", t => t.DeliveryMan_Id)
                .ForeignKey("dbo.TbItems", t => t.Item_Id)
                .ForeignKey("dbo.TbRoute", t => t.Route_Id)
                .ForeignKey("dbo.TbTime", t => t.Time_Id)
                .Index(t => t.DeliveryMan_Id)
                .Index(t => t.Item_Id)
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
                        Time = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TbItemSentHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionDate = c.DateTime(nullable: false),
                        IncomingBalanceInKip = c.Double(nullable: false),
                        IncomingBalanceInBaht = c.Double(nullable: false),
                        IncomingBalanceInDollar = c.Double(nullable: false),
                        DeliveryMan_Id = c.Int(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbUser", t => t.DeliveryMan_Id)
                .ForeignKey("dbo.TbItems", t => t.Item_Id)
                .Index(t => t.DeliveryMan_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.TbItemsPickupHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionDate = c.DateTime(nullable: false),
                        DeliveryMan_Id = c.Int(),
                        Item_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TbUser", t => t.DeliveryMan_Id)
                .ForeignKey("dbo.TbItems", t => t.Item_Id)
                .Index(t => t.DeliveryMan_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.TbPrice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Condition = c.Double(nullable: false),
                        PriceSet = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TbItemsPickupHistory", "Item_Id", "dbo.TbItems");
            DropForeignKey("dbo.TbItemsPickupHistory", "DeliveryMan_Id", "dbo.TbUser");
            DropForeignKey("dbo.TbItemSentHistory", "Item_Id", "dbo.TbItems");
            DropForeignKey("dbo.TbItemSentHistory", "DeliveryMan_Id", "dbo.TbUser");
            DropForeignKey("dbo.TbItemSentAllocation", "Time_Id", "dbo.TbTime");
            DropForeignKey("dbo.TbItemSentAllocation", "Route_Id", "dbo.TbRoute");
            DropForeignKey("dbo.TbItemSentAllocation", "Item_Id", "dbo.TbItems");
            DropForeignKey("dbo.TbItemSentAllocation", "DeliveryMan_Id", "dbo.TbUser");
            DropForeignKey("dbo.TbItemsPickUpAllocation", "Item_Id", "dbo.TbItems");
            DropForeignKey("dbo.TbItemsPickUpAllocation", "DeliveryMan_Id", "dbo.TbUser");
            DropForeignKey("dbo.TbUser", "Status_Id", "dbo.TbStatus");
            DropForeignKey("dbo.TbUser", "Role_Id", "dbo.TbRole");
            DropForeignKey("dbo.TbConsolidatedItems", "Items_Id", "dbo.TbItems");
            DropForeignKey("dbo.TbItems", "Status_Id", "dbo.TbItemStatus");
            DropForeignKey("dbo.TbItems", "Customer_Id", "dbo.TbCustomer");
            DropForeignKey("dbo.TbConsolidatedItems", "Consolidator_Id", "dbo.TbConsolidateList");
            DropForeignKey("dbo.TbConsolidateList", "Customer_Id", "dbo.TbCustomer");
            DropForeignKey("dbo.TbCustomer", "Status_Id", "dbo.TbStatus");
            DropIndex("dbo.TbItemsPickupHistory", new[] { "Item_Id" });
            DropIndex("dbo.TbItemsPickupHistory", new[] { "DeliveryMan_Id" });
            DropIndex("dbo.TbItemSentHistory", new[] { "Item_Id" });
            DropIndex("dbo.TbItemSentHistory", new[] { "DeliveryMan_Id" });
            DropIndex("dbo.TbItemSentAllocation", new[] { "Time_Id" });
            DropIndex("dbo.TbItemSentAllocation", new[] { "Route_Id" });
            DropIndex("dbo.TbItemSentAllocation", new[] { "Item_Id" });
            DropIndex("dbo.TbItemSentAllocation", new[] { "DeliveryMan_Id" });
            DropIndex("dbo.TbUser", new[] { "Status_Id" });
            DropIndex("dbo.TbUser", new[] { "Role_Id" });
            DropIndex("dbo.TbItemsPickUpAllocation", new[] { "Item_Id" });
            DropIndex("dbo.TbItemsPickUpAllocation", new[] { "DeliveryMan_Id" });
            DropIndex("dbo.TbItems", new[] { "Status_Id" });
            DropIndex("dbo.TbItems", new[] { "Customer_Id" });
            DropIndex("dbo.TbCustomer", new[] { "Status_Id" });
            DropIndex("dbo.TbConsolidateList", new[] { "Customer_Id" });
            DropIndex("dbo.TbConsolidatedItems", new[] { "Items_Id" });
            DropIndex("dbo.TbConsolidatedItems", new[] { "Consolidator_Id" });
            DropTable("dbo.TbPrice");
            DropTable("dbo.TbItemsPickupHistory");
            DropTable("dbo.TbItemSentHistory");
            DropTable("dbo.TbTime");
            DropTable("dbo.TbRoute");
            DropTable("dbo.TbItemSentAllocation");
            DropTable("dbo.TbRole");
            DropTable("dbo.TbUser");
            DropTable("dbo.TbItemsPickUpAllocation");
            DropTable("dbo.TbItemStatus");
            DropTable("dbo.TbItems");
            DropTable("dbo.TbStatus");
            DropTable("dbo.TbCustomer");
            DropTable("dbo.TbConsolidateList");
            DropTable("dbo.TbConsolidatedItems");
        }
    }
}
