using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Persistence.AddAuditFieldInterceptors
{


    public class AddAuditFieldInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddAuditFieldInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private int? GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                    return userId;
            }
            return null;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SetFields(eventData);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
        {
            SetFields(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private void SetFields(DbContextEventData eventData)
        {
            var userId = GetCurrentUserId();

            if (userId == null)
                return; // کاربر لاگین نیست

            var context = eventData.Context;
            var entries = context.ChangeTracker.Entries();

            foreach (var entry in entries.Where(e => e.State == EntityState.Added))
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "CreateUser"))
                    entry.Property("CreateUser").CurrentValue = userId;
                if (entry.Properties.Any(p => p.Metadata.Name == "CreateDate"))
                    entry.Property("CreateDate").CurrentValue = DateTime.Now;
                if (entry.Properties.Any(p => p.Metadata.Name == "IsDeleted"))
                    entry.Property("IsDeleted").CurrentValue = false;
            }

            foreach (var entry in entries.Where(e => e.State == EntityState.Modified))
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdateUser"))
                    entry.Property("UpdateUser").CurrentValue = userId;
                if (entry.Properties.Any(p => p.Metadata.Name == "UpdateDate"))
                    entry.Property("UpdateDate").CurrentValue = DateTime.Now;
            }
        }

        //private static void SetFields(DbContextEventData eventData)
        //{

        //    var ChangeTracker = eventData.Context.ChangeTracker;
        //    var AddedEntities = ChangeTracker.Entries().Where(c => c.State == EntityState.Added);
        //    var ModifiedEntities = ChangeTracker.Entries().Where(c => c.State == EntityState.Modified);
        //    foreach (var item in AddedEntities)
        //    {
        //        item.Property("CreateUser").CurrentValue = _userId;
        //        item.Property("CreateDate").CurrentValue = DateTime.Now;
        //        item.Property("IsDeleted").CurrentValue = false;
        //    }

        //    foreach (var item in ModifiedEntities)
        //    {
        //        item.Property("UpdateUser").CurrentValue = _userId;
        //        item.Property("UpdateDate").CurrentValue = DateTime.Now;
        //    }
        //}
    }
}


