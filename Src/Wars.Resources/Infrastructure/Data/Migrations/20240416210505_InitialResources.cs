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
                name: "Resources",
                schema: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VillageId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Clay = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    Iron = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    Wood = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    WarehouseCapacity = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_VillageId",
                schema: "Resources",
                table: "Resources",
                column: "VillageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resources",
                schema: "Resources");
        }
    }
}
