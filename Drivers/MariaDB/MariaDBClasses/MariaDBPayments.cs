using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.MariaDB;
using MySqlConnector;

namespace MyAPP.Driver.MariaDB;

class MariaDBPayments : IDBPayments
{
    private Payments FromReader(MySqlDataReader reader)
    {
        return new Payments
        {
            PaymentsID = (int)reader.GetInt64("PaymentsID"),
            Method = reader.GetString("Method"),
            Reference = reader.GetString("Reference"),
            Amount = (float)reader.GetDouble("Amount"),
            PoliciesID = (int)reader.GetInt64("PoliciesID"),
            PaymentDate = reader.GetDateTime("PaymentDate")
        };
    }
    public void Delete(int id)
    {
        string query = "DELETE FROM Payments WHERE PaymentsID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public Payments? Get(int id)
    {
        string query = "SELECT PaymentsID, Method, Reference, Amount, PoliciesID, PaymentDate FROM Payments WHERE PaymentsID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<Payments> Get()
    {
        string query = "SELECT PaymentsID, Method, Reference, Amount, PoliciesID, PaymentDate FROM Payments";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, Payments item)
    {
        string query = @"UPDATE Payments
                        SET Method = @Method, 
                            Reference = @Reference, 
                            Amount = @Amount, 
                            PoliciesID = @PoliciesID,
                            PaymentDate = @PaymentDate
                        WHERE PaymentsID = @PaymentsID";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@PaymentsID", item.PaymentsID),
            new MySqlParameter("@Method", item.Method),
            new MySqlParameter("@Reference", item.Reference),
            new MySqlParameter("@Amount", item.Amount),
            new MySqlParameter("@PoliciesID", item.PoliciesID),
            new MySqlParameter("@PaymentDate", item.PaymentDate)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(Payments item)
    {
        string query = "INSERT INTO Payments (PaymentsID, Method, Reference, Amount, PoliciesID, PaymentDate) VALUES (@PaymentsID, @Method, @Amount, @PoliciesID, @PaymentDate)";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@PaymentsID", item.PaymentsID),
            new MySqlParameter("@Method", item.Method),
            new MySqlParameter("@Reference", item.Reference),
            new MySqlParameter("@Amount", item.Amount),
            new MySqlParameter("@PoliciesID", item.PoliciesID),
            new MySqlParameter("@PaymentDate", item.PaymentDate)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
