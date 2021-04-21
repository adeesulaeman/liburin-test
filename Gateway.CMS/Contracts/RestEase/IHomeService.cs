using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Gateway.CMS.Contracts.RestEase
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IHomeService
    {

    }
}
