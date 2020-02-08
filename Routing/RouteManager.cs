using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HttpServer
{
    public class RouteAction
    {
        public RouteAction(Type type, MethodInfo method)
        {
            Type = type;
            Method = method;
        }

        public Type Type { get; private set; }
        public MethodInfo Method { get; private set; }
    }

    public class RouteManager
    {
        private Dictionary<string, RouteAction> m_Routes;

        public RouteManager()
        {
            m_Routes = new Dictionary<string, RouteAction>();
        }

        public void Init()
        {
            Console.WriteLine("RouteManager init...");
            foreach (var type in this.GetType().Assembly.GetTypes())
            {                
                if (type.GetInterfaces().Any(x => x.FullName.Contains("IRouteHandler")))
                {
                    MethodInfo[] methods = type.GetMethods();
                    for (int i = 0; i < methods.Length; i++)
                    {
                        MethodInfo method = methods[i];
                        object[] attibutes = method.GetCustomAttributes(typeof(RouteAttribute), false);

                        if (attibutes.Length > 0)
                        {
                            RouteAttribute attibute = (RouteAttribute)attibutes.FirstOrDefault();
                            string route = attibute.Route;

                            AddRoute(route, type, method);
                        }
                    }
                }
            }
        }

        public void AddRoute(string route, Type type, MethodInfo method)
        {
            if(m_Routes.ContainsKey(route))
            {
                Console.WriteLine("error route!");
                return;
            }
            m_Routes.Add(route, new RouteAction(type, method));

            Console.WriteLine(string.Format("add route {0} -> {1}:{2}", route, type.Name, method.Name));
        }

        public RouteAction GetRoute(string route)
        {
            if (m_Routes.ContainsKey(route))
            {
                return m_Routes[route];
            }
            return null;
        }
    }
}