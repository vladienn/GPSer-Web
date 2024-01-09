using MQTTnet.Client;

namespace GPSer.Core.State;

public interface IRemoteClientState
{
    public IMqttClient MqttClient { get; set; }
}