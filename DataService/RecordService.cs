using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

public class RecordService
{
    public static List<Record> GetAllRecords()
    {
        string sql = "select record.*,room.title from record LEFT JOIN room ON record.room_id=room.id where record.is_deleted=0";
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);

        List<Record> records = new List<Record>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            Record record = new Record();
            record.ID = Convert.ToInt32(row["id"]);
            record.File = row["file"].ToString();
            record.Name = row["title"].ToString();
            record.CreateTime = (DateTime)row["create_time"];
            record.EndTime = (DateTime)row["end_time"];
            records.Add(record);
        }
        return records;
    }

    public static Record GetRecordByID(int id)
    {
        string sql = string.Format("select record.*, room.title from record LEFT JOIN room ON record.room_id=room.id where record.id={0}", id);
        DataSet ds = MysqlHelper.ExecuteDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            Record record = new Record();
            record.ID = Convert.ToInt32(row["id"]);
            record.File = row["file"].ToString();
            record.Name = row["title"].ToString();
            record.CreateTime = (DateTime)row["create_time"];
            record.EndTime = (DateTime)row["end_time"];
            return record;
        }
        return null;
    }
    
    public static Record AddRecord(int roomid, string creattime, string file)
    {
        MySqlConnection connection = MysqlHelper.CreateConnection();

        string sql = string.Format("insert into record(room_id, create_time, file) values({0},'{1}','{2}')", roomid, creattime, file);
        int rows = MysqlHelper.ExecuteNonQuery(connection, sql);
        if (rows > 0)
        {
            MySqlDataReader reader = MysqlHelper.ExecuteReader(connection, "select LAST_INSERT_ID()");
            reader.Read();
            int id = reader.GetInt32(0);
            reader.Close();

            MysqlHelper.CloseConnection(connection);

            return GetRecordByID(id);
        }
        
        return null;
    }

    public static bool SoftDeleteRecord(int id)
    {
        string sql = string.Format("update record SET is_deleted=1 where id={0}", id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

    public static bool DeleteRecord(int id)
    {
        string sql = string.Format("delete from record where id={0}", id);
        int rows = MysqlHelper.ExecuteNonQuery(sql);
        if (rows > 0)
        {
            return true;
        }
        return false;
    }

}