using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.MariaDB;
using MySqlConnector;

namespace MyAPP.Driver.MariaDB;

class MariaDBInsuranceTypes : IDBInsuranceTypes
{
    private InsuranceTypes FromReader(MySqlDataReader reader)
    {
        return new InsuranceTypes
        {
            InsuranceTypesID = (int)reader.GetInt64("InsuranceTypesID"),
            Name = reader.GetString("Name")
        };
    }
    public void Delete(int id)
    {
        string query = "DELETE FROM InsuranceTypes WHERE InsuranceTypesID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public InsuranceTypes? Get(int id)
    {
        string query = "SELECT InsuranceTypesID, Name FROM InsuranceTypes WHERE InsuranceTypesID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<InsuranceTypes> Get()
    {
        string query = "SELECT InsuranceTypesID, Name FROM InsuranceTypes";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, InsuranceTypes item)
    {
        string query = @"UPDATE InsuranceTypes
                        SET Name = @Name
                        WHERE InsuranceTypesID = @InsuranceTypesID";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@InsuranceTypesID", item.InsuranceTypesID),
            new MySqlParameter("@Name", item.Name)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(InsuranceTypes item)
    {
        string query = "INSERT INTO InsuranceTypes (InsuranceTypesID, Name) VALUES (@InsuranceTypesID, @Name)";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@InsuranceTypesID", item.InsuranceTypesID),
            new MySqlParameter("@Name", item.Name)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
