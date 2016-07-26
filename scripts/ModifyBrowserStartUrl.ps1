Param($config, $value)

$doc = New-Object System.Xml.XmlDocument
$doc.Load($config)
$node = $doc.SelectSingleNode('configuration/specBind/application[@key="startUrl"]')
$node.Attributes['startUrl'].Value = $value
$doc.Save($config)