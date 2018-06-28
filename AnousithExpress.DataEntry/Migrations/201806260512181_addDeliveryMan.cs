namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDeliveryMan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbItemAllocation", "DeliveryMan_Id", c => c.Int());
            CreateIndex("dbo.TbItemAllocation", "DeliveryMan_Id");
            AddForeignKey("dbo.TbItemAllocation", "DeliveryMan_Id", "dbo.TbUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TbItemAllocation", "DeliveryMan_Id", "dbo.TbUser");
            DropIndex("dbo.TbItemAllocation", new[] { "DeliveryMan_Id" });
            DropColumn("dbo.TbItemAllocation", "DeliveryMan_Id");
        }
    }
}
