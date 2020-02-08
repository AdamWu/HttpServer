using System;

namespace HttpServer
{
    public class RouteAttribute : Attribute
    {
        public string Route;

        public RouteAttribute(string s)
        {
            Route = s;
        }
    }
}