# Evidências da CP5

**Estado: coleta concluída com sucesso.** A validação foi executada em
2026-10-05T01:20:37Z com banco SQLite temporário e API em porta local
temporária. Foram gerados 88 arquivos reais em `evidencias/`, incluindo
respostas HTTP/JSON, Swagger, rate limit, health check, build e testes.

## Evidências coletadas

Os arquivos em `evidencias/` registram as chamadas realizadas contra uma
instância temporária da API. Cada `.http` registra método, URL, headers enviados,
status, headers recebidos e corpo; os corpos JSON também são salvos separadamente.

| Requisito | Arquivos gerados em `evidencias/` |
| --- | --- |
| v1 como lista; query, header e URL | `v1-query.*`, `v1-header.*`, `v1-url.*` |
| v1 preservada com parâmetros de página | `v1-sem-paginacao.*` |
| v2 padrão e explícita | `v2-padrao.*`, `v2-query.*`, `v2-header.*`, `v2-url.*` |
| Headers de versões | Headers dos arquivos `v1-query.http` e `v2-padrao.http` |
| Swagger com grupos e depreciação | `swagger-v1.*`, `swagger-v2.*`, `swagger-ui.http` |
| 400 com regra inválida | `invalido-page-0.*`, `invalido-pageSize-9999.*` e outros limites |
| Duas páginas distintas e totais | `pagina-1.*`, `pagina-2.*` |
| Página além do total e overflow | `pagina-999.*`, `pagina-2147483647.*` |
| Teto de 100 aceito | `tamanho-maximo.*` |
| Escrita e busca por ID nas versões | `crud-1.0-*`, `crud-2.0-*` |
| Recursos neutros | `clientes.*`, `marcas.*`, `seed.*` e documentos Swagger |
| 429 com Retry-After e JSON | Última resposta `rate-limit-*.http` |
| Health 200 imediatamente após 429 | `health-apos-429.*` |
| Build, testes e data da execução | `build.txt`, `test.txt`, `execucao.txt` |

A execução foi revisada e os arquivos estão incluídos nesta pasta. O trecho HTML
da UI e os documentos OpenAPI são a evidência textual do Swagger permitida pelo
enunciado; uma captura de tela adicional é opcional. O build e os testes devem
ser reproduzidos com `dotnet build` e `dotnet test`.
