# Roteiro incremental revisado após a versão v1.8.0

## 1. Convenção de numeração dos artefatos

A numeração dos arquivos de patch acompanha exclusivamente os prompts
incrementais funcionais.

- Prompt 01 → `0001-...`
- Prompt 02 → `0002-...`
- ...
- Prompt 20 → `0020-...`
- Prompt 21 → `0021-...`

As exceções históricas anteriores à versão `v1.8.0` permanecem registradas no
histórico do projeto, mas não alteram a sequência futura. A partir do Prompt 21,
o prefixo do patch volta a coincidir com o número do prompt.

Revisões intermediárias, auditorias, correções emergenciais e análises não
devem utilizar a sequência numérica dos prompts. Esses arquivos deverão usar
nomes descritivos, por exemplo:

```text
revisao-geral-v1.8.0.md
correcao-revisao-geral-v1.8.0.patch
auditoria-persistencia-experimental.md
```

## 2. Estado atual consolidado

Os Prompts 01 a 20 foram implementados, integrados e versionados. A versão
pública atual é `v1.8.0`.

O estado consolidado inclui:

- solução .NET 9;
- domínio com `Board`, `Move`, `Match`, `GameRules` e objetos de valor;
- `Match` como limite de consistência;
- exposição somente para leitura de `Match.Board`;
- Strategies `Random`, `Heuristic` e `Minimax`;
- resolução de Strategy fora do domínio;
- aplicação Console com adaptadores testáveis;
- máquina de estados de apresentação;
- temas ASCII e Unicode;
- feedback visual e animações com atrasos injetáveis;
- serviços de áudio com fallback;
- configurações JSON;
- histórico de partidas e estatísticas em JSON;
- exportação CSV;
- créditos baseados em `CITATION.cff`;
- testes automatizados sem Console físico, espera real ou dispositivo de áudio;
- documentação arquitetural e operacional atualizada.

A revisão geral posterior ao Prompt 20 também corrigiu:

- warnings `CS1573` e `CA1416`;
- cópia de `CITATION.cff` para o diretório de saída;
- versão de fallback da tela de créditos;
- datas e consolidação do `CHANGELOG.md`;
- documentos que descreviam arquitetura anterior à `v1.8.0`.

## 3. Situação dos prompts concluídos

| Faixa | Situação | Versões principais |
|---|---|---|
| Prompts 01–04 | Concluídos | `v1.0.0` e `v1.1.0` |
| Prompts 05–08 | Concluídos | `v1.2.0` |
| Prompts 09–10 | Concluídos | `v1.3.0` e `v1.4.0` |
| Prompts 11–12 | Concluídos | `v1.4.0` e `v1.5.0` |
| Prompts 13–16 | Concluídos | `v1.6.0` |
| Prompt 17 | Concluído | `v1.7.0` |
| Prompts 18–20 | Concluídos | `v1.8.0` |

## 4. Decisões arquiteturais obrigatórias

Os prompts restantes devem respeitar as seguintes decisões:

1. `Domain` não depende de `AI`, `Presentation`, `Persistence` ou `Audio`;
2. `ComputerPlayer` não armazena diretamente uma Strategy;
3. a associação entre participante computacional e Strategy pertence à camada
   externa ao domínio;
4. Strategies recebem `IReadOnlyBoard`;
5. `Match` mantém o `Board` mutável como detalhe privado;
6. simulações de IA não modificam o tabuleiro original;
7. atrasos, animações e áudio não entram em `Domain`, `AI` ou
   `MatchController`;
8. conversões entre entidades e registros persistentes ocorrem fora do domínio;
9. interfaces existentes não devem ser recriadas;
10. falhas de infraestrutura devem ser distinguidas de erros de domínio;
11. dados corrompidos não devem ser sobrescritos silenciosamente sem estratégia
    de recuperação ou preservação;
12. configurações alteradas pela interface devem ser efetivamente persistidas;
13. mudanças de áudio e efeitos devem produzir comportamento coerente durante a
    sessão, sem exigir reinicialização desnecessária;
14. `DefaultStrategy` e `RandomSeed` devem ser aplicados pela configuração de
    partida e pelos modos automáticos;
