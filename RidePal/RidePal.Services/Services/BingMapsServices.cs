using RidePal.Services.DTOModels;
using RidePal.Services.Helpers;
using RidePal.Services.Interfaces;
using RidePal.Services.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static RidePal.Services.Models.GetDistanceModel;
using static RidePal.Services.Models.GetLocationModel;

namespace RidePal.Services.Services
{
    public class BingMapsServices : IBingMapsServices
    {
        private readonly HttpClient _client;

        public BingMapsServices(HttpClient client)
        {
            _client = client;
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
                locationUrl = string.Format(Constants.LocationUrlWithAddress, countryRegion, adminDistinct, addressLine);
            }
            else
            {
                locationUrl = string.Format(Constants.LocationUrlWithoutAddress, countryRegion, adminDistinct);
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

        public async Task<TripDTO> GetTrip(TripQuerryParameters parameters)
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

            if (res.travelDuration == -1 || res.travelDuration == -1)
            {
                throw new InvalidOperationException(Constants.INVALID_DATA);
            }
            if (parameters.DepartAddress == null && parameters.ArriveAddress == null)
            {
                return new TripDTO
                {
                    StartPoint = $"{parameters.DepartCity}, {parameters.DepartCountry}".Trim(),
                    Destination = $"{parameters.ArriveCity}, {parameters.ArriveCountry}".Trim(),
                    Duration = Math.Round(res.travelDuration, 2),
                    Distance = Math.Round(res.travelDistance, 2)
                };
            }

            return new TripDTO
            {
                StartPoint = $"{parameters.DepartCity}, {parameters.DepartCountry}, {parameters.DepartAddress}".Trim(),
                Destination = $"{parameters.ArriveCity}, {parameters.ArriveCountry}, {parameters.ArriveAddress}".Trim(),
                Duration = Math.Round(res.travelDuration, 2),
                Distance = Math.Round(res.travelDistance, 2)
            };
        }
    }
}