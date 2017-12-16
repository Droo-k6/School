**cscd 437 regex tester**
Program to implement a set of regexes and accept user input to match against.
Program itself was done in C#, built a python script to help test the program itself.

---

List of regexes to implement

1. Social Security Number (can be with dashes, whitespace, or no spaces at all)
2. US Phone number - parentheses are optional, as well as the dash between the last two sections
3. E-mail address
4. Name on a class roster, assuming zero or more middle initials - Last name, First name, MI
5. Date in MM-DD-YYYY format - separators can be -'s, /'s
6. House address - Street number, street name, abbreviation for road, street, boulevard or avenue (full version of those items should also be accepted)
7. City followed by state followed by zip as it should appear on a letter
8. Military time, including seconds
9. US Currency down to the penny (ex: $123,456,789.23)
10. URL, optionally including http:// or https://, (Links to an external site.)Links to an external site. upper and lower case should be accepted
11. A password that contains at least 10 characters and includes at least one upper case character, one lower case character, one digit, one punctuation mark, and does not have more than 3 consecutive lower case characters
12. All words containing an odd number of alphabetic characters, ending in "ion"

---

Source files

**Program.cs**
C# source, main program

**tester.py**
builds up a list of input files and executes against the program
reports what inputs failed (did not match expected).

---
**Testing improvements**
The python script wasn't necessary for testing, I just wasn't familiar with unit testing when I took this class.