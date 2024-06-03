using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAPS_App.Migrations
{
    /// <inheritdoc />
    public partial class SAPSUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CriminalRecords_Case_Managers_ManagerId",
                table: "CriminalRecords");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "CriminalRecords",
                newName: "CaseManagerNo");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "CriminalRecords",
                newName: "CaseManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalRecords_ManagerId",
                table: "CriminalRecords",
                newName: "IX_CriminalRecords_CaseManagerNo");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Case_Managers",
                newName: "CaseManagerNo");

            migrationBuilder.AddColumn<string>(
                name: "CaseManagerId",
                table: "Case_Managers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalRecords_Case_Managers_CaseManagerNo",
                table: "CriminalRecords",
                column: "CaseManagerNo",
                principalTable: "Case_Managers",
                principalColumn: "CaseManagerNo",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CriminalRecords_Case_Managers_CaseManagerNo",
                table: "CriminalRecords");

            migrationBuilder.DropColumn(
                name: "CaseManagerId",
                table: "Case_Managers");

            migrationBuilder.RenameColumn(
                name: "CaseManagerNo",
                table: "CriminalRecords",
                newName: "ManagerId");

            migrationBuilder.RenameColumn(
                name: "CaseManagerId",
                table: "CriminalRecords",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_CriminalRecords_CaseManagerNo",
                table: "CriminalRecords",
                newName: "IX_CriminalRecords_ManagerId");

            migrationBuilder.RenameColumn(
                name: "CaseManagerNo",
                table: "Case_Managers",
                newName: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CriminalRecords_Case_Managers_ManagerId",
                table: "CriminalRecords",
                column: "ManagerId",
                principalTable: "Case_Managers",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
