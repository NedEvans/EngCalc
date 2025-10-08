using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringCalc.Migrations
{
    /// <inheritdoc />
    public partial class RenameGlobalVariableToGlobalConstant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old index
            migrationBuilder.DropIndex(
                name: "IX_GlobalVariables_JobId_VariableName",
                table: "GlobalVariables");

            // Rename table
            migrationBuilder.RenameTable(
                name: "GlobalVariables",
                newName: "GlobalConstants");

            // Rename columns
            migrationBuilder.RenameColumn(
                name: "GlobalVariableId",
                table: "GlobalConstants",
                newName: "GlobalConstantId");

            migrationBuilder.RenameColumn(
                name: "VariableName",
                table: "GlobalConstants",
                newName: "ConstantName");

            migrationBuilder.RenameColumn(
                name: "VariableValue",
                table: "GlobalConstants",
                newName: "ConstantValue");

            // Create new index
            migrationBuilder.CreateIndex(
                name: "IX_GlobalConstants_JobId_ConstantName",
                table: "GlobalConstants",
                columns: new[] { "JobId", "ConstantName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop new index
            migrationBuilder.DropIndex(
                name: "IX_GlobalConstants_JobId_ConstantName",
                table: "GlobalConstants");

            // Rename columns back
            migrationBuilder.RenameColumn(
                name: "ConstantValue",
                table: "GlobalConstants",
                newName: "VariableValue");

            migrationBuilder.RenameColumn(
                name: "ConstantName",
                table: "GlobalConstants",
                newName: "VariableName");

            migrationBuilder.RenameColumn(
                name: "GlobalConstantId",
                table: "GlobalConstants",
                newName: "GlobalVariableId");

            // Rename table back
            migrationBuilder.RenameTable(
                name: "GlobalConstants",
                newName: "GlobalVariables");

            // Create old index
            migrationBuilder.CreateIndex(
                name: "IX_GlobalVariables_JobId_VariableName",
                table: "GlobalVariables",
                columns: new[] { "JobId", "VariableName" });
        }
    }
}
