namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class confirmConsolidation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbConsolidateList", "isCustomerConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbConsolidateList", "isCustomerConfirmed");
        }
    }
}
