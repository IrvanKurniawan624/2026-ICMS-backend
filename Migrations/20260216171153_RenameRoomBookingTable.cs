using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameRoomBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomBooking",
                table: "RoomBooking");

            migrationBuilder.RenameTable(
                name: "RoomBooking",
                newName: "RoomBookings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomBookings",
                table: "RoomBookings",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomBookings",
                table: "RoomBookings");

            migrationBuilder.RenameTable(
                name: "RoomBookings",
                newName: "RoomBooking");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomBooking",
                table: "RoomBooking",
                column: "Id");
        }
    }
}
