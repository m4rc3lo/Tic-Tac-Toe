# 27. Roteiro incremental de prompts

Cada etapa deverá produzir um estado compilável, testado, documentado e pronto para integração. O branch principal deverá permanecer funcional. Os identificadores permanecerão em inglês; comentários de código, documentação e mensagens de commit serão escritos em português do Brasil.

## Prompt 01 — Inventário do projeto legado

```text
Analise o projeto legado sem alterar o código. Produza docs/01-projeto-original.md com estrutura, responsabilidades, dependências, problemas de acoplamento, riscos, oportunidades de reutilização e uma tabela classificando cada arquivo como manter, adaptar, substituir ou remover. Inclua diagrama Mermaid interpretado textualmente e registre que o estado corresponde à tag v1.0.0.
```

**Branch:** `docs/legacy-inventory`  
**Versão:** permanece `1.0.0`; registrar em `Unreleased`.

## Prompt 02 — Solução .NET 9 e `.gitignore`

```text
Crie TicTacToe.sln para .NET 9, com src/TicTacToe.Console e tests/TicTacToe.Tests usando xUnit. Configure nullable, implicit usings, UTF-8 e geração de documentação XML. Adicione .gitignore para C#/.NET, IDEs, testes, cobertura, publicação e dados locais. Crie docs, data, exports, patches e prompts. Execute restore, build e test.
```

**Branch:** `chore/dotnet-solution`  
**Versão prevista:** `1.1.0`.

## Prompt 03 — Arquivos legais e governança

```text
Revise ou crie LICENSE, LICENSE.md, NOTICE, CITATION.cff e CHANGELOG.md. Adicione timeline Mermaid de v1.0.0 até v2.0.0, com texto antes e depois. Crie docs/00-decisoes-e-escopo.md com convenções, política de idioma, comentários XML, fluxo Git e versionamento.
```

**Branch:** `docs/project-governance`.

## Prompt 04 — Requisitos e arquitetura inicial

```text
Crie docs/02-requisitos.md e docs/03-arquitetura.md com objetivo, escopo, requisitos, restrições, critérios de aceitação, camadas e dependências. Inclua diagramas Mermaid de contexto, componentes e estados, sempre interpretados textualmente.
```

**Branch:** `docs/requirements-architecture`.

## Prompt 05 — Modelo conceitual

```text
Crie docs/04-modelo-conceitual.md para Board, Move, Match, Player, HumanPlayer, ComputerPlayer, Symbol, GameState, GameResult e GameRules. Inclua diagrama conceitual. Implemente somente enumerações e objetos de valor mínimos, com comentários XML em português do Brasil e testes básicos.
```

**Branch:** `refactor/domain-foundations`.

## Prompt 06 — `Board`

```text
Implemente Board com armazenamento encapsulado, validação, aplicação e desfazimento de jogadas, casas livres e tabuleiro completo. Não implemente Console ou IA. Documente a API com comentários XML e crie testes xUnit completos.
```

**Branch:** `refactor/domain-board`.

## Prompt 07 — `GameRules`

```text
Implemente GameRules para vitória, empate e partida em andamento, retornando a sequência vencedora. Crie testes para linhas, colunas, diagonais, empates e falsos positivos. Atualize documentação e diagramas.
```

**Branch:** `refactor/game-rules`.

## Prompt 08 — `Match` e turnos

```text
Implemente Match como agregado com tabuleiro, jogadores, jogador atual, histórico, estado e resultado. Garanta alternância e encerramento corretos. Crie testes e diagrama de sequência do fluxo de jogadas. Documente invariantes com comentários XML.
```

**Branch:** `refactor/match-aggregate`  
**Versão prevista após os prompts 05 a 08:** `1.2.0`.

## Prompt 09 — Strategy e estratégia aleatória

```text
Crie IMoveStrategy e ComputerPlayer. Implemente RandomMoveStrategy com gerador injetável e semente controlável. Crie testes de validade e reprodutibilidade. Documente o padrão Strategy no código e em docs/10-inteligencia-artificial.md.
```

**Branch:** `feat/random-strategy`  
**Versão prevista:** `1.3.0`.

## Prompt 10 — Fluxo básico de aplicação

```text
Implemente controlador mínimo para coordenar partidas sem acoplar o domínio ao Console. Crie interfaces simuláveis para entrada, saída e seleção de jogadas. Garanta testes sem interação humana.
```

**Branch:** `refactor/application-flow`.

## Prompt 11 — Estratégia heurística

```text
Implemente HeuristicMoveStrategy com prioridades de vitória, bloqueio, centro, cantos, laterais e desempate aleatório. Crie testes por prioridade e documente algoritmo, pseudocódigo e diagrama de decisão.
```

**Branch:** `feat/heuristic-strategy`  
**Versão prevista:** `1.4.0`.

## Prompt 12 — Estratégia Minimax

```text
Implemente MinimaxMoveStrategy com avaliação terminal, profundidade e contagem de estados. Preserve o Board original. Crie testes de vitória, bloqueio, empate sob jogo perfeito e validade. Documente complexidade e limitações.
```

**Branch:** `feat/minimax-strategy`  
**Versão prevista:** `1.5.0`.

## Prompt 13 — Abstrações de apresentação

```text
Crie interfaces para impedir chamadas diretas ao Console na aplicação. Implemente ConsoleInput, ConsoleRenderer e BoardRenderer. Produza tela jogável simples e testes com implementações simuladas.
```

**Branch:** `feat/console-presentation`.

## Prompt 14 — `ScreenManager`

```text
Implemente ScreenManager e os estados SplashScreen, MainMenu, MatchSetup, Playing, MatchResult, Statistics, ExperimentSetup, Settings, Help e Exit. Atualize diagramas de estados e sequência.
```

