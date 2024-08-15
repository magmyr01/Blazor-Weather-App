using System.Globalization;
using WeatherDashboard.Models;

namespace WeatherDashboard.Services
{
	public class CitiesService
	{
		Dictionary<string, SwedishCitiesModel> swedishCities;
		private TransferService TransferService { get; set; }

		public CitiesService(TransferService transferService)
		{
			swedishCities = new Dictionary<string, SwedishCitiesModel>();
			TransferService = transferService;
			FetchCityList();

			TransferService.SelectedCityChanged += OnSelectedCityChanged;
		}

		public List<SwedishCitiesModel> FilterCities(string searchQuery)
		{
			return swedishCities.Where(c => c.Key.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).Take(20).Select(p => p.Value).ToList();
		}

		public SwedishCitiesModel GetSelectedCity()
		{
			return TransferService.SelectedCity;
		}

		public void UpdateSelectedCity(SwedishCitiesModel newCity)
		{
			TransferService.SelectedCity = newCity;
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

		private void OnSelectedCityChanged(object? sender, SwedishCitiesModel e)
		{
			SelectedCityChanged.Invoke(this, e);
		}

		public event EventHandler<SwedishCitiesModel> SelectedCityChanged = (sender, value) => { };
	}
}