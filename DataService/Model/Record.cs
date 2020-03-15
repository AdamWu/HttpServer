using System;
using System.Collections.Generic;

public class Record
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string File { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime EndTime { get; set; }

    public Dictionary<string, object> ToJson()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("id", ID);
        dic.Add("name", Name);
        dic.Add("file", File);
        dic.Add("create_time", CreateTime);
        dic.Add("end_time", EndTime);
        return dic;
    }
}