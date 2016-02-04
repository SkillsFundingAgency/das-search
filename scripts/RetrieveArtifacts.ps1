Param($url)

$webClient = new-object System.Net.WebClient
$buildId = $webClient.DownloadString($url + "/api/version")
Write-Host $buildId