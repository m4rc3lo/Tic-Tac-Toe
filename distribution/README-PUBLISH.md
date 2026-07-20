# Tic-Tac-Toe Console AI — execução do pacote

## Requisitos

Pacotes dependentes do framework exigem o .NET Runtime 9 compatível com o
sistema operacional e a arquitetura. Pacotes autocontidos já incluem o runtime.

## Executar no Windows

```powershell
.\TicTacToe.Console.exe
```

Também é possível executar:

```powershell
dotnet .\TicTacToe.Console.dll
```

## Executar no Linux

```bash
chmod +x TicTacToe.Console
./TicTacToe.Console
```

No pacote dependente do framework:

```bash
dotnet TicTacToe.Console.dll
```

## Dados locais

Na primeira execução, a aplicação cria os diretórios e arquivos necessários em
relação ao diretório do executável. O pacote não inclui histórico, estatísticas
ou resultados experimentais.

`settings.example.json` é apenas uma referência. A configuração ativa é criada
pela aplicação em `data/settings.json`.

## Compatibilidade

Quando a entrada ou saída está redirecionada, a aplicação reduz
automaticamente cores, animações, limpeza de tela, áudio e interação por teclas.
