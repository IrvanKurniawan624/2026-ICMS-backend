using ICMS.Api.Enums;

namespace ICMS.Api.Models;

public class RoomBooking
{
    public Guid Id { get; set; }
    public string BookerName { get; set; } = string.Empty;
    public string RoomName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
