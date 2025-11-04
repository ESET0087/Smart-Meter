using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace smart_meter.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToConsumer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "passwordhash",
                table: "consumer",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "passwordhash",
                table: "consumer");
        }
    }
}
