# ASCII art, Unicode, temas e créditos

## 1. Finalidade

Esta etapa amplia exclusivamente a camada de apresentação. Preferências visuais,
artes textuais e capacidades do terminal não são conhecidas pelo domínio, pela
inteligência artificial ou pelo controlador de partidas.

Também foi incluída uma tela de créditos disponível no menu principal. Seus
dados são derivados dos campos relevantes de `CITATION.cff`, com fallback
seguro quando o arquivo não está disponível no diretório de execução.

## 2. Configuração visual

`PresentationPreferences` contém quatro opções independentes:

- cores ANSI;
- caracteres Unicode;
- limpeza de tela;
- efeitos visuais.

O diagrama apresenta como a configuração chega aos adaptadores.

```mermaid
classDiagram
    class PresentationPreferences {
        +bool UseAnsiColors
        +bool UseUnicode
        +bool ClearScreen
        +bool VisualEffects
    }

    class ConsoleTheme {
        +PresentationPreferences Preferences
        +string VerticalSeparator
        +string HorizontalSeparator
        +colorize_accent(text) string
        +clear(writer)
    }

    class ConsoleBoardRenderer
    class ConsoleGameOutput
    class SplashScreen
    class SettingsScreen

    ConsoleTheme *-- PresentationPreferences
    ConsoleBoardRenderer --> ConsoleTheme
    ConsoleGameOutput --> ConsoleTheme
    SplashScreen --> ConsoleTheme
    SettingsScreen --> PresentationPreferences
```

O mesmo objeto de preferências é fornecido ao tema e ao `ScreenContext`.
Alterações realizadas em `SettingsScreen` passam a valer nas renderizações
posteriores sem alterar entidades de domínio.

## 3. Renderização ASCII e Unicode

O tabuleiro possui dois modos. O modo ASCII usa `|`, `-` e `+`; o modo Unicode
usa `│`, `─` e `┼`.

```text
ASCII                 Unicode

X |   |               X │   │
--+---+--              ──┼───┼──
  | O |                  │ O │
```

Os testes comparam a saída textual integral, incluindo espaços e quebras de
linha. Isso protege o fallback ASCII e a variante Unicode contra alterações
acidentais.

## 4. Artes e cores

`AsciiArtCatalog` fornece:

- logotipo;
- vitória;
- derrota;
- empate.

Quando efeitos visuais estão desativados, as artes são omitidas. Quando cores
ANSI estão desativadas, nenhuma sequência de escape é emitida.

A limpeza de tela também é opcional. Ela usa sequências ANSI somente quando
explicitamente habilitada.

## 5. Créditos

A tela de créditos é alcançada pelo menu principal e retorna ao menu após a
leitura.

```mermaid
stateDiagram-v2
    MainMenu --> Credits : opção Créditos
    Credits --> MainMenu : continuar
```

`CitationMetadataLoader` lê título, versão, autor, licença e repositório de
`CITATION.cff`. Como a publicação pode não copiar esse arquivo, a apresentação
possui fallback com os mesmos campos essenciais.

## 6. Limitações e fallback

Nem todo terminal oferece suporte uniforme a Unicode ou ANSI. As principais
limitações são:

- fontes sem glifos Unicode podem exibir quadrados;
- terminais antigos podem imprimir códigos ANSI literalmente;
- saída redirecionada não deve receber limpeza de tela;
- largura de caracteres pode variar conforme fonte e locale;
- `CITATION.cff` pode não estar ao lado do executável publicado.

O fallback recomendado é desativar Unicode, cores, limpeza e efeitos visuais.
Nesse modo, a aplicação permanece funcional com texto e caracteres ASCII
básicos.
