# Revisão técnica após o Prompt 10

**Projeto analisado:** Tic-Tac-Toe Console AI  
**Data da revisão:** 2026-07-16  
**Versão declarada:** 1.3.0  
**Estado adicional:** alterações de Application em `Unreleased`

## 1. Síntese executiva

O projeto apresenta evolução consistente em relação ao legado: o domínio foi
separado de Console, regras foram isoladas, o agregado `Match` controla o ciclo
da partida, a Strategy aleatória é reproduzível e o fluxo de aplicação pode ser
testado sem interação humana.

A base é adequada para continuidade, mas há duas dívidas arquiteturais de alta
prioridade:

1. dependência conceitual circular entre `Domain` e `AI`;
2. possibilidade de modificar `Match.Board` por fora do agregado.

Também havia documentação conceitual desatualizada e timelines sem datas. O
patch desta revisão corrige a documentação e registra explicitamente as dívidas.

## 2. Evidências do repositório

- 25 arquivos C# em `src`;
- 19 arquivos C# em `tests`;
- 9 documentos Markdown em `docs` antes desta revisão;
- 70 métodos `[Fact]`;
- 3 métodos `[Theory]`;
- 8 casos declarados com `[InlineData]`;
- tags presentes: `v1.0.0`, `v1.1.0`, `v1.2.0` e `v1.3.0`;
- commits dos marcos atuais concentrados em 15 e 16 de julho de 2026;
- artefatos `bin/` e `obj/` aparecem no ZIP, mas não estão versionados no Git.

O ambiente usado nesta revisão não possui o SDK do .NET, portanto não foi
possível repetir `dotnet build` e `dotnet test`. A revisão de código foi estática.
O ZIP contém artefatos de compilação Release e o usuário havia informado
execução anterior bem-sucedida da suíte.

## 3. Avaliação do código

### 3.1 Pontos fortes

- `BoardPosition` e `Move` são pequenos, imutáveis e validam invariantes;
- `Board` encapsula o array bidimensional;
- `GameRules` é puro e não altera o tabuleiro;
- `GameEvaluation` copia defensivamente a sequência vencedora;
- `Match` centraliza turno, histórico, estado e resultado;
- `RandomMoveStrategy` usa fonte pseudoaleatória injetável;
- sementes controláveis favorecem experimentação reproduzível;
- `MatchController` depende de portas, não de `System.Console`;
- testes usam filas, fakes e estratégias determinísticas;
- nomenclatura segue o padrão acordado: tipos em CamelCase e métodos/variáveis em `snake_case`;
- comentários XML estão em português do Brasil.

### 3.2 Achado crítico: limite do agregado pode ser contornado

`Match` expõe `Board` publicamente, e `Board` expõe `apply_move` e `undo_move`.
Um consumidor pode alterar o tabuleiro sem atualizar:

- `Moves`;
- `CurrentPlayer`;
- `State`;
- `Result`;
- `WinningPositions`.

Isso contradiz o comentário XML de `Match`, segundo o qual jogadas não devem ser
aplicadas diretamente ao tabuleiro.

**Recomendação:** criar `IReadOnlyBoard` ou uma visão imutável para consumidores.
As mutações devem ficar internas ao agregado. Para Minimax, usar clone/snapshot
ou um tabuleiro de simulação separado.

### 3.3 Achado alto: ciclo conceitual Domain ↔ AI

`AI` depende de `Domain`, como esperado. Porém `Domain.ComputerPlayer` importa
`TicTacToe.AI` e armazena `IMoveStrategy`. Isso cria:

- `AI → Domain`;
- `Domain → AI`.

A relação contraria `docs/03-arquitetura.md`, que afirma que o domínio não
conhece estratégias.

**Recomendação preferencial:** manter `ComputerPlayer` como marcador de domínio e
associar estratégias na camada `Application`, por exemplo em um registro
`IComputerStrategyResolver`.

### 3.4 Achado alto: exceção de saída pode ser interpretada como jogada inválida

Em `MatchController.run`, o bloco `try` abrange tanto `match.apply_move` quanto
`game_output.show_match`. Se uma implementação de saída lançar
`InvalidOperationException`, o controlador poderá tratá-la como posição inválida.

**Recomendação:** restringir o `try/catch` apenas à chamada
`match.apply_move(position)` e chamar `show_match` depois do bloco.

### 3.5 Achado médio: `GameState.NotStarted` ainda não participa do fluxo

`Match` nasce diretamente em `InProgress`. A enumeração contém `NotStarted`, mas
não há operação `start`. Isso não quebra o fluxo atual, porém existe diferença
entre modelo e comportamento.

