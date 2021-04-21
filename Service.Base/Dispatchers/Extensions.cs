using Autofac;
using Service.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Dispatchers
{
    public static class Extensions
    {
        public static void AddDispatchers(this ContainerBuilder builder)
        {
            builder.RegisterType<Dispatcher>().As<IDispatcher>();
        }
    }
}
