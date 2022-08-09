using GPSer.API.State;
using MQTTnet;
using MQTTnet.Client;

namespace GPSer.API.Services;

public class MQTTConectionService : BackgroundService
{
    private readonly ILogger<MQTTConectionService> logger;
    private readonly IRemoteClientState remoteClientState;
    private readonly IServiceProvider services;

    public MQTTConectionService(IRemoteClientState remoteClientState, IServiceProvider services, ILogger<MQTTConectionService> logger)
    {
        this.remoteClientState = remoteClientState;
        this.services = services;
        this.logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}

