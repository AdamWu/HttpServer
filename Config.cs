using System;
using System.IO;
using System.Collections.Generic;

public class Config
{
    Dictionary<string, string> config = new Dictionary<string, string>();
    
    public string GetValue(string key)
    {
        if (config.ContainsKey(key)) return config[key];
        return null;
    }

    public bool Init()
    {
        // ¶ÁÈ¡¶Ë¿ÚºÅ
        string filename = Environment.CurrentDirectory + "/config.ini";
        if (!File.Exists(filename))
        {
            Logger.Error("config file not found!");
            return false;
        }
        string[] lines = File.ReadAllLines(filename);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] strs = lines[i].Split('=');
            if (strs.Length == 2)
            {
                config[strs[0]] = strs[1];
            }
        }

        return true;
    }
}