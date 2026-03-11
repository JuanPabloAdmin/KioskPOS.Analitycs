using KioskPos.Analytics.Analytics.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;



namespace KioskPos.Analytics.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class AnalyticsDbContext :
    AbpDbContext<AnalyticsDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // ═══ Analytics Entities ═══
    public DbSet<KioskRegistration> KioskRegistrations { get; set; }
    public DbSet<PosConnectionConfig> PosConnectionConfigs { get; set; }
    public DbSet<SyncHistory> SyncHistories { get; set; }
    public DbSet<CachedSalesOrder> CachedSalesOrders { get; set; }
    public DbSet<CachedInvoice> CachedInvoices { get; set; }
    public DbSet<DashboardSnapshot> DashboardSnapshots { get; set; }
    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<KioskRegistration>(b =>
        {
            b.ToTable(AnalyticsConsts.DbTablePrefix + "KioskRegistrations", AnalyticsConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.DisplayName).IsRequired().HasMaxLength(128);
            b.Property(x => x.Color).HasMaxLength(16);
            b.Property(x => x.Icon).HasMaxLength(16);
            b.HasIndex(x => new { x.TenantId, x.WorkplaceId });
        });

        builder.Entity<PosConnectionConfig>(b =>
        {
            b.ToTable(AnalyticsConsts.DbTablePrefix + "PosConnectionConfigs", AnalyticsConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.DisplayName).IsRequired().HasMaxLength(256);
            b.Property(x => x.ApiBaseUrl).IsRequired().HasMaxLength(512);
            b.Property(x => x.ApiToken).IsRequired().HasMaxLength(512);
            b.Property(x => x.AcmsBaseUrl).HasMaxLength(512);
            b.Property(x => x.AcmsApiToken).HasMaxLength(512);
            b.Property(x => x.KioskIdentifier).HasMaxLength(128);
            b.HasIndex(x => x.TenantId);
        });

        builder.Entity<SyncHistory>(b =>
        {
            b.ToTable(AnalyticsConsts.DbTablePrefix + "SyncHistories", AnalyticsConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => new { x.TenantId, x.PosConnectionConfigId, x.BusinessDay });
        });

        builder.Entity<CachedSalesOrder>(b =>
        {
            b.ToTable(AnalyticsConsts.DbTablePrefix + "CachedSalesOrders", AnalyticsConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => new { x.TenantId, x.BusinessDay });
            b.HasIndex(x => new { x.TenantId, x.GlobalId }).IsUnique();
            b.Property(x => x.Serie).IsRequired().HasMaxLength(32);
            b.Property(x => x.GlobalId).HasMaxLength(128);
            b.Property(x => x.LinesJson).HasColumnType("nvarchar(max)");
            b.Property(x => x.PaymentsJson).HasColumnType("nvarchar(max)");
        });

        builder.Entity<CachedInvoice>(b =>
        {
            b.ToTable(AnalyticsConsts.DbTablePrefix + "CachedInvoices", AnalyticsConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => new { x.TenantId, x.BusinessDay });
            b.HasIndex(x => new { x.TenantId, x.GlobalId }).IsUnique();
            b.Property(x => x.Serie).IsRequired().HasMaxLength(32);
            b.Property(x => x.GlobalId).HasMaxLength(128);
            b.Property(x => x.LinesJson).HasColumnType("nvarchar(max)");
            b.Property(x => x.PaymentsJson).HasColumnType("nvarchar(max)");
        });

        builder.Entity<DashboardSnapshot>(b =>
        {
            b.ToTable(AnalyticsConsts.DbTablePrefix + "DashboardSnapshots", AnalyticsConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasIndex(x => new { x.TenantId, x.PosConnectionConfigId, x.BusinessDay }).IsUnique();
            b.Property(x => x.PaymentBreakdownJson).HasColumnType("nvarchar(max)");
            b.Property(x => x.TopProductsJson).HasColumnType("nvarchar(max)");
            b.Property(x => x.TopFamiliesJson).HasColumnType("nvarchar(max)");
            b.Property(x => x.HourlySalesJson).HasColumnType("nvarchar(max)");
        });


        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(AnalyticsConsts.DbTablePrefix + "YourEntities", AnalyticsConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
