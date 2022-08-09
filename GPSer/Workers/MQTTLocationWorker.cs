using GPSer.API.Data.UnitOfWork;
using GPSer.API.Data.UnitOfWork.Specs;
using GPSer.API.Models;
using GPSer.API.State;
using GPSer.Models;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace GPSer.API.Workers;

public class MQTTLocationWorker : BackgroundService
{
    private readonly ILogger<MQTTLocationWorker> logger;
    private readonly IRemoteClientState remoteClientState;
    private readonly IServiceProvider services;

    public MQTTLocationWorker(ILogger<MQTTLocationWorker> logger, IServiceProvider services, IRemoteClientState remoteClientState)
    {
        this.logger = logger;
        this.services = services;
        this.remoteClientState = remoteClientState;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("MQTT Location worker is running...");

        MqttFactory mqttFactory = new MqttFactory();
        remoteClientState.MqttClient = mqttFactory.CreateMqttClient();

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer("127.0.0.1", 1883)
            .Build();

        try
        {
            // TODO
            // Setup message handling before connecting so that queued messages
            // are also handled properly. When there is no event handler attached all
            // received messages get lost.

            await remoteClientState.MqttClient.ConnectAsync(mqttClientOptions, cancellationToken);

            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic("4312513255"); })
                .Build();

            await remoteClientState.MqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot connect to mqtt broker");
        }

        remoteClientState.MqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                using var scope = services.CreateScope();

                var deviceRepo = scope.ServiceProvider.GetRequiredService<IRepository<Device>>();
                var locationDataRepo = scope.ServiceProvider.GetRequiredService<IRepository<LocationData>>();

                string topic = e.ApplicationMessage.Topic;
                if (string.IsNullOrWhiteSpace(topic) == false)
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    Console.WriteLine($"Topic: {topic}. Message Received: {payload}");

                    //var device = deviceRepo.FirstOrDefault(new BySerialNumberSpec(topic));
                    var devices = await deviceRepo.ListAllAsync();

                    foreach (Device device in devices)
                    {
                        var newLocationData = new LocationData
                        {
                            Id = new Guid(),
                            Latitude = payload.Substring(0, 6),
                            Longitude = payload.Substring(7, 6),
                            Speed = 12.3,
                            Device = device,
                            DeviceId = device.Id
                        };

                        await locationDataRepo.AddAsync(newLocationData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
        };

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //await BackgroundCheck(stoppingToken);
    }

    private async Task BackgroundCheck(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            remoteClientState.MqttClient.ApplicationMessageReceivedAsync += e =>
            {
                try
                {

                    using var scope = services.CreateScope();

                    var deviceRepo = scope.ServiceProvider.GetRequiredService<IRepository<Device>>();
                    var locationDataRepo = scope.ServiceProvider.GetRequiredService<IRepository<LocationData>>();

                    string topic = e.ApplicationMessage.Topic;
                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine($"Topic: {topic}. Message Received: {payload}");

                        var device = deviceRepo.FirstOrDefault(new BySerialNumberSpec(topic));
                       
                        var newLocationData = new LocationData
                        {
                            Id = new Guid(),
                            Latitude = payload.Substring(0, 6),
                            Longitude = payload.Substring(7, 12),
                            Speed = 12.3,
                            Device = device,
                            DeviceId = device.Id
                        };

                        locationDataRepo.Add(newLocationData);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }

                return Task.CompletedTask;
            };
        }
    }
}