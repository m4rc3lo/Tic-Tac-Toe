# Análise estatística avançada dos experimentos

## 1. Objetivo

O pipeline avançado complementa o resumo básico com medidas de posição,
dispersão, incerteza e visualização das distribuições observadas.

Ele não substitui `scripts/analyze-reference-experiment.py`. O analisador
básico continua sendo usado para validação da candidata, enquanto
`scripts/analyze-reference-experiment-advanced.py` produz os artefatos
científicos complementares.

## 2. Pipeline

Os hashes e os esquemas são validados antes de qualquer cálculo.

```mermaid
flowchart LR
    A[JSON e CSV brutos] --> B[Validação de hashes e esquemas]
    B --> C[Estatísticas descritivas]
    C --> D[Bootstrap e Wilson]
    D --> E[CSV agregado]
    D --> F[Gráficos PNG e SVG]
    E --> G[Relatório Markdown]
    F --> G
```

O fluxo impede que gráficos ou interpretações sejam produzidos sobre arquivos
incompletos ou divergentes.

## 3. Estatísticas

Para duração, jogadas e estados avaliados são calculados:

- número de observações;
- mínimo e máximo;
- percentis 5 e 95;
- primeiro e terceiro quartis;
- mediana e média;
- variância e desvio padrão amostrais;
- intervalo interquartil;
- coeficiente de variação;
- intervalo bootstrap de 95% da média;
- intervalo bootstrap de 95% da mediana.

O bootstrap usa 5.000 reamostragens e semente fixa `1900`. As proporções de
vitória, empate e falha usam intervalo de Wilson de 95%.

## 4. Gráficos

O analisador gera:

- barras empilhadas dos resultados por cenário;
- box plot da duração;
- box plot do número de jogadas;
- box plot dos estados avaliados;
- função de distribuição acumulada empírica da duração;
- dispersão entre estados avaliados e duração;
- duração pela ordem de execução;
- heatmap da taxa de vitória de X.

Cada figura é exportada em PNG e SVG.

## 5. Relatório interpretativo

O arquivo `reference-analysis.md` é produzido a partir dos resultados
observados. O texto identifica:

- cenário com maior e menor duração mediana;
- cenário com maior variabilidade relativa;
- presença de falhas;
- diferenças entre média e mediana;
- limitações de generalização.

A interpretação permanece descritiva e não afirma causalidade ou significância
estatística sem testes específicos.

## 6. Dependência

Matplotlib está isolado em `requirements-experiments.txt`. Ele não integra:

- a aplicação .NET;
- o projeto de testes C#;
- os executáveis publicados;
- os pacotes autocontidos.

## 7. Execução

```powershell
python -m pip install `
    -r .\requirements-experiments.txt

python .\scripts\analyze-reference-experiment-advanced.py `
    $output
```

Ou:

```powershell
powershell.exe `
    -NoProfile `
    -ExecutionPolicy Bypass `
    -File .\scripts\run-reference-analysis-advanced.ps1 `
    -ExperimentDirectory $output
```

## 8. Artefatos

O diretório experimental recebe:

- `reference-summary-advanced.json`;
- `reference-descriptive-statistics.csv`;
- `reference-analysis.md`;
- `figures/*.png`;
- `figures/*.svg`.

Dados brutos e conjuntos completos de figuras devem ser anexados à release.
Somente resultados pequenos, revisados e deliberadamente selecionados devem
ser versionados.
