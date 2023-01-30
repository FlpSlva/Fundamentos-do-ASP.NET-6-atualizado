using Blog.Data;
using Blog.Domain.Entities;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Blog.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {

            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();
                var posts = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Select(x =>
                new ListPostsViewModel
                {

                    Id = x.Id,
                    Title = x.Title,
                    slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name}, {x.Author.Email}",



                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts

                }));

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet("v1/posts/{int:id}")]
        public async Task<IActionResult> DetailsAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25
             )
        {
            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();
                var posts = await context
                    .Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                    .Include(x => x.Category)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .FirstOrDefaultAsync(x => x.Category.Id == id);

                if (posts == null) return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts

                }));
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<Post>("Falha interna no servidor"));
            }
        }

        [HttpGet("v1/posts/{category}")]
        public async Task<IActionResult> GetByCategoryAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] string category,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {

            try
            {
                var count = await context.Posts.AsNoTracking().CountAsync();
                var posts = await context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Category.Slug == category)
                .Select(x =>
                new ListPostsViewModel
                {

                    Id = x.Id,
                    Title = x.Title,
                    slug = x.Slug,
                    LastUpdateDate = x.LastUpdateDate,
                    Category = x.Category.Name,
                    Author = $"{x.Author.Name}, {x.Author.Email}",
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts

                }));

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
