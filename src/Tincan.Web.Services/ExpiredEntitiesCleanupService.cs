using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Tincan.Web.Services
{
    public class ExpiredEntitiesCleanupService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private Timer _timer;

        public ExpiredEntitiesCleanupService(IServiceScopeFactory scopeFactory, ILogger<ExpiredEntitiesCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        private async void PerformCleanup(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
                var deletedMessageCount = await repository.DeleteExpiredMessages(DateTime.UtcNow);
                _logger.LogInformation($"ExpiredEntitiesCleanupService removed {deletedMessageCount} messages");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting service ExpiredEntitiesCleanupService");
            _timer = new Timer(PerformCleanup, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping service ExpiredEntitiesCleanupService");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
