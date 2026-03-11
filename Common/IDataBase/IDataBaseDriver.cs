using MyAPP.Common;
namespace MyAPP.Common;

public interface IDatabaseDriver
{
    void Connect(string connectionString);
    void ExecuteQuery(string sql);
}