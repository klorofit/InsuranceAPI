using Microsoft.AspNetCore.Mvc;
using MyAPP.Common;
using MyAPP.Singletons;

[ApiController]
[Route("[controller]")]
public class AgreementsVehiclesContrl : Controller
{
    [HttpGet]
    public List<AgreementsVehicles> Get()
    {
        List<AgreementsVehicles> result = DAO.Instance.AgreementsVehicles.Get();
        return result;
    }
    [HttpGet("{id}")]
    public AgreementsVehicles? Get(int id)
    {
        AgreementsVehicles? result = DAO.Instance.AgreementsVehicles.Get(id);
        return result;
    }
    [HttpDelete]
    public void Delete(int id)
    {
        DAO.Instance.AgreementsVehicles.Delete(id);
    }
    [HttpPut]
    public void Put(int id, AgreementsVehicles item)
    {
        DAO.Instance.AgreementsVehicles.Put(id, item);
    }
    [HttpPost]
    public void Post(AgreementsVehicles item)
    {
        DAO.Instance.AgreementsVehicles.Post(item);
    } 
}