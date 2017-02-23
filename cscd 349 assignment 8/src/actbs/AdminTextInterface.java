package actbs;

import actbs.BaseTextInterface.CommandBookSpecific;
import actbs.BaseTextInterface.CommandDisplayDetails;

public class AdminTextInterface extends BaseTextInterface {
	// public call to use
	public static void menu() {
		// create object
		AdminTextInterface _menu = new AdminTextInterface();
		// call loop
		_menu.menuLoop();
	}
	
	// constructor
	private AdminTextInterface() {
		super("Administrator Interface");
	}
	
	// create command list
	protected void createCommands() {
		// airline system manager
		AirlineSystemManager _airManager = new AirlineSystemManager();
		// cruise system manager
		CruiseSystemManager _seaManager = new CruiseSystemManager();
		
		// create airport
		addCommand(new CommandCreateLocation(_airManager));
		// create airline
		addCommand(new CommandCreateProvider(_airManager));
		// create flight
		addCommand(new CommandCreateTripNoEnd(_airManager));
		// create flight section
		addCommand(new CommandCreateTripSectionLayout(_airManager));
		// display airline system information
		addCommand(new CommandDisplayDetails(_airManager));
		
		// create seaport
		addCommand(new CommandCreateLocation(_seaManager));
		// create cruiseline
		addCommand(new CommandCreateProvider(_seaManager));
		// create ship
		addCommand(new CommandCreateTransport(_seaManager));
		// create trip
		addCommand(new CommandCreateTrip(_seaManager));
		// create trip section
		addCommand(new CommandCreateTripSection(_seaManager));
		// display cruise system information
		addCommand(new CommandDisplayDetails(_seaManager));
	}
}
