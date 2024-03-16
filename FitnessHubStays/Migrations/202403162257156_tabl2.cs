namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabl2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Bookings", new[] { "UserID" });
            DropIndex("dbo.Bookings", new[] { "RoomID" });
            DropIndex("dbo.Bookings", new[] { "WorkoutSessionID" });
            CreateIndex("dbo.Bookings", new[] { "UserID", "RoomID", "WorkoutSessionID" }, unique: true, name: "Booking");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Bookings", "Booking");
            CreateIndex("dbo.Bookings", "WorkoutSessionID");
            CreateIndex("dbo.Bookings", "RoomID");
            CreateIndex("dbo.Bookings", "UserID");
        }
    }
}
