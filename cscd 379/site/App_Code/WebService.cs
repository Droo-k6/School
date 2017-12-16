using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Serialization;

// web service
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {

	// fields
	private string connectionString;
	private SqlConnection connection;

	public WebService() {
		//Uncomment the following line if using designed components 
		//InitializeComponent(); 

		// setup connection
		try {
			connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ZipDB"].ConnectionString;
			connection = new SqlConnection(connectionString);
			connection.Open();
		}catch(Exception _e) {
			Trace.TraceWarning(string.Format("Failed to open DB connection, {0}: \"{1}\"\n{2}", _e.GetType().ToString(), _e.Message, _e.StackTrace));
		}
		Trace.TraceInformation("DB setup");
	}

	// test method
	[WebMethod]
	public string HelloWorld() {
		return "Hello World";
	}

	// get zip
	[WebMethod]
	[XmlInclude(typeof(ZipLocation_lab))]
	public object GetLocation(string _zip) {
		// check connection
		if ((connection == null) || (connection.State != ConnectionState.Open)) {
			Trace.TraceWarning("DB is not open");
			return ZipLocation_lab.CreateInvalid(_zip, "Failed to connect to DB");
		}
		Trace.TraceInformation("DB is open");

		// prep query
		SqlCommand _cmd = new SqlCommand(@"select City, StateAbbreviation as State, (case when(Zip_Codes.Class is null) then '' else (select Description from Class where Class.Class = Zip_Codes.Class) end) as Class from Zip_Codes, States where(ZipCode = @ZIP) and(State_Code = StateCode)", connection);
		_cmd.Parameters.AddWithValue("@ZIP", _zip);

		// execute
		SqlDataReader _reader = _cmd.ExecuteReader();

		// read info
		if (!_reader.Read()) {
			return ZipLocation_lab.CreateInvalid(_zip, "No info");
		}

		ZipLocation_lab _loc = new ZipLocation_lab(_zip, _reader);

		// close reader
		_reader.Close();

		// build result
		return _loc;
	}
}
