using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Base.Contracts
{
    public interface IDispatcher
    {
        Task<TResult> DoAsync<TResult>(IRequest<TResult> query);
    }
}
