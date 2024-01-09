using GPSer.Core.State;
using GPSer.Data.UnitOfWork;
using GPSer.Model;
using MQTTnet;
using MQTTnet.Client;
using System.Text;

namespace GPSer.Workers;

public class MQTTComandReaderWorker : BackgroundService
{
    private readonly ILogger<MQTTComandReaderWorker> logger;
    private readonly IRemoteClientState remoteClientState;
    private readonly IServiceProvider services;

    public MQTTComandReaderWorker(ILogger<MQTTComandReaderWorker> logger, IRemoteClientState remoteClientState, IServiceProvider services)
    {
        this.logger = logger;
        this.remoteClientState = remoteClientState;
        this.services = services;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("MQTT Location worker is running...");

        var scope = services.CreateScope();
        var deviceState = scope.ServiceProvider.GetRequiredService<IDeviceState>();

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
                .WithTopicFilter(f => { f.WithTopic("+/hb/ack"); })
                .Build();

            await remoteClientState.MqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot connect to mqtt broker");
        }
        remoteClientState.MqttClient.ApplicationMessageReceivedAsync += e =>
        {
            try
            {
                var locationDataRepo = scope.ServiceProvider.GetRequiredService<IRepository<LocationData>>();

                string topic = e.ApplicationMessage.Topic;
                if (!string.IsNullOrWhiteSpace(topic))
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    Console.WriteLine($"Topic: {topic}. Message Received: {payload}");

                    var deviceSerialNumber = topic.Substring(0, topic.IndexOf("/"));

                    if (deviceState.Items.ContainsKey(deviceSerialNumber))
                    {
                        deviceState.Items[deviceSerialNumber].Status = DeviceStatus.Online;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }

            return Task.CompletedTask;
        };

        await base.StartAsync(cancellationToken);
    }

    //Heartbeat
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = services.CreateScope();
            var deviceState = scope.ServiceProvider.GetRequiredService<IDeviceState>();

            foreach (var device in deviceState.Items)
            {
                var approvalMessage = new MqttApplicationMessageBuilder()
                    .WithTopic($"{device.Key}/hb")
                    .WithPayload("Received!")
                    .Build();

                deviceState.Items[device.Key].Status = DeviceStatus.Offline;

                await remoteClientState.MqttClient.PublishAsync(approvalMessage, stoppingToken);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(5000), stoppingToken);
        }
    }
}
