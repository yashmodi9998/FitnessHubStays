namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "WorkoutSessionID", "dbo.WorkoutSessions");
            DropIndex("dbo.Bookings", "Booking");
            CreateIndex("dbo.Bookings", new[] { "UserID", "RoomID", "CheckInDate" }, unique: true, name: "Booking");
            DropColumn("dbo.Bookings", "WorkoutSessionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "WorkoutSessionID", c => c.Int());
            DropIndex("dbo.Bookings", "Booking");
            CreateIndex("dbo.Bookings", new[] { "UserID", "RoomID", "WorkoutSessionID" }, unique: true, name: "Booking");
            AddForeignKey("dbo.Bookings", "WorkoutSessionID", "dbo.WorkoutSessions", "WorkoutSessionID");
        }
    }
}
