using Service.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.ViewModels.Common
{
    public class PagedQueryVM : IPagedQuery
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }

        //private string _search;
        //public string Search { get { return _search == null ? "" : _search; } set { _search = value; } }
        public string Search { get; set; } = "";
    }

    public class PagedResultVM<T> : IPagedResult<T>, IResponse
    {
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int ResultPerPage { get; set; }

        public PagedResultVM()
        {
            Data = new HashSet<T>();
        }
    }

    public class SortableVM : ISortable
    {
        public string Field { get; set; } = "";
        public int Order { get; set; }
    }
}
