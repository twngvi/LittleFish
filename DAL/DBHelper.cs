using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DBHelper
{
    private SqlConnection _connection;

    public DBHelper()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        _connection = new SqlConnection(connectionString);
    }

    public SqlConnection GetConnection()
    {
        return _connection;
    }

    public void OpenConnection()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
    }

    public void CloseConnection()
    {
        if (_connection.State != ConnectionState.Closed)
            _connection.Close();
    }

    public DataTable ExecuteQuery(string query)
    {
        SqlDataAdapter adapter = new SqlDataAdapter(query, _connection);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }
}