**Recomendação:** ou remover `NotStarted` enquanto não necessário, ou introduzir
um ciclo explícito de criação e início quando a configuração de partidas existir.

### 3.6 Achado médio: responsabilidades de `ComputerPlayer`

`ComputerPlayer` é simultaneamente entidade de domínio e contexto Strategy. Essa
combinação simplifica o estágio atual, mas acopla identidade do jogador ao
algoritmo.

**Recomendação:** permitir trocar estratégia por configuração de partida, não por
mutação da entidade, especialmente nos experimentos AI versus AI.

### 3.7 Achado médio: snapshots de saída nos testes

`RecordingOutput` armazena repetidamente a mesma referência de `Match`. Assim,
todos os itens de `ShownMatches` apontam para o estado final, e não para o estado
observado em cada chamada.

**Recomendação:** quando os testes precisarem validar evolução visual, registrar
snapshots ou DTOs imutáveis com turno, tabuleiro, estado e resultado.

### 3.8 Achado baixo: alocações de coleções somente leitura

`Board.get_available_positions()` cria lista e `AsReadOnly()` em cada chamada;
`Match.Moves` cria um novo wrapper a cada acesso. Para jogo da velha, o custo é
irrelevante. Para Minimax, muitas avaliações podem amplificar alocações.

**Recomendação:** medir antes de otimizar. Em Minimax, considerar API de enumeração
ou spans/snapshots apenas se os experimentos mostrarem necessidade.

## 4. Avaliação dos testes

A cobertura comportamental é boa para o estágio atual:

- coordenadas e jogadas;
- aplicação e desfazimento;
- linhas, colunas e diagonais;
- empate e falsos positivos;
- turnos e encerramento;
- aleatoriedade reproduzível;
- delegação Strategy;
- fluxo de aplicação sem interação humana.

Lacunas recomendadas:

1. teste que demonstre a mutação indevida de `Match.Board`;
2. teste de exceção lançada por `IGameOutput`;
3. teste de exceção lançada por `IMoveSelector`;
4. teste de snapshot de estados apresentados;
5. teste de múltiplas partidas com mesma semente;
6. teste de integridade arquitetural entre namespaces/camadas.

## 5. Avaliação da documentação

### 5.1 Pontos fortes

- todos os diagramas Mermaid possuem contextualização textual;
- requisitos, arquitetura, modelo, regras, Match, IA e Application possuem
  documentos próprios;
- `CHANGELOG.md`, `CITATION.cff` e `Directory.Build.props` estão alinhados em
  `1.3.0`;
- o histórico de versões está organizado segundo versionamento semântico.

### 5.2 Problemas encontrados

- `README.md` parava em `v1.1.0`;
- timelines não possuíam datas;
- `docs/04-modelo-conceitual.md` ainda dizia que `Board`, `Match`, `Player` e
  `GameRules` não estavam implementados;
- o diagrama conceitual apresentava assinaturas antigas;
- `docs/06-match-aggregate.md` dizia que `ComputerPlayer` ainda não possuía
  Strategy;
- a divergência `Domain ↔ AI` não estava registrada;
- a numeração salta de `docs/07` para `docs/10`, o que deve ser explicado em um
  índice documental futuro.

## 6. Timeline revisada

Datas confirmadas pelo histórico Git:

- 2020-11-23: publicação inicial do código legado;
- 2026-07-15: `v1.0.0`;
- 2026-07-15: `v1.1.0`;
- 2026-07-16: `v1.2.0`;
- 2026-07-16: `v1.3.0`;
- 2026-07-16: início de `Unreleased` com fluxo básico de aplicação.

As versões `v1.4.0` a `v2.0.0` permanecem com **data a definir**. Não foram
inventadas datas futuras.

## 7. Prioridades recomendadas

### Antes do Prompt 11

1. corrigir o `try/catch` de `MatchController`;
2. decidir como eliminar `Domain → AI`;
3. restringir mutação externa de `Match.Board`;
4. aplicar o patch documental desta revisão;
5. executar build e testes em ambiente .NET 9.

### Antes do Minimax

1. definir snapshot/clone de tabuleiro;
2. definir API segura para simulação e desfazimento;
3. medir alocações de casas livres;
4. criar testes de não alteração do estado original;
5. registrar métricas de nós avaliados e tempo de decisão fora de `Move`.

## 8. Conclusão

O projeto está em bom estado para dez prompts de refatoração: há separação
substancial do legado, testes numerosos e documentação acima da média. A
continuidade deve priorizar a coerência das fronteiras arquiteturais antes de
adicionar complexidade às estratégias. Corrigidas essas fronteiras, a base será
adequada para heurística, Minimax, apresentação e experimentação.
