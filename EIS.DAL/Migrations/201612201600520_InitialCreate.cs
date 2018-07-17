namespace EIS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Email = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.String(),
                        Contact = c.String(),
                        Address = c.String(),
                        DOJ = c.DateTime(nullable: false),
                        Designation = c.String(),
                        TotalExp = c.Double(),
                        RelevantExp = c.Double(),
                        BankName = c.String(),
                        IFSCCode = c.String(),
                        AcNo = c.String(),
                        PAN = c.String(),
                        RoleId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        RoleCode = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "RoleId", "dbo.Role");
            DropIndex("dbo.Employee", new[] { "RoleId" });
            DropTable("dbo.Role");
            DropTable("dbo.Employee");
        }
    }
}
