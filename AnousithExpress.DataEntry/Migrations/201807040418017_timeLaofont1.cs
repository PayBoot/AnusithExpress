namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeLaofont1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TbTime", "Time", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TbTime", "Time", c => c.String(maxLength: 10));
        }
    }
}
