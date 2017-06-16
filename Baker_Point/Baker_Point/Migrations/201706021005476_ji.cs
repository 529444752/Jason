namespace Baker_Point.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ji : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.likedLists",
                c => new
                    {
                        likedListId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BlogsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.likedListId)
                .ForeignKey("dbo.Blogs", t => t.BlogsId, cascadeDelete: true)
                .Index(t => t.BlogsId);
            
            AddColumn("dbo.Blogs", "likedNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.likedLists", new[] { "BlogsId" });
            DropForeignKey("dbo.likedLists", "BlogsId", "dbo.Blogs");
            DropColumn("dbo.Blogs", "likedNumber");
            DropTable("dbo.likedLists");
        }
    }
}
