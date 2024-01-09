namespace GPSer.Core.State;

public interface IDeviceState
{
    Dictionary<string, DeviceStateItem> Items { get; set; }
}
