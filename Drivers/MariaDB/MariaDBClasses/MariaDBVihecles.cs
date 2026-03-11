using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.MariaDB;
using MySqlConnector;

namespace MyAPP.Driver.MariaDB;

class MariaDBVehicles : IDBVehicles
{
    private Vehicles FromReader(MySqlDataReader reader)
    {
        return new Vehicles
        {
            VehiclesID = (int)reader.GetInt64("VehiclesID"),
            Vin = reader.GetString("Vin"),
            Make = reader.GetString("Make"),
            Model = reader.GetString("Model"),
            Year = (int)reader.GetInt64("Year")
        };
    }
    public void Delete(int id)
    {
        string query = "DELETE FROM Vehicles WHERE VehiclesID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public Vehicles? Get(int id)
    {
        string query = "SELECT VehiclesID, Vin, Make, Model, Year FROM Vehicles WHERE VehiclesID = @id";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@id", id)
        };
        
        return DAO.Instance.ReadSingle(query, FromReader, parameters);
    }
    public List<Vehicles> Get()
    {
        string query = "SELECT VehiclesID, Vin, Make, Model, Year FROM Vehicles";
        return DAO.Instance.ExecuteReader(query, FromReader);
    }
    public void Put(int id, Vehicles item)
    {
        string query = @"UPDATE Vehicles
                        SET Vin = @Vin, 
                            Make = @Make, 
                            Model = @Model,
                            Year = @Year
                        WHERE VehiclesID = @VehiclesID";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@VehiclesID", item.VehiclesID),
            new MySqlParameter("@Vin", item.Vin),
            new MySqlParameter("@Make", item.Make),
            new MySqlParameter("@Model", item.Model),
            new MySqlParameter("@Year", item.Year)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(Vehicles item)
    {
        string query = "INSERT INTO Vehicles (VehiclesID, Vin, Make, Model, Year) VALUES (@VehiclesID, @Vin, @Make, @Model, @Year)";
        
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@VehiclesID", item.VehiclesID),
            new MySqlParameter("@Vin", item.Vin),
            new MySqlParameter("@Make", item.Make),
            new MySqlParameter("@Model", item.Model),
            new MySqlParameter("@Year", item.Year)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
