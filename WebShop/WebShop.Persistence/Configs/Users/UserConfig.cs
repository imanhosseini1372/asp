using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Entities.Users;

namespace WebShop.Persistence.Configs.Users
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            #region Config Field UserName

            builder.Property(e => e.UserName)
               .HasMaxLength(100)
               .IsRequired()
               .IsUnicode(false);
            builder.HasIndex(e => e.UserName).IsUnique().IsClustered(false);

            #endregion

            #region Config Field Password

            builder.Property(e => e.Password)
                  .IsRequired()
                  .IsUnicode(false);

            #endregion

            #region Config Field Email

            builder.Property(e => e.Email).IsRequired()
               .IsUnicode(false);

            builder.HasIndex(e => e.Email).IsClustered(false);

            #endregion

            #region Config Field Mobile

            builder.Property(e=>e.Mobile)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(11);

            builder.HasIndex(e => e.Mobile).IsClustered(false);
            
            #endregion






        }
    }
}
