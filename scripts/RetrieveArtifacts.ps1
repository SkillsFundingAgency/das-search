Param($url, $authToken, $zipFile)

Add-Type -AssemblyName System.IO.Compression.FileSystem

function Unzip($zipfile, $outpath)
{
    $testdir = "$outpath\test"
    if(Test-Path $testdir){
        Write-Host "OK"
        Remove-Item -Recurse -Force $testdir
        Write-Host "Output path deleted: $testdir"
    }
    
    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
    Write-Host "Copy done"
}

$webClient = new-object System.Net.WebClient
$data = $webClient.DownloadString($url + "/api/version")
$buildId = ($data | ConvertFrom-Json).BuildId
Write-Host $buildId
$url2 = "https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/build/builds/$buildId/artifacts?api-version=2.0"
Write-Host $url2

$buildFolder = (Get-Item -Path ".\build" -Verbose).FullName
$zipFile = $buildFolder + "\" + $zipFile
Write-Host $zipFile

$headers = @{Authorization=("Bearer {0}" -f $authToken)}

$json = Invoke-RestMethod -Uri $url2 -headers $headers -Method Get
$downloadUrl = $json.value[0].resource.downloadUrl
Write-Host $downloadUrl
$webClient.Headers.Add("Authorization", ("Bearer {0}" -f $authToken))
$webClient.DownloadFile($downloadUrl, $zipFile)

Write-Host "Download done"

Unzip $zipFile $buildFolder

Get-ChildItem -Path ".\" -Filter *.dll -Recurse

ls ".\build"

Write-Host "Retrive artifact done"