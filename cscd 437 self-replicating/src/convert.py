# used to setup a program for self replication
# args: input_filename output_filename
# both should be .c files
# the tag to mark the replication location is /*@replicate*/
# this is just an initial version, improvements could be made to this script

import sys

# main
# check if arguments available
if (len(sys.argv) != 3):
	print("invalid arguments\nusage: convert.py input_name output_name\nnote: should be '.c' files\nspecify points for the replication portion with /*@replicate*/")
	sys.exit()

# pull filenames
inputFilename = sys.argv[1]
outputFilename = sys.argv[2]

# read input contents
try:
	inputfile = open(inputFilename, "r")
except IOError:
	print("file \"" + inputFilename + "\" does not exist")
	sys.exit()

inputContents = inputfile.read()
inputfile.close()


# parse contents/build char array
# also finds location to insert replication portion
# int array that will be the char array
contentsArr = []
# for inserting a replication portion
insertedReplication = False
# string to identify an insertion
insertMark = "/*@replicate*/"
# string to insert at replicate marker
#replicatePortion = "printf(\"replicate me\\n\")"
replicatePortion = "//replicate\n\tint counter=1;\n\tif(argc>1)\n\t\tcounter=atoi(argv[1]);\n\tif(counter>5)\n\t\treturn 0;\n\t++counter;\n\tchar ss[20];\n\tsprintf(ss,\"./rout %d &\",counter);\n\tFILE *repout = fopen(\"fout.c\",\"w\");\n\tprintf(\"char s[]={\");\n\tfputs(\"char s[]={\",repout);\n\tint i=0;\n\tfor (; s[i]; ++i){\n\t\tprintf(\"%d,\", s[i]);\n\t\tfprintf(repout,\"%d,\", s[i]);\n\t}\n\tprintf(\"%s\",s);\n\tfprintf(repout,\"%s\",s);\n\tfclose(repout);\n\tsystem(\"gcc fout.c -o rout\");\n\tsystem(ss);"

# track matching a string
matchCounter = 0
matchLength = len(insertMark)
for c in inputContents:
	if (matchCounter > 0):
		if (c == insertMark[matchCounter]):
			matchCounter+=1
			if (matchCounter >= matchLength):
				matchCounter=0
				insertedReplication = True
				for rc in replicatePortion:
					contentsArr.append(ord(rc))
		else:
			for i in range(matchCounter):
				contentsArr.append(ord(insertMark[i]))
			contentsArr.append(ord(c))
			matchCounter = 0
	elif (c == insertMark[0]):
		matchCounter = 1
	else:
		contentsArr.append(ord(c))


# check if a replication portion was inserted
if not insertedReplication:
	print("ERROR: no replication points found, mark with \"/*@replicate*/\"")
	sys.exit()

# finalize char array
contentsArr.insert(0,ord('\n'))
contentsArr.insert(0,ord(';'))
contentsArr.insert(0,ord('}'))
contentsArr.insert(0,ord('0'))
# python puts some weird value in place
#contentsArr.append(0)

#map(ord,s)	convert string to list of ascii values
#map(chr,a)	convert list to list of char values (from ascii)

# write to output file
outputfile = open(outputFilename, "w")

# write char array out
outputfile.write("char s[]={")
for k in contentsArr:
	outputfile.write("{},".format(k));
outputfile.write("0")
# convert arr to string to use
finalcontents = ''.join(map(chr,contentsArr))

# write contents
outputfile.write(finalcontents);

outputfile.close()

print("ended successfully")

