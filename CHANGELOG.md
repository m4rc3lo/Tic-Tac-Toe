# Changelog

Todas as alterações relevantes deste projeto serão documentadas neste arquivo.

O projeto utiliza versionamento semântico. A versão `1.0.0` identifica o
estado legado preservado antes do início da refatoração. A série `1.x`
registrará a evolução incremental, enquanto a versão `2.0.0` representará
a conclusão da nova arquitetura.

## Linha do tempo

O diagrama distingue o histórico original, os marcos efetivamente concluídos
e as versões futuras, cujas datas ainda não foram definidas.

```mermaid
timeline
    title Evolução do Tic-Tac-Toe Console AI
    section Histórico original
        2020-11-23 : Código legado publicado no repositório
    section Marcos concluídos
        2026-07-15 : v1.0.0 — estado legado preservado
        2026-07-15 : v1.1.0 — estrutura .NET 9 e governança
        2026-07-16 : v1.2.0 — domínio, regras e agregado Match
        2026-07-16 : v1.3.0 — Strategy e estratégia aleatória
        2026-07-16 : v1.4.0 — aplicação, fronteiras e heurística
        2026-07-17 : v1.5.0 — estratégia Minimax
        2026-07-17 : v1.6.0 — apresentação e estados
        2026-07-17 : v1.7.0 — recursos audiovisuais
        2026-07-18 : v1.8.0 — persistência e exportação
    section Marcos planejados
        Data a definir : v1.9.0 — experimentação e consolidação
        Data a definir : v2.0.0 — refatoração completa
```

As versões intermediárias poderão ser ajustadas conforme a granularidade
real das implementações, sem modificar os marcos `v1.0.0` e `v2.0.0`.

## [Unreleased]

## [1.8.0] - 2026-07-18

### Added

- `ApplicationSettings` e `ApplicationDirectories` com valores padrão.
- `ISettingsRepository`, `JsonSettingsRepository` e `SettingsValidator`.
- Persistência JSON com propriedades desconhecidas toleradas e recuperação segura.
- Registros imutáveis para partidas, participantes, jogadas e sequência vencedora.
- Repositórios JSON de histórico e estatísticas.
- `MatchRecordMapper` para conversão fora do domínio.
- `MatchStatisticsCalculator` com agregação geral e por Strategy.
- `MatchPersistenceService` com tentativa de rollback do histórico.
- Infraestrutura CSV UTF-8 sem dependências externas.
- Exportadores de partidas, jogadas, estatísticas e métricas experimentais.
- Escape de separadores, aspas e quebras de linha.
- Datas ISO 8601 e números com cultura invariável.
- Testes em diretórios temporários e testes de esquema CSV.
- `docs/20-configuracoes-json.md`.
- `docs/21-partidas-e-estatisticas-json.md`.
- `docs/22-exportacao-csv.md`.

### Changed

- `Program.Main` passou a carregar `data/settings.json` antes da composição.
- `ConsoleMatchSessionRunner` passou a medir e persistir partidas concluídas.
- `MatchConfiguration` passou a aceitar semente opcional.
- Atraso configurado passou a controlar animações da apresentação.
- Versão do projeto e metadados atualizados para `1.8.0`.

## [1.7.0] - 2026-07-17

### Added

- `IAudioService` e eventos `AudioCue`.
- `ConsoleBeepAudioService` para Windows.
- `TerminalBellAudioService` para terminais Unix-like.
- `SilentAudioService` para execução silenciosa.
- `FallbackAudioService` para recuperação de falhas.
- `AudioServiceSelector` com seleção configurável e testável.
- Opção de áudio na tela de configurações.
- Testes sem dispositivo físico.
- `docs/15-audio.md`.

### Changed

- Saída da partida passou a emitir sinais de jogada, erro e resultado.
- Versão do projeto e metadados atualizados para `1.7.0`.


### Added

- `IDelayService` com implementações real e imediata.
- `IAnimationService` e `AnimationService` para texto progressivo, indicador e barra de progresso.
- `IVisualFeedbackService` e `VisualFeedbackService` para última jogada e sequência vencedora.
- `AnimatedMoveSelector` para feedback antes de decisões computacionais.
- Testes de animação e feedback sem espera real.
- `docs/19-feedback-visual-e-animacoes.md`.

