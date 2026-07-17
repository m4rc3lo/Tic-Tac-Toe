# Implementações de Console

## 1. Finalidade

Este documento descreve os adaptadores concretos introduzidos após a versão
`1.5.0`. Eles conectam as portas da camada `Application` aos fluxos textuais do
.NET sem adicionar regras ao domínio.

A etapa fornece uma partida mínima jogável entre uma pessoa, usando `X`, e uma
estratégia Minimax, usando `O`.

## 2. Adaptadores

A apresentação concreta contém:

- `ConsoleGameInput`, que implementa `IGameInput`;
- `ConsoleGameOutput`, que implementa `IGameOutput`;
- `ConsoleBoardRenderer`, que renderiza `IReadOnlyBoard`;
- `Program`, que compõe as dependências.

O diagrama apresenta a direção das dependências.

```mermaid
classDiagram
    class IGameInput {
        <<interface>>
        +read_move(match, player) BoardPosition
    }

    class IGameOutput {
        <<interface>>
        +show_match(match)
        +show_invalid_move(player, position, message)
        +show_result(match)
    }

    class ConsoleGameInput {
        -TextReader reader
        -TextWriter writer
        +read_move(match, player) BoardPosition
        +try_parse_position(input, position) bool
    }

    class ConsoleGameOutput {
        -TextWriter writer
        -ConsoleBoardRenderer board_renderer
    }

    class ConsoleBoardRenderer {
        -TextWriter writer
        +render(board)
    }

    IGameInput <|.. ConsoleGameInput
    IGameOutput <|.. ConsoleGameOutput
    ConsoleGameOutput --> ConsoleBoardRenderer
```

Os adaptadores dependem das portas de aplicação e dos contratos somente para
leitura do domínio. Nenhuma interface existente foi recriada.

## 3. Entrada

As coordenadas são apresentadas ao usuário no intervalo de `1` a `3` e
convertidas internamente para índices entre `0` e `2`.

São aceitos espaço, tabulação, vírgula ou ponto e vírgula como separadores. A
conversão utiliza `int.TryParse`, de modo que texto inválido e valores fora do
intervalo não dependem de exceções para o fluxo normal.

O diagrama mostra a repetição da leitura.

```mermaid
flowchart TD
    A[Solicitar linha e coluna] --> B[Ler linha]
    B --> C{Entrada encerrada?}
    C -- Sim --> X[Lançar EndOfStreamException]
    C -- Não --> D{Parsing válido?}
    D -- Não --> E[Exibir orientação]
    E --> A
    D -- Sim --> F{Casa disponível?}
    F -- Não --> G[Informar casa ocupada]
    G --> A
    F -- Sim --> H[Retornar BoardPosition]
```

Somente o encerramento inesperado do fluxo é tratado como exceção. Erros
corrigíveis de digitação ou ocupação geram mensagem e nova tentativa.

## 4. Saída e tabuleiro

A renderização inicial utiliza somente caracteres ASCII. Ela apresenta:

- coordenadas;
- símbolos `X` e `O`;
- separadores;
- jogador atual;
- jogada inválida;
- vitória ou empate.

Unicode, cores ANSI, limpeza de tela, temas e artes serão adicionados em etapas
posteriores.

## 5. Composição mínima

`Program.Main` funciona como composition root. Ele instancia leitores,
escritores, adaptadores, resolvedor de Strategy, controlador e participantes.

O fluxo completo é apresentado a seguir.

```mermaid
sequenceDiagram
    participant Program
    participant Input as ConsoleGameInput
    participant Output as ConsoleGameOutput
    participant Controller as MatchController
    participant Match
    participant AI as MinimaxMoveStrategy

    Program->>Controller: run(match)
    Controller->>Output: show_match(match)

    loop enquanto a partida estiver em andamento
        alt turno humano
            Controller->>Input: read_move(match, player)
            Input-->>Controller: posição
        else turno computacional
            Controller->>AI: choose_move(board, O)
            AI-->>Controller: posição
        end

        Controller->>Match: apply_move(position)
        Controller->>Output: show_match(match)
    end

    Controller->>Output: show_result(match)
```

A composição não implementa menu ou configuração interativa. Essas
responsabilidades pertencem às próximas etapas de apresentação.

## 6. Testabilidade

`TextReader` e `TextWriter` são injetados nos adaptadores. Os testes utilizam
`StringReader` e `StringWriter`, sem acesso ao Console físico.

A suíte cobre:

- parsing válido;
- separadores aceitos;
- entradas inválidas;
- repetição após erro;
- repetição após casa ocupada;
- renderização ASCII;
- jogador atual;
- resultado;
- mensagem de jogada inválida.
