namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addprice1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TbPrice", "Condition", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TbPrice", "Condition", c => c.String());
        }
    }
}
