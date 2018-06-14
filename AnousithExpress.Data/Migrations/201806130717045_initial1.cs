namespace AnousithExpress.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TbTime", "Time", c => c.String(maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TbTime", "Time", c => c.DateTime(nullable: false));
        }
    }
}
