using System.Text.RegularExpressions;
namespace MyAPP.Common;

public class Vehicles
{
    private string _vin = string.Empty;
    private string _make = string.Empty;
    private string _model = string.Empty;
    
    public int VehiclesID { get; set; }
    
    public required string Vin
    {
        get
        {
            return _vin;
        }
        set
        {
            if (value.Length > 50) // Из таблицы базы данных: vin varchar(50)
            {
                throw new ArgumentException($"Vin must be shorter than 50 characters!");
            }
            else
            {
                // VIN может содержать буквы и цифры, не только русские буквы
                _vin = value;
            }
        }
    }
    
    public required string Make
    {
        get
        {
            return _make;
        }
        set
        {
            if (value.Length > 100) // Из таблицы базы данных: make varchar(100)
            {
                throw new ArgumentException($"Make must be shorter than 100 characters!");
            }
            else
            {
                // Марка машины может содержать разные символы
                _make = value;
            }
        }
    }
    
    public required string Model
    {
        get
        {
            return _model;
        }
        set
        {
            if (value.Length > 100) // Из таблицы базы данных: model varchar(100)
            {
                throw new ArgumentException($"Model must be shorter than 100 characters!");
            }
            else
            {
                _model = value;
            }
        }
    }
    
    public int Year { get; set; }
}