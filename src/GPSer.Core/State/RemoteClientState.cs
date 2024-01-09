using MQTTnet.Client;

namespace GPSer.Core.State;

public class RemoteClientState : IRemoteClientState
{
    public IMqttClient MqttClient { get; set; } = null!;
}