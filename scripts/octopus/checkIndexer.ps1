$env = $OctopusParameters['Octopus.Environment.Name']
$elasticUrl =  ($ElasticLogServerUrls -split ";")[0]

$reqBody = @"
{
  "query": {
    "bool": {
      "must": [
        {
          "match": {
            "Application": "search-index"
          }
        },
        {
          "match": {
            "Environment": "$env"
          }
        },
        {
          "match": {
            "level": "Info"
          }
        },
        {
          "match_phrase": {
            "message": "Starting indexer processing loop."
          }
        }
      ], 
      "filter": {
        "range": {
          "@timestamp": {
            "gte": "now-10m"
          }
        }
      }
    }
  }
}
"@

Write-Host $reqBody;

try
{
    $endPoint = "$elasticUrl/logstash-*/_search"
    Write-Host ("Querying "+$endPoint)
    $wr = [System.Net.HttpWebRequest]::Create($endPoint)
    $wr.Method= 'POST';
    $wr.ContentType="application/json";
    $Body = [byte[]][char[]]$reqBody;
    $wr.Timeout = 10000;

    $Stream = $wr.GetRequestStream();

    $Stream.Write($Body, 0, $Body.Length);

    $Stream.Flush();
    $Stream.Close();

    $resp = $wr.GetResponse().GetResponseStream()

    $sr = New-Object System.IO.StreamReader($resp) 

    $respTxt = $sr.ReadToEnd()

    Write-Host $respTxt
    Write-Host "----------"
    if(!($respTxt -match '"total":1,') )
    {
      Write-Host "!ERROR"
      throw "Can't find message"
    }else{
      Write-Host "!OK"
    }
}
catch
{
    $errorStatus = "Exception Message: " + $_.Exception.Message;
    Write-Host $errorStatus;
}