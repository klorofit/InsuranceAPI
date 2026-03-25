using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.MariaDB;
using MySqlConnector;

namespace MyAPP.Driver.MariaDB;

class MariaDBAgreementsVehicles : IDBAgreementsVehicles
{
    private AgreementsVehicles FromReader(MySqlDataReader reader)
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
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public AgreementsVehicles? Get(int id)
    {
        string query = "SELECT AgreementsVehiclesID, PoliciesID, VehiclesID FROM AgreementsVehicles WHERE AgreementsVehiclesID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
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
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@AgreementsVehiclesID", item.AgreementsVehiclesID),
            new MySqlParameter("@PoliciesID", item.PoliciesID),
            new MySqlParameter("@VehiclesID", item.VehiclesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(AgreementsVehicles item)
    {
        string query = "INSERT INTO AgreementsVehicles (AgreementsVehiclesID, PoliciesID, VehiclesID) VALUES (@AgreementsVehiclesID, @PoliciesID, @VehiclesID)";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@AgreementsVehiclesID", item.AgreementsVehiclesID),
            new MySqlParameter("@PoliciesID", item.PoliciesID),
            new MySqlParameter("@VehiclesID", item.VehiclesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
