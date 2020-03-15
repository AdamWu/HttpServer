using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

public class RoomService
{
    static Room ConvertFromDataRow(DataRow row)
    {
        Room room = new Room();
        room.ID = Convert.ToInt32(row["id"]);
        room.Title = row["title"].ToString();
        room.Description = row["description"].ToString();
        room.Time = Convert.ToInt32(row["time"]);
        room.Attendance = Convert.ToInt32(row["attendance"]);
        room.State = Convert.ToInt32(row["state"]);
        room.CreateTime = (DateTime)row["create_time"];
        return room;
    }

    public static List<Room> GetAllRooms()
    {
        string sql = "select * from room where is_deleted=0";
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);

        List<Room> rooms = new List<Room>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Room room = ConvertFromDataRow(row);
            rooms.Add(room);
        }
        return rooms;
    }

    public static Room GetRoomByID(int id)
    {
        string sql = string.Format("select * from room where id={0}", id);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            Room room = ConvertFromDataRow(row);
            return room;
        }
        return null;
    }

    public static List<Room> GetRoomByState(int state)
    {
        List<Room> rooms = new List<Room>();
        string sql = string.Format("select * from room where state={0}", state);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Room room = ConvertFromDataRow(row);
            rooms.Add(room);
        }
        return rooms;
    }

    public static Room AddRoom(int userid, string title, string description, int time, int attendance)
    {
        MySqlConnection connection = MysqlHelper.CreateConnection();

        string sql = string.Format("insert into room(title, description, time, attendance, user_id) values('{0}','{1}',{2},{3},{4})", title, description, time, attendance, userid);
        int rows = MysqlHelper.ExecuteNonQuery(connection, sql);
        if (rows > 0)
        {
            MySqlDataReader reader = MysqlHelper.ExecuteReader(connection, "select LAST_INSERT_ID()");
            reader.Read();
            int id = reader.GetInt32(0);
            reader.Close();

            MysqlHelper.CloseConnection(connection);
            return GetRoomByID(id);
        }
        MysqlHelper.CloseConnection(connection);

        return null;
    }

    public static bool SoftDeleteRoom(int id)
    {
        string sql = string.Format("update room SET is_deleted=1 where id={0}", id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

    public static bool DeleteRoom(int id)
    {
        string sql = string.Format("delete from room where id={0}", id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

    public static bool UpdateRoomStateByID(int id, int state)
    {
        string sql = string.Format("update room SET state={0} where id={1}", state, id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }
}