using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EtkinlikYonetimSistemi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class YeniIlkMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IlgiAlanlari",
                columns: table => new
                {
                    IlgiAlaniId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnaKategoriMi = table.Column<bool>(type: "bit", nullable: false),
                    UstKategoriId = table.Column<int>(type: "int", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IlgiAlanlari", x => x.IlgiAlaniId);
                    table.ForeignKey(
                        name: "FK_IlgiAlanlari_IlgiAlanlari_UstKategoriId",
                        column: x => x.UstKategoriId,
                        principalTable: "IlgiAlanlari",
                        principalColumn: "IlgiAlaniId");
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SifreHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnayliMi = table.Column<bool>(type: "bit", nullable: false),
                    LoginSayisi = table.Column<int>(type: "int", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etkinlikler",
                columns: table => new
                {
                    EtkinlikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kapasite = table.Column<int>(type: "int", nullable: false),
                    KalanBilet = table.Column<int>(type: "int", nullable: false),
                    KategoriId = table.Column<int>(type: "int", nullable: false),
                    BiletFiyati = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etkinlikler", x => x.EtkinlikId);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_IlgiAlanlari_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "IlgiAlanlari",
                        principalColumn: "IlgiAlaniId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IlgiAlaniKullanici",
                columns: table => new
                {
                    IlgiAlanlariIlgiAlaniId = table.Column<int>(type: "int", nullable: false),
                    KullanicilarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IlgiAlaniKullanici", x => new { x.IlgiAlanlariIlgiAlaniId, x.KullanicilarId });
                    table.ForeignKey(
                        name: "FK_IlgiAlaniKullanici_IlgiAlanlari_IlgiAlanlariIlgiAlaniId",
                        column: x => x.IlgiAlanlariIlgiAlaniId,
                        principalTable: "IlgiAlanlari",
                        principalColumn: "IlgiAlaniId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IlgiAlaniKullanici_Kullanicilar_KullanicilarId",
                        column: x => x.KullanicilarId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SatinAlimlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SatinAlimId = table.Column<int>(type: "int", nullable: false),
                    SatinAlimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatinAlimlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SatinAlimlar_Kullanicilar_Id",
                        column: x => x.Id,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SatinAlimlar_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sepetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SepetId = table.Column<int>(type: "int", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sepetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sepetler_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Biletler",
                columns: table => new
                {
                    BiletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EtkinlikId = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StokAdedi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biletler", x => x.BiletId);
                    table.ForeignKey(
                        name: "FK_Biletler_Etkinlikler_EtkinlikId",
                        column: x => x.EtkinlikId,
                        principalTable: "Etkinlikler",
                        principalColumn: "EtkinlikId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SatinAlimBiletleri",
                columns: table => new
                {
                    SatinAlimId = table.Column<int>(type: "int", nullable: false),
                    BiletId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatinAlimBiletleri", x => new { x.SatinAlimId, x.BiletId });
                    table.ForeignKey(
                        name: "FK_SatinAlimBiletleri_Biletler_BiletId",
                        column: x => x.BiletId,
                        principalTable: "Biletler",
                        principalColumn: "BiletId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SatinAlimBiletleri_SatinAlimlar_SatinAlimId",
                        column: x => x.SatinAlimId,
                        principalTable: "SatinAlimlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SepetBiletleri",
                columns: table => new
                {
                    SepetId = table.Column<int>(type: "int", nullable: false),
                    BiletId = table.Column<int>(type: "int", nullable: false),
                    Adet = table.Column<int>(type: "int", nullable: false),
                    BiletId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SepetBiletleri", x => new { x.SepetId, x.BiletId });
                    table.ForeignKey(
                        name: "FK_SepetBiletleri_Biletler_BiletId",
                        column: x => x.BiletId,
                        principalTable: "Biletler",
                        principalColumn: "BiletId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SepetBiletleri_Biletler_BiletId1",
                        column: x => x.BiletId1,
                        principalTable: "Biletler",
                        principalColumn: "BiletId");
                    table.ForeignKey(
                        name: "FK_SepetBiletleri_Sepetler_SepetId",
                        column: x => x.SepetId,
                        principalTable: "Sepetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "IlgiAlanlari",
                columns: new[] { "IlgiAlaniId", "Aciklama", "Ad", "AnaKategoriMi", "BasePrice", "UpdatedDate", "UstKategoriId" },
                values: new object[,]
                {
                    { 1, "Müzik etkinlikleri", "Müzik", true, 200.00m, null, 1 },
                    { 2, "Spor etkinlikleri", "Spor", true, 180.00m, null, 2 },
                    { 3, "Teknoloji etkinlikleri", "Teknoloji", true, 150.00m, null, 3 },
                    { 4, "Gezi etkinlikleri", "Gezi", true, 250.00m, null, 4 },
                    { 5, "Tiyatro etkinlikleri", "Tiyatro", true, 300.00m, null, 5 },
                    { 11, "Rock mÃ¼zik etkinlikleri", "Rock", false, 0m, null, 1 },
                    { 12, "Pop mÃ¼zik etkinlikleri", "Pop", false, 0m, null, 1 },
                    { 13, "Klasik mÃ¼zik etkinlikleri", "Klasik", false, 0m, null, 1 },
                    { 14, "Halk mÃ¼ziÄŸi etkinlikleri", "Halk MÃ¼zigi", false, 0m, null, 1 },
                    { 15, "Metal mÃ¼zik etkinlikleri", "Metal", false, 0m, null, 1 },
                    { 16, "Rap mÃ¼zik etkinlikleri", "Rap", false, 0m, null, 1 },
                    { 17, "DJ etkinlikleri", "DJ", false, 0m, null, 1 },
                    { 21, "Futbol maÃ§larÄ±  ", "Futbol", false, 0m, null, 2 },
                    { 22, "Basketbol maÃ§larÄ±", "Basketbol", false, 0m, null, 2 },
                    { 23, "Voleybol maÃ§larÄ±", "Voleybol", false, 0m, null, 2 },
                    { 24, "Formula 1 yarÄ±ÅŸlarÄ±", "Formula 1", false, 0m, null, 2 },
                    { 25, "Doğa yürüyüşleri", "Doga Yuruyusu", false, 0m, null, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_EtkinlikId",
                table: "Biletler",
                column: "EtkinlikId");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_KategoriId",
                table: "Etkinlikler",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_IlgiAlaniKullanici_KullanicilarId",
                table: "IlgiAlaniKullanici",
                column: "KullanicilarId");

            migrationBuilder.CreateIndex(
                name: "IX_IlgiAlanlari_UstKategoriId",
                table: "IlgiAlanlari",
                column: "UstKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_SatinAlimBiletleri_BiletId",
                table: "SatinAlimBiletleri",
                column: "BiletId");

            migrationBuilder.CreateIndex(
                name: "IX_SatinAlimlar_KullaniciId",
                table: "SatinAlimlar",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_SepetBiletleri_BiletId",
                table: "SepetBiletleri",
                column: "BiletId");

            migrationBuilder.CreateIndex(
                name: "IX_SepetBiletleri_BiletId1",
                table: "SepetBiletleri",
                column: "BiletId1");

            migrationBuilder.CreateIndex(
                name: "IX_Sepetler_KullaniciId",
                table: "Sepetler",
                column: "KullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IlgiAlaniKullanici");

            migrationBuilder.DropTable(
                name: "SatinAlimBiletleri");

            migrationBuilder.DropTable(
                name: "SepetBiletleri");

            migrationBuilder.DropTable(
                name: "SatinAlimlar");

            migrationBuilder.DropTable(
                name: "Biletler");

            migrationBuilder.DropTable(
                name: "Sepetler");

            migrationBuilder.DropTable(
                name: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "IlgiAlanlari");
        }
    }
}
