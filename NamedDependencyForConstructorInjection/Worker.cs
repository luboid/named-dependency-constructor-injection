using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NamedDependencyForConstructorInjection
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public Worker(IServiceScopeFactory serviceScopeFactory, ILogger<Worker> logger)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                RunService();
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void RunService()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            using var service = scope.ServiceProvider.GetRequiredService<WorkerService>();
            service.Run();
        }
    }
}
