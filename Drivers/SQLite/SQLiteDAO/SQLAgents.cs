using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;


namespace MyAPP.Driver.SQLiteDAO;

class DBAgents : IDBAgents
{
    private Agents FromReader(SqliteDataReader reader)
    {
        return new Agents
        {
            AgentsID = (int)reader.GetInt64("AgentsID"),
            FirstName = reader.GetString("FirstName"),
            LastName = reader.GetString("LastName"),
            Phone = reader.GetString("Phone"),
            Email = reader.GetString("Email"),
            HireDate = reader.GetDateTime("HireDate")
        };
    }
    public void Delete(int id)
    {
        string query = "DELETE FROM Agents WHERE AgentsID = @id";
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public Agents? Get(int id)
    {
        string query = "SELECT AgentsID, FirstName, LastName, Phone, Email, HireDate FROM Agents WHERE AgentsID = @id";
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<Agents> Get()
    {
        string query = "SELECT AgentsID, FirstName, LastName, Phone, Email, HireDate FROM Agents";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, Agents item)
    {
        string query = @"UPDATE Agents
                        SET FirstName = @FirstName, 
                            LastName = @LastName, 
                            Phone = @Phone, 
                            Email = @Email,
                            HireDate = @HireDate
                        WHERE AgentsID = @AgentsID";
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@AgentsID", item.AgentsID),
            new SqliteParameter("@FirstName", item.FirstName),
            new SqliteParameter("@LastName", item.LastName),
            new SqliteParameter("@Phone", item.Phone),
            new SqliteParameter("@Email", item.Email),
            new SqliteParameter("@HireDate", item.HireDate)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(Agents item)
    {
        string query = "INSERT INTO Agents (AgentsID, FirstName, LastName, Phone, Email HireDate) VALUES (@AgentsID, @FirstName, @LastName, @Phone, @Email @HireDate)";
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@AgentsID", item.AgentsID),
            new SqliteParameter("@FirstName", item.FirstName),
            new SqliteParameter("@LastName", item.LastName),
            new SqliteParameter("@Phone", item.Phone),
            new SqliteParameter("@Email", item.Email),
            new SqliteParameter("@HireDate", item.HireDate)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
