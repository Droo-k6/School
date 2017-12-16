"""
Tester for cscd437as5
Mathew McCain, 5/3/2017

usage
python tester.py "input files directory"
	-p program directory, directory of program to use input against
	-o output name, create final input file with given name
	one of the above options must be selected, quotes are valid for a full path
example
	python tester.py "..\inputs" -p "E:\Documents\My Documents\Visual Studio 2015\Projects\cscd437as5\cscd437as5\bin\Debug\cscd437as5.exe"
	python tester.py "..\inputs" -p "..\vs\cscd437as5\cscd437as5\bin\Debug\cscd437as5.exe"
	python tester.py "..\inputs" -o input.txt
"""

# libraries
import sys
import os
import argparse
#from pathlib import Path
import ntpath
import re

# classes
# parses info for an input file to build
class InputFile():
	# initializer
	# _str, string to use
	# _choice, choice made
	# _output, expected string
	# _expected, expected count of output
	def __init__(self, _str, _choice, _output, _expected):
		self.str = _str
		self.choice = _choice
		self.output = _output
		self.expected = _expected

	# builds object for given filename
	def build(_filepath):
		# verify exists
		if not(os.path.isfile(_filepath)):
			print("filepath is not a file: \"{}\"".format(_filepath))
			return None
		
		# get filename
		_filename = ntpath.basename(_filepath)
		
		# split off extensions
		_splitName = _filename.split(".")[0]
		# determine choice and expected result
		_split = _splitName.split("_")
		
		if (len(_split) != 2):
			print("filename in invalid format: \"{}\"".format(_filename))
			return None
			
		_choice = _split[0]
		_result = _split[1].lower()
		
		if (_result != "valid" and _result != "invalid"):
			print("invalid expected result \"{}\": \"{}\"".format(_result, _filename))
			return None
		
		# open file
		_file = open(_filepath, 'r')
		# get lines
		_lines = _file.readlines()
		_file.close
		
		# count of expected results = # of lines
		_expected = len(_lines)
		
		# build input string
		_input = ""
		for _line in _lines:
			_input += "{}\n{}".format(_choice,_line)
		_input += "\nq"
		
		# build object
		_fileInput = InputFile(_input, _choice, _result, _expected)
		return _fileInput
		
	# builds list of objects for given directory
	def getFiles(_path):
		_list = []
		
		# get list of files
		_files = os.listdir(_path)
		for _file in _files:
			_list.append(InputFile.build(_path + "/" +_file))
		
		return _list

# functions

# parses program arguments
def getArgs():
	# build parser
	parser = argparse.ArgumentParser(description="Parse optional syntax")
	
	# positional
	# input directory
	parser.add_argument("input")
	
	# optional arguments
	# program ndirectory
	parser.add_argument("-p", help="directory for program")
	# name for final input file to create
	parser.add_argument("-o", help="name for final input file")
	
	# parse arguments
	_args = parser.parse_args()
	# convert to a dictionary
	_dict = vars(_args)
	
	return _dict

# writes list of input files to output file
def writeTo(_outputName, _inputFiles):
	# build file contents
	_contentList = []
	for _input in _inputFiles:
		# lump off q at the end
		_str = _input.str.replace("\nq","\n")
		_contentList.append(_str)
		
	# build contents
	_contents = "".join(_contentList) + "q"
	
	# write to file
	_output = open(output_name, 'w')
	_output.write(_contents)
	_output.close()
	
# runs tests
def runTest(_programDir, _inputFiles):
	# validate executable exists
	if not(os.path.isfile(_programDir)):
		print("file does not exist: \"{}\"".format(_programDir))
		return
	
	# run tests
	for _input in _inputFiles:
		execTest(_programDir, _input)
		
	# run program
	#os.system("\"{}\"".format(_programDir))
	
