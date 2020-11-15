using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace NamedDependencyForConstructorInjection
{
    public class WorkerItem : IDisposable
    {
        public WorkerItem()
        { 
        }

        public string Name { get; set; }

        public void Dispose()
        {
            Debug.Write($"WorkerItem.WorkerItem:{Name} called.");
        }
    }
}
