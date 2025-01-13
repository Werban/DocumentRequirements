using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Document> Documents { get; set; }
    public DbSet<Requirement> Requirements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связи между Document и Requirement
        modelBuilder.Entity<Requirement>()
            .HasOne(r => r.Document) 
            .WithMany(d => d.Requirements) 
            .HasForeignKey(r => r.DocumentId); 

        // Преобразование даты для Document
        modelBuilder.Entity<Document>()
            .Property(d => d.EffectiveDate)
            .HasConversion(
                v => v.ToUniversalTime(), 
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); 
    }
}