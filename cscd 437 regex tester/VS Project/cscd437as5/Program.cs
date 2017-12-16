// Mathew McCain
// cscd437 assignment 5: regex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// namespace for program
namespace cscd437as5 {
	// delegate for functions to test regex
	delegate bool Matches(string _str);

	// struct for a menu option
	struct MenuOption {
		// constructors
		public MenuOption(string _option, string _title, Matches _fnc) {
			Option = _option;
			Title = _title;
			Fnc = _fnc;
		}

		// fields
		public readonly string Option, Title;
		public readonly Matches Fnc;
	}

	// program class
	class Program {
		// menu options
		private static MenuOption[] Options = {
			new MenuOption("a","Social Security Number", MatchSSN),
			new MenuOption("b","US Phone Number", MatchPhone),
			new MenuOption("c","E-mail address (local@host.domain)", MatchEmail),
			new MenuOption("d","Name on a class roster, assuming one or more middle initials - Last name, First name, MI (middle initials seperated by spaces)", MatchRosterName),
			new MenuOption("e","Date in MM-DD-YYYY format (trailing zeroes can be left off)", MatchDate),
			new MenuOption("f","House address - Street number, street name, abbreviation for road, street, boulevard or avenue", MatchHouseAddress),
			new MenuOption("g","City followed by state (2 letter abbreviation) followed by zip as it should appear on a letter, Zip 4 code extension allowed (ex: Kent, WA 98030 or Cheney, WA 99004-9999)", MatchCityAddress),
			new MenuOption("h","Military time, including seconds (formats: HM, H:M, HM:S, H:M:S)", MatchMilitaryTime),
			new MenuOption("i","US Currency down to the penny (ex: $123,456,789.23, $1234.05, $1,457, $0.50, $.49)", MatchUSCurrency),
			new MenuOption("j","URL, including http:// (upper and lower case should be accepted)", MatchURL),
			new MenuOption("k","A password that contains at least 10 characters and includes at least one upper case character, lower case character, digit, punctuation mark, and does not have more than 3 consecutive lower case characters", MatchPassword),
			new MenuOption("l","All words containing an odd number of alphabetic characters, ending in \"ion\"", MatchOddIonWord)
		};

		// main method
		static void Main() {
			// attempt to stretch console window
			try {
				Console.SetWindowSize(190, Console.WindowHeight);
			}catch(Exception _e) {
				Console.WriteLine("Failed to stretch console window - {0}\n", _e.Message);
			}

			// loop for menu
			while(true) {
				// build menu
				BuildMenu();

				// get choice
				Console.Write("choice: ");
				string _choice = Console.ReadLine();

				// if option = 'q', quit
				if (_choice.Equals("q", StringComparison.OrdinalIgnoreCase)) {
					Console.WriteLine("Quiting...");
					break;
				}

				// get string
				Console.Write("Enter string: ");
				string _str = Console.ReadLine();
				_str = _str.Trim();

				// find correct menu option
				foreach(MenuOption _option in Options) {
					// match option, ignore case
					if (_choice.Equals(_option.Option, StringComparison.OrdinalIgnoreCase)) {

						// call delegate, report result
						try {
							Console.WriteLine("{0}", (_option.Fnc(_str)) ? ("Valid") : ("Invalid"));
						} catch(Exception _e) {
							Console.WriteLine("Exception occurred {0}: \"{1}\"", _e.GetType().ToString(), _e.Message);
						}
						

						break;
					}
				}

				// blank line for spacing
				Console.WriteLine("\n");
			}
		}

		// builds menu output
		private static void BuildMenu() {
			Console.WriteLine("Select regex to match -");

			// build from options array
			foreach(MenuOption _option in Options) {
				Console.WriteLine(" {0}. {1}", _option.Option, _option.Title);
			}

			// add on quit option
			Console.WriteLine(" q. quit");
		}


		// method to shorten regex matching
		private static bool MatchRegex(string _str, string _pattern) {
			// match
			// only matching ASCII
			Match _match = Regex.Match(_str, _pattern, RegexOptions.ECMAScript);
			// check if result found, and if length matches string
			return ((_match.Success) && (_match.Length == _str.Length));
		}


		// regex match functions
		// follow Matches() delegate

		// match SSN
		private static bool MatchSSN(string _str) {
			/*
				match formats
				###-##-####
				### ## ####
				######### 

				regex:
					^
						(\d{3}-\d{2}-\d{4})		// ###-##-####
						|
						(\d{3} \d{2} \d{4})		// ### ## ####
						|
						(\d{9})					// ######### 
					$
			*/
			return MatchRegex(_str, @"^(\d{3}-\d{2}-\d{4})|(\d{3} \d{2} \d{4})|(\d{9})$");
		}

