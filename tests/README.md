# Estratégia e classificação dos testes

## 1. Classificação

A classificação é estrutural e baseada no tipo de dependência exercitada.

| Classe | Localização principal | Característica |
|---|---|---|
| unitário | `Domain/`, `AI/`, partes de `Application/`, `Audio/`, `Presentation/` e `Compatibility/` | memória, contratos simulados e nenhuma fronteira persistente |
| integração local | `Persistence/` e repositórios experimentais | combina componentes reais usando diretórios temporários |
| validação de publicação | `Publication/` e scripts de `scripts/` | inspeciona perfis e conteúdo de pacotes publicados |

Algumas pastas possuem testes de mais de uma classe. A classificação é definida
pelo comportamento do teste, não apenas pelo namespace.

## 2. Restrições

A suíte não deve depender de:

- relógio real;
- Console físico;
- dispositivo de áudio;
- ordem global;
- arquivos permanentes;
- caminhos absolutos fixos;
- pseudoaleatoriedade sem semente;
- cultura corrente;
- snapshots extensos sem justificativa.

## 3. Execução

Suíte completa:

```bash
dotnet test TicTacToe.sln --configuration Release
```

Cobertura:

```powershell
.\scripts\test-coverage.ps1
```

```bash
./scripts/test-coverage.sh
```

Os relatórios são gerados em `artifacts/coverage/` e não são versionados.
