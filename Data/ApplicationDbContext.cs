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
    public DbSet<RoomBooking> RoomBookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoomBooking>()
        .ToTable("RoomBookings");

        modelBuilder.Entity<RoomBooking>().HasData(
            new RoomBooking
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                BookerName = "Irvan Aditya Kurniawan",
                RoomName = "C231",
                StartTime = new DateTime(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                Status = BookingStatus.Pending,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }
        );

    }

}
