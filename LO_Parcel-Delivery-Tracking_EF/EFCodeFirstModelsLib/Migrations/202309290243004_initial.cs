namespace EFCodeFirstModelsLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        address_id = c.Int(nullable: false, identity: true),
                        address_line = c.String(nullable: false),
                        city = c.String(nullable: false),
                        suburb = c.String(),
                        PostalCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.address_id);
            
            CreateTable(
                "dbo.Parcel_participants",
                c => new
                    {
                        participant_id = c.Int(nullable: false, identity: true),
                        participant_name = c.String(),
                        address_id = c.Int(nullable: false),
                        phone_number = c.String(nullable: false),
                        email_address = c.String(),
                    })
                .PrimaryKey(t => t.participant_id)
                .ForeignKey("dbo.Address", t => t.address_id, cascadeDelete: true)
                .Index(t => t.address_id);
            
            CreateTable(
                "dbo.Receivers",
                c => new
                    {
                        receiver_id = c.Int(nullable: false, identity: true),
                        participant_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.receiver_id)
                .ForeignKey("dbo.Parcel_participants", t => t.participant_id, cascadeDelete: false)
                .Index(t => t.participant_id);
            
            CreateTable(
                "dbo.Parcels",
                c => new
                    {
                        parcel_id = c.Int(nullable: false, identity: true),
                        sender_id = c.Int(nullable: false),
                        receiver_id = c.Int(nullable: false),
                        weight = c.Double(nullable: false),
                        parcel_status = c.String(),
                        delivery_date = c.DateTime(),
                        additional_notes = c.String(),
                        Parcel_ParcelId = c.Int(),
                    })
                .PrimaryKey(t => t.parcel_id)
                .ForeignKey("dbo.Parcels", t => t.Parcel_ParcelId)
                .ForeignKey("dbo.Receivers", t => t.receiver_id, cascadeDelete: true)
                .ForeignKey("dbo.Senders", t => t.sender_id, cascadeDelete: true)
                .Index(t => t.sender_id)
                .Index(t => t.receiver_id)
                .Index(t => t.Parcel_ParcelId);
            
            CreateTable(
                "dbo.Personnels",
                c => new
                    {
                        personnel_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(nullable: false),
                        last_name = c.String(nullable: false),
                        phone_number = c.String(nullable: false),
                        email_address = c.String(),
                        personnel_role = c.String(),
                        availability = c.String(nullable: false),
                        Parcel_ParcelId = c.Int(),
                    })
                .PrimaryKey(t => t.personnel_id)
                .ForeignKey("dbo.Parcels", t => t.Parcel_ParcelId)
                .Index(t => t.Parcel_ParcelId);
            
            CreateTable(
                "dbo.Senders",
                c => new
                    {
                        sender_id = c.Int(nullable: false, identity: true),
                        participant_id = c.Int(nullable: false),
                        TypeOfSender = c.String(),
                    })
                .PrimaryKey(t => t.sender_id)
                .ForeignKey("dbo.Parcel_participants", t => t.participant_id, cascadeDelete: false)
                .Index(t => t.participant_id);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        delivery_id = c.Int(nullable: false, identity: true),
                        parcel_id = c.Int(nullable: false),
                        personnel_id = c.Int(nullable: false),
                        delivery_status = c.String(),
                        delivery_date = c.DateTime(),
                    })
                .PrimaryKey(t => t.delivery_id)
                .ForeignKey("dbo.Parcels", t => t.parcel_id, cascadeDelete: true)
                .ForeignKey("dbo.Personnels", t => t.personnel_id, cascadeDelete: true)
                .Index(t => t.parcel_id)
                .Index(t => t.personnel_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deliveries", "personnel_id", "dbo.Personnels");
            DropForeignKey("dbo.Deliveries", "parcel_id", "dbo.Parcels");
            DropForeignKey("dbo.Parcels", "sender_id", "dbo.Senders");
            DropForeignKey("dbo.Senders", "participant_id", "dbo.Parcel_participants");
            DropForeignKey("dbo.Parcels", "receiver_id", "dbo.Receivers");
            DropForeignKey("dbo.Personnels", "Parcel_ParcelId", "dbo.Parcels");
            DropForeignKey("dbo.Parcels", "Parcel_ParcelId", "dbo.Parcels");
            DropForeignKey("dbo.Receivers", "participant_id", "dbo.Parcel_participants");
            DropForeignKey("dbo.Parcel_participants", "address_id", "dbo.Address");
            DropIndex("dbo.Deliveries", new[] { "personnel_id" });
            DropIndex("dbo.Deliveries", new[] { "parcel_id" });
            DropIndex("dbo.Senders", new[] { "participant_id" });
            DropIndex("dbo.Personnels", new[] { "Parcel_ParcelId" });
            DropIndex("dbo.Parcels", new[] { "Parcel_ParcelId" });
            DropIndex("dbo.Parcels", new[] { "receiver_id" });
            DropIndex("dbo.Parcels", new[] { "sender_id" });
            DropIndex("dbo.Receivers", new[] { "participant_id" });
            DropIndex("dbo.Parcel_participants", new[] { "address_id" });
            DropTable("dbo.Deliveries");
            DropTable("dbo.Senders");
            DropTable("dbo.Personnels");
            DropTable("dbo.Parcels");
            DropTable("dbo.Receivers");
            DropTable("dbo.Parcel_participants");
            DropTable("dbo.Address");
        }
    }
}
