using System.Globalization;
using WeatherDashboard.Models;

namespace WeatherDashboard.Services
{
	public class CitiesService
	{
		public HttpClient HttpClient { get; set; }


		Dictionary<string, SwedishCitiesModel> swedishCities;

		public CitiesService(HttpClient httpClient)
		{
			HttpClient = httpClient;
			swedishCities = new Dictionary<string, SwedishCitiesModel>();
			FetchCityList();
		}

		private void FetchCityList()
		{
			using (var reader = new StreamReader("./Data/svenska-stader.csv"))
			{
				using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
				{
					swedishCities = csv.GetRecords<SwedishCitiesModel>().DistinctBy(x => x.Locality).ToDictionary(x => x.Locality, x => x);
				}
			}
		}

		public List<SwedishCitiesModel> FilterCities(string searchQuery)
		{
			return swedishCities.Where(c => c.Key.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).Take(20).Select(p => p.Value).ToList();
		}
	}
}