namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedICollectionsFromParcel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Parcels", "Parcel_ParcelId", "dbo.Parcels");
            DropForeignKey("dbo.Personnels", "Parcel_ParcelId", "dbo.Parcels");
            DropIndex("dbo.Parcels", new[] { "Parcel_ParcelId" });
            DropIndex("dbo.Personnels", new[] { "Parcel_ParcelId" });
            DropColumn("dbo.Parcels", "Parcel_ParcelId");
            DropColumn("dbo.Personnels", "Parcel_ParcelId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Personnels", "Parcel_ParcelId", c => c.Int());
            AddColumn("dbo.Parcels", "Parcel_ParcelId", c => c.Int());
            CreateIndex("dbo.Personnels", "Parcel_ParcelId");
            CreateIndex("dbo.Parcels", "Parcel_ParcelId");
            AddForeignKey("dbo.Personnels", "Parcel_ParcelId", "dbo.Parcels", "parcel_id");
            AddForeignKey("dbo.Parcels", "Parcel_ParcelId", "dbo.Parcels", "parcel_id");
        }
    }
}
