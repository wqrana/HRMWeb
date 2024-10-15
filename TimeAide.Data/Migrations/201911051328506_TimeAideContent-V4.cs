namespace TimeAide.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeAideContentV4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserInformation", "SSNEnd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserInformation", "SSNEnd", c => c.String(maxLength: 9));
        }
    }
}
