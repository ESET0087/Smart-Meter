using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace smart_meter.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orgunit",
                columns: table => new
                {
                    orgunitid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parentid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("orgunit_pkey", x => x.orgunitid);
                    table.ForeignKey(
                        name: "orgunit_parentid_fkey",
                        column: x => x.parentid,
                        principalTable: "orgunit",
                        principalColumn: "orgunitid");
                });

            migrationBuilder.CreateTable(
                name: "tariff",
                columns: table => new
                {
                    tariffid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    effectivefrom = table.Column<DateOnly>(type: "date", nullable: false),
                    effectiveto = table.Column<DateOnly>(type: "date", nullable: true),
                    baserate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    taxrate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tariff_pkey", x => x.tariffid);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    passwordhash = table.Column<byte[]>(type: "bytea", nullable: false),
                    displayname = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    lastloginutc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    failedlogincount = table.Column<int>(type: "integer", nullable: false),
                    lastfailedloginutc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lockoutendutc = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "consumer",
                columns: table => new
                {
                    consumerid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    photo = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    orgunitid = table.Column<int>(type: "integer", nullable: false),
                    tariffid = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'Active'::character varying"),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    createdby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "'system'::character varying"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedby = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consumer_pkey", x => x.consumerid);
                    table.ForeignKey(
                        name: "consumer_orgunitid_fkey",
                        column: x => x.orgunitid,
                        principalTable: "orgunit",
                        principalColumn: "orgunitid");
                    table.ForeignKey(
                        name: "consumer_tariffid_fkey",
                        column: x => x.tariffid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                });

            migrationBuilder.CreateTable(
                name: "tariffslab",
                columns: table => new
                {
                    tariffslabid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariffid = table.Column<int>(type: "integer", nullable: false),
                    fromkwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    tokwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    rateperkwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tariffslab_pkey", x => x.tariffslabid);
                    table.ForeignKey(
                        name: "tariffslab_tariffid_fkey",
                        column: x => x.tariffid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                });

            migrationBuilder.CreateTable(
                name: "todrule",
                columns: table => new
                {
                    todruleid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariffid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    starttime = table.Column<TimeOnly>(type: "time(0) without time zone", precision: 0, scale: 0, nullable: false),
                    endtime = table.Column<TimeOnly>(type: "time(0) without time zone", precision: 0, scale: 0, nullable: false),
                    rateperkwh = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    isdeleted = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("todrule_pkey", x => x.todruleid);
                    table.ForeignKey(
                        name: "todrule_tariffid_fkey",
                        column: x => x.tariffid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                });

            migrationBuilder.CreateTable(
                name: "consumeraddress",
                columns: table => new
                {
                    addressid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consumerid = table.Column<long>(type: "bigint", nullable: false),
                    houseno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    state = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    pincode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consumeraddress_pkey", x => x.addressid);
                    table.ForeignKey(
                        name: "consumeraddress_consumerid_fkey",
                        column: x => x.consumerid,
                        principalTable: "consumer",
                        principalColumn: "consumerid");
                });

            migrationBuilder.CreateTable(
                name: "meter",
                columns: table => new
                {
                    meterserialno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ipaddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    iccid = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    imsi = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    firmware = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    installtsutc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'Active'::character varying"),
                    consumerid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("meter_pkey", x => x.meterserialno);
                    table.ForeignKey(
                        name: "meter_consumerid_fkey",
                        column: x => x.consumerid,
                        principalTable: "consumer",
                        principalColumn: "consumerid");
                });

            migrationBuilder.CreateTable(
                name: "tarrifdetails",
                columns: table => new
                {
                    tarrifdetailid = table.Column<int>(type: "integer", nullable: false),
                    tarrifid = table.Column<int>(type: "integer", nullable: false),
                    tariffslabid = table.Column<int>(type: "integer", nullable: false),
                    todruleid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tarrifdetails_pkey", x => x.tarrifdetailid);
                    table.ForeignKey(
                        name: "tarrifdetails_tariffslabid_fkey",
                        column: x => x.tariffslabid,
                        principalTable: "tariffslab",
                        principalColumn: "tariffslabid");
                    table.ForeignKey(
                        name: "tarrifdetails_tarrifid_fkey",
                        column: x => x.tarrifid,
                        principalTable: "tariff",
                        principalColumn: "tariffid");
                    table.ForeignKey(
                        name: "tarrifdetails_todruleid_fkey",
                        column: x => x.todruleid,
                        principalTable: "todrule",
                        principalColumn: "todruleid");
                });

            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    billid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consumerid = table.Column<long>(type: "bigint", nullable: false),
                    meterserialno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    billingperiodstart = table.Column<DateOnly>(type: "date", nullable: false),
                    billingperiodend = table.Column<DateOnly>(type: "date", nullable: false),
                    totalunitsconsumed = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    baseamount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    taxamount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    totalamount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true, computedColumnSql: "(baseamount + taxamount)", stored: true),
                    generatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    paymentdate = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    duedate = table.Column<DateOnly>(type: "date", nullable: false),
                    ispaid = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    discdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("bill_pkey", x => x.billid);
                    table.ForeignKey(
                        name: "bill_consumerid_fkey",
                        column: x => x.consumerid,
                        principalTable: "consumer",
                        principalColumn: "consumerid");
                    table.ForeignKey(
                        name: "bill_meterserialno_fkey",
                        column: x => x.meterserialno,
                        principalTable: "meter",
                        principalColumn: "meterserialno");
                });

            migrationBuilder.CreateTable(
                name: "meterreadings",
                columns: table => new
                {
                    readingid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    meterserialno = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    readingdatetime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    energyconsumed = table.Column<decimal>(type: "numeric(10,3)", precision: 10, scale: 3, nullable: false),
                    voltage = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    current = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("meterreadings_pkey", x => x.readingid);
                    table.ForeignKey(
                        name: "meterreadings_meterserialno_fkey",
                        column: x => x.meterserialno,
                        principalTable: "meter",
                        principalColumn: "meterserialno");
                });

            migrationBuilder.CreateTable(
                name: "arrears",
                columns: table => new
                {
                    arrearid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    consumerid = table.Column<long>(type: "bigint", nullable: false),
                    billid = table.Column<long>(type: "bigint", nullable: false),
                    arreartype = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    paidstatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValueSql: "'Unpaid'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("arrears_pkey", x => x.arrearid);
                    table.ForeignKey(
                        name: "arrears_billid_fkey",
                        column: x => x.billid,
                        principalTable: "bill",
                        principalColumn: "billid");
                    table.ForeignKey(
                        name: "arrears_consumerid_fkey",
                        column: x => x.consumerid,
                        principalTable: "consumer",
                        principalColumn: "consumerid");
                });

            migrationBuilder.CreateIndex(
                name: "arrears_billid_key",
                table: "arrears",
                column: "billid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_arrears_consumerid",
                table: "arrears",
                column: "consumerid");

            migrationBuilder.CreateIndex(
                name: "IX_bill_consumerid",
                table: "bill",
                column: "consumerid");

            migrationBuilder.CreateIndex(
                name: "IX_bill_meterserialno",
                table: "bill",
                column: "meterserialno");

            migrationBuilder.CreateIndex(
                name: "IX_consumer_orgunitid",
                table: "consumer",
                column: "orgunitid");

            migrationBuilder.CreateIndex(
                name: "IX_consumer_tariffid",
                table: "consumer",
                column: "tariffid");

            migrationBuilder.CreateIndex(
                name: "consumeraddress_consumerid_key",
                table: "consumeraddress",
                column: "consumerid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_meter_consumerid",
                table: "meter",
                column: "consumerid");

            migrationBuilder.CreateIndex(
                name: "IX_meterreadings_meterserialno",
                table: "meterreadings",
                column: "meterserialno");

            migrationBuilder.CreateIndex(
                name: "idx_orgunit_type",
                table: "orgunit",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_orgunit_parentid",
                table: "orgunit",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "idx_tariff_effectivefrom",
                table: "tariff",
                column: "effectivefrom");

            migrationBuilder.CreateIndex(
                name: "IX_tariffslab_tariffid",
                table: "tariffslab",
                column: "tariffid");

            migrationBuilder.CreateIndex(
                name: "IX_tarrifdetails_tariffslabid",
                table: "tarrifdetails",
                column: "tariffslabid");

            migrationBuilder.CreateIndex(
                name: "IX_tarrifdetails_tarrifid",
                table: "tarrifdetails",
                column: "tarrifid");

            migrationBuilder.CreateIndex(
                name: "IX_tarrifdetails_todruleid",
                table: "tarrifdetails",
                column: "todruleid");

            migrationBuilder.CreateIndex(
                name: "idx_tod_name",
                table: "todrule",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_todrule_tariffid",
                table: "todrule",
                column: "tariffid");

            migrationBuilder.CreateIndex(
                name: "User_username_key",
                table: "User",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "arrears");

            migrationBuilder.DropTable(
                name: "consumeraddress");

            migrationBuilder.DropTable(
                name: "meterreadings");

            migrationBuilder.DropTable(
                name: "tarrifdetails");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "bill");

            migrationBuilder.DropTable(
                name: "tariffslab");

            migrationBuilder.DropTable(
                name: "todrule");

            migrationBuilder.DropTable(
                name: "meter");

            migrationBuilder.DropTable(
                name: "consumer");

            migrationBuilder.DropTable(
                name: "orgunit");

            migrationBuilder.DropTable(
                name: "tariff");
        }
    }
}
