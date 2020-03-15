using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

public class SceneService
{
    public static List<Scene> GetAllScenes()
    {
        string sql = "select * from scene where is_deleted=0";
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);

        List<Scene> scenes = new List<Scene>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Scene scene = new Scene();
            scene.ID = Convert.ToInt32(row["id"]);
            scene.Name = row["name"].ToString();
            scene.Description = row["description"].ToString();
            scene.File = row["file"].ToString();
            scene.CreateTime = (DateTime)row["create_time"];
            scenes.Add(scene);
        }
        return scenes;
    }

    public static Scene GetSceneByID(int id)
    {
        string sql = string.Format("select * from scene  where id={0}", id);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            Scene scene = new Scene();
            scene.ID = Convert.ToInt32(row["id"]);
            scene.Name = row["name"].ToString();
            scene.Description = row["description"].ToString();
            scene.File = row["file"].ToString();
            scene.CreateTime = (DateTime)row["create_time"];
            return scene;
        }
        return null;
    }

    public static Scene AddScene(string name, string description, string file)
    {
        MySqlConnection connection = MysqlHelper.CreateConnection();

        string sql = string.Format("insert into scene(name, description, file) values('{0}','{1}','{2}')", name, description, file);
        int rows = MysqlHelper.ExecuteNonQuery(connection, sql);
        if (rows > 0)
        {
            MySqlDataReader reader = MysqlHelper.ExecuteReader(connection, "select LAST_INSERT_ID()");
            reader.Read();
            int id = reader.GetInt32(0);
            reader.Close();

            MysqlHelper.CloseConnection(connection);

            return GetSceneByID(id);
        }

        return null;
    }

    public static bool SoftDeleteScene(int id)
    {
        string sql = string.Format("update scene SET is_deleted=1 where id={0}", id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

    public static bool DeleteScene(int id)
    {
        string sql = string.Format("delete from scene where id={0}", id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

}