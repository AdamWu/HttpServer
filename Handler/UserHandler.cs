using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Data;
using Newtonsoft.Json;

namespace HttpServer
{
    public class Token
    {
        const int EXPIRE = 60 * 2;
        //const int EXPIRE = 60 * 60 * 24;// 24h

        public string Uid;
        public string Key;
        public int ExpireTime;

        public Token(string uid, string key)
        {
            Uid = uid;
            Key = key;
            ExpireTime = (int)(DateTime.Now-new DateTime(1970,1,1)).TotalSeconds + EXPIRE;// 24h
        }
    }

    public class UserHandler : BaseHandler
    {
        // token
        static Dictionary<string, Token> Sessions = new Dictionary<string, Token>();
        
        Token GetToken(string uid)
        {
            foreach (string key in Sessions.Keys)
            {
                Token token = Sessions[key];
                if(token.Uid == uid) return token;
            }
            return null;
        }

        Token GenerateToken(string uid)
        {
            string str = uid.ToString() + System.DateTime.Now.ToString();
            string result = "";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));

            for (int i = 0; i < bytResult.Length; i++)
            {
                //16进制转换 
                string temps = bytResult[i].ToString("x");
                if (temps.Length == 1)
                {
                    temps = "0" + temps;
                }
                result = result + temps;
            }
            Token token = new Token(uid, result);
            Sessions.Add(result, token);
            return token;
        }
        
        static public bool ValidateToken(string key)
        {
            if (key == null) return false;
            if (Sessions.ContainsKey(key))
            {
                Token token = Sessions[key];
                double time = (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
                if (token.ExpireTime > time)
                {
                    return true;
                }
            }
            return false;
        }

        [RouteAttribute("/user/token")]
        public void ValideToken(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            // 验证token
            string token = request.QueryString["token"];
            if (!ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 0);
            dic.Add("data", new Dictionary<string, object>());

            Response(context, dic);
        }
        
        [RouteAttribute("/user/list")]
        public void GetAllUsers(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string sql = "select * from user";
            DataSet ds = MysqlHelper.ExecuteDataSet(sql);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 0);
            List<object> data = new List<object>();
            dic.Add("data", data);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Dictionary<string, object> user = new Dictionary<string, object>();
                user.Add("id", row["id"]);
                user.Add("name", row["name"]);
                data.Add(user);
            }
            
            Response(context, dic);
        }

        [RouteAttribute("/user/add")]
        public void AddUser(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            string name = request.QueryString["name"];
            string pwd = request.QueryString["password"];

            // 验证token
            string token = request.QueryString["token"];
            if (!ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }
            if (name == null || pwd == null)
            {
                ResponseParameterInvalid(context);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            string sql = string.Format("select * from user where name='{0}'", name);
            DataSet ds = MysqlHelper.ExecuteDataSet(sql);
            if(ds.Tables[0].Rows.Count == 0)
            {
                sql = string.Format("insert into user(name, password) values('{0}','{1}')",name, pwd);
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
            } else
            {
                dic.Add("code", -1);
                dic.Add("msg", "name already exists");
            }

            Response(context, dic);
        }

        [RouteAttribute("/user/delete")]
        public void DeleteUser(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string id = request.QueryString["id"];

            // 验证token
            string token = request.QueryString["token"];
            if (!ValidateToken(token))
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
            string sql = string.Format("delete from user where id={0}", id);
            int rows = MysqlHelper.ExecuteNonQuery(sql);
            if (rows > 0)
            {
                dic.Add("code", 0);
                dic.Add("data", new Dictionary<string, object>());
            }
            else
            {
                dic.Add("code", -1);
                dic.Add("msg", "operate fail");
            }
            Response(context, dic);
        }
        
        [RouteAttribute("/user/login")]
        public void Login(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string name = request.QueryString["name"];
            string pwd = request.QueryString["password"];

            if (name == null || pwd == null)
            {
                ResponseParameterInvalid(context);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            string sql = string.Format("select id from user where name='{0}' and password='{1}'", name, pwd);
            DataSet ds = MysqlHelper.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string uid = row.Field<int>("id").ToString();

                Token token = GetToken(uid);
                if (token != null)
                {
                    Sessions.Remove(token.Key);
                }
                token = GenerateToken(uid);

                dic.Add("code", 0);
                Dictionary<string, object> data = new Dictionary<string, object>();
                dic.Add("data", data);
                data.Add("uid", uid);
                data.Add("token", token.Key);
                data.Add("expire_time", token.ExpireTime);
            }
            else
            {
                dic.Add("code", -1);
                dic.Add("msg", "wrong name or password");
            }

            Response(context, dic);
        }

        [RouteAttribute("/user/logout")]
        public void Logout(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            string token = request.QueryString["token"];
            if (!ValidateToken(token))
            {
                ResponseTokenInvalid(context);
                return;
            }

            if (Sessions.ContainsKey(token))
            {
                Sessions.Remove(token);
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("code", 0);
            dic.Add("data", new Dictionary<string, object>());
            Response(context, dic);
        }
    }
}