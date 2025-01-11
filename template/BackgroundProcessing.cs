public class BackgroundTask : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Logic here
            await Task.Delay(1000, stoppingToken);
        }
    }
}