**Branch:** `feat/screen-manager`  
**Versão prevista:** `1.6.0`.

## Prompt 15 — ASCII art e temas

```text
Implemente AsciiArtCatalog, ConsoleTheme e tabuleiros Unicode e ASCII. Adicione logotipo e artes de vitória, derrota e empate. Permita desativar cores e Unicode. Documente limitações de terminal.
```

**Branch:** `feat/ascii-themes`.

## Prompt 16 — Feedback visual e animações

```text
Implemente AnimationService e VisualFeedbackService com texto progressivo, indicador de análise, destaques e barra de progresso. Não use atrasos no domínio. Injete serviço de atraso para testes.
```

**Branch:** `feat/visual-feedback`.

## Prompt 17 — Áudio

```text
Crie IAudioService, ConsoleBeepAudioService, TerminalBellAudioService e SilentAudioService. Isole dependências de plataforma e trate falhas sem encerrar a aplicação. Produza docs/08-audio.md.
```

**Branch:** `feat/audio-services`  
**Versão prevista:** `1.7.0`.

## Prompt 18 — Configurações JSON

```text
Crie ApplicationSettings, ISettingsRepository e JsonSettingsRepository. Implemente valores padrão, diretórios, arquivo ausente, arquivo inválido e gravação temporária. Crie testes em diretórios temporários.
```

**Branch:** `feat/json-settings`.

## Prompt 19 — Partidas e estatísticas JSON

```text
Crie registros, interfaces e repositórios JSON para partidas, jogadas e estatísticas. Salve a partida concluída e atualize estatísticas. Crie testes e diagrama entidade-relacionamento.
```

**Branch:** `feat/json-matches`.

## Prompt 20 — Exportação CSV

```text
Implemente exportadores CSV sem biblioteca externa, em UTF-8 e com ponto e vírgula. Trate separadores, aspas e quebras de linha. Crie testes e documente esquemas.
```

**Branch:** `feat/csv-export`  
**Versão prevista:** `1.8.0`.

## Prompt 21 — Modo automático demonstrativo

```text
Implemente IA contra IA com renderização, atraso configurável e identificação das estratégias. Reutilize Match e GameRules. Permita interrupção segura e crie testes sem Console físico.
```

**Branch:** `feat/automatic-mode`.

## Prompt 22 — Modo experimental

```text
Implemente ExperimentController para confrontos em lote sem renderização, animações ou áudio. Configure estratégias, quantidade, primeiro participante e semente. Colete métricas e exporte JSON e CSV, incluindo a versão da aplicação.
```

**Branch:** `feat/experiment-mode`.

## Prompt 23 — Documentação experimental

```text
Crie docs/11-experimentacao.md com pergunta, hipóteses, variáveis, cenários, execuções, sementes, métricas, ameaças à validade e gráficos. Inclua fluxograma e diagrama de sequência Mermaid interpretados textualmente.
```

**Branch:** `docs/experiment-plan`.

## Prompt 24 — Robustez

```text
Revise entrada, arquivos, serialização, áudio e codificação. Garanta mensagens claras, fallback seguro e ausência de encerramentos inesperados. Não use exceções para fluxo normal. Crie testes de falha controlada.
```

**Branch:** `fix/external-boundaries`.

## Prompt 25 — Compatibilidade

```text
Teste e documente Windows e pelo menos um sistema Unix-like. Verifique Unicode, ANSI, Console.Beep, terminal bell, caminhos e separadores. Adicione modo de compatibilidade e atualize docs/14-limitacoes.md.
```

**Branch:** `test/cross-platform`.

## Prompt 26 — Publicação

```text
Configure dotnet publish para win-x64 e linux-x64, dependente do framework e autocontido. Documente dependências e execução. Não versione binários e confirme o .gitignore.
```

**Branch:** `chore/publish-config`.

## Prompt 27 — Revisão arquitetural

```text
Revise acoplamento, responsabilidades, dependências circulares, duplicação e acessos indevidos. Atualize diagramas para refletir o código real e melhore comentários XML quando necessário.
```

**Branch:** `refactor/architecture-review`.

## Prompt 28 — Cobertura e qualidade dos testes

```text
Execute toda a suíte, identifique lacunas e adicione testes para domínio, IA, aplicação e persistência. Evite testes frágeis. Documente estratégia e limitações em docs/12-testes.md.
```

**Branch:** `test/coverage-review`.

## Prompt 29 — Experimento de referência

```text
Execute experimento com todas as combinações de estratégias, alternando X e O e registrando sementes. Exporte JSON e CSV. Produza docs/13-resultados.md com tabelas, interpretação e limitações.
```

**Branch:** `experiment/reference-run`.

## Prompt 30 — Revisão legal e documental

```text
Revise LICENSE, LICENSE.md, NOTICE, CITATION.cff, README.md, CHANGELOG.md e toda a documentação. Confirme dependências, atribuições, modificações sobre o legado, política de idioma e documentação XML.
```

**Branch:** `docs/final-review`.

## Prompt 31 — Preparação da versão `v1.9.0`

```text
Prepare a versão 1.9.0 como candidata final. Atualize versão, CHANGELOG.md, CITATION.cff e README.md. Execute restore, build, test, formatação e publicações de validação. Confirme ausência de dados temporários, pessoais, binários, segredos e artefatos locais.
```

**Branch:** `release/v1.9.0`.

## Prompt 32 — Consolidação da versão `v2.0.0`

```text
Prepare a versão 2.0.0 como conclusão da refatoração. Atualize versão, CHANGELOG.md, CITATION.cff, README.md e documentação. Execute toda a validação e um experimento curto. Produza instruções para criar a tag anotada v2.0.0 e o release no GitHub.
```

**Branch:** `release/v2.0.0`.

---