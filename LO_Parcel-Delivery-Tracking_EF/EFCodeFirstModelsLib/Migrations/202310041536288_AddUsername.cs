namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsername : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Parcels", "receiver_id", "dbo.Receivers");
            DropForeignKey("dbo.Parcels", "sender_id", "dbo.Senders");
            RenameColumn(table: "dbo.Address", name: "PostalCode", newName: "postal_code");
            AddColumn("dbo.Parcel_participants", "user_name", c => c.String());
            AddColumn("dbo.Parcels", "Sender_SenderId", c => c.Int());
            AddColumn("dbo.Parcels", "Receiver_ReceiverId", c => c.Int());
            AddColumn("dbo.Personnels", "user_name", c => c.String());
            AlterColumn("dbo.Address", "address_line", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Address", "city", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Address", "suburb", c => c.String(maxLength: 100));
            AlterColumn("dbo.Address", "postal_code", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Parcels", "additional_notes", c => c.String(maxLength: 255));
            AlterColumn("dbo.Deliveries", "delivery_status", c => c.String(maxLength: 50));
            AlterColumn("dbo.Personnels", "first_name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Personnels", "last_name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Parcels", "Sender_SenderId");
            CreateIndex("dbo.Parcels", "Receiver_ReceiverId");
            AddForeignKey("dbo.Parcels", "Receiver_ReceiverId", "dbo.Receivers", "receiver_id");
            AddForeignKey("dbo.Parcels", "receiver_id", "dbo.Receivers", "receiver_id");
            AddForeignKey("dbo.Parcels", "sender_id", "dbo.Senders", "sender_id");
            AddForeignKey("dbo.Parcels", "Sender_SenderId", "dbo.Senders", "sender_id");
            DropColumn("dbo.Personnels", "personnel_role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Personnels", "personnel_role", c => c.String());
            DropForeignKey("dbo.Parcels", "Sender_SenderId", "dbo.Senders");
            DropForeignKey("dbo.Parcels", "sender_id", "dbo.Senders");
            DropForeignKey("dbo.Parcels", "receiver_id", "dbo.Receivers");
            DropForeignKey("dbo.Parcels", "Receiver_ReceiverId", "dbo.Receivers");
            DropIndex("dbo.Parcels", new[] { "Receiver_ReceiverId" });
            DropIndex("dbo.Parcels", new[] { "Sender_SenderId" });
            AlterColumn("dbo.Personnels", "last_name", c => c.String(nullable: false));
            AlterColumn("dbo.Personnels", "first_name", c => c.String(nullable: false));
            AlterColumn("dbo.Deliveries", "delivery_status", c => c.String());
            AlterColumn("dbo.Parcels", "additional_notes", c => c.String());
            AlterColumn("dbo.Address", "postal_code", c => c.String(nullable: false));
            AlterColumn("dbo.Address", "suburb", c => c.String());
            AlterColumn("dbo.Address", "city", c => c.String(nullable: false));
            AlterColumn("dbo.Address", "address_line", c => c.String(nullable: false));
            DropColumn("dbo.Personnels", "user_name");
            DropColumn("dbo.Parcels", "Receiver_ReceiverId");
            DropColumn("dbo.Parcels", "Sender_SenderId");
            DropColumn("dbo.Parcel_participants", "user_name");
            RenameColumn(table: "dbo.Address", name: "postal_code", newName: "PostalCode");
            AddForeignKey("dbo.Parcels", "sender_id", "dbo.Senders", "sender_id", cascadeDelete: true);
            AddForeignKey("dbo.Parcels", "receiver_id", "dbo.Receivers", "receiver_id", cascadeDelete: true);
        }
    }
}
