using Autofac;
using MediatR;
using Service.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Base.Dispatchers
{
    public class Dispatcher : IDispatcher
    {
        private readonly IComponentContext _context;
        public Dispatcher(IComponentContext context)
        {
            _context = context;
        }
        public async Task<TResult> DoAsync<TResult>(IRequest<TResult> query)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _context.Resolve(handlerType);
            CancellationToken cancellationToken = new CancellationTokenSource().Token;
            return await handler.Handle((dynamic)query, cancellationToken);
        }
    }
}
