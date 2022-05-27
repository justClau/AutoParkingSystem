using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APSDataAccessLibrary.Migrations
{
    public partial class withBuilder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_VehicleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ParkingLots_VehicleId",
                table: "ParkingLots");

            migrationBuilder.CreateIndex(
                name: "IX_Users_VehicleId",
                table: "Users",
                column: "VehicleId",
                unique: true,
                filter: "[VehicleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLots_VehicleId",
                table: "ParkingLots",
                column: "VehicleId",
                unique: true,
                filter: "[VehicleId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_VehicleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ParkingLots_VehicleId",
                table: "ParkingLots");

            migrationBuilder.CreateIndex(
                name: "IX_Users_VehicleId",
                table: "Users",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLots_VehicleId",
                table: "ParkingLots",
                column: "VehicleId");
        }
    }
}
