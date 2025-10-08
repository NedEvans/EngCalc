using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringCalc.Migrations
{
    /// <inheritdoc />
    public partial class AddAppConstantsAndEnhanceGlobalConstants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppConstantId",
                table: "GlobalConstants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "GlobalConstants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "GlobalConstants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GlobalConstantBindings",
                table: "CardInstances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GlobalConstantOverrides",
                table: "CardInstances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InputSnapshot",
                table: "CardInstances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppConstants",
                columns: table => new
                {
                    AppConstantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConstantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Standard = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConstants", x => x.AppConstantId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalConstants_AppConstantId",
                table: "GlobalConstants",
                column: "AppConstantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppConstants_ConstantName",
                table: "AppConstants",
                column: "ConstantName");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalConstants_AppConstants_AppConstantId",
                table: "GlobalConstants",
                column: "AppConstantId",
                principalTable: "AppConstants",
                principalColumn: "AppConstantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GlobalConstants_AppConstants_AppConstantId",
                table: "GlobalConstants");

            migrationBuilder.DropTable(
                name: "AppConstants");

            migrationBuilder.DropIndex(
                name: "IX_GlobalConstants_AppConstantId",
                table: "GlobalConstants");

            migrationBuilder.DropColumn(
                name: "AppConstantId",
                table: "GlobalConstants");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "GlobalConstants");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "GlobalConstants");

            migrationBuilder.DropColumn(
                name: "GlobalConstantBindings",
                table: "CardInstances");

            migrationBuilder.DropColumn(
                name: "GlobalConstantOverrides",
                table: "CardInstances");

            migrationBuilder.DropColumn(
                name: "InputSnapshot",
                table: "CardInstances");
        }
    }
}
