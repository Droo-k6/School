package actbs;

import java.util.ArrayList;
import java.util.Scanner;

abstract class BaseTextInterface {
	void menuLoop() {
		Scanner _scanner = new Scanner(System.in);
		displayMenu();
		while(true) {
			System.out.print("Enter choice ('help' for help): ");
			String _input = _scanner.nextLine();
			if (_input.equals("quit")) {
				break;
			}
			if (_input.equals("menu")) {
				displayMenu();
				continue;
			}
			if (_input.equals("help")) {
				displayHelp();
				continue;
			}
			// treat as number
			try {
				int _cmdChoice = Integer.parseInt(_input) - 1;
				if ((_cmdChoice < 0)||(_cmdChoice >= commandList.size())) {
					displayInvalidChoice();
				}else{
					CommandBase _command = commandList.get(_cmdChoice);
					
					// check if takes arguments
					String _argsMsg = _command.arguments();
					if (_argsMsg.length() <= 0) {
						// execute command
						try {
							_command.execute(new ArrayList<String>());
						} catch (InvalidNumberOfArguments _e) {
							System.out.println(_e.getMessage());
						}
					}else{
						System.out.println("Arguments: " + _command.arguments());
						String _specific = _command.argsSpecific();
						if (_specific.length() > 0) {
							System.out.println(_specific);
						}
						System.out.print("Enter arguments ('back' to return to menu): ");
						_input = _scanner.nextLine();
						// check if keyword
						if (_input.equals("quit")) {
							break;
						}
						if (_input.equals("back")) {
							continue;
						}
						// tokenize
						ArrayList<String> _toks = tokenize(_input);
						// execute command
						try {
							_command.execute(_toks);
						} catch (InvalidNumberOfArguments _e) {
							System.out.println(_e.getMessage());
						}
					}
				}
			}catch(NumberFormatException _e) {
				displayInvalidChoice();
			}
		}
		_scanner.close();
	}
	void displayMenu() {
		System.out.println(title + " Options: ");
		int _i = 1;
		for(CommandBase _command : commandList) {
			System.out.printf("\t%1$d) %2$s\n", _i++, _command.getTitle());
		}
	}
	void displayHelp() {
		System.out.println("Help:");
		System.out.println("\tenter option # to access command");
		System.out.println("\tenter 'back' to back out of a command");
		System.out.println("\tenter 'quit' to quit the menu");
		System.out.println("\tenter 'help' for help");
		System.out.println("\tenter 'menu' for menu");
	}
	void displayInvalidChoice() {
		System.out.println("Invalid choice (enter 'help' to display help)");
	}
	// tokenize string to arraylist
	ArrayList<String> tokenize(String _str) {
		ArrayList<String> _toks = new ArrayList<String>();
		// tokenize by whitespace
		for(String _tok : _str.split("\\s+")) {
			_toks.add(_tok);
		}
		return _toks;
	}
	
	// members
	// Title of menu
	private String title;
	// array of commands
	private ArrayList<CommandBase> commandList = new ArrayList<CommandBase>();
	
	// constructor
	protected BaseTextInterface(String _title) {
		// set title
		title = _title;
		// call factory
		createCommands();
	}
	
	// command factory
	abstract protected void createCommands();
	
	// add command
	protected void addCommand(CommandBase _command) {
		commandList.add(_command);
	}
	
	// inner classes
	// base of commands
	protected static abstract class CommandBase {
		// Display name
		private String title;
		// Manager to call
		protected SystemManager manager;
		// constructor
		CommandBase(SystemManager _manager, String _title) {
			title = _title;
			manager = _manager;
		}
		// get title
		public String getTitle() {
			return title;
		}
		// check arguments meet minimum
		void checkArgs(ArrayList<String> _args, int _minSize) throws InvalidNumberOfArguments {
			if (_args.size() <  _minSize) {
				throw new InvalidNumberOfArguments(_minSize,_args.size());
			}
		}
		// display arguments required for command
		abstract public String arguments();
		// for specifics of arguments (such as class options)
		public String argsSpecific() {
			return "";
		}
		// execute desired command, takes input tokens
		abstract public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments;
		// verify stirng converts to int
		protected int verifyInt(String _str) {
			int _ret = -1;
			try {
				_ret = Integer.parseInt(_str);
			}catch(NumberFormatException _e){
				System.out.println("Invalid number");
				return -1;
			}
			return _ret;
		}
		
		
	}
	// display system details
	protected static class CommandDisplayDetails extends CommandBase {
		CommandDisplayDetails(SystemManager _manager) {
			super(_manager, String.format("Display %1$s system details", _manager.getSystemTypeName()));
		}
		public String arguments() {
			return "";
		}
		public void execute(ArrayList<String> _toks) {
			manager.displaySystemDetails();
		}
	}
	// process file input
	protected static class CommandParseFile extends CommandBase {
		CommandParseFile(SystemManager _manager) {
			super(_manager, String.format("Parse file into %1$s system", _manager.getSystemTypeName()));
		}
		public String arguments() {
			return "filename";
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 1);
			
