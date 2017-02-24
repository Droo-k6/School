package actbs;

public class SimpleTextInterface extends BaseTextInterface {
	// public call to use
	public static void menu() {
		// create object
		SimpleTextInterface _menu = new SimpleTextInterface();
		// call loop
		_menu.menuLoop();
	}
	
	// constructor
	private SimpleTextInterface() {
		super("Simple Interface");
	}
	
	// create command list
	protected void createCommands() {
		// airline system manager
		AirlineSystemManager _airManager = new AirlineSystemManager();
		// cruise system manager
		CruiseSystemManager _seaManager = new CruiseSystemManager();
		
		// search for flights
		addCommand(new CommandSearchTrips(_airManager));
		// search for cruises
		addCommand(new CommandSearchTrips(_seaManager));
		
		// book specific airline seat
		addCommand(new CommandBookSpecific(_airManager));
		// book airline seat with preference
		addCommand(new CommandBookPreference(_airManager));
		// book any available seat
		addCommand(new CommandBookAvailable(_airManager));
		// book any available cabin
		addCommand(new CommandBookAvailable(_seaManager));
			
		// change price of flight section
		addCommand(new CommandChangeSectionPrice(_airManager));
		// change price of flight section given origin/location
		addCommand(new CommandChangePathSectionPrice(_airManager));
		// change price of cabin section
		addCommand(new CommandChangeSectionPrice(_seaManager));
			
		// display airline manager details
		addCommand(new CommandDisplayDetails(_airManager));
		// dump airline manager to file
		addCommand(new CommandToFile(_airManager));
		// parse airline input file
		addCommand(new CommandParseFile(_airManager));
		
		// display cruise manager details
		addCommand(new CommandDisplayDetails(_seaManager));
		// dump cruise manager to file
		addCommand(new CommandToFile(_seaManager));
		// parse cruise input file
		addCommand(new CommandParseFile(_seaManager));
	}
}
