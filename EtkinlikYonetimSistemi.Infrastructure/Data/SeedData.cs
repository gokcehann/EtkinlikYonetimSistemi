using EtkinlikYonetimSistemi.Domain.Entities;

namespace EtkinlikYonetimSistemi.Infrastructure.Data
{
    public static class SeedData
    {
        public static List<IlgiAlani> GetIlgiAlanlari()
        {
            var ilgiAlanlari = new List<IlgiAlani>();

            // Ana Kategoriler
            var muzik = new IlgiAlani
            {
                IlgiAlaniId = 1,
                Ad = "Müzik",
                Aciklama = "Müzik etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 200.00m,
                UstKategoriId = 1
            };

            var spor = new IlgiAlani
            {
                IlgiAlaniId = 2,
                Ad = "Spor",
                Aciklama = "Spor etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 180.00m,
                UstKategoriId = 2
            };

            var teknoloji = new IlgiAlani
            {
                IlgiAlaniId = 3,
                Ad = "Teknoloji",
                Aciklama = "Teknoloji etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 150.00m,
                UstKategoriId = 3
            };

            var gezi = new IlgiAlani
            {
                IlgiAlaniId = 4,
                Ad = "Gezi",
                Aciklama = "Gezi etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 250.00m,
                UstKategoriId = 4
            };

            var tiyatro = new IlgiAlani
            {
                IlgiAlaniId = 5,
                Ad = "Tiyatro",
                Aciklama = "Tiyatro etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 300.00m,
                UstKategoriId = 5
            };

            // MÃ¼zik Alt Kategorileri
            var rock = new IlgiAlani
            {
                IlgiAlaniId = 11,
                Ad = "Rock",
                Aciklama = "Rock mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var pop = new IlgiAlani
            {
                IlgiAlaniId = 12,
                Ad = "Pop",
                Aciklama = "Pop mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var klasik = new IlgiAlani
            {
                IlgiAlaniId = 13,
                Ad = "Klasik",
                Aciklama = "Klasik mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var halkMuzigi = new IlgiAlani
            {
                IlgiAlaniId = 14,
                Ad = "Halk Mzigi",
                Aciklama = "Halk mzigi etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var metal = new IlgiAlani
            {
                IlgiAlaniId = 15,
                Ad = "Metal",
                Aciklama = "Metal mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var rap = new IlgiAlani
            {
                IlgiAlaniId = 16,
                Ad = "Rap",
                Aciklama = "Rap mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var dj = new IlgiAlani
            {
                IlgiAlaniId = 17,
                Ad = "DJ",
                Aciklama = "DJ etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            // Spor Alt Kategorileri
            var futbol = new IlgiAlani
            {
                IlgiAlaniId = 21,
                Ad = "Futbol",
                Aciklama = "Futbol maÃ§larÄ±  ",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var basketbol = new IlgiAlani
            {
                IlgiAlaniId = 22,
                Ad = "Basketbol",
                Aciklama = "Basketbol maÃ§larÄ±",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var voleybol = new IlgiAlani
            {
                IlgiAlaniId = 23,
                Ad = "Voleybol",
                Aciklama = "Voleybol maÃ§larÄ±",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var formula1 = new IlgiAlani
            {
                IlgiAlaniId = 24,
                Ad = "Formula 1",
                Aciklama = "Formula 1 yarÄ±ÅŸlarÄ±",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var dogaYuruyusu = new IlgiAlani
            {
                IlgiAlaniId = 25,
                Ad = "Doga Yuruyusu",
                Aciklama = "Doğa yürüyüşleri",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };


            ilgiAlanlari.AddRange(new[] { muzik, spor, teknoloji, tiyatro, gezi, rock, pop, klasik, halkMuzigi, metal, rap, dj, futbol, basketbol, voleybol, formula1, dogaYuruyusu });

            return ilgiAlanlari;
        }
    }
}
