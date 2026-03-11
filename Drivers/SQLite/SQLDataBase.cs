using MyAPP.Common;
using MyAPP.Driver.SQLiteDAO;
using MyAPP.Driver;

namespace MyAPP.Driver;

public class SQLiteDatabase : IDatabase
{
    public SQLiteDatabase(DBConfig config)
    {
        DAO.Initialize(config);
        CreateTablesIfNotExists();
    }
    private void CreateTablesIfNotExists()
    {
            foreach (var sql in DDL.Defenition)
            {
                DAO.Instance.ExecuteNonQuery(sql);
            }
    }
    public IDBAgents Agents => new DBAgents();
    public IDBAgrementsVehicles AgrementsVehicles => new DBAgrementsVehicles();
    public IDBClients Clients => new DBClients();
    public IDBInsurance Insurance => new DBInsurance();
    public IDBInsuranceTypes InsuranceTypes => new DBInsuranceTypes();
    public IDBPayments Payments => new DBPayments();
    public IDBPolicies Policies => new DBPolicies();
    public IDBVehicles Vehicles => new DBVehicles();
    public IDBUsers Users => new DBUsers();
    public IRefreshToken RefreshTokens => new DBRefreshTokens();
}
