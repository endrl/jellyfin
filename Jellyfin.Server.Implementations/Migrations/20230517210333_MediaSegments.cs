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
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "jellyfin",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Preferences",
                schema: "jellyfin",
                newName: "Preferences");

            migrationBuilder.RenameTable(
                name: "Permissions",
                schema: "jellyfin",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "ItemDisplayPreferences",
                schema: "jellyfin",
                newName: "ItemDisplayPreferences");

            migrationBuilder.RenameTable(
                name: "ImageInfos",
                schema: "jellyfin",
                newName: "ImageInfos");

            migrationBuilder.RenameTable(
                name: "HomeSection",
                schema: "jellyfin",
                newName: "HomeSection");

            migrationBuilder.RenameTable(
                name: "DisplayPreferences",
                schema: "jellyfin",
                newName: "DisplayPreferences");

            migrationBuilder.RenameTable(
                name: "Devices",
                schema: "jellyfin",
                newName: "Devices");

            migrationBuilder.RenameTable(
                name: "DeviceOptions",
                schema: "jellyfin",
                newName: "DeviceOptions");

            migrationBuilder.RenameTable(
                name: "CustomItemDisplayPreferences",
                schema: "jellyfin",
                newName: "CustomItemDisplayPreferences");

            migrationBuilder.RenameTable(
                name: "ApiKeys",
                schema: "jellyfin",
                newName: "ApiKeys");

            migrationBuilder.RenameTable(
                name: "ActivityLogs",
                schema: "jellyfin",
                newName: "ActivityLogs");

            migrationBuilder.RenameTable(
                name: "AccessSchedules",
                schema: "jellyfin",
                newName: "AccessSchedules");

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
                name: "MediaSegmentCreator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Creator = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSegmentCreator", x => x.Id);
                    table.UniqueConstraint("AK_MediaSegmentCreator_Creator", x => x.Creator);
                });

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
                    table.ForeignKey(
                        name: "FK_Segments_MediaSegmentCreator_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "MediaSegmentCreator",
                        principalColumn: "Creator",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Segments_CreatorId",
                table: "Segments",
                column: "CreatorId");

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

            migrationBuilder.DropTable(
                name: "MediaSegmentCreator");

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

            migrationBuilder.EnsureSchema(
                name: "jellyfin");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "Preferences",
                newName: "Preferences",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permissions",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "ItemDisplayPreferences",
                newName: "ItemDisplayPreferences",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "ImageInfos",
                newName: "ImageInfos",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "HomeSection",
                newName: "HomeSection",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "DisplayPreferences",
                newName: "DisplayPreferences",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "Devices",
                newName: "Devices",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "DeviceOptions",
                newName: "DeviceOptions",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "CustomItemDisplayPreferences",
                newName: "CustomItemDisplayPreferences",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "ApiKeys",
                newName: "ApiKeys",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "ActivityLogs",
                newName: "ActivityLogs",
                newSchema: "jellyfin");

            migrationBuilder.RenameTable(
                name: "AccessSchedules",
                newName: "AccessSchedules",
                newSchema: "jellyfin");
        }
    }
}
