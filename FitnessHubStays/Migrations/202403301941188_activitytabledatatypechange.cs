namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activitytabledatatypechange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Activities", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "EndTime", c => c.String());
            AlterColumn("dbo.Activities", "StartTime", c => c.String());
        }
    }
}
