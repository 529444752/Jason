namespace Baker_Point.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _as : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        AttachmentId = c.Int(nullable: false, identity: true),
                        BlogsId = c.Int(nullable: false),
                        TYPE = c.String(),
                        Src = c.String(),
                    })
                .PrimaryKey(t => t.AttachmentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Attachments");
        }
    }
}
