using MyAPP.Common;
using MyAPP.Driver.MariaDB;
using MyAPP.Driver;


namespace MyAPP.Driver.MariaDB;

    public class MariaDBDatabase : IDatabase
    {
        public MariaDBDatabase(DBConfig config)
        {
                DAO.Initialize(config);
        }
        public IDBAgents Agents => new MariaDBAgents();
        public IDBAgrementsVehicles AgrementsVehicles => new MariaDBAgrementsVehicles();
        public IDBClients Clients => new MariaDBClients();
        public IDBInsurance Insurance => new MariaDBInsurance();
        public IDBInsuranceTypes InsuranceTypes => new MariaDBInsuranceTypes();
        public IDBPayments Payments => new MariaDBPayments();
        public IDBPolicies Policies => new MariaDBPolicies();
        public IDBVehicles Vehicles => new MariaDBVehicles();
        public IDBUsers Users => new MariaDBUsers();
        public IRefreshToken RefreshTokens => new MariaDBRefreshTokens();
        
    }
