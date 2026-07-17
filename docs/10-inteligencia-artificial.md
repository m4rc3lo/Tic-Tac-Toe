# Inteligência artificial e padrão Strategy

## 1. Finalidade

Este documento descreve a infraestrutura de inteligência artificial do **Tic-Tac-Toe Console AI** até a versão `1.4.0`.

O módulo contém uma estratégia aleatória, utilizada como linha de base, e uma estratégia heurística baseada em prioridades táticas e posicionais. Ambas implementam `IMoveStrategy`, recebem `IReadOnlyBoard` e não modificam o estado pertencente ao agregado `Match`.

## 2. Fronteiras arquiteturais

O padrão Strategy permite trocar o algoritmo de decisão sem alterar o domínio ou o fluxo da partida. Depois da revisão arquitetural posterior ao Prompt 10, `ComputerPlayer` não armazena uma Strategy. A associação entre participante computacional e algoritmo é resolvida na camada `Application`.

O diagrama apresenta as relações atuais entre aplicação, estratégias e domínio.

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

    class IMoveStrategy {
        <<interface>>
        +choose_move(board, symbol) BoardPosition
    }

    class RandomMoveStrategy
    class HeuristicMoveStrategy

    class IReadOnlyBoard {
        <<interface>>
        +int OccupiedCount
        +bool IsFull
        +get_symbol(position) Symbol
        +get_available_positions() IReadOnlyList
        +is_position_available(position) bool
    }

    IComputerMoveStrategyResolver --> ComputerPlayer
    IComputerMoveStrategyResolver --> IMoveStrategy
    IMoveStrategy <|.. RandomMoveStrategy
    IMoveStrategy <|.. HeuristicMoveStrategy
    RandomMoveStrategy --> IReadOnlyBoard
    HeuristicMoveStrategy --> IReadOnlyBoard
```

A direção das dependências permanece `Application → AI → Domain`. O módulo `Domain` não conhece estratégias, e o tabuleiro exposto por `Match` é somente para leitura.

## 3. Contrato de estratégia

A interface recebe o estado consultável do tabuleiro e o símbolo controlado:

```csharp
public interface IMoveStrategy
{
    BoardPosition choose_move(
        IReadOnlyBoard board,
        Symbol symbol);
}
```

Ela retorna apenas uma `BoardPosition`. A criação de `Move`, a numeração do turno, a aplicação no tabuleiro, a alternância e o encerramento continuam sob responsabilidade de `Match`.

## 4. Aleatoriedade injetável

As estratégias que precisam desempatar alternativas dependem de `IRandomSource`, em vez de utilizar `Random` diretamente. Essa decisão permite:

- sementes controláveis;
- reprodução de experimentos;
- testes determinísticos;
- injeção de índices conhecidos;
- substituição do gerador sem alterar o algoritmo.

As estratégias aleatória e heurística oferecem construção sem parâmetros, com semente e com fonte injetada:

```csharp
new RandomMoveStrategy();
new RandomMoveStrategy(2026);
new RandomMoveStrategy(random_source);

new HeuristicMoveStrategy();
new HeuristicMoveStrategy(2026);
new HeuristicMoveStrategy(random_source);
```

O construtor sem parâmetros é adequado ao uso interativo. Em testes e experimentos, deve-se registrar a semente ou injetar a fonte explicitamente.

## 5. Estratégia aleatória

`RandomMoveStrategy` consulta as casas disponíveis e seleciona uma delas pelo índice fornecido por `IRandomSource`.

O fluxo da estratégia aleatória é apresentado a seguir.

```mermaid
sequenceDiagram
    participant Application
    participant Strategy as RandomMoveStrategy
    participant Board as IReadOnlyBoard
    participant Random as IRandomSource

    Application->>Strategy: choose_move(board, symbol)
    Strategy->>Board: get_available_positions()
    Board-->>Strategy: posições livres
    Strategy->>Random: next(quantidade)
    Random-->>Strategy: índice
    Strategy-->>Application: posição selecionada
