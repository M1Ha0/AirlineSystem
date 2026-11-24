using AirlineClient.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AirlineClient.Services
{
    public class CrewService : BaseService<CrewMember>
    {
        private readonly HttpClient _http;

        public CrewService()
        {
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionState.AccessToken);
        }

        public override async Task<List<CrewMember>> GetAll()
        {
            var res = await _http.GetFromJsonAsync<List<CrewMember>>($"{SessionState.ApiBaseUrl}/api/Crew");
            return res ?? new List<CrewMember>();
        }

        public override async Task Add(CrewMember obj)
        {
            var response = await _http.PostAsJsonAsync($"{SessionState.ApiBaseUrl}/api/Crew", obj);
            response.EnsureSuccessStatusCode();
        }

        public override async Task Update(CrewMember obj)
        {
            var response = await _http.PutAsJsonAsync($"{SessionState.ApiBaseUrl}/api/Crew/{obj.Id}", obj);
            response.EnsureSuccessStatusCode();
        }

        public override async Task Delete(CrewMember obj)
        {
            var response = await _http.DeleteAsync($"{SessionState.ApiBaseUrl}/api/Crew/{obj.Id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
