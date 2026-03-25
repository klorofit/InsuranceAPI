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
        string query = "DELETE FROM AgreementsVehicles WHERE AgreementsVehiclesID = @id";
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public AgreementsVehicles? Get(int id)
    {
        string query = "SELECT AgreementsVehiclesID, PoliciesID, VehiclesID FROM AgreementsVehicles WHERE AgreementsVehiclesID = @id";
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<AgreementsVehicles> Get()
    {
        string query = "SELECT AgreementsVehiclesID, PoliciesID, VehiclesID FROM AgreementsVehicles";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, AgreementsVehicles item)
    {
        string query = @"UPDATE AgreementsVehicles
                        SET PoliciesID = @PoliciesID, 
                            VehiclesID = @VehiclesID
                        WHERE AgreementsVehiclesID = @AgreementsVehicles";
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@AgreementsVehiclesID", item.AgreementsVehiclesID),
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@VehiclesID", item.VehiclesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(AgreementsVehicles item)
    {
        string query = "INSERT INTO AgreementsVehicles (AgreementsVehiclesID, PoliciesID, VehiclesID) VALUES (@AgreementsVehiclesID, @PoliciesID, @VehiclesID)";
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@AgreementsVehiclesID", item.AgreementsVehiclesID),
            new SqliteParameter("@PoliciesID", item.PoliciesID),
            new SqliteParameter("@VehiclesID", item.VehiclesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
