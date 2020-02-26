using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

public class UserService
{
    public static List<User> GetAllUsers()
    {
        string sql = "select * from user";
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);

        List<User> users = new List<User>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            User user = new User();
            user.ID = Convert.ToInt32(row["id"]);
            user.Name = row["name"].ToString();
            users.Add(user);
        }

        return users;
    }

    public static User GetUserByID(int id)
    {
        string sql = string.Format("select * from user where id={0}", id);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            User user = new User();
            user.ID = Convert.ToInt32(row["id"]);
            user.Name = row["name"].ToString();
            return user;
        }
        return null;
    }

    public static User GetUserByName(string name)
    {
        string sql = string.Format("select * from user where name='{0}'", name);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            User user = new User();
            user.ID = Convert.ToInt32(row["id"]);
            user.Name = row["name"].ToString();
            return user;
        }
        return null;
    }

    public static User GetUserByNameAndPassword(string name, string password)
    {
        string sql = string.Format("select * from user where name='{0}' and password='{1}'", name, password);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            User user = new User();
            user.ID = Convert.ToInt32(row["id"]);
            user.Name = row["name"].ToString();
            return user;
        }
        return null;
    }

    public static User AddUser(string name, string password)
    {
        MySqlConnection connection = MysqlHelper.CreateConnection();

        string sql = string.Format("insert into user(name, password) values('{0}','{1}')", name, password);
        int rows = MysqlHelper.ExecuteNonQuery(connection, sql);
        if (rows > 0)
        {
            MySqlDataReader reader = MysqlHelper.ExecuteReader(connection, "select LAST_INSERT_ID()");
            reader.Read();
            int id = reader.GetInt32(0);
            reader.Close();

            MysqlHelper.CloseConnection(connection);

            return GetUserByID(id);
        }
        
        return null;
    }

    public static bool DeleteUser(int id)
    {
        string sql = string.Format("delete from user where id={0}", id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

}