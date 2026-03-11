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
        string query = "DELETE FROM Policies WHERE PoliciesID = @id";
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public Policies? Get(int id)
    {
        string query = "SELECT PoliciesID, PolicyNumber, ClientsID, InsuranceID, AgentsID, Startdate, EndDate, SumInsured, Premium, Status FROM Policies WHERE PoliciesID = @id";
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<Policies> Get()
    {
        string query = "SELECT PoliciesID, PolicyNumber, ClientsID, InsuranceID, AgentsID, Startdate, EndDate, SumInsured, Premium, Status FROM Policies";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, Policies item)
    {
        string query = @"UPDATE Policies
                        SET PolicyNumber = @PolicyNumber, 
                            ClientsID = @ClientsID, 
                            InsuranceID = @InsuranceID, 
                            AgentsID = @AgentsID,
                            Startdate = @Startdate,
                            EndDate = @EndDate,
                            SumInsured = @SumInsured,
                            Premium = @Premium,
                            Status = @Status
                        WHERE PoliciesID = @PoliciesID";
        
        var parameters = new List<SqliteParameter> 
        {
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@PolicyNumber", item.PolicyNumber),
            new SqliteParameter("@ClientsID", item.ClientsID),
            new SqliteParameter("@InsuranceID", item.InsuranceID),
            new SqliteParameter("@AgentsID", item.AgentsID),
            new SqliteParameter("@StartDate", item.StartDate),
            new SqliteParameter("@EndDate", item.EndDate),
            new SqliteParameter("@SumInsured", item.SumInsured),
            new SqliteParameter("@Premium", item.Premium),
            new SqliteParameter("@Status", item.Status)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(Policies item)
    {
        string query = "INSERT INTO Policies (PoliciesID, PolicyNumber, ClientsID, InsuranceID, AgentsID, Startdate, EndDate, SumInsured, Premium, Status) VALUES (@PoliciesID, @PolicyNumber, @InsuranceID, @AgentsID, @hire_date, @SumInsured, @Premium, @Status)";
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@PolicyNumber", item.PolicyNumber),
            new SqliteParameter("@ClientsID", item.ClientsID),
            new SqliteParameter("@InsuranceID", item.InsuranceID),
            new SqliteParameter("@AgentsID", item.AgentsID),
            new SqliteParameter("@StartDate", item.StartDate),
            new SqliteParameter("@EndDate", item.EndDate),
            new SqliteParameter("@SumInsured", item.SumInsured),
            new SqliteParameter("@Premium", item.Premium),
            new SqliteParameter("@Status", item.Status)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
