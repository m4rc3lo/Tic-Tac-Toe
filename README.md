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
| `v2.0.0` | 2026-07-22 | arquitetura e fluxos consolidados |

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
| [`docs/32-release-v1.9.0.md`](docs/32-release-v1.9.0.md) | Checklist histórico da candidata v1.9.0. |
| [`docs/34-release-v2.0.0.md`](docs/34-release-v2.0.0.md) | Consolidação, validação e publicação da v2.0.0. |
| [`docs/35-migracao-v1.9.0-v2.0.0.md`](docs/35-migracao-v1.9.0-v2.0.0.md) | Preservação de dados e atualização da v1.9.0. |
| [`docs/36-auditoria-release-v2.0.0.md`](docs/36-auditoria-release-v2.0.0.md) | Auditoria de arquivos, legalidade, dados e privacidade. |

Também há um índice dedicado em [`docs/README.md`](docs/README.md).


## Versão 2.0.0

A versão `2.0.0` conclui o escopo funcional da refatoração e consolida domínio,
aplicação, Strategies, apresentação, persistência, exportação, modos automático
e experimental, testes, documentação e publicação multiplataforma.

Antes de criar a tag, execute o checklist completo descrito em
[`docs/34-release-v2.0.0.md`](docs/34-release-v2.0.0.md):

```powershell
powershell.exe `
    -NoProfile `
    -ExecutionPolicy Bypass `
    -File .\scripts\validate-release-v2.0.0.ps1
