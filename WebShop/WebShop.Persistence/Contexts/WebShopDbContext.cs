using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebShop.Application.Repositories.Framework.Interfaces.Contexts;
using WebShop.Domain.Entities.Users;
using WebShop.Persistence.AddAuditFieldInterceptors;

namespace WebShop.Persistence.Contexts
{
    public class WebShopDbContext : DbContext, IWebShopDbContext
    {
        #region Inject
        private readonly AddAuditFieldInterceptor _auditInterceptor;
        public WebShopDbContext(DbContextOptions options, AddAuditFieldInterceptor auditInterceptor) : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        #endregion

        #region properties

        #region Dbset

        public  DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        #endregion

        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //SetConfigEntities
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            //ShdowPropertiesAndQueryFilter
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {

                //AddShadowPropertiesAllTables
                modelBuilder.Entity(item.ClrType).Property<int>("CreateUser").IsRequired();
                modelBuilder.Entity(item.ClrType).Property<DateTime>("CreateDate").IsRequired();
                modelBuilder.Entity(item.ClrType).Property<int>("UpdateUser");
                modelBuilder.Entity(item.ClrType).Property<DateTime>("UpdateDate");


                // FilterIsDeletedAllTables
                var isDeletedProperty = item.ClrType.GetProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
                {
                    var parameter = Expression.Parameter(item.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Property(parameter, isDeletedProperty),
                        Expression.Constant(false));
                    var lambda = Expression.Lambda(body, parameter);

                    modelBuilder.Entity(item.ClrType).HasQueryFilter(lambda);
                }

            }
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            configurationBuilder.Properties<string>().HaveMaxLength(255).AreUnicode(true);
        }
        #endregion

    }
}
