using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Base.Contracts;

namespace Service.Identity.Models
{
    public class Module : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        public Module()
        {
            Permissions = new HashSet<Permission>();
        }
    }
}
