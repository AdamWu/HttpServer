using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Data;

namespace HttpServer
{

    public class RecordHandler : BaseHandler
    {
        [RouteAttribute("/record/list")]
        public void GetAllRecords(HttpListenerContext context)
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

            List<Record> records = RecordService.GetAllRecords();
            for (int i = 0; i < records.Count; i ++)
            {
                Record record = records[i];
                data.Add(record.ToJson());
            }
            
            Response(context, result);
        }

        [RouteAttribute("/record/get")]
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

            Record record = RecordService.GetRecordByID(int.Parse(id));
            if (record != null)
            {
                result.Add("code", 0);
                result.Add("data", record.ToJson());
            }
            else
            {
                result.Add("code", -1);
                result.Add("msg", "operate fail");
            }

            Response(context, result);
        }

        [RouteAttribute("/record/add")]
        public void AddRecord(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
        }

        [RouteAttribute("/record/delete")]
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
            bool success = RecordService.SoftDeleteRecord(int.Parse(id));
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