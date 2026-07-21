# Matriz da documentação

## 1. Finalidade

A matriz relaciona cada documento ao marco de versão e ao componente que ele
descreve. `Unreleased` identifica conteúdo implementado depois de `v1.9.0` e
ainda não associado a uma tag formal.

| Documento | Versão ou período | Componente | Finalidade |
|---|---|---|---|
| `00-decisoes-e-escopo.md` | 1.2.0+ | Governança | convenções, Git, idioma e versões |
| `01-projeto-original.md` | 1.0.0 | Legado | inventário histórico |
| `02-requisitos.md` | 1.2.0+ | Projeto | requisitos e critérios |
| `03-arquitetura.md` | 1.2.0–Unreleased | Arquitetura | camadas e composition root |
| `04-modelo-conceitual.md` | 1.2.0 | Domain | entidades e relações |
| `05-game-rules.md` | 1.2.0 | Domain | regras e avaliação |
| `06-match-aggregate.md` | 1.2.0 | Domain | agregado e invariantes |
| `07-fluxo-aplicacao.md` | 1.4.0 | Application | controlador e portas |
| `08-revisao-prompt-10.md` | 1.3.0 | Governança | revisão histórica intermediária |
| `09-correcao-fronteiras-arquiteturais.md` | 1.4.0 | Domain/AI/Application | fronteiras corrigidas |
| `10-inteligencia-artificial.md` | 1.3.0–1.5.0 | AI | Strategies e Minimax |
| `11-experimentacao.md` | Unreleased | Application/Persistence | protocolo experimental |
| `12-testes.md` | Unreleased | Tests | estratégia e cobertura |
| `13-resultados.md` | Unreleased | Experiment | experimento de referência |
| `14-limitacoes.md` | Unreleased | Compatibility | plataformas e terminal |
| `15-audio.md` | 1.7.0 | Audio | beep, bell e fallback |
| `16-apresentacao-console.md` | 1.6.0 | Presentation | adaptadores Console |
| `17-screen-manager.md` | 1.6.0–Unreleased | Presentation | navegação e ciclos |
| `18-temas-e-creditos.md` | 1.7.0–Unreleased | Presentation | ASCII, Unicode e citação |
| `19-feedback-visual-e-animacoes.md` | 1.7.0 | Presentation | feedback e atraso |
| `20-configuracoes-json.md` | 1.8.0 | Persistence | settings e esquema |
| `21-partidas-e-estatisticas-json.md` | 1.8.0–Unreleased | Persistence | histórico, estatísticas e recuperação |
| `22-exportacao-csv.md` | 1.8.0 | Persistence | CSV e esquemas |
| `23-modo-automatico.md` | Unreleased | Application/Presentation | IA contra IA demonstrativa |
| `24-modo-experimental.md` | Unreleased | Application/Persistence | lotes sem apresentação |
| `25-robustez-fronteiras-externas.md` | Unreleased | Infrastructure | falhas e recuperação |
| `26-publicacao.md` | Unreleased | Publication | perfis e empacotamento |
| `27-glossario.md` | Unreleased | Projeto | vocabulário |
| `28-uso-ia-generativa.md` | Unreleased | Governança | transparência de IA |
| `29-revisao-arquitetural-final.md` | Unreleased | Arquitetura | auditoria final |
| `30-revisao-legal-documental.md` | Unreleased | Governança/Legal | auditoria legal e documental |
| `31-matriz-documentacao.md` | 1.9.0 | Governança | rastreabilidade documental |
| `32-release-v1.9.0.md` | 1.9.0 | Release | checklist e validação da candidata |

## 2. Leitura da matriz

Documentos que atravessam várias versões registram evolução arquitetural.
Documentos históricos, como `01-projeto-original.md` e
`08-revisao-prompt-10.md`, não devem ser reescritos para parecer documentação
do estado atual; sua função é preservar rastreabilidade.

```mermaid
flowchart LR
    A[Versão ou Unreleased] --> B[Documento]
    C[Componente] --> B
    B --> D[Decisão, esquema ou procedimento]
    D --> E[Teste, código ou artefato]
```

A matriz deve ser atualizada quando um documento novo for criado ou quando uma
release mover conteúdo de `Unreleased` para uma versão numerada.
