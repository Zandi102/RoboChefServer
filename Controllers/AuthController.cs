using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace RoboChefServer {

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMongoDatabase _database;

        public AuthController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAccount([FromBody] User user)
        {
            try
            {
                if (user.Username == null || user.Email == null || user.Password == null)
                {
                    return BadRequest("Not All Required Fields Are Present");
                }

                var collection = _database.GetCollection<User>("User");

                var result = await collection.FindAsync(x => x.Username == user.Username);

                if (result.FirstOrDefault() != null)
                {
                    // User not found, return an error response
                    return BadRequest($"User with username {user.Username} already exists!");
                }

                result = await collection.FindAsync(x => x.Email == user.Email);

                if (result.FirstOrDefault() != null)
                {
                    // User not found, return an error response
                    return BadRequest($"User with email {user.Email} already exists!");
                }

                await collection.InsertOneAsync(user);

                return Ok(user);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AccountLogin([FromBody] User user)
        {
            try
            {
                var collection = _database.GetCollection<User>("User");

                var result = await collection.FindAsync(x => x.Username == user.Username && x.Password == user.Password);

                if (result.FirstOrDefault() == null)
                {
                    // User not found, return an error response
                    return BadRequest("Invalid login credentials");
                }

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}