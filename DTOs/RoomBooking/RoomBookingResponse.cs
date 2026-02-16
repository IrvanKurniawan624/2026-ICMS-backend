using ICMS.Api.Enums;

namespace ICMS.Api.DTOs.RoomBooking;

public class RoomBookingResponse
{
    public Guid Id { get; set; }

    public string BookerName { get; set; } = string.Empty;

    public string RoomName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public BookingStatus Status { get; set; }
}
