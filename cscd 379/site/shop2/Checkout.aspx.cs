using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// checkout class
public partial class Checkout : System.Web.UI.Page {
	// cost per ounce
	private const double costPerOunce = 0.46;
	// order information
	private int totalItems = 0, totalQuantity = 0;
	private double totalPrice = 0, totalWeight = 0;
	private double totalShipping = 0, finalTotal = 0;

	// page load
	protected void Page_Load(object sender, EventArgs e) {
		if (IsPostBack) {
			return;
		}

		// validate previous page
		try {
			if (PreviousPage == null) {
				Response.Redirect("./Browse.aspx", true);
			}
			// IsPostBack?
		}catch(InvalidOperationException _e) {
			Response.Redirect("./Browse.aspx", true);
		}

		// check if cart empty
		ShoppingCart_S2 _cart = (ShoppingCart_S2)((ShopMaster)Master).GetCart();
		IReadOnlyCollection<ShoppingCart_S2.ShoppingCartItem_S2> _items = _cart.Items;
		if (_items.Count <= 0) {
			Response.Redirect("./Browse.aspx", true);
		}

		int _total = 0;
		foreach(ShoppingCart_S2.ShoppingCartItem_S2 _item in _items) {
			_total += _item.Quantity;
		}

		if (_total <= 0) {
			Response.Redirect("./Browse.aspx", true);
		}

		// build page
		buildDisplay();
	}

	// builds page
	private void buildDisplay() {
		// get totals
		calculateTotals();
		// populate summary table
		CellItems.Text = totalItems.ToString();
		CellQuantity.Text = totalQuantity.ToString();
		CellWeight.Text = totalWeight.ToString("f2");
		CellPrice.Text = totalPrice.ToString("C2");
		CellShipping.Text = totalShipping.ToString("C2");
		CellTotals.Text = finalTotal.ToString("C2");

	}

	// calculates totals
	private void calculateTotals() {
		// get cart summary
		ShoppingCart_S2 _cart = (ShoppingCart_S2)((ShopMaster)Master).GetCart();
		IReadOnlyCollection<ShoppingCart_S2.ShoppingCartItem_S2> _items = _cart.Items;
		int _totalItems = 0, _totalQuantity = 0;
		double _totalPrice = 0, _totalWeight = 0, _finalPrice = 0;

		foreach (ShoppingCart_S2.ShoppingCartItem_S2 _cartItem in _items) {
			if (_cartItem.Quantity <= 0) {
				continue;
			}
			++_totalItems;
			_totalQuantity += _cartItem.Quantity;

			Item_S2 _item = (Item_S2)((ShopMaster)Master).GetItem(_cartItem.ItemID);
			_totalPrice += _item.Price * _cartItem.Quantity;
			_totalWeight += _item.Weight * _cartItem.Quantity;
		}

		_finalPrice = _totalPrice + _totalWeight * costPerOunce;

		totalItems = _totalItems;
		totalQuantity = _totalQuantity;
		totalPrice = _totalPrice;
		totalWeight = _totalWeight;
		totalShipping = _totalWeight * costPerOunce;
		finalTotal = _finalPrice;
	}

