using System.Collections;
using System.Collections.Generic;

namespace Aec.Infrastructure.Framework.Mappers
{
    public interface IObjectMapper
    {
        List<TResult> MapTo<TResult>(IEnumerable source);
        TResult MapTo<TResult>(object source);
        TResult MapPropertiesToInstance<TResult>(object source, TResult value);
        TResult DynamicMapTo<TResult>(object source);
    }
}
