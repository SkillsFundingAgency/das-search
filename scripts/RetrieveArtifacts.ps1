Param($url, $username, $password)

Add-Type -AssemblyName System.IO.Compression.FileSystem

function Unzip($zipfile, $outpath)
{
    Remove-Item -Recurse -Force "$outpath\test"
    Write-Host "Output path deleted: $outpath\test"
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
    Write-Host "Copy done"
}

$webClient = new-object System.Net.WebClient
$buildId = $webClient.DownloadString($url + "/api/version") -replace '"', ''
Write-Host $buildId
$url2 = "https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/build/builds/$buildId/artifacts?api-version=2.0"
Write-Host $url2

$buildFolder = (Get-Item -Path ".\build" -Verbose).FullName
$zipFile = $buildFolder + "\tests.zip"
Write-Host $zipFile

$basicAuth = ("{0}:{1}" -f $username,$password)
$basicAuth = [System.Text.Encoding]::UTF8.GetBytes($basicAuth)
$basicAuth = [System.Convert]::ToBase64String($basicAuth)
$headers = @{Authorization=("Basic {0}" -f $basicAuth)}

$json = Invoke-RestMethod -Uri $url2 -headers $headers -Method Get
$downloadUrl = $json.value[1].resource.downloadUrl
Write-Host $downloadUrl
$webClient.Headers.Add("Authorization", ("Basic {0}" -f $basicAuth))
$webClient.DownloadFile($downloadUrl, $zipFile)

Write-Host "Download done"

Unzip $zipFile $buildFolder

Get-ChildItem -Path ".\" -Filter *.dll -Recurse

ls ".\build\test"

Write-Host "Retrive artifact done"