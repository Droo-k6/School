***cscd 349 Final Project***

Java

A system manager for an airline/cruise (2 seperate systems). Supporting creating trips/flights, cabins/seats (as well as booking), setting prices, searching for specific flight/trip paths, times and cost. As well as a text menu interface and ability to read to/from a file.
Employs design patterns such as strategy, state, command and factory.

The FinalUML.pdf was made for the project, created using [draw.io](draw.io), FinalUML.xml is the xml file for the tool.

---
***Files/Classes***


**actbs package**
Holds all classes for Airline and Cruise Systems, and abstractions.

 * **SystemManager**
Abstract class to define base SystemManager functionality that children must implement so base/public calls all work, such as functions to create a provider (Airline, Cruiseline), or location (Airport,Seaport), etc.
Holds references to all Providers amd Locations in a system.
  * **AirlineSystemManager**
SystemManager for Airline system. Implements Airline specific functionality.
  * **CruiseSystemManager**
SystemManager for Cruise system. Implements Crusie specific functionality.

 * **Provider**
Abstract class to define a provider for a system. Holds references to trips and transports associated.
  * **Airline**
Provider for Airline system
  * **Cruise**
Provider for Cruise system

 * **Location**
Abstract class to define a location for a system. Contains a name, allows subclasses to implement own rules for naming.
  * **Airport**
Location for Airline system.
  * **Seaport**
Location for Cruise system.

 * **Trip**
Abstract class to define a trip for a system.
Holds identifer, date/time for beginning and end of a trip, a list of locations for the trip path as well as spot sections and associated transport.
  * **Flight**
Trip for Airline system.
  * **CruiseTrip**
Trip for Cruise system.

 * **Transport**
Abstract class to define a transport for a system.
Holds a list of trips associated with the transport.
  * **N/A for Airline system**
  * **Ship**
Transport for Cruise system.

 * **Spot**
Abstract class for a spot on a trip for a system. Holds bool for if booked, or if spot implementation uses columns instead of just rows/sequential spot. As well as 2 values for row and column.
  * **Seat**
Spot class for Airline system.
  * **Cabin**
Spot class for Cruise System. Does not use columns.

 * **SpotClass**
Interface for class of spot enumerations. Providing methods to help with user input/output.
Spot classes themselves are the classifications of the spots (such as first class, economy, etc for Airline)
  * **SeatClass**
SpotClass enumeration for Airline system.
  * **CabinClass**
SpotClass enumeration for Cabin system.

 * **SpotLayoutClass**
Interface for class of layout enumerations. Providing methods to help with user input/output.
SpotLayout classes for classifications of layouts, such as for Airline system - how many rows/columns of seats for a section.
  * **LayoutClass**
SpotLayoutClass enumeration for Airline system.
  * **N/A for Cruise system**

 * **SpotSection**
Abstract class for a section of spots on a trip. Holds value for price of a spot, class of layout, class of spots, the number of available spots and the total count.
  * **FlightSection**
SpotSection for Airline system.
  * **CabinSection**
SpotSection for Cruise system.

 * **SpotPreference**
Interface for spot preferences. Provindg methods to help with user input/output, and if a specific spot matches a preference.
Spot Preference Enumeration for when booking seats to prefer seats on window/aisle for airline system for example. In current state only applicable for systems that use columns in spots (such as airline).
  * **SeatPreference**
SpotPreference for Airline system.
  * **N/A for Cruise system**

 * **TimeOfDay**
Class used to just hold a time of day, used in trip.

 * **BaseTextInterface**
 Abstract class for base of a text interface, providing all functionality for text interface such as menu loop and calling appropriate commands from input. Child classes only need to implement a factory method to populate the commands list.
  * **SimpleTextInterface**
 A text interface/menu allowing ability to search flights/cruises, book seats/cabins with a preference or any available/specific seat, change prices of flight/cabin sections for a trip/flight, dump details of the system in a readable format to the console. As well as dump contents to a file that can be parsed back into a system later, or parse from a specific file.
  * **AdminTextInterface**
Text interface/menu allowind to create locations/trips/trip sections and display system information of both an Airline system and a Cruise system.
 

 * **SystemToFromFile**
 * **Parser**
  * **Various nested classes** All pertain to the state of the parsing loop, no reason to be public.

 * **Various Exceptions** All exceptions for issues in the system, mainly for bad inputs.

---
***Reflection***

The multiple exception classes are pointless, should remove all of them and just use the generic exceptions in the Java API. Their only real purpose is being pre-formated exceptions that don't need to be copy/pasted all over, but still could've employed a static class/methods to create the exception messages needed.

Abstracted a bit too much in similarties between the AirlineSystemManager and CruiseSystemManager, the project offered extra credit if implemented a TrainSystemManager as well - but I chose not do. If did go through with the extra credit then the level of abstraction done could be justified, but as of right now its a little messy.
Originally the first part of the project was just the AirlineSystemManager, then expanded to include CruiseSystemManager and ended up doing the whole combining both Systems into a similar abstraction. But it does work out, and can be expanded on.

The various interfaces/abstract classes and implemented static methods could be simplified down, although most exist just to simplify down generation of display messages to user.

TimeOfDay has to have an equivalent in the Java API, not sure why it wouldn't.
