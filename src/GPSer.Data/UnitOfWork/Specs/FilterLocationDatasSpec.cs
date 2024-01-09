using Ardalis.Specification;
using GPSer.Model;

namespace GPSer.Data.UnitOfWork;

public class FilterLocationDatasSpec : Specification<LocationData>
{
    public FilterLocationDatasSpec(Guid deviceId, DateTime? from, DateTime? to)
    {
        Query.Where(x => x.DeviceId == deviceId);

        if (from != null)
        {
            Query.Where(x => x.CreatedAt >= from.Value);
        }

        if (to != null)
        {
            Query.Where(x => x.CreatedAt <= to.Value);
        }
    }
}