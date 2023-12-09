using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Configuration.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AbbreviatedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationDataType",
                columns: table => new
                {
                    ConfigurationValueDataTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationDataType", x => x.ConfigurationValueDataTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationEntryType",
                columns: table => new
                {
                    ConfigurationEntryTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationEntryType", x => x.ConfigurationEntryTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Environment",
                columns: table => new
                {
                    EnvironmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniqueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbbreviatedDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environment", x => x.EnvironmentId);
                });

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    LabelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.LabelId);
                });

            migrationBuilder.CreateTable(
                name: "LocalContext",
                columns: table => new
                {
                    LocalContextId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalContext", x => x.LocalContextId);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationEntry",
                columns: table => new
                {
                    ConfigurationEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigurationEntryTypeId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationEntry", x => x.ConfigurationEntryId);
                    table.ForeignKey(
                        name: "FK_ConfigurationEntry_ConfigurationEntryType_ConfigurationEntryTypeId",
                        column: x => x.ConfigurationEntryTypeId,
                        principalTable: "ConfigurationEntryType",
                        principalColumn: "ConfigurationEntryTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfigurationEntry_Label_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Label",
                        principalColumn: "LabelId");
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationValue",
                columns: table => new
                {
                    ConfigurationValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigurationEntryId = table.Column<int>(type: "int", nullable: false),
                    ConfigurationValueDataTypeId = table.Column<int>(type: "int", nullable: true),
                    ApplicationId = table.Column<int>(type: "int", nullable: true),
                    EnvironmentId = table.Column<int>(type: "int", nullable: true),
                    LocalContextId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationValue", x => x.ConfigurationValueId);
                    table.ForeignKey(
                        name: "FK_ConfigurationValue_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "ApplicationId");
                    table.ForeignKey(
                        name: "FK_ConfigurationValue_ConfigurationDataType_ConfigurationValueDataTypeId",
                        column: x => x.ConfigurationValueDataTypeId,
                        principalTable: "ConfigurationDataType",
                        principalColumn: "ConfigurationValueDataTypeId");
                    table.ForeignKey(
                        name: "FK_ConfigurationValue_ConfigurationEntry_ConfigurationEntryId",
                        column: x => x.ConfigurationEntryId,
                        principalTable: "ConfigurationEntry",
                        principalColumn: "ConfigurationEntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfigurationValue_Environment_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environment",
                        principalColumn: "EnvironmentId");
                    table.ForeignKey(
                        name: "FK_ConfigurationValue_LocalContext_LocalContextId",
                        column: x => x.LocalContextId,
                        principalTable: "LocalContext",
                        principalColumn: "LocalContextId");
                });

            migrationBuilder.InsertData(
                table: "Application",
                columns: new[] { "ApplicationId", "AbbreviatedName", "DateCreated", "DateModified", "Description", "DomainId", "IsActive", "IsDeleted", "Name", "UniqueName" },
                values: new object[,]
                {
                    { 1, "Demo App", new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "This is a demo application.", new Guid("500f86a2-65f7-4fc2-836a-2b14f8686209"), true, false, "Demo Application", "Demo Application-500f86a2" },
                    { 2, null, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("a49262fd-9ab9-452e-92b9-bfb742c94bd0"), true, false, "Demo API", "Demo API-a49262fd" },
                    { 3, null, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("dd65fe33-97d0-4632-b7f4-677ffd2fdf14"), false, false, "Demo WinForm Application", "Demo WinForm Application-dd65fe33" }
                });

            migrationBuilder.InsertData(
                table: "ConfigurationDataType",
                columns: new[] { "ConfigurationValueDataTypeId", "DateCreated", "DateModified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "string" },
                    { 2, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "boolean" }
                });

            migrationBuilder.InsertData(
                table: "ConfigurationEntryType",
                columns: new[] { "ConfigurationEntryTypeId", "DateCreated", "DateModified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "application setting" },
                    { 2, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "connection string" },
                    { 3, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "feature toggle" }
                });

            migrationBuilder.InsertData(
                table: "Environment",
                columns: new[] { "EnvironmentId", "AbbreviatedDisplayName", "DateCreated", "DateModified", "DisplayName", "DomainId", "IsActive", "UniqueName" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "Local", new Guid("fc52da1b-439a-4ffa-90f9-a9f7f585deb9"), true, "Local-fc52da1b" },
                    { 2, "Dev", new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "Development", new Guid("69c89b3b-654c-4461-baad-bd4f115d8a11"), true, "Development-69c89b3b" },
                    { 3, "Test", new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "Testing", new Guid("629e10af-8c7e-4b76-9e01-985e28a6a08c"), true, "Testing-629e10af" },
                    { 4, "Stage", new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "Staging", new Guid("105c1e10-8701-43ca-a457-cd01f14591e5"), true, "Staging-105c1e10" },
                    { 5, "Prod", new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "Production", new Guid("bf420f08-a3ec-4bf9-bb87-90dc04c4b6b9"), true, "Production-bf420f08" },
                    { 6, "Train", new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "Training", new Guid("f2000413-1926-44f4-ba3f-19ca8b2da955"), false, "Training-f2000413" }
                });

            migrationBuilder.InsertData(
                table: "LocalContext",
                columns: new[] { "LocalContextId", "DateCreated", "DateModified", "Identifier" },
                values: new object[] { 1, new DateTime(2023, 9, 29, 23, 0, 0, 0, DateTimeKind.Utc), null, "kyle.sexton@wachter.com" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationEntry_ConfigurationEntryTypeId",
                table: "ConfigurationEntry",
                column: "ConfigurationEntryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationEntry_LabelId",
                table: "ConfigurationEntry",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationValue_ApplicationId",
                table: "ConfigurationValue",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationValue_ConfigurationEntryId",
                table: "ConfigurationValue",
                column: "ConfigurationEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationValue_ConfigurationValueDataTypeId",
                table: "ConfigurationValue",
                column: "ConfigurationValueDataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationValue_EnvironmentId",
                table: "ConfigurationValue",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationValue_LocalContextId",
                table: "ConfigurationValue",
                column: "LocalContextId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationValue");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "ConfigurationDataType");

            migrationBuilder.DropTable(
                name: "ConfigurationEntry");

            migrationBuilder.DropTable(
                name: "Environment");

            migrationBuilder.DropTable(
                name: "LocalContext");

            migrationBuilder.DropTable(
                name: "ConfigurationEntryType");

            migrationBuilder.DropTable(
                name: "Label");
        }
    }
}
