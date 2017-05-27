namespace Baker_Point.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        BlogsId = c.Int(nullable: false, identity: true),
                        BlogTitle = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                        Comments = c.Int(nullable: false),
                        PostTime = c.DateTime(nullable: false),
                        Context = c.String(nullable: false),
                        titleImg = c.String(),
                    })
                .PrimaryKey(t => t.BlogsId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        BlogsId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        PostTime = c.DateTime(nullable: false),
                        Context = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Blogs", new[] { "UserId" });
            DropForeignKey("dbo.Comments", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Blogs", "UserId", "dbo.UserProfile");
            DropTable("dbo.Comments");
            DropTable("dbo.Blogs");
        }
    }
}
