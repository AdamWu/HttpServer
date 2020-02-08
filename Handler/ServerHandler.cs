using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Data;
using Newtonsoft.Json;

namespace HttpServer
{
    public class ServerHandler : BaseHandler
    {

        
        [RouteAttribute("/server/register")]
        public void ValideToken(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 0);
            dic.Add("data", new Dictionary<string, object>());

            Response(context, dic);
        }
   
    }
}