using Microsoft.AspNetCore.Mvc;

namespace HealthMediator.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
		// POST api/health
		[HttpGet]
		public IActionResult Get()
		{
			return new JsonResult(new { Name = "TestName", Description = "TestDescription" });
		}

		// POST api/health
		[HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/health/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/health/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
