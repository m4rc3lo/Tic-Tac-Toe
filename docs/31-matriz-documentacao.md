# Matriz da documentação

## 1. Finalidade

A matriz relaciona cada documento ao marco de versão e ao componente que ele
descreve. A versão `2.0.0` consolida o conteúdo que, durante a candidata `v1.9.0`, estava identificado como `2.0.0`.

| Documento | Versão ou período | Componente | Finalidade |
|---|---|---|---|
| `00-decisoes-e-escopo.md` | 1.2.0+ | Governança | convenções, Git, idioma e versões |
| `01-projeto-original.md` | 1.0.0 | Legado | inventário histórico |
| `02-requisitos.md` | 1.2.0+ | Projeto | requisitos e critérios |
| `03-arquitetura.md` | 1.2.0–2.0.0 | Arquitetura | camadas e composition root |
| `04-modelo-conceitual.md` | 1.2.0 | Domain | entidades e relações |
| `05-game-rules.md` | 1.2.0 | Domain | regras e avaliação |
| `06-match-aggregate.md` | 1.2.0 | Domain | agregado e invariantes |
| `07-fluxo-aplicacao.md` | 1.4.0 | Application | controlador e portas |
| `08-revisao-prompt-10.md` | 1.3.0 | Governança | revisão histórica intermediária |
| `09-correcao-fronteiras-arquiteturais.md` | 1.4.0 | Domain/AI/Application | fronteiras corrigidas |
| `10-inteligencia-artificial.md` | 1.3.0–1.5.0 | AI | Strategies e Minimax |
| `11-experimentacao.md` | 2.0.0 | Application/Persistence | protocolo experimental |
| `12-testes.md` | 2.0.0 | Tests | estratégia e cobertura |
| `13-resultados.md` | 2.0.0 | Experiment | experimento de referência |
| `14-limitacoes.md` | 2.0.0 | Compatibility | plataformas e terminal |
| `15-audio.md` | 1.7.0 | Audio | beep, bell e fallback |
| `16-apresentacao-console.md` | 1.6.0 | Presentation | adaptadores Console |
| `17-screen-manager.md` | 1.6.0–2.0.0 | Presentation | navegação e ciclos |
| `18-temas-e-creditos.md` | 1.7.0–2.0.0 | Presentation | ASCII, Unicode e citação |
| `19-feedback-visual-e-animacoes.md` | 1.7.0 | Presentation | feedback e atraso |
| `20-configuracoes-json.md` | 1.8.0 | Persistence | settings e esquema |
| `21-partidas-e-estatisticas-json.md` | 1.8.0–2.0.0 | Persistence | histórico, estatísticas e recuperação |
| `22-exportacao-csv.md` | 1.8.0 | Persistence | CSV e esquemas |
| `23-modo-automatico.md` | 2.0.0 | Application/Presentation | IA contra IA demonstrativa |
| `24-modo-experimental.md` | 2.0.0 | Application/Persistence | lotes sem apresentação |
| `25-robustez-fronteiras-externas.md` | 2.0.0 | Infrastructure | falhas e recuperação |
| `26-publicacao.md` | 2.0.0 | Publication | perfis e empacotamento |
| `27-glossario.md` | 2.0.0 | Projeto | vocabulário |
| `28-uso-ia-generativa.md` | 2.0.0 | Governança | transparência de IA |
| `29-revisao-arquitetural-final.md` | 2.0.0 | Arquitetura | auditoria final |
| `30-revisao-legal-documental.md` | 2.0.0 | Governança/Legal | auditoria legal e documental |
| `31-matriz-documentacao.md` | 1.9.0 | Governança | rastreabilidade documental |
| `32-release-v1.9.0.md` | 1.9.0 | Release | checklist histórico da candidata |
| `33-analise-estatistica-experimentos.md` | 1.9.0–2.0.0 | Experiment | análise estatística avançada |
| `34-release-v2.0.0.md` | 2.0.0 | Release | consolidação e publicação final |
| `35-migracao-v1.9.0-v2.0.0.md` | 2.0.0 | Compatibility | migração e preservação de dados |
| `36-auditoria-release-v2.0.0.md` | 2.0.0 | Governança | auditoria final |

## 2. Leitura da matriz

Documentos que atravessam várias versões registram evolução arquitetural.
Documentos históricos, como `01-projeto-original.md` e
`08-revisao-prompt-10.md`, não devem ser reescritos para parecer documentação
do estado atual; sua função é preservar rastreabilidade.

```mermaid
flowchart LR
    A[Versão ou 2.0.0] --> B[Documento]
    C[Componente] --> B
    B --> D[Decisão, esquema ou procedimento]
    D --> E[Teste, código ou artefato]
```

A matriz deve ser atualizada quando um documento novo for criado ou quando uma
release mover conteúdo de `2.0.0` para uma versão numerada.
