using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations.Randevu
{
    public partial class RandevuCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Poliklinikler",
                columns: table => new
                {
                    PoliklinikId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PoliklinikAd = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poliklinikler", x => x.PoliklinikId);
                });

            migrationBuilder.CreateTable(
                name: "Doktorlar",
                columns: table => new
                {
                    DoktorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DoktorAd = table.Column<string>(type: "TEXT", nullable: true),
                    PoliklinikId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doktorlar", x => x.DoktorId);
                    table.ForeignKey(
                        name: "FK_Doktorlar_Poliklinikler_PoliklinikId",
                        column: x => x.PoliklinikId,
                        principalTable: "Poliklinikler",
                        principalColumn: "PoliklinikId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    RandevuId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HastaAd = table.Column<string>(type: "TEXT", nullable: true),
                    HastaTc = table.Column<string>(type: "TEXT", nullable: true),
                    HastaTel = table.Column<string>(type: "TEXT", nullable: true),
                    DoktorId = table.Column<int>(type: "INTEGER", nullable: false),
                    RandevuTarih = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.RandevuId);
                    table.ForeignKey(
                        name: "FK_Randevular_Doktorlar_DoktorId",
                        column: x => x.DoktorId,
                        principalTable: "Doktorlar",
                        principalColumn: "DoktorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doktorlar_PoliklinikId",
                table: "Doktorlar",
                column: "PoliklinikId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_DoktorId",
                table: "Randevular",
                column: "DoktorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "Doktorlar");

            migrationBuilder.DropTable(
                name: "Poliklinikler");
        }
    }
}
