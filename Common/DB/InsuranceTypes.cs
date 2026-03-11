using System.Text.RegularExpressions;
namespace MyAPP.Common;

public class InsuranceTypes
{
    private const int TextLen = 100;
    private const string TextPattern = @"^[А-ЯЁ][а-яё-]+ [А-ЯЁ][а-яё-]+(?: [А-ЯЁ][а-яё]+)?$";
    private string _name = string.Empty;
    
    public int InsuranceTypesID { get; set; }
    
    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            if (value.Length > TextLen)
            {
                throw new ArgumentException($"Name must be shorter than {TextLen}!");
            }
            else
            {
                if (Regex.IsMatch(value, TextPattern))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException($"Name format error!");
                }
            }
        }
    }
}