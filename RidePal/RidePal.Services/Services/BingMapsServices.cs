using RidePal.Data.Models;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using RidePal.Services.Helpers;
using System.Text.Json;
using static RidePal.Services.Models.GetLocationModel;
using static RidePal.Services.Models.GetDistanceModel;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RidePal.Data.DataInitialize.Interfaces;

namespace RidePal.Services.Services
{
    public class BingMapsServices : IBingMapsServices
    {
        
        private readonly HttpClient _client;

        private readonly IFetchSongs fetchSongs;

        public BingMapsServices(HttpClient client, IFetchSongs fetchSongs)
        {
            _client = client;
            this.fetchSongs = fetchSongs;
        }

        public async Task<LocationPoint> GetLocation(string countryRegion, string adminDistinct, string addressLine)
        {

            adminDistinct = adminDistinct.Trim();
            adminDistinct = Regex.Replace(adminDistinct, @"\s+", " ");
            countryRegion = countryRegion.Trim();
            countryRegion = Regex.Replace(countryRegion, @"\s+", " ");
            string locationUrl;
            if (addressLine != null)
            {
                addressLine = addressLine.Trim();
                addressLine = Regex.Replace(addressLine, @"\s+", " ");
                locationUrl = string.Format(Constants.LocationUrlWithoutAddress, adminDistinct, addressLine);
            }
            else
            {
                locationUrl = string.Format(Constants.LocationUrlWithAddress, adminDistinct, addressLine);
            }

            locationUrl = HttpUtility.UrlPathEncode(locationUrl);

            var response = await _client.GetAsync(locationUrl);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            var responseObject = await JsonSerializer.DeserializeAsync<GetLocation>(responseStream);

            var res = responseObject.resourceSets[0].resources[0].point.coordinates;
            

            return new LocationPoint
            {
                longtitude = res[0],
                latitude = res[1]
            };



        }

        public async Task<Trip> GetTrip(TripQuerryParameters parameters)
        {
            var departPoint = await GetLocation(parameters.DepartCountry, parameters.DepartCity, parameters.DepartAddress);

            var arrivePoint = await GetLocation(parameters.ArriveCountry, parameters.ArriveCity, parameters.ArriveAddress);

            string distanceUrl;

            distanceUrl = string.Format(Constants.MatrixUr, departPoint.longtitude, departPoint.latitude, arrivePoint.longtitude, arrivePoint.latitude);

            var response = await _client.GetAsync(distanceUrl);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            var responseObject = await JsonSerializer.DeserializeAsync<GetDistance>(responseStream);

            var res = responseObject.resourceSets[0].resources[0].results[0];

            return new Trip
            {
                StartPoint = parameters.DepartCity,
                Destination = parameters.ArriveCity,
                Duration = res.travelDuration,
                Distance = res.travelDistance
            };
        }
    }
}
