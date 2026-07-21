param(
    [Parameter(Mandatory = $true)]
    [string]$ExperimentDirectory
)

$ErrorActionPreference = "Stop"
$project_root = (
    Resolve-Path (
        Join-Path $PSScriptRoot ".."
    )
).Path
$requirements = Join-Path `
    $project_root `
    "requirements-experiments.txt"
$analysis_script = Join-Path `
    $project_root `
    "scripts\analyze-reference-experiment-advanced.py"

python -m pip install `
    --requirement $requirements

if ($LASTEXITCODE -ne 0) {
    throw "Não foi possível instalar as dependências experimentais."
}

python $analysis_script $ExperimentDirectory

if ($LASTEXITCODE -ne 0) {
    throw "A análise experimental avançada falhou."
}
