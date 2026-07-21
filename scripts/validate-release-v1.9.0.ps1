param(
    [string]$OutputRoot = (
        Join-Path `
            $env:LOCALAPPDATA `
            "TicTacToe\release\v1.9.0"
    ),
    [switch]$SkipPublish
)

$ErrorActionPreference = "Stop"
$utf8_no_bom = New-Object System.Text.UTF8Encoding($false)
[Console]::InputEncoding = $utf8_no_bom
[Console]::OutputEncoding = $utf8_no_bom
$OutputEncoding = $utf8_no_bom

if ($env:OS -eq "Windows_NT") {
    & chcp.com 65001 > $null
}
$projectRoot = (
    Resolve-Path (
        Join-Path $PSScriptRoot ".."
    )
).Path
$solution = Join-Path $projectRoot "TicTacToe.sln"
$project = Join-Path `
    $projectRoot `
    "src\TicTacToe.Console\TicTacToe.Console.csproj"
$coverageDirectory = Join-Path $OutputRoot "coverage"
$experimentDirectory = Join-Path $OutputRoot "experiment-short"
$publishDirectory = Join-Path $projectRoot "artifacts\publish"
$reportPath = Join-Path $OutputRoot "release-validation.json"

function Invoke-Checked {
    param(
        [Parameter(Mandatory = $true)]
        [scriptblock]$Command,
        [Parameter(Mandatory = $true)]
        [string]$Description
    )

    Write-Host ""
    Write-Host "==> $Description"

    & $Command

    if ($LASTEXITCODE -ne 0) {
        throw "$Description falhou com código $LASTEXITCODE."
    }
}

function Assert-NoTrackedLocalData {
    $tracked = @(
        git -C $projectRoot ls-files
    )
    $forbidden = @(
        $tracked | Where-Object {
            $_ -match '(^|/)(bin|obj|artifacts|TestResults|coverage|publish)/' -or
            $_ -match '(^|/)data/.*\.json$' -or
            $_ -match '(^|/)exports/.*\.csv$' -or
            $_ -match '\.(user|suo|nupkg|snupkg|trx|coverage)$'
        }
    )

    if ($forbidden.Count -gt 0) {
        throw (
            "Arquivos locais ou binários rastreados: " +
            ($forbidden -join ", ")
        )
    }
}

