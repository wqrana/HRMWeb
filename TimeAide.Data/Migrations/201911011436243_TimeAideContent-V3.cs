namespace TimeAide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeAideContentV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInformation", "PictureFilePath", c => c.String(maxLength: 512));
            AddColumn("dbo.UserInformation", "ResumeFilePath", c => c.String(maxLength: 512));
            DropColumn("dbo.UserInformation", "imgPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserInformation", "imgPhoto", c => c.Binary(storeType: "image"));
            DropColumn("dbo.UserInformation", "ResumeFilePath");
            DropColumn("dbo.UserInformation", "PictureFilePath");
        }
    }
}
