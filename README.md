<h1 align="center">AutoHub — Concessionária & Oficina Mecânica</h1>

<p align="center">
  Projeto desenvolvido para as disciplinas de CP1, CP2 e CP3 — FIAP
</p>

---

<h2 align="center">📋 Integrantes</h2>

<table align="center">
    <tr>
        <td align="center">
            <img src="https://avatars.githubusercontent.com/u/202198493?v=4" width="100px;" alt="Matheus Roque"/>
            <br>
            <sub>
                <b>Matheus Roque</b><br>
                <b>RM: 561959</b>
            </sub>
        </td>
        <td align="center">
            <img src="https://avatars.githubusercontent.com/u/200883157?s=400&u=4c0d649624f6736e702b60244099bdf4b887eda7&v=4" width="100px;" alt="Giovane dos Santos"/>
            <br>
            <sub>
                <b>Giovane dos Santos</b><br>
                <b>RM: 561336</b>
            </sub>
        </td>
    </tr>
</table>

---

<h2 align="center">🚗 Domínio Escolhido</h2>

<p align="center">
  O domínio escolhido é o de uma <b>concessionária de veículos com oficina mecânica</b>.<br>
  O sistema contempla dois módulos integrados: venda de veículos do estoque e gestão de ordens de serviço da oficina (manutenções, serviços e peças).
</p>

---

<h2 align="center">🗂️ Entidades Modeladas</h2>

<table align="center">
    <tr>
        <th>Entidade</th>
        <th>Descrição</th>
    </tr>
    <tr><td>Marca</td><td>Fabricantes dos veículos (ex: Toyota, Honda)</td></tr>
    <tr><td>Modelo</td><td>Modelos de cada marca (ex: Corolla, Civic)</td></tr>
    <tr><td>Cliente</td><td>Clientes da concessionária e oficina mecânica</td></tr>
    <tr><td>Funcionario</td><td>Vendedores e mecânicos da empresa</td></tr>
    <tr><td>VeiculoCliente</td><td>Veículos dos clientes trazidos para reparo na oficina</td></tr>
    <tr><td>VeiculoEstoque</td><td>Veículos disponíveis para venda no pátio</td></tr>
    <tr><td>Servico</td><td>Catálogo de serviços oferecidos pela oficina</td></tr>
    <tr><td>Peca</td><td>Catálogo de peças e componentes mecânicos</td></tr>
    <tr><td>OrdemServico</td><td>Ordens de serviço abertas para manutenção</td></tr>
    <tr><td>OsServico</td><td>Tabela pivot — serviços executados em cada OS</td></tr>
    <tr><td>OsPeca</td><td>Tabela pivot — peças utilizadas em cada OS</td></tr>
    <tr><td>Venda</td><td>Registro das vendas de veículos do estoque</td></tr>
</table>

---

<h2 align="center">🔗 Relacionamentos</h2>

| Entidades                     | Cardinalidade | Observação                                       |
| ----------------------------- | ------------- | ------------------------------------------------ |
| Marca → Modelo                | 1:N           | Uma marca tem vários modelos                     |
| Modelo → VeiculoCliente       | 1:N           | Um modelo aparece em vários veículos de clientes |
| Modelo → VeiculoEstoque       | 1:N           | Um modelo aparece em vários veículos do estoque  |
| Cliente → VeiculoCliente      | 1:N           | Um cliente pode ter vários veículos na oficina   |
| Cliente → Venda               | 1:N           | Um cliente pode fazer várias compras             |
| VeiculoCliente → OrdemServico | 1:N           | Um veículo pode ter múltiplas OS                 |
| VeiculoEstoque → Venda        | 1:1           | Um veículo do estoque é vendido apenas uma vez   |
| Funcionario → OrdemServico    | 1:N           | Um mecânico atende múltiplas OS                  |
| Funcionario → Venda           | 1:N           | Um vendedor faz múltiplas vendas                 |
| OrdemServico ↔ Servico        | N:N           | Via tabela pivot OsServico                       |
| OrdemServico ↔ Peca           | N:N           | Via tabela pivot OsPeca                          |

---

<h2 align="center">📐 MER</h2>

<p align="center">
  <img src="docs/MerDiagram.png" alt="Diagrama MER" width="900px"/>
</p>

---

<h2 align="center">🏗️ Estrutura do Projeto (Clean Architecture)</h2>

