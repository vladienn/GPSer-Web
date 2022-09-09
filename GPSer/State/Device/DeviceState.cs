namespace GPSer.API.State;

public class DeviceState : IDeviceState
{
    public Dictionary<string, DeviceStateItem> Items { get; set; } = new();
}
