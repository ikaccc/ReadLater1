namespace ReadLater.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_User_Bookmark_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookmarks", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookmarks", "UserId");
        }
    }
}
