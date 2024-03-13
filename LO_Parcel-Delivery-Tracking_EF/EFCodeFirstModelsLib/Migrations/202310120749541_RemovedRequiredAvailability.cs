namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredAvailability : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Personnels", "availability", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Personnels", "availability", c => c.String(nullable: false));
        }
    }
}