		// match US phone number
		private static bool MatchPhone(string _str) {
			/*
				match formats
				###-###-####
				### ### ####
				(###)### ####
				(###)###-####
				(###)-###-####
				(###) ###-####
				(###) ### ####

				regex:
					^
						(\d{3}-\d{3}-\d{4})			// ###-###-####
						|
						(\d{3} \d{3} \d{4})			// ### ### ####
						|
						(\(\d{3}\)\d{3} \d{4})		// (###)### ####
						|
						(\(\d{3}\)\d{3}-\d{4})		// (###)###-####
						|
						(\(\d{3}\)-\d{3}-\d{4})		// (###)-###-####
						|
						(\(\d{3}\) \d{3}-\d{4})		// (###) ###-####
						|
						(\(\d{3}\) \d{3} \d{4})		// (###) ### ####
					$
			*/
			return MatchRegex(_str, @"^(\d{3}-\d{3}-\d{4})|(\d{3} \d{3} \d{4})|(\(\d{3}\)\d{3} \d{4})|(\(\d{3}\)\d{3}-\d{4})|(\(\d{3}\)-\d{3}-\d{4})|(\(\d{3}\) \d{3}-\d{4})|(\(\d{3}\) \d{3} \d{4})$");
		}

		// match email address
		private static bool MatchEmail(string _str) {
			/*
				match format
				local@host.domain
					local, alphanumeric characters
					host, alphanumeric characters, can contain multiple '.'s, but none trailing/consecutive
					domain, alphanumeric characters

				regex:
					[\w^_]	
						using ECMA script, so \w = [a-zA-Z0-9_]
						^_ to avoid '_'

					^
						(?!.*\.{2})				// stop consecutive '.'s
						[a-zA-Z0-9]+			// match local portion
						@
						(?!\.)					// stop '.' after '@'
						[a-zA-Z0-9\.]+			// match host
						\.
						[a-zA-Z0-9]+			// match domain
					$
			*/
			return MatchRegex(_str, @"^(?!.*\.{2})[a-zA-Z0-9]+@(?!\.)[a-zA-Z0-9\.]+\.[a-zA-Z0-9]+$");
		}

		// match name in a roster
		private static bool MatchRosterName(string _str) {
			/*
				match format
				last, first, m+
				last,first,m+
				last, first,m+
				last,first, m+
					last - last name, alphabetic (case insensitive)
					first - first name, alphabetic (case insensitive)
					m+ - middle initials - seperated by spaces

				regex:
					^
						[a-zA-Z]+,\s*		// last name, (optional whitespace)
						[a-zA-Z]+,\s*		// first name, (optional whitespace)
						[a-zA-Z]			// atleast one middle initial
						(\s+[a-zA-Z])*		// any following middle initials
						\s*					// allow trailing whitespace
					$

			*/
			return MatchRegex(_str, @"^[a-zA-Z]+,\s*[a-zA-Z]+,\s*[a-zA-Z](\s+[a-zA-Z])*\s*$");
		}

		// match date
		private static bool MatchDate(string _str) {
			/*
				match format
				M-D-Y
					M, 1/2 digits 01-12
					D, 1/2 digits 01-32
					Y, 4 digits, any

				regex:
					^
						(						// group for month
							(1[0-2])			// allow 10,11,12
							|
							(0?[1-9])			// allow 1-9 (optional 0 at front)
						)
						-
						(						// group for day
							(3[0-1])			// allow 30, 31
							|
							([1-2][0-9])		// allow  10-29
							|
							(0?[1-9])			// allow 1-9 (optional 0 at front)
						)
						-
						(\d{4})					// year, 0000-9999
					$
			*/

			// regex check first
			if (MatchRegex(_str, @"^((1[0-2])|(0?[1-9]))-((3[0-2])|([1-2]?[0-9])|(0?[1-9]))-(\d{4})$")) {
				// check if given date is valid
				// parse to DateTime
				string[] _formats = { "MM-dd-yyyy", "M-dd-yyyy", "MM-d-yyyy", "M-d-yyyy" };
				DateTime _date;

				return DateTime.TryParseExact(_str, _formats, null, DateTimeStyles.None, out _date);
			}

			return false;
		}

		// match house address
		private static bool MatchHouseAddress(string _str) {
			/*
				match format
					STREET NAME ABR
						STREET, street number - set of numbers
						NAME, street name - set of alphabetic characters
						ABR, street abreviation - only the following accepted
							RD, ST, BLVD, AVE

				regex:
					^
						\d+				// street number
						\s
						[a-zA-Z]+		// street name
						\s
						(				// accepted abbreviations (case insensitive)
							([rR][dD])
							|([sS][tT])
							|([bB][lL][vV][dD])
							|([aA][vV][eE])
						)
					$

			*/
			return MatchRegex(_str, @"^\d+\s[a-zA-Z]+\s(([rR][dD])|([sS][tT])|([bB][lL][vV][dD])|([aA][vV][eE]))$");
		}

