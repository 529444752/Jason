namespace Baker_Point.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new23 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeaturedLists",
                c => new
                    {
                        FeaturedListId = c.Int(nullable: false, identity: true),
                        BlogsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FeaturedListId)
                .ForeignKey("dbo.Blogs", t => t.BlogsId, cascadeDelete: true)
                .Index(t => t.BlogsId);
            
            AddColumn("dbo.Blogs", "IsFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.FeaturedLists", new[] { "BlogsId" });
            DropForeignKey("dbo.FeaturedLists", "BlogsId", "dbo.Blogs");
            DropColumn("dbo.Blogs", "IsFeatured");
            DropTable("dbo.FeaturedLists");
        }
    }
}
