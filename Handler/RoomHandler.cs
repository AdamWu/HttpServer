using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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

            // 结果
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("code", 0);
            List<object> data = new List<object>();
            result.Add("data", data);

            List<Room> rooms = RoomService.GetAllRooms();
            for (int i = 0; i < rooms.Count; i ++)
            {
                Room room = rooms[i];
                data.Add(room.ToJson());
            }

            Response(context, result);
        }

        [RouteAttribute("/room/add")]
        public void AddRoom(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string title = request.QueryString["title"];
            string description = request.QueryString["description"];
            string time = request.QueryString["time"];
            string attendance = request.QueryString["attendance"];

            // 验证token
            string token = request.QueryString["token"];
            if (!UserHandler.ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }
            if (title == null || description == null || time==null || attendance==null)
            {
                ResponseParameterInvalid(context);
                return;
            }
            title = Encoding.GetEncoding("utf-8").GetString(request.ContentEncoding.GetBytes(title));
            description = Encoding.GetEncoding("utf-8").GetString(request.ContentEncoding.GetBytes(description));
            string uid = UserHandler.GetUidByToken(token);

            Dictionary<string, object> result = new Dictionary<string, object>();
            Room room = RoomService.AddRoom(int.Parse(uid), title, description, int.Parse(time), int.Parse(attendance));
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
            
            Dictionary<string, object> result = new Dictionary<string, object>();

            bool success = RoomService.SoftDeleteRoom(int.Parse(id));
            if (success)
            {
                result.Add("code", 0);
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("id", int.Parse(id));
                result.Add("data", data);
            } else
            {
                result.Add("code", -1);
                result.Add("msg", "operate fail");
            }
            Response(context, result);
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

        [RouteAttribute("/room/inprogress")]
        public void GetRoomInProgress(HttpListenerContext context)
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

            Dictionary<string, object> result = new Dictionary<string, object>();

            List<Room> rooms = RoomService.GetRoomByState(1);
            if (rooms.Count > 0)
            {
                result.Add("code", 0);
                result.Add("data", rooms[0].ToJson());
            }
            else
            {
                result.Add("code", -1);
                result.Add("msg", "no exam");
            }

            Response(context, result);
        }

        [RouteAttribute("/room/begin")]
        public void RoomBegin(HttpListenerContext context)
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

            Dictionary<string, object> result = new Dictionary<string, object>();

            Room room = RoomService.GetRoomByID(int.Parse(id));
            if (room != null)
            {
                if (room.State == 0)
                {
                    bool success = RoomService.UpdateRoomStateByID(int.Parse(id), 1);
                    if (success)
                    {
                        result.Add("code", 0);
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        data.Add("id", int.Parse(id));
                        result.Add("data", data);
                    } else
                    {
                        result.Add("code", -1);
                        result.Add("msg", "operate fail");
                    }
                } else
                {
                    result.Add("code", -1);
                    result.Add("msg", "already begin");
                }
            }
            else
            {
                result.Add("code", -1);
                result.Add("msg", "operate fail");
            }

            Response(context, result);
        }

        [RouteAttribute("/room/end")]
        public void RoomEnd(HttpListenerContext context)
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

            Dictionary<string, object> result = new Dictionary<string, object>();

            Room room = RoomService.GetRoomByID(int.Parse(id));
            if (room != null)
            {
                if (room.State == 1)
                {
                    bool success = RoomService.UpdateRoomStateByID(int.Parse(id), 0);
                    if (success)
                    {
                        result.Add("code", 0);
                        Dictionary<string, object> data = new Dictionary<string, object>();
                        data.Add("id", int.Parse(id));
                        result.Add("data", data);
                    }
                    else
                    {
                        result.Add("code", -1);
                        result.Add("msg", "operate fail");
                    }
                }
                else
                {
                    result.Add("code", -1);
                    result.Add("msg", "already end");
                }
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