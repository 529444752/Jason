namespace Baker_Point.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAvatars",
                c => new
                    {
                        UserAvatarId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        imgSrc = c.String(),
                    })
                .PrimaryKey(t => t.UserAvatarId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserAvatars", new[] { "UserId" });
            DropForeignKey("dbo.UserAvatars", "UserId", "dbo.UserProfile");
            DropTable("dbo.UserAvatars");
        }
    }
}
