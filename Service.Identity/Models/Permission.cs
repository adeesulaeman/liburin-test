using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Models
{
    public class Permission : BaseEntity, IEntity
    {
        [ForeignKey("role_id")]
        public long RoleId { get; set; }
        public Role Role { get; set; }

        [ForeignKey("module_id")]
        public long ModuleId { get; set; }
        public Module Module { get; set; }

        [ForeignKey("operation_id")]
        public long OperationId { get; set; }
        public Operation Operation { get; set; }
    }
}
