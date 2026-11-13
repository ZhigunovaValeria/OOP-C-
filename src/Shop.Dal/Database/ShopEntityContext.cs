using Microsoft.EntityFrameworkCore;
using Shop.Dal.Entities;

namespace Shop.Dal.Database;

public sealed class ShopEntityContext : DbContext
{
    public string DataSource { get; private set; }

    public ShopEntityContext(string dataSource = "shop.db")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dataSource);
        DataSource = Path.GetFullPath(dataSource);
        Directory.CreateDirectory(Path.GetDirectoryName(DataSource)!);
        Database.EnsureCreated();
    }

    public DbSet<ShopDao> Shops { get; set; } = null!;
    public DbSet<GoodDao> Goods { get; set; } = null!;
    public DbSet<ClientDao> Clients { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={DataSource}");
}
