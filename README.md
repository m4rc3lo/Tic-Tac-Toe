# Tic-Tac-Toe Console AI

```text
 _______ _        _______            _______
|__   __(_)      |__   __|          |__   __|
   | |   _  ___     | | __ _  ___     | | ___   ___
   | |  | |/ __|    | |/ _` |/ __|    | |/ _ \ / _ \
   | |  | | (__     | | (_| | (__     | | (_) |  __/
   |_|  |_|\___|    |_|\__,_|\___|    |_|\___/ \___|

              Console AI · .NET 9
```

Refatoração didática e experimental de uma implementação legada de jogo da
velha. O projeto combina domínio encapsulado, Strategies de inteligência
artificial, aplicação Console, persistência JSON, exportação CSV, modo
demonstrativo, experimentação reproduzível e publicação multiplataforma.

## Recursos

- partida pessoa contra IA;
- demonstração IA contra IA;
- Strategies `Random`, `Heuristic` e `Minimax`;
- temas ASCII e Unicode;
- cores, animações e áudio com fallback;
- configurações persistentes;
- histórico e estatísticas;
- exportação CSV;
- confrontos experimentais em lote;
- compatibilidade Windows e Unix-like;
- quatro perfis de publicação.

## Estado do projeto

| Versão | Data | Marco |
|---|---|---|
| `v1.0.0` | 2026-07-15 | legado preservado |
| `v1.1.0` | 2026-07-15 | solução .NET 9 e governança |
| `v1.2.0` | 2026-07-16 | domínio e agregado |
| `v1.3.0` | 2026-07-16 | Strategy aleatória |
| `v1.4.0` | 2026-07-16 | aplicação e heurística |
| `v1.5.0` | 2026-07-17 | Minimax |
| `v1.6.0` | 2026-07-17 | apresentação e navegação |
| `v1.7.0` | 2026-07-17 | recursos audiovisuais |
| `v1.8.0` | 2026-07-18 | persistência e CSV |
| `v1.9.0` | 2026-07-21 | experimentação, robustez e publicação candidata |
| `v2.0.0` | a definir | consolidação final |

## Estrutura

```text
src/TicTacToe.Console/      aplicação
tests/TicTacToe.Tests/      testes automatizados
docs/                       documentação técnica
distribution/               arquivos incluídos na publicação
scripts/                    validações de empacotamento
legacy/                     código legado preservado
data/                       dados criados em runtime
exports/                    exportações criadas em runtime
```

## Requisitos de desenvolvimento

- SDK .NET 9;
- Git.

## Compilar e testar

```bash
dotnet restore TicTacToe.sln
dotnet build TicTacToe.sln --configuration Release
dotnet test TicTacToe.sln --configuration Release --no-build
```

Para tratar warnings como erros:

```bash
dotnet build TicTacToe.sln --configuration Release -warnaserror
```

## Executar

```bash
dotnet run --project src/TicTacToe.Console/TicTacToe.Console.csproj
```

## Publicar

Exemplo:

```bash
dotnet publish src/TicTacToe.Console/TicTacToe.Console.csproj \
  /p:PublishProfile=linux-x64-framework-dependent
```

Consulte [`docs/26-publicacao.md`](docs/26-publicacao.md) para os quatro perfis,
validação, atualização e permissões.

## Dados e privacidade

O repositório e os pacotes não incluem histórico, estatísticas ou resultados
locais. A aplicação cria `data/` e `exports/` somente durante a execução.

Arquivos JSON corrompidos são preservados em quarentena antes da recuperação.

## Uso de IA generativa

IA generativa foi utilizada como apoio incremental à análise, geração de
patches, testes e documentação. A validação, integração e responsabilidade
permanecem humanas.

Consulte
[`docs/28-uso-ia-generativa.md`](docs/28-uso-ia-generativa.md).


## Arquivos locais no Windows

Para experimentos ou publicações com muitas substituições de arquivos, evite
gravar diretamente em pastas sincronizadas pelo Dropbox. Um diretório local
pode ser criado em `%LOCALAPPDATA%`:

```powershell
$output = Join-Path `
    $env:LOCALAPPDATA `
    "TicTacToe\experiments\reference"

New-Item -ItemType Directory -Path $output -Force
Get-ChildItem $output -Recurse
explorer.exe $output
```

Antes de remover dados, confirme o caminho:

```powershell
Resolve-Path $output
Test-Path $output
Get-ChildItem $output -Force
```

Consulte [`docs/13-resultados.md`](docs/13-resultados.md) e
[`docs/26-publicacao.md`](docs/26-publicacao.md) para os fluxos completos.

## Convenções

- identificadores em inglês;
- classes e tipos em `CamelCase`;
- métodos e variáveis em `snake_case`;
- documentação e commits em português do Brasil;
- quatro espaços;
- UTF-8 e LF;
- dependências externas mínimas.

## Documentação

| Documento | Descrição |
|---|---|
| [`docs/00-decisoes-e-escopo.md`](docs/00-decisoes-e-escopo.md) | Decisões gerais, convenções e escopo. |
| [`docs/01-projeto-original.md`](docs/01-projeto-original.md) | Inventário e contexto do código legado. |
| [`docs/02-requisitos.md`](docs/02-requisitos.md) | Requisitos funcionais, não funcionais e restrições. |
| [`docs/03-arquitetura.md`](docs/03-arquitetura.md) | Módulos, dependências e componentes. |
| [`docs/04-modelo-conceitual.md`](docs/04-modelo-conceitual.md) | Entidades, objetos de valor e relações. |
| [`docs/05-game-rules.md`](docs/05-game-rules.md) | Regras e avaliação do jogo. |
| [`docs/06-match-aggregate.md`](docs/06-match-aggregate.md) | Agregado Match e invariantes. |
| [`docs/07-fluxo-aplicacao.md`](docs/07-fluxo-aplicacao.md) | Controlador e portas da aplicação. |
| [`docs/08-revisao-prompt-10.md`](docs/08-revisao-prompt-10.md) | Revisão intermediária após o Prompt 10. |
| [`docs/09-correcao-fronteiras-arquiteturais.md`](docs/09-correcao-fronteiras-arquiteturais.md) | Correção de dependências e fronteiras. |
| [`docs/10-inteligencia-artificial.md`](docs/10-inteligencia-artificial.md) | Strategies Random, Heuristic e Minimax. |
| [`docs/11-experimentacao.md`](docs/11-experimentacao.md) | Plano formal de experimentação. |
| [`docs/12-testes.md`](docs/12-testes.md) | Estratégia, classificação e cobertura dos testes. |
| [`docs/13-resultados.md`](docs/13-resultados.md) | Protocolo, execução e análise do experimento de referência. |
| [`docs/14-limitacoes.md`](docs/14-limitacoes.md) | Compatibilidade e limitações multiplataforma. |
| [`docs/15-audio.md`](docs/15-audio.md) | Serviços de áudio e fallback. |
| [`docs/16-apresentacao-console.md`](docs/16-apresentacao-console.md) | Entrada, saída e renderização Console. |
| [`docs/17-screen-manager.md`](docs/17-screen-manager.md) | Estados e navegação. |
| [`docs/18-temas-e-creditos.md`](docs/18-temas-e-creditos.md) | Temas, ASCII, Unicode e créditos. |
| [`docs/19-feedback-visual-e-animacoes.md`](docs/19-feedback-visual-e-animacoes.md) | Animações e feedback visual. |
| [`docs/20-configuracoes-json.md`](docs/20-configuracoes-json.md) | Configuração persistente em JSON. |
| [`docs/21-partidas-e-estatisticas-json.md`](docs/21-partidas-e-estatisticas-json.md) | Histórico e estatísticas. |
| [`docs/22-exportacao-csv.md`](docs/22-exportacao-csv.md) | Esquemas e regras CSV. |
| [`docs/23-modo-automatico.md`](docs/23-modo-automatico.md) | Demonstração IA contra IA. |
| [`docs/24-modo-experimental.md`](docs/24-modo-experimental.md) | Execução experimental em lote. |
| [`docs/25-robustez-fronteiras-externas.md`](docs/25-robustez-fronteiras-externas.md) | Falhas externas e recuperação. |
| [`docs/26-publicacao.md`](docs/26-publicacao.md) | Perfis e validação de publicação. |
| [`docs/27-glossario.md`](docs/27-glossario.md) | Vocabulário geral, técnico e de domínio. |
| [`docs/28-uso-ia-generativa.md`](docs/28-uso-ia-generativa.md) | Transparência e fluxo de uso de IA generativa. |
| [`docs/29-revisao-arquitetural-final.md`](docs/29-revisao-arquitetural-final.md) | Auditoria final de dependências, composição e imutabilidade. |
| [`docs/30-revisao-legal-documental.md`](docs/30-revisao-legal-documental.md) | Revisão legal, metadados e consistência documental. |
| [`docs/31-matriz-documentacao.md`](docs/31-matriz-documentacao.md) | Relação entre documentos, versões e componentes. |
| [`docs/32-release-v1.9.0.md`](docs/32-release-v1.9.0.md) | Checklist e validação da candidata v1.9.0. |

Também há um índice dedicado em [`docs/README.md`](docs/README.md).


## Candidata v1.9.0

A versão `1.9.0` está preparada como candidata final. A tag só deve ser criada
após a execução e aprovação do checklist em
[`docs/32-release-v1.9.0.md`](docs/32-release-v1.9.0.md).

```powershell
powershell.exe `
    -NoProfile `
    -ExecutionPolicy Bypass `
    -File .\scripts
alidate-release-v1.9.0.ps1
```

## Licença e citação

O projeto usa Apache License 2.0. Consulte:

- [`LICENSE`](LICENSE);
- [`LICENSE.md`](LICENSE.md);
- [`NOTICE`](NOTICE);
- [`CITATION.cff`](CITATION.cff).
