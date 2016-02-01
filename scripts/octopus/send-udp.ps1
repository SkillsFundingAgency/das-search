$Port = $OctopusParameters['LogstashPort']
$IPAddressStr = $OctopusParameters['ElasticServerIp']

function Send-Text($Text='Sample Text') {
    $endpoint = New-Object System.Net.IPEndPoint ([IPAddress]$IPAddressStr,$Port)
    $udpclient= New-Object System.Net.Sockets.UdpClient
    $bytes=[Text.Encoding]::ASCII.GetBytes($Text)
    $udpclient.Connect($endpoint)
    $bytesSent=$udpclient.Send($bytes,$bytes.length)
    $udpclient.Close()
}

Send-Text "Hello World PS!"