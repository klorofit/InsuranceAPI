using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;

namespace MyAPP.Driver.SQLiteDAO;

class DBAgreementsVehicles : IDBAgreementsVehicles
{
    private AgreementsVehicles FromReader(SqliteDataReader reader)
    {
        return new AgreementsVehicles
        {
            AgreementsVehiclesID = (int)reader.GetInt64("AgreementsVehiclesID"),
            PoliciesID = (int)reader.GetInt64("PoliciesID"),
            VehiclesID = (int)reader.GetInt64("VehiclesID")
        };
    }
    public void Delete(int id)
    {
        DAO.Instance.ExecuteNonQuery("DELETE FROM AgreementsVehicles WHERE AgreementsVehiclesID = @id",
            new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public AgreementsVehicles? Get(int id)
    {
        return DAO.Instance.ReadSingle(
            "SELECT AgreementsVehiclesID, PoliciesID, VehiclesID FROM AgreementsVehicles WHERE AgreementsVehiclesID = @id",
            FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<AgreementsVehicles> Get()
    {
        return DAO.Instance.ExecuteReader(
            "SELECT AgreementsVehiclesID, PoliciesID, VehiclesID FROM AgreementsVehicles", FromReader);
    }
    public void Put(int id, AgreementsVehicles item)
    {
        string query = @"UPDATE AgreementsVehicles SET PoliciesID=@PoliciesID, VehiclesID=@VehiclesID WHERE AgreementsVehiclesID=@AgreementsVehiclesID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@AgreementsVehiclesID", id),
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@VehiclesID", item.VehiclesID)
        });
    }
    public void Post(AgreementsVehicles item)
    {
        string query = @"INSERT INTO AgreementsVehicles (PoliciesID, VehiclesID) VALUES (@PoliciesID, @VehiclesID)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@VehiclesID", item.VehiclesID)
        });
    }
}
