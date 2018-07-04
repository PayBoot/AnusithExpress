namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class balancetransfer1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbConsolidateList", "isBalanceTransfer", c => c.Boolean(nullable: false));
            DropColumn("dbo.TbConsolidateList", "BalanceTransfer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TbConsolidateList", "BalanceTransfer", c => c.Boolean(nullable: false));
            DropColumn("dbo.TbConsolidateList", "isBalanceTransfer");
        }
    }
}
