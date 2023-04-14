param(
    [string]$InputString
)

function IsEmail($input) {
    return $input -match "^[\w\.-]+@[\w\.-]+\.\w+$"
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
    }
}

$mnems | ForEach-Object { $_.SamAccountName }
