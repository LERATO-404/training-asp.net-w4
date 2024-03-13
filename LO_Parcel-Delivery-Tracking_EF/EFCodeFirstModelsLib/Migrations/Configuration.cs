namespace EFCodeFirstModelsLib.Migrations
{
    using EFCodeFirstModelsLib.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ParcelDeliveryTrackingDBEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ParcelDeliveryTrackingDBEntities context)
        {
            // Address data
            context.address.AddOrUpdate(a => a.AddressID,
                        new Address() { AddressID = 1, AddressLine = "123 Main St", City = "Cityville", Suburb = "Downtown", PostalCode = "12345" },
                        new Address() { AddressID = 2, AddressLine = "456 Elm St", City = "Townsville", Suburb = "UpTown", PostalCode = "4567" },
                        new Address() { AddressID = 3, AddressLine = "789 Oak St", City = "Metroville", Suburb = "Suburbia", PostalCode = "6789" },
                        new Address() { AddressID = 4, AddressLine = "101 Pine St", City = "Villageton", Suburb = "Ruralvile", PostalCode = "1357" },
                        new Address() { AddressID = 5, AddressLine = "222 Maple St", City = "Cityville", Suburb = "Downtown", PostalCode = "2468" },
                        new Address() { AddressID = 6, AddressLine = "333 Birch St", City = "Townsville", Suburb = "Uptown", PostalCode = "36910" }
                           );

            // personnel data
            context.personnel.AddOrUpdate(pl => pl.PersonnelId,
                    new Personnel() { PersonnelId = 1, FirstName = "David", LastName = "Johnson", EmailAddress = "david@example.com", UserName = "David.Johnson", Availability = "On Duty" },
                    new Personnel() { PersonnelId = 2, FirstName = "Sarah", LastName = "Smith", PhoneNumber = "555-333-4444", EmailAddress = "sarah@example.com", UserName = "SarahS", Availability = "On Duty" },
                    new Personnel() { PersonnelId = 3, FirstName = "Michael", LastName = "Brown", PhoneNumber = "555-555-6666", EmailAddress = "michael@example.com", UserName = "MicBrown", Availability = "Off Duty" },
                    new Personnel() { PersonnelId = 4, FirstName = "Linda", LastName = "Willams", PhoneNumber = "555-777-8888", EmailAddress = "linda@example.com", UserName = "LindaW"},
                    new Personnel() { PersonnelId = 5, FirstName = "Tom", LastName = "Davids", PhoneNumber = "555-999-0000", EmailAddress = "tom@example.com", UserName = "TomD", Availability = "On Duty" },
                    new Personnel() { PersonnelId = 6, FirstName = "Laura", LastName = "Anderson", PhoneNumber = "555-222-3333", EmailAddress = "laura@example.com", UserName = "LauraA" }
            );

            // Parcel Participants data
            context.participants.AddOrUpdate(pp => pp.ParticipantId,
                        new ParcelParticipant() { ParticipantId = 1, ParticipantName = "John Smith", AddressID = 1, PhoneNumber = "235-222-3333", EmailAddress = "JohnSmith@gmail.com", UserName ="JohnS123" },
                        new ParcelParticipant() { ParticipantId = 2, ParticipantName = "Jane Doe", AddressID = 2, PhoneNumber = "455-222-3333", EmailAddress = "Jane@gmail.com", UserName = "JaneDoe" },
                        new ParcelParticipant() { ParticipantId = 3, ParticipantName = "ABC Corp.", AddressID = 3, PhoneNumber = "647-222-3333", EmailAddress = "ABC@gmail.com", UserName = "ABC.Corp" },
                        new ParcelParticipant() { ParticipantId = 4, ParticipantName = "Emily Brown", AddressID = 5, PhoneNumber = "155-222-3333", EmailAddress = "Emily@gmail.com", UserName = "EmilyB" },
                        new ParcelParticipant() { ParticipantId = 5, ParticipantName = "Blue Ltd.", AddressID = 4, PhoneNumber = "115-222-3333", EmailAddress = "Blue@gmail.com", UserName = "Blue.Ltd" },
                        new ParcelParticipant() { ParticipantId = 6, ParticipantName = "Bob Johson", AddressID = 6, PhoneNumber = "535-222-3333", EmailAddress = "Bob@gmail.com", UserName = "Bob.Johnson" }
                           );

            // sender data
            context.senders.AddOrUpdate(s => s.SenderId,
                     new Sender() { SenderId = 1, ParticipantId = 1, TypeOfSender = "Individual" },
                     new Sender() { SenderId = 2, ParticipantId = 3, TypeOfSender = "Business" },
                     new Sender() { SenderId = 3, ParticipantId = 5, TypeOfSender = "Individual" },
                     new Sender() { SenderId = 4, ParticipantId = 6, TypeOfSender = "Individual" },
                     new Sender() { SenderId = 5, ParticipantId = 5, TypeOfSender = "Business" },
                     new Sender() { SenderId = 6, ParticipantId = 4, TypeOfSender = "Individual" }
            );

            // receiver data
            context.receivers.AddOrUpdate(r => r.ReceiverId,
                     new Receiver() { ReceiverId = 1, ParticipantID = 1 },
                     new Receiver() { ReceiverId = 2, ParticipantID = 2 },
                     new Receiver() { ReceiverId = 3, ParticipantID = 3 },
                     new Receiver() { ReceiverId = 4, ParticipantID = 4 },
                     new Receiver() { ReceiverId = 5, ParticipantID = 5 },
                     new Receiver() { ReceiverId = 6, ParticipantID = 6 }
                        );

            // parcel data
            context.parcels.AddOrUpdate(p => p.ParcelId,
                    new Parcel() { ParcelId = 1, SenderID = 1, ReceiverID = 2, Weight = 5.50, ParcelStatus = "In Transit", DeliveryDate = new DateTime(2023, 09, 15, 09, 30, 00), AdditionalNotes = "Fragile item" },
                    new Parcel() { ParcelId = 2, SenderID = 3, ReceiverID = 4, Weight = 8.20, ParcelStatus = "Delivered", DeliveryDate = new DateTime(2023, 09, 10, 14, 15, 00), AdditionalNotes = "Handle with care" },
                    new Parcel() { ParcelId = 3, SenderID = 5, ReceiverID = 6, Weight = 3.10, ParcelStatus = "On Hold", DeliveryDate =  DateTime.Now.AddDays(7), AdditionalNotes = "Customer requested delay" },
                    new Parcel() { ParcelId = 4, SenderID = 2, ReceiverID = 1, Weight = 7.10, ParcelStatus = "In Transit", DeliveryDate = new DateTime(2023, 09, 20, 11, 45, 00), AdditionalNotes = "Express delivery" },
                    new Parcel() { ParcelId = 5, SenderID = 4, ReceiverID = 3, Weight = 6.00, ParcelStatus = "Delivered", DeliveryDate = new DateTime(2023, 09, 05, 13, 20, 00), AdditionalNotes = "" },
                    new Parcel() { ParcelId = 6, SenderID = 6, ReceiverID = 5, Weight = 4.50, ParcelStatus = "In Transit", DeliveryDate = new DateTime(2023, 09, 18, 10, 00, 00), AdditionalNotes = "Fragile item" },
                    new Parcel() { ParcelId = 7, SenderID = 1, ReceiverID = 1, Weight = 7.10, ParcelStatus = "Delivered", DeliveryDate = new DateTime(2022, 10, 20, 11, 45, 00), AdditionalNotes = "Express delivery" },
                    new Parcel() { ParcelId = 8, SenderID = 1, ReceiverID = 3, Weight = 10.00, ParcelStatus = "Delivered", DeliveryDate = new DateTime(2021, 09, 06, 13, 20, 00), AdditionalNotes = "" },
                    new Parcel() { ParcelId = 9, SenderID = 1, ReceiverID = 5, Weight = 4.50, ParcelStatus = "Delivered", DeliveryDate = new DateTime(2022, 09, 18, 11, 00, 00), AdditionalNotes = "" }
            );

            // deliveries data
            context.deliveries.AddOrUpdate(d => d.DeliveryId,
                    new Delivery() { DeliveryId = 1, ParcelID = 1, DeliveryPersonnel = 1, DeliveryStatus = "In Progress" },
                    new Delivery() { DeliveryId = 2, ParcelID = 3, DeliveryPersonnel = 2, DeliveryStatus = "Scheduled" },
                    new Delivery() { DeliveryId = 3, ParcelID = 2, DeliveryPersonnel = 4, DeliveryStatus = "Completed", DeliveryDate = new DateTime(2023, 9, 10, 15, 30, 00) },
                    new Delivery() { DeliveryId = 4, ParcelID = 5, DeliveryPersonnel = 5, DeliveryStatus = "Completed", DeliveryDate = new DateTime (2023, 9, 5, 10,20,00)} ,
                    new Delivery() { DeliveryId = 5, ParcelID = 6, DeliveryPersonnel = 6, DeliveryStatus = "In Progress" },
                    new Delivery() { DeliveryId = 6, ParcelID = 4, DeliveryPersonnel = 3, DeliveryStatus = "In Progress" },
                    new Delivery() { DeliveryId = 7, ParcelID = 7, DeliveryPersonnel = 4, DeliveryStatus = "Completed", DeliveryDate = new DateTime(2022, 10, 19, 15, 30, 00) },
                    new Delivery() { DeliveryId = 8, ParcelID = 8, DeliveryPersonnel = 5, DeliveryStatus = "Completed", DeliveryDate = new DateTime(2021, 9, 5, 10, 20, 00) },
                    new Delivery() { DeliveryId = 9, ParcelID = 9, DeliveryPersonnel = 5, DeliveryStatus = "Completed", DeliveryDate = new DateTime(2021, 9, 18, 10, 20, 00) }
            );
        }
    }
}
