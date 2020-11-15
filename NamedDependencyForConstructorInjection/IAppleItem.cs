using DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamedDependencyForConstructorInjection
{
    public interface IAppleItem : IMark<WorkerItem>
    {
    }
}
