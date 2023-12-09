using Microsoft.EntityFrameworkCore.Migrations;
using System.Globalization;

#nullable disable

namespace Configuration.EntityFramework.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcDeleteConfigValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // https://binick.blog/2022/07/02/how-to-use-a-raw-sql-script-with-entity-framework-migrations/

            // TODO: test this and make it easier to reuse

            MigrationAttribute migrationAttribute = (MigrationAttribute)GetType()
                .GetCustomAttributes(typeof(MigrationAttribute), false)
                .Single();

            string fileName = $"{migrationAttribute.Id}.sql";

            string filePath = string.Format(
                CultureInfo.InvariantCulture,
                "{1}{0}RawMigrations{0}{2}",
                Path.DirectorySeparatorChar,
                AppContext.BaseDirectory,
                fileName);

            string file = File.ReadAllText(filePath);

            migrationBuilder.Sql(file);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // TODO: add a script to safely drop the procedure (if exists)
            // for now just doing this inline
            migrationBuilder.Sql("DROP PROCEDURE dbo.usp_ConfigurationValue_Delete");
        }
    }
}
