namespace ministryofjusticeWebUi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        ReceiverId = c.String(),
                        IsViewed = c.Boolean(nullable: false),
                        CaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cases", t => t.CaseId, cascadeDelete: true)
                .Index(t => t.CaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "CaseId", "dbo.Cases");
            DropIndex("dbo.Notifications", new[] { "CaseId" });
            DropTable("dbo.Notifications");
        }
    }
}
