using IFCE.AutoGate.IoT.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace IFCE.AutoGate.IoT.Infrastructure
{
    class TransitService
    {
        private readonly HttpClient _httpClient;

        public TransitService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Insert(Transit transit)
        { 
            var content = new StringContent(transit.ToJson(), Encoding.UTF8, "application/json");
            var response = _httpClient.Post("/api/transits", content);
            Debug.WriteLine($"Got response from {_httpClient.BaseAddress.OriginalString}/api/transits, status code: {response.StatusCode}");
            response.EnsureSuccessStatusCode();
        }
    } 
} 
