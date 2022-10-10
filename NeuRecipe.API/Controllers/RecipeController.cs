using Microsoft.AspNetCore.Mvc;
using NeuRecipe.Application.DTO;
using NeuRecipe.Application.Services.Interfaces;

namespace NeuRecipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipe(CreateRecipeDTO recipe)
        {
                var result = await recipeService.CreateRecipe(recipe);
                return StatusCode(201, result); 
        }
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await recipeService.GetRecipes();
            return Ok(recipes);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRecipe(UpdateRecipeDTO recipe)
        {
            var result = await recipeService.UpdateRecipe(recipe);
            return StatusCode(200, result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await recipeService.DeleteRecipe(id);
            return Ok(response);
        }
        [HttpGet("Image/{id}")]
        public async Task<IActionResult> GetImageByID(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"Invalid Id : {id}");
            }
            var result = await recipeService.GetImageById(id);
            return Ok(result);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"Invalid Id : {id}");
            }
            var result = await recipeService.GetRecipeById(id);
            return Ok(result);
        }
    }
}
