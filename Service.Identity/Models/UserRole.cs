using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Models
{
    public class UserRole : BaseEntity, IEntity
    {
        [ForeignKey("user_id")]
        public long UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("role_id")]
        public long RoleId { get; set; }
        public Role Role { get; set; }
    }
}
