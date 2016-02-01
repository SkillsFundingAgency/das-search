Param($config, $key, $value)

$doc = New-Object System.Xml.XmlDocument
$doc.Load($config)
$node = $doc.SelectSingleNode('configuration/appSettings/add[@key="' + $key + '"]')
$node.Attributes['value'].Value = $value
$doc.Save($config)