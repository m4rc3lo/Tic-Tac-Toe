param(
    [Parameter(Mandatory = $true)]
    [string]$PublishDirectory
)

$ErrorActionPreference = "Stop"
$path = (Resolve-Path $PublishDirectory).Path

$required = @(
    "CITATION.cff",
    "README-PUBLISH.md",
    "settings.example.json"
)

$missing = @(
    $required | Where-Object {
        -not (Test-Path (Join-Path $path $_) -PathType Leaf)
    }
)

$forbiddenDirectories = @(
    "data",
    "exports"
) | Where-Object {
    Test-Path (Join-Path $path $_) -PathType Container
}

$forbiddenNames = @(
    "settings.json",
    "matches.json",
    "statistics.json",
    "experiment-result.json",
    "experiment-metrics.csv"
)

$forbiddenFiles = @(
    Get-ChildItem $path -File -Recurse |
        Where-Object { $_.Name -in $forbiddenNames } |
        ForEach-Object {
            $_.FullName.Substring($path.Length).TrimStart("\", "/")
        }
)

$sizeBytes = (
    Get-ChildItem $path -File -Recurse |
        Measure-Object -Property Length -Sum
).Sum

if ($null -eq $sizeBytes) {
    $sizeBytes = 0
}

[pscustomobject]@{
    Directory = $path
    SizeBytes = [int64]$sizeBytes
    SizeMiB = [math]::Round($sizeBytes / 1MB, 2)
    MissingFiles = $missing -join ", "
    ForbiddenEntries =
        @($forbiddenDirectories + $forbiddenFiles) -join ", "
}

if ($missing.Count -gt 0 -or
    $forbiddenDirectories.Count -gt 0 -or
    $forbiddenFiles.Count -gt 0) {
    throw "A publicação contém arquivos ausentes ou dados locais proibidos."
}
