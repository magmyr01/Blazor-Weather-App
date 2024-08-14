namespace WeatherDashboard.Models
{
	public class SwedishCitiesModel
	{
		public string Locality { get; set; } = "";
		public string Municipality { get; set; } = "";
		public string County { get; set; } = "";
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}
