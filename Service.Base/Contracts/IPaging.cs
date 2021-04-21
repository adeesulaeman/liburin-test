using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Contracts
{
    public interface IPagedQuery
    {
        int Page { get; set; }
        int ItemsPerPage { get; set; }
        string Search { get; set; }
    }

    public interface IPagedResult<T>
    {
        int CurrentPage { get; set; }
        int TotalRecords { get; set; }
        IEnumerable<T> Data { get; set; }
        int ResultPerPage { get; set; }
    }

    public interface ISortable
    {
        string Field { get; set; }
        int Order { get; set; }
    }
}
