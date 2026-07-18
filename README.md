# Tic-Tac-Toe Console AI

Refatoração didática de uma implementação legada de jogo da velha, desenvolvida
em C# para .NET 9.

## Estado do projeto

- `v1.0.0` — 2026-07-15: estado legado preservado;
- `v1.1.0` — 2026-07-15: solução .NET 9, governança e infraestrutura inicial;
- `v1.2.0` — 2026-07-16: domínio, regras e agregado de partida;
- `v1.3.0` — 2026-07-16: padrão Strategy e estratégia aleatória;
- `v1.4.0` — 2026-07-16: fluxo básico de aplicação, correções arquiteturais e estratégia heurística;
- `v1.5.0` — 2026-07-17: estratégia Minimax e métricas de busca;
- `v1.6.0` — 2026-07-17: apresentação Console e máquina de estados;
- `v1.7.0` — 2026-07-17: recursos visuais, animações e áudio com fallback;
- `v1.8.0` — 2026-07-18: persistência JSON e exportação CSV;
- `Unreleased`: próxima etapa de desenvolvimento;
- `v2.0.0` — data a definir: consolidação final da refatoração.

O código legado foi movido para `legacy/` e não participa da compilação. A nova
implementação será construída em `src/`, com testes em `tests/`.

## Requisitos

- SDK do .NET 9;
- Git.

## Restaurar dependências

```bash
dotnet restore
```

## Compilar

```bash
dotnet build
```

## Executar

```bash
dotnet run --project src/TicTacToe.Console
```

## Executar os testes

```bash
dotnet test
```

## Convenções iniciais

- identificadores em inglês;
- tipos em `CamelCase`;
- métodos e variáveis em `snake_case`;
- comentários e documentação em português do Brasil;
- indentação com quatro espaços, sem tabulações;
- arquivos de texto em UTF-8 e finais de linha LF.