15. o composition root deve ser progressivamente decomposto quando crescer;
16. cada etapa deve terminar compilável, testada, documentada e pronta para
    merge.

---

# Prompts pendentes revisados

## Prompt 21 — Modo automático demonstrativo e integração das configurações

```text
Implemente IA contra IA em modo demonstrativo, reutilizando Match,
MatchController, IComputerMoveStrategyResolver e as Strategies existentes.

Antes de iniciar o modo automático, complete a integração das configurações
necessárias ao fluxo:

- aplicar ApplicationSettings.DefaultStrategy como seleção inicial;
- aplicar ApplicationSettings.RandomSeed quando nenhuma semente específica for
  informada;
- permitir escolher Strategy de X e Strategy de O;
- persistir alterações realizadas em SettingsScreen por ISettingsRepository;
- tornar a habilitação de áudio e efeitos visuais observável durante a sessão,
  sem reconstruir regras de domínio;
- evitar ampliar Program.Main com lógica de negócio ou navegação.

Inclua no modo demonstrativo:

- tela de configuração própria;
- identificação das Strategies;
- semente efetivamente utilizada;
- renderização;
- atraso configurável;
- pausa ou cancelamento seguro;
- retorno ao menu;
- opção explícita para persistir ou não a partida demonstrativa no histórico.

Não implemente lógica paralela de partida. A execução deve continuar usando
Match e MatchController.

A interrupção deve ser abstraída por um contrato testável, sem depender
diretamente de Console.KeyAvailable nos testes.

Crie testes com:

- saída simulada;
- atraso imediato;
- Strategy e semente padrão;
- cancelamento controlado;
- retorno ao menu;
- persistência e não persistência conforme a opção;
- atualização e gravação das configurações;
- áudio reativo sem dispositivo físico.
```

**Branch:** `feat/automatic-mode`  
**Prefixo do patch:** `0021`

### Justificativa da revisão

Este prompt absorve os pontos da revisão da `v1.8.0` que são pré-requisitos do
modo automático: Strategy padrão, semente padrão, persistência da tela de
configurações e reação do áudio durante a sessão.

---

## Prompt 22 — Modo experimental

```text
Implemente ExperimentController para confrontos em lote.

O modo experimental não deve utilizar:

- renderização;
- animações;
- áudio;
- atrasos;
- ScreenManager;
- histórico normal de partidas, salvo quando explicitamente solicitado.

Configure:

- Strategy de X;
- Strategy de O;
- quantidade de partidas;
- alternância dos símbolos;
- alternância do primeiro participante;
- semente base;
- política determinística de sequência de sementes;
- versão da aplicação;
- diretório de saída;
- comportamento diante de falha individual.

Colete, por execução:

- identificador do experimento;
- número da execução;
- Strategy de X;
- Strategy de O;
- semente efetivamente utilizada;
- resultado;
- quantidade de jogadas;
- duração;
- estados avaliados, quando disponíveis;
- falha e mensagem de falha;
- versão da aplicação.

Defina um contrato opcional de métricas de busca para Strategies que forneçam
estados avaliados, sem acoplar ExperimentController a MinimaxMoveStrategy.

Exporte JSON e CSV por interfaces de persistência. A saída deve ser gravada com
arquivo temporário seguido de substituição. Falhas em uma execução não devem
invalidar resultados anteriores do lote.

Crie testes reproduzíveis, sem tempo real e sem arquivos permanentes, cobrindo:

- sequência de sementes;
- alternância;
- agregação;
- captura de falhas;
- métricas opcionais;
- JSON e CSV;
- ausência de dependências de apresentação.
```

**Branch:** `feat/experiment-mode`  
**Prefixo do patch:** `0022`

### Ajustes em relação ao roteiro anterior

O experimento passa a distinguir explicitamente os resultados experimentais do
histórico normal e a obter métricas por contrato, evitando verificações de tipo
concreto da Strategy.

---

## Prompt 23 — Documentação experimental

