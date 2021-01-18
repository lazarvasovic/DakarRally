using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DakarRally.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeName = table.Column<string>(type: "TEXT", nullable: false),
                    RepairLength = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleSubtypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubtypeName = table.Column<string>(type: "TEXT", nullable: true),
                    MaxSpeed = table.Column<double>(type: "REAL", nullable: false),
                    LigthMalfunProbab = table.Column<double>(type: "REAL", nullable: false),
                    HeavyMalfunProbab = table.Column<double>(type: "REAL", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleSubtypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleSubtypes_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamName = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    ManufacturingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VehicleStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleSubtypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    RaceId = table.Column<int>(type: "INTEGER", nullable: true),
                    DistanceReached = table.Column<double>(type: "REAL", nullable: false),
                    RepairTimeLeft = table.Column<double>(type: "REAL", nullable: false),
                    FinishTime = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleSubtypes_VehicleSubtypeId",
                        column: x => x.VehicleSubtypeId,
                        principalTable: "VehicleSubtypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MalfunctionStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MalfunctionType = table.Column<int>(type: "INTEGER", nullable: false),
                    Time = table.Column<double>(type: "REAL", nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MalfunctionStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MalfunctionStatistics_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MalfunctionStatistics_VehicleId",
                table: "MalfunctionStatistics",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_RaceId",
                table: "Vehicles",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleSubtypeId",
                table: "Vehicles",
                column: "VehicleSubtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleSubtypes_VehicleTypeId",
                table: "VehicleSubtypes",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MalfunctionStatistics");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "VehicleSubtypes");

            migrationBuilder.DropTable(
                name: "VehicleTypes");
        }
    }
}
