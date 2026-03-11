using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class ClientsContrl : Controller
{
    [HttpGet]
    public List<Clients> Get()
    {
        List<Clients> result = DAO.Instance.Clients.Get();
        return result;
    }
    [HttpGet("{id}")]
    public Clients? Get(int id)
    {
        Clients? result = DAO.Instance.Clients.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.Clients.Delete(id);
    }
    [HttpPut]
    public void Put(int id, Clients item)
    {
        DAO.Instance.Clients.Put(id, item);
    }
    [HttpPost]
    public void Post(Clients item)
    {
        DAO.Instance.Clients.Post(item);
    } 
}