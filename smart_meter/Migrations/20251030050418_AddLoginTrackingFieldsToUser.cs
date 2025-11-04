using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smart_meter.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginTrackingFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "failedlogincount",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastfailedloginutc",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lockoutendutc",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);
        }

        // / <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "failedlogincount",
                table: "User");

            migrationBuilder.DropColumn(
                name: "lastfailedloginutc",
                table: "User");

            migrationBuilder.DropColumn(
                name: "lockoutendutc",
                table: "User");
        }
    }
}
