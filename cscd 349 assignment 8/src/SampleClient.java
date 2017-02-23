import java.util.ArrayList;

import actbs.*;

public class SampleClient {
	public static void main(String[] args) {
	//	testAirlineSystemManager();
	//	testCruiseSystemManager();
		testSimpleTextInterface();
	//	testAdminTextInterface();
	}
	public static void testAirlineSystemManager() {
		System.out.println("/////////////////////////////////////////////Testing airline system manager");
		
		SystemManager sys = new AirlineSystemManager();
		sys.createLocation("DEN");
		sys.createLocation("DFW");
		sys.createLocation("LON");
		sys.createLocation("ARD");
		sys.createLocation("DEN");		// invalid name exists
		sys.createLocation("DENW");		// invalid name length
		sys.createLocation("SE1");		// invalid character in name
		
		sys.createProvider("DELTA");
		sys.createProvider("AMER");
		sys.createProvider("FRONT");
		sys.createProvider("FRONTIER"); // invalid name length
		sys.createProvider("FRONT"); 	// invalid name exists
		sys.createProvider("FAK0");		// invalid character
		
		sys.createTrip("DELTA", "DEN", "LON", 2013, 10, 10, 12, 00, "123");
		sys.createTrip("DELTA", "DEN", "DEH", 2013, 8, 8, 12, 00, "567abc");		// invalid airport destination
		sys.createTrip("DEL", "DEN", "LON", 2013, 9, 8, 12, 00, "567"); 			// invalid airline
		sys.createTrip("DELTA", "LON33", "DEN33", 2013, 5, 7, 12, 00, "123");		// invalid airport origin/destination
		sys.createTrip("AMER", "DEN", "LON", 2010, 40, 100, 12, 00, "123abc");		// invalid date (month & day)
		sys.createTrip("AMER", "DEN", "LON", 2010, 0, 2, 12, 00, "123abc");			// invalid date (month)
		sys.createTrip("AMER", "DEN", "LON", 2010, 13, 13, 12, 00, "123abc");		// invalid date (month)
		sys.createTrip("FRONT", "DEN", "DFW", 2012, 12, 0, 12, 00, "123abc");		// invalid date (day)
		sys.createTrip("FRONT", "DEN", "DFW", 2012, 12, 32, 12, 00, "123abc");		// invalid date (day)
		sys.createTrip("FRONT", "DEN", "DFW", 2012, 12, 33, 12, 00, "123abc");		// invalid date (day)
		sys.createTrip("FRONT", "DEN", "DFW", 2012, 2, 30, 12, 00, "123abc");		// invalid date (day)
		sys.createTrip("FRONT", "DEN", "DFW", -100, 10, 24, 12, 00, "123abc");		// invalid date (year)
		sys.createTrip("FRONT", "DEN", "DFW", 2012, 12, 31, 12, 00, "123abc");
		sys.createTrip("DELTA", "LON", "DFW", 2012, 6, 20, 12, 00, "123abc");		// invalid identifier (exists)
		sys.createTrip("AMER", "DFW", "LON", 2011, 5, 18, 12, 00, "");				// invalid identifier (empty)
		sys.createTrip("AMER", "DFW", "DFW", 2012, 9, 28, 12, 00, "polaw4");		// invalid path (airports same)
		sys.createTrip("FRONT", "DEN", "DFW", 2012, 9, 28, 12, 00, "tukp1a");
		sys.createTrip("FRONT", "DEN", "DFW", 2012, 9, 28, 12, 00, "tukp1b");
		
		sys.createSpotSection("asd","123", "economy", 100, "small", 2);			// invalid airline
		sys.createSpotSection("DELTA","abc", "economy", 100, "small", 2);		// invalid flight
		sys.createSpotSection("DELTA","123", "economy", 100, "small", 2);		
		sys.createSpotSection("DELTA","123", "first", 100, "small", 2);		
		sys.createSpotSection("DELTA","123", "first", 100,"small", 2);			// Invalid class (exists)
		sys.createSpotSection("FRONT","123abc", "economy", 100, "medium", 0);	// Invalid rows (< 1)
		sys.createSpotSection("FRONT","123abc", "economy", 100, "medium", 101);	// Invalid rows (> 100)
		sys.createSpotSection("FRONT","123abc", "economy", -5, "medium", 100);	// invalid price (< 0)
		sys.createSpotSection("FRONT","123abc", "economy", 100, "medium", 100);
		sys.createSpotSection("FRONT","tukp1a", "economy", 50, "medium", 50);	// invalid price for airline/seatclass/path
		sys.createSpotSection("FRONT","tukp1a", "economy", 150, "wide", 50);	// invalid price for airline/seatclass/path
		sys.createSpotSection("FRONT","tukp1a", "economy", 100, "wide", 50);
		sys.createSpotSection("FRONT","tukp1a", "business", 500, "wide", 50);
		sys.createSpotSection("FRONT","tukp1a", "first", 720, "wide", 10);
		sys.createSpotSection("FRONT","tukp1b", "economy", 100, "wide", 100);
		sys.createSpotSection("FRONT","tukp1b", "value", 65, "wide", 100);
		sys.createSpotSection("FRONT","tukp1b", "free", 65, "wide", 100);		// invalid spot class name
		sys.createSpotSection("FRONT","tukp1b", "value", 65, "large", 100);		// invalid layout class name
		
		
		// book specific seats
		sys.bookSpot("DELTA", "123", "first", 1, 'A');
		sys.bookSpot("DELTA", "123", "economy", 1, 'A');
		sys.bookSpot("DELTA", "123", "economy", 1, 'B');
		sys.bookSpot("DELTA888", "123", "business", 1, 'A'); 		// Invalid airline
		sys.bookSpot("DELTA", "123haha7", "business", 1, 'A'); 		// Invalid flightId
		sys.bookSpot("DELTA", "123", "economy", 1, 'A'); 			// Invalid seat, booked
		sys.bookSpot("DELTA", "123", "economy", 2, 'D'); 			// invalid column
		sys.bookSpot("FRONT", "123abc", "economy", 0, 'A'); 		// Invalid row (< 1)
		sys.bookSpot("FRONT", "123abc", "economy", 101, 'A');		// Invalid row (> max 100)
		sys.bookSpot("FRONT", "123abc", "economy", 1, '!');			// Invalid column (invalid character)
		sys.bookSpot("FRONT", "123abc", "economy", 1, 'K');			// Invalid column (past range)
		sys.bookSpot("FRONT", "123abc", "business", 1, 'A');		// Invalid section
		sys.bookSpot("FRONT","123abc", "free", 2,'A');				// invalid seat class name
		
		System.out.println("booking 510 any");
		// just book any open seats
		for (int _i = 0; _i <= 510; ++_i) {
			sys.bookSpot("FRONT", "TUKP1A", "economy");
		}
		
		// attempt to book all seats
		System.out.println("booking all seats on FRONT 123abc");
		for(int _i = 1; _i <= 100; ++_i) {
			for (int _j = 0; _j < 4; ++_j) {
				sys.bookSpot("FRONT","123abc", "economy", _i, (char)(_j+65));
			}
		}
		
		// attempt again (on smaller range)
		// should all be booked
		// 1A,1B,2A,2B,3A,3B,4A,4B,5A,5B
		System.out.println("attempting book all again");
		for(int _i = 1; _i <= 5; ++_i) {
			for (int _j = 0; _j < 2; ++_j) {
				sys.bookSpot("FRONT","123abc", "economy", _i, (char)(_j+65));
			}
		}
		
		// book preference seats
		sys.bookSpot("123a","tukp1b", "value", "window");		// Invalid airline
		sys.bookSpot("FRONT","zsw1", "value", "window");		// Invalid flightID
		
		// 100 rows, wide layout (10 columns)
		// attempt to book all window seats (200 seats)(200/1000)
		System.out.println("booking 200 window seats");
		for (int _i = 0; _i < 200; ++_i) {
			sys.bookSpot("FRONT","tukp1b", "value", "window");
		}
		// attempt to book all aisle seats (2 gaps, 4*100, 400)(600/1000)
		System.out.println("booking 400 aisle seats");
		for (int _i = 0; _i < 400; ++_i) {
			sys.bookSpot("FRONT","tukp1b", "value", "aisle");
		}
		// attempt to book more seats, split between aisle/window (none are avail however)(800/1000)
		System.out.println("booking 200 alternating aisle/window seats");
		for (int _i = 0; _i < 200; ++_i) {
			if (_i % 2 == 0) {
				sys.bookSpot("FRONT","tukp1b", "value", "window");
			}else{
				sys.bookSpot("FRONT","tukp1b", "value", "aisle");
			}
		}
		// attempt to over book (first 200 will book, last 100 will not)
		System.out.println("booking 300 seats");
		for (int _i = 0; _i < 300; ++_i) {
			sys.bookSpot("FRONT","tukp1b", "value", "none");
		}
		
		sys.findAvailableTrips("DEN", "DEN");		// invalid (origin == destination)
		sys.findAvailableTrips("NUL", "DEN");		// invalid origin airport
		sys.findAvailableTrips("LON", "NIL");		// invalid destination airport
		sys.findAvailableTrips("ARD", "LON");		// no available flights
		sys.findAvailableTrips("DEN", "LON");
		sys.findAvailableTrips("DEN", "DFW");
		
		sys.displaySystemDetails();
		
		// dump to file
		sys.dumpToFile("testair.ams");
		
		// create manager with file
		sys = new AirlineSystemManager("test1.ams");
		
		// display details
		sys.displaySystemDetails();
		
		// load another file
		sys.parseFile("testair.ams");
		
		// display details
		sys.displaySystemDetails();
		
		System.out.println("/////////////////////////////////////////////");
	}
	public static void testCruiseSystemManager() {
		System.out.println("/////////////////////////////////////////////Testing cruise system manager");
		
		// create cruise manager
		SystemManager sys = new CruiseSystemManager();
		
		// create seaports
		sys.createLocation("HAW");
		sys.createLocation("HAI");
		sys.createLocation("JPN");
		sys.createLocation("LAS");
		sys.createLocation("LAS");		// invalid name exists
		sys.createLocation("PO");		// invalid name length
		sys.createLocation("PORT");		// invalid name length
		sys.createLocation("PO4");		// invalid character in name
		
		// create cruises
		sys.createProvider("SEA");
		sys.createProvider("PACF");
		sys.createProvider("OCEAN");
		sys.createProvider("ATLANTIC"); // invalid name length
		sys.createProvider("PACF"); 	// invalid name exists
		sys.createProvider("Real1");	// invalid character
		
		// create ships
		sys.createTransport("AES","OHSEA");		// provider does not exist
		sys.createTransport("SEA",""); 			// invalid name length
		sys.createTransport("SEA","OHSE4!");	// invalid name character
		sys.createTransport("SEA","OHSEA");
		sys.createTransport("SEA","OHSEA1");
		sys.createTransport("SEA","OHSEA2");
		sys.createTransport("SEA","OHSEA3");
		sys.createTransport("PACF","AHOY");
		
		// create trips
		String[] _locs1 = new String[]{"HAW","FAK","JPN"};			// bad list
		String[] _locs2 = new String[]{"HAW","LAS","JPN","HAW"};	// ok list
		String[] _locs3 = new String[]{"JPN","LAS","HAW"};
		String[] _locs4 = new String[]{"JPN","LAS","JPN"};
		String[] _locs5 = new String[]{"LAS","HAI","HAW"};
		
		sys.createTrip("ASEA", _locs1, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea");	// provider does not exist
		sys.createTrip("SEA", _locs1, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea");	// location "FAK" does not exist
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "");			// invalid trip name
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "to sea");	// invalid trip name
		
		sys.createTrip("SEA", _locs2, -1, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea");		// invalid start date (year)
		sys.createTrip("SEA", _locs2, 2000, 0, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea");	// invalid start date (month)
		sys.createTrip("SEA", _locs2, 2000, 13, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea");	// invalid start date (month)
		sys.createTrip("SEA", _locs2, 2000, 1, -1, 12, 00, 2000, 1, 14, 12, 00, "tosea");	// invalid start date (day)
		sys.createTrip("SEA", _locs2, 2000, 1, 32, 12, 00, 2000, 1, 14, 12, 00, "tosea");	// invalid start date (day)
		
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, -2000, 1, 14, 12, 00, "tosea");	// invalid end date (year)
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 0, 14, 12, 00, "tosea");	// invalid end date (month)
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 13, 14, 12, 00, "tosea");	// invalid end date (month)
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, -13, 12, 00, "tosea");	// invalid end date (day)
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 32, 12, 00, "tosea");	// invalid end date (day)
		
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 1999, 12, 1, 12, 00, "tosea");	// end date before start
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 2, 1, 12, 00, "tosea");		// length too long
		
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea");
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea");	// duplicate id name
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea1");
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea2");
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea3");
		sys.createTrip("SEA", _locs2, 2000, 1, 1, 12, 00, 2000, 1, 14, 12, 00, "tosea4");	// no ship available at date
		sys.createTrip("SEA", _locs2, 2000, 1, 14, 12, 00, 2000, 2, 1, 12, 00, "tosea4");	// no ship available at date
		sys.createTrip("SEA", _locs2, 2000, 1, 15, 12, 00, 2000, 2, 1, 12, 00, "tosea4");
		
		sys.createTrip("PACF", _locs3, 2000, 1, 1, 12, 00, 2000, 1, 13, 12, 00, "rand1");
		sys.createTrip("PACF", _locs3, 2000, 1, 15, 12, 00, 2000, 2, 1, 12, 00, "rand2");
		
		// create trip sections
		sys.createSpotSection("ASEA","tosea", "family", 1200, 150);		// provider does not exist
		sys.createSpotSection("SEA","tose", "family", 1200, 150);		// trip does not exist
		sys.createSpotSection("SEA","tosea", "familys", 1200, 150);		// invalid cabin class
		sys.createSpotSection("SEA","tosea", "family", -1200, 150);		// invalid price
		sys.createSpotSection("SEA","tosea", "family", 1200, 0);		// invalid count
		sys.createSpotSection("SEA","tosea", "family", 1200, 150);		// invalid count
		sys.createSpotSection("SEA","tosea", "family", 1200, 100);
		sys.createSpotSection("SEA","tosea", "family", 800, 50);		// section already exists
		
		sys.createSpotSection("PACF","rand1", "family", 1200, 100);
		sys.createSpotSection("PACF","rand1", "deluxfamily", 2000, 75);
		sys.createSpotSection("PACF","rand1", "couples", 800, 60);
		sys.createSpotSection("PACF","rand1", "deluxcouples", 1500, 50);
		
		sys.createSpotSection("PACF","rand2", "family", 1200, 100);
		sys.createSpotSection("PACF","rand2", "deluxfamily", 2000, 75);
		sys.createSpotSection("PACF","rand2", "couples", 800, 60);
		sys.createSpotSection("PACF","rand2", "deluxcouples", 1500, 50);
		
		// book spot
		sys.bookSpot("S3A", "TOSEA", "family");		// invalid provider
		sys.bookSpot("SEA", "TOSE4", "family");		// invalid trip ID
		sys.bookSpot("SEA", "TOSEA", "fam1ly");		// invalid cabin class
		// book all (and some extra - full)
		for (int _i = 0; _i < 105; ++ _i) {
			sys.bookSpot("SEA", "TOSEA", "family");
		}
		
		// find trips
		sys.findAvailableTrips("DEN", "DFW");	// invalid origin
		sys.findAvailableTrips("HAW", "DFW");	// invalid destination
		sys.findAvailableTrips("HAW", "JPN");	// no trips
		sys.findAvailableTrips("HAW", "HAW");
		sys.findAvailableTrips("LAS", "JPN");
		sys.findAvailableTrips("LAS", "HAW");
		sys.findAvailableTrips("JPN", "JPN");
		sys.findAvailableTrips("JPN", "HAW");
		
		// find trips with class + start date
		sys.findAvailableTrips("AFK", "JPN", "family", 2000, 1, 1);		// invalid origin
		sys.findAvailableTrips("LAS", "FAK", "family", 2000, 1, 1);		// invalid destination
		sys.findAvailableTrips("LAS", "JPN", "faasdly", 2000, 1, 1);	// invalid class
		sys.findAvailableTrips("LAS", "JPN", "family", 0, 1, 1);		// invalid year
		sys.findAvailableTrips("LAS", "JPN", "family", 2000, 0, 1);		// invalid month
		sys.findAvailableTrips("LAS", "JPN", "family", 2000, 13, 1);	// invalid month
		sys.findAvailableTrips("LAS", "JPN", "family", 2000, 1, 0);		// invalid day
		sys.findAvailableTrips("LAS", "JPN", "family", 2000, 1, 1);		// no trips
		sys.findAvailableTrips("JPN", "HAW", "family", 2000, 1, 1);
		sys.findAvailableTrips("JPN", "HAW", "deluxfamily", 2000, 1, 1);
		sys.findAvailableTrips("JPN", "HAW", "family", 2000, 1, 15);
		sys.findAvailableTrips("JPN", "HAW", "couples", 2000, 1, 15);
		
		sys.displaySystemDetails();
		
		// dump to file
		sys.dumpToFile("testcruise.ams");
		
		// create manager with file
		sys = new CruiseSystemManager("test1.ams");
		
		// display details
		sys.displaySystemDetails();
		
		// load another file
		sys.parseFile("testcruise.ams");
		
		// display details
		sys.displaySystemDetails();
		
		System.out.println("/////////////////////////////////////////////");
	}
	public static void testSimpleTextInterface() {
		System.out.println("/////////////////////////////////////////////Testing Simple text interface");
		SimpleTextInterface.menu();
		System.out.println("/////////////////////////////////////////////");
	}
	public static void testAdminTextInterface() {
		System.out.println("/////////////////////////////////////////////Testing admin text interface");
		AdminTextInterface.menu();
		System.out.println("/////////////////////////////////////////////");
	}	
}