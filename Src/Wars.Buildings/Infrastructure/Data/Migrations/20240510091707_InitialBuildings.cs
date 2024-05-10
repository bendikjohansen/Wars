using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wars.Buildings.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialBuildings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Buildings");

            migrationBuilder.CreateTable(
                name: "Villages",
                schema: "Buildings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BuildingLevels_ClayPit = table.Column<int>(type: "integer", nullable: false),
                    BuildingLevels_LumberCamp = table.Column<int>(type: "integer", nullable: false),
                    BuildingLevels_IronMine = table.Column<int>(type: "integer", nullable: false),
                    BuildingLevels_Warehouse = table.Column<int>(type: "integer", nullable: false),
                    BuildingLevels_Headquarter = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingUpgrade",
                schema: "Buildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Building = table.Column<int>(type: "integer", nullable: false),
                    Cost_Clay = table.Column<int>(type: "integer", nullable: false),
                    Cost_Iron = table.Column<int>(type: "integer", nullable: false),
                    Cost_Wood = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    VillageId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingUpgrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingUpgrade_Villages_VillageId",
                        column: x => x.VillageId,
                        principalSchema: "Buildings",
                        principalTable: "Villages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuildingUpgrade_VillageId",
                schema: "Buildings",
                table: "BuildingUpgrade",
                column: "VillageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuildingUpgrade",
                schema: "Buildings");

            migrationBuilder.DropTable(
                name: "Villages",
                schema: "Buildings");
        }
    }
}
