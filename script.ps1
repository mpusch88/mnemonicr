param(
    [string]$InputString
)

function IsEmail($inputString) {
    return $inputString -match "^[\w\.-]+@[\w\.-]+\.\w+$"
}

foreach ($arg in $args) {
    if($arg -eq "-FirstSearch") {
        Add-PSSnapin Microsoft.Exchange.Management.PowerShell.SnapIn
    }
}

$entries = $InputString -split ";"
$mnems = @()

foreach ($entry in $entries) {
    $entry = $entry.Trim()

    if (IsEmail $entry) {
        $mnems += Get-ADUser -Filter "EmailAddress -eq '$entry'" -Properties EmailAddress, SamAccountName | Select-Object SamAccountName
    } else {
        $nameParts = $entry -split ","

        if ($nameParts.Length -eq 2) {
            $firstName = $nameParts[0].Trim()
            $lastName = $nameParts[1].Trim()
            $mnems += Get-ADUser -Filter "GivenName -eq '$firstName' -and Surname -eq '$lastName'" -Properties GivenName, Surname, SamAccountName | Select-Object SamAccountName
        }
        else {
            Write-Host "No results found"
        }
    }
}

$mnems | ForEach-Object { $_.SamAccountName }
