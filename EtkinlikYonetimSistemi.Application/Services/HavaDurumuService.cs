using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using System.Net.Http.Json;

namespace EtkinlikYonetimSistemi.Application.Services
{
    public class HavaDurumuService : IHavaDurumuService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.open-meteo.com/v1/forecast";

        public HavaDurumuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HavaDurumuDto> GetHavaDurumuAsync(DateTime tarih, string sehir)
        {
            try
            {
                // Şehir koordinatlarını al (örnek olarak İstanbul koordinatları)
                var (latitude, longitude) = GetSehirKoordinatlari(sehir);

                var url = $"{_baseUrl}?latitude={latitude}&longitude={longitude}&current=temperature_2m,weathercode&timezone=auto";
                var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);

                if (response == null || response.Current == null)
                    return null;

                return new HavaDurumuDto
                {
                    Durum = GetHavaDurumuAciklamasi(response.Current.WeatherCode),
                    Sicaklik = response.Current.Temperature2m,
                    Icon = GetHavaDurumuIkonu(response.Current.WeatherCode)
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private (double latitude, double longitude) GetSehirKoordinatlari(string sehir)
        {
            // Örnek şehir koordinatları
            return sehir.ToLower() switch
            {
                "istanbul" => (41.0082, 28.9784),
                "ankara" => (39.9334, 32.8597),
                "izmir" => (38.4237, 27.1428),
                _ => (41.0082, 28.9784) // Varsayılan olarak İstanbul
            };
        }

        private string GetHavaDurumuAciklamasi(int weatherCode)
        {
            return weatherCode switch
            {
                0 => "Açık",
                1 or 2 or 3 => "Parçalı Bulutlu",
                45 or 48 => "Sisli",
                51 or 53 or 55 => "Çisenti",
                61 or 63 or 65 => "Yağmurlu",
                71 or 73 or 75 => "Karlı",
                77 => "Kar Taneleri",
                80 or 81 or 82 => "Sağanak Yağışlı",
                85 or 86 => "Kar Yağışlı",
                95 => "Gök Gürültülü Fırtına",
                96 or 99 => "Dolu ile Gök Gürültülü Fırtına",
                _ => "Bilinmeyen"
            };
        }

        private string GetHavaDurumuIkonu(int weatherCode)
        {
            return weatherCode switch
            {
                0 => "☀️", // Açık
                1 or 2 or 3 => "⛅", // Parçalı Bulutlu
                45 or 48 => "🌫️", // Sisli
                51 or 53 or 55 => "🌦️", // Çisenti
                61 or 63 or 65 => "🌧️", // Yağmurlu
                71 or 73 or 75 => "🌨️", // Karlı
                77 => "❄️", // Kar Taneleri
                80 or 81 or 82 => "⛈️", // Sağanak Yağışlı
                85 or 86 => "🌨️", // Kar Yağışlı
                95 => "⛈️", // Gök Gürültülü Fırtına
                96 or 99 => "⛈️", // Dolu ile Gök Gürültülü Fırtına
                _ => "❓" // Bilinmeyen
            };
        }
    }

    public class OpenMeteoResponse
    {
        public CurrentWeather Current { get; set; }
    }

    public class CurrentWeather
    {
        public double Temperature2m { get; set; }
        public int WeatherCode { get; set; }
    }
}