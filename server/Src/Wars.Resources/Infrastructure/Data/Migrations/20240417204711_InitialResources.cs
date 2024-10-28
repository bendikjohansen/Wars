using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wars.Resources.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Resources");

            migrationBuilder.CreateTable(
                name: "Villages",
                schema: "Resources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ResourceBuilding_ClayPit = table.Column<int>(type: "integer", nullable: false),
                    ResourceBuilding_IronMine = table.Column<int>(type: "integer", nullable: false),
                    ResourceBuilding_LumberCamp = table.Column<int>(type: "integer", nullable: false),
                    ResourceInventory_Clay = table.Column<float>(type: "real", precision: 18, scale: 6, nullable: false),
                    ResourceInventory_Iron = table.Column<float>(type: "real", precision: 18, scale: 6, nullable: false),
                    ResourceInventory_UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResourceInventory_WarehouseCapacity = table.Column<int>(type: "integer", nullable: false),
                    ResourceInventory_Wood = table.Column<float>(type: "real", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villages",
                schema: "Resources");
        }
    }
}
