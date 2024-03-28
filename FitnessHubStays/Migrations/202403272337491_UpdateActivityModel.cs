namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActivityModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "ActivityDay", c => c.String());
            DropColumn("dbo.Activities", "ActivityDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "ActivityDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Activities", "ActivityDay");
        }
    }
}
