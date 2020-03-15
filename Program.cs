using System;
using System.Collections.Generic;
using System.Threading;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Config.Init())
            {
                Logger.Error("config init fail");
                return;
            }

            MysqlHelper.Init(Config.GetValue("mysql","ip"), int.Parse(Config.GetValue("mysql", "port")),Config.GetValue("mysql", "user"), Config.GetValue("mysql", "password"), Config.GetValue("mysql", "database"));

            //HttpServer httpServer = new HttpServer(Config.GetValue("server","ip"), int.Parse(Config.GetValue("server","port")));
            HttpServer httpServer = new HttpServer("127.0.0.1", 8080);
            httpServer.Start();
            
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
