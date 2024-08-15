using WeatherDashboard.Models;

namespace WeatherDashboard.Services
{
	public class WeatherService
	{
		private readonly HttpClient httpClient;
		private TransferService TransferService;

		public WeatherService(HttpClient httpClient, TransferService transferService)
		{
			this.httpClient = httpClient;
			TransferService = transferService;
		}

		public async Task<MyResponseType?> GetDataAsync(double longitude, double latitude)
		{
			if(TransferService.WeatherInfo is null)
			{
				try
				{
					var response = await httpClient.GetAsync($"/api/category/pmp3g/version/2/geotype/point/lon/{longitude}/lat/{latitude}/data.json");

					response.EnsureSuccessStatusCode();

					TransferService.WeatherInfo = await response.Content.ReadFromJsonAsync<MyResponseType>();
					return TransferService.WeatherInfo;
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine(e.Message);
					return null;
				}
			}
			return TransferService.WeatherInfo;
		}

		public static string WeatherCodeToWeatherSymbolConverter(int weatherCode)
		{
			switch (weatherCode)
			{
				case 1:
				case 2:
					return "/Icons/WeatherIcons/brightness-high.svg";
				case 3:
				case 4:
					return "/Icons/WeatherIcons/cloud-sun.svg";
				case 5:
				case 6:
					return "/Icons/WeatherIcons/clouds.svg";
				case 7:
					return "/Icons/WeatherIcons/cloud-fog.svg";
				case 8:
				case 18:
					return "/Icons/WeatherIcons/cloud-hail.svg";
				case 9:
				case 19:
					return "/Icons/WeatherIcons/could-rain.svg";
				case 10:
				case 20:
					return "/Icons/WeatherIcons/cloud-rain-heavy.svg";
				case 11:
					return "/Icons/WeatherIcons/could-lightning-rain.svg";
				case 12:
				case 13:
				case 22:
				case 23:
				case 25:
					return "/Icons/WeatherIcons/could-sleet.svg";
				case 14:
				case 15:
				case 16:
				case 17:
				case 24:
				case 26:
				case 27:
					return "/Icons/WeatherIcons/cloud-snow.svg";
				case 21:
					return "/Icons/WeatherIcons/cloud-lightning.svg";
				default:
					return "/Icons/WeatherIcons/brightness-high.svg";
			}
		}

		public async Task<List<DaySummary>> Get10DaySummary(double longitude, double latitude)
		{
			MyResponseType response = await GetDataAsync(longitude, latitude) ?? throw new Exception("GetDataAsync in WeatherService returned null");

			var list = new List<DaySummary>();

			DateTime pointerDate = DateTime.Now.Date;
			int iterator = 0;

			while(iterator < response.TimeSeries.Count)
			{
				double MaxTemp = double.MinValue;
				double MinTemp = double.MaxValue;
				double WindSpeed = 0;
				int Humidity = 0;

				while (iterator < response.TimeSeries.Count && response.TimeSeries[iterator].ValidTime.Date == pointerDate)
				{
					MaxTemp = MaxTemp < response.TimeSeries[iterator].ParametersDictionary["t"].Values[0] ? response.TimeSeries[iterator].ParametersDictionary["t"].Values[0] : MaxTemp;
					MinTemp = MinTemp > response.TimeSeries[iterator].ParametersDictionary["t"].Values[0] ? response.TimeSeries[iterator].ParametersDictionary["t"].Values[0] : MinTemp;
					WindSpeed = WindSpeed < response.TimeSeries[iterator].ParametersDictionary["ws"].Values[0] ? response.TimeSeries[iterator].ParametersDictionary["ws"].Values[0] : WindSpeed;
					Humidity = Humidity < (int)response.TimeSeries[iterator].ParametersDictionary["r"].Values[0] ? (int)response.TimeSeries[iterator].ParametersDictionary["r"].Values[0] : Humidity;

					iterator++;
				}
				if(iterator < response.TimeSeries.Count)
				{
					pointerDate = response.TimeSeries[iterator].ValidTime.Date;
				}

				list.Add(new DaySummary()
				{
					MaxTemp = MaxTemp,
					MinTemp = MinTemp,
					WindSpeed = WindSpeed,
					RelativeHumidity = Humidity
				});
			}

			return list;
		}
	}
}
