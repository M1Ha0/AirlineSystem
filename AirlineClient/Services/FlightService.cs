using AirlineClient.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AirlineClient.Services
{
    public class FlightService : BaseService<Flight>
    {
        private readonly HttpClient _http;

        public FlightService()
        {
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionState.AccessToken);
        }

        public override async Task<List<Flight>> GetAll()
        {
            var res = await _http.GetFromJsonAsync<List<Flight>>($"{SessionState.ApiBaseUrl}/api/Flight");
            return res ?? new List<Flight>();
        }

        public override async Task Add(Flight obj)
        {
            var response = await _http.PostAsJsonAsync($"{SessionState.ApiBaseUrl}/api/Flight", obj);
            response.EnsureSuccessStatusCode();
        }

        public override async Task Update(Flight obj)
        {
            var response = await _http.PutAsJsonAsync($"{SessionState.ApiBaseUrl}/api/Flight/{obj.Id}", obj);
            response.EnsureSuccessStatusCode();
        }

        public override async Task Delete(Flight obj)
        {
            var response = await _http.DeleteAsync($"{SessionState.ApiBaseUrl}/api/Flight/{obj.Id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Flight?> AssignCrewMember(int flightId, int crewMemberId, string role)
        {
            var url = $"{SessionState.ApiBaseUrl}/api/Flight/{flightId}/assign?crewMemberId={crewMemberId}&role={Uri.EscapeDataString(role)}";
            var response = await _http.PostAsync(url, null);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Flight>();
        }

        public async Task<Flight?> RemoveAssignment(int flightId, int assignmentId)
        {
            var url = $"{SessionState.ApiBaseUrl}/api/Flight/{flightId}/assign/{assignmentId}";
            var response = await _http.DeleteAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Flight>();
        }
    }
}
