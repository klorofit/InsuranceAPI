using System.Text.RegularExpressions;
namespace MyAPP.Common;

public class Payments
{
    private string _method = string.Empty;
    private string _reference = string.Empty;
    
    public int PaymentsID { get; set; }
    public int PolicyID { get; set; }
    public DateTime PaymentDate { get; set; }
    public float Amount { get; set; }
    
    public required string Method
    {
        get
        {
            return _method;
        }
        set
        {
            if (value.Length > 50) 
            {
                throw new ArgumentException($"Method must be shorter than 50 characters!");
            }
            else
            {
                _method = value;
            }
        }
    }
    
    public required string Reference
    {
        get
        {
            return _reference;
        }
        set
        {
            if (value.Length > 100) 
            {
                throw new ArgumentException($"Reference must be shorter than 100 characters!");
            }
            else
            {
                _reference = value;
            }
        }
    }
}