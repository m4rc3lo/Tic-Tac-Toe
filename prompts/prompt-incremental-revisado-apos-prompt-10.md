# Roteiro incremental revisado após o Prompt 10

## 1. Convenção de numeração dos artefatos

A numeração dos arquivos de patch acompanha exclusivamente os prompts incrementais funcionais.

- Prompt 01 → `0001-...`
- Prompt 02 → `0002-...`
- ...
- Prompt 10 → `0010-...`
- Prompt 11 → `0012-...`

O número `0011` já foi utilizado durante o ciclo de revisão documental. A partir deste ponto, o próximo artefato incremental será nomeado com o prefixo `0012`, ainda que corresponda ao Prompt 11.

Revisões intermediárias, auditorias, correções emergenciais e análises não devem utilizar a sequência numérica dos prompts. Esses arquivos deverão usar nomes descritivos, por exemplo:

```text
revisao-documentacao-pos-prompt-10.patch
correcao-fronteiras-arquiteturais.patch
auditoria-arquitetura-pos-prompt-20.md
```

## 2. Estado atual consolidado

Os Prompts 01 a 10 foram implementados e integrados. Depois deles, foram realizadas duas etapas intermediárias:

1. revisão detalhada do código e da documentação;
2. correção das fronteiras arquiteturais.

O estado atual inclui:

- solução .NET 9;
- domínio com `Board`, `Move`, `Match`, `GameRules` e objetos de valor;
- visão somente para leitura de tabuleiro por `IReadOnlyBoard`;
- `Match` como limite de consistência;
- `Player`, `HumanPlayer` e `ComputerPlayer`;
- padrão Strategy sem dependência `Domain → AI`;
- `RandomMoveStrategy` com fonte pseudoaleatória injetável;
- resolução de estratégias na camada `Application`;
- fluxo básico de aplicação sem dependência de Console;
- portas simuláveis de entrada, saída e seleção;
- testes automatizados sem interação humana;
- documentação revisada e timeline com datas confirmadas;
- versão pública atual `1.3.0`;
- alterações posteriores registradas em `Unreleased`.

## 3. Situação dos prompts concluídos

| Prompt | Situação | Versão associada |
|---|---|---|
| 01 — Inventário legado | Concluído | `1.0.0` / Unreleased |
| 02 — Solução .NET 9 | Concluído | `1.1.0` |
| 03 — Governança | Concluído | `1.1.0` |
| 04 — Requisitos e arquitetura | Concluído | `1.1.0` |
| 05 — Modelo conceitual | Concluído | `1.2.0` |
| 06 — Board | Concluído | `1.2.0` |
| 07 — GameRules | Concluído | `1.2.0` |
| 08 — Match | Concluído | `1.2.0` |
| 09 — Strategy aleatória | Concluído | `1.3.0` |
| 10 — Fluxo básico de aplicação | Concluído | `Unreleased` após `1.3.0` |

## 4. Ajustes arquiteturais incorporados ao roteiro

Os próximos prompts devem respeitar as seguintes decisões:

1. `Domain` não depende de `AI`;
2. `ComputerPlayer` não armazena diretamente uma Strategy;
3. a associação entre participante computacional e Strategy pertence à camada `Application`;
4. Strategies recebem `IReadOnlyBoard`;
5. `Match` mantém o `Board` mutável como detalhe privado;
6. simulações de IA não podem modificar o tabuleiro original;
7. falhas de saída não devem ser tratadas como jogadas inválidas;
8. interfaces já existentes não devem ser recriadas em prompts posteriores;
9. documentos futuros devem usar nomes ainda não ocupados;
10. toda etapa deve terminar compilável, testada, documentada e pronta para merge.

---

# Prompts pendentes revisados

## Prompt 11 — Estratégia heurística

