using AirlineClient.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace AirlineClient.Services
{
    public class AuthService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<string> Register(User user)
        {
            var response = await _client.PostAsJsonAsync($"{SessionState.ApiBaseUrl}/register", user);
            if (response.IsSuccessStatusCode)
            {
                return $"Пользователь {user.Email} успешно создан";
            }
            string error = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(error) ? $"Ошибка регистрации пользователя {user.Email}" : error;
        }

        public async Task<AuthResponse?> SignIn(User user)
        {
            var response = await _client.PostAsJsonAsync($"{SessionState.ApiBaseUrl}/login", user);
            if (!response.IsSuccessStatusCode) return null;

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AuthResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