```text
Crie docs/11-experimentacao.md com:

- pergunta de pesquisa;
- hipóteses;
- variáveis independentes;
- variáveis dependentes;
- variáveis controladas;
- unidade experimental;
- cenários;
- quantidade de execuções;
- alternância de símbolos e primeiro participante;
- política de sementes;
- métricas;
- tratamento de falhas;
- procedimento;
- formatos JSON e CSV;
- plano de análise;
- gráficos previstos;
- ameaças à validade;
- instruções de reprodução.

A documentação deve refletir exatamente ExperimentController,
ExperimentMetricRecord e os nomes reais das colunas CSV.

Inclua:

- fluxograma Mermaid;
- diagrama de sequência Mermaid;
- diagrama do fluxo de dados entre execução, agregação e exportação.

Todo diagrama deve possuir parágrafo interpretativo antes e depois.
```

**Branch:** `docs/experiment-plan`  
**Prefixo do patch:** `0023`

---

## Prompt 24 — Robustez das fronteiras externas

```text
Revise sistematicamente:

- entrada de Console;
- arquivos e diretórios;
- configurações JSON;
- histórico e estatísticas JSON;
- exportação CSV;
- áudio;
- codificação;
- créditos;
- interrupção do modo automático;
- persistência dos resultados experimentais.

Implemente:

- mensagens claras e contextualizadas;
- fallback seguro;
- distinção entre erro de domínio e erro de infraestrutura;
- ausência de exceções usadas como fluxo normal;
- preservação ou quarentena de JSON corrompido antes da substituição;
- recuperação segura de configuração inválida;
- proteção da navegação contra falhas de persistência;
- consistência recuperável entre matches.json e statistics.json;
- exceções de áudio limitadas a falhas operacionais esperadas;
- diagnóstico sem exposição de dados sensíveis.

Falhas ao salvar uma partida não devem encerrar inesperadamente o
ScreenManager. A apresentação deve informar o problema e permitir retorno ao
menu, preservando a causa para diagnóstico.

Revise o limite de transições do ScreenManager para que ele detecte ciclos em
testes sem impor um limite arbitrário à utilização legítima da aplicação.

Crie testes de falhas controladas para cada fronteira.
```

**Branch:** `fix/external-boundaries`  
**Prefixo do patch:** `0024`

### Achados da revisão cobertos

Este prompt deve resolver explicitamente:

- JSON corrompido sobrescrito;
- falha de persistência propagada pela navegação;
- consistência entre histórico e estatísticas;
- captura excessivamente ampla no fallback de áudio;
- limite global de transições do ScreenManager.

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
- execução sem terminal interativo;
- caminhos;
- separadores;
- permissões;
- diretórios de dados e exportação;
- CITATION.cff no diretório publicado;
- codificação UTF-8 sem BOM;
- publicação autocontida;
- cancelamento do modo automático.

Implemente um modo de compatibilidade que desative automaticamente recursos
inadequados quando a saída estiver redirecionada ou o terminal não oferecer
capacidade suficiente.

Não presuma suporte com base apenas no sistema operacional. Separe detecção de
plataforma de detecção de capacidade.

Crie testes com detectores de ambiente injetáveis e atualize
docs/14-limitacoes.md.
```

Corrigir warning:
```
\MatchConfiguration.cs(11,10): error CS1573: Parameter 'RandomSeed' has no matching param tag in the XML comment for 'MatchConfiguration.MatchConfiguration(string, StrategyKind, int?)' (but other parameters do)
```

**Branch:** `test/cross-platform`  
**Prefixo do patch:** `0025`

---

## Prompt 26 — Configuração de publicação

```text
Configure dotnet publish para:

- win-x64 dependente do framework;
- win-x64 autocontido;
- linux-x64 dependente do framework;
- linux-x64 autocontido.

Garanta que a publicação inclua, quando necessário:

- CITATION.cff;
- arquivos de configuração padrão;
- documentação mínima de execução;
- diretórios criados somente em runtime;
- nenhum histórico, estatística ou resultado local.

Documente:

- requisitos;
- comandos;
- diretórios gerados;
- execução;
- tamanho aproximado medido;
- diferenças entre modos;
- localização de dados por plataforma;
- permissões;
- atualização sem perda de dados;
- limitações de single-file, trimming e ReadyToRun, quando aplicáveis.

