using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;

namespace MyAPP.Driver.SQLiteDAO;

class DBPolicies : IDBPolicies
{
    private Policies FromReader(SqliteDataReader reader)
    {
        return new Policies
        {
            PoliciesID = (int)reader.GetInt64("PoliciesID"),
            PolicyNumber = reader.GetString("PolicyNumber"),
            ClientsID = (int)reader.GetInt64("ClientsID"),
            InsuranceID = (int)reader.GetInt64("InsuranceID"),
            AgentsID = (int)reader.GetInt64("AgentsID"),
            StartDate = reader.GetDateTime("StartDate"),
            EndDate = reader.GetDateTime("EndDate"),
            SumInsured = (float)reader.GetDouble("SumInsured"),
            Premium = (float)reader.GetDouble("Premium"),
            Status = reader.GetString("Status")
        };
    }
    public void Delete(int id)
    {
        DAO.Instance.ExecuteNonQuery("DELETE FROM Policies WHERE PoliciesID = @id",
            new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public Policies? Get(int id)
    {
        return DAO.Instance.ReadSingle(
            "SELECT PoliciesID, PolicyNumber, ClientsID, InsuranceID, AgentsID, StartDate, EndDate, SumInsured, Premium, Status FROM Policies WHERE PoliciesID = @id",
            FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<Policies> Get()
    {
        return DAO.Instance.ExecuteReader(
            "SELECT PoliciesID, PolicyNumber, ClientsID, InsuranceID, AgentsID, StartDate, EndDate, SumInsured, Premium, Status FROM Policies", FromReader);
    }
    public void Put(int id, Policies item)
    {
        string query = @"UPDATE Policies SET PolicyNumber=@PolicyNumber, ClientsID=@ClientsID, InsuranceID=@InsuranceID, AgentsID=@AgentsID,
                            StartDate=@StartDate, EndDate=@EndDate, SumInsured=@SumInsured, Premium=@Premium, Status=@Status
                        WHERE PoliciesID=@PoliciesID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@PoliciesID", id),
            new SqliteParameter("@PolicyNumber", item.PolicyNumber),
            new SqliteParameter("@ClientsID", item.ClientsID),
            new SqliteParameter("@InsuranceID", item.InsuranceID),
            new SqliteParameter("@AgentsID", item.AgentsID),
            new SqliteParameter("@StartDate", item.StartDate),
            new SqliteParameter("@EndDate", item.EndDate),
            new SqliteParameter("@SumInsured", item.SumInsured),
            new SqliteParameter("@Premium", item.Premium),
            new SqliteParameter("@Status", item.Status)
        });
    }
    public void Post(Policies item)
    {
        string query = @"INSERT INTO Policies (PolicyNumber, ClientsID, InsuranceID, AgentsID, StartDate, EndDate, SumInsured, Premium, Status)
                         VALUES (@PolicyNumber, @ClientsID, @InsuranceID, @AgentsID, @StartDate, @EndDate, @SumInsured, @Premium, @Status)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@PolicyNumber", item.PolicyNumber),
            new SqliteParameter("@ClientsID", item.ClientsID),
            new SqliteParameter("@InsuranceID", item.InsuranceID),
            new SqliteParameter("@AgentsID", item.AgentsID),
            new SqliteParameter("@StartDate", item.StartDate),
            new SqliteParameter("@EndDate", item.EndDate),
            new SqliteParameter("@SumInsured", item.SumInsured),
            new SqliteParameter("@Premium", item.Premium),
            new SqliteParameter("@Status", item.Status)
        });
    }
}
