namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BookingSessions", "ActivityID", "dbo.Activities");
            DropForeignKey("dbo.BookingSessions", "BookingID", "dbo.Bookings");
            DropIndex("dbo.BookingSessions", "Booking Activity");
            CreateTable(
                "dbo.BookingActivities",
                c => new
                    {
                        BookingActivityID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        ActivityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingActivityID)
                .ForeignKey("dbo.Activities", t => t.ActivityID, cascadeDelete: true)
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .Index(t => new { t.BookingID, t.ActivityID }, unique: true, name: "Booking Activity");
            
            AddColumn("dbo.Activities", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Activities", "EndTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Activities", "ActivityDay");
            DropTable("dbo.BookingSessions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BookingSessions",
                c => new
                    {
                        BookingSessionID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        ActivityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingSessionID);
            
            AddColumn("dbo.Activities", "ActivityDay", c => c.String());
            DropForeignKey("dbo.BookingActivities", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.BookingActivities", "ActivityID", "dbo.Activities");
            DropIndex("dbo.BookingActivities", "Booking Activity");
            DropColumn("dbo.Activities", "EndTime");
            DropColumn("dbo.Activities", "StartTime");
            DropTable("dbo.BookingActivities");
            CreateIndex("dbo.BookingSessions", new[] { "BookingID", "ActivityID" }, unique: true, name: "Booking Activity");
            AddForeignKey("dbo.BookingSessions", "BookingID", "dbo.Bookings", "BookingID", cascadeDelete: true);
            AddForeignKey("dbo.BookingSessions", "ActivityID", "dbo.Activities", "ActivityID", cascadeDelete: true);
        }
    }
}
