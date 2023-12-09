using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.EntityFramework.Providers.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class EnvironmentAddDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Environment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Environment",
                keyColumn: "EnvironmentId",
                keyValue: 1,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Environment",
                keyColumn: "EnvironmentId",
                keyValue: 2,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Environment",
                keyColumn: "EnvironmentId",
                keyValue: 3,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Environment",
                keyColumn: "EnvironmentId",
                keyValue: 4,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Environment",
                keyColumn: "EnvironmentId",
                keyValue: 5,
                column: "Description",
                value: null);

            migrationBuilder.UpdateData(
                table: "Environment",
                keyColumn: "EnvironmentId",
                keyValue: 6,
                column: "Description",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Environment");
        }
    }
}
