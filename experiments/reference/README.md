# Experimento de referência

Este diretório versiona somente o protocolo pequeno e reproduzível. Os dados
brutos completos, o manifesto com hashes e os gráficos gerados devem ser
anexados à release correspondente.

Execução:

```powershell
dotnet run --project .\src\TicTacToe.Console --configuration Release -- \
  --reference-experiment \
  --commit (git rev-parse HEAD) \
  --output .\artifacts\experiments\reference
```

Depois, execute `scripts/analyze-reference-experiment.py` para validar os
esquemas e gerar resumo, tabelas e gráficos SVG.
