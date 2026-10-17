using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;

namespace MyAPP.Driver.SQLiteDAO;

class DBInsuranceTypes : IDBInsuranceTypes
{
    private InsuranceTypes FromReader(SqliteDataReader reader)
    {
        return new InsuranceTypes
        {
            InsuranceTypesID = (int)reader.GetInt64("InsuranceTypesID"),
            Name = reader.GetString("Name")
        };
    }
    public void Delete(int id)
    {
        DAO.Instance.ExecuteNonQuery("DELETE FROM InsuranceTypes WHERE InsuranceTypesID = @id",
            new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public InsuranceTypes? Get(int id)
    {
        return DAO.Instance.ReadSingle(
            "SELECT InsuranceTypesID, Name FROM InsuranceTypes WHERE InsuranceTypesID = @id",
            FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<InsuranceTypes> Get()
    {
        return DAO.Instance.ExecuteReader("SELECT InsuranceTypesID, Name FROM InsuranceTypes", FromReader);
    }
    public void Put(int id, InsuranceTypes item)
    {
        string query = @"UPDATE InsuranceTypes SET Name=@Name WHERE InsuranceTypesID=@InsuranceTypesID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@InsuranceTypesID", id),
            new SqliteParameter("@Name", item.Name)
        });
    }
    public void Post(InsuranceTypes item)
    {
        // AUTOINCREMENT — не вставляем ID (иначе получается 0)
        string query = @"INSERT INTO InsuranceTypes (Name) VALUES (@Name)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@Name", item.Name)
        });
    }
}