### Changed

- Splash passou a utilizar texto progressivo.
- Saída de partida passou a destacar última jogada e sequência vencedora.
- Executor de partidas passou a decorar a seleção de jogadas computacionais.


### Added

- `AsciiArtCatalog` com logotipo e artes de vitória, derrota e empate.
- `ConsoleTheme` e `PresentationPreferences` para capacidades visuais configuráveis.
- Renderização Unicode e fallback ASCII do tabuleiro.
- `CreditsScreen` com metadados derivados de `CITATION.cff`.
- Testes exatos dos modos Unicode e ASCII.
- `docs/18-temas-e-creditos.md` com limitações e fallback.

### Changed

- Menu principal ampliado com a opção Créditos.
- Tela de configurações passou a alternar Unicode, ANSI, limpeza e efeitos.
- Splash e resultados passaram a respeitar o tema visual.


## [1.6.0] - 2026-07-17

### Added

- `ScreenManager` com máquina de estados centralizada e limite de transições.
- Contratos `IScreen`, `ScreenTransition`, `ScreenContext` e `IMatchSessionRunner`.
- Estados Splash, menu, configuração, partida, resultado, estatísticas, experimentos, configurações, ajuda e saída.
- Configuração de nome e Strategy antes da partida.
- Testes de transição, retorno ao menu, saída, configuração e proteção contra ciclos.
- `docs/17-screen-manager.md` com diagramas de estados e sequência.
- `ConsoleGameInput` como adaptador de `IGameInput` baseado em `TextReader`.
- `ConsoleGameOutput` como adaptador de `IGameOutput` baseado em `TextWriter`.
- `ConsoleBoardRenderer` para tabuleiro ASCII com coordenadas.
- Composição mínima de partida pessoa contra Minimax em `Program.Main`.
- Testes de parsing e renderização sem dependência do Console físico.
- `docs/16-apresentacao-console.md` com arquitetura e fluxo dos adaptadores.

### Changed

- `Program.Main` passou a compor e iniciar a máquina de estados.
- Versão do projeto e metadados de citação atualizados para `1.6.0`.

## [1.5.0] - 2026-07-17

### Added

- `SearchBoard` como representação interna e independente para estados de busca.
- `MinimaxMoveStrategy` com alternância entre maximização e minimização.
- `MinimaxAnalysis` com posição, pontuação, profundidade e estados visitados.
- Avaliação terminal que favorece vitórias rápidas e posterga derrotas.
- Desempate determinístico pela ordem de linha e coluna.
- Testes de vitória, bloqueio, jogo perfeito, isolamento e métricas.
- Documentação da árvore de busca, complexidade, limitações e otimizações.

### Changed

- `docs/10-inteligencia-artificial.md` ampliado com o algoritmo Minimax.
- Versão do projeto e metadados de citação atualizados para `1.5.0`.

## [1.4.0] - 2026-07-16

### Added

- `HeuristicMoveStrategy` com prioridades de vitória, bloqueio, centro, cantos e laterais.
- Simulação interna para hipóteses sem modificar o tabuleiro original.
- Desempates reproduzíveis por `IRandomSource`.
- Testes isolados de prioridades, validade, reprodutibilidade e preservação do estado.
- `IReadOnlyBoard` e `BoardView` para exposição segura do tabuleiro de `Match`.
- `IComputerMoveStrategyResolver` e resolvedor configurável por símbolo.
- Testes de fronteira do agregado, resolução de Strategy e propagação de falhas.
- `docs/09-correcao-fronteiras-arquiteturais.md` com decisões e diagramas.
- Revisão documental após o Prompt 10, com datas na timeline e atualização do estado do projeto.
- Camada `Application` com controlador mínimo de partidas.
- Portas `IGameInput`, `IGameOutput` e `IMoveSelector`.
- `DefaultMoveSelector` para participantes humanos e computacionais.
- Testes do fluxo completo sem Console ou interação humana.
- `docs/07-fluxo-aplicacao.md` com componentes e sequência.

### Changed

