namespace ReadLater.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Rank_Bookmark_Field : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Bookmarks", "Rank", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bookmarks", "Rank", c => c.Int());
        }
    }
}
