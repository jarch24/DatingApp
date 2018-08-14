using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatinApp.API.Data;
using DatinApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly DataContext _db;

        public ValuesController(DataContext db)
        {
            _db = db;

        }
        // GET api/values
        [HttpGet("get")]        
        public IActionResult Get()
        {
            return Ok(_db.Value.ToList());
        }

        [HttpGet("getval")]        
        public async Task<IActionResult> GetValues(string query)
        {
            if(String.IsNullOrEmpty(query)){
                return Ok(String.Empty);
            }
            return Ok(await _db.Value.Where(t => t.Name.ToLower().StartsWith(query)).ToListAsync()); 
        }

        // GET api/values/5
        [HttpGet("{id}"), ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Get(int id)
        {
            
            return Ok(await _db.Value.FirstOrDefaultAsync(t => t.Id == id));
        }

        // POST api/values
        [HttpPost]
        
        public void Post(Value val)
        {
            if(val != null)
            {

                _db.Value.Add(val);
                _db.SaveChanges();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
