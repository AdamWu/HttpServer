using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

public class UserService
{
    static User ConvertFromDataRow(DataRow row)
    {
        User user = new User();
        user.ID = Convert.ToInt32(row["id"]);
        user.Name = row["name"].ToString();
        user.Type = Convert.ToInt32(row["type"]);
        user.CreateTime = (DateTime)row["create_time"];
        user.LastLoginTime = (DateTime)row["last_login_time"];
        return user;
    }

    public static List<User> GetAllUsers()
    {
        string sql = "select * from user";
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);

        List<User> users = new List<User>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            User user = ConvertFromDataRow(row);
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
            User user = ConvertFromDataRow(row);
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
            User user = ConvertFromDataRow(row);
            return user;
        }
        return null;
    }

    public static User GetUserByNameAndPassword(string name, string password, int type)
    {
        string sql = string.Format("select * from user where name='{0}' and password='{1}' and type={2}", name, password, type);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            User user = ConvertFromDataRow(row);
            return user;
        }
        return null;
    }

    public static bool UpdateLoginTimeByID(int id)
    {
        string sql = string.Format("update user SET last_login_time='{0}' where id={1}", DateTime.Now.ToString(), id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

    public static User AddUser(string name, string password, int type=0)
    {
        MySqlConnection connection = MysqlHelper.CreateConnection();

        string sql = string.Format("insert into user(name, password, type) values('{0}','{1}',{2})", name, password,type);
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