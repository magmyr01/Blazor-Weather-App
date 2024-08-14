namespace WeatherDashboard.Models
{
	public class MyResponseType
	{
		public DateTime ApprovedTime { get; set; }
		public DateTime ReferenceTime { get; set; }
		public required Geometry Geometry { get; set; }
		public required List<TimeSerie> TimeSeries { get; set; }
	}

	public class Geometry
	{
		public string Type { get; set; } = "";
		public  required List<List<double>> Coordinates { get; set; }
	}

	public class TimeSerie
	{
		public DateTime ValidTime { get; set; }
		public required List<Parameters> Parameters { get; set; }
		public Dictionary<string, Parameters> ParametersDictionary => Parameters.ToDictionary(x => x.Name, x => x);
	}

	public class Parameters
	{
		public string Name { get; set; } = "";
		public string LevelType { get; set; } = "";
		public double Level { get; set; }
		public string Unit { get; set; } = "";
		public required List<double> Values { get; set; }
	}
}
