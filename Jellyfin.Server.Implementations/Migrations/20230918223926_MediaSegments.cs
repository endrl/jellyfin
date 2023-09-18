using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jellyfin.Server.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class MediaSegments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SegmentCommercialAction",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SegmentIntroAction",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SegmentOutroAction",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SegmentPreviewAction",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SegmentRecapAction",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Start = table.Column<double>(type: "REAL", nullable: false),
                    End = table.Column<double>(type: "REAL", nullable: false),
                    CreatorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Action = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => new { x.ItemId, x.Type, x.TypeIndex });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Segments_ItemId",
                table: "Segments",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Segments");

            migrationBuilder.DropColumn(
                name: "SegmentCommercialAction",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SegmentIntroAction",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SegmentOutroAction",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SegmentPreviewAction",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SegmentRecapAction",
                table: "Users");
        }
    }
}
