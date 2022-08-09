using MQTTnet.Client;

namespace GPSer.API.State;

public class RemoteClientState : IRemoteClientState
{
    public IMqttClient MqttClient { get; set; } = null!;
}