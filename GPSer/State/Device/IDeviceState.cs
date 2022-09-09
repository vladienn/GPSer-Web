using GPSer.Models;

namespace GPSer.API.State;

public interface IDeviceState
{
    Dictionary<string, DeviceStateItem> Items { get; set; }
}
