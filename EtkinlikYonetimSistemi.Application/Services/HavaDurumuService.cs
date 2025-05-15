using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace EtkinlikYonetimSistemi.Application.Services
{
    public class HavaDurumuService : IHavaDurumuService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public HavaDurumuService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeatherMap:ApiKey"];
        }

        public async Task<HavaDurumuDto> GetHavaDurumuAsync(DateTime tarih, string sehir)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}?q={sehir}&appid={_apiKey}&units=metric&lang=tr");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadFromJsonAsync<OpenWeatherMapResponse>();
                if (content == null)
                    return null;

                return new HavaDurumuDto
                {
                    Durum = content.Weather[0].Description,
                    Sicaklik = content.Main.Temp,
                    Icon = content.Weather[0].Icon
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private class OpenWeatherMapResponse
        {
            public Weather[] Weather { get; set; }
            public Main Main { get; set; }
            public Wind Wind { get; set; }
        }

        private class Weather
        {
            public string Description { get; set; }
            public string Icon { get; set; }
        }

        private class Main
        {
            public double Temp { get; set; }
            public double Humidity { get; set; }
        }

        private class Wind
        {
            public double Speed { get; set; }
        }
    }
}