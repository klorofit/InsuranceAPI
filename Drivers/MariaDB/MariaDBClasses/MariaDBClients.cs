using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.MariaDB;
using MySqlConnector;

namespace MyAPP.Driver.MariaDB;

class MariaDBClients : IDBClients
{
    private Clients FromReader(MySqlDataReader reader)
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
        string query = "DELETE FROM Clients WHERE ClientsID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public Clients? Get(int id)
    {
        string query = "SELECT ClientsID, FirstName, LastName, Phone, Email, Passport, BirthDate FROM Clients WHERE ClientsID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<Clients> Get()
    {
        string query = "SELECT ClientsID, FirstName, LastName, Phone, Email, Passport, BirthDate FROM Clients";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, Clients item)
    {
        string query = @"UPDATE Clients
                        SET FirstName = @FirstName, 
                            LastName = @LastName, 
                            Phone = @Phone, 
                            Email = @Email,
                            Passport = @Passport,
                            BirthDate = @BirthDate
                        WHERE ClientsID = @ClientsID";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@ClientsID", item.ClientsID),
            new MySqlParameter("@FirstName", item.FirstName),
            new MySqlParameter("@LastName", item.LastName),
            new MySqlParameter("@Phone", item.Phone),
            new MySqlParameter("@Email", item.Email),
            new MySqlParameter("@Passport", item.Passport),
            new MySqlParameter("@BirthDate", item.BirthDate)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(Clients item)
    {
        string query = "INSERT INTO Clients (ClientsID, FirstName, LastName, Phone, Email, Passport, BirthDate) VALUES (@ClientsID, @FirstName, @Phone, @Email, @Passport, @BirthDate)";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@ClientsID", item.ClientsID),
            new MySqlParameter("@FirstName", item.FirstName),
            new MySqlParameter("@LastName", item.LastName),
            new MySqlParameter("@Phone", item.Phone),
            new MySqlParameter("@Email", item.Email),
            new MySqlParameter("@Passport", item.Passport),
            new MySqlParameter("@BirthDate", item.BirthDate)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
