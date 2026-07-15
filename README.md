# Tic-Tac-Toe Console AI

Refatoração didática de uma implementação legada de jogo da velha, desenvolvida
em C# para .NET 9.

## Estado do projeto

- `v1.0.0`: estado legado preservado;
- `v1.1.0`: solução .NET 9, projeto Console e infraestrutura inicial de testes;
- `v2.0.0`: versão final planejada da refatoração.

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
