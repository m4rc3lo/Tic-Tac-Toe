# Relatório de consolidação da versão 2.0.0

**Data:** 22 de julho de 2026  
**Branch preparada:** `release/v2.0.0`  
**Patch:** `0032-consolidacao-v2.0.0.patch`

## Resultado

A cópia local do projeto foi consolidada para a versão `2.0.0`. O pacote final
contém apenas arquivos rastreados ou destinados ao commit da release. Foram
excluídos do pacote versionável o ambiente virtual Python, saídas de build,
publicações locais, caches e o arquivo local `output.txt`.

Durante a auditoria também foi identificado um bytecode Python rastreado em
`scripts/__pycache__/`. O arquivo foi removido, e `.gitignore` passou a excluir
`__pycache__/`, `*.pyc`, `*.pyo` e `*.pyd` por meio do padrão `*.py[cod]`.

## Alterações principais

- versão do assembly atualizada para `2.0.0`;
- `CITATION.cff` atualizado para 22 de julho de 2026;
- changelog e timeline consolidados;
- README atualizado com release, migração e referências em ordem alfabética;
- índices documentais consolidados;
- novo checklist de release;
- notas de migração da v1.9.0;
- auditoria de arquivos, dados, privacidade e licença;
- novo validador PowerShell da v2.0.0;
- teste de consistência legal atualizado;
- ambientes virtuais e caches Python explicitamente ignorados;
- patch `0032` registrado.

## Verificações executadas

- árvore Git inicial limpa;
- coerência textual da versão entre `Directory.Build.props`, `CITATION.cff` e
  `CHANGELOG.md`;
- `git diff --check`;
- compilação sintática dos scripts Python com `py_compile`;
- existência de todos os links Markdown locais;
- presença de todos os documentos no índice;
- ausência de diretórios proibidos entre arquivos rastreados do pacote final;
- busca estática por padrões comuns de segredos;
- inspeção do ZIP final para confirmar ausência de `bin`, `obj`, `artifacts`,
  `.venv-experiments`, `output.txt`, `__pycache__` e bytecodes Python.

## Validações pendentes antes da tag

O ambiente utilizado para preparar os arquivos não possui SDK .NET nem
PowerShell. Portanto, não foram executados neste ambiente:

- restore e build;
- suíte de testes;
- cobertura;
- quatro publicações;
- teste de atualização com dados reais da v1.9.0;
- experimento curto reproduzível;
- smoke tests Windows e Linux.

Execute na máquina de desenvolvimento:

```powershell
powershell.exe `
    -NoProfile `
    -ExecutionPolicy Bypass `
    -File .\scripts\validate-release-v2.0.0.ps1
```

A tag `v2.0.0` só deve ser criada após aprovação dessas verificações.

## Checksums dos arquivos entregues

- Patch SHA-256: `9a8a1d595a68ec730d95a19c4ee2fec74562abfdcf19ce6b28b3f5dc3dc8371d`
- ZIP SHA-256: `73617bd8482107cb26595207b9a0108105a024550f74588fb7804902b8926632`
