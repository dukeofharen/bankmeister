echo Deleting old bin and obj folders and .lock files
powershell.exe -noprofile -command "Get-ChildItem .\ -include bin,obj,project.lock.json -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }" || GOTO exception

echo Packing up for release
cd %~dp0\src\Bankmeister
dotnet restore || GOTO exception
dotnet publish --configuration=release --runtime=win10-x64 || GOTO exception

GOTO success

:exception
echo Build error!

:success
echo Everything cool!