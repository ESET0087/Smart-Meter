using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smart_meter.Migrations
{
    /// <inheritdoc />
    public partial class hello : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "testing",
                table: "tariff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "testing",
                table: "tariff",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
