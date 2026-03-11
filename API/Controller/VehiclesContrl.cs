using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class VehiclesContrl : Controller
{
    [HttpGet]
    public List<Vehicles> Get()
    {
        List<Vehicles> result = DAO.Instance.Vehicles.Get();
        return result;
    }
    [HttpGet("{id}")]
    public Vehicles? Get(int id)
    {
        Vehicles? result = DAO.Instance.Vehicles.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.Vehicles.Delete(id);
    }
    [HttpPut]
    public void Put(int id, Vehicles item)
    {
        DAO.Instance.Vehicles.Put(id, item);
    }
    [HttpPost]
    public void Post(Vehicles item)
    {
        DAO.Instance.Vehicles.Post(item);
    } 
}