	// for submit click
	protected void SubmitClick(object sender, EventArgs e) {

		// validate inputs
		if (!IsValid) {
			return;
		}

		// get order text
		string name = TextboxName.Text;
		string street = TextboxStreet.Text;
		string city = TextboxCity.Text;
		string state = TextboxState.Text;
		string zip = TextboxZIP.Text;
		string emailstr = TextboxEmail.Text;

		// get order number
		string orderStr = ((ShopMaster)Master).GetNextOrderNumber();

		// build email
		MailAddress emailSource = new MailAddress("cscd379s17shop2@gmail.com", "Keiths Autos");
		MailAddress emailDestination = new MailAddress(emailstr, name);
		MailMessage message = new MailMessage(emailSource, emailDestination);
		message.Subject = "Order Confirmation";
		message.IsBodyHtml = true;
		message.Body = buildMessageBody(name, street, city, state, zip, emailstr, orderStr);
		// send
		SmtpClient client = new SmtpClient();
		client.UseDefaultCredentials = false;
		client.EnableSsl = true;
		client.Host = "smtp.gmail.com";
		client.Port = 587;
		client.DeliveryMethod = SmtpDeliveryMethod.Network;
		client.Credentials = new NetworkCredential("cscd379s17shop2@gmail.com","feedinghorse");
		
		try {
			client.Send(message);
		}catch(Exception _e) {
			Trace.Warn("checkout", "failed to send confirmation email", _e);
			labelWarning.Visible = true;
			labelWarning.Text = "There was an issue sending the confirmation email";
			return;
		}

		// set order str
		Session["OrderStr"] = orderStr;

		// transfer to confirmation page
		Response.Redirect("./Confirmation.aspx", true);
	}

	// builds message body (HTML)
	private string buildMessageBody(string name, string street, string city, string state, string zip, string email, string orderStr) {
		// get totals
		calculateTotals();

		string body = "";

		// opening
		body += string.Format("<p>Order Confirmation #{0}</p>", orderStr);

		// order summary
		body += "<h5>Summary:</h5>";
		body += string.Format("<table>{0}{1}{2}{3}{4}{5}</table>",
			string.Format("<tr><td>Items</td><td align=\"right\">{0}</td></tr>", totalItems),
			string.Format("<tr><td>Quantity</td><td align=\"right\">{0}</td></tr>", totalQuantity),
			string.Format("<tr><td>Price</td><td align=\"right\">{0:C2}</td></tr>", totalPrice),
			string.Format("<tr><td>Weight</td><td align=\"right\">{0:f2}</td></tr>", totalWeight),
			string.Format("<tr><td>Shipping Total</td align=\"right\"><td>{0:C2}</td></tr>", totalShipping),
			string.Format("<tr><td>Final Total</td><td align=\"right\">{0:C2}</td></tr>", finalTotal)
			);

		// order information
		body += "<h5>Shipping Information</h5>";
		body += string.Format("<table>{0}{1}{2}</table>",
			string.Format("<tr><td>{0}</td></tr>", name),
			string.Format("<tr><td>{0}</td></tr>", street),
			string.Format("<tr><td>{0} {1} {2}</td></tr>", city, state, zip)
			);

		// order items
		body += "<h5>Items</h5>";
		body += "<table>";
		// build title row
		body += "<tr><td>ID</td><td>Name</td><td>Unit Price</td><td>Unit Weight</td><td>Quantity</td></tr>";
		// build rows for each item
		ShoppingCart_S2 _cart = (ShoppingCart_S2)((ShopMaster)Master).GetCart();
		IReadOnlyCollection<ShoppingCart_S2.ShoppingCartItem_S2> _items = _cart.Items;
		foreach (ShoppingCart_S2.ShoppingCartItem_S2 _cartItem in _items) {
			Item_S2 _item = (Item_S2)((ShopMaster)Master).GetItem(_cartItem.ItemID);
			body += string.Format("<tr>{0}{1}{2}{3}{4}</tr>",
				string.Format("<td>{0}</td>", _item.ID),
				string.Format("<td>\"{0}\"</td>", _item.Name),
				string.Format("<td>{0:C2}</td>", _item.Price),
				string.Format("<td>{0:f2}</td>", _item.Weight),
				string.Format("<td>{0}</td>", _cartItem.Quantity)
				);
		}
		body += "</table>";

		return body;
	}

	// validates email
	protected void ValidateEmail(object source, ServerValidateEventArgs args) {
		try {
			Trace.Write("checkout",string.Format("checking email \"{0}\"", args.Value));
			MailAddress _addr = new MailAddress(args.Value);
			args.IsValid = true;
		}catch(Exception _e) {
			Trace.Warn("checkout", "invalid email", _e);
			args.IsValid = false;
		}
	}
}
 