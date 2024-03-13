namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequiredParcel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Parcels", "additional_notes", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Parcels", "additional_notes", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
