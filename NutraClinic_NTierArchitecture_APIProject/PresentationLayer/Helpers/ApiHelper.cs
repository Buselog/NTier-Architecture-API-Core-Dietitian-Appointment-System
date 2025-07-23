using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace PresentationLayer.Helpers
{
    public class ApiHelper
    {
        private readonly IHttpContextAccessor _http;
        private readonly IHttpClientFactory _factory;
        private readonly string _apiBaseUrl;

        public ApiHelper(
            IHttpContextAccessor http,
            IHttpClientFactory factory,
            IConfiguration config)
        {
            _http = http;
            _factory = factory;
            _apiBaseUrl = config["ApiBaseUrl"] ?? "https://localhost:7298/";
        }

        public HttpClient CreateClientWithToken()
        {
            var client = _factory.CreateClient("NutraApi");

            if (client.BaseAddress is null)
                client.BaseAddress = new Uri(_apiBaseUrl);

            var token = _http.HttpContext?.Request.Cookies["JwtToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
