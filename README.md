<h1 align="center">CP1 - (MER) e Criação do Projeto WebAPI </h1>

###

<h2 align="center">📋Integrantes📋</h2>

###

<table align="center">
    <tr>
        <td align="center">
            <img src="https://avatars.githubusercontent.com/u/202198493?v=4" width="100px;" alt="Foto do Integrante Matheus Roque"/>
            <br>
            <sub>
                <b>Matheus Roque</b><br>
                <b>Rm: 561959</b>
            </sub>
        </td>
        <td align="center">
            <img src="https://avatars.githubusercontent.com/u/200883157?s=400&u=4c0d649624f6736e702b60244099bdf4b887eda7&v=4" width="100px;" alt="Foto do Integrante Giovane dos Santos"/>
            <br>
            <sub>
                <b>Giovane dos Santos</b><br>
                <b>Rm: 561336</b>
            </sub>
        </td>
    </tr>
</table>



<h2 align="center">Domínio Escolhido</h2>

###

<p align="center">
  O domínio escolhido para o projeto de uma WebAPI é o de uma concessionária com inclusão de uma oficina de automóveis. 
</p>



###

<h2 align="center">Quais Entidades foram Modeladas </h2>

###

<p align="center">
Foram modeladas as seguintes entidades:

    - Marcas
    - Modelos
    - Clientes
    - Funcionarios
    - VeiculosClientes (Oficina)
    - VeiculosEstoque (Concessionária)
    - Servicos
    - Pecas
    - OrdemServico
    - OsServiços
    - OsPecas
    - Vendas
</p>


<h2 align="center">Resumo dos Relacionamentos</h2>

###

| Entidades | Cardinalidade | Tipo | Observação |
|---|---|---|---|
| Marcas → Modelos | 1:N | Não-identificador | Uma marca tem vários modelos |
| Modelos → VeiculosCliente | 1:N | Não-identificador | Um modelo aparece em vários veículos de clientes |
| Modelos → VeiculosEstoque | 1:N | Não-identificador | Um modelo aparece em vários veículos do estoque |
| Clientes → VeiculosCliente | 1:N | Não-identificador | Um cliente pode ter vários veículos na oficina |
| Clientes → Vendas | 1:N | Não-identificador | Um cliente pode fazer várias compras |
| VeiculosCliente → OrdemServico | 1:N | Não-identificador | Um veículo pode ter múltiplas OS |
| VeiculosEstoque → Vendas | 1:1 | Exclusiva | Um veículo do estoque é vendido apenas uma vez |
| Funcionarios → OrdemServico | 1:N | Não-identificador | Um mecânico atende múltiplas OS |
| Funcionarios → Vendas | 1:N | Não-identificador | Um vendedor faz múltiplas vendas |
| OrdemServico ↔ Servicos | N:N | Identificador | Via OsServicos — linha sólida |
| OrdemServico ↔ Pecas | N:N | Identificador | Via OsPecas — linha sólida |

- Um VeiculosEstoque só pode estar vinculado a UMA Venda (1:1). Ao vender, atualizar status para 'vendido'.
- OrdemServico pertence a VeiculosCliente, nunca a VeiculosEstoque — domínios separados.
- preco_cobrado em OsServicos e preco_unitario em OsPecas devem ser gravados como snapshot — preços podem mudar.
- estoque_qtd em Pecas deve ser decrementado ao inserir registro em OsPecas.
- Marca e modelo não são cadastrados diretamente nos veículos — são referenciados via id_modelo.
- Um Funcionario pode atuar como Vendedor em Vendas e Mecânico em OrdemServico — o cargo define a função.