namespace Baker_Point.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Changelogs",
                c => new
                    {
                        ChangelogId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        UserId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        BlogsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChangelogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Changelogs");
        }
    }
}