			String _filename = _toks.get(0);
			manager.parseFile(_filename);
		}
	}
	// dump to file
	protected static class CommandToFile extends CommandBase {
		CommandToFile(SystemManager _manager) {
			super(_manager, String.format("Dump %1$s system to file", _manager.getSystemTypeName()));
		}
		public String arguments() {
			return "filename";
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			if (_toks.size() <= 0) {
				throw new InvalidNumberOfArguments(1,_toks.size());
			}
			
			String _filename = _toks.get(0);
			manager.dumpToFile(_filename);
		}
	}
	// book specific spot
	protected static class CommandBookSpecific extends CommandBase {
		CommandBookSpecific(SystemManager _manager) {
			super(_manager, String.format("Book specific %1$s", _manager.getSpotTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [%2$s name] [%3$s class name] [row #] [column character]", manager.getProviderTypeName(), manager.getTripTypeName(), manager.getSpotClassTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names options
			_str += String.format("%1$s class options:\n%2$s\n", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"));
			
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 5);
			
			String _providerName = _toks.get(0);
			String _tripName = _toks.get(1);
			String _className = _toks.get(2);
			String _rowStr = _toks.get(3);
			String _colStr = _toks.get(4);
			
			int _row = verifyInt(_rowStr);
			if (_row < 0){return;}
			
			char _col = _colStr.charAt(0);
			
			manager.bookSpot(_providerName,_tripName,_className,_row,_col);
		}
	}
	// book preference spot
	protected static class CommandBookPreference extends CommandBase {
		CommandBookPreference(SystemManager _manager) {
			super(_manager, String.format("Book preference %1$s", _manager.getSpotTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [%2$s name] [%3$s class name] [preference]", manager.getProviderTypeName(), manager.getTripTypeName(), manager.getSpotClassTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names, preferences
			_str += String.format("%1$s class options:\n%2$s\n%3$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"), manager.getPreferenceOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 4);
			
			String _providerName = _toks.get(0);
			String _tripName = _toks.get(1);
			String _className = _toks.get(2);
			String _pref = _toks.get(3);
			
			manager.bookSpot(_providerName,_tripName,_className,_pref);
		}
	}
	// book available spot
	protected static class CommandBookAvailable extends CommandBase {
		CommandBookAvailable(SystemManager _manager) {
			super(_manager, String.format("Book any available %1$s", _manager.getSpotTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [%2$s name] [%3$s class name]", manager.getProviderTypeName(), manager.getTripTypeName(), manager.getSpotClassTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names
			_str += String.format("%1$s class options:\n%2$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 3);
			
			String _providerName = _toks.get(0);
			String _tripName = _toks.get(1);
			String _className = _toks.get(2);
			
			manager.bookSpot(_providerName,_tripName,_className);
		}
	}
	// change trip section price
	protected static class CommandChangeSectionPrice extends CommandBase {
		CommandChangeSectionPrice(SystemManager _manager) {
			super(_manager, String.format("Change %1$s section price", _manager.getSpotTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [%2$s name] [%3$s class name] [new price]", manager.getProviderTypeName(), manager.getTripTypeName(), manager.getSpotClassTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names
			_str += String.format("%1$s class options:\n%2$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 4);
			
			String _providerName = _toks.get(0);
			String _tripName = _toks.get(1);
			String _className = _toks.get(2);
			String _priceStr = _toks.get(3);
			
			int _price = verifyInt(_priceStr);
			if (_price < 0) {return;}
			
			manager.changePrice(_providerName,_tripName,_className,_price);
		}
	}
	// change trips with matching path section price
	protected static class CommandChangePathSectionPrice extends CommandBase {
		CommandChangePathSectionPrice(SystemManager _manager) {
			super(_manager, String.format("Change path %1$s section price", _manager.getSpotTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [origin %2$s name] [destination %2$s name] [%3$s class name] [new price]", manager.getProviderTypeName(), manager.getLocationTypeName(), manager.getSpotClassTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names
			_str += String.format("%1$s class options:\n%2$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 5);
			
			String _providerName = _toks.get(0);
			String _className = _toks.get(1);
			String _originName = _toks.get(2);
			String _destName = _toks.get(3);
			String _priceStr = _toks.get(4);
			
			int _price = verifyInt(_priceStr);
			if (_price < 0) {return;}
			
			manager.changePrice(_providerName,_originName,_destName,_className,_price);
		}
	}
	// create location
	protected static class CommandCreateLocation extends CommandBase {
		CommandCreateLocation(SystemManager _manager) {
			super(_manager, String.format("create %1$s", _manager.getLocationTypeName()));
		}
		public String arguments() {
			return String.format("%1$s name", manager.getLocationTypeName());
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 1);
			
			String _locationName = _toks.get(0);
			
			manager.createLocation(_locationName);
		}
	}
	// create provider
	protected static class CommandCreateProvider extends CommandBase {
		CommandCreateProvider(SystemManager _manager) {
			super(_manager, String.format("create %1$s", _manager.getProviderTypeName()));
		}
		public String arguments() {
			return String.format("%1$s name", manager.getProviderTypeName());
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 1);
			
			String _providerName = _toks.get(0);
			
			manager.createProvider(_providerName);
		}
	}
	// create transport
	protected static class CommandCreateTransport extends CommandBase {
		CommandCreateTransport(SystemManager _manager) {
			super(_manager, String.format("create %1$s", _manager.getTransportTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [%2$s name]", manager.getProviderTypeName(), manager.getTransportTypeName());
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 2);
			
			String _providerName = _toks.get(0);
			String _transportName = _toks.get(1);
			
			manager.createTransport(_providerName,_transportName);
		}
	}
	// create trip (2 locations) (no end date)
	protected static class CommandCreateTripNoEnd extends CommandBase {
		CommandCreateTripNoEnd(SystemManager _manager) {
			super(_manager, String.format("Create %1$s no end date", _manager.getTripTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [origin %2$s] [destination %2$s] [start date year] [start date month] [start date day] [start date hour] [start date minute] [%3$s ID]", manager.getProviderTypeName(), manager.getLocationTypeName(), manager.getTripTypeName());
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 5);
			
			String _providerName = _toks.get(0);
			String _origName = _toks.get(1);
			String _destName = _toks.get(2);
			String _yearStr = _toks.get(3);
			String _monthStr = _toks.get(4);
			String _dayStr = _toks.get(5);
			String _hourStr = _toks.get(6);
			String _minStr = _toks.get(7);
			String _tripID = _toks.get(8);
			
			int _year = verifyInt(_yearStr);
			if (_year < 0) {return;}
			
			int _month = verifyInt(_monthStr);
			if (_month < 0) {return;}
			
			int _day = verifyInt(_dayStr);
			if (_day < 0) {return;}
			
			int _hour =verifyInt(_hourStr);
			if (_hour < 0) {return;}
			
			int _minute = verifyInt(_minStr);
			if (_minute < 0) {return;}
			
			manager.createTrip(_providerName,_origName,_destName,_year,_month,_day,_hour,_minute,_tripID);
		}
	}
	// create trip
	protected static class CommandCreateTrip extends CommandBase {
		CommandCreateTrip(SystemManager _manager) {
			super(_manager, String.format("Create %1$s", _manager.getTripTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [start date year] [start date month] [start date day] [start date hour] [start date minute] [end date year] [end date month] [end date day] [end date hour] [end date minute] [%3$s ID] [list of %2$ss (seperate by spaces)]", manager.getProviderTypeName(), manager.getLocationTypeName(), manager.getTripTypeName());
			}
		public String argsSpecific() {
			String _str = "";
			// spot class names
			_str += String.format("%1$s class options:\n%2$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 14);
			
			String _providerName = _toks.get(0);
			String _startDateYearStr = _toks.get(1);
			String _startDateMonthStr = _toks.get(2);
			String _startDateDayStr = _toks.get(3);
			String _startDateHourStr = _toks.get(4);
			String _startDateMinStr = _toks.get(5);
			String _endDateYearStr = _toks.get(6);
			String _endDateMonthStr = _toks.get(7);
			String _endDateDayStr = _toks.get(8);
			String _endDateHourStr = _toks.get(9);
			String _endDateMinStr = _toks.get(10);
			String _tripID = _toks.get(11);
			
			// read locations
			ArrayList<String> _locs = new ArrayList<String>();
			for (int _i = 12; _i < (_toks.size()); ++_i) {
				_locs.add(_toks.get(_i));
			}
			// transfer arraylist to array
			String[] _locations = new String[_locs.size()];
			int _i = 0;
			for (String _s : _locs) {
				_locations[_i++] = _s;
			}
			
			int _startDateYear = verifyInt(_startDateYearStr);
			if (_startDateYear < 0) {return;}
			
			int _startDateMonth = verifyInt(_startDateMonthStr);
			if (_startDateMonth < 0) {return;}
			
			int _startDateDay = verifyInt(_startDateDayStr);
			if (_startDateDay < 0) {return;}
			
			int _startDateHour =verifyInt(_startDateHourStr);
			if (_startDateHour < 0) {return;}
			
			int _startDateMin = verifyInt(_startDateMinStr);
			if (_startDateMin < 0) {return;}
			
			
			int _endDateYear = verifyInt(_endDateYearStr);
			if (_endDateYear < 0) {return;}
			
			int _endDateMonth = verifyInt(_endDateMonthStr);
			if (_endDateMonth < 0) {return;}
			
			int _endDateDay = verifyInt(_endDateDayStr);
			if (_endDateDay < 0) {return;}
			
			int _endDateHour =verifyInt(_endDateHourStr);
			if (_endDateHour < 0) {return;}
			
			int _endDateMin = verifyInt(_endDateMinStr);
			if (_endDateMin < 0) {return;}
			
			
			manager.createTrip(_providerName,_locations,_startDateYear,_startDateMonth,_startDateDay,_startDateHour,_startDateMin,_endDateYear,_endDateMonth,_endDateDay,_endDateHour,_endDateMin,_tripID);
		}
	}
	// creat trip section with layout
	protected static class CommandCreateTripSectionLayout extends CommandBase {
		CommandCreateTripSectionLayout(SystemManager _manager) {
			super(_manager, String.format("Create %1$s section with layout", _manager.getSpotTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [%2$s name] [%3$s class name] [price] [layout class] [rows]", manager.getProviderTypeName(),manager.getTripTypeName(),manager.getSpotClassTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names, layout class names
			_str += String.format("%1$s class options:\n%2$sLayout options:\n%3$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"), manager.getLayoutOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 6);
			
			String _providerName = _toks.get(0);
			String _tripID = _toks.get(1);
			String _className = _toks.get(2);
			String _priceStr = _toks.get(3);
			String _layout = _toks.get(4);
			String _rowStr = _toks.get(5);
			
			int _price = verifyInt(_priceStr);
			if (_price < 0) {return;}
			
			int _rows = verifyInt(_rowStr);
			if (_rows < 0) {return;}
			
			manager.createSpotSection(_providerName,_tripID,_className,_price,_layout,_rows);
		}
	}
	// create trip section
	protected static class CommandCreateTripSection extends CommandBase {
		CommandCreateTripSection(SystemManager _manager) {
			super(_manager, String.format("Create %1$s section", _manager.getSpotTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s name] [%2$s ID] [%3$s class name] [%3$s price] [%3$s count]", manager.getProviderTypeName(), manager.getTripTypeName(), manager.getSpotClassTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names
			_str += String.format("%1$s class options:\n%2$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 5);
			
			String _providerName = _toks.get(0);
			String _tripID = _toks.get(1);
			String _className = _toks.get(2);
			String _priceStr = _toks.get(3);
			String _countStr = _toks.get(4);
			
			int _price = verifyInt(_priceStr);
			if (_price < 0) {return;}
			
			int _count = verifyInt(_countStr);
			if (_count < 0) {return;}
			
			manager.createSpotSection(_providerName,_tripID,_className,_price,_count);
		}
	}
	// search for trips
	protected static class CommandSearchTrips extends CommandBase {
		CommandSearchTrips(SystemManager _manager) {
			super(_manager, String.format("Search for %1$s", _manager.getTripTypeName()));
		}
		public String arguments() {
			return String.format("[%1$s class name] [origin %2$s name] [destination %2$s name] [start date year] [start date month] [start date day]", manager.getSpotClassTypeName(), manager.getLocationTypeName());
		}
		public String argsSpecific() {
			String _str = "";
			// spot class names
			_str += String.format("%1$s class options:\n%2$s", manager.getSpotClassTypeName(), manager.getSpotClassOptions("\t"));
			return _str;
		}
		public void execute(ArrayList<String> _toks) throws InvalidNumberOfArguments {
			checkArgs(_toks, 6);
			
			String _className = _toks.get(0);
			String _origin = _toks.get(1);
			String _destination = _toks.get(2);
			
			String _yearStr = _toks.get(3);
			String _monthStr = _toks.get(4);
			String _dayStr = _toks.get(5);
			
			int _year = verifyInt(_yearStr);
			if (_year < 0) {return;}
			
			int _month = verifyInt(_monthStr);
			if (_month < 0) {return;}
			
			int _day = verifyInt(_dayStr);
			if (_day < 0) {return;}
			
			manager.findAvailableTrips(_origin,_destination,_className,_year,_month,_day);
		}
	}		
}
