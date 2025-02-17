using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ProcessingLibrary
{
	public class MyElectrocarPower
	{
		private string _objectCategoryId;
		private string _id;
		private string _name;
		private string _admArea;
		private string _district;
		private string _address;
		private string _longitudeWGS84;
		private string _latitudeWGS84;
		private string _globalId;
		private string _geodataCenter;
		private string _geoarea;
        public MyElectrocarPower() { }
		public MyElectrocarPower(string objectCategoryId, string id, string name,
			string admArea, string district, string address, string longitude,
			string latitude, string globalId, string geodataCenter, string geoarea)
		{
			_objectCategoryId = objectCategoryId;
			_id = id;
			_name = name;
			_admArea = admArea;
			_district = district;
			_address = address;
			_longitudeWGS84 = longitude;
			_latitudeWGS84 = latitude;
			_globalId = globalId;
			_geodataCenter = geodataCenter;
			_geoarea = geoarea;
		}
		[JsonPropertyName("object_category_Id")]
		public string ObjectCategoryId
		{
			get { return _objectCategoryId; }
			set { _objectCategoryId = value; }
		}
		[JsonPropertyName("ID")]
		public string Id
		{
			get { return _id; }
			set { _id = value; }
		}
		[JsonPropertyName("Name")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		[JsonPropertyName("AdmArea")]
		public string AdmArea
		{
			get { return _admArea; }
			set { _admArea = value; }
		}
		[JsonPropertyName("District")]
		public string District
		{
			get { return _district; }
			set { _district = value; }
		}
		[JsonPropertyName("Address")]
		public string Address
		{
			get { return _address; }
			set { _address = value; }
		}
		[JsonPropertyName("Longitude_WGS84")]
		public string Longitude
		{
			get { return _longitudeWGS84; }
			set { _longitudeWGS84 = value; }
		}
		[JsonPropertyName("Latitude_WGS84")]
		public string Latitude
		{
			get { return _latitudeWGS84; }
			set { _latitudeWGS84 = value; }
		}
		[JsonPropertyName("global_id")]
		public string GlobalId
		{
			get { return _globalId; }
			set { _globalId = value; }
		}
		[JsonPropertyName("geodata_center")]
		public string GeodataCenter
		{
			get { return _geodataCenter; }
			set { _geodataCenter = value; }
		}
		[JsonPropertyName("geoarea")]
		public string Geoarea
		{
			get { return _geoarea; }
			set { _geoarea = value; }
		}
		/// <summary>
		/// Метод для записи в CSV.
		/// </summary>
		/// <returns></returns>
		public string ToStringCsv()
		{
			return $"\"{_objectCategoryId}\";\"{_id}\";\"{_name}\";\"{_admArea}\";\"{_district}\";" +
				$"\"{_address}\";\"{_longitudeWGS84}\";\"{_latitudeWGS84}\";\"{_globalId}\";" +
				$"\"{_geodataCenter}\";\"{_geoarea}\";";

        }
	}
}