Não versione binários. Confirme o .gitignore e crie testes ou verificações de
empacotamento para os arquivos obrigatórios.
```

```
vamos criar mais alguns arquivos de documentação em /docs (um clossário termos gerais, termos tecnicos e termos de domínio de aplicação; um arquivo com esclarecimento sobre uso de IA genertativa, descreva com texto e diagramas o nosso fluxo aqui; atualize o readme na raiz do projeto com mais expiclações, uma tabela com links e descrição para cada arquivo de documetação, gostaria de uma art ascii para este readme também)
Vamos atualizar o 
```

**Branch:** `chore/publish-config`  
**Prefixo do patch:** `0026`

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
- separação entre Domain, AI, Application, Presentation, Persistence e Audio;
- crescimento do composition root;
- contratos de métricas experimentais;
- fluxo de persistência;
- navegação.

Confirme especificamente:

- ausência de Domain → AI;
- Match.Board somente para leitura;
- Strategies sem alteração do estado original;
- adaptadores externos fora do domínio;
- ausência de JSON e CSV no domínio;
- ausência de atrasos no domínio, AI e MatchController;
- associação entre participante computacional e Strategy fora do domínio;
- composição de dependências em ponto único;
- telas sem referências diretas entre si.

Extraia fábricas ou módulos de composição quando Program.Main estiver
concentrando responsabilidades excessivas. Não introduza um contêiner de
injeção de dependência externo.

Revise também a imutabilidade dos registros persistentes. Evite expor arrays
mutáveis como IReadOnlyList sem cópia defensiva.

Atualize diagramas para refletir o código real e melhore comentários XML quando
necessário.
```

**Branch:** `refactor/architecture-review`  
**Prefixo do patch:** `0027`

---

## Prompt 28 — Cobertura e qualidade dos testes

```text
Execute toda a suíte, gere relatório de cobertura e identifique lacunas.

Adicione testes para:

- domínio;
- regras;
- agregado;
- Strategies;
- resolução de Strategy;
- fluxo de aplicação;
- apresentação;
- configuração reativa;
- persistência;
- recuperação de arquivos corrompidos;
- CSV;
- modo automático;
- experimentação;
- falhas externas;
- compatibilidade;
- publicação.

Evite:

- tempo real;
- Console físico;
- dispositivo de áudio;
- ordem global;
- arquivos permanentes;
- caminhos fixos;
- valores pseudoaleatórios não controlados;
- dependência da cultura local;
- snapshots frágeis sem justificativa.

Classifique testes como unitários, integração local ou validação de publicação.
Documente estratégia, cobertura observada e limitações em docs/12-testes.md.

Não estabeleça uma porcentagem de cobertura como objetivo isolado. Priorize
ramos críticos, invariantes e fronteiras externas.
```

**Branch:** `test/coverage-review`  
**Prefixo do patch:** `0028`

---

## Prompt 29 — Experimento de referência

```text
Execute um experimento de referência com todas as combinações disponíveis de
Strategies.

Inclua:

- alternância de X e O;
- alternância do primeiro participante;
- sementes registradas;
- política de sementes documentada;
- número de repetições justificado;
- versão da aplicação;
- commit ou tag;
- sistema operacional;
- runtime .NET;
- processador, quando disponível;
- duração;
- métricas por partida;
- métricas agregadas;
- falhas.

Use apenas o modo experimental sem animação, áudio ou atraso.

Exporte JSON e CSV. Valide os esquemas antes da análise.

Produza docs/13-resultados.md com:

- protocolo executado;
- tabelas;
- gráficos;
- interpretação;
- limitações;
- ameaças à validade;
- instruções de reprodução;
- hashes ou identificação dos arquivos de resultados.

Não versione dados excessivamente grandes. Defina quais resultados pequenos e
reprodutíveis pertencem ao repositório e quais devem ser anexados à release.
```

**Branch:** `experiment/reference-run`  
**Prefixo do patch:** `0029`

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
- índice dos documentos;
- documentos em docs;
- comentários XML;
- esquemas JSON e CSV;
- instruções de publicação e experimento.

Confirme:

