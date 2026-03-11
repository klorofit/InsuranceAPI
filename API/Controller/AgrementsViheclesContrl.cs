using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class AgrementsVehiclesContrl : Controller
{
    [HttpGet]
    public List<AgrementsVehicles> Get()
    {
        List<AgrementsVehicles> result = DAO.Instance.AgrementsVehicles.Get();
        return result;
    }
    [HttpGet("{id}")]
    public AgrementsVehicles? Get(int id)
    {
        AgrementsVehicles? result = DAO.Instance.AgrementsVehicles.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.AgrementsVehicles.Delete(id);
    }
    [HttpPut]
    public void Put(int id, AgrementsVehicles item)
    {
        DAO.Instance.AgrementsVehicles.Put(id, item);
    }
    [HttpPost]
    public void Post(AgrementsVehicles item)
    {
        DAO.Instance.AgrementsVehicles.Post(item);
    } 
}