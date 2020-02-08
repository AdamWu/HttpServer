using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace HttpServer
{
    public class RoomHandler : BaseHandler
    {
        [RouteAttribute("/room/list")]
        public void GetAllRooms(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            // 验证token
            string token = request.QueryString["token"];
            if (!UserHandler.ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }

            string sql = "select * from room";
            DataSet ds = MysqlHelper.ExecuteDataSet(sql);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 0);
            List<object> data = new List<object>();
            dic.Add("data", data);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Dictionary<string, object> room = new Dictionary<string, object>();
                room.Add("id", row["id"]);
                room.Add("title", row["title"]);
                room.Add("state", row["state"]);
                room.Add("create_time", row["create_time"]);
                data.Add(room);
            }

            Response(context, dic);
        }

        [RouteAttribute("/room/add")]
        public void AddRoom(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string title = request.QueryString["title"];
            string description = request.QueryString["description"];
            
            // 验证token
            string token = request.QueryString["token"];
            if (!UserHandler.ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }
            if (title == null || description == null)
            {
                ResponseParameterInvalid(context);
                return;
            }

            MySqlConnection connection = MysqlHelper.CreateConnection();      

            Dictionary<string, object> dic = new Dictionary<string, object>();
            string sql = string.Format("insert into room(title, description) values('{0}','{1}')", title, description);
            int rows = MysqlHelper.ExecuteNonQuery(connection, sql);
            if (rows > 0)
            {
                MySqlDataReader reader = MysqlHelper.ExecuteReader(connection, "select LAST_INSERT_ID()");
                reader.Read();
                int id = reader.GetInt32(0);
                reader.Close();
                reader = MysqlHelper.ExecuteReader(connection, string.Format("select * from room where id={0}", id));
                if (reader.Read())
                {
                    Dictionary<string, object> data = new Dictionary<string, object>();
                    dic.Add("code", 0);
                    dic.Add("data", data);
                    
                    data.Add("id", reader["id"]);
                    data.Add("title", reader["title"]);
                    data.Add("state", reader["state"]);
                    data.Add("create_time", reader["create_time"]);
                } else
                {
                    dic.Add("code", -1);
                    dic.Add("msg", "operate fail");
                }

            } else
            {
                dic.Add("code", -1);
                dic.Add("msg", "operate fail");
            }
            MysqlHelper.CloseConnection(connection);
            
            Response(context, dic);
        }

        [RouteAttribute("/room/delete")]
        public void DeleteRoom(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string id = request.QueryString["id"];

            // 验证token
            string token = request.QueryString["token"];
            if (!UserHandler.ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }
            if (id == null)
            {
                ResponseParameterInvalid(context);
                return;
            }
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string sql = string.Format("delete from room where id={0}", id);
            int rows = MysqlHelper.ExecuteNonQuery(sql);
            if (rows > 0)
            {
                dic.Add("code", 0);
                dic.Add("data", new Dictionary<string, object>());
            } else
            {
                dic.Add("code", -1);
                dic.Add("msg", "operate fail");
            }
            Response(context, dic);
        }
        
        [RouteAttribute("/room/get")]
        public void GetRoom(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string id = request.QueryString["id"];

            // 验证token
            string token = request.QueryString["token"];
            if (!UserHandler.ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }
            if (id == null)
            {
                ResponseParameterInvalid(context);
                return;
            }
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string sql = string.Format("select * from room where id={0}", id);
            DataSet ds = MysqlHelper.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("id", row["id"]);
                data.Add("title", row["title"]);
                data.Add("state", row["state"]);
                data.Add("create_time", row["create_time"]);

                dic.Add("code", 0);
                dic.Add("data", data);
            }
            else
            {
                dic.Add("code", -1);
                dic.Add("msg", "operate fail");
            }
            
            Response(context, dic);
        }
        
    }
}