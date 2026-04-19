<h1 align="center">AutoHub — Concessionária & Oficina Mecânica</h1>

<p align="center">
  Projeto desenvolvido para as disciplinas de CP1 e CP2 — FIAP
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
  O sistema contempla dois módulos distintos: vendas de veículos do estoque e gestão de ordens de serviço da oficina.
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
    <tr><td>Cliente</td><td>Clientes da concessionária e oficina</td></tr>
    <tr><td>Funcionario</td><td>Vendedores e mecânicos da empresa</td></tr>
    <tr><td>VeiculoCliente</td><td>Veículos dos clientes trazidos para reparo na oficina</td></tr>
    <tr><td>VeiculoEstoque</td><td>Veículos disponíveis para venda na concessionária</td></tr>
    <tr><td>Servico</td><td>Catálogo de serviços oferecidos pela oficina</td></tr>
    <tr><td>Peca</td><td>Catálogo de peças utilizadas nos reparos</td></tr>
    <tr><td>OrdemServico</td><td>Ordens de serviço abertas na oficina</td></tr>
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
  <img src="docs/MER.png" alt="Diagrama MER" width="900px"/>
</p>

---

<h2 align="center">🏗️ Estrutura do Projeto</h2>

```
AutoHub/
├── docs/
│   └── MER.png
├── src/
│   ├── AutoHub.API/               → Endpoints, Program.cs, appsettings
│   ├── AutoHub.Application/       → Interfaces de repositório (contratos)
│   ├── AutoHub.Domain/            → Entidades do domínio
│   │   └── Entities/
│   └── AutoHub.Infrastructure/    → DbContext, Configurations, Migrations, Repositórios
│       ├── Data/
│       │   ├── ApplicationDbContext.cs
│       │   ├── Configurations/    → Fluent API (1 arquivo por entidade)
│       │   └── Migrations/
│       └── Repositories/
└── README.md
```

---

<h2 align="center">🗄️ CP2 — Persistência com EF Core</h2>

### Banco de dados utilizado

**SQLite** — banco em arquivo, sem necessidade de instalação, totalmente reproduzível.

O arquivo `autohub.db` é gerado automaticamente na primeira execução via `db.Database.Migrate()`.

### Como rodar o projeto

**Pré-requisitos:** .NET 9 SDK instalado

```bash
# 1. Clonar o repositório
git clone <url-do-repositorio>
cd AutoHub

# 2. Restaurar dependências
dotnet restore

# 3. Rodar a API
dotnet run --project src/AutoHub.API
```

A API sobe em `http://localhost:5017` e o Swagger fica disponível em `http://localhost:5017/swagger`.

> As migrations são aplicadas automaticamente ao subir o projeto. Não é necessário rodar `dotnet ef database update` manualmente.

### Endpoints disponíveis

| Método | Rota                | Descrição                             |
| ------ | ------------------- | ------------------------------------- |
| GET    | `/health`           | Verifica se a API está rodando        |
| POST   | `/seed`             | Popula o banco com dados de exemplo   |
| GET    | `/clientes`         | Lista todos os clientes               |
| GET    | `/clientes/{id}`    | Busca um cliente pelo ID              |
| GET    | `/ordens-servico`   | Lista OS com serviços e peças (N:N)   |
| GET    | `/veiculos-estoque` | Lista veículos disponíveis para venda |

### Decisões técnicas

- **Repositório genérico** — `IRepository<T>` na camada Application, implementado por `Repository<T>` na Infrastructure, registrado via `AddScoped` no `Program.cs`
- **Fluent API** — uma classe `IEntityTypeConfiguration<T>` por entidade, aplicadas via `ApplyConfigurationsFromAssembly`
- **Índices únicos** mapeados em: `Marca.Nome`, `Cliente.Cpf`, `Funcionario.Matricula`, `VeiculoCliente.PlacaVeiculo`, `VeiculoEstoque.Chassis`
- **PKs compostas** nas tabelas pivot `OsServico` e `OsPeca` via `HasKey(x => new { x.A, x.B })`
- **Relacionamento 1:1** entre `VeiculoEstoque` e `Venda` via `HasOne/WithOne`
