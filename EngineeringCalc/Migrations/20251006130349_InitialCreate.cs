using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringCalc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CardVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CodeTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MathMLFormula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InputVariables = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutputVariables = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesignLoadVariable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CapacityVariable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "MaterialLibrary",
                columns: table => new
                {
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Standard = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLibrary", x => x.MaterialId);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Jobs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GlobalVariables",
                columns: table => new
                {
                    GlobalVariableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    VariableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VariableValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalVariables", x => x.GlobalVariableId);
                    table.ForeignKey(
                        name: "FK_GlobalVariables_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculationRevisions",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalculationId = table.Column<int>(type: "int", nullable: false),
                    RevisionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationRevisions", x => x.RevisionId);
                });

            migrationBuilder.CreateTable(
                name: "Calculations",
                columns: table => new
                {
                    CalculationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    CalculationTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CurrentRevisionId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculations", x => x.CalculationId);
                    table.ForeignKey(
                        name: "FK_Calculations_CalculationRevisions_CurrentRevisionId",
                        column: x => x.CurrentRevisionId,
                        principalTable: "CalculationRevisions",
                        principalColumn: "RevisionId");
                    table.ForeignKey(
                        name: "FK_Calculations_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInstances",
                columns: table => new
                {
                    CardInstanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    CalculationRevisionId = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    LocalVariables = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalculatedResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignLoad = table.Column<double>(type: "float", nullable: true),
                    CalculatedCapacity = table.Column<double>(type: "float", nullable: true),
                    CheckResult = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastCalculated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInstances", x => x.CardInstanceId);
                    table.ForeignKey(
                        name: "FK_CardInstances_CalculationRevisions_CalculationRevisionId",
                        column: x => x.CalculationRevisionId,
                        principalTable: "CalculationRevisions",
                        principalColumn: "RevisionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardInstances_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculationRevisions_CalculationId",
                table: "CalculationRevisions",
                column: "CalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_Calculations_CurrentRevisionId",
                table: "Calculations",
                column: "CurrentRevisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Calculations_JobId_CalculationTitle",
                table: "Calculations",
                columns: new[] { "JobId", "CalculationTitle" });

            migrationBuilder.CreateIndex(
                name: "IX_CardInstances_CalculationRevisionId",
                table: "CardInstances",
                column: "CalculationRevisionId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInstances_CardId",
                table: "CardInstances",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardType",
                table: "Cards",
                column: "CardType");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalVariables_JobId_VariableName",
                table: "GlobalVariables",
                columns: new[] { "JobId", "VariableName" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProjectId_JobName",
                table: "Jobs",
                columns: new[] { "ProjectId", "JobName" });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLibrary_MaterialType_Grade",
                table: "MaterialLibrary",
                columns: new[] { "MaterialType", "Grade" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectName",
                table: "Projects",
                column: "ProjectName");

            migrationBuilder.AddForeignKey(
                name: "FK_CalculationRevisions_Calculations_CalculationId",
                table: "CalculationRevisions",
                column: "CalculationId",
                principalTable: "Calculations",
                principalColumn: "CalculationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalculationRevisions_Calculations_CalculationId",
                table: "CalculationRevisions");

            migrationBuilder.DropTable(
                name: "CardInstances");

            migrationBuilder.DropTable(
                name: "GlobalVariables");

            migrationBuilder.DropTable(
                name: "MaterialLibrary");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Calculations");

            migrationBuilder.DropTable(
                name: "CalculationRevisions");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
