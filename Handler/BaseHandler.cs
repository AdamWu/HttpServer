using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Data;
using Newtonsoft.Json;

namespace HttpServer
{
    public class BaseHandler : IRouteHandler
    {
        public void Response(HttpListenerContext context, string json)
        {
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(json);
                writer.Close();
            }
        }

        public void Response(HttpListenerContext context, Dictionary<string, object> dic)
        {
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(JsonConvert.SerializeObject(dic));
                writer.Close();
            }
        }

        public void ResponseTokenInvalid(HttpListenerContext context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", -1);
            dic.Add("msg", "token invalid!");

            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(JsonConvert.SerializeObject(dic));
                writer.Close();
            }
        }

        public void ResponseParameterInvalid(HttpListenerContext context)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", -1);
            dic.Add("msg", "parameter invalid!");

            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write(JsonConvert.SerializeObject(dic));
                writer.Close();
            }
        }

    }

}