namespace Project_4.Publisher.Api.Services;

public class RabbitmqInitializerService : IHostedService
{
    private readonly IRabbitmqPublishService _rabbitmqPublishService;
    public RabbitmqInitializerService(IRabbitmqPublishService rabbitmqPublisher)
    {
        _rabbitmqPublishService = rabbitmqPublisher;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _rabbitmqPublishService.InitializeAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}