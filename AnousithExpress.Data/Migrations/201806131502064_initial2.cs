namespace AnousithExpress.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TbItems", "Descripttion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TbItems", "Descripttion");
        }
    }
}
