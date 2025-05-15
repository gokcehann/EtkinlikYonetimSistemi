using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtkinlikYonetimSistemi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IlgiAlanlari",
                keyColumn: "IlgiAlaniId",
                keyValue: 14,
                columns: new[] { "Aciklama", "Ad" },
                values: new object[] { "Halk mzigi etkinlikleri", "Halk Mzigi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "IlgiAlanlari",
                keyColumn: "IlgiAlaniId",
                keyValue: 14,
                columns: new[] { "Aciklama", "Ad" },
                values: new object[] { "Halk mÃ¼ziÄŸi etkinlikleri", "Halk MÃ¼zigi" });
        }
    }
}