function Assert-NoSensitiveContent {
    # URLs públicas, autoria e caminhos fictícios de teste não são dados
    # locais. A auditoria procura somente o perfil e a raiz reais desta máquina.
    $excludedPaths = @(
        "README.md",
        "docs/13-resultados.md",
        "docs/26-publicacao.md",
        "scripts/validate-release-v1.9.0.ps1"
    )
    $textExtensions = @(
        ".cs",
        ".csproj",
        ".props",
        ".targets",
        ".json",
        ".md",
        ".cff",
        ".txt",
        ".csv",
        ".xml",
        ".ps1",
        ".sh",
        ".yml",
        ".yaml",
        ".gitignore",
        ".gitattributes"
    )
    $userProfile = [Environment]::GetFolderPath(
        [Environment+SpecialFolder]::UserProfile
    )
    $patterns = @(
        [pscustomobject]@{
            Name = "caminho pessoal real"
            Regex = [regex]::new(
                [regex]::Escape($userProfile),
                [Text.RegularExpressions.RegexOptions]::IgnoreCase
            )
        },
        [pscustomobject]@{
            Name = "caminho absoluto do repositório local"
            Regex = [regex]::new(
                [regex]::Escape($projectRoot),
                [Text.RegularExpressions.RegexOptions]::IgnoreCase
            )
        },
        [pscustomobject]@{
            Name = "identificador de chave AWS"
            Regex = [regex]::new(
                'AKIA[0-9A-Z]{16}',
                [Text.RegularExpressions.RegexOptions]::None
            )
        },
        [pscustomobject]@{
            Name = "chave privada"
            Regex = [regex]::new(
                'BEGIN (RSA |EC |OPENSSH )?PRIVATE KEY',
                [Text.RegularExpressions.RegexOptions]::IgnoreCase
            )
        },
        [pscustomobject]@{
            Name = "segredo atribuído em texto"
            Regex = [regex]::new(
                '(password|passwd|secret|token|api[_-]?key)\s*[:=]\s*[^\s<>{}]+',
                [Text.RegularExpressions.RegexOptions]::IgnoreCase
            )
        }
    )
    $findings = [System.Collections.Generic.List[string]]::new()
    $trackedFiles = @(
        git -C $projectRoot ls-files
    )

    foreach ($relativePath in $trackedFiles) {
        $normalizedPath = $relativePath.Replace("\", "/")

        if ($excludedPaths -contains $normalizedPath) {
            continue
        }

        $extension = [IO.Path]::GetExtension($normalizedPath).ToLowerInvariant()
        $fileName = [IO.Path]::GetFileName($normalizedPath).ToLowerInvariant()

        if (
            $textExtensions -notcontains $extension -and
            $textExtensions -notcontains $fileName
        ) {
            continue
        }

        $absolutePath = Join-Path $projectRoot $relativePath

        try {
            $lineNumber = 0

            foreach ($line in Get-Content -LiteralPath $absolutePath) {
                $lineNumber++

                foreach ($pattern in $patterns) {
                    if ($pattern.Regex.IsMatch($line)) {
                        $findings.Add(
                            "$normalizedPath`:$lineNumber " +
                            "[$($pattern.Name)]"
                        )
                    }
                }
            }
        }
        catch {
            throw (
                "Não foi possível auditar o arquivo " +
                "'$normalizedPath': $($_.Exception.Message)"
            )
        }
    }

    if ($findings.Count -gt 0) {
        throw (
            "Conteúdo potencialmente sensível encontrado:`n" +
            ($findings -join "`n")
        )
    }
}

function Assert-VersionConsistency {
    [xml]$props = Get-Content (
        Join-Path $projectRoot "Directory.Build.props"
    )
    $version = [string]$props.Project.PropertyGroup.Version
    $citation = Get-Content (
        Join-Path $projectRoot "CITATION.cff"
    )

    if ($version -ne "1.9.0") {
        throw "Directory.Build.props não contém a versão 1.9.0."
    }

    if ($citation -notcontains 'version: "1.9.0"') {
        throw "CITATION.cff não contém a versão 1.9.0."
    }
}

Remove-Item `
    $OutputRoot `
    -Recurse `
    -Force `
    -ErrorAction SilentlyContinue
New-Item `
    -ItemType Directory `
    -Path $OutputRoot `
    -Force |
    Out-Null

Push-Location $projectRoot

try {
    Assert-VersionConsistency
    Assert-NoTrackedLocalData
    Assert-NoSensitiveContent

    Invoke-Checked `
        { git diff --check } `
        "Verificação de whitespace"

    Invoke-Checked `
        { dotnet restore $solution } `
        "Restore"

    Invoke-Checked `
        {
            dotnet build `
                $solution `
                --configuration Release `
                --no-restore `
                -warnaserror
        } `
        "Build Release sem warnings"

    Invoke-Checked `
        {
            dotnet test `
                $solution `
                --configuration Release `
                --no-build
        } `
        "Suíte completa"

    Remove-Item `
        $coverageDirectory `
        -Recurse `
        -Force `
        -ErrorAction SilentlyContinue

    Invoke-Checked `
        {
            dotnet test `
                $solution `
                --configuration Release `
                --no-build `
                --collect "XPlat Code Coverage" `
                --settings (
                    Join-Path `
                        $projectRoot `
                        "tests\coverage.runsettings"
                ) `
                --results-directory $coverageDirectory
        } `
        "Relatório de cobertura"

    Invoke-Checked `
        {
            dotnet test `
                (
                    Join-Path `
                        $projectRoot `
                        "tests\TicTacToe.Tests\TicTacToe.Tests.csproj"
                ) `
                --configuration Release `
                --no-build `
                --filter "FullyQualifiedName~AutomaticMatchRunnerTests"
        } `
        "Execução curta do runner automático"

    $commit = (
        git -C $projectRoot rev-parse HEAD
    ).Trim()

    Remove-Item `
        $experimentDirectory `
        -Recurse `
        -Force `
        -ErrorAction SilentlyContinue

    Invoke-Checked `
        {
            dotnet run `
                --project $project `
                --configuration Release `
                --no-build `
                -- `
                --reference-experiment `
                --commit $commit `
                --output $experimentDirectory `
                --repetitions 2 `
                --base-seed 1900
        } `
        "Experimento curto reproduzível"

    Invoke-Checked `
        {
            python `
                (
                    Join-Path `
                        $projectRoot `
                        "scripts\analyze-reference-experiment.py"
                ) `
                $experimentDirectory
        } `
        "Validação dos JSON e CSV experimentais"

    if (-not $SkipPublish) {
        $profiles = @(
            "win-x64-framework-dependent",
            "win-x64-self-contained",
            "linux-x64-framework-dependent",
            "linux-x64-self-contained"
        )

        foreach ($profile in $profiles) {
            Invoke-Checked `
                {
                    dotnet publish `
                        $project `
                        /p:PublishProfile=$profile
                } `
                "Publicação $profile"

            & (
                Join-Path `
                    $projectRoot `
                    "scripts\validate-publish.ps1"
            ) (
                Join-Path `
                    $publishDirectory `
                    $profile
            )
        }
    }

    $ignored = @(
        git -C $projectRoot status --ignored --short
    )
    $coverageReport = Get-ChildItem `
        $coverageDirectory `
        -Filter "coverage.cobertura.xml" `
        -File `
        -Recurse |
        Select-Object -First 1

    if ($null -eq $coverageReport) {
        throw "Relatório de cobertura não encontrado."
    }

    [xml]$coverage = Get-Content $coverageReport.FullName
    $manifestPath = Join-Path `
        $experimentDirectory `
        "reference-manifest.json"
    $summaryPath = Join-Path `
        $experimentDirectory `
        "reference-summary.json"
    $svgPath = Join-Path `
        $experimentDirectory `
        "reference-results.svg"

    foreach ($requiredPath in @(
        $manifestPath,
        $summaryPath,
        $svgPath
    )) {
        if (-not (Test-Path $requiredPath -PathType Leaf)) {
            throw "Arquivo esperado ausente: $requiredPath"
        }
    }

    $report = [ordered]@{
        version = "1.9.0"
        candidateDate = "2026-07-21"
        commit = $commit
        branch = (
            git -C $projectRoot branch --show-current
        ).Trim()
        dotnet = (
            dotnet --version
        ).Trim()
        coverage = [ordered]@{
            report = $coverageReport.FullName
            lineRate = $coverage.coverage.'line-rate'
            branchRate = $coverage.coverage.'branch-rate'
            linesCovered = $coverage.coverage.'lines-covered'
            linesValid = $coverage.coverage.'lines-valid'
            branchesCovered = $coverage.coverage.'branches-covered'
            branchesValid = $coverage.coverage.'branches-valid'
        }
        experiment = [ordered]@{
            directory = $experimentDirectory
            repetitionsPerScenario = 2
            baseSeed = 1900
            manifestSha256 = (
                Get-FileHash `
                    $manifestPath `
                    -Algorithm SHA256
            ).Hash.ToLowerInvariant()
        }
        publishSkipped = [bool]$SkipPublish
        ignoredEntries = $ignored
        completedAt = (
            Get-Date
        ).ToUniversalTime().ToString("O")
    }

    $reportJson = $report |
        ConvertTo-Json -Depth 8

    [IO.File]::WriteAllText(
        $reportPath,
        $reportJson,
        $utf8_no_bom
    )

    Write-Host ""
    Write-Host "Candidata v1.9.0 validada."
    Write-Host "Relatório: $reportPath"
}
finally {
    Pop-Location
}
