@echo off

setlocal enabledelayedexpansion

set "CONFIG_FILE=Config.ini"

set "DIR1=Gir"
set "DIR2=Kuromi"

set "CONFIGURATIONS=Debug Release"

for %%d in ("%DIR1%" "%DIR2%") do (
	for %%c in (%CONFIGURATIONS%) do (
		set "DEST_DIR=%%~d\bin\%%c\net9.0"
		
		if not exist "!DEST_DIR!" mkdir "!DEST_DIR!"
		
		echo Copying %CONFIG_FILE% to "!DEST_DIR!"
		copy /Y "%CONFIG_FILE%" "!DEST_DIR!"
	)
)

echo Config was copied successfully!

pause
