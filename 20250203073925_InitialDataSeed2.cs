using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace asp2.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Idk Road");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "idk Road");
        }
    }
}
