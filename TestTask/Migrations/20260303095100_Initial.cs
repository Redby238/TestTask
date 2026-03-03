using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxiTrips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PickupDatetimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DropoffDatetimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassengerCount = table.Column<int>(type: "int", nullable: true),
                    TripDistance = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    StoreAndFwdFlag = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PULocationID = table.Column<int>(type: "int", nullable: true),
                    DOLocationID = table.Column<int>(type: "int", nullable: true),
                    FareAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TipAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TripDurationSeconds = table.Column<int>(type: "int", nullable: true, computedColumnSql: "DATEDIFF(SECOND, PickupDatetimeUTC, DropoffDatetimeUTC)", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxiTrips", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxiTrips_PULocationID_Includes",
                table: "TaxiTrips",
                column: "PULocationID")
                .Annotation("SqlServer:Include", new[] { "TipAmount" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxiTrips_TripDistance",
                table: "TaxiTrips",
                column: "TripDistance",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_TaxiTrips_TripDuration",
                table: "TaxiTrips",
                column: "TripDurationSeconds",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxiTrips");
        }
    }
}
