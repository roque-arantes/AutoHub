using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoHub.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Cpf = table.Column<string>(type: "VARCHAR(11)", maxLength: 11, nullable: false),
                    Telefone = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    Logradouro = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Numero = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    Cidade = table.Column<string>(type: "VARCHAR(60)", maxLength: 60, nullable: false),
                    Estado = table.Column<string>(type: "VARCHAR(2)", maxLength: 2, nullable: false),
                    Cep = table.Column<string>(type: "VARCHAR(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Matricula = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Cargo = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    Cpf = table.Column<string>(type: "VARCHAR(11)", maxLength: 11, nullable: false),
                    Telefone = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true),
                    DataAdmissao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pecas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    CodigoFabricante = table.Column<string>(type: "VARCHAR(60)", maxLength: 60, nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    EstoqueQuantidade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pecas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    PrecoBase = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    MarcaId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modelos_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VeiculosCliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlacaVeiculo = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: false),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    Observacoes = table.Column<string>(type: "VARCHAR(300)", maxLength: 300, nullable: true),
                    ClienteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModeloId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeiculosCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeiculosCliente_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VeiculosCliente_Modelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "Modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VeiculosEstoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AnoFabricacao = table.Column<int>(type: "INTEGER", nullable: false),
                    Cor = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    Kilometragem = table.Column<int>(type: "INTEGER", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false),
                    Chassis = table.Column<string>(type: "VARCHAR(17)", maxLength: 17, nullable: false),
                    ModeloId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeiculosEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeiculosEstoque_Modelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "Modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdensServico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    ValorFinal = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Diagnostico = table.Column<string>(type: "VARCHAR(300)", maxLength: 300, nullable: true),
                    FuncionarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VeiculoClienteId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdensServico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdensServico_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdensServico_VeiculosCliente_VeiculoClienteId",
                        column: x => x.VeiculoClienteId,
                        principalTable: "VeiculosCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DataVenda = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrecoOriginal = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(9,2)", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(9,2)", nullable: false),
                    FormaPagamento = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VeiculoEstoqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClienteId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vendas_VeiculosEstoque_VeiculoEstoqueId",
                        column: x => x.VeiculoEstoqueId,
                        principalTable: "VeiculosEstoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OsPecas",
                columns: table => new
                {
                    PecaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrdemServicoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsPecas", x => new { x.OrdemServicoId, x.PecaId });
                    table.ForeignKey(
                        name: "FK_OsPecas_OrdensServico_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalTable: "OrdensServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OsPecas_Pecas_PecaId",
                        column: x => x.PecaId,
                        principalTable: "Pecas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OsServicos",
                columns: table => new
                {
                    ServicoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrdemServicoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoCobrado = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsServicos", x => new { x.OrdemServicoId, x.ServicoId });
                    table.ForeignKey(
                        name: "FK_OsServicos_OrdensServico_OrdemServicoId",
                        column: x => x.OrdemServicoId,
                        principalTable: "OrdensServico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OsServicos_Servicos_ServicoId",
                        column: x => x.ServicoId,
                        principalTable: "Servicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Cpf",
                table: "Clientes",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Matricula",
                table: "Funcionarios",
                column: "Matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marcas_Nome",
                table: "Marcas",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modelos_MarcaId",
                table: "Modelos",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServico_FuncionarioId",
                table: "OrdensServico",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServico_VeiculoClienteId",
                table: "OrdensServico",
                column: "VeiculoClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_OsPecas_PecaId",
                table: "OsPecas",
                column: "PecaId");

            migrationBuilder.CreateIndex(
                name: "IX_OsServicos_ServicoId",
                table: "OsServicos",
                column: "ServicoId");

            migrationBuilder.CreateIndex(
                name: "IX_VeiculosCliente_ClienteId",
                table: "VeiculosCliente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_VeiculosCliente_ModeloId",
                table: "VeiculosCliente",
                column: "ModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_VeiculosCliente_PlacaVeiculo",
                table: "VeiculosCliente",
                column: "PlacaVeiculo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VeiculosEstoque_Chassis",
                table: "VeiculosEstoque",
                column: "Chassis",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VeiculosEstoque_ModeloId",
                table: "VeiculosEstoque",
                column: "ModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_ClienteId",
                table: "Vendas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_FuncionarioId",
                table: "Vendas",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_VeiculoEstoqueId",
                table: "Vendas",
                column: "VeiculoEstoqueId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OsPecas");

            migrationBuilder.DropTable(
                name: "OsServicos");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropTable(
                name: "Pecas");

            migrationBuilder.DropTable(
                name: "OrdensServico");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "VeiculosEstoque");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "VeiculosCliente");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Modelos");

            migrationBuilder.DropTable(
                name: "Marcas");
        }
    }
}
