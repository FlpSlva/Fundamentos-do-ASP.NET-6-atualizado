using Blog.Data;
using Blog.Extensions;
using Blog.Domain.Entities;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        {
            try
            {

                var categories = await context.Categories.ToListAsync();

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch(DbUpdateException e)
            {
                return BadRequest("Não foi possível listar categorias ");
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<List<Category>>("Erro: Falha interna no servidor"));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null) return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories/")]
        public async Task<IActionResult> PostAsync(
            [FromServices] BlogDataContext context,
            [FromBody] EditorCategoryViewModel model)
        {

            if (!ModelState.IsValid) return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category(0, model.Name, model.Slug );


                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}",category);
            }
            catch (DbUpdateException e)
            {
                return BadRequest("Não foi possível listar categorias ");
            }
            catch (Exception)
            {

                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetAsync(
           [FromServices] BlogDataContext context,
           [FromRoute] int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(category);
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context,
            [FromBody] EditorCategoryViewModel model)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound();
            
            category.ChangeName(model.Name);
            category.ChangeSlug(model.Slug);


            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(model);
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context
           )
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null) return NotFound();


            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok();
        }





    }
}
