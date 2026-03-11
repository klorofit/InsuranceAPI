using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class PoliciesContrl : Controller
{
    [HttpGet]
    public List<Policies> Get()
    {
        List<Policies> result = DAO.Instance.Policies.Get();
        return result;
    }
    [HttpGet("{id}")]
    public Policies? Get(int id)
    {
        Policies? result = DAO.Instance.Policies.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.Policies.Delete(id);
    }
    [HttpPut]
    public void Put(int id, Policies item)
    {
        DAO.Instance.Policies.Put(id, item);
    }
    [HttpPost]
    public void Post(Policies item)
    {
        DAO.Instance.Policies.Post(item);
    } 
}