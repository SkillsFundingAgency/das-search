Param($folder, $testdll)

function XmlDocTransform($xml, $xdt)
{
    if (!$xml -or !(Test-Path -path $xml -PathType Leaf)) {
        throw "File not found. $xml";
    }
    if (!$xdt -or !(Test-Path -path $xdt -PathType Leaf)) {
        throw "File not found. $xdt";
    }

    $scriptPath = (Get-Variable MyInvocation -Scope 1).Value.InvocationName | split-path -parent
    Add-Type -LiteralPath "$scriptPath\Microsoft.Web.XmlTransform.dll"

    $xmldoc = New-Object Microsoft.Web.XmlTransform.XmlTransformableDocument;
    $xmldoc.PreserveWhitespace = $true
    $xmldoc.Load($xml);

    $transf = New-Object Microsoft.Web.XmlTransform.XmlTransformation($xdt);
    if ($transf.Apply($xmldoc) -eq $false)
    {
        throw "Transformation failed."
    }
    $xmldoc.Save($xml);
}

get-childitem $folder -include App.*.config -recurse | foreach ($_) {
	$shortname = $_.name -replace 'App.', '' -replace '.config', '' -replace ' ','' -replace '[.]',''
	$newdll = $testdll -replace '[.]dll', ("." + $shortname + ".dll")
	$newconfig = ($folder + "\" + $newdll + ".config")
	$newconfig
	$_.fullname
	#Copy-Item  ($folder + "\" + $testdll) ($folder + "\" + $newdll)
	#Copy-Item  ($folder + "\App.config") $newconfig
	XmlDocTransform($newconfig, '\"$_.fullname\"')
}

#remove-item $testdll 
#remove-item ($testdll + ".config")


# foreach transform

	# apply transform
	# run tests
	