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
        DAO.Instance.ExecuteNonQuery("DELETE FROM Vehicles WHERE VehiclesID = @id",
            new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public Vehicles? Get(int id)
    {
        return DAO.Instance.ReadSingle(
            "SELECT VehiclesID, Vin, Make, Model, Year FROM Vehicles WHERE VehiclesID = @id",
            FromReader, new List<SqliteParameter> { new SqliteParameter("@id", id) });
    }
    public List<Vehicles> Get()
    {
        return DAO.Instance.ExecuteReader("SELECT VehiclesID, Vin, Make, Model, Year FROM Vehicles", FromReader);
    }
    public void Put(int id, Vehicles item)
    {
        string query = @"UPDATE Vehicles SET Vin=@Vin, Make=@Make, Model=@Model, Year=@Year WHERE VehiclesID=@VehiclesID";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@VehiclesID", id),
            new SqliteParameter("@Vin", item.Vin),
            new SqliteParameter("@Make", item.Make),
            new SqliteParameter("@Model", item.Model),
            new SqliteParameter("@Year", item.Year)
        });
    }
    public void Post(Vehicles item)
    {
        string query = @"INSERT INTO Vehicles (Vin, Make, Model, Year) VALUES (@Vin, @Make, @Model, @Year)";
        DAO.Instance.ExecuteNonQuery(query, new List<SqliteParameter>
        {
            new SqliteParameter("@Vin", item.Vin),
            new SqliteParameter("@Make", item.Make),
            new SqliteParameter("@Model", item.Model),
            new SqliteParameter("@Year", item.Year)
        });
    }
}
