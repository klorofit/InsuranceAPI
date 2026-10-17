using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;

namespace MyAPP.Driver.SQLiteDAO;

class DBInsurance : IDBInsurance
{
    private Insurance FromReader(SqliteDataReader reader)
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
        DAO.Instance.ExecuteNonQuery("DELETE FROM Insurance WHERE InsuranceID = @id",
            new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public Insurance? Get(int id)
    {
        return DAO.Instance.ReadSingle(
            "SELECT InsuranceID, Name, Description, Terms, Tariff, InsuranceTypesID FROM Insurance WHERE InsuranceID = @id",
            FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<Insurance> Get()
    {
        return DAO.Instance.ExecuteReader(
            "SELECT InsuranceID, Name, Description, Terms, Tariff, InsuranceTypesID FROM Insurance", FromReader);
    }
    public void Put(int id, Insurance item)
    {
        string query = @"UPDATE Insurance SET Name=@Name, Description=@Description, Terms=@Terms, Tariff=@Tariff, InsuranceTypesID=@InsuranceTypesID WHERE InsuranceID=@InsuranceID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@InsuranceID", id),
            new SqliteParameter("@Name", item.Name),
            new SqliteParameter("@Description", item.Description),
            new SqliteParameter("@Terms", item.Terms),
            new SqliteParameter("@Tariff", item.Tariff),
            new SqliteParameter("@InsuranceTypesID", item.InsuranceTypesID)
        });
    }
    public void Post(Insurance item)
    {
        string query = @"INSERT INTO Insurance (Name, Description, Terms, Tariff, InsuranceTypesID)
                         VALUES (@Name, @Description, @Terms, @Tariff, @InsuranceTypesID)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@Name", item.Name),
            new SqliteParameter("@Description", item.Description),
            new SqliteParameter("@Terms", item.Terms),
            new SqliteParameter("@Tariff", item.Tariff),
            new SqliteParameter("@InsuranceTypesID", item.InsuranceTypesID)
        });
    }
}
