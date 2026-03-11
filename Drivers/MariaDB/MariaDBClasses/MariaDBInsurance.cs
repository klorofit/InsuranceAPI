using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.MariaDB;
using MySqlConnector;

namespace MyAPP.Driver.MariaDB;

class MariaDBInsurance : IDBInsurance
{
    private Insurance FromReader(MySqlDataReader reader)
    {
        return new Insurance
        {
            InsuranceID = (int)reader.GetInt64("InsuranceID"),
            Name = reader.GetString("Name"),
            Description = reader.GetString("Description"),
            Terms = reader.GetString("Terms"),
            Tariff = (float)reader.GetDouble("Tariff"),
            InsuranceTypesID = (int)reader.GetInt64("InsuranceTypesID")
        };
    }
    public void Delete(int id)
    {
        string query = "DELETE FROM Insurance WHERE InsuranceID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public Insurance? Get(int id)
    {
        string query = "SELECT InsuranceID, Name, Description, Terms, Tariff, InsuranceTypesID FROM Insurance WHERE InsuranceID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<Insurance> Get()
    {
        string query = "SELECT InsuranceID, Name, Description, Terms, Tariff, InsuranceTypesID FROM Insurance";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, Insurance item)
    {
        string query = @"UPDATE Insurance
                        SET Name = @Name, 
                            Description = @Description, 
                            Terms = @Terms, 
                            Tariff = @Tariff,
                            InsuranceTypesID = @InsuranceTypesID
                        WHERE InsuranceID = @InsuranceID";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@InsuranceID", item.InsuranceID),
            new MySqlParameter("@Name", item.Name),
            new MySqlParameter("@Description", item.Description),
            new MySqlParameter("@Terms", item.Terms),
            new MySqlParameter("@Tariff", item.Tariff),
            new MySqlParameter("@InsuranceTypesID", item.InsuranceTypesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(Insurance item)
    {
        string query = "INSERT INTO Insurance (InsuranceID, Name, Description, Terms, Tariff InsuranceTypesID) VALUES (@InsuranceID, @Name, @Terms, @Tariff @InsuranceTypesID)";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@InsuranceID", item.InsuranceID),
            new MySqlParameter("@Name", item.Name),
            new MySqlParameter("@Description", item.Description),
            new MySqlParameter("@Terms", item.Terms),
            new MySqlParameter("@Tariff", item.Tariff),
            new MySqlParameter("@InsuranceTypesID", item.InsuranceTypesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
