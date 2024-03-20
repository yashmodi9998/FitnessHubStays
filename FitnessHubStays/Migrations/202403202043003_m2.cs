namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Bookings", "Booking");
            CreateTable(
                "dbo.BookingSessions",
                c => new
                    {
                        BookingSessionID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        WorkoutSessionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingSessionID)
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .ForeignKey("dbo.WorkoutSessions", t => t.WorkoutSessionID, cascadeDelete: true)
                .Index(t => t.BookingID)
                .Index(t => t.WorkoutSessionID);
            
            CreateIndex("dbo.Bookings", new[] { "UserID", "RoomID" }, unique: true, name: "Booking");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingSessions", "WorkoutSessionID", "dbo.WorkoutSessions");
            DropForeignKey("dbo.BookingSessions", "BookingID", "dbo.Bookings");
            DropIndex("dbo.BookingSessions", new[] { "WorkoutSessionID" });
            DropIndex("dbo.BookingSessions", new[] { "BookingID" });
            DropIndex("dbo.Bookings", "Booking");
            DropTable("dbo.BookingSessions");
            CreateIndex("dbo.Bookings", new[] { "UserID", "RoomID", "CheckInDate" }, unique: true, name: "Booking");
        }
    }
}
