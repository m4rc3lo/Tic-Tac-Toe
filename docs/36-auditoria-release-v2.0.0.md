# Auditoria da release v2.0.0

## 1. Escopo

Este documento registra a auditoria da cópia local recebida em 22 de julho de
2026 e separa o conteúdo versionado dos resíduos locais presentes no arquivo
compactado.

## 2. Estado inicial

- branch recebida: `main`;
- working tree Git: limpa;
- último commit observado: registro dos prompts e patches;
- diretórios locais de build presentes: `bin/`, `obj/` e `artifacts/`;
- ambiente virtual local presente: `.venv-experiments/`;
- arquivo local volumoso presente: `output.txt`;
- um bytecode Python em `scripts/__pycache__/` estava rastreado e foi removido;
- esses itens estavam ignorados, foram removidos do rastreamento ou passaram a ser explicitamente ignorados.

## 3. Arquivos que não devem ser versionados

| Categoria | Exemplos observados | Tratamento |
|---|---|---|
| build .NET | `bin/`, `obj/` | ignorar e regenerar |
| publicação | `artifacts/` | anexar à release, não ao Git |
| ambiente Python | `.venv-experiments/` | ignorar e recriar por requirements |
| cache Python | `__pycache__/`, `*.pyc` | remover e regenerar automaticamente |
| saída local | `output.txt` | ignorar ou arquivar fora do Git |
| dados de execução | `data/*.json`, `exports/*.csv` | manter locais |

A presença no ZIP local não implica rastreamento no Git. O pacote versionável
final exclui esses itens.

## 4. Auditoria funcional e documental

A versão foi atualizada em:

- `Directory.Build.props`;
- `CITATION.cff`;
- `CHANGELOG.md`;
- README e índices;
- teste de consistência legal;
- validador de release.

Foram criados documentos de consolidação, migração e auditoria.

## 5. Auditoria legal

- licença principal: Apache License 2.0;
- `LICENSE`, `LICENSE.md` e `NOTICE` preservados;
- `CITATION.cff` atualizado para `2.0.0`;
- dependências de teste permanecem separadas do projeto de produção;
- referências acadêmicas e técnicas foram consolidadas no README.

## 6. Dados e privacidade

Não devem integrar commit ou pacote público:

- caminhos pessoais;
- tokens e chaves;
- dados pessoais;
- histórico local não curado;
- resultados brutos não selecionados;
- diretórios sincronizados ou temporários.

Resultados de referência podem ser anexados opcionalmente à GitHub Release,
acompanhados de manifesto, hashes, protocolo e versão.

## 7. Limitação da auditoria executada neste ambiente

O ambiente usado para preparar este patch não possui SDK .NET nem PowerShell.
Consequentemente, build, testes, cobertura, publicação e execução dos scripts
PowerShell não foram realizados aqui. Eles permanecem obrigatórios antes da
tag e devem ser executados na máquina de desenvolvimento conforme
`34-release-v2.0.0.md`.

As verificações estáticas realizadas incluem:

- leitura do estado Git;
- inventário de arquivos;
- coerência textual de versões;
- índice documental;
- links locais Markdown;
- auditoria de resíduos locais;
- geração do patch e do pacote versionável.
