using System;
using System.Collections.Generic;

namespace TransportLibrary
{
    [Serializable]
    public class RouteContent
    {
        public List<string> DirectRoute { get; private set; }
        public List<string> ReverseRoute { get; private set; }

        public RouteContent(List<string> directRoute, List<string> reverseRoute)
        {
            DirectRoute = directRoute;
            ReverseRoute = reverseRoute;
        }

        public RouteContent() { }
    }
}
