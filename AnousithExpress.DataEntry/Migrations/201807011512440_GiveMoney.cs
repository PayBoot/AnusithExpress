namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GiveMoney : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbItemSentHistory", "GiveMoney", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbItemSentHistory", "GiveMoney");
        }
    }
}
