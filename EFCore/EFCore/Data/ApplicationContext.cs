using EFCore.Data.Configuration;
using EFCore.Domain;
using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Pedido> Pedidos { get; set; }
    
    // DbContextOptionBuilder -> é através dele que informamos o provider que iremos utilizar
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-747LPER;Initial Catalog=CursoEFCore;Integrated Security=true;TrustServerCertificate=True;User Id = DESKTOP-747LPER");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // De forma manual, podemos adicionar as configurações de cada entidade
        // como no exemplo abaixo: 
        // modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        // modelBuilder.ApplyConfiguration(new PedidoConfiguration());
        // modelBuilder.ApplyConfiguration(new PedidoitemConfiguration());
        // modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
        
        // Ou podemos utilizar o método ApplyConfigurationsFromAssembly
        // que irá buscar todas as classes que implementam a interface IEntityTypeConfiguration
        // e irá aplicar as configurações de cada uma delas.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
    
    
}