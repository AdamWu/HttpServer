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
                Console.WriteLine(string.Format("server started at {0}:{1}", _ip, _port));
                
                routeManager.Init();

            } catch (Exception e)
            {
                throw new Exception(e.Message);
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
        
        public async void Listen()
        {
            while (true)
            {
                var context = await _listener.GetContextAsync();
                Console.WriteLine("Client connected");
                Task.Factory.StartNew(() => ProcessRequest(context));
            }
        }

        private void ProcessRequest(IAsyncResult result)
        {
            HttpListenerContext context = null;
            try
            {
                _listener.BeginGetContext(ProcessRequest, null);
                context = _listener.EndGetContext(result);
                
                string url = context.Request.Url.AbsolutePath;

                RouteAction ra = routeManager.GetRoute(url);
                if(ra != null)
                {
                    context.Response.StatusCode = 200;
                    //object obj = Activator.CreateInstance(ra.Type);
                    //ra.Method.Invoke(obj, new object[] { context });
                } else
                {
                    context.Response.StatusCode = 404;
                }

                context.Response.Close();

                Console.WriteLine(string.Format("[{0}] {1} {2} status:{3}", DateTime.Now.ToString(), context.Request.HttpMethod, url, context.Response.StatusCode));
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 400;
                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write(e.Message);
                    writer.Close();
                }
            }
        }

        private void ProcessRequest(object o)
        {
            HttpListenerContext context = (HttpListenerContext)o;

            string url = context.Request.Url.AbsolutePath;

            RouteAction ra = routeManager.GetRoute(url);
            if (ra != null)
            {
                context.Response.StatusCode = 200;
                object obj = Activator.CreateInstance(ra.Type);
                ra.Method.Invoke(obj, new object[] { context });
            }
            else
            {
                context.Response.StatusCode = 404;
            }

            context.Response.Close();

            Console.WriteLine(string.Format("[{0}] {1} {2} status:{3}", DateTime.Now.ToString(), context.Request.HttpMethod, url, context.Response.StatusCode));
        }
    }
}