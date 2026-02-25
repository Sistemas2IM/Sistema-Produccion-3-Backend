using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.GoogleChat
{
    [Route("api/[controller]")]
    [ApiController]
    public class soporteGoogleChatController : ControllerBase
    {
        // GET: api/<soporteGoogleChatController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<soporteGoogleChatController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<soporteGoogleChatController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<soporteGoogleChatController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<soporteGoogleChatController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
