using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCampus_HAU_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStatisticsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "room_device_status_histories",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    room_device_id = table.Column<int>(type: "int", nullable: false),
                    device_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    old_status = table.Column<bool>(type: "bit", nullable: false),
                    new_status = table.Column<bool>(type: "bit", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    quantity_affected = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_device_status_histories", x => x.history_id);
                    table.ForeignKey(
                        name: "FK_room_device_status_histories_room_devices_room_device_id",
                        column: x => x.room_device_id,
                        principalTable: "room_devices",
                        principalColumn: "room_device_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_device_status_histories_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "room_id");
                });

            migrationBuilder.CreateTable(
                name: "unit_status_histories",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    unit_id = table.Column<int>(type: "int", nullable: false),
                    device_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    old_status = table.Column<bool>(type: "bit", nullable: false),
                    new_status = table.Column<bool>(type: "bit", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    room_id = table.Column<int>(type: "int", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit_status_histories", x => x.history_id);
                    table.ForeignKey(
                        name: "FK_unit_status_histories_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "room_id");
                    table.ForeignKey(
                        name: "FK_unit_status_histories_units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "units",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_room_device_status_histories_room_device_id_changed_at",
                table: "room_device_status_histories",
                columns: new[] { "room_device_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_room_device_status_histories_room_id",
                table: "room_device_status_histories",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_unit_status_histories_room_id",
                table: "unit_status_histories",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_unit_status_histories_unit_id_changed_at",
                table: "unit_status_histories",
                columns: new[] { "unit_id", "changed_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "room_device_status_histories");

            migrationBuilder.DropTable(
                name: "unit_status_histories");
        }
    }
}
