using Ardalis.Specification;
using GPSer.Models;

namespace GPSer.API.Data.UnitOfWork.Specs;

public class BySerialNumberSpec : Specification<Device>
{
    public BySerialNumberSpec(string serialNumber) =>
        Query.Where(x => x.SerialNumber == serialNumber);
}