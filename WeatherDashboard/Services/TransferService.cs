using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public class TransferService
{
	private SwedishCitiesModel? selectedCity;
	private MyResponseType? weatherInfo;

	public SwedishCitiesModel SelectedCity 
	{
		get
		{
			if(selectedCity is null)
			{
				selectedCity = new SwedishCitiesModel
				{
					Locality = "Stockholm",
					County = "Stockholm",
					Municipality = "Stockholm",
					Latitude = 59.325117,
					Longitude = 18.071093
				};
			}
			return selectedCity;
		}
		set
		{
			if(selectedCity != value)
			{
				selectedCity = value;
				SelectedCityChanged.Invoke(this, value);
				weatherInfo = null;
			}
		}
	}

	public MyResponseType? WeatherInfo
	{
		get => weatherInfo; 
		set
		{
			weatherInfo = value;
		}
	}

	public event EventHandler<SwedishCitiesModel> SelectedCityChanged = (sender, value) => {};
}
