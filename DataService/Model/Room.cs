using System;
using System.Collections.Generic;

public class Room
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Time { get; set; }
    public int Attendance { get; set; }
    public int State { get; set; }

    public DateTime CreateTime { get; set; }
    
    public Dictionary<string, object> ToJson()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("id", ID);
        dic.Add("title", Title);
        dic.Add("description", Description);
        dic.Add("time", Time);
        dic.Add("attendance", Attendance);
        dic.Add("state", State);
        dic.Add("create_time", CreateTime);
        return dic;
    }
}