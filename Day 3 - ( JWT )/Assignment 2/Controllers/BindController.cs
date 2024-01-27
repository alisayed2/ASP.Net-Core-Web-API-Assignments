using Assignment_2.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BindController : ControllerBase
    {
        #region Default Binding 
        // Primitive Type Defualt Binding
        [HttpGet] // from Query string
        public IActionResult Get(int id , string name)
        {
            return Ok("Primitive From Query String");
        }

        [HttpGet("{id}")] // from Route Data
        public IActionResult Get(int id)
        {
            return Ok("Primitive From Route Data");
        }

        // Complex Type Defualt Binding 
        [HttpPost]                                   
        public IActionResult Post(Employee employee , string name) // Name => from query
        {
            return Ok("Complex From Body");
        }
        #endregion

        #region Custom Binding

        // Primitive Type Custom Binding
        [HttpPost("{id}")]
        public IActionResult Post([FromBody]string name,int id)
        {
            return Ok("Primitive From Body");
        }

        // Complex Type Custom Binding 
        [HttpPost("{name}/{salary}/{address}/{age}")]
        public IActionResult Post([FromRoute]Employee employee, int id)
        {
            return Ok("Copmlex From Route");
        }

        #endregion
    }
}
