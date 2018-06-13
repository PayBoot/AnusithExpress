namespace AnousithExpress.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbItemAllocation", "Item_Id", c => c.Int());
            CreateIndex("dbo.TbItemAllocation", "Item_Id");
            AddForeignKey("dbo.TbItemAllocation", "Item_Id", "dbo.TbItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TbItemAllocation", "Item_Id", "dbo.TbItems");
            DropIndex("dbo.TbItemAllocation", new[] { "Item_Id" });
            DropColumn("dbo.TbItemAllocation", "Item_Id");
        }
    }
}
