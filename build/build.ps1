Write-Host "Running build script..."

function GetVersionFromCsProj([string] $pathToProject){
	[xml] $content = Get-Content $pathToProject
	return $content.Project.PropertyGroup.Version[0]
}

function GetApiKey([string] $keyFile){
	if(Test-Path($keyFile)){
		[string] $content = Get-Content $keyFile
		return $content;
	}

	return "";
}

function SetApiKey([string] $keyFile, [string] $content){
	[System.IO.File]::WriteAllText($keyFile, $content);
}

function DoInvokeExpression([string] $command){
	Write-Host "Executing: $command" -ForegroundColor Green
	Invoke-Expression "& $command"
}

$NugetUrl = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$NugetPath = Join-Path $PSScriptRoot "nuget.exe"
if (!(Test-Path $NugetPath)) {
    Write-Host "Downloading NuGet.exe..."
    (New-Object System.Net.WebClient).DownloadFile($NugetUrl, $NugetPath);
}

#Invoke-Expression "& `"$CAKE_EXE`" `"$Script`" -target=`"$Target`" -configuration=`"$Configuration`" -verbosity=`"$Verbosity`" $UseMono $UseDryRun $UseExperimental $ScriptArgs"

$projectName = "Amusoft.EventManagement";
$configuration = "Release";

$nugetUploadKey = "$PSScriptRoot/apiKey.txt"
$libProject = "$PSScriptRoot/../src/$projectName/$projectName.csproj";
$testProject = "$PSScriptRoot/../tests/$projectName.Test/$projectName.Test.csproj";
$libOutput = "$PSScriptRoot/../src/$projectName/bin/$configuration/";
$version = GetVersionFromCsProj($libProject);
$publishOutput = "$PSScriptRoot/../artifacts/"

Write-Host "Discovered version $version"

if(Test-Path($libOutput)){
	Write-Host "Cleaning output directory [$libOutput]" -ForegroundColor Green
	Remove-Item -Path $libOutput -Force -Recurse
}

DoInvokeExpression("dotnet restore `"$libProject`"")

DoInvokeExpression("dotnet build `"$libProject`" -c $configuration")

#DoInvokeExpression("dotnet test `"$testProject`" -c $configuration -v minimal --no-restore -f net40")
#DoInvokeExpression("dotnet test `"$testProject`" -c $configuration -v minimal --no-restore -f net45")
DoInvokeExpression("dotnet test `"$testProject`" -c $configuration -v minimal --no-restore -f netcoreapp2.0")

if((Read-Host "Pack? (y/n)") -eq "y"){
	
	if(Test-Path($publishOutput)){
		Write-Host "Cleaning output directory [$publishOutput]" -ForegroundColor Green
		Remove-Item -Path $publishOutput -Force -Recurse
	}

	DoInvokeExpression("dotnet pack `"$libProject`" -c $configuration -o $publishOutput --include-symbols --include-source")

	$pkgFiles = [System.IO.Directory]::GetFiles($publishOutput, "*.nupkg")	
	if($pkgFiles.Count -eq 0){
		Write-Host "No .nupkg file found." -ForegroundColor Red
		exit -1
	}

	$nl = [System.Environment]::NewLine
	$pkgJoin = [string]::Join($nl, $pkgFiles);
	
	Write-Host ([string]::Format("Found {0} files:{1}{2}", $pkgFiles.Count, $nl, $pkgJoin)) -ForegroundColor Cyan 

	if((Read-Host "Open folder? (y/n)") -eq "y"){
		Write-Host "Opening folder: $publishOutput" -ForegroundColor Green
		Invoke-Expression "explorer.exe $publishOutput"
	}

	if((Read-Host "Publish to nuget? (y/n)") -eq "y"){

		$apiKey = GetApiKey($nugetUploadKey)
		if($apiKey -eq ""){
			$apiKey = Read-Host "API Key?"
			SetApiKey $nugetUploadKey $apiKey
		}
		
		$firstFile = $pkgFiles[0]
		$secondFile = $pkgFiles[1]

		DoInvokeExpression("$NugetPath push $firstFile -ApiKey $apiKey -Source nuget.org")
		DoInvokeExpression("$NugetPath push $secondFile -ApiKey $apiKey -Source https://nuget.smbsrc.net")
	}
} 

Read-Host -Prompt "Script done. Press <enter>"