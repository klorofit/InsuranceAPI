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
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public Agents? Get(int id)
    {
        string query = "SELECT AgentsID, FirstName, LastName, Phone, Email, HireDate FROM Agents WHERE AgentsID = @id";
        return DAO.Instance.ReadSingle(query, FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<Agents> Get()
    {
        return DAO.Instance.ExecuteReader("SELECT AgentsID, FirstName, LastName, Phone, Email, HireDate FROM Agents", FromReader);
    }
    public void Put(int id, Agents item)
    {
        string query = @"UPDATE Agents SET FirstName=@FirstName, LastName=@LastName, Phone=@Phone, Email=@Email, HireDate=@HireDate WHERE AgentsID=@AgentsID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@AgentsID", id),
            new SqliteParameter("@FirstName", item.FirstName),
            new SqliteParameter("@LastName", item.LastName),
            new SqliteParameter("@Phone", (object?)item.Phone ?? DBNull.Value),
            new SqliteParameter("@Email", item.Email),
            new SqliteParameter("@HireDate", item.HireDate)
        });
    }
    public void Post(Agents item)
    {
        string query = @"INSERT INTO Agents (FirstName, LastName, Phone, Email, HireDate)
                         VALUES (@FirstName, @LastName, @Phone, @Email, @HireDate)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@FirstName", item.FirstName),
            new SqliteParameter("@LastName", item.LastName),
            new SqliteParameter("@Phone", (object?)item.Phone ?? DBNull.Value),
            new SqliteParameter("@Email", item.Email),
            new SqliteParameter("@HireDate", item.HireDate)
        });
    }
}
