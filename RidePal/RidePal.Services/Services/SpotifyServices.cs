
using RidePal.Data.Models;
using RidePal.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static RidePal.Services.Models.SpotifyResultModel;

namespace RidePal.Services.Services
{
    public  class SpotifyServices:ISpotifyServices
    {
        private readonly HttpClient _client;

        public SpotifyServices(HttpClient client)
        {
            _client = client;
        }
        public async Task<IEnumerable<Track>> GetRapSongs(string token)
        {
            var list = new List<Track>();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            for (int i = 0; i < 1000; i+=50)
            {
                var response = await _client.GetAsync($"search?q=genre%3Arap&type=track&country=BG&limit=50&offset={i}");

                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();

                var responseObject = await JsonSerializer.DeserializeAsync<SpotifyResultObject>(responseStream);


                var res = responseObject?.tracks?.items.Select(t => new Track
                {
                    Title = t.name,
                    Duration = t.duration_ms,
                    Rank = t.popularity,
                    PreviewURL = t.preview_url,
                    GenreName = t.type
                });

                list.AddRange(res);
            }

            return list;
        }
    }
}
