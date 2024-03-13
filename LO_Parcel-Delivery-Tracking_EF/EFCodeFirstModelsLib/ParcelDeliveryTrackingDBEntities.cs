using EFCodeFirstModelsLib.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace EFCodeFirstModelsLib
{
    public partial class ParcelDeliveryTrackingDBEntities : DbContext
    {
        public ParcelDeliveryTrackingDBEntities()
            : base("name=ParcelDeliveryTrackingDBEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parcel>()
                .HasOptional(p => p.Sender)
                .WithMany()
                .HasForeignKey(p => p.SenderID)
                .WillCascadeOnDelete(false); // Specify NoAction for delete behavior

            modelBuilder.Entity<Parcel>()
                .HasOptional(p => p.Receiver)
                .WithMany()
                .HasForeignKey(p => p.ReceiverID)
                .WillCascadeOnDelete(false); // Specify NoAction for delete behavior

            modelBuilder.Entity<Address>().Property(a => a.City).IsRequired();

           

        }

        public virtual DbSet<Address> address { get; set; }
        public virtual DbSet<Personnel> personnel { get; set; }
        public virtual DbSet<ParcelParticipant> participants { get; set; }
        public virtual DbSet<Sender> senders { get; set; }
        public virtual DbSet<Receiver> receivers { get; set; }
        public virtual DbSet<Parcel> parcels { get; set; }
        public virtual DbSet<Delivery> deliveries { get; set; }
    }
}