```text
Implemente HeuristicMoveStrategy no módulo AI, respeitando IMoveStrategy e recebendo IReadOnlyBoard. Use as prioridades, nesta ordem: vitória imediata, bloqueio de vitória adversária, centro, cantos, laterais e desempate pseudoaleatório.

A estratégia não deve modificar o tabuleiro recebido. Para avaliar jogadas hipotéticas, utilize uma representação de simulação interna ou cópia controlada, sem expor mutações de Match.Board.

Reutilize IRandomSource para desempates reproduzíveis. Crie testes isolados para cada prioridade, para desempates com semente controlada, para validade da posição retornada e para preservação do tabuleiro original.

Atualize docs/10-inteligencia-artificial.md com algoritmo, pseudocódigo, complexidade, limitações e diagrama Mermaid de decisão, sempre com parágrafo antes e depois.
```

**Branch:** `feat/heuristic-strategy`  
**Versão prevista:** `1.4.0`  
**Prefixo do patch:** `0012`

### Observação

Não modificar `ComputerPlayer` para armazenar Strategy. A associação continuará sendo feita por `IComputerMoveStrategyResolver` na camada `Application`.

---

## Prompt 12 — Estratégia Minimax

```text
Implemente MinimaxMoveStrategy no módulo AI, respeitando IMoveStrategy e recebendo IReadOnlyBoard.

Crie uma representação de estado de busca independente do Board pertencente a Match. A busca não deve aplicar nem desfazer jogadas no tabuleiro original.

Implemente:
- avaliação terminal;
- alternância entre maximização e minimização;
- profundidade;
- escolha determinística quando houver empate;
- contagem de estados visitados;
- resultado da análise separado da entidade Move.

Crie testes para:
- vitória imediata;
- bloqueio obrigatório;
- empate sob jogo perfeito;
- validade da posição;
- preservação integral do tabuleiro original;
- reprodutibilidade;
- contagem de estados maior que zero;
- tabuleiro sem posições disponíveis.

Documente o algoritmo, árvore de busca, função de avaliação, complexidade temporal e espacial, limitações e possíveis otimizações em docs/10-inteligencia-artificial.md.
```

**Branch:** `feat/minimax-strategy`  
**Versão prevista:** `1.5.0`

### Recomendação

Introduzir um tipo como `SearchBoard`, `BoardSnapshot` ou equivalente, interno ao módulo AI. Não reabrir mutação pública em `Match.Board`.

---

## Prompt 13 — Implementações de Console

```text
Implemente as adaptações concretas de Console para as portas já existentes da camada Application.

Crie:
- ConsoleGameInput implementando IGameInput;
- ConsoleGameOutput implementando IGameOutput;
- ConsoleBoardRenderer para exibir IReadOnlyBoard;
- composição mínima no ponto de entrada da aplicação.

Não recrie IGameInput, IGameOutput ou IMoveSelector.

A entrada deve:
- aceitar coordenadas válidas;
- rejeitar texto inválido sem encerrar o programa;
- permitir nova tentativa;
- não usar exceções para fluxo normal.

A saída deve:
- apresentar tabuleiro;
- jogador atual;
- jogadas inválidas;
- resultado final.

Crie testes para parsing e renderização usando TextReader e TextWriter, sem depender de Console físico.
```

**Branch:** `feat/console-presentation`

### Alteração em relação ao roteiro original

O prompt original dizia para criar interfaces de apresentação. Essas interfaces já existem desde o Prompt 10. Este prompt deve somente criar adaptadores concretos.

---

## Prompt 14 — ScreenManager e estados de navegação

```text
Implemente ScreenManager e uma máquina de estados de apresentação com:
- SplashScreen;
- MainMenu;
- MatchSetup;
- Playing;
- MatchResult;
- Statistics;
- ExperimentSetup;
- Settings;
- Help;
- Exit.

Os estados de tela devem coordenar a camada Application, sem conter regras de domínio.

Defina contratos mínimos para telas e transições. Evite referências diretas entre todas as telas. Centralize a navegação no ScreenManager.

Crie testes para:
- transições válidas;
- retorno ao menu;
- saída;
- configuração de partida;
- ausência de loops infinitos em implementações simuladas.

Atualize diagramas de estados e sequência.
```

