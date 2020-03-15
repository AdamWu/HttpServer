using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Data;

namespace HttpServer
{
    public class SceneHandler : BaseHandler
    {
        [RouteAttribute("/scene/list")]
        public void GetAllScenes(HttpListenerContext context)
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
            result.Add("code", 0);
            List<object> data = new List<object>();
            result.Add("data", data);

            List<Scene> scenes = SceneService.GetAllScenes();
            for (int i = 0; i < scenes.Count; i ++)
            {
                Scene scene = scenes[i];
                data.Add(scene.ToJson());
            }
            
            Response(context, result);
        }

        [RouteAttribute("/scene/get")]
        public void AddUser(HttpListenerContext context)
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

            Scene scene = SceneService.GetSceneByID(int.Parse(id));
            if (scene != null)
            {
                result.Add("code", 0);
                result.Add("data", scene.ToJson());
            }
            else
            {
                result.Add("code", -1);
                result.Add("msg", "operate fail");
            }

            Response(context, result);
        }

        [RouteAttribute("/scene/delete")]
        public void DeleteUser(HttpListenerContext context)
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
            bool success = SceneService.SoftDeleteScene(int.Parse(id));
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
            Response(context, result);
        }
        
    }
}