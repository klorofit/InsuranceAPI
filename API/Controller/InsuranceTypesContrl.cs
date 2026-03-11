using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class InsuranceTypesContrl : Controller
{
    [HttpGet]
    public List<InsuranceTypes> Get()
    {
        List<InsuranceTypes> result = DAO.Instance.InsuranceTypes.Get();
        return result;
    }
    [HttpGet("{id}")]
    public InsuranceTypes? Get(int id)
    {
        InsuranceTypes? result = DAO.Instance.InsuranceTypes.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.InsuranceTypes.Delete(id);
    }
    [HttpPut]
    public void Put(int id, InsuranceTypes item)
    {
        DAO.Instance.InsuranceTypes.Put(id, item);
    }
    [HttpPost]
    public void Post(InsuranceTypes item)
    {
        DAO.Instance.InsuranceTypes.Post(item);
    } 
}