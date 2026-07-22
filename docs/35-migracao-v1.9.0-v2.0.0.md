# Migração da v1.9.0 para a v2.0.0

## 1. Visão geral

A versão `2.0.0` consolida a candidata `1.9.0`. Não há mudança intencionalmente
incompatível nos formatos públicos de configurações, histórico, estatísticas e
resultados experimentais produzidos pela candidata.

A alteração de versão principal representa a conclusão arquitetural da
refatoração iniciada sobre o legado, e não uma decisão de descartar dados da
v1.9.0.

## 2. Procedimento recomendado

1. encerre a aplicação v1.9.0;
2. faça backup do diretório local de dados;
3. instale ou extraia a build v2.0.0 em um novo diretório;
4. mantenha os dados no diretório local esperado pela aplicação;
5. inicie a v2.0.0;
6. confira configurações, histórico e estatísticas;
7. execute uma partida e uma exportação curta;
8. preserve o backup até concluir a verificação.

## 3. Dados que devem ser preservados

- configurações JSON;
- histórico de partidas;
- estatísticas agregadas;
- resultados experimentais escolhidos pelo usuário;
- exportações que ainda sejam necessárias.

Dados locais não devem ser copiados para o repositório Git.

## 4. Teste de atualização

O teste mínimo utiliza uma cópia temporária dos dados da v1.9.0:

```text
cópia dos dados v1.9.0
→ inicialização da v2.0.0
→ leitura das configurações
→ consulta de histórico e estatísticas
→ nova partida
→ nova exportação
→ comparação com o backup
```

Arquivos inválidos devem seguir a política de quarentena e recuperação, sem
apagamento silencioso.

## 5. Retorno à versão anterior

Se houver falha:

1. encerre a v2.0.0;
2. preserve os arquivos gerados durante o teste;
3. restaure o backup em um diretório separado;
4. execute a v1.9.0 somente sobre a cópia restaurada;
5. registre versão, plataforma, build e passos de reprodução.

## 6. Compatibilidade experimental

Resultados antigos preservam o campo `application_version` original. Eles não
devem ser reescritos como se tivessem sido gerados pela v2.0.0. Novas execuções
registram a versão `2.0.0` e podem ser comparadas desde que o protocolo, as
sementes e o ambiente sejam considerados.
