using System.Text.RegularExpressions;
namespace MyAPP.Common;

public class InsuranceTypes
{
    private const int TextLen = 100;
    // Было: строго 2–3 слова с заглавной — ломало чтение старых записей (ID=0, одно слово и т.п.)
    // Теперь: непустая строка, кириллица/латиница/цифры/пробел/дефис
    private static readonly Regex TextPattern = new(
        @"^[\p{L}\p{N}][\p{L}\p{N}\s\-]{0,98}$",
        RegexOptions.Compiled);

    private string _name = string.Empty;

    public int InsuranceTypesID { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            if (value == null)
                throw new ArgumentException("Name is required!");

            var trimmed = value.Trim();
            if (trimmed.Length == 0)
                throw new ArgumentException("Name is required!");
            if (trimmed.Length > TextLen)
                throw new ArgumentException($"Name must be shorter than {TextLen}!");
            if (!TextPattern.IsMatch(trimmed))
                throw new ArgumentException("Name format error!");

            _name = trimmed;
        }
    }
}
