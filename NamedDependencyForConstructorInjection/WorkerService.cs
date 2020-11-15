using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamedDependencyForConstructorInjection
{
    internal class WorkerService : IDisposable
    {
        private readonly WorkerItem _pear;
        private readonly WorkerItem _apple;
        private readonly ILogger<Worker> _logger;
        public WorkerService(IPearItem pear, IAppleItem apple, ILogger<Worker> logger)
        {
            _pear = pear.Value;
            _apple = apple.Value;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Worker item: {time}", _apple.Name);
            _logger.LogInformation("Worker item: {time}", _pear.Name);
        }

        public void Dispose()
        {
        }
    }
}
