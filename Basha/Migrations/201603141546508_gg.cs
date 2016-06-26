namespace Basha.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gg : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.flowerDiscs", "FlowerId");
            AddForeignKey("dbo.flowerDiscs", "FlowerId", "dbo.Flowers", "FlowerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.flowerDiscs", "FlowerId", "dbo.Flowers");
            DropIndex("dbo.flowerDiscs", new[] { "FlowerId" });
        }
    }
}
