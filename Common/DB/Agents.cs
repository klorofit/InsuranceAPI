using System.Text.RegularExpressions;
namespace MyAPP.Common;

public class Agents
{
    private const int NameLen = 100;
    private const string PhonePattern = @"^\d{11}$";
    private string _phone = string.Empty;
    private const string NamePattern = @"^[А-ЯЁ][а-яё]+$";
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _email = string.Empty;
    
    public int AgentsID { get; set; }
    
    public required string FirstName
    {
        get
        {
            return _firstName;
        }
        set
        {
            if (value.Length > NameLen)
            {
                throw new ArgumentException($"FirstName must be shorter than {NameLen}!");
            }
            else
            {
                if (Regex.IsMatch(value, NamePattern))
                {
                    _firstName = value;
                }
                else
                {
                    throw new ArgumentException($"FirstName format error!");
                }
            }
        }
    }
    
    public required string LastName
    {
        get
        {
            return _lastName;
        }
        set
        {
            if (value.Length > NameLen)
            {
                throw new ArgumentException($"LastName must be shorter than {NameLen}!");
            }
            else
            {
                if (Regex.IsMatch(value, NamePattern))
                {
                    _lastName = value;
                }
                else
                {
                    throw new ArgumentException($"LastName format error!");
                }
            }
        }
    }
    
    public string Phone
    {
        get
        {
            return _phone;
        }
        set
        {
            if (Regex.IsMatch(value, PhonePattern))
            {
                _phone = value;
            }
            else
            {
                throw new ArgumentException($"Phone format error!");
            }
        }
    }
    
    public required string Email
    {
        get
        {
            return _email;
        }
        set
        {
            if (value.Length > NameLen)
            {
                throw new ArgumentException($"Email must be shorter than {NameLen}!");
            }
            else
            {
                if (IsValidEmail(value))
                {
                    _email = value;
                }
                else
                {
                    throw new ArgumentException($"Email format error!");
                }
            }
        }
    }
    
    public DateTime HireDate { get; set; }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}