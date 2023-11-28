using System.Formats.Tar;
using EFCore.Domain;
using EFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

// using var db = new ApplicationContext();
// var existe = db.Database.GetPendingMigrations().Any();
// if (existe) 
// {
//     // Aqui podemos fazer um tratamento de erro, caso exista alguma migração pendente
// }


class Program
{
    static void Main(string[] args)
    {
        //InserirDados();
        //InserirDadosEmMassa();
        // ConsultarDados();
         //CadastrarPedido();
        // ConsultarPedidoCarregamentoAdiantado();
        // AtualizarDados();
        RemoverRegistro();
    }


    private static void RemoverRegistro()
    {
        using var db = new ApplicationContext();
        // var cliente = db.Clientes.Find(2); // primeiro localiza o registro para depois remover
        // var cliente = new Cliente {Id = 3};
        
        var cliente = new Cliente {Id = 3}; // apenas uma interação
        
        // Opções para remover:
        // db.Clientes.Remove(cliente);
        // db.Remove(cliente);
         db.Entry(cliente).State = EntityState.Deleted;
        
        db.SaveChanges();
        
    }

    private static void AtualizarDados()
    {
        using var db = new ApplicationContext();
        // var cliente = db.Clientes.Find(1);
        
        var cliente = new Cliente 
        {
            Id = 1
        };
        
        var clienteDesconectado = new 
        {
            Nome = "Cliente desconectado passo 3",
            Telefone = "99999999999"
        };
        
        db.Attach(cliente);
        db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);
        
        // db.Clientes.Update(cliente);
        db.SaveChanges();
    } 
        
    private static void ConsultarPedidoCarregamentoAdiantado()
    {
        using var db = new ApplicationContext();
        var pedidos = db.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(p=>p.Produto)
            .ToList();
        
        Console.WriteLine(pedidos.Count);
    }
    
    private static void CadastrarPedido()
    {
        using var db = new ApplicationContext();
        
        var cliente = db.Clientes.FirstOrDefault();
        var produto = db.Produtos.FirstOrDefault();
        
        var pedido = new Pedido
        {
            ClienteId = cliente.Id,
            IniciadoEm = DateTime.Now,
            FinalizadoEm = DateTime.Now,
            Observacao = "Pedido teste",
            Status = StatusPedidos.Analise,
            TipoFrete = TipoFrete.SemFrete,
            Itens = new List<PedidoItem>
            {
                new()
                {
                    ProdutoId = produto.Id,
                    Desconto = 0,
                    Quantidade = 1,
                    Valor = 10,
                }
            }
        };

        db.Pedidos.Add(pedido);
        
        db.SaveChanges();

    }
    
    private static void ConsultarDados()
    {
        using var db = new ApplicationContext();
        // var consultaPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();
        // ASNoTracking -> serve para que o EF não fique rastreando as entidades que foram consultadas
        var consultaPorMetodo = db.Clientes
            .Where(p => p.Id > 0)
            .OrderBy(p => p.Id)
            .ToList();
        foreach (var cliente in consultaPorMetodo)
        {
            Console.WriteLine($"Consultando cliente: {cliente.Id}");
            // db.Clientes.Find(cliente.Id);
            db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
        }
    }
    
    private static void InserirDados()
    {
        var produto = new Produto
        {
            Descricao = "Produto teste",
            CodigoBarras = "123456789123",
            Valor = 10m,
            TipoProduto = TipoProduto.MercadoriaParaRevenda,
            Ativo = true
        };
    
        using var db = new ApplicationContext();
        db.Produtos.Add(produto);

        var regitros = db.SaveChanges();
        Console.WriteLine($"Total de registros: {regitros}");

    }

    private static void InserirDadosEmMassa()
    {
        var produto = new Produto
        {
            Descricao = "Produto teste",
            CodigoBarras = "1234567891231",
            Valor = 10m,
            TipoProduto = TipoProduto.MercadoriaParaRevenda,
            Ativo = true
        };
        
        var cliente = new Cliente
        {
            Nome = "Caio Camboim",
            CEP = "99999999",
            Cidade = "Cidade",
            Estado = "SP",
            Telefone = "99999999999"
        };
        
        var listaClientes = new[]
        {
            new Cliente
            {
                Nome = "teste1",
                CEP = "99999999",
                Cidade = "Cidade",
                Estado = "SP",
                Telefone = "99999999999"
            },
            new Cliente
            {
                Nome = "teste2",
                CEP = "99999999",
                Cidade = "Cidade",
                Estado = "SP",
                Telefone = "99999999999"
            }
        };
        
        using var db = new ApplicationContext();
        // db.AddRange(produto, cliente);
        db.Clientes.AddRange(listaClientes);

        var registros = db.SaveChanges();
        Console.WriteLine($"Total de registros: {registros}");
    }
}


