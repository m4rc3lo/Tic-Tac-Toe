# Revisão geral da versão v1.8.0

## 1. Escopo e estado verificado

A revisão foi realizada sobre o repositório real fornecido, com `main` limpo e a tag `v1.8.0` apontando para o commit de integração da exportação CSV. O usuário informou 169 testes aprovados e quatro warnings (`CS1573` e `CA1416`).

## 2. Síntese executiva

A arquitetura apresenta boa separação entre `Domain`, `Application`, `AI`, `Persistence`, `Presentation` e `Audio`. O domínio está adequadamente isolado de Console, JSON, CSV, atrasos e áudio. A suíte de 169 testes fornece uma base sólida para a continuação incremental.

A versão, entretanto, ainda possui lacunas de integração: configurações alteradas na tela não são persistidas, áudio não é recomposto após alteração, Strategy e semente padrão do JSON não alimentam a configuração da partida, e os exportadores CSV não possuem fluxo de usuário. Essas lacunas não invalidam `v1.8.0`, mas devem ser resolvidas antes da consolidação `v2.0.0`.

## 3. Correções imediatas incluídas

1. documentação XML dos parâmetros `audio_enabled` e `animation_delay_milliseconds`;
2. guarda explícita de Windows antes de `Console.Beep`;
3. fallback de créditos usando a versão real do assembly;
4. cópia de `CITATION.cff` para o diretório de saída;
5. data de `v1.8.0` alinhada para 18 de julho de 2026;
6. consolidação do bloco `v1.8.0` no changelog;
7. atualização de documentos que ainda descreviam arquitetura e roadmap anteriores.

## 4. Achados prioritários

### 4.1 Alta prioridade

**Configurações da tela não são persistidas.** `SettingsScreen` modifica somente `PresentationPreferences`. `ISettingsRepository.save` não é chamado e as alterações desaparecem ao reiniciar.

**Áudio não reage à alteração durante a sessão.** `IAudioService` é selecionado uma única vez em `Program.Main`. Alternar `AudioEnabled` depois disso não troca o serviço ativo.

**Strategy e semente padrão estão sem integração.** `ApplicationSettings.DefaultStrategy` e `RandomSeed` são validados e carregados, mas `MatchSetupScreen` sempre inicia sem usar esses valores.

**Falha de persistência pode encerrar a aplicação após a partida.** `ConsoleMatchSessionRunner` propaga exceções de `IMatchPersistenceService`. A partida já terminou, mas uma falha de disco pode interromper a navegação. Recomenda-se uma fronteira de erro com mensagem e continuidade.

**JSON inválido do histórico pode causar perda silenciosa.** Os repositórios retornam coleção vazia em erro de desserialização. A próxima gravação pode substituir o arquivo corrompido sem preservá-lo para diagnóstico. Recomenda-se renomear para `.corrupt-<timestamp>` antes da recuperação.

### 4.2 Média prioridade

**Consistência entre dois arquivos não é transacional contra queda do processo.** O rollback cobre exceção durante a execução, mas não queda entre `matches.json` e `statistics.json`. Uma abordagem robusta é tratar o histórico como fonte de verdade e regenerar estatísticas na inicialização.

**Imutabilidade é superficial.** Os records usam `IReadOnlyList`, mas recebem arrays mutáveis. Consumidores que preservem a referência concreta podem alterar os elementos. Cópias defensivas ou coleções imutáveis reforçariam o contrato.

**Limite global de 1000 transições.** `ScreenManager` usa o limite também no uso interativo legítimo. Uma sessão muito longa pode ser encerrada como falso positivo. O limite deveria ser opcional em produção e reduzido apenas nos testes.

**Fallback de áudio captura qualquer `Exception`.** Isso atende à continuidade, mas também pode ocultar defeitos de programação. Recomenda-se capturar exceções operacionais conhecidas e registrar diagnóstico opcional.

**CSV ainda não está acessível pela navegação.** Os exportadores e o serviço existem, mas não há tela/comando que carregue histórico e gere os arquivos no diretório configurado.

### 4.3 Baixa prioridade

- `Program.Main` concentra muitas construções e tende a crescer; uma classe de composição reduziria o acoplamento do ponto de entrada.
- `CitationMetadataLoader` implementa um parser deliberadamente simples de CFF/YAML; é suficiente para o arquivo atual, mas não geral.
- telas provisórias de estatísticas e experimentos estão coerentes com o roadmap, mas devem ser claramente marcadas como placeholders.

## 5. Testes e qualidade

Pontos fortes:

- cobertura de regras e invariantes do domínio;
- Strategies testadas de forma determinística;
- testes de Console com `TextReader` e `TextWriter`;
- persistência e CSV testados em diretórios temporários;
- animações sem espera real;
- áudio testado sem dispositivo físico.

Lacunas recomendadas:

- integração `SettingsScreen -> ISettingsRepository`;
- recomposição dinâmica do áudio;
- uso de Strategy/semente padrão;
- falha de persistência sem encerramento da navegação;
- recuperação com backup de JSON corrompido;
- exportação CSV completa via `CsvExportService`;
- teste de publicação verificando `CITATION.cff`;
- teste de composição da aplicação com serviços simulados.

## 6. Documentação

A documentação é extensa e possui bons diagramas Mermaid contextualizados. As principais inconsistências encontradas eram históricas: roadmap de `docs/00`, estado de implementação de `docs/04`, propriedade de Strategy em `docs/06`, limitação de áudio em `docs/15` e data de `v1.8.0`. O patch corretivo atualiza esses pontos.

## 7. Conclusão

A versão `v1.8.0` é uma base tecnicamente consistente para avançar. Recomenda-se aplicar primeiro o patch corretivo de warnings e documentação. Antes dos prompts de experimentação e consolidação, as prioridades funcionais são: persistir configurações alteradas, tornar áudio/configuração reativos, usar Strategy e semente padrão, proteger a navegação contra falhas de persistência e integrar a exportação CSV à interface.
