using SearchWaySystem;
using System.Collections.Generic;
using System.ServiceModel;

namespace YarTransportWebService
{
    [ServiceContract]
    public interface ITransportService
    {
        [OperationContract]
        List<RouteInfo> GetRoutes(string pointOfDeparture, string pointOfDestination,
            bool isBusChecked, bool isTrolleyChecked, bool isTramChecked, bool isMiniBusChecked);

        [OperationContract]
        List<string> GetStations();
    }
}
