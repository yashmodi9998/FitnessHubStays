namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig11 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BookingSessions", new[] { "BookingID" });
            DropIndex("dbo.BookingSessions", new[] { "ActivityID" });
            CreateIndex("dbo.BookingSessions", new[] { "BookingID", "ActivityID" }, unique: true, name: "Booking Activity");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BookingSessions", "Booking Activity");
            CreateIndex("dbo.BookingSessions", "ActivityID");
            CreateIndex("dbo.BookingSessions", "BookingID");
        }
    }
}