		// match city, state zip
		private static bool MatchCityAddress(string _str) {
			/*
				match format
					CITY, STATE ZIP
					CITY, STATE ZIP-4CODE
						CITY, the city - alphabetic
						STATE, state abbreviation - 2 alphabetic characters
						ZIP, zip code - 5 numbers
						4CODE, zip-4code - 4 numbers

				regex:
					^
						[a-zA-Z]+		// city
						,\s
						[a-zA-Z]{2}		// state abbreviation
						\s
						\d{5}			// zip
						(-\d{4})?		// optional 4code
					$
			*/
			return MatchRegex(_str, @"^[a-zA-Z]+, [a-zA-Z]{2} \d{5}(-\d{4})?$");
		}

		// match military time
		private static bool MatchMilitaryTime(string _str) {
			/*
				match formats:
					HM
					H:M
					HM:S
					H:M:S
						H - hours, 2 numbers (00-23)
						M - minutes, 2 numbers (00-59)
						S - seconds, 2 numbers (00-59)

				regex:
					^
						(					// match hours
							([01][0-9])		// 00-19
							|
							(2[0-3])		// 20-23
						)
						:?					// optional
						([0-5][0-9])		// minutes, 00-59
						(:[0-5][0-9])?		// : seconds, 00-59 (optional)
					$
			*/
			return MatchRegex(_str, @"^(([01][0-9])|(2[0-3])):?([0-5][0-9])(:[0-5][0-9])?$");
		}

		// match US currency including penny
		private static bool MatchUSCurrency(string _str) {
			/*
				match formats
					$A.B
						$ - dollar sign (optional)
						A - whole dollars (optional - if cents are available)
							if any commas, must be properly seperated
						B - cents to the penny (optional - if whole dollars avail)

				regex:
					^
						\$
						(
							(				// match only cents, no whole dollars
								\.\d{2}		// cents to the penny
							)
							|
							(				// match dollars, no commas
								\d+			// whole dollars
								(\.\d{2})?	// optional cents
							)
							|
							(				// match dollars, commas
								\d{1,3}		// initial values
								(,\d{3})*	// proper comma seperation
								(\.\d{2})?	// optional cents
							)
						)
					$
			*/
			return MatchRegex(_str, @"^\$((\.\d{2})|(\d+(\.\d{2})?)|(\d{1,3}(,\d{3})*(\.\d{2})?))$");
		}

		// match URL
		private static bool MatchURL(string _str) {
			/*
				match formats
					A://B.C
						A- protocol (optional if '://' not present
						B- host, alphanumeric, '-', '.'
							no trailing/consecutive symbols
						C-domain, alphanumeric, '-'
							no trailing/consecutive symbols
				
				regex:
					^
						([hH][tT]{2}[pP][sS]?:\/\/)?	// match protocol (optional, case insensitive)
						(								// host
							([a-zA-Z0-9]\.[a-zA-Z0-9])	// allow '.' in between alphanumerics
							|
							([a-zA-Z0-9]\-[a-zA-Z0-9])	// allow '-' in between alphanumerics
							|
							([a-zA-Z0-9])
						)+
						\.
						(								// domain
							([a-zA-Z0-9]\-[a-zA-Z0-9])	// allow '-' in between alphanumerics
							|
							([a-zA-Z0-9])
						)+
					$
			*/
			return MatchRegex(_str, @"^([hH][tT]{2}[pP][sS]?:\/\/)?(([a-zA-Z0-9]\.[a-zA-Z0-9])|([a-zA-Z0-9]\-[a-zA-Z0-9])|([a-zA-Z0-9]))+\.(([a-zA-Z0-9]\-[a-zA-Z0-9])|([a-zA-Z0-9]))+$");
		}

		// match a password
		private static bool MatchPassword(string _str) {
			/*
				match format
					at least 10 characters
					at least one upper case letter
					at least one lower case letter
					at least one number
					at least one punctuation mark
						.,;:!?'"-_()[]{}<>@#$%^&*\/|=+`~
					no more than 3 consecutive lower case letters
					no whitespace

				regex:
					(?=.*[a-z])					// atleast on lower case letter
					(?=.*[A-Z])					// atleast one upper case letter
					(?=.*[0-9])					// atleast one number
					(?=.*[!-//:-@\[-`\{-~])		// atleast one punctuation
					(?!.*[a-z]{4,})				// no more than 3 consecutive lower case letters
					^
						[a-zA-Z0-9!-//:-@\[-`\{-~]{10,}		// alphanumeric, punctuation, atleast 10 matches
					$	
			*/
			return MatchRegex(_str, @"(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!-//:-@\[-`\{-~])(?!.*[a-z]{4,})^[a-zA-Z0-9!-//:-@\[-`\{-~]{10,}$");
		}

		// match a word containing an odd nubmer of alphabetic characters, ending in "ion"
		private static bool MatchOddIonWord(string _str) {
			/*
				match format
					odd number of alphabetic characters
					ends in "ion"
					case insensitive

				regex:
					^
						(					// match groups of 2 letters, even number here + 3 = odd number
							([a-zA-Z]){2}
						)*
						[iI][oO][nN]		// "ion", case insensitive
					$
			*/
			return MatchRegex(_str, @"^(([a-zA-Z]){2})*[iI][oO][nN]$");
		}
	}
}
