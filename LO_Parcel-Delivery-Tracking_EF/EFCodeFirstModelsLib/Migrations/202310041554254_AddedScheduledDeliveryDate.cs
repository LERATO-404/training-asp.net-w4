namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedScheduledDeliveryDate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Parcels", name: "delivery_date", newName: "scheduled_delivery_date");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Parcels", name: "scheduled_delivery_date", newName: "delivery_date");
        }
    }
}
