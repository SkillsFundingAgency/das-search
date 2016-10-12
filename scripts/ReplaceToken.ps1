Param($file, $key, $value)

(cat $file) -replace '@@$key@@', "$value" > $file