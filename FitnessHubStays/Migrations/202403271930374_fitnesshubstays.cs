namespace FitnessHubStays.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fitnesshubstays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityID = c.Int(nullable: false, identity: true),
                        ActivityName = c.String(),
                        ActivityDate = c.DateTime(nullable: false),
                        ActivityDuration = c.Int(nullable: false),
                        ActivityPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ActivityID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        RoomID = c.Int(nullable: false),
                        CheckInDate = c.DateTime(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Rooms", t => t.RoomID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .Index(t => new { t.UserID, t.RoomID }, unique: true, name: "Booking");
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        RoomID = c.Int(nullable: false, identity: true),
                        RoomNumber = c.String(),
                        RoomType = c.String(),
                        RoomPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RoomStatus = c.String(),
                    })
                .PrimaryKey(t => t.RoomID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.BookingSessions",
                c => new
                    {
                        BookingSessionID = c.Int(nullable: false, identity: true),
                        BookingID = c.Int(nullable: false),
                        ActivityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingSessionID)
                .ForeignKey("dbo.Activities", t => t.ActivityID, cascadeDelete: true)
                .ForeignKey("dbo.Bookings", t => t.BookingID, cascadeDelete: true)
                .Index(t => new { t.BookingID, t.ActivityID }, unique: true, name: "Booking Activity");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BookingSessions", "BookingID", "dbo.Bookings");
            DropForeignKey("dbo.BookingSessions", "ActivityID", "dbo.Activities");
            DropForeignKey("dbo.Bookings", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "RoomID", "dbo.Rooms");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.BookingSessions", "Booking Activity");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bookings", "Booking");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.BookingSessions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Rooms");
            DropTable("dbo.Bookings");
            DropTable("dbo.Activities");
        }
    }
}
