namespace Di2.Data
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Di2.Data.Common.Models;
    using Di2.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
            typeof(ApplicationDbContext).GetMethod(
                nameof(SetIsDeletedQueryFilter),
                BindingFlags.NonPublic | BindingFlags.Static);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        //public DbSet<SubCategoryMaterial> SubCategoryMaterials { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<PriceList> PriceLists { get; set; }

        public DbSet<OrderSupplier> OrderSuppliers { get; set; }

        public DbSet<SupplyOrderStatus> SupplyOrderStatuses { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<ContactForm> ContactForms { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            ConfigureUserIdentityRelations(builder);

            EntityIndexesConfiguration.Configure(builder);

            var entityTypes = builder.Model.GetEntityTypes().ToList();

            // Set global query filter for not deleted entities only
            var deletableEntityTypes = entityTypes
                .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
            foreach (var deletableEntityType in deletableEntityTypes)
            {
                var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
                method.Invoke(null, new object[] { builder });
            }

            // Disable cascade delete
            var foreignKeys = entityTypes
                .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
            foreach (var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //builder.Entity<DeliveryBatch>().HasKey(k => new
            // { k.MaterialId, k.SupplierId, });

            builder.Entity<PriceList>().HasKey(k => new
            { k.MaterialId, k.SupplierId, });

            builder.Entity<SubCategoryMaterial>().HasKey(x => new { x.SubCategoryId, x.MaterialId });
            //builder.Entity<SubCategoryMaterial>().HasOne(k => k.Material)
             //    .WithMany(s => s.SubCategoryMaterials).HasForeignKey(k => k.MaterialId);

            //builder.Entity<SubCategoryMaterial>().HasOne(k => k.SubCategory)
             //   .WithMany(s => s.SubCategoryMaterials).HasForeignKey(k => k.SubCategoryId);

            builder.Entity<PriceList>().HasOne(k => k.Material)
                .WithMany(s => s.PriceLists).HasForeignKey(k => k.MaterialId);

            builder.Entity<PriceList>().HasOne(k => k.Supplier)
                .WithMany(s => s.PriceLists).HasForeignKey(k => k.SupplierId);
        }

        private void ConfigureUserIdentityRelations(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
            where T : class, IDeletableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
