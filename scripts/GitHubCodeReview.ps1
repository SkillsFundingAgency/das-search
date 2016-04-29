function New-SearchPattern
{
    param($name, $regex)

    $searchPattern = New-Object PSObject

    $searchPattern | add-member -type NoteProperty -Name Name -value $name
    $searchPattern | add-member -type NoteProperty -Name Regex -value $regex

    return $searchPattern    
}

function New-FileRegexLineMatch
{
    param($filepath, $lineNum, $pattern, $value)

    $fileRegexMatch = New-Object PSObject

    $fileRegexMatch | add-member -type NoteProperty -Name FilePath -value $filepath
    $fileRegexMatch | add-member -type NoteProperty -Name LineNumber -value $lineNum
    $fileRegexMatch | add-member -type NoteProperty -Name Pattern -value $pattern
    $fileRegexMatch | add-member -type NoteProperty -Name Value -value $value

    return $fileRegexMatch    
}

function GetSearchPatterns
{
    $patterns = New-Object System.Collections.ArrayList 

    [void] $patterns.Add((New-SearchPattern -name 'IP' -regex "\d{2,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))    
    [void] $patterns.Add((New-SearchPattern -name 'GUID' -regex "{[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12}"))
    [void] $patterns.Add((New-SearchPattern -name 'Password' -regex "pass=|password=|key="))
    [void] $patterns.Add((New-SearchPattern -name 'Email' -regex "([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})"))
    [void] $patterns.Add((New-SearchPattern -name 'HTTPS' -regex "https.*"))   

    return $patterns
}

function GetRegexMatches
{
    param([string]$filepath, [string]$pattern)    

    return select-string -Path $filepath -Pattern $pattern -AllMatches 
}

function AnalyseFile
{
    param($patterns, [string]$filePath)

    $searchResults = New-Object System.Collections.ArrayList

    foreach($pattern in $patterns)
    {      
        $matchResults = GetRegexMatches -filepath $filePath -pattern $pattern.Regex  

        foreach($result in $matchResults)
        {
            foreach($match in $result.Matches)
             {
                $fileMatch = New-FileRegexLineMatch -filepath $filepath -pattern $pattern -lineNum $match.LineNumber -value $match.value
                [void] $searchResults.Add($fileMatch)               
            }
        }
    }

    return $searchResults
}

function WriteMatchToConsole
{
    param($matches)

    foreach($match in $matches)
    {
        Write-Host ([string]::Format("{0} found ({1}) [line {2} - {3}]", $match.pattern.name, $match.value, $match.LineNumber, $match.filePath))
    }
}

function RunScript
{
    $baseDirectory = "<Source Directory>"
    $files = Get-ChildItem -Path $baseDirectory -recurse -filter "*.cs"
    $testFiles = $files | Resolve-Path -Relative #| Select-Object -first 200 


    $searchPatterns = GetSearchPatterns

    $searchResults = New-Object System.Collections.ArrayList

    foreach($file in $testFiles)
    {
        Write-Host -NoNewLine ([string]::Format("Scanning [{0}]....",$file))

        $searchResults += AnalyseFile -filepath $file -patterns $searchPatterns  

        Write-Host "Done"      
    }

    Write-Host "Displaying Search Results:"

    WriteMatchToConsole -matches $searchResults
}

RunScript