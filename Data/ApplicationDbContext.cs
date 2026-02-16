using ICMS.Api.Enums;
using ICMS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ICMS.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //? Force Without Timezone (on pgSQL)
        modelBuilder.Entity<RoomBooking>()
            .Property(b => b.StartTime)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<RoomBooking>()
            .Property(b => b.EndTime)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<RoomBooking>()
            .Property(b => b.CreatedAt)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<RoomBooking>()
            .Property(b => b.UpdatedAt)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<RoomBooking>()
            .Property(b => b.DeletedAt)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<RoomBooking>().HasData(
            new RoomBooking
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                BookerName = "Irvan Aditya Kurniawan",
                RoomName = "C231",
                StartTime = new DateTime(2026, 1, 1, 8, 0, 0),
                EndTime = new DateTime(2026, 1, 1, 10, 0, 0),
                Status = BookingStatus.Pending,
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );

    }

}
