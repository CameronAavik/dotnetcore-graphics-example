:: Clean output directory
rmdir /s /q out

:: Back up the current project.json
ren project.json project.json.bak

:: Windows
copy "build_jsons\project.windows.json" "project.json"
dotnet restore
dotnet publish -c Release -o "out\windows"
del project.json

:: OSX
copy "build_jsons\project.osx.json" "project.json"
dotnet restore
dotnet publish -c Release -o "out\osx"
del project.json

:: Linux
copy "build_jsons\project.linux.json" "project.json"
dotnet restore
dotnet publish -c Release -o "out\linux"
del project.json

:: Restore backup
ren project.json.bak project.json
dotnet restore