using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using nCore2Test.DataAccess.Entities;
using Digipolis.DataAccess;

namespace nCore2Test.Controllers
{
    [Route("[controller]")]
    public class CarsController : Controller
    {
        public CarsController(IUowProvider uowProvider)
        {
            _uowProvider = uowProvider;
        }

        private readonly IUowProvider _uowProvider;
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<Car>();
                return Ok(await repo.GetAllAsync()); 
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<Car>();
                return Ok(await repo.QueryAsync(x => x.Id == id));
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Car car)
        {
            using(var uow = _uowProvider.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<Car>();
                repo.Add(car);
                await uow.SaveChangesAsync();
                return Ok(car.Id);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (var uow = _uowProvider.CreateUnitOfWork())
            {
                var repo = uow.GetRepository<Car>();
                repo.Remove(id);
                return NoContent();
            }
        }
    }
}
