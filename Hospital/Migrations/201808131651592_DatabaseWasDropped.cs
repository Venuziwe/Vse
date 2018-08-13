namespace Hospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DatabaseWasDropped : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        CardID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        BirthDay = c.DateTime(nullable: false),
                        Diseases = c.String(),
                    })
                .PrimaryKey(t => t.CardID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        DepartmentID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Visits",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PersonLastName = c.String(),
                        PersonFirstName = c.String(),
                        PersonMiddleName = c.String(),
                        PersonBirthDay = c.DateTime(nullable: false),
                        VisitTime = c.DateTime(nullable: false),
                        Available = c.Boolean(nullable: false),
                        UserID = c.String(),
                        DoctorID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Doctors", t => t.DoctorID)
                .Index(t => t.DoctorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visits", "DoctorID", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Visits", new[] { "DoctorID" });
            DropIndex("dbo.Doctors", new[] { "DepartmentID" });
            DropTable("dbo.Visits");
            DropTable("dbo.Doctors");
            DropTable("dbo.Departments");
            DropTable("dbo.Cards");
        }
    }
}
