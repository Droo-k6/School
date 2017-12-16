@echo off
IF NOT EXIST "bin" (
	mkdir "bin"
)
javac -d "bin" src\cscd467\hw45\*.java