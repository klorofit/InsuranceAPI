
namespace MyAPP.Common
{
    public interface IDatabase
    {
        IDBAgents Agents { get; }
        IDBAgreementsVehicles AgreementsVehicles { get; }
        IDBClients Clients { get; }
        IDBInsurance Insurance { get; }
        IDBInsuranceTypes InsuranceTypes { get; }
        IDBPayments Payments { get; }
        IDBPolicies Policies { get; }
        IDBVehicles Vehicles { get; }
        IDBUsers Users { get; }
    }
}