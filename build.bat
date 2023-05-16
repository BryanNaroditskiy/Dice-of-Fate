@echo off
echo Attempting APK build
cd DiceGame
dotnet publish -c Release -f:net6.0-android
pause