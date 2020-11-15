# named-dependency-constructor-injection
.Net Core named dependency for constructor injection.
Inject WorkItem class with diffrent settings two times.

```c#
    public class WorkerItem : IDisposable
    {
        public WorkerItem()
        { 
        }

        public string Name { get; set; }
```

```c#
    public interface IAppleItem : IMark<WorkerItem> { }
    public interface IPearItem : IMark<WorkerItem> { }  
```

```c#
    internal class WorkerService : IDisposable
    {
        private readonly WorkerItem _pear;
        private readonly WorkerItem _apple;
        private readonly ILogger<Worker> _logger;
        
        public WorkerService(IPearItem pear, IAppleItem apple, ILogger<Worker> logger)
        {
            _pear = pear.Value;
            _apple = apple.Value;
```

```c#
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IAppleItem>((_) =>
                    {
                        var apple = new WorkerItem { Name = "Apple" };
                        return Proxy.CreateInstance<IAppleItem, WorkerItem>(apple);
                    });
                    services.AddScoped<IPearItem>((_) =>
                    {
                        var pear = new WorkerItem { Name = "Pear" };
                        return Proxy.CreateInstance<IPearItem, WorkerItem>(pear);
                    });
                    services.AddScoped<WorkerService>();
                    services.AddHostedService<Worker>();
                });
```