```

A estratégia não considera vitória, bloqueio ou qualidade posicional. Por isso, ela funciona como linha de base experimental.

## 6. Estratégia heurística

`HeuristicMoveStrategy` avalia as categorias na seguinte ordem:

1. vitória imediata;
2. bloqueio de vitória imediata do adversário;
3. centro;
4. cantos;
5. laterais.

Quando uma categoria possui mais de uma alternativa equivalente, a escolha usa `IRandomSource`. Não há desempate aleatório entre categorias diferentes: a prioridade superior sempre prevalece.

### 6.1 Pseudocódigo

O pseudocódigo resume a decisão sem detalhes de implementação.

```text
função escolher_jogada(tabuleiro, símbolo):
    livres ← obter_casas_livres(tabuleiro)
    simulação ← copiar_tabuleiro(tabuleiro)

    vitórias ← casas que vencem para símbolo
    se vitórias não estiver vazia:
        retornar desempatar(vitórias)

    adversário ← símbolo oposto
    bloqueios ← casas que vencem para adversário
    se bloqueios não estiver vazia:
        retornar desempatar(bloqueios)

    se centro estiver livre:
        retornar centro

    cantos ← cantos livres
    se cantos não estiver vazio:
        retornar desempatar(cantos)

    laterais ← laterais livres
    retornar desempatar(laterais)
```

A busca por vitória e bloqueio simula cada casa livre em uma cópia interna. Depois de cada hipótese, a célula simulada volta a `Empty`. Nenhuma operação mutável é chamada no `IReadOnlyBoard` original.

### 6.2 Diagrama de decisão

O diagrama mostra a precedência estrita das categorias avaliadas.

```mermaid
flowchart TD
    A[Receber IReadOnlyBoard e símbolo] --> B{Há casas livres?}
    B -- Não --> X[Rejeitar seleção]
    B -- Sim --> C[Copiar símbolos para simulação interna]
    C --> D{Há vitória imediata?}
    D -- Sim --> E[Desempatar vitórias]
    D -- Não --> F{Adversário pode vencer?}
    F -- Sim --> G[Desempatar bloqueios]
    F -- Não --> H{Centro livre?}
    H -- Sim --> I[Escolher centro]
    H -- Não --> J{Há canto livre?}
    J -- Sim --> K[Desempatar cantos]
    J -- Não --> L[Desempatar laterais]
    E --> M[Retornar posição]
    G --> M
    I --> M
    K --> M
    L --> M
```

A decisão é determinística quando existe uma única alternativa na categoria de maior prioridade. O gerador é consultado somente quando há duas ou mais alternativas equivalentes.

## 7. Representação de simulação

A estratégia heurística contém uma representação interna de nove células. O construtor dessa representação copia cada símbolo por meio de `IReadOnlyBoard.get_symbol`.

A simulação permite:

- inserir temporariamente `X` ou `O`;
- verificar as oito sequências vencedoras;
- desfazer a hipótese somente na cópia;
- preservar integralmente o tabuleiro original.

Essa representação é privada à estratégia. Ela não amplia a superfície pública de mutação e não permite contornar o agregado `Match`.

## 8. Complexidade

Para um tabuleiro de tamanho fixo `3 × 3`, o custo prático é constante. Em termos do número `n` de casas livres:

- cópia do tabuleiro: `O(9)`;
- avaliação de vitórias do agente: até `n` hipóteses;
- avaliação de bloqueios: até `n` hipóteses;
- cada hipótese verifica oito linhas de três posições;
- filtragem de cantos e laterais: custo constante.

De forma generalizada para este algoritmo, o tempo é `O(n)` porque o número e o tamanho das sequências vencedoras são fixos. O espaço adicional é `O(1)` para o tabuleiro `3 × 3`.

## 9. Invariantes

As estratégias preservam as seguintes invariantes:

1. o tabuleiro não pode ser nulo;
2. o símbolo não pode ser `Empty`;
3. deve existir pelo menos uma casa livre;
4. a posição retornada deve estar disponível;
5. o tabuleiro recebido não é modificado;
6. o índice pseudoaleatório deve pertencer ao intervalo solicitado;
7. sementes iguais reproduzem os mesmos desempates para estados equivalentes;
8. vitória imediata precede bloqueio;
9. bloqueio precede preferências posicionais;
10. centro precede cantos, e cantos precedem laterais.

## 10. Limitações

A estratégia heurística não realiza busca em profundidade. Consequentemente, ela:

- não prevê armadilhas com duas ameaças simultâneas;
- não procura criar bifurcações deliberadamente;
- não garante jogo perfeito;
- não atribui pontuações graduais a estados futuros;
- pode perder contra sequências que exigem planejamento além de um turno.

Essas limitações justificam a implementação posterior de `MinimaxMoveStrategy`, que deverá usar um estado de busca independente e preservar o `IReadOnlyBoard` original.
