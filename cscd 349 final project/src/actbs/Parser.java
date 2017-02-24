package actbs;

import java.util.ArrayList;

class Parser {
	// method to call to parse
	// not really needed, could just use object instead of calling this
	static void parse(SystemManager _manager, String _contents) throws InvalidCharacterRead {
		// create instance, will parse through
		Parser _parser = new Parser(_manager, _contents);
	}
	
	// member states
	// states (initialize here for simplicity)
	// base state
	private StateBase stateBase = new StateBase(this);
	// ending state
	private StateEnd stateEnd = new StateEnd(this);
	// reading locations
	private StateLocations stateLocations = new StateLocations(this);
	// reading providers
	private StateProviders stateProviders = new StateProviders(this);
	// reading between providers (intermediary)
	private StateProvidersBetween stateProvidersBetween = new StateProvidersBetween(this);
	// reading provider transports
	private StateProviderTransport stateProviderTransport = new StateProviderTransport(this);
	// reading between trips (intermediary between whole trip sections)
	private StateTripsBetween stateTripsBetween = new StateTripsBetween(this);
	// reading trip ID
	private StateTripID stateTripID = new StateTripID(this);
	
	// reading date year
	private StateTripDateYear stateTripDateYear = new StateTripDateYear(this);
	// reading date month
	private StateTripDateMonth stateTripDateMonth = new StateTripDateMonth(this);
	// reading date day
	private StateTripDateDay stateTripDateDay = new StateTripDateDay(this);
	// reading date hour
	private StateTripDateHour stateTripDateHour = new StateTripDateHour(this);
	// reading date minute
	private StateTripDateMinute stateTripDateMinute = new StateTripDateMinute(this);
	
	// reading end date year
	private StateTripEndDateYear stateTripEndDateYear = new StateTripEndDateYear(this);
	// reading end date month
	private StateTripEndDateMonth stateTripEndDateMonth = new StateTripEndDateMonth(this);
	// reading end date day
	private StateTripEndDateDay stateTripEndDateDay = new StateTripEndDateDay(this);
	// reading end date hour
	private StateTripEndDateHour stateTripEndDateHour = new StateTripEndDateHour(this);
	// reading end date minute
	private StateTripEndDateMinute stateTripEndDateMinute = new StateTripEndDateMinute(this);
	
	// reading path location
	private StateTripLocation stateTripLocation = new StateTripLocation(this);
	// reading trip section class
	private StateTripSectionClass stateTripSectionClass = new StateTripSectionClass(this);
	// reading trip section price
	private StateTripSectionPrice stateTripSectionPrice = new StateTripSectionPrice(this);
	// reading trip section layout
	private StateTripSectionLayout stateTripSectionLayout = new StateTripSectionLayout(this);
	// reading trip section count
	private StateTripSectionCount stateTripSectionCount = new StateTripSectionCount(this);
	
	// make state abstract
	// SystemManager
	private SystemManager manager;
	
	// current info being read (not initialzied because states will handle)
	// current state (initialize)
	private State currentState = stateBase;
	// current string
	private String currentString;
	// current int (as string)
	private String currentStringint;
	// current provider name
	private String currentProviderName;
	// current trip ID
	private String currentTripID;
	// current date/end date year
	private int currentDateYear, currentEndDateYear;
	// current date/end date month
	private int currentDateMonth, currentEndDateMonth;
	// current date/end date day
	private int currentDateDay, currentEndDateDay = -1;
	// current date/end date hour
	private int currentDateHour, currentEndDateHour;
	// current date/end date minute
	private int currentDateMinute, currentEndDateMinute;
	// current arraylist of location names (converted to array at end)
	private ArrayList<String> currentTripLocations;
	// current trip spot section class character
	private String currentSpotSectionClass;
	// current trip spot section price
	private int currentSpotSectionPrice;
	// current trip spot section layout character
	private String currentSpotSectionLayout;
	// current trip spot section count
	private int currentSpotSectionCount;
	
