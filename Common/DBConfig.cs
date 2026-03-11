using MyAPP.Common;
namespace MyAPP.Common;

public class DBConfig
{
    public string Location { get; set; } = string.Empty;
    public string DataBase { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public DBConfig(string location, string database, string username, string password)
    {
        Location = location;
        DataBase = database;
        UserName = username;
        Password = password;
    }
        public DBConfig()
    {
        Location = string.Empty;
        DataBase = string.Empty;
        UserName = string.Empty;
        Password = string.Empty;
    }
}