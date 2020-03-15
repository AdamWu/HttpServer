using System;
using System.Data;

using MySql.Data.MySqlClient;


class MysqlHelper
{        
    static string connectionString = "server=127.0.0.1;port=3306;user=;password=;database=;";

    public static void Init(string ip, int port,string user, string password, string database)
    {
        connectionString = string.Format("server={0};port={1};user={2};password={3};database={4};", ip, port, user, password, database);
    }

    public static MySqlConnection CreateConnection()
    {
        MySqlConnection conn = new MySqlConnection(connectionString);
        try
        {
            conn.Open();
            return conn;
        }
        catch (MySqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void CloseConnection(MySqlConnection connection)
    {
        if(connection != null)
        {
            connection.Close();
        }
    }

    public static int ExecuteNonQuery(MySqlConnection connection, string sql)
    {
        if (connection.State != ConnectionState.Open) connection.Open();

        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            int rows = cmd.ExecuteNonQuery();
            return rows;
        }
        catch (MySqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }
        
    public static int ExecuteNonQuery(string sql)
    {
        MySqlConnection conn = new MySqlConnection(connectionString);
        try
        {
            conn.Open();

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int rows = cmd.ExecuteNonQuery();
            return rows;
        }
        catch (MySqlException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }

    public static MySqlDataReader ExecuteReader(string sql)
    {
        MySqlConnection conn = new MySqlConnection(connectionString);
        try
        {
            conn.Open();
                
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            return reader;
        }
        catch (MySqlException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }

    public static MySqlDataReader ExecuteReader(MySqlConnection connection, string sql)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        catch (MySqlException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static DataSet ExecuteDataSet(string sql)
    {
        DataSet ds = new DataSet();
        MySqlConnection conn = new MySqlConnection(connectionString);
        try
        {
            conn.Open();

            MySqlDataAdapter cmd = new MySqlDataAdapter(sql, conn);
            cmd.Fill(ds, "ds");
        }
        catch (MySqlException ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            conn.Close();
        }
        return ds;
    }

    public static DataSet ExecuteDataSet(MySqlConnection connection,string sql)
    {
        DataSet ds = new DataSet();
        try
        {
            MySqlDataAdapter cmd = new MySqlDataAdapter(sql, connection);
            cmd.Fill(ds, "ds");
        }
        catch (MySqlException ex)
        {
            throw new Exception(ex.Message);
        }
        return ds;
    }
}