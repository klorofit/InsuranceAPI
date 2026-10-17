using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;

namespace MyAPP.Driver.SQLiteDAO;

class DBClients : IDBClients
{
    private Clients FromReader(SqliteDataReader reader)
    {
        return new Clients
        {
            ClientsID = (int)reader.GetInt64("ClientsID"),
            FirstName = reader.GetString("FirstName"),
            LastName = reader.GetString("LastName"),
            Phone = reader.GetString("Phone"),
            Email = reader.GetString("Email"),
            Passport = reader.GetString("Passport"),
            BirthDate = reader.GetDateTime("BirthDate")
        };
    }
    public void Delete(int id)
    {
        DAO.Instance.ExecuteNonQuery("DELETE FROM Clients WHERE ClientsID = @id",
            new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public Clients? Get(int id)
    {
        return DAO.Instance.ReadSingle(
            "SELECT ClientsID, FirstName, LastName, Phone, Email, Passport, BirthDate FROM Clients WHERE ClientsID = @id",
            FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<Clients> Get()
    {
        return DAO.Instance.ExecuteReader(
            "SELECT ClientsID, FirstName, LastName, Phone, Email, Passport, BirthDate FROM Clients", FromReader);
    }
    public void Put(int id, Clients item)
    {
        string query = @"UPDATE Clients SET FirstName=@FirstName, LastName=@LastName, Phone=@Phone, Email=@Email, Passport=@Passport, BirthDate=@BirthDate WHERE ClientsID=@ClientsID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@ClientsID", id),
            new SqliteParameter("@FirstName", item.FirstName),
            new SqliteParameter("@LastName", item.LastName),
            new SqliteParameter("@Phone", (object?)item.Phone ?? DBNull.Value),
            new SqliteParameter("@Email", item.Email),
            new SqliteParameter("@Passport", (object?)item.Passport ?? DBNull.Value),
            new SqliteParameter("@BirthDate", item.BirthDate)
        });
    }
    public void Post(Clients item)
    {
        string query = @"INSERT INTO Clients (FirstName, LastName, Phone, Email, Passport, BirthDate)
                         VALUES (@FirstName, @LastName, @Phone, @Email, @Passport, @BirthDate)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@FirstName", item.FirstName),
            new SqliteParameter("@LastName", item.LastName),
            new SqliteParameter("@Phone", (object?)item.Phone ?? DBNull.Value),
            new SqliteParameter("@Email", item.Email),
            new SqliteParameter("@Passport", (object?)item.Passport ?? DBNull.Value),
            new SqliteParameter("@BirthDate", item.BirthDate)
        });
    }
}
