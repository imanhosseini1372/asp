using WebShop.Domain.Entities.Framework;

namespace WebShop.Domain.Entities.Users
{
    public class Role : BaseEntity
    {
        public string RoleTitle { get; set; }
        public  ICollection<User>? Users { get; set; }
    }
}
