using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAPS_App.Migrations
{
    /// <inheritdoc />
    public partial class ExtendCrim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CaseManagerId",
                table: "CriminalRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "IssuerId",
                table: "CriminalRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "CriminalRecords");

            migrationBuilder.AlterColumn<string>(
                name: "CaseManagerId",
                table: "CriminalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
