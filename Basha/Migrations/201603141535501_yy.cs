namespace Basha.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class yy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.flowerDiscs",
                c => new
                    {
                        flowerDiscId = c.Int(nullable: false, identity: true),
                        FlowerId = c.Int(nullable: false),
                        fcolor = c.String(),
                    })
                .PrimaryKey(t => t.flowerDiscId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.flowerDiscs");
        }
    }
}