# performs 1 test
def execTest(_programDir, _input):
	# names of files to use
	_filenameInput = "temp"
	_filenameOutput = "out"
	
	# create input file
	_fileInput = open(_filenameInput, 'w')
	_fileInput.write(_input.str)
	_fileInput.close()
	
	# execute program
	# redirect input file, direct stdout to given file
	os.system("\"{}\" < {} 1> {}".format(_programDir, _filenameInput, _filenameOutput))
	
	# read output file
	_fileOut = open(_filenameOutput, 'r')
	_contents = _fileOut.read()
	_fileOut.close()
	
	# get reults
	displayResults(_input, _contents)
	
	# delete files
	os.remove(_filenameInput)
	os.remove(_filenameOutput)
	
# determines results of program
def displayResults(_input, _results):
	# match occurences of result
	#_matches = re.findall("\\s{}\\s".format(_input.output), _contents, flags=re.IGNORECASE)
	#print("{} \"{}\": {}/{}".format(_input.choice, _input.output, len(_matches), _input.expected))
	
	# match all occurences of valid
	#_matchesValid = re.findall("\\svalid\\s", _results, flags=re.IGNORECASE)
	_matchesValid = getMatches("\\svalid\\s", _results)
	# match all occurences of invalid
	#_matchesInvalid = re.findall("\\sinvalid\\s", _results, flags=re.IGNORECASE)
	_matchesInvalid = getMatches("\\sinvalid\\s", _results)
	
	# display results
	_matched = 0
	if (_input.output == "valid"):
		_matched = len(_matchesValid)
	else:
		_matched = len(_matchesInvalid)
	print("{} \"{}\": {}/{}".format(_input.choice, _input.output, _matched, _input.expected))
	
	# determine if totals to expected output
	if ((len(_matchesValid) + len(_matchesInvalid)) != _input.expected):
		print("\tone input failed to report a result, {}/{}".format((len(_matchesValid) + len(_matchesInvalid)), _input.expected))
	
	# test if mismatch in expected count
	if (_matched != _input.expected):
		# display results in order
		displayMatches(_matchesValid, _matchesInvalid)
	
# gets all match objects for a regex search
def getMatches(_pattern, _str):
	# build iterator
	_it = re.finditer(_pattern, _str, flags=re.IGNORECASE)
	
	_list = []
	for _match in _it:
		_list.append(_match)
	
	return _list
	
# displays matches in order
def displayMatches(_matchesValid, _matchesInvalid):
	#print("valid: {}".format(_matchesValid))
	#print("invalid: {}".format(_matchesInvalid))
	
	# put into sorted list
	_list = []
	while ((len(_matchesValid) > 0) and (len(_matchesInvalid) > 0)):
		_valid = _matchesValid[0]
		_invalid = _matchesInvalid[0]
		# compare span values
		if (_valid.span()[0] < _invalid.span()[0]):
			_matchesValid.pop(0)
			_list.append("valid")
		else:
			_matchesInvalid.pop(0)
			_list.append("invalid")
	
	# empty lists
	while (len(_matchesValid) > 0):
		_matchesValid.pop(0)
		_list.append("valid")
		
	while (len(_matchesInvalid) > 0):
		_matchesInvalid.pop(0)
		_list.append("invalid")
		
	# display results
	i = 0
	for _result in _list:
		i += 1
		print("\t{}: {}".format(i, _result))
	
#############################################################################
# MAIN

# get arguments
args = getArgs()

directory_input = args['input'];
directory_program = args['p'];
output_name = args['o'];

#print("input directory: {}".format(directory_input))
#print("program directory: {}".format(directory_program))
#print("output file: {}".format(output_name))

# validate input path exists
if not(os.path.exists(directory_input)):
	print("input path does not exist: \"{}\"".format(directory_input))
	sys.exit()

# validate program or output name selected
if ((directory_program is None) and (output_name is None)):
	print("must use a program or provide an output file name")
	sys.exit()
	
# parse input files
inputFiles = InputFile.getFiles(directory_input)

#for _input in inputFiles:
	#print("{}|{}, {}".format(_input.choice, _input.output, _input.expected))

# build output file if desired
if not(output_name is None):
	writeTo(output_name, inputFiles)

# run tester if program directory supplied
if not(directory_program is None):
	runTest(directory_program, inputFiles)

