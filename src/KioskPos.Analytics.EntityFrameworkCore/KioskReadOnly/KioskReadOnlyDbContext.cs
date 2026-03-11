using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace KioskPos.Analytics.EntityFrameworkCore.KioskReadOnly;

/// <summary>
/// DbContext de SOLO LECTURA para la base de datos existente del Kiosk.
/// NO genera migraciones. Mapea tablas existentes sin alterarlas.
/// </summary>
[ConnectionStringName("Kiosk")]
public class KioskReadOnlyDbContext : AbpDbContext<KioskReadOnlyDbContext>
{
    public DbSet<KioskConfigurationEntity> Configurations { get; set; } = null!;
    public DbSet<KioskSelectedFamilyEntity> SelectedFamilies { get; set; } = null!;
    public DbSet<KioskSessionEntity> Sessions { get; set; } = null!;
    public DbSet<KioskLogEntity> Logs { get; set; } = null!;
    public DbSet<KioskCheckoutOperationEntity> CheckoutOperations { get; set; } = null!;

    public KioskReadOnlyDbContext(DbContextOptions<KioskReadOnlyDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ── Mapeo a tablas existentes del Kiosk ──
        // IMPORTANTE: Los nombres de tabla/columna deben coincidir
        // exactamente con la DB existente del Kiosk.
        // Ajustar si los nombres reales difieren.

        builder.Entity<KioskConfigurationEntity>(b =>
        {
            b.ToTable("Configurations");
            b.HasKey(e => e.Key);
            b.Property(e => e.Key).HasColumnName("Key");
            b.Property(e => e.Value).HasColumnName("Value");
        });

        builder.Entity<KioskSelectedFamilyEntity>(b =>
        {
            b.ToTable("SelectedFamilies");
            b.HasKey(e => e.Id);
            b.Property(e => e.Name).HasColumnName("Name");
            b.Property(e => e.OrderIndex).HasColumnName("OrderIndex");
        });

        builder.Entity<KioskSessionEntity>(b =>
        {
            b.ToTable("Sessions");
            b.HasKey(e => e.Id);
        });

        builder.Entity<KioskLogEntity>(b =>
        {
            b.ToTable("Logs");
            b.HasKey(e => e.Id);
        });

        builder.Entity<KioskCheckoutOperationEntity>(b =>
        {
            b.ToTable("CheckoutOperations");
            b.HasKey(e => e.Id);
        });
    }
}
