# Partidas e estatísticas em JSON

## 1. Finalidade

Esta etapa persiste partidas concluídas e estatísticas agregadas sem adicionar
dependências de JSON ao domínio. A conversão ocorre em `MatchRecordMapper`, na
camada `Persistence`.

Cada registro contém participantes, Strategies, jogadas, resultado, sequência
vencedora, duração, semente e versão da aplicação.

## 2. Modelo persistente

Os registros são imutáveis e representam somente dados serializáveis.

O diagrama entidade-relacionamento apresenta a estrutura lógica dos arquivos.

```mermaid
erDiagram
    MATCH_RECORD ||--|| FIRST_PARTICIPANT : possui
    MATCH_RECORD ||--|| SECOND_PARTICIPANT : possui
    MATCH_RECORD ||--o{ MATCH_MOVE : registra
    MATCH_RECORD ||--o{ WINNING_POSITION : identifica
    MATCH_STATISTICS ||--o{ STRATEGY_STATISTICS : agrega

    MATCH_RECORD {
        uuid id
        datetime started_at
        datetime finished_at
        long duration_milliseconds
        string result
        int random_seed
        string application_version
    }

    FIRST_PARTICIPANT {
        string name
        string kind
        string symbol
        string strategy
    }

    SECOND_PARTICIPANT {
        string name
        string kind
        string symbol
        string strategy
    }

    MATCH_MOVE {
        int turn_number
        int row
        int column
        string symbol
    }

    WINNING_POSITION {
        int row
        int column
    }

    MATCH_STATISTICS {
        int total_matches
        int x_wins
        int o_wins
        int draws
        int total_moves
        double average_moves
        double average_duration
    }

    STRATEGY_STATISTICS {
        string strategy
        int matches
        int wins
        int losses
        int draws
    }
```

O histórico é armazenado como coleção de `MatchRecord`. As estatísticas são
recalculadas a partir do histórico atualizado, evitando incrementos acumulados
com regras divergentes.

## 3. Conversão fora do domínio

`MatchRecordMapper` recebe `MatchPersistenceContext`. Esse contexto combina a
partida concluída com dados externos que não pertencem ao agregado:

- instante inicial;
- instante final;
- Strategy associada a cada participante;
- semente;
- versão da aplicação.

O domínio não conhece `MatchRecord`, repositórios ou caminhos de arquivo.

## 4. Repositórios JSON

As interfaces são:

- `IMatchHistoryRepository`;
- `IMatchStatisticsRepository`.

As implementações concretas utilizam `System.Text.Json`, propriedades em
camelCase, UTF-8 sem BOM e substituição por arquivo temporário.

O fluxo de salvamento é apresentado a seguir.

```mermaid
sequenceDiagram
    participant Runner as ConsoleMatchSessionRunner
    participant Service as MatchPersistenceService
    participant Mapper as MatchRecordMapper
    participant History as IMatchHistoryRepository
    participant Calculator as MatchStatisticsCalculator
    participant Statistics as IMatchStatisticsRepository

    Runner->>Service: persist(context)
    Service->>Mapper: map(context)
    Mapper-->>Service: MatchRecord
    Service->>History: load_all()
    History-->>Service: histórico anterior
    Service->>History: replace_all(histórico + partida)
    Service->>Calculator: calculate(histórico atualizado)
    Calculator-->>Service: estatísticas
    Service->>Statistics: save(estatísticas)
```

Caso a atualização das estatísticas falhe, o serviço tenta restaurar o histórico
anterior antes de propagar a falha. Em armazenamento local normal, isso evita
que uma partida permaneça registrada sem a estatística correspondente.

## 5. Integração com a execução

`ConsoleMatchSessionRunner` mede a duração com `Stopwatch`, executa
`MatchController` e persiste somente após o encerramento.

A versão é obtida do assembly em execução. A semente vem da configuração da
partida e pode ser nula.

Os arquivos padrão são:

```text
data/matches.json
data/statistics.json
```

O diretório-base respeita `ApplicationSettings.Directories.Data`.

## 6. Recuperação e arquivos temporários

Arquivos ausentes ou JSON inválido retornam histórico vazio ou estatísticas
zeradas. A próxima gravação substitui o conteúdo por JSON válido.

Cada escrita utiliza:

1. criação do diretório;
2. arquivo `*.tmp-<guid>`;
3. serialização completa;
4. movimento com substituição;
5. remoção de temporário residual.

## 7. Testes

Os testes usam diretórios temporários e verificam:

- criação de diretórios;
- round-trip do histórico;
- round-trip das estatísticas;
- recuperação de JSON inválido;
- ausência de temporários;
- mapeamento completo;
- agregação por resultado e Strategy;
- persistência coordenada;
- rollback quando a estatística falha.
