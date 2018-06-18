namespace AnousithExpress.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbItemAllocation", "DateToDeliver", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbItemAllocation", "DateToDeliver");
        }
    }
}
