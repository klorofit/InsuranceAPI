using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class AgentsContrl : Controller
{
    [HttpGet]
    public List<Agents> Get()
    {
        List<Agents> result = DAO.Instance.Agents.Get();
        return result;
    }
    [HttpGet("{id}")]
    public Agents? Get(int id)
    {
        Agents? result = DAO.Instance.Agents.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.Agents.Delete(id);
    }
    [HttpPut]
    public void Put(int id, Agents item)
    {
        DAO.Instance.Agents.Put(id, item);
    }
    [HttpPost]
    public void Post(Agents item)
    {
        DAO.Instance.Agents.Post(item);
    } 
}