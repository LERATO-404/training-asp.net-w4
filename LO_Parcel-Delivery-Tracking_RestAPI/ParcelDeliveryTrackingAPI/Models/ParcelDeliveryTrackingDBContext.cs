using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ParcelDeliveryTrackingAPI.Models
{
    public partial class ParcelDeliveryTrackingDBContext : DbContext
    {
        public ParcelDeliveryTrackingDBContext()
        {
        }

        public ParcelDeliveryTrackingDBContext(DbContextOptions<ParcelDeliveryTrackingDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Delivery> Deliveries { get; set; } = null!;
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; } = null!;
        public virtual DbSet<Parcel> Parcels { get; set; } = null!;
        public virtual DbSet<ParcelParticipant> ParcelParticipants { get; set; } = null!;
        public virtual DbSet<Personnel> Personnels { get; set; } = null!;
        public virtual DbSet<Receiver> Receivers { get; set; } = null!;
        public virtual DbSet<Sender> Senders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.AddressLine)
                    .HasMaxLength(100)
                    .HasColumnName("address_line");

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .HasColumnName("city");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(50)
                    .HasColumnName("postal_code");

                entity.Property(e => e.Suburb)
                    .HasMaxLength(100)
                    .HasColumnName("suburb");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasIndex(e => e.ParcelId, "IX_parcel_id");

                entity.HasIndex(e => e.PersonnelId, "IX_personnel_id");

                entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("delivery_date");

                entity.Property(e => e.DeliveryStatus)
                    .HasMaxLength(50)
                    .HasColumnName("delivery_status");

                entity.Property(e => e.ParcelId).HasColumnName("parcel_id");

                entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");

                entity.HasOne(d => d.Parcel)
                    .WithMany(p => p.Deliveries)
                    .HasForeignKey(d => d.ParcelId)
                    .HasConstraintName("FK_dbo.Deliveries_dbo.Parcels_parcel_id");

                entity.HasOne(d => d.Personnel)
                    .WithMany(p => p.Deliveries)
                    .HasForeignKey(d => d.PersonnelId)
                    .HasConstraintName("FK_dbo.Deliveries_dbo.Personnels_personnel_id");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            modelBuilder.Entity<Parcel>(entity =>
            {
                entity.HasIndex(e => e.ReceiverReceiverId, "IX_Receiver_ReceiverId");

                entity.HasIndex(e => e.SenderSenderId, "IX_Sender_SenderId");

                entity.HasIndex(e => e.ReceiverId, "IX_receiver_id");

                entity.HasIndex(e => e.SenderId, "IX_sender_id");

                entity.Property(e => e.ParcelId).HasColumnName("parcel_id");

                entity.Property(e => e.AdditionalNotes)
                    .HasMaxLength(255)
                    .HasColumnName("additional_notes");

                entity.Property(e => e.ParcelStatus).HasColumnName("parcel_status");

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.Property(e => e.ReceiverReceiverId).HasColumnName("Receiver_ReceiverId");

                entity.Property(e => e.ScheduledDeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("scheduled_delivery_date");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.Property(e => e.SenderSenderId).HasColumnName("Sender_SenderId");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.ParcelReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("FK_dbo.Parcels_dbo.Receivers_receiver_id");

                entity.HasOne(d => d.ReceiverReceiver)
                    .WithMany(p => p.ParcelReceiverReceivers)
                    .HasForeignKey(d => d.ReceiverReceiverId)
                    .HasConstraintName("FK_dbo.Parcels_dbo.Receivers_Receiver_ReceiverId");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.ParcelSenders)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK_dbo.Parcels_dbo.Senders_sender_id");

                entity.HasOne(d => d.SenderSender)
                    .WithMany(p => p.ParcelSenderSenders)
                    .HasForeignKey(d => d.SenderSenderId)
                    .HasConstraintName("FK_dbo.Parcels_dbo.Senders_Sender_SenderId");
            });

            modelBuilder.Entity<ParcelParticipant>(entity =>
            {
                entity.HasKey(e => e.ParticipantId)
                    .HasName("PK_dbo.Parcel_participants");

                entity.ToTable("Parcel_participants");

                entity.HasIndex(e => e.AddressId, "IX_address_id");

                entity.Property(e => e.ParticipantId).HasColumnName("participant_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.EmailAddress).HasColumnName("email_address");

                entity.Property(e => e.ParticipantName).HasColumnName("participant_name");

                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

                entity.Property(e => e.UserName).HasColumnName("user_name");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.ParcelParticipants)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_dbo.Parcel_participants_dbo.Address_address_id");
            });

            modelBuilder.Entity<Personnel>(entity =>
            {
                entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");

                entity.Property(e => e.Availability).HasColumnName("availability");

                entity.Property(e => e.EmailAddress).HasColumnName("email_address");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .HasColumnName("last_name");

                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");

                entity.Property(e => e.UserName).HasColumnName("user_name");
            });

            modelBuilder.Entity<Receiver>(entity =>
            {
                entity.HasIndex(e => e.ParticipantId, "IX_participant_id");

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.Property(e => e.ParticipantId).HasColumnName("participant_id");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.Receivers)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Receivers_dbo.Parcel_participants_participant_id");
            });

            modelBuilder.Entity<Sender>(entity =>
            {
                entity.HasIndex(e => e.ParticipantId, "IX_participant_id");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.Property(e => e.ParticipantId).HasColumnName("participant_id");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.Senders)
                    .HasForeignKey(d => d.ParticipantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.Senders_dbo.Parcel_participants_participant_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
