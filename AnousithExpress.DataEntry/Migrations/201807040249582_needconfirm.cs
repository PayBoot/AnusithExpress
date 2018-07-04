namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class needconfirm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbConsolidateList", "needConfirm", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbConsolidateList", "needConfirm");
        }
    }
}