**Branch:** `feat/screen-manager`  
**Versão prevista:** `1.6.0`

---

## Prompt 15 — ASCII art, Unicode e temas

```text
Implemente:
- AsciiArtCatalog;
- ConsoleTheme;
- renderização de tabuleiro Unicode;
- renderização alternativa somente ASCII;
- logotipo;
- artes de vitória, derrota e empate.

Permita desativar:
- cores ANSI;
- Unicode;
- limpeza de tela;
- efeitos visuais.

As preferências devem ser fornecidas à apresentação por configuração, sem dependência no domínio.

Crie testes de saída textual exata para os modos Unicode e ASCII. Documente limitações de terminal e fallback.
```

**Branch:** `feat/ascii-themes`

---

## Prompt 16 — Feedback visual e animações

```text
Implemente:
- IAnimationService;
- AnimationService;
- IVisualFeedbackService;
- VisualFeedbackService;
- IDelayService.

Inclua:
- texto progressivo;
- indicador de análise da IA;
- destaque da última jogada;
- destaque da sequência vencedora;
- barra de progresso.

Nenhum atraso deve existir em Domain, AI ou MatchController. Serviços de atraso devem ser injetáveis e substituíveis por implementação imediata nos testes.

Crie testes sem espera real.
```

**Branch:** `feat/visual-feedback`

---

## Prompt 17 — Áudio

```text
Crie:
- IAudioService;
- ConsoleBeepAudioService;
- TerminalBellAudioService;
- SilentAudioService;
- serviço de seleção com fallback.

Isole dependências de plataforma. Falhas de áudio não devem encerrar a aplicação.

Documente:
- capacidades por plataforma;
- fallback;
- configuração;
- limitações;
- testes possíveis sem dispositivo de áudio.

Produza docs/15-audio.md.
```

**Branch:** `feat/audio-services`  
**Versão prevista:** `1.7.0`

### Alteração de nome documental

`docs/08-audio.md` não deve ser usado, pois a numeração `08` já foi ocupada pela revisão após o Prompt 10. O novo nome será `docs/15-audio.md`.

---

## Prompt 18 — Configurações JSON

```text
Crie:
- ApplicationSettings;
- ISettingsRepository;
- JsonSettingsRepository;
- SettingsValidator.

Inclua configurações para:
- Unicode;
- cores;
- animações;
- áudio;
- atraso;
- diretórios;
- estratégia padrão;
- semente opcional.

Implemente:
- valores padrão;
- criação de diretório;
- arquivo ausente;
- arquivo inválido;
- propriedades desconhecidas;
- gravação temporária seguida de substituição;
- recuperação segura.

Crie testes usando diretórios temporários.
```

**Branch:** `feat/json-settings`

---

## Prompt 19 — Partidas e estatísticas JSON

```text
Crie registros imutáveis para persistência de:
- partida;
- participantes;
- estratégias;
- jogadas;
- resultado;
- sequência vencedora;
- duração;
- semente;
- versão da aplicação.

Crie interfaces e repositórios JSON para:
- histórico de partidas;
- estatísticas agregadas.

A conversão entre Domain e registros persistentes deve ocorrer fora do domínio.

Salve partidas concluídas e atualize estatísticas de forma consistente. Crie testes em diretórios temporários e diagrama entidade-relacionamento.
```

**Branch:** `feat/json-matches`

---

## Prompt 20 — Exportação CSV

```text
Implemente exportadores CSV sem biblioteca externa.

Use:
- UTF-8;
- ponto e vírgula;
- cabeçalho;
- escape de separadores;
- escape de aspas;
- suporte a quebras de linha;
- representação estável de datas e números.

Exporte partidas, jogadas, estatísticas e métricas experimentais.

Crie testes de esquema e casos especiais. Documente cada coluna.
```

**Branch:** `feat/csv-export`  
**Versão prevista:** `1.8.0`

---

