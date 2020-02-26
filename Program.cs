using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string sql = "create table if not exists user(" +
                "id int PRIMARY KEY AUTO_INCREMENT," +
                "name varchar(100) unique NOT NULL," +
                "password varchar(100) NOT NULL)";
            MysqlHelper.ExecuteNonQuery(sql);
            sql = "create table if not exists room(" +
                "id int PRIMARY KEY AUTO_INCREMENT," +
                "title varchar(255) NOT NULL," +
                "description text NOT NULL," +
                "time int DEFAULT'120' NOT NULL," +
                "attendance int DEFAULT'100' NOT NULL," +
                "create_time datetime DEFAULT CURRENT_TIMESTAMP NOT NULL," +
                "update_time datetime DEFAULT CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,"+
                "end_time datetime,"+
                "state int DEFAULT'0' NOT NULL)";
            MysqlHelper.ExecuteNonQuery(sql);
            sql = "create table if not exists record(" +
                "id int PRIMARY KEY AUTO_INCREMENT," +
                "room_id varchar(255) NOT NULL," +
                "create_time datetime DEFAULT CURRENT_TIMESTAMP NOT NULL," +
                "state int DEFAULT'0' NOT NULL)";
            MysqlHelper.ExecuteNonQuery(sql);

            Config config = new Config();
            if (!config.Init())
            {
                return;
            }
            
            HttpServer httpServer = new HttpServer(config.GetValue("ip"), int.Parse(config.GetValue("port")));
            //HttpServer httpServer = new HttpServer("127.0.0.1", 8080);
            httpServer.Start();
            
            Thread.Sleep(System.Threading.Timeout.Infinite);
        }
    }
}
