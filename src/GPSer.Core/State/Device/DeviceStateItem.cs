﻿using GPSer.Model;

namespace GPSer.Core.State;

public class DeviceStateItem
{
    public DeviceStateItem(Device device, DeviceStatus deviceStatus)
    {
        Device = device;
        Status = deviceStatus;
    }

    public Device Device { get; }

    public DeviceStatus Status { get; set; }
}
