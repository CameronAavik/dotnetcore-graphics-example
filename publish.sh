# Clean output directory
rm -rf out

# Back up the current project.json
mv project.json project.json.bak

# Windows
cp "/build_jsons/project.windows.json" "project.json"
dotnet restore
dotnet publish -c Release -o "out/windows"
rm project.json

# OSX
cp "/build_jsons/project.osx.json" "project.json"
dotnet restore
dotnet publish -c Release -o "out/osx"
rm project.json

# Linux
cp "/build_jsons/project.linux.json" "project.json"
dotnet restore
dotnet publish -c Release -o "out/linux"
rm project.json

# Restore backup
mv project.json.bak project.json
dotnet restore