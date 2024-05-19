using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class MarketContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductStorage> ProductStorages { get; set; }
        private string _connectionString;

        public MarketContext(string connectionString) {  _connectionString = connectionString; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql(_connectionString);
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("ProductId");
                entity.Property(e => e.Name).HasColumnName("ProductName").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(255).IsRequired(); 
                entity.Property(e => e.Price).HasColumnType("integer");

                entity.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
                entity.HasMany(x => x.Storages).WithMany(x => x.Products);

            });
            modelBuilder.Entity<Storage>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("StorageID");
                entity.Property(e => e.Name).HasColumnName("StorageName").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(255).IsRequired();

                entity.Property(e => e.Count).HasColumnName("Count").HasColumnType("integer");

                entity.HasMany(e => e.Products).WithMany(e => e.Storages).UsingEntity(e => e.ToTable("ProductStorage"));

            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(p => p.Id).HasName("CategoryID");
                entity.Property(e => e.Name).HasColumnName("CategoryName").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(255).IsRequired();


            });
            modelBuilder.Entity<ProductStorage>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.HasKey(p => p.StorageId);
            });


        }
    }
}
