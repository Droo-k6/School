@echo off
SET remDirectory=bin
IF EXIST %remDirectory% (
	rmdir /s /q %remDirectory%
	echo %remDirectory% deleted
) ELSE (
	echo %remDirectory% does not exist
)