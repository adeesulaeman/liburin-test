using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Models
{
    public class UserType:BaseEntity,IEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ICollection<User> Users { get; set; }

        public UserType()
        {
            Users = new HashSet<User>();
        }
    }
}
