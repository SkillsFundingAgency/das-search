Param($file, $key, $value)

$contents = cat $file
$contents -replace "@@$key@@", "$value" > $file