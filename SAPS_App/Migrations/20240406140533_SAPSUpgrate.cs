using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAPS_App.Migrations
{
    /// <inheritdoc />
    public partial class SAPSUpgrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManagerName",
                table: "CriminalRecords",
                newName: "CaseManagerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaseManagerName",
                table: "CriminalRecords",
                newName: "ManagerName");
        }
    }
}
