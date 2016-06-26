namespace Basha.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Flowers",
                c => new
                    {
                        FlowerId = c.Int(nullable: false, identity: true),
                        FlowerName = c.String(),
                        Qty = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FlowerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Flowers");
        }
    }
}
