using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class InsuranceContrl : Controller
{
    [HttpGet]
    public List<Insurance> Get()
    {
        List<Insurance> result = DAO.Instance.Insurance.Get();
        return result;
    }
    [HttpGet("{id}")]
    public Insurance? Get(int id)
    {
        Insurance? result = DAO.Instance.Insurance.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.Insurance.Delete(id);
    }
    [HttpPut]
    public void Put(int id, Insurance item)
    {
        DAO.Instance.Insurance.Put(id, item);
    }
    [HttpPost]
    public void Post(Insurance item)
    {
        DAO.Instance.Insurance.Post(item);
    } 
}