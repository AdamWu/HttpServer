using System;
using System.IO;
using System.Collections.Generic;

public class Config
{
    static Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
    
    public static string GetValue(string section, string key)
    {
        if (values.ContainsKey(section))
        {
            if (values[section].ContainsKey(key))
                return values[section][key];
        }
        return null;
    }

    static void SetValue(string section, string key, string value)
    {
        if (!values.ContainsKey(section))
        {
            values.Add(section, new Dictionary<string, string>());
        }
        values[section][key] = value;
    }

    public static bool Init()
    {
        // ¶ÁÈ¡¶Ë¿ÚºÅ
        string filename = Environment.CurrentDirectory + "/config.ini";
        if (!File.Exists(filename))
        {
            return false;
        }
        string[] lines = File.ReadAllLines(filename);
        string section = "";
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                section = line.Substring(1, line.Length-2);
                continue;
            }
            string[] strs = lines[i].Split('=');
            if (strs.Length == 2)
            {
                SetValue(section, strs[0].Trim(), strs[1].Trim());
            }
        }
        return true;
    }
    
}