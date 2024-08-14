using WeatherDashboard.Models;

namespace WeatherDashboard.Services;

public class TransferService
{
	private SwedishCitiesModel? selectedCity;
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
			selectedCity = value;
			SelectedCityChanged.Invoke(this, value);
		}
	}

	public event EventHandler<SwedishCitiesModel> SelectedCityChanged = (sender, value) => {};
}
