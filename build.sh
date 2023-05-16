echo Attempting APK build
cd DiceGame
dotnet publish -c Release -f:net6.0-android
echo Attempting iOS build
dotnet publish -c Release -f net6.0-ios -r ios-arm64
