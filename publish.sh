# Back up the source file
mv Program.cs Program.cs.temp
echo "#define WINDOWS" | cat . Program.cs.temp > Program.cs
dotnet publish -c Release -r win10-x64
rm Program.cs
echo "#define OSX" | cat . Program.cs.temp > Program.cs
dotnet publish -c Release -r osx.10.10-x64
rm Program.cs
echo "#define LINUX" | cat . Program.cs.temp > Program.cs
dotnet publish -c Release -r ubuntu.14.04-x64
rm Program.cs
mv Program.cs.temp Program.cs
cp "glfw.dll" "./bin/Release/netcoreapp1.0/win10-x64/publish"
cp "libglfw.dylib" "./bin/Release/netcoreapp1.0/osx.10.10-x64/publish"
cp "libglfw.so" "./bin/Release/netcoreapp1.0/ubuntu.14.04-x64/publish"