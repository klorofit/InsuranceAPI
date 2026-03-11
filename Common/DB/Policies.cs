using System.Text.RegularExpressions;
namespace MyAPP.Common;

public class Policies
{
    private string _policyNumber = string.Empty;
    private string _status = string.Empty;
    
    public int PoliciesID { get; set; }
    
    public required string PolicyNumber
    {
        get
        {
            return _policyNumber;
        }
        set
        {
            if (value.Length > 50) 
            {
                throw new ArgumentException($"PolicyNumber must be shorter than 50 characters!");
            }
            else
            {
                _policyNumber = value;
            }
        }
    }
    
    public int ClientsID { get; set; }
    public int InsuranceID { get; set; }
    public int AgentsID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public float SumInsured { get; set; }
    public float Premium { get; set; }
    
    public required string Status
    {
        get
        {
            return _status;
        }
        set
        {
            if (value.Length > 50) 
            {
                throw new ArgumentException($"Status must be shorter than 50 characters!");
            }
            else
            {
                _status = value;
            }
        }
    }
}