```
AutoHub/
├── docs/
│   └── MerDiagram.png
├── src/
│   ├── AutoHub.Domain/            → Regras de negócio puras, entidades e exceções de domínio
│   │   ├── Common/                → BaseEntity com Id Guid
│   │   ├── Entities/              → Marca, Modelo, Cliente, VeiculoEstoque, etc.
│   │   └── Exceptions/            → DomainException, ResourceNotFoundException, ConflictException
│   ├── AutoHub.Application/       → Contratos de repositório, DTOs e serviços de aplicação
│   │   ├── DTOs/                  → Request e Response DTOs para Clientes, Marcas, Veículos
│   │   ├── Interfaces/            → IRepository<T>, IClienteService, IMarcaService, etc.
│   │   └── Services/              → ClienteService, MarcaService, VeiculoEstoqueService
│   ├── AutoHub.Infrastructure/    → EF Core, SQLite, DbContext, Migrations e Repositórios
│   │   ├── Data/                  → ApplicationDbContext, Configurations (Fluent API), DataSeeder
│   │   └── Repositories/          → Repository<T> genérico
│   └── AutoHub.API/               → Controllers, Swagger OpenAPI com XML, Exception Handler
│       ├── Controllers/           → ClientesController, MarcasController, VeiculosEstoqueController, SeedController
│       ├── Exceptions/            → GlobalExceptionHandler (IExceptionHandler + RFC 7807)
│       └── Extensions/            → SwaggerExtensions
└── README.md
```

---

<h2 align="center">🚀 Como Executar o Projeto</h2>

### Pré-requisitos
- **.NET 9 SDK** instalado.
- O banco de dados utilizado é o **SQLite** (banco local em arquivo `autohub.db`), totalmente autocontido e sem necessidade de instalar SGBDs externos.

### Execução passo a passo
```bash
# 1. Restaurar dependências da solution
dotnet restore

# 2. Executar o projeto da API
dotnet run --project src/AutoHub.API
```

A API estará disponível em:
- **URL Base:** `http://localhost:5017`
- **Swagger UI:** `http://localhost:5017/swagger`

> **Nota:** As migrations do banco de dados são aplicadas automaticamente no início da aplicação (`db.Database.Migrate()`).

---

<h2 align="center">📌 CP3 — Evolução da API, Swagger e Tratamento Global</h2>

### 1. Controllers e DTOs (Clean Architecture)
A camada **API** não acessa o `DbContext` diretamente. Toda a comunicação ocorre via **DTOs de entrada e saída**, passando por serviços de aplicação e pelo repositório genérico `IRepository<T>`:

| Recurso | Método | Rota | Descrição |
| --- | --- | --- | --- |
| **Seed** | POST | `/api/seed` | Popula o banco com dados de exemplo |
| **Clientes** | GET | `/api/clientes` | Lista todos os clientes |
| **Clientes** | GET | `/api/clientes/{id}` | Busca cliente por ID (GUID) |
| **Clientes** | POST | `/api/clientes` | Cria um novo cliente com validação de CPF e campos |
| **Clientes** | PUT | `/api/clientes/{id}` | Atualiza dados de um cliente existente |
| **Clientes** | DELETE | `/api/clientes/{id}` | Remove um cliente |
| **Marcas** | GET | `/api/marcas` | Lista todas as marcas |
| **Marcas** | GET | `/api/marcas/{id}` | Busca marca por ID (GUID) |
| **Marcas** | POST | `/api/marcas` | Cadastra nova marca de veículos |
| **Marcas** | PUT | `/api/marcas/{id}` | Atualiza dados da marca |
| **Marcas** | DELETE | `/api/marcas/{id}` | Remove uma marca |
| **Veículos Estoque** | GET | `/api/veiculos-estoque` | Lista veículos à venda |
| **Veículos Estoque** | GET | `/api/veiculos-estoque/{id}` | Busca veículo em estoque por ID |
| **Veículos Estoque** | POST | `/api/veiculos-estoque` | Cadastra veículo validando modelo e chassi |
| **Veículos Estoque** | PUT | `/api/veiculos-estoque/{id}` | Atualiza veículo em estoque |
| **Veículos Estoque** | DELETE | `/api/veiculos-estoque/{id}` | Remove veículo do estoque |
| **Health** | GET | `/health` | Verificação de disponibilidade da aplicação |