## Prompt 21 — Modo automático demonstrativo

```text
Implemente IA contra IA em modo demonstrativo, reutilizando Match, MatchController, IComputerMoveStrategyResolver e as Strategies existentes.

Inclua:
- renderização;
- atraso configurável;
- identificação das estratégias;
- semente;
- interrupção segura;
- retorno ao menu.

Não implemente lógica paralela de partida.

Crie testes com saída simulada, atraso imediato e cancelamento controlado, sem Console físico.
```

**Branch:** `feat/automatic-mode`

---

## Prompt 22 — Modo experimental

```text
Implemente ExperimentController para confrontos em lote.

O modo experimental não deve utilizar:
- renderização;
- animações;
- áudio;
- atrasos.

Configure:
- Strategy de X;
- Strategy de O;
- quantidade de partidas;
- alternância do primeiro participante;
- semente base;
- sequência de sementes;
- identificação da versão.

Colete:
- vitórias de X;
- vitórias de O;
- empates;
- quantidade de jogadas;
- duração;
- estados avaliados, quando disponíveis;
- falhas.

Exporte JSON e CSV por interfaces de persistência.
```

**Branch:** `feat/experiment-mode`

---

## Prompt 23 — Documentação experimental

```text
Crie docs/11-experimentacao.md com:
- pergunta de pesquisa;
- hipóteses;
- variáveis independentes;
- variáveis dependentes;
- variáveis controladas;
- cenários;
- quantidade de execuções;
- sementes;
- métricas;
- procedimento;
- ameaças à validade;
- plano de análise;
- gráficos previstos.

Inclua fluxograma e diagrama de sequência Mermaid, com interpretação textual antes e depois.
```

**Branch:** `docs/experiment-plan`

---

## Prompt 24 — Robustez das fronteiras externas

```text
Revise:
- entrada de Console;
- arquivos;
- diretórios;
- JSON;
- CSV;
- áudio;
- codificação;
- configurações;
- interrupção do modo automático.

Garanta:
- mensagens claras;
- fallback seguro;
- ausência de encerramentos inesperados;
- distinção entre erro de domínio e erro de infraestrutura;
- ausência de exceções usadas como fluxo normal.

Crie testes de falhas controladas.
```

**Branch:** `fix/external-boundaries`

---

## Prompt 25 — Compatibilidade multiplataforma

```text
Teste e documente Windows e pelo menos um sistema Unix-like.

Verifique:
- Unicode;
- ANSI;
- Console.Beep;
- terminal bell;
- limpeza de tela;
- redirecionamento de entrada e saída;
- caminhos;
- separadores;
- permissões;
- diretório de dados;
- publicação autocontida.

Adicione modo de compatibilidade e atualize docs/14-limitacoes.md.
```

**Branch:** `test/cross-platform`

---

## Prompt 26 — Configuração de publicação

```text
Configure dotnet publish para:
- win-x64 dependente do framework;
- win-x64 autocontido;
- linux-x64 dependente do framework;
- linux-x64 autocontido.

Documente:
- requisitos;
- comandos;
- diretórios gerados;
- execução;
- tamanho aproximado;
- diferenças entre modos.

Não versione binários. Confirme o .gitignore.
```

**Branch:** `chore/publish-config`

---

## Prompt 27 — Revisão arquitetural final

```text
Revise:
- direção das dependências;
- acoplamento;
- responsabilidades;
- ciclos;
- duplicação;
- acesso ao estado mutável;
- separação entre Domain, AI, Application, Presentation, Persistence e Audio.

Confirme especificamente:
- ausência de Domain → AI;
- Match.Board somente para leitura;
- Strategies sem alteração do estado original;
- adaptadores externos fora do domínio;
- composição de dependências em ponto único.

Atualize diagramas para refletir o código real e melhore comentários XML quando necessário.
```

**Branch:** `refactor/architecture-review`

---

## Prompt 28 — Cobertura e qualidade dos testes

