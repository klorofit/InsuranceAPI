using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;

namespace MyAPP.Driver.SQLiteDAO;

class DBPayments : IDBPayments
{
    private Payments FromReader(SqliteDataReader reader)
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
        DAO.Instance.ExecuteNonQuery("DELETE FROM Payments WHERE PaymentsID = @id",
            new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public Payments? Get(int id)
    {
        return DAO.Instance.ReadSingle(
            "SELECT PaymentsID, Method, Reference, Amount, PoliciesID, PaymentDate FROM Payments WHERE PaymentsID = @id",
            FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<Payments> Get()
    {
        return DAO.Instance.ExecuteReader(
            "SELECT PaymentsID, Method, Reference, Amount, PoliciesID, PaymentDate FROM Payments", FromReader);
    }
    public void Put(int id, Payments item)
    {
        string query = @"UPDATE Payments SET Method=@Method, Reference=@Reference, Amount=@Amount, PoliciesID=@PoliciesID, PaymentDate=@PaymentDate WHERE PaymentsID=@PaymentsID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@PaymentsID", id),
            new SqliteParameter("@Method", item.Method),
            new SqliteParameter("@Reference", item.Reference),
            new SqliteParameter("@Amount", item.Amount),
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@PaymentDate", item.PaymentDate)
        });
    }
    public void Post(Payments item)
    {
        string query = @"INSERT INTO Payments (Method, Reference, Amount, PoliciesID, PaymentDate)
                         VALUES (@Method, @Reference, @Amount, @PoliciesID, @PaymentDate)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@Method", item.Method),
            new SqliteParameter("@Reference", item.Reference),
            new SqliteParameter("@Amount", item.Amount),
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@PaymentDate", item.PaymentDate)
        });
    }
}
