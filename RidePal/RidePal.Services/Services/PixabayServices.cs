using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RidePal.Services.Services
{
    public class PixabayServices : IPixabayServices
    {
        private readonly HttpClient client;

        public PixabayServices(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetImageURL()
        {
            var searchUrl = "https://pixabay.com/api/?key=31208625-54e60a24a8bb2ce33717762bd&q=music&image_type=photo";

            var response = await client.GetAsync(searchUrl);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            var responseObject = await JsonSerializer.DeserializeAsync<GetImageModel>(responseStream);

            Random rnd = new Random();

            int num = rnd.Next(1, responseObject.hits.Length);

            var res = responseObject.hits[num].largeImageURL;

            return res;
        }
    }
}