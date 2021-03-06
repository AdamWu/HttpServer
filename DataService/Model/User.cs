using System;
using System.Collections.Generic;

public class User
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int Type { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime LastLoginTime { get; set; }

    public Dictionary<string, object> ToJson()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("id", ID);
        dic.Add("name", Name);
        dic.Add("type", Type);
        dic.Add("create_time", CreateTime);
        dic.Add("last_login_time", LastLoginTime);
        return dic;
    }
}