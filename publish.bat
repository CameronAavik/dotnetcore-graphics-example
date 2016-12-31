dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r osx.10.10-x64
dotnet publish -c Release -r ubuntu.14.04-x64
copy ".\glfw.dll" ".\bin\Release\netcoreapp1.0\win10-x64\publish\glfw.dll"
copy ".\libglfw.dylib" ".\bin\Release\netcoreapp1.0\osx.10.10-x64\publish\libglfw.dylib"
copy ".\libglfw.so" ".\bin\Release\netcoreapp1.0\ubuntu.14.04-x64\publish\libglfw.so"