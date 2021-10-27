using chatApi.Data;
using chatApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }
        // test 
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {

            try
            {

                var thing = _context.Users.Find(-1);

                var thingToReturn = thing.ToString();


                return thingToReturn;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Cumpeter says no!");
            }
           
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("this was not a good request");
        }

        // test the user
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() 
        {
            return "secret text";
        }
    }
}
