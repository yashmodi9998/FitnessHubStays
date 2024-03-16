namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabl1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookings", "WorkoutSessionID", "dbo.WorkoutSessions");
            DropIndex("dbo.Bookings", new[] { "WorkoutSessionID" });
            AlterColumn("dbo.Bookings", "WorkoutSessionID", c => c.Int());
            CreateIndex("dbo.Bookings", "WorkoutSessionID");
            AddForeignKey("dbo.Bookings", "WorkoutSessionID", "dbo.WorkoutSessions", "WorkoutSessionID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "WorkoutSessionID", "dbo.WorkoutSessions");
            DropIndex("dbo.Bookings", new[] { "WorkoutSessionID" });
            AlterColumn("dbo.Bookings", "WorkoutSessionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Bookings", "WorkoutSessionID");
            AddForeignKey("dbo.Bookings", "WorkoutSessionID", "dbo.WorkoutSessions", "WorkoutSessionID", cascadeDelete: true);
        }
    }
}
