using System;
namespace RoboChefServer.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Driver;

    namespace RoboChefServer
    {
        //test
        [ApiController]
        [Route("recipe")]
        public class RecipeController : ControllerBase
        {
            private readonly IMongoDatabase _database;

            private readonly IOpenAIProxy _openAIProxy;

            public RecipeController(IMongoDatabase database, IOpenAIProxy openAiProxy)
            {
                _database = database;
                _openAIProxy = openAiProxy;
            }

            [HttpGet]
            [Route("generate")]
            public async Task<IActionResult> GenerateRecipe(string prompt)
            {
                try
                {
                    if (string.IsNullOrEmpty(prompt))
                    {
                        return BadRequest("Please enter a valid prompt");
                    }

                    var gptResp = await _openAIProxy.SendChatMessage(prompt);

                    var response = "";

                    foreach(var item in gptResp)
                    {
                        response += item.Content;
                    }

                    return Ok(response);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            [HttpPost]
            [Route("addRecipe")]
            public async Task<IActionResult> AddRecipe([FromBody] Recipe recipe)
            {
                try
                {
                    if (string.IsNullOrEmpty(recipe.RecipeDescription))
                    {
                        return BadRequest("Missing Required Parameters name and quantity");
                    }

                    var collection = _database.GetCollection<Recipe>("GroceryItem");

                    var result = await collection.FindAsync(x => x.RecipeDescription!.Equals(recipe.RecipeDescription) && x.Username == recipe.Username);

                    if (result.FirstOrDefault() != null)
                    {
                        return BadRequest($"Recipe already exists");
                    }

                    await collection.InsertOneAsync(recipe);

                    return Ok(recipe);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
