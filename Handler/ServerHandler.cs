using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Data;

namespace HttpServer
{
    public class ServerHandler : BaseHandler
    {
        const string Key = "yysmart";

        [RouteAttribute("/server/register")]
        public void ServerRegister(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 0);
            dic.Add("data", new Dictionary<string, object>());

            Response(context, dic);
        }

        [RouteAttribute("/server/userinfo")]
        public void UserInfo(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string token = request.QueryString["token"];

            Dictionary<string, object> result = new Dictionary<string, object>();
            string uid = UserHandler.GetUidByToken(token);
            if (uid != null)
            {
                User user = UserService.GetUserByID(int.Parse(uid));
                if (user != null)
                {
                    result.Add("code", 0);
                    result.Add("data", user.ToJson());
                }
                else
                {
                    result.Add("code", -1);
                    result.Add("data", "user not found!");
                }
            }
            else
            {
                result.Add("code", -1);
                result.Add("data", "operate fail");
            }
            Response(context, result);
        }

        [RouteAttribute("/server/roominfo")]
        public void RoomInfo(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string id = request.QueryString["id"];

            Dictionary<string, object> result = new Dictionary<string, object>();

            Room room = RoomService.GetRoomByID(int.Parse(id));
            if (room != null)
            {
                result.Add("code", 0);
                result.Add("data", room.ToJson());
            }
            else
            {
                result.Add("code", -1);
                result.Add("msg", "operate fail");
            }

            Response(context, result);
        }
        
        [RouteAttribute("/server/roomend")]
        public void RoomEnd(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string roomid = request.QueryString["roomid"];
            string createtime = request.QueryString["createtime"];

            //读取客户端发送过来的数据
            string filename = "";
            using (Stream body = request.InputStream)
            {
                if (!Directory.Exists("frame")) Directory.CreateDirectory("frame");
                filename = "frame/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bin";
                FileStream fs = new FileStream(filename, FileMode.Create);
                byte[] buff = new byte[1024];
                int count = 0;
                while ((count = body.Read(buff, 0, 1024)) != 0)
                {
                    fs.Write(buff, 0, count);
                }
                fs.Close();
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            Record record = RecordService.AddRecord(int.Parse(roomid), createtime, filename);
            if (record != null)
            {
                result.Add("code", 0);
                result.Add("data", new Dictionary<string, object>());
            }
            else
            {
                result.Add("code", -1);
                result.Add("msg", "operate fail");
            }

            Response(context, result);
        }

        [RouteAttribute("/server/scene")]
        public void Scene(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string name = request.QueryString["name"];
            string description = request.QueryString["description"];

            //读取客户端发送过来的数据
            string filename = "";
            using (Stream body = request.InputStream)
            {
                if (!Directory.Exists("scene")) Directory.CreateDirectory("scene");
                filename = "scene/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip";
                FileStream fs = new FileStream(filename, FileMode.Create);
                byte[] buff = new byte[1024];
                int count = 0;
                while ((count = body.Read(buff, 0, 1024)) != 0)
                {
                    fs.Write(buff, 0, count);
                }
                fs.Close();
            }

            Dictionary<string, object> result = new Dictionary<string, object>();
            Scene scene = SceneService.AddScene(name, description, filename);
            if (scene != null)
            {
                result.Add("code", 0);
                result.Add("data", new Dictionary<string, object>());
            }
            else
            {
                result.Add("code", -1);
                result.Add("msg", "operate fail");
            }

            Response(context, result);
        }
    }
}