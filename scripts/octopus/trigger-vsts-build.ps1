# -- o Variables o -- #

$Port = $OctopusParameters['LogstashPort']
$IPAddressStr = $OctopusParameters['ElasticServerIp']

$username = $OctopusParameters['VSTSUserName']
$password = $OctopusParameters['VSTSToken']
$builddefnumber = $OctopusParameters['BuildConfigurationId']

function Send-Text($Text='Sample Text') {
    $endpoint = New-Object System.Net.IPEndPoint ([IPAddress]$IPAddressStr,$Port)
    $udpclient= New-Object System.Net.Sockets.UdpClient
    $bytes=[Text.Encoding]::ASCII.GetBytes($Text)
    $udpclient.Connect($endpoint)
    $bytesSent=$udpclient.Send($bytes,$bytes.length)
    $udpclient.Close()
}

$account =  "sfa-gov-uk.visualstudio.com"
$project = "Digital%20Apprenticeship%20Service"
$uri = "https://$account/DefaultCollection/$project/_apis/build/builds/1?api-version=2.0"

# -- o Auth / headers / body o -- #

$basicAuth = ("{0}:{1}" -f $username,$password)
$basicAuth = [System.Text.Encoding]::UTF8.GetBytes($basicAuth)
$basicAuth = [System.Convert]::ToBase64String($basicAuth)
$headers = @{Authorization=("Basic {0}" -f $basicAuth)}

$body = "{'definition': {'id': $builddefnumber}}"

# -- o post request o -- #

try{
    Invoke-RestMethod -Uri $uri -headers $headers -Method Post -Body $body -ContentType "application/json"
    Send-Text "Info - Octopus - VSTS build started"
}catch{
    Send-Text "Error - Octopus - Problem starting VSTS build - $_"
    throw $_
}