```

As instruções de migração estão em
[`docs/35-migracao-v1.9.0-v2.0.0.md`](docs/35-migracao-v1.9.0-v2.0.0.md).

## Licença e citação

O projeto usa Apache License 2.0. Consulte:

- [`LICENSE`](LICENSE);
- [`LICENSE.md`](LICENSE.md);
- [`NOTICE`](NOTICE);
- [`CITATION.cff`](CITATION.cff).

## Referências

As referências abaixo consolidam as obras e documentações empregadas nos
materiais técnicos e nas apresentações associadas ao projeto. A ordenação é
alfabética e a formatação procura se aproximar das normas da ABNT para os dados
disponíveis.

BASS, Len; CLEMENTS, Paul; KAZMAN, Rick. *Software architecture in practice*. 4. ed. Boston: Addison-Wesley, 2021.

BRENNAN, Karen; RESNICK, Mitchel. New frameworks for studying and assessing the development of computational thinking. In: **AMERICAN EDUCATIONAL RESEARCH ASSOCIATION ANNUAL MEETING**, 2012. Proceedings [...]. Vancouver: AERA, 2012.

CHACON, Scott; STRAUB, Ben. *Pro Git*. 2. ed. New York: Apress, 2014. Disponível em: <https://git-scm.com/book/en/v2>. Acesso em: 22 jul. 2026.

CHOOSE A LICENSE. *Choose an open source license*. Disponível em: <https://choosealicense.com/>. Acesso em: 22 jul. 2026.

CITATION FILE FORMAT. *Citation File Format*. Disponível em: <https://citation-file-format.github.io/>. Acesso em: 22 jul. 2026.

CORMEN, Thomas H. et al. *Introduction to algorithms*. Cambridge: MIT Press, 2022.

DENNING, Peter J.; TEDRE, Matti. *Computational thinking*. Cambridge: MIT Press, 2019.

FOWLER, Martin. *Patterns of enterprise application architecture*. Boston: Addison-Wesley, 2002.

FOWLER, Martin. *UML distilled: a brief guide to the standard object modeling language*. 3. ed. Boston: Addison-Wesley, 2004.

FREEMAN, Eric; ROBSON, Elisabeth. *Head first design patterns*. 2. ed. Sebastopol: O'Reilly, 2020.

FULLERTON, Tracy. *Game design workshop: a playcentric approach to creating innovative games*. 4. ed. Boca Raton: CRC Press, 2018.

GAMMA, Erich et al. *Design patterns: elements of reusable object-oriented software*. Boston: Addison-Wesley, 1994.

GIT PROJECT. *Git documentation*. Disponível em: <https://git-scm.com/>. Acesso em: 22 jul. 2026.


GIL, Antonio Carlos. *Como elaborar projetos de pesquisa*. 6. ed. São Paulo: Atlas, 2017.

GITHUB. *About CITATION files*. Disponível em: <https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/about-citation-files>. Acesso em: 22 jul. 2026.

GITHUB. *Licensing a repository*. Disponível em: <https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/licensing-a-repository>. Acesso em: 22 jul. 2026.

GOODRICH, Michael T.; TAMASSIA, Roberto; GOLDWASSER, Michael H. *Data structures and algorithms in C++*. 2. ed. Hoboken: Wiley, 2011.

GREGORY, Jason. *Game engine architecture*. 3. ed. Boca Raton: CRC Press, 2018.

JSON. *Introducing JSON*. Disponível em: <https://www.json.org/json-en.html>. Acesso em: 22 jul. 2026.

LARMAN, Craig. *Applying UML and patterns*. 3. ed. Upper Saddle River: Prentice Hall, 2004.

MARTIN, Robert C. *Clean architecture: a craftsman's guide to software structure and design*. Boston: Prentice Hall, 2017.

MICROSOFT. *.NET documentation*. Disponível em: <https://learn.microsoft.com/dotnet/>. Acesso em: 22 jul. 2026.

MICROSOFT. *C# language reference*. Disponível em: <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/>. Acesso em: 22 jul. 2026.

MICROSOFT. *PowerShell documentation*. Disponível em: <https://learn.microsoft.com/powershell/>. Acesso em: 22 jul. 2026.

NYSTROM, Robert. *Game programming patterns*. 2014. Disponível em: <https://gameprogrammingpatterns.com/>. Acesso em: 22 jul. 2026.


OPENAI. *GPT-5.6 no ChatGPT*. Disponível em: <https://help.openai.com/pt-br/articles/20001354-gpt-56-in-chatgpt>. Acesso em: 22 jul. 2026.

OPEN SOURCE INITIATIVE. *Licenses*. Disponível em: <https://opensource.org/licenses>. Acesso em: 22 jul. 2026.

PYTHON PACKAGING AUTHORITY. *Python Packaging User Guide*. Disponível em: <https://packaging.python.org/>. Acesso em: 22 jul. 2026.

PYTHON SOFTWARE FOUNDATION. *Python*. Disponível em: <https://www.python.org/>. Acesso em: 22 jul. 2026.

PYTHON SOFTWARE FOUNDATION. *venv — creation of virtual environments*. Disponível em: <https://docs.python.org/3/library/venv.html>. Acesso em: 22 jul. 2026.

RESNICK, Mitchel. *Lifelong kindergarten: cultivating creativity through projects, passion, peers, and play*. Cambridge: MIT Press, 2017.

SCHELL, Jesse. *The art of game design: a book of lenses*. 3. ed. Boca Raton: CRC Press, 2019.

SEDGEWICK, Robert; WAYNE, Kevin. *Algorithms*. 4. ed. Boston: Addison-Wesley, 2011.

SEMANTIC VERSIONING. *Versionamento Semântico 2.0.0*. Disponível em: <https://semver.org/lang/pt-BR/>. Acesso em: 22 jul. 2026.

SOFTWARE PACKAGE DATA EXCHANGE. *SPDX License List*. Disponível em: <https://spdx.org/licenses/>. Acesso em: 22 jul. 2026.

SOMMERVILLE, Ian. *Software engineering*. 10. ed. Boston: Pearson, 2016.


WAZLAWICK, Raul Sidnei. *Metodologia de pesquisa para ciência da computação*. 3. ed. Rio de Janeiro: GEN LTC, 2020.

WING, Jeannette M. Computational thinking. *Communications of the ACM*, New York, v. 49, n. 3, p. 33–35, 2006.
