using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public class HttpServer
    {
        private HttpListener _listener;
        
        private string _ip;
        private int _port;

        private RouteManager routeManager;

        public HttpServer(string ip, int port)
        {
            _ip = ip;
            _port = port;

            routeManager = new RouteManager();
        }

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(string.Format("http://{0}:{1}/", _ip, _port.ToString()));
            try
            {
                _listener.Start();
                Logger.Info(string.Format("server started at {0}:{1}", _ip, _port));
                
                routeManager.Init();

            } catch (Exception e)
            {
                Logger.Error(e.ToString());
                return;
            }
            //_listener.BeginGetContext(ProcessRequest, null);
            
            while (true)
            {
                HttpListenerContext ctx = _listener.GetContext();
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRequest), ctx);
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }
        
        private void ProcessRequest(object o)
        {
            HttpListenerContext context = (HttpListenerContext)o;

            var request = context.Request;
            var response = context.Response;
            string url = context.Request.Url.AbsolutePath;

            RouteAction ra = routeManager.GetRoute(url);
            if (ra != null)
            {
                response.StatusCode = 200;
                object obj = Activator.CreateInstance(ra.Type);
                ra.Method.Invoke(obj, new object[] { context });
            }
            else
            {
                // 静态资源
                string filePath = Environment.CurrentDirectory+ url;
                if (File.Exists(filePath))
                {
                    response.StatusCode = 200;
                    string exeName = Path.GetExtension(filePath);
                    response.ContentType = GetContentType(exeName);
                    FileStream fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                    int byteLength = (int)fileStream.Length;
                    byte[] fileBytes = new byte[byteLength];
                    fileStream.Read(fileBytes, 0, byteLength);
                    fileStream.Close();
                    fileStream.Dispose();
                    response.ContentLength64 = byteLength;
                    response.OutputStream.Write(fileBytes, 0, byteLength);
                    response.OutputStream.Close();
                }
                else
                {
                    response.StatusCode = 404;
                    response.ContentLength64 = 0;
                }
            }

            Logger.Info(string.Format("{0} {1} status:{2}", context.Request.HttpMethod, url, context.Response.StatusCode));

            context.Response.Close();

        }

        /// 获取文件对应MIME类型  
        protected string GetContentType(string fileExtention)
        {
            if (string.Compare(fileExtention, ".html", true) == 0 || string.Compare(fileExtention, ".htm", true) == 0)
                return "text/html;charset=utf-8";
            else if (string.Compare(fileExtention, ".js", true) == 0)
                return "application/javascript";
            else if (string.Compare(fileExtention, ".css", true) == 0)
                return "application/javascript";
            else if (string.Compare(fileExtention, ".png", true) == 0)
                return "image/png";
            else if (string.Compare(fileExtention, ".jpg", true) == 0 || string.Compare(fileExtention, ".jpeg", true) == 0)
                return "image/jpeg";
            else if (string.Compare(fileExtention, ".gif", true) == 0)
                return "image/gif";
            else
                return "application/octet-stream";
        }
    }
}