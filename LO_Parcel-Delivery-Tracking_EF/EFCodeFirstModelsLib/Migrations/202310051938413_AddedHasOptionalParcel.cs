namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHasOptionalParcel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Parcels", new[] { "sender_id" });
            DropIndex("dbo.Parcels", new[] { "receiver_id" });
            AlterColumn("dbo.Parcels", "sender_id", c => c.Int());
            AlterColumn("dbo.Parcels", "receiver_id", c => c.Int());
            CreateIndex("dbo.Parcels", "sender_id");
            CreateIndex("dbo.Parcels", "receiver_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Parcels", new[] { "receiver_id" });
            DropIndex("dbo.Parcels", new[] { "sender_id" });
            AlterColumn("dbo.Parcels", "receiver_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Parcels", "sender_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Parcels", "receiver_id");
            CreateIndex("dbo.Parcels", "sender_id");
        }
    }
}
