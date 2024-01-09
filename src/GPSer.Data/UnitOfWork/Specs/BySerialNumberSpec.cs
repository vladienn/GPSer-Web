using Ardalis.Specification;
using GPSer.Model;

namespace GPSer.Data.UnitOfWork.Specs;

public class BySerialNumberSpec : Specification<Device>
{
    public BySerialNumberSpec(string serialNumber) =>
        Query.Where(x => x.SerialNumber == serialNumber);
}