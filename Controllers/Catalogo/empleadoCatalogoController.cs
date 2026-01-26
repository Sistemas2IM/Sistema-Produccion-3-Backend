using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class empleadoCatalogoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
       private readonly IMapper _mapper;

        public empleadoCatalogoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<empleadoCatalogoController>
        [HttpGet("get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<empleadoCatalogoController>/5
        [HttpGet("get/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<empleadoCatalogoController>
        [HttpPost("post")]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<empleadoCatalogoController>/5
        [HttpPut("put/{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<empleadoCatalogoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
