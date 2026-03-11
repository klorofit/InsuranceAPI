using System.Data;
using Microsoft.Data.Sqlite;
using MyAPP.Common;
using MyAPP.Driver;
using MyAPP.Driver.SQLiteDAO;

namespace MyAPP.Driver.SQLiteDAO;

class DBVehicles : IDBVehicles
{
    private Vehicles FromReader(SqliteDataReader reader)
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
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public Vehicles? Get(int id)
    {
        string query = "SELECT VehiclesID, Vin, Make, Model, Year FROM Vehicles WHERE VehiclesID = @id";
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@id", id)
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
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@VehiclesID", item.VehiclesID),
            new SqliteParameter("@Vin", item.Vin),
            new SqliteParameter("@Make", item.Make),
            new SqliteParameter("@Model", item.Model),
            new SqliteParameter("@Year", item.Year)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
    public void Post(Vehicles item)
    {
        string query = "INSERT INTO Vehicles (VehiclesID, Vin, Make, Model, Year) VALUES (@VehiclesID, @Vin, @Make, @Model, @Year)";
        
        var parameters = new List<SqliteParameter>
        {
            new SqliteParameter("@VehiclesID", item.VehiclesID),
            new SqliteParameter("@Vin", item.Vin),
            new SqliteParameter("@Make", item.Make),
            new SqliteParameter("@Model", item.Model),
            new SqliteParameter("@Year", item.Year)
        };
        
        DAO.Instance.ExecuteNonQuery(query, parameters);
    }
}