	// constructor, begins parsing
	private Parser(SystemManager _manager, String _contents) throws InvalidCharacterRead {
		// set manager/contents
		manager = _manager;
		// parse the string
		for (char _c : (_contents.toCharArray())) {
			currentState.read(_c);
		}
	}
	// set current state
	private void setState(State _state) {
		currentState = _state;
		// tell state to reset necessary parser variables
		currentState.reset();
	}
	
	// need from manager
	// char to section class name
	// char to layout class name
	// if either = null
	// dont use that variant
	
	// inner classes for parsing
	// state pattern
	// interface for states
	private static abstract class State {
		protected Parser parser;
		private String exceptionMsg = "";
		protected State(Parser _parser) {
			parser = _parser;
		}
		protected State(Parser _parser, String _msg) {
			parser = _parser;
			exceptionMsg = _msg;
		}
		// ran when state is switched
		// reset necessary variables
		abstract void reset();
		// check given character
		// default for reading, covers all possible character
		void read(char _c) throws InvalidCharacterRead {
			try {
				if (Character.isWhitespace(_c)) {
					// ignore
				}else if (Character.isAlphabetic(_c)) {
					readLetter(_c);
				}else if (Character.isDigit(_c)) {
					readDigit(_c);
				}else if (_c == ',') {
					readComma();
				}else if (_c == '|') {
					readPipe();
				}else if (_c == ':') {
					readColon();
				}else if (_c == '[') {
					readLeftBlock();
				}else if (_c == ']') {
					readRightBlock();
				}else if (_c == '{') {
					readLeftBracket();
				}else if (_c == '}') {
					readRightBracket();
				}else{
					throw new InvalidCharacterRead(_c);
				}
			}catch(InvalidCharacterReadExpected _e) {
				throw new InvalidCharacterRead(_c, _e.getMessage());
			}
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when ':' is read
		void readColon() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when '[' is read
		void readLeftBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when ']' is read
		void readRightBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when '{' is read
		void readLeftBracket() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
		// when '}' is read
		void readRightBracket() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			throw new InvalidCharacterReadExpected(exceptionMsg);
		}
	}
	private static class StateBase extends State {
		// if read locations
		public boolean readLocations = false;
		// if read providers
		public boolean readProviders = false;
		StateBase(Parser _parser) {
			super(_parser);
		}
		void reset() {}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			if (readLocations && readProviders) {
				throw new InvalidCharacterReadExpected("end of file");
			}else{
				throw new InvalidCharacterReadExpected("[");
			}
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			if (readLocations && readProviders) {
				throw new InvalidCharacterReadExpected("end of file");
			}else{
				throw new InvalidCharacterReadExpected("[");
			}
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			if (readLocations && readProviders) {
				throw new InvalidCharacterReadExpected("end of file");
			}else{
				throw new InvalidCharacterReadExpected("[");
			}
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			if (readLocations && readProviders) {
				throw new InvalidCharacterReadExpected("end of file");
			}else{
				throw new InvalidCharacterReadExpected("[");
			}
		}
		// when '[' is read
		void readLeftBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// change state to read locations
			parser.setState(parser.stateLocations);
		}
		// when ']' is read
		void readRightBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			if (readLocations && readProviders) {
				throw new InvalidCharacterReadExpected("end of file");
			}else{
				throw new InvalidCharacterReadExpected("[");
			}
		}
		// when '{' is read
		void readLeftBracket() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			if (readProviders) {
				throw new InvalidCharacterReadExpected("end of file");
			}else{
				// swap to read providers
				parser.setState(parser.stateProviders);
			}
		}
		// when '}' is read
		void readRightBracket() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			if (readLocations && readProviders) {
				throw new InvalidCharacterReadExpected("end of file");
			}else{
				throw new InvalidCharacterReadExpected("[");
			}
		}
	}
	private static class StateLocations extends State {
		StateLocations(Parser _parser) {
			super(_parser,"location name character comma or ']'");
		}
		void reset() {
			parser.currentString = "";
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// create location
			parser.manager.createLocation(parser.currentString);
			reset();
		}
		// when ']' is read
		void readRightBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// create location
			parser.manager.createLocation(parser.currentString);
			// swap to base state
			parser.setState(parser.stateBase);
			// tell base state locations read
			parser.stateBase.readLocations = true;
		}
	}
	private static class StateProviders extends State {
		StateProviders(Parser _parser) {
			super(_parser,"[");
		}
		void reset() {
			parser.currentString = "";
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// ignore
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set current provider name
			parser.currentProviderName = parser.currentString;
			// create provider
			parser.manager.createProvider(parser.currentProviderName);
			// swap to read provider transport
			parser.setState(parser.stateProviderTransport);
		}
		// when '[' is read
		void readLeftBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set current provider name
			parser.currentProviderName = parser.currentString;
			// create provider
			parser.manager.createProvider(parser.currentProviderName);
			// swap to read trips
			parser.setState(parser.stateTripID);
		}
		// when '}' is read
		void readRightBracket() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set to end state
			parser.setState(parser.stateEnd);
		}
	}
	private static class StateProvidersBetween extends State {
		StateProvidersBetween(Parser _parser) {
			super(_parser,"expected comma or '}' for providers field");
		}
		void reset() {}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// swap to reading provider name
			parser.setState(parser.stateProviders);
		}
		// when '}' is read
		void readRightBracket() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// swap to end of file
			parser.setState(parser.stateEnd);
		}
	}
	private static class StateProviderTransport extends State {
		StateProviderTransport(Parser _parser) {
			super(_parser,"transport name character/digit or '|' or '['");
		}
		void reset() {
			parser.currentString = "";
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// create transport
			parser.manager.createTransport(parser.currentProviderName,parser.currentString);
			reset();
		}
		// when '[' is read
		void readLeftBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// create transport
			parser.manager.createTransport(parser.currentProviderName,parser.currentString);
			// swap to read trips ID
			parser.setState(parser.stateTripID);
		}
	}
	private static class StateTripsBetween extends State {
		StateTripsBetween(Parser _parser) {
			super(_parser,"expected comma or ']' for trips field");
		}
		void reset() {}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// swap to reading trip ID
			parser.setState(parser.stateTripID);
		}
		// when ']' is read
		void readRightBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// swap to reading provider name
			parser.setState(parser.stateProviders);
		}
	}
	private static class StateTripID extends State {
		StateTripID(Parser _parser) {
			super(_parser,"trip id character/digit, or '|'");
		}
		void reset() {
			parser.currentString = "";
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set current trip id
			parser.currentTripID = parser.currentString;
			// swap state to read date year
			parser.setState(parser.stateTripDateYear);
		}
		// when ']' is read
		void readRightBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// swap to providers intermediary
			parser.setState(parser.stateProvidersBetween);
		}
	}
	private static class StateTripDateYear extends State {
		StateTripDateYear(Parser _parser) {
			super(_parser,"digit for trip date year, or comma");
		}
		void reset() {
			parser.currentStringint = "";
			// reset all date values
			parser.currentDateYear = -1;
			parser.currentDateMonth = -1;
			parser.currentDateDay = -1;
			parser.currentDateHour = -1;
			parser.currentDateMinute = -1;
			parser.currentEndDateYear = -1;
			parser.currentEndDateMonth = -1;
			parser.currentEndDateDay = -1;
			parser.currentEndDateHour = -1;
			parser.currentEndDateMinute = -1;
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _year = Integer.parseInt(parser.currentStringint);
			// set current date year
			parser.currentDateYear = _year;
			// swap to read date month
			parser.setState(parser.stateTripDateMonth);
		}
	}
	private static class StateTripDateMonth extends State {
		StateTripDateMonth(Parser _parser) {
			super(_parser,"digit for trip date month, or comma");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _month = Integer.parseInt(parser.currentStringint);
			// set current date month
			parser.currentDateMonth = _month;
			// swap to read date day
			parser.setState(parser.stateTripDateDay);
		}
	}
	private static class StateTripDateDay extends State {
		StateTripDateDay(Parser _parser) {
			super(_parser,"digit for trip date day, or comma");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _day = Integer.parseInt(parser.currentStringint);
			// set current date day
			parser.currentDateDay = _day;
			// swap to read date hour
			parser.setState(parser.stateTripDateHour);
		}
	}
	private static class StateTripDateHour extends State {
		StateTripDateHour(Parser _parser) {
			super(_parser,"digit for trip date hour, or comma");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _hour = Integer.parseInt(parser.currentStringint);
			// set current date hour
			parser.currentDateHour = _hour;
			// swap to read date minute
			parser.setState(parser.stateTripDateMinute);
		}
	}
	private static class StateTripDateMinute extends State {
		StateTripDateMinute(Parser _parser) {
			super(_parser,"digit for trip date minute, or comma, or '|'");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _minute = Integer.parseInt(parser.currentStringint);
			// set current date minute
			parser.currentDateMinute = _minute;
			// swap to read end date year
			parser.setState(parser.stateTripEndDateYear);
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _minute = Integer.parseInt(parser.currentStringint);
			// set current date minute
			parser.currentDateMinute = _minute;
			// swap to read trip location
			parser.setState(parser.stateTripLocation);
			// reset arraylist of strings
			parser.currentTripLocations = new ArrayList<String>();
		}
	}
	private static class StateTripEndDateYear extends State {
		StateTripEndDateYear(Parser _parser) {
			super(_parser,"digit for trip end date year, or comma");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _year = Integer.parseInt(parser.currentStringint);
			// set current end date year
			parser.currentEndDateYear = _year;
			// swap to read end date month
			parser.setState(parser.stateTripEndDateMonth);
		}
	}
	private static class StateTripEndDateMonth extends State {
		StateTripEndDateMonth(Parser _parser) {
			super(_parser,"digit for trip end date month, or comma");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _month = Integer.parseInt(parser.currentStringint);
			// set current end date month
			parser.currentEndDateMonth = _month;
			// swap to read end date day
			parser.setState(parser.stateTripEndDateDay);
		}
	}
	private static class StateTripEndDateDay extends State {
		StateTripEndDateDay(Parser _parser) {
			super(_parser,"digit for trip end date day, or comma");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _day = Integer.parseInt(parser.currentStringint);
			// set current end date day
			parser.currentEndDateDay = _day;
			// swap to read end date hour
			parser.setState(parser.stateTripEndDateHour);
		}
	}
	private static class StateTripEndDateHour extends State {
		StateTripEndDateHour(Parser _parser) {
			super(_parser,"digit for trip end date hour, or comma");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _hour = Integer.parseInt(parser.currentStringint);
			// set current end date day
			parser.currentEndDateHour = _hour;
			// swap to read end date minute
			parser.setState(parser.stateTripEndDateMinute);
		}
	}
	private static class StateTripEndDateMinute extends State {
		StateTripEndDateMinute(Parser _parser) {
			super(_parser,"digit for trip end date minute, or '|'");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a digit is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string int
			parser.currentStringint += _c;
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// convert string to int
			int _minute = Integer.parseInt(parser.currentStringint);
			// set current end date minute
			parser.currentEndDateMinute = _minute;
			// swap to read trip location
			parser.setState(parser.stateTripLocation);
		}
	}
	private static class StateTripLocation extends State {
		StateTripLocation(Parser _parser) {
			super(_parser,"character for trip location name, or '[' or '|'");
		}
		void reset() {
			parser.currentString = "";
			parser.currentTripLocations = new ArrayList<String>();
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when a pipe is read
		void readPipe() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add current string to list
			parser.currentTripLocations.add(parser.currentString);
			// reset current string
			parser.currentString = "";
		}
		// when '[' is read
		void readLeftBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add current string to list
			parser.currentTripLocations.add(parser.currentString);
			
			// create the trip
			// convert arraylist<string> to String[]
			String[] _list = new String[parser.currentTripLocations.size()];
			int _i = 0;
			for (String _str : parser.currentTripLocations) {
				_list[_i++] = _str;
			}
			
			// check if end date available
			if (parser.currentEndDateYear != -1) {
				parser.manager.createTrip(
						parser.currentProviderName, 
						_list,
						parser.currentDateYear, 
						parser.currentDateMonth, 
						parser.currentDateDay,
						parser.currentDateHour,
						parser.currentDateMinute,
						parser.currentEndDateYear, 
						parser.currentEndDateMonth, 
						parser.currentEndDateDay,
						parser.currentEndDateHour,
						parser.currentEndDateMinute,
						parser.currentTripID);
			}else{
				parser.manager.createTrip(
						parser.currentProviderName, 
						_list, 
						parser.currentDateYear, 
						parser.currentDateMonth, 
						parser.currentDateDay,
						parser.currentDateHour,
						parser.currentDateMinute,
						parser.currentTripID);
			}
			
			// swap state to read trip section
			parser.setState(parser.stateTripSectionClass);
		}
	}
	private static class StateTripSectionClass extends State {
		StateTripSectionClass(Parser _parser) {
			super(_parser,"character for section class name or ':'");
		}
		void reset() {
			parser.currentString = "";
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when ':' is read
		void readColon() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set as spot section class
			parser.currentSpotSectionClass = parser.currentString;
			// swap to reading section price
			parser.setState(parser.stateTripSectionPrice);
		}
		// when ']' is read
		void readRightBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set state to read trips between
			parser.setState(parser.stateTripsBetween);
		}
	}
	private static class StateTripSectionPrice extends State {
		StateTripSectionPrice(Parser _parser) {
			super(_parser,"digit for section class price or ':'");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// when a letter is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentStringint += _c;
		}
		// when ':' is read
		void readColon() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set as spot section price
			parser.currentSpotSectionPrice = Integer.parseInt(parser.currentStringint);
			// swap to reading section layout
			parser.setState(parser.stateTripSectionLayout);
		}
	}
	private static class StateTripSectionLayout extends State {
		StateTripSectionLayout(Parser _parser) {
			super(_parser,"letter for section layout class or ':'");
		}
		void reset() {
			parser.currentString = "";
		}
		// when a letter is read
		void readLetter(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentString += _c;
		}
		// when ':' is read
		void readColon() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// set as spot section layout
			parser.currentSpotSectionLayout = parser.currentString;
			// swap to reading section count
			parser.setState(parser.stateTripSectionCount);
		}
	}
	private static class StateTripSectionCount extends State {
		StateTripSectionCount(Parser _parser) {
			super(_parser,"digit for section count, comma or ']'");
		}
		void reset() {
			parser.currentStringint = "";
		}
		// create section from current info
		private void createSection() {
			// set section count
			parser.currentSpotSectionCount = Integer.parseInt(parser.currentStringint);
			// create section
			// get string for section class
			// parser.currentSpotSectionClass
			String _sectionStr = parser.manager.getSpotClassFileToString(parser.currentSpotSectionClass);
			// get string for layout class
			String _layoutStr = parser.manager.getLayoutFileToString(parser.currentSpotSectionLayout);
			
			// check if system uses layouts
			if (_layoutStr != null) {
				parser.manager.createSpotSection(
						parser.currentProviderName, 
						parser.currentTripID, 
						_sectionStr,
						parser.currentSpotSectionPrice,
						_layoutStr,
						parser.currentSpotSectionCount);
			}else{
				parser.manager.createSpotSection(
						parser.currentProviderName, 
						parser.currentTripID, 
						_sectionStr,
						parser.currentSpotSectionPrice,
						parser.currentSpotSectionCount);
			}
		}
		
		// when a letter is read
		void readDigit(char _c) throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// add to current string
			parser.currentStringint += _c;
		}
		// when a comma is read
		void readComma() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// create section from current info
			createSection();
			// set state to read section class
			parser.setState(parser.stateTripSectionClass);
		}
		// when ']' is read
		void readRightBlock() throws InvalidCharacterRead, InvalidCharacterReadExpected {
			// create section from current info
			createSection();
			// set state to read trips between
			parser.setState(parser.stateTripsBetween);
		}
	}
	private static class StateEnd extends State {
		StateEnd(Parser _parser) {
			super(_parser,"end of file");
		}
		void reset() {}
	}
}
