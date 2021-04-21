using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Models
{
    public class Role:BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }

        [ForeignKey("group_id")]
        public long GroupId { get; set; }
        public Group Group { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Permission> Permissions { get; set; }

        public Role()
        {
            UserRoles = new HashSet<UserRole>();
            Permissions = new HashSet<Permission>();
        }
    }
}
