using RidePal.Data.DataInitialize.Interfaces;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RidePal.Data.DataInitialize
{
    public class FetchSongs : IFetchSongs
    {
        private readonly HttpClient client;

        public FetchSongs(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Track>> GetTracks(string genre)
        {
            var fetchUri = $"track?q=top%20{genre}";

            var response = await client.GetAsync(fetchUri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            var responseDesirialized = await JsonSerializer.DeserializeAsync<TrackFetch>(responseStream);

            Console.WriteLine(responseDesirialized.data.Length);
            return new List<Track>();
        }
    }
}
