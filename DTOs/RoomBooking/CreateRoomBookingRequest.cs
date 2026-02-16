namespace ICMS.Api.DTOs.RoomBooking;

using System.ComponentModel.DataAnnotations;

public class CreateRoomBookingRequest
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
}
