using System.Text.Json.Serialization;
using AutoHub.Application.Interfaces;
using AutoHub.Domain.Entities;
using AutoHub.Infrastructure.Data;
using AutoHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/seed", async (ApplicationDbContext db) =>
{
    if (await db.Marcas.AnyAsync())
    {
        return Results.Ok(new { message = "Dados já foram inseridos." });
    }

    var marca1 = new Marca { Id = Guid.NewGuid(), Nome = "Toyota" };
    var marca2 = new Marca { Id = Guid.NewGuid(), Nome = "Honda" };

    var modelo1 = new Modelo { Id = Guid.NewGuid(), Nome = "Corolla", MarcaId = marca1.Id };
    var modelo2 = new Modelo { Id = Guid.NewGuid(), Nome = "Civic", MarcaId = marca2.Id };

    var cliente = new Cliente
    {
        Id = Guid.NewGuid(),
        Nome = "Ana Souza",
        Cpf = "12345678901",
        Telefone = "11999999999",
        Email = "ana.souza@email.com",
        Logradouro = "Rua das Flores",
        Numero = "123",
        Cidade = "Sao Paulo",
        Estado = "SP",
        Cep = "01000-000"
    };

    var funcionario = new Funcionario
    {
        Id = Guid.NewGuid(),
        Nome = "Carlos Lima",
        Matricula = "FUNC-001",
        Cargo = "Mecanico",
        Cpf = "10987654321",
        Telefone = "11888888888",
        Email = "carlos.lima@email.com",
        DataAdmissao = DateTime.UtcNow.AddYears(-2),
        Salario = 4500.00m,
        Status = "Ativo"
    };

    var veiculoCliente = new VeiculoCliente
    {
        Id = Guid.NewGuid(),
        PlacaVeiculo = "ABC1234",
        Ano = 2020,
        Observacoes = "Revisao programada",
        ClienteId = cliente.Id,
        ModeloId = modelo1.Id
    };

    var servico = new Servico
    {
        Id = Guid.NewGuid(),
        Descricao = "Troca de oleo e filtros",
        PrecoBase = 350.00m
    };

    var peca = new Peca
    {
        Id = Guid.NewGuid(),
        Nome = "Filtro de oleo",
        CodigoFabricante = "FO-987",
        PrecoUnitario = 45.90m,
        EstoqueQuantidade = 20
    };

    var ordemServico = new OrdemServico
    {
        Id = Guid.NewGuid(),
        DataAbertura = DateTime.UtcNow,
        Status = "Aberta",
        Diagnostico = "Manutencao preventiva",
        FuncionarioId = funcionario.Id,
        VeiculoClienteId = veiculoCliente.Id
    };

    var osServico = new OsServico
    {
        OrdemServicoId = ordemServico.Id,
        ServicoId = servico.Id,
        Quantidade = 1,
        PrecoCobrado = 350.00m
    };

    var osPeca = new OsPeca
    {
        OrdemServicoId = ordemServico.Id,
        PecaId = peca.Id,
        Quantidade = 1,
        PrecoUnitario = 45.90m
    };

    await db.Marcas.AddRangeAsync(marca1, marca2);
    await db.Modelos.AddRangeAsync(modelo1, modelo2);
    await db.Clientes.AddAsync(cliente);
    await db.Funcionarios.AddAsync(funcionario);
    await db.VeiculosCliente.AddAsync(veiculoCliente);
    await db.Servicos.AddAsync(servico);
    await db.Pecas.AddAsync(peca);
    await db.OrdensServico.AddAsync(ordemServico);
    await db.OsServicos.AddAsync(osServico);
    await db.OsPecas.AddAsync(osPeca);

    await db.SaveChangesAsync();

    return Results.Created("/seed", new { message = "Dados de exemplo inseridos com sucesso." });
});

app.MapGet("/clientes", async (ApplicationDbContext db) =>
{
    var clientes = await db.Clientes
        .AsNoTracking()
        .ToListAsync();

    return Results.Ok(clientes);
});

app.MapGet("/clientes/{id:guid}", async (Guid id, ApplicationDbContext db) =>
{
    var cliente = await db.Clientes
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);

    return cliente is null ? Results.NotFound() : Results.Ok(cliente);
});

app.MapGet("/ordens-servico", async (ApplicationDbContext db) =>
{
    var ordens = await db.OrdensServico
        .Include(x => x.OsServicos)
        .ThenInclude(x => x.Servico)
        .Include(x => x.OsPecas)
        .ThenInclude(x => x.Peca)
        .AsNoTracking()
        .ToListAsync();

    return Results.Ok(ordens);
});

app.MapGet("/veiculos-estoque", async (ApplicationDbContext db) =>
{
    var veiculos = await db.VeiculosEstoque
        .AsNoTracking()
        .ToListAsync();

    return Results.Ok(veiculos);
});

app.Run();
