Param($config, $key, $value)

$doc = New-Object System.Xml.XmlDocument
$doc.Load($config)
$node = $doc.SelectSingleNode('configuration/specBind/browserFactory/settings/add[@name="' + $key + '"]')
$node.Attributes['value'].Value = $node.Attributes['value'].Value + ' ' + $value
$doc.Save($config)