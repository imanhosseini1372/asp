using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities.Users;

namespace WebShop.Application.Repositories.Framework.Interfaces.Contexts
{
    public interface IWebShopDbContext
    {
        #region Dbset

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        #endregion

        #region Methods
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());

        #endregion
    }
}
