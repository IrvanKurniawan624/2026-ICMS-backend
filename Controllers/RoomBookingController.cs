using ICMS.Api.Data;
using ICMS.Api.DTOs.RoomBooking;
using ICMS.Api.Enums;
using ICMS.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomBookingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomBookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomBookingResponse>>> GetAll()
    {
        var bookings = await _context.RoomBookings
            .Where(b => b.DeletedAt == null)
            .Select(b => new RoomBookingResponse
            {
                Id = b.Id,
                BookerName = b.BookerName,
                RoomName = b.RoomName,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status
            })
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomBookingResponse>> GetById(Guid id)
    {
        var booking = await _context.RoomBookings
            .Where(b => b.Id == id && b.DeletedAt == null)
            .Select(b => new RoomBookingResponse
            {
                Id = b.Id,
                BookerName = b.BookerName,
                RoomName = b.RoomName,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status
            })
            .FirstOrDefaultAsync();

        if (booking == null)
            return NotFound();

        return Ok(booking);
    }

    [HttpPost]
    public async Task<ActionResult<RoomBookingResponse>> Create(CreateRoomBookingRequest request)
    {
        var startUtc = DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(request.EndTime, DateTimeKind.Utc);

        if (endUtc <= startUtc)
            return BadRequest("EndTime must be greater than StartTime.");

        var booking = new RoomBooking
        {
            Id = Guid.NewGuid(),
            BookerName = request.BookerName,
            RoomName = request.RoomName,
            StartTime = startUtc,
            EndTime = endUtc,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.RoomBookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, new RoomBookingResponse
        {
            Id = booking.Id,
            BookerName = booking.BookerName,
            RoomName = booking.RoomName,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status
        });
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<RoomBookingResponse>> Update(Guid id, UpdateRoomBookingRequest request)
    {
        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

        if (booking == null)
            return NotFound();

        var startUtc = DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(request.EndTime, DateTimeKind.Utc);

        if (endUtc <= startUtc)
            return BadRequest("EndTime must be greater than StartTime.");

        booking.BookerName = request.BookerName;
        booking.RoomName = request.RoomName;
        booking.StartTime = startUtc;
        booking.EndTime = endUtc;
        booking.Status = request.Status;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new RoomBookingResponse
        {
            Id = booking.Id,
            BookerName = booking.BookerName,
            RoomName = booking.RoomName,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

        if (booking == null)
            return NotFound();

        booking.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("deleted")]
    public async Task<ActionResult<IEnumerable<RoomBookingResponse>>> GetDeleted()
    {
        var bookings = await _context.RoomBookings
            .Where(b => b.DeletedAt != null)
            .Select(b => new RoomBookingResponse
            {
                Id = b.Id,
                BookerName = b.BookerName,
                RoomName = b.RoomName,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status
            })
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt != null);

        if (booking == null)
            return NotFound();

        booking.DeletedAt = null;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPatch("{id}/approve")]
    public async Task<IActionResult> ApproveBooking(Guid id)
    {
        var booking = await _context.RoomBookings.FindAsync(id);

        if (booking == null)
            return NotFound();

        if (booking.Status != BookingStatus.Pending)
            return BadRequest("Only pending bookings can be approved.");

        booking.Status = BookingStatus.Approved;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Booking approved." });
    }

    [HttpPatch("{id}/reject")]
    public async Task<IActionResult> RejectBooking(Guid id)
    {
        var booking = await _context.RoomBookings.FindAsync(id);

        if (booking == null)
            return NotFound();

        if (booking.Status != BookingStatus.Pending)
            return BadRequest("Only pending bookings can be rejected.");

        booking.Status = BookingStatus.Rejected;

        await _context.SaveChangesAsync();

        return Ok(new { message = "Booking rejected." });
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveBookings()
    {
        var now = DateTime.UtcNow;

        var bookings = await _context.RoomBookings
            .Where(b =>
                b.DeletedAt == null &&
                (
                    b.Status == BookingStatus.Pending || 
                    b.EndTime.ToUniversalTime() >= now
                )
            )
            .ToListAsync();


        return Ok(bookings);
    }




}
