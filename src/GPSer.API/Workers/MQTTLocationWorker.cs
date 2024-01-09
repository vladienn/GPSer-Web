using GPSer.Core.State;
using GPSer.Data.UnitOfWork;
using GPSer.Model;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace GPSer.Workers;

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

        var scope = services.CreateScope();

        var deviceRepo = scope.ServiceProvider.GetRequiredService<IRepository<Device>>();
        var deviceState = scope.ServiceProvider.GetRequiredService<IDeviceState>();

        var devices = await deviceRepo.ListAllAsync();

        foreach (var device in devices)
        {
            deviceState.Items[device.SerialNumber] = new DeviceStateItem(device, DeviceStatus.Offline);
        }

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
                .WithTopicFilter(f => { f.WithTopic("+/loc"); })
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
                var locationDataRepo = scope.ServiceProvider.GetRequiredService<IRepository<LocationData>>();

                string topic = e.ApplicationMessage.Topic;
                if (!string.IsNullOrWhiteSpace(topic))
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    Console.WriteLine($"Topic: {topic}. Message Received: {payload}");

                    var deviceSerialNumber = topic.Substring(0, topic.IndexOf("/loc"));
                    var deviceRead = deviceState.Items.Where(x => x.Key == deviceSerialNumber).Select(x => x.Value.Device).FirstOrDefault();

                    if (deviceRead != null)
                    {
                        var newLocationData = new LocationData
                        {
                            Id = new Guid(),
                            Latitude = payload.Substring(0, 6),
                            Longitude = payload.Substring(7, 6),
                            Speed = Convert.ToDouble(payload.Substring(14, 5)),
                            Device = deviceRead,
                            DeviceId = deviceRead.Id
                        };

                        await locationDataRepo.AddAsync(newLocationData);
                    }

                    var approvalMessage = new MqttApplicationMessageBuilder()
                    .WithTopic($"{deviceSerialNumber}/ack")
                    .WithPayload("Received!")
                    .Build();

                    await remoteClientState.MqttClient.PublishAsync(approvalMessage, cancellationToken);
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
        while (!stoppingToken.IsCancellationRequested)
        {
            //logger.LogInformation("ping");
            await Task.Delay(TimeSpan.FromMilliseconds(5000), stoppingToken);
        }
    }
}