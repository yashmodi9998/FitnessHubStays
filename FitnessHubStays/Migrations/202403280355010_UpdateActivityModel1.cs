namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActivityModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "Status");
        }
    }
}
