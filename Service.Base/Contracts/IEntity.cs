using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Contracts
{
    public interface IEntity
    {
        long Id { get; set; }

        bool IsActive { get; set; }
        bool IsDeleted { get; set; }

        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }

        string ModifiedBy { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}
