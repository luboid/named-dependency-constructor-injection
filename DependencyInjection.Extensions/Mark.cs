using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.Extensions
{
    public class Mark<T> : IMark<T>
    {
        public Mark(T value)
        {
            this.Value = value;
        }

        public T Value { get; }

        public void Dispose()
        {
            if (Value is IDisposable d && d != null)
            {
                d.Dispose();
            }
        }
    }
}
