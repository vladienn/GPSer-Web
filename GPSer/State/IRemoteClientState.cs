using MQTTnet.Client;

namespace GPSer.API.State;

public interface IRemoteClientState
{
    public IMqttClient MqttClient { get; set; }
}

