using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

// master page
public partial class ShopMaster : System.Web.UI.MasterPage {

	// members
	private string connectionString;
	private SqlConnection connection;

	// page load event
	protected void Page_Load(object sender, EventArgs e) {
		// setup connection
		//connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;
		//connection = new SqlConnection(connectionString);
		// master page_load fired after content page_load

		// load ads
		loadAds();
	}

	// loads advertisement banners
	private void loadAds() {
		// get list of adds
		List<Ad_S1> _ads;

		try {
			_ads = GetAds();
		} catch(Exception _e) {
			Trace.Warn("Shop","Failed to get ads",_e);
			return;
		}

		// get random horizontal and vertical ads
		try {
			Ad_S1 _adH = Ad_S1.GetAdd(_ads, Ad_S1.AddType.Horizontal);
			SetAdHorizontal(_adH);
		}catch(Exception _e) {
			Trace.Warn("Shop", "Failed to set horizontal ad", _e);
		}

		try {
			Ad_S1 _adV = Ad_S1.GetAdd(_ads, Ad_S1.AddType.Vertical);
			SetAdVertical(_adV);
		} catch (Exception _e) {
			Trace.Warn("Shop", "Failed to set vertical ad", _e);
		}

	}

	// sets horizontal ad
	private void SetAdHorizontal(Ad_S1 _ad) {
		// get image name
		string _imagePath = GetImagePath(_ad.ImageName);

		// get control for image
		Control _topBanner = FindControl("bannerTop");
		HyperLink _image = (HyperLink)FindControl("bannerTopImg");
		if (_image == null) {
			_image = new HyperLink();
			_image.ID = "bannerTopImg";

			// add to bannerTop
			_topBanner.Controls.Add(_image);
		}

		// setup image
		_image.ImageUrl = _imagePath;
		_image.NavigateUrl = string.Format("http://{0}", _ad.Link);
	}

	// sets vertical ad
	private void SetAdVertical(Ad_S1 _ad) {
		// get image name
		string _imagePath = GetImagePath(_ad.ImageName);

		// get control for image
		Control _topBanner = FindControl("bannerLeft");
		HyperLink _image = (HyperLink)FindControl("bannerLeftImg");
		if (_image == null) {
			// create image
			_image = new HyperLink();
			_image.ID = "bannerLeftImg";

			// add to bannerTop
			_topBanner.Controls.Add(_image);
		}

		// setup image
		_image.ImageUrl = _imagePath;
		_image.NavigateUrl = string.Format("http://{0}", _ad.Link);
	}

	// session methods
	// get shopping cart
	public object GetCart() {
		// get cart from session state
		ShoppingCart_S1 _cart = (ShoppingCart_S1)Session["Cart"];

		// if does not exist, create
		if (_cart == null) {
			_cart = new ShoppingCart_S1();
			Session.Add("Cart", _cart);
		}

		return _cart;
	}

	// db methods

	// open connection
	private bool OpenConnection() {
		// check if connection exists
		// create if not
		if (connection == null) {
			connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;
			connection = new SqlConnection(connectionString);
		}

		if (connection.State == ConnectionState.Open) {
			return true;
		}

		try {
			connection.Open();
			return true;
		} catch(Exception _e) {
			Trace.Warn("DB", "Error opening connection", _e);
			return false;
		}
	}

	// close connection
	public void CloseConnection() {
		try {
			connection.Close();
		} catch (Exception _e) {
			Trace.Warn("DB", "Error closing connection", _e);
		}

	}

	// get ads
	public List<Ad_S1> GetAds() {
		if (!OpenConnection()) {
			throw new InvalidOperationException("Failed to open DB");
		}

		// get ads
		SqlCommand _cmd = new SqlCommand("SELECT * FROM Ads", connection);
		SqlDataReader _reader = _cmd.ExecuteReader();

		// parse reader, build list
		List<Ad_S1> _ads = new List<Ad_S1>();

		while (_reader.Read()) {
			try {
				_ads.Add(new Ad_S1(_reader));
			} catch(Exception _e) {
				Trace.Warn("DB", "Failed to parse ad record", _e);
			}
		}

		_reader.Close();

		return _ads;
	}

	// get items
	public List<object> GetItems() {
		if (!OpenConnection()) {
			throw new InvalidOperationException("Failed to open DB");
		}

		// get items
		SqlCommand _cmd = new SqlCommand("SELECT * FROM Items", connection);
		SqlDataReader _reader = _cmd.ExecuteReader();

		// parse reader, build list

		List<object> _items = new List<object>();

		while (_reader.Read()) {
			try {
				_items.Add(new Item_S1(_reader));
			} catch (Exception _e) {
				Trace.Warn("DB", "Failed to parse item record", _e);
			}
		}

		_reader.Close();

		return _items;
	}

	// get item #
	public object GetItem(int _id) {
		if (!OpenConnection()) {
			throw new InvalidOperationException("Failed to open DB");
		}

		// get item
		SqlCommand _cmd = new SqlCommand("SELECT * FROM Items WHERE ItemID=@ID", connection);
		_cmd.Parameters.AddWithValue("@ID", _id);

		SqlDataReader _reader = _cmd.ExecuteReader();

		if (!_reader.Read()) {
			_reader.Close();
			throw new ArgumentException("Failed to find item");
		}

		Item_S1 _item = new Item_S1(_reader);
		_reader.Close();

		return _item;
	}


	// get path for given image name
	public string GetImagePath(string _imageName) {
		return string.Format("./img/{0}", _imageName);
	}

	// gets item thumbnail image
	public Image GetItemImageThumb(object _o) {
		Item_S1 _item = (Item_S1)_o;

		string _imagepath = GetImagePath(_item.ImageThumbName);

		Image _image = new Image();
		_image.ImageUrl = _imagepath;
		_image.Attributes["onerror"] = "this.src='./img/noimages.jpg';";

		return _image;
	}

	// gets item image
	public Image GetItemImage(object _o) {
		Item_S1 _item = (Item_S1)_o;

		string _imagepath = GetImagePath(_item.ImageName);

		Image _image = new Image();
		_image.ImageUrl = _imagepath;
		_image.Attributes["onerror"] = "this.src='./img/noimage.jpg';";

		return _image;
	}
}
