using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.MariaDB;
using MySqlConnector;

namespace MyAPP.Driver.MariaDB;

class MariaDBAgrementsVehicles : IDBAgrementsVehicles
{
    private AgrementsVehicles FromReader(MySqlDataReader reader)
    {
        return new AgrementsVehicles
        {
            AgrementsVehiclesID = (int)reader.GetInt64("AgrementsVehiclesID"),
            PoliciesID = (int)reader.GetInt64("PoliciesID"),
            VehiclesID = (int)reader.GetInt64("VehiclesID")
        };
    }
    public void Delete(int id)
    {
        string query = "DELETE FROM AgrementsVehicles WHERE AgrementsVehiclesID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public AgrementsVehicles? Get(int id)
    {
        string query = "SELECT AgrementsVehiclesID, PoliciesID, VehiclesID FROM AgrementsVehicles WHERE AgrementsVehiclesID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<AgrementsVehicles> Get()
    {
        string query = "SELECT AgrementsVehiclesID, PoliciesID, VehiclesID FROM AgrementsVehicles";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, AgrementsVehicles item)
    {
        string query = @"UPDATE AgrementsVehicles
                        SET PoliciesID = @PoliciesID, 
                            VehiclesID = @VehiclesID
                        WHERE AgrementsVehiclesID = @AgrementsVehicles";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@AgrementsVehiclesID", item.AgrementsVehiclesID),
            new MySqlParameter("@PoliciesID", item.PoliciesID),
            new MySqlParameter("@VehiclesID", item.VehiclesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(AgrementsVehicles item)
    {
        string query = "INSERT INTO AgrementsVehicles (AgrementsVehiclesID, PoliciesID, VehiclesID) VALUES (@AgrementsVehiclesID, @PoliciesID, @VehiclesID)";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@AgrementsVehiclesID", item.AgrementsVehiclesID),
            new MySqlParameter("@PoliciesID", item.PoliciesID),
            new MySqlParameter("@VehiclesID", item.VehiclesID)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
