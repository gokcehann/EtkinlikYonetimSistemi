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
                Id = 1,
                Ad = "MÃ¼zik",
                Aciklama = "MÃ¼zik etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 200.00m,
                UstKategoriId = 1
            };

            var spor = new IlgiAlani
            {
                Id = 2,
                Ad = "Spor",
                Aciklama = "Spor etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 180.00m,
                UstKategoriId = 2
            };

            var teknoloji = new IlgiAlani
            {
                Id = 3,
                Ad = "Teknoloji",
                Aciklama = "Teknoloji etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 150.00m,
                UstKategoriId = 3
            };

            var gezi = new IlgiAlani
            {
                Id = 4,
                Ad = "Gezi",
                Aciklama = "Gezi etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 250.00m,
                UstKategoriId = 4
            };

            var tiyatro = new IlgiAlani
            {
                Id = 5,
                Ad = "Tiyatro",
                Aciklama = "Tiyatro etkinlikleri",
                AnaKategoriMi = true,
                BasePrice = 300.00m,
                UstKategoriId = 5
            };

            // MÃ¼zik Alt Kategorileri
            var rock = new IlgiAlani
            {
                Id = 11,
                Ad = "Rock",
                Aciklama = "Rock mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var pop = new IlgiAlani
            {
                Id = 12,
                Ad = "Pop",
                Aciklama = "Pop mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var klasik = new IlgiAlani
            {
                Id = 13,
                Ad = "Klasik",
                Aciklama = "Klasik mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var halkMuzigi = new IlgiAlani
            {
                Id = 14,
                Ad = "Halk MÃ¼zigi",
                Aciklama = "Halk mÃ¼ziÄŸi etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var metal = new IlgiAlani
            {
                Id = 15,
                Ad = "Metal",
                Aciklama = "Metal mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var rap = new IlgiAlani
            {
                Id = 16,
                Ad = "Rap",
                Aciklama = "Rap mÃ¼zik etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            var dj = new IlgiAlani
            {
                Id = 17,
                Ad = "DJ",
                Aciklama = "DJ etkinlikleri",
                AnaKategoriMi = false,
                UstKategoriId = 1
            };

            // Spor Alt Kategorileri
            var futbol = new IlgiAlani
            {
                Id = 21,
                Ad = "Futbol",
                Aciklama = "Futbol maÃ§larÄ±  ",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var basketbol = new IlgiAlani
            {
                Id = 22,
                Ad = "Basketbol",
                Aciklama = "Basketbol maÃ§larÄ±",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var voleybol = new IlgiAlani
            {
                Id = 23,
                Ad = "Voleybol",
                Aciklama = "Voleybol maÃ§larÄ±",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var formula1 = new IlgiAlani
            {
                Id = 24,
                Ad = "Formula 1",
                Aciklama = "Formula 1 yarÄ±ÅŸlarÄ±",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };

            var dogaYuruyusu = new IlgiAlani
            {
                Id = 25,
                Ad = "DoÄŸa YÃ¼rÃ¼yÃ¼ÅŸÃ¼",
                Aciklama = "DoÄŸa yÃ¼rÃ¼yÃ¼ÅŸleri",
                AnaKategoriMi = false,
                UstKategoriId = 2
            };


            // TÃ¼m kategorileri listeye ekle
            ilgiAlanlari.AddRange(new[] { muzik, spor, teknoloji, tiyatro, gezi, rock, pop, klasik, halkMuzigi, metal, rap, dj, futbol, basketbol, voleybol, formula1, dogaYuruyusu });

            return ilgiAlanlari;
        }
    }
}
