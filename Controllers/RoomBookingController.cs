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
        if (request.EndTime <= request.StartTime)
            return BadRequest("EndTime must be greater than StartTime.");

        var booking = new RoomBooking
        {
            Id = Guid.NewGuid(),
            BookerName = request.BookerName,
            RoomName = request.RoomName,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.Now
        };

        _context.RoomBookings.Add(booking);
        await _context.SaveChangesAsync();

        var response = new RoomBookingResponse
        {
            Id = booking.Id,
            BookerName = booking.BookerName,
            RoomName = booking.RoomName,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status
        };

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RoomBookingResponse>> Update(Guid id, UpdateRoomBookingRequest request)
    {
        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

        if (booking == null)
            return NotFound();

        if (request.EndTime <= request.StartTime)
            return BadRequest("EndTime must be greater than StartTime.");

        booking.BookerName = request.BookerName;
        booking.RoomName = request.RoomName;
        booking.StartTime = request.StartTime;
        booking.EndTime = request.EndTime;
        booking.Status = request.Status;
        booking.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        var response = new RoomBookingResponse
        {
            Id = booking.Id,
            BookerName = booking.BookerName,
            RoomName = booking.RoomName,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

        if (booking == null)
            return NotFound();

        booking.DeletedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return NoContent();
    }



}
