using System;
namespace RoboChefServer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;

    namespace RoboChefServer
    {

        [ApiController]
        [Route("grocery")]
        public class GroceryController : ControllerBase
        {
            private readonly IMongoDatabase _database;

            public GroceryController(IMongoDatabase database)
            {
                _database = database;
            }

            [HttpPost]
            [Route("addItem")]
            public async Task<IActionResult> AddGroceryItem([FromBody] GroceryItem item)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Quantity))
                    {
                        return BadRequest("Missing Required Parameters name and quantity");
                    }

                    var collection = _database.GetCollection<GroceryItem>("GroceryItem");

                    var result = await collection.FindAsync(x => x.Name == item.Name && x.Username == item.Username);

                    if (result.FirstOrDefault() != null)
                    {
                        return BadRequest($"GroceryItem with name {item.Name} Already Exists");
                    }

                    await collection.InsertOneAsync(item);

                    return Ok(item);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpDelete]
            [Route("deleteItem")]
            public async Task<IActionResult> DeleteGroceryItem([FromBody] GroceryItem item)
            {
                try
                {
                    if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Username))
                    {
                        return BadRequest("Missing Required Parameters name and username");
                    }

                    var collection = _database.GetCollection<GroceryItem>("GroceryItem");

                    var result = await collection.FindAsync(x => x.Name == item.Name && x.Username == item.Username);

                    if (result.FirstOrDefault() == null)
                    {
                        return BadRequest($"GroceryItem {item.Name} for user {item.Username} does not exist for deletion");
                    }

                    var response = await collection.DeleteOneAsync(x => x.Name == item.Name && x.Username == item.Username);

                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpGet]
            [Route("GetItemsByUsername")]
            public async Task<IActionResult> GetGroceryItems(string username)
            {
                try
                {
                    if (string.IsNullOrEmpty(username))
                    {
                        return BadRequest("Username is missing.");
                    }

                    var collection = _database.GetCollection<GroceryItem>("GroceryItem");

                    var result = await collection.Find(x => x.Username == username).ToListAsync();

                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpPatch]
            [Route("UpdateItem")]
            public async Task<IActionResult> UpdateGroceryItem([FromBody] GroceryItem item)
            {
                try
                {
                    if (item == null)
                    {
                        return BadRequest("Item to update is missing");
                    }

                    var update = Builders<GroceryItem>.Update.Set(x => x.Name, item.Name) // Set the updated properties of the item
                                         .Set(x => x.Quantity, item.Quantity)
                                         .Set(x => x.Username, item.Username);

                    var collection = _database.GetCollection<GroceryItem>("GroceryItem");

                    var result = await collection.UpdateOneAsync<GroceryItem>(x => x.Id == item.Id, update);

                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
