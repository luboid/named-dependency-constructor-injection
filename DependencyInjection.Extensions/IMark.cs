using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Extensions
{
    public interface IMark<T> : IDisposable
    {
        T Value { get; }
    }
}
