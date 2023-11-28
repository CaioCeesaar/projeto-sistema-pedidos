using EFCore.Data.Configuration;
using EFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ApplicationContext : DbContext
{
    private static readonly ILoggerFactory _logger = 
        LoggerFactory.Create(p => p.AddConsole());
    
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    
    // DbContextOptionBuilder -> é através dele que informamos o provider que iremos utilizar
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(_logger)
            .EnableSensitiveDataLogging()
            .UseSqlServer("Data Source=DESKTOP-747LPER;Initial Catalog=CursoEFCore;Integrated Security=true;TrustServerCertificate=True;User Id = DESKTOP-747LPER", 
                p=>p.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(5), 
                    errorNumbersToAdd: null)
                    .MigrationsHistoryTable("curso_ef_core"));
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
        MapearPropriedadesEsquecidas(modelBuilder);
    }
    
    private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));
            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue) 
                {
                    property.SetColumnType("VARCHAR(100)");
                }
            }
        }
    }
}