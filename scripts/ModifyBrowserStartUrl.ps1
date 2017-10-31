Param($config, $value)

$configFiles = Get-ChildItem $config

foreach ($configFile in $configFiles)
{
	$doc = New-Object System.Xml.XmlDocument
	$doc.Load($configFile)
	$node = $doc.SelectSingleNode('configuration/specBind/application')
	$node.Attributes['startUrl'].Value = $value
	$doc.Save($configFile)
	Write-Host "updated startUrl in $configFile to $value" -BackgroundColor darkgreen
}