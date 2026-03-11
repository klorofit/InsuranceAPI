using System.Text.RegularExpressions;
namespace MyAPP.Common;

public class Insurance
{
    private const int NameLen = 100;
    private const int DescLen = 255;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _terms = string.Empty;
    
    public int InsuranceID { get; set; }
    
    public required string Name
    {
        get
        {
            return _name;
        }
        set
        {
            if (value.Length > NameLen)
            {
                throw new ArgumentException($"Name must be shorter than {NameLen}!");
            }
            else
            {
                _name = value;
            }
        }
    }
    
    public required string Description
    {
        get
        {
            return _description;
        }
        set
        {
            if (value.Length > DescLen)
            {
                throw new ArgumentException($"Description must be shorter than {DescLen}!");
            }
            else
            {
                _description = value;
            }
        }
    }
    
    public required string Terms
    {
        get
        {
            return _terms;
        }
        set
        {
            if (value.Length > DescLen)
            {
                throw new ArgumentException($"Terms must be shorter than {DescLen}!");
            }
            else
            {
                _terms = value;
            }
        }
    }
    
    public float Tariff { get; set; }
    public int InsuranceTypesID { get; set; }
}