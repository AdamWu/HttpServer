using System;
using System.Collections.Generic;

public class Scene
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string File { get; set; }

    public DateTime CreateTime { get; set; }

    public Dictionary<string, object> ToJson()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("id", ID);
        dic.Add("name", Name);
        dic.Add("description", Description);
        dic.Add("file", File);
        dic.Add("create_time", CreateTime);
        return dic;
    }
}