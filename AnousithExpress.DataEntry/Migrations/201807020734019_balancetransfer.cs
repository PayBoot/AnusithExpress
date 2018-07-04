namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class balancetransfer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbConsolidateList", "BalanceTransfer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbConsolidateList", "BalanceTransfer");
        }
    }
}