```text
Execute toda a suíte e identifique lacunas.

Adicione testes para:
- domínio;
- regras;
- agregado;
- Strategies;
- resolução de Strategy;
- fluxo de aplicação;
- apresentação;
- persistência;
- experimentação;
- falhas externas;
- compatibilidade.

Evite:
- testes dependentes de tempo real;
- Console físico;
- ordem global;
- arquivos permanentes;
- valores pseudoaleatórios não controlados;
- snapshots frágeis sem justificativa.

Documente estratégia e limitações em docs/12-testes.md.
```

**Branch:** `test/coverage-review`

---

## Prompt 29 — Experimento de referência

```text
Execute experimento com todas as combinações disponíveis de Strategies.

Inclua:
- alternância de X e O;
- alternância do primeiro participante;
- sementes registradas;
- número suficiente de repetições;
- versão da aplicação;
- ambiente;
- duração;
- métricas por partida e agregadas.

Exporte JSON e CSV.

Produza docs/13-resultados.md com:
- tabelas;
- gráficos;
- interpretação;
- limitações;
- ameaças à validade;
- instruções de reprodução.
```

**Branch:** `experiment/reference-run`

---

## Prompt 30 — Revisão legal e documental

```text
Revise:
- LICENSE;
- LICENSE.md;
- NOTICE;
- CITATION.cff;
- README.md;
- CHANGELOG.md;
- documentos em docs;
- comentários XML.

Confirme:
- dependências;
- atribuições;
- modificações sobre o legado;
- política de idioma;
- versões;
- datas;
- comandos;
- nomes reais de arquivos;
- consistência entre diagramas e código.

Crie ou atualize um índice da documentação.
```

**Branch:** `docs/final-review`

---

## Prompt 31 — Preparação da versão v1.9.0

```text
Prepare a versão 1.9.0 como candidata final.

Atualize:
- Directory.Build.props;
- CHANGELOG.md;
- CITATION.cff;
- README.md;
- timeline;
- documentação de publicação.

Execute:
- restore;
- build Release;
- test;
- verificação de whitespace;
- verificação de arquivos ignorados;
- publicações de validação;
- experimento curto.

Confirme ausência de:
- dados temporários;
- dados pessoais;
- binários;
- segredos;
- arquivos locais;
- sementes não registradas em resultados.
```

**Branch:** `release/v1.9.0`

---

## Prompt 32 — Consolidação da versão v2.0.0

```text
Prepare a versão 2.0.0 como conclusão da refatoração.

Atualize:
- versão;
- CHANGELOG.md;
- CITATION.cff;
- README.md;
- timeline;
- documentação;
- limitações;
- instruções de instalação e execução.

Execute:
- toda a suíte;
- publicações suportadas;
- experimento curto reproduzível;
- validação multiplataforma disponível;
- auditoria de arquivos;
- auditoria legal.

Produza instruções para:
- commit de release;
- merge;
- tag anotada v2.0.0;
- push da tag;
- GitHub Release;
- anexação opcional de artefatos de publicação.
```

**Branch:** `release/v2.0.0`

---

# 5. Sequência recomendada imediata

A próxima sequência deve ser:

1. confirmar build e testes após a correção arquitetural;
2. integrar a branch corretiva;
3. iniciar o Prompt 11 em `feat/heuristic-strategy`;
4. nomear o patch do Prompt 11 com prefixo `0012`;
5. manter revisões intermediárias com nomes descritivos, sem numeração incremental;
6. criar a tag `v1.4.0` somente depois da estratégia heurística integrada e validada.

# 6. Critério de conclusão de cada prompt

Cada prompt somente será considerado concluído quando houver:

- branch correta;
- código compilável;
- testes aprovados;
- `git diff --check` sem erros;
- documentação XML atualizada;
- documentação Markdown atualizada;
- diagramas Mermaid com parágrafo antes e depois;
- `CHANGELOG.md` atualizado;
- versão ajustada quando prevista;
- commit em português do Brasil;
- merge `--no-ff`;
- branch principal funcional.