---

### 2. Swagger / OpenAPI Completo
- **Metadados:** Configurados com título corporativo, versão `v1`, descrição e informações de contato.
- **Comentários XML:** Ativados via `<GenerateDocumentationFile>true</GenerateDocumentationFile>` no projeto `AutoHub.API` e carregados via `IncludeXmlComments`.
- **Respostas Tipadas:** Cada action possui atributos `[ProducesResponseType]` para respostas de sucesso (`200 OK`, `201 Created`, `204 NoContent`) e respostas de erro (`400 BadRequest`, `404 NotFound`, `409 Conflict`).

---

### 3. Repositório Genérico Tipado
- Interface `IRepository<T>` localizada na camada **Application**, com restrição `where T : BaseEntity`.
- Implementação `Repository<T>` na camada **Infrastructure** utilizando `ApplicationDbContext` com:
  - `GetAllAsync()` (com `.AsNoTracking()` para otimização de leitura)
  - `GetByIdAsync(Guid id)`
  - `ExistsByIdAsync(Guid id)`
  - `FirstOrDefaultAsync(predicate)`
  - `FindAsync(predicate)`
  - `AddAsync(entity)`, `Update(entity)`, `Delete(entity)`
  - `SaveChangesAsync()`
- Injeção de dependência registrada no `Program.cs`:
  ```csharp
  builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
  ```

---

### 4. Tratamento Global de Erros (GlobalExceptionHandler)
- Implementação da interface nativa `IExceptionHandler` do ASP.NET Core (.NET 9) em `AutoHub.API/Exceptions/GlobalExceptionHandler.cs`.
- Retorno padronizado no formato **RFC 7807** (`application/problem+json`, `ProblemDetails`).
- Registro no pipeline do `Program.cs` com `app.UseExceptionHandler()` antes dos controllers.
- Registro estruturado com `ILogger` e identificador de rastreamento (`traceId: HttpContext.TraceIdentifier`).
- Em ambiente de produção, detalhes sensíveis de stack trace e banco de dados **não** são expostos.

#### Tabela de Mapeamento de Exceções:
| Exceção | Status HTTP | RFC 7807 Title | Cenário |
| --- | --- | --- | --- |
| `ResourceNotFoundException` | **404 Not Found** | Recurso não encontrado | ID buscado não existe no banco |
| `KeyNotFoundException` | **404 Not Found** | Recurso não encontrado | Chave informada inexistente |
| `ConflictException` | **409 Conflict** | Conflito de dados | Duplicidade de CPF, Chassi ou Nome único |
| `DomainException` | **400 Bad Request** | Violação de regra de negócio | Regra de negócio ou invariante violada |
| `ArgumentException` | **400 Bad Request** | Parâmetro inválido | Argumentos incorretos fornecidos |
| `Exception` (inesperada) | **500 Internal Server Error** | Erro interno no servidor | Falhas não tratadas (stack oculta fora de Dev) |

#### Exemplo de Resposta de Erro (RFC 7807):
```json
{
  "type": "about:blank",
  "title": "Recurso não encontrado",
  "status": 404,
  "detail": "Cliente com identificador '00000000-0000-0000-0000-000000000000' não foi encontrado(a).",
  "instance": "/api/clientes/00000000-0000-0000-0000-000000000000",
  "traceId": "0HNODRHBMK48B:00000001"
}
```

---

<h2 align="center">🎯 Preparação para o CP4</h2>

A arquitetura do CP3 já foi desenhada para a transição direta para o **CP4**:
1. **Application Services Desacoplados (`ClienteService`, `VeiculoEstoqueService`, etc.):**
   - Injetam apenas `IRepository<T>`, prontos para testes unitários com **Moq** no projeto `AutoHub.Application.Tests` (verificando chamadas `Times.Never` em falha e `Times.Once` em sucesso).
2. **Entidades Ricas com Invariantes de Domínio:**
   - Métodos `Validar()` com regras reais (tamanho de CPF, anos válidos, preços positivos), prontos para testes no `AutoHub.Domain.Tests` com **xUnit** (`[Fact]` para caminho feliz e `[Theory]` + `[InlineData]` para exceções de domínio).
3. **Observabilidade Estruturada com Logs:**
   - Controllers e `GlobalExceptionHandler` já utilizam logging estruturado com `traceId` (`HttpContext.TraceIdentifier`).
