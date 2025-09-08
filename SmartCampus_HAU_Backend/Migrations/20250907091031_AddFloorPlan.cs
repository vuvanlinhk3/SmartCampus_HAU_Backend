using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCampus_HAU_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddFloorPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "units",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "room_devices",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "floorplans",
                columns: table => new
                {
                    floorplan_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    floor_number = table.Column<int>(type: "int", nullable: false),
                    image_url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_floorplans", x => x.floorplan_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_user_name",
                table: "AspNetUsers",
                column: "user_name",
                unique: true,
                filter: "[user_name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_floorplans_floor_number",
                table: "floorplans",
                column: "floor_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "floorplans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_user_name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "units",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "room_devices",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
