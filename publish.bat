:: Back up the source file
ren Program.cs Program.cs.temp
:: Windows publish
copy Program.cs.temp Program.cs
echo #define WINDOWS > define.txt
type Program.cs >> define.txt
dotnet publish -c Release -r win10-x64
del Program.cs
:: OSX publish
copy Program.cs.temp Program.cs
echo #define OSX > define.txt
type Program.cs >> define.txt
dotnet publish -c Release -r osx.10.10-x64
del Program.cs
:: Linux publish
copy Program.cs.temp Program.cs
echo #define LINUX > define.txt
type Program.cs >> define.txt
dotnet publish -c Release -r ubuntu.14.04-x64
del Program.cs
del define.txt
:: Restore backup
ren Program.cs.temp Program.cs
:: Copy over GLFW
copy ".\glfw.dll" ".\bin\Release\netcoreapp1.0\win10-x64\publish\glfw.dll"
copy ".\libglfw.dylib" ".\bin\Release\netcoreapp1.0\osx.10.10-x64\publish\libglfw.dylib"
copy ".\libglfw.so" ".\bin\Release\netcoreapp1.0\ubuntu.14.04-x64\publish\libglfw.so"