- dependências efetivamente utilizadas;
- atribuições;
- modificações sobre o legado;
- política de idioma;
- versões;
- datas;
- comandos;
- branches e tags;
- nomes reais de arquivos;
- nomes reais de classes;
- consistência entre diagramas e código;
- consistência entre CITATION.cff e assembly;
- inclusão de CITATION.cff na publicação;
- ausência de referências a versões ou arquiteturas antigas.

Crie ou atualize um índice da documentação e uma matriz que relacione cada
documento à versão e ao componente correspondente.
```

Adicione a documentação, onde for pertinente, a questão de arquivo bloqueado pelo dropbox, dos arquivos em appdata e de comandos de manipulação de arquivo no powershell.

**Branch:** `docs/final-review`  
**Prefixo do patch:** `0030`

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
- documentação de publicação;
- documentação experimental;
- índice de documentação.

Execute:

- restore;
- build Release;
- test;
- relatório de cobertura;
- verificação de warnings;
- verificação de whitespace;
- verificação de arquivos ignorados;
- auditoria de dados locais;
- publicações de validação;
- execução curta do modo automático;
- experimento curto reproduzível;
- validação dos arquivos JSON e CSV gerados.

Confirme ausência de:

- dados temporários;
- dados pessoais;
- binários;
- segredos;
- arquivos locais;
- resultados experimentais não intencionais;
- sementes não registradas;
- caminhos absolutos;
- warnings não justificados.

A tag somente deve ser criada após todas as verificações e após a aprovação da
documentação.
```

**Branch:** `release/v1.9.0`  
**Prefixo do patch:** `0031`

---

## Prompt 32 — Consolidação da versão v2.0.0

```text
Prepare a versão 2.0.0 como conclusão da refatoração.

Antes da mudança de versão, confirme que não existem itens funcionais planejados
para depois da release. A versão 2.0.0 deve representar a arquitetura e os
fluxos documentados, não apenas uma alteração nominal.

Atualize:

- versão;
- CHANGELOG.md;
- CITATION.cff;
- README.md;
- timeline;
- documentação;
- índice;
- limitações;
- instruções de instalação;
- execução;
- configuração;
- modo automático;
- modo experimental;
- exportação;
- publicação.

Execute:

- toda a suíte;
- cobertura;
- build sem warnings;
- publicações suportadas;
- teste de atualização preservando dados;
- experimento curto reproduzível;
- validação multiplataforma disponível;
- auditoria de arquivos;
- auditoria legal;
- auditoria de dados e privacidade;
- verificação de links e comandos.

Produza instruções para:

- commit de release;
- merge;
- tag anotada v2.0.0;
- push da tag;
- GitHub Release;
- checksums dos artefatos;
- anexação opcional de publicações;
- anexação opcional dos resultados de referência;
- notas de migração da v1.9.0.
```

**Branch:** `release/v2.0.0`  
**Prefixo do patch:** `0032`

---

# 5. Sequência recomendada imediata

A próxima sequência deve ser:

1. confirmar que as correções da revisão da `v1.8.0` foram integradas;
2. confirmar build Release, testes e warnings;
3. iniciar o Prompt 21 em `feat/automatic-mode`;
4. nomear o patch do Prompt 21 com prefixo `0021`;
5. integrar no Prompt 21 a persistência das configurações, Strategy/semente
   padrão e comportamento reativo do áudio;
6. manter revisões intermediárias com nomes descritivos, sem numeração;
7. criar `v1.9.0` somente no Prompt 31.

# 6. Critério de conclusão de cada prompt

Cada prompt somente será considerado concluído quando houver:

- branch correta;
- patch com a numeração correspondente, exceto correções intermediárias;
- código compilável;
- testes aprovados;
- warnings eliminados ou formalmente justificados;
- `git diff --check` sem erros;
- documentação XML atualizada;
- documentação Markdown atualizada;
- diagramas Mermaid com parágrafo antes e depois;
- `CHANGELOG.md` atualizado;
- versão ajustada somente quando prevista;
- arquivos temporários ausentes;
- nenhum dado local ou pessoal incluído;
- commit em português do Brasil;
- merge `--no-ff`;
- branch principal funcional.
