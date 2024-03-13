namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredPhoneNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Personnels", "phone_number", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Personnels", "phone_number", c => c.String(nullable: false));
        }
    }
}
