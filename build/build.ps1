Write-Host "Running build script..."

function GetVersionFromCsProj([string] $pathToProject){
	[xml] $content = Get-Content $pathToProject
	return $content.Project.PropertyGroup.Version[0]
}

function DoInvokeExpression([string] $command){
	Write-Host "Executing: $command" -ForegroundColor Green
	Invoke-Expression "& $command"
}

#Invoke-Expression "& `"$CAKE_EXE`" `"$Script`" -target=`"$Target`" -configuration=`"$Configuration`" -verbosity=`"$Verbosity`" $UseMono $UseDryRun $UseExperimental $ScriptArgs"

$projectName = "Amusoft.EventManagement";
$configuration = "Release";

$libProject = "../src/$projectName/$projectName.csproj";
$testProject = "../tests/$projectName.Test/$projectName.Test.csproj";
$libOutput = "../src/$projectName/bin/$configuration/";
$version = GetVersionFromCsProj($libProject);
$publishOutput = "../artifacts/"

Write-Host "Discovered version $version"

if(Test-Path($libOutput)){
	Write-Host "Cleaning output directory [$libOutput]" -ForegroundColor Green
	Remove-Item -Path $libOutput -Force -Recurse
}

DoInvokeExpression("dotnet restore `"$libProject`"")

DoInvokeExpression("dotnet build `"$libProject`" -c $configuration -o $publishOutput")

DoInvokeExpression("dotnet test `"$testProject`" -c $configuration -v minimal --no-restore -f net40")
DoInvokeExpression("dotnet test `"$testProject`" -c $configuration -v minimal --no-restore -f net45")
DoInvokeExpression("dotnet test `"$testProject`" -c $configuration -v minimal --no-restore -f netcoreapp2.0")

Read-Host -Prompt "Script done. Press <enter>"
