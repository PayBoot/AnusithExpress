namespace AnousithExpress.DataEntry.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addprice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TbPrice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Condition = c.String(),
                        PriceSet = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TbPrice");
        }
    }
}
