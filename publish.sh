dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r osx.10.10-x64
dotnet publish -c Release -r ubuntu.14.04-x64
cp "glfw.dll" "./bin/Release/netcoreapp1.0/win10-x64/publish"
cp "libglfw.dylib" "./bin/Release/netcoreapp1.0/osx.10.10-x64/publish"
cp "libglfw.so" "./bin/Release/netcoreapp1.0/ubuntu.14.04-x64/publish"