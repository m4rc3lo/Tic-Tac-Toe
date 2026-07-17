# Correção das fronteiras arquiteturais

## 1. Finalidade

Esta etapa corretiva foi realizada após a revisão do Prompt 10. Seu objetivo é
restabelecer a direção das dependências, proteger o limite do agregado `Match` e
distinguir falhas de domínio de falhas de saída.

A versão permanece `1.3.0`; as alterações ficam em `Unreleased`.

## 2. Dependências entre camadas

Antes da correção, `ComputerPlayer` armazenava uma `IMoveStrategy`. Como
`IMoveStrategy` pertence a `AI` e depende de tipos de `Domain`, existia um ciclo
conceitual entre os módulos.

O diagrama apresenta a direção corrigida.

```mermaid
flowchart LR
    Presentation[Presentation futura]
    Application[Application]
    AI[AI]
    Domain[Domain]

    Presentation --> Application
    Application --> AI
    Application --> Domain
    AI --> Domain
```

`Domain` não referencia `AI`. `ComputerPlayer` representa apenas um participante
computacional. A camada `Application` resolve a Strategy por meio de
`IComputerMoveStrategyResolver`.

## 3. Associação de estratégias

O resolvedor associa participantes computacionais a estratégias sem modificar a
entidade do domínio.

```mermaid
classDiagram
    class ComputerPlayer {
        +string Name
        +Symbol Symbol
    }

    class IComputerMoveStrategyResolver {
        <<interface>>
        +resolve_strategy(player) IMoveStrategy
    }

    class ConfiguredComputerMoveStrategyResolver {
        -IReadOnlyDictionary strategies
        +resolve_strategy(player) IMoveStrategy
    }

    class DefaultMoveSelector {
        -IGameInput game_input
        -IComputerMoveStrategyResolver strategy_resolver
        +select_move(match, player) BoardPosition
    }

    class IMoveStrategy {
        <<interface>>
        +choose_move(board, symbol) BoardPosition
    }

    DefaultMoveSelector --> IComputerMoveStrategyResolver
    IComputerMoveStrategyResolver <|.. ConfiguredComputerMoveStrategyResolver
    ConfiguredComputerMoveStrategyResolver --> IMoveStrategy
    DefaultMoveSelector --> ComputerPlayer
```

A configuração por símbolo é suficiente para a partida atual, pois `Match`
impede que os dois participantes controlem o mesmo símbolo.

## 4. Tabuleiro somente para leitura

`Match` mantém um `Board` mutável privado, mas expõe somente `IReadOnlyBoard`.
Uma instância interna de `BoardView` encaminha consultas e não fornece operações
de aplicação ou desfazimento.

```mermaid
classDiagram
    class Match {
        -Board board
        +IReadOnlyBoard Board
        +apply_move(position) Move
    }

    class IReadOnlyBoard {
        <<interface>>
        +int OccupiedCount
        +bool IsFull
        +get_symbol(position) Symbol
        +get_available_positions() IReadOnlyList
        +is_position_available(position) bool
    }

    class BoardView {
        -Board board
    }

    class Board {
        +apply_move(move)
        +undo_move(position) Symbol
    }

    Match *-- Board
    Match *-- BoardView
    IReadOnlyBoard <|.. BoardView
    IReadOnlyBoard <|.. Board
    BoardView --> Board
```

Consumidores de `Match` não podem converter a visão pública para `Board`, pois o
objeto exposto é um `BoardView`. Estratégias também recebem `IReadOnlyBoard`.

## 5. Tratamento de exceções no controlador

O controlador captura `InvalidOperationException` apenas ao aplicar uma jogada.
A apresentação do novo estado ocorre fora do bloco.

```mermaid
sequenceDiagram
    participant Controller as MatchController
    participant Match
    participant Output as IGameOutput

    Controller->>Match: apply_move(position)

    alt jogada inválida
        Match-->>Controller: InvalidOperationException
        Controller->>Output: show_invalid_move(...)
    else jogada válida
        Match-->>Controller: Move
        Controller->>Output: show_match(match)
    end
```

Uma falha de `IGameOutput` agora se propaga ao chamador, em vez de ser
classificada como erro de posição.

## 6. Testes adicionados

A etapa acrescenta testes para:

- visão pública de tabuleiro não conversível para `Board`;
- atualização da visão após jogadas válidas;
- resolução de Strategy por símbolo;
- ausência de configuração de Strategy;
- propagação de falhas da saída;
- delegação do seletor à Strategy resolvida.
