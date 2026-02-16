using System.ComponentModel.DataAnnotations;
using ICMS.Api.Enums;

namespace ICMS.Api.DTOs.RoomBooking;

public class UpdateRoomBookingRequest
{
    [Required]
    
    [MaxLength(100)]
    public string BookerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RoomName { get; set; } = string.Empty;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public BookingStatus Status { get; set; }
}