- Dependência `Domain → AI` removida de `ComputerPlayer`.
- Estratégias passaram a receber `IReadOnlyBoard`.
- `MatchController` passou a capturar apenas falhas da aplicação de jogadas.
- `docs/10-inteligencia-artificial.md` ampliado com algoritmo heurístico, pseudocódigo, complexidade e limitações.
- Versão do projeto e metadados de citação atualizados para `1.4.0`.

## [1.3.0] - 2026-07-16

### Added

- Contrato `IMoveStrategy` para algoritmos intercambiáveis de decisão.
- Contrato `IRandomSource` para geração pseudoaleatória injetável.
- `SystemRandomSource` com suporte a semente controlável.
- `RandomMoveStrategy` para seleção de casas livres.
- Associação obrigatória entre `ComputerPlayer` e uma estratégia.
- Testes de validade, delegação, intervalo e reprodutibilidade.
- `docs/10-inteligencia-artificial.md` com documentação do padrão Strategy.

### Changed

- Versão do projeto e metadados de citação atualizados para `1.3.0`.

## [1.2.0] - 2026-07-16

### Added

- `LICENSE.md` com síntese acessível das condições de licenciamento.
- `NOTICE` com origem, atribuições e observações sobre o legado.
- `CITATION.cff` com metadados para citação acadêmica do software.
- `docs/00-decisoes-e-escopo.md` com convenções, política de idioma, documentação XML, fluxo Git, dependências e versionamento.
- `docs/02-requisitos.md` com escopo, requisitos funcionais, requisitos não funcionais, restrições e critérios de aceitação.
- `docs/03-arquitetura.md` com camadas, responsabilidades, contratos e regras de dependência.
- Diagramas Mermaid de contexto, componentes, sequência e estados com interpretação textual.
- `docs/04-modelo-conceitual.md` com entidades, objetos de valor, enumerações e relações do domínio.
- Enumerações `Symbol`, `GameState` e `GameResult`.
- Objetos de valor imutáveis `BoardPosition` e `Move`.
- `Board` com armazenamento encapsulado, consulta de símbolos, casas livres, aplicação e desfazimento de jogadas.
- `GameRules` para detecção de vitória, empate e partida em andamento.
- `GameEvaluation` com resultado e sequência vencedora imutável.
- `Player`, `HumanPlayer` e `ComputerPlayer`.
- Agregado `Match` com tabuleiro, jogadores, turno, histórico, estado e resultado.
- `docs/05-game-rules.md` com diagramas e interpretação das regras.
- `docs/06-match-aggregate.md` com invariantes e diagrama de sequência.
- Testes automatizados do domínio, das regras e do agregado.

### Changed

- Licença da linha de refatoração alterada de MIT para Apache License 2.0.
- Timeline do projeto consolidada entre `v1.0.0` e `v2.0.0`.
- Política de dependências definida para privilegiar a biblioteca padrão do .NET.
- Descoberta de testes xUnit habilitada no projeto de testes.
- Versão do projeto atualizada para `1.2.0`.

## [1.1.0] - 2026-07-15

### Added

- Solução `TicTacToe.sln` configurada para .NET 9.
- Projeto Console em `src/TicTacToe.Console`.
- Projeto de testes xUnit em `tests/TicTacToe.Tests`.
- Configuração compartilhada de nullable, implicit usings, UTF-8 e documentação XML.
- `.editorconfig` com indentação de quatro espaços e proibição de tabulações.
- `.gitignore` para C#/.NET, ambientes de desenvolvimento, testes, cobertura, publicação e dados locais.
- Diretórios iniciais para documentação, dados, exportações, patches e prompts.
- Inventário técnico do projeto legado em `docs/01-projeto-original.md`.
- Registro das responsabilidades, dependências, riscos e oportunidades de reutilização.
- Classificação inicial dos arquivos para a refatoração.
- Diagrama de dependências do código legado.

### Changed

- Arquivos C# legados movidos para `legacy/`, fora da compilação da nova solução.
- `README.md` ampliado com requisitos, estrutura e comandos básicos.

## [1.0.0] - 2026-07-15

### Added

- Preservação do estado legado anterior à refatoração.

### Known limitations

- Ausência de solução e projeto .NET versionados.
- Ausência de testes automatizados.
- Forte acoplamento entre regras, fluxo e interface Console.
