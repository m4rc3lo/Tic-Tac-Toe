param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$results = Join-Path $PSScriptRoot "..\artifacts\coverage"

if (Test-Path $results) {
    Remove-Item $results -Recurse -Force
}

New-Item -ItemType Directory -Path $results -Force | Out-Null

dotnet test `
    (Join-Path $PSScriptRoot "..\TicTacToe.sln") `
    --configuration $Configuration `
    --collect "XPlat Code Coverage" `
    --settings (Join-Path $PSScriptRoot "..\tests\coverage.runsettings") `
    --results-directory $results

$report = Get-ChildItem `
    $results `
    -Filter "coverage.cobertura.xml" `
    -File `
    -Recurse |
    Select-Object -First 1

if ($null -eq $report) {
    throw "O relatório coverage.cobertura.xml não foi encontrado."
}

[xml]$coverage = Get-Content $report.FullName
$root = $coverage.coverage

[pscustomobject]@{
    Report = $report.FullName
    LineRate = [double]::Parse(
        $root.'line-rate',
        [Globalization.CultureInfo]::InvariantCulture)
    BranchRate = [double]::Parse(
        $root.'branch-rate',
        [Globalization.CultureInfo]::InvariantCulture)
    LinesCovered = [int]$root.'lines-covered'
    LinesValid = [int]$root.'lines-valid'
    BranchesCovered = [int]$root.'branches-covered'
    BranchesValid = [int]$root.'branches-valid'
}
