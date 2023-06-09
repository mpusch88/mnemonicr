param(
    [string]$InputString,
    [switch]$FirstSearch,
    [switch]$EXO_On,
    [switch]$EXO_Modified,
    [string]$Username
)

function IsEmail($inputString) {
    return $inputString -match "^[\w\.-]+@[\w\.-]+\.\w+$"
}

Write-Host("Input string: $InputString")
Write-Host("First search: $FirstSearch")

if ($EXO_On) {
    Write-Host("Username: $Username")
}

if ($FirstSearch) {
    Add-PSSnapin Microsoft.Exchange.Management.PowerShell.SnapIn
    
    if($EXO_On) {
        Connect-ExchangeOnline -UserPrincipalName $Username -ShowProgress $false -ShowBanner $false
    }
}
else {
    if($EXO_Modified && !$EXO_On) {
        Disconnect-ExchangeOnline
    }
}

$entries = $InputString -split ";"
$mnems = @()

foreach ($entry in $entries) {
    
    Write-Host("Processing entry: $entry")

    $entry = $entry.Trim()

    if (IsEmail $entry) {

        Write-Host "Email: $entry"

        $mnems += Get-Mailbox $entry | Select-Object SamAccountName
    }
    else {
        $nameParts = $entry -split ","

        if ($nameParts.Length -eq 2) {
            $firstName = $nameParts[0].Trim()
            $lastName = $nameParts[1].Trim()

            Write-Host "Name: $firstName $lastName"

            $mnems += Get-Mailbox $lastName, $firstName | Select-Object SamAccountName
        }
        else {
            $noResults = @{
                Message = "No results found for $entry"
            }

            $obj = New-Object -TypeName PsObject -Property $noResults

            $mnems += $obj
        }
    }
}

$mnems | ForEach-Object {
    if ($_.Message) {
        $_.Message
    }
    else {
        $_.SamAccountName
    }
}
