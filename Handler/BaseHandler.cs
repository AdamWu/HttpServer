using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace HttpServer
{
    public class BaseHandler : IRouteHandler
    {
        public void Response(HttpListenerContext context, string json)
        {
            context.Response.ContentType = "application/json;charset=UTF-8";
            context.Response.ContentEncoding = Encoding.UTF8;
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(json);
                writer.Close();
            }
         }

        public void Response(HttpListenerContext context, Dictionary<string, object> dic)
        {
            Response(context, MiniJSON.Json.Serialize(dic));
        }

        public void ResponseTokenInvalid(HttpListenerContext context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", -1);
            dic.Add("msg", "token invalid!");

            Response(context, dic);
        }

        public void ResponseParameterInvalid(HttpListenerContext context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", -1);
            dic.Add("msg", "parameter invalid!");

            Response(context, dic);
        }

    }

}