using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.EntityFramework.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "Version",
                table: "Application",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.UpdateData(
                table: "LocalContext",
                keyColumn: "LocalContextId",
                keyValue: 1,
                column: "Identifier",
                value: "default");

            migrationBuilder.InsertData(
                table: "LocalContext",
                columns: new[] { "LocalContextId", "DateCreated", "DateModified", "Identifier" },
                values: new object[] { 2, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "kyle.sexton@wachter.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalContext",
                keyColumn: "LocalContextId",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Application");

            migrationBuilder.UpdateData(
                table: "LocalContext",
                keyColumn: "LocalContextId",
                keyValue: 1,
                column: "Identifier",
                value: "kyle.sexton@wachter.com");
        }
    }
}
