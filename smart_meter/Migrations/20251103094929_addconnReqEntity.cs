using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace smart_meter.Migrations
{
    /// <inheritdoc />
    public partial class addconnReqEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "connection_request",
                columns: table => new
                {
                    request_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consumer_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    request_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    approved_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    remarks = table.Column<string>(type: "text", nullable: true),
                    action_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connection_request", x => x.request_id);
                    table.ForeignKey(
                        name: "FK_connection_request_User_action_by",
                        column: x => x.action_by,
                        principalTable: "User",
                        principalColumn: "userid");
                    table.ForeignKey(
                        name: "FK_connection_request_consumer_consumer_id",
                        column: x => x.consumer_id,
                        principalTable: "consumer",
                        principalColumn: "consumerid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_connection_request_action_by",
                table: "connection_request",
                column: "action_by");

            migrationBuilder.CreateIndex(
                name: "IX_connection_request_consumer_id",
                table: "connection_request",
                column: "consumer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "connection_request");
        }
    }
}
