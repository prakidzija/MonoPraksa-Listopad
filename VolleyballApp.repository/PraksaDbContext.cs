using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VolleyballApp.model;

public partial class PraksaDbContext : DbContext
{
    public PraksaDbContext(DbContextOptions<PraksaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerRegistration> PlayerRegistrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clubs_pkey");

            entity.ToTable("clubs");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(40)
                .HasColumnName("adress");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("players_pkey");

            entity.ToTable("players");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Position)
                .HasMaxLength(20)
                .HasColumnName("position");
        });

        modelBuilder.Entity<PlayerRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("player_registrations_pkey");

            entity.ToTable("player_registrations");

            entity.HasIndex(e => e.ClubId, "idx_registrations_club_id");

            entity.HasIndex(e => e.PlayerId, "idx_registrations_player_id");

            entity.HasIndex(e => new { e.ClubId, e.JerseyNumber }, "player_registrations_club_id_jersey_number_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ClubId).HasColumnName("club_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.JerseyNumber).HasColumnName("jersey_number");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.RegistrationType)
                .HasMaxLength(20)
                .HasColumnName("registration_type");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Club).WithMany(p => p.PlayerRegistrations)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("player_registrations_club_id_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerRegistrations)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("player_registrations_player_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
