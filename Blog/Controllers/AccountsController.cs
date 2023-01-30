using Blog.Data;
using Blog.Extensions;
using Blog.Domain.Entities;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace Blog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/accounts/create")]
        public async Task<IActionResult> PostAsync(
        [FromBody] RegisterViewModel model,
        [FromServices] BlogDataContext context,
        [FromServices] EmailService emailService)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            try
            {
                var user = new User(model.Name, model.Email, "string", model.Email.Replace("@", "-").Replace(".", "-"), "string");
                var roles = new Role(user.Name, user.Email);
                var posts = new Post();
                user.AddRole(roles);
                user.AddPosts(posts);
                

                var password = PasswordGenerator.Generate(25);
                user.SetPasswordHash(PasswordHasher.Hash(password));

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                emailService.Send(
                    user.Name,
                    user.Email,
                    "Bem vindo ao Blog",
                    $"Sua senha é <strong>{password}</strong>");
                

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException)
            {

                return StatusCode(400, new ResultViewModel<string>("05x12 - Este E-mail ja está cadastrado"));
            }
            catch
            {

                return StatusCode(500, new ResultViewModel<dynamic>("05x12 - Falha interna no servidor"));
            }

        }

        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login([FromServices] TokenServices tokenServices,
            [FromServices] BlogDataContext context, [FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await context
                .Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Pasword))
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

            try
            {
                var token = tokenServices.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, errors: null));
            }
            catch
            {

                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
            }
        }

        [Authorize]
        [HttpPost("v1/accounts/upload-image")]
        public async Task<IActionResult> UploadImage([FromBody] UploadImageViewModel model,
            [FromServices] BlogDataContext context)
        {
            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, " ");
            var bytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
            }

            var user = await context
                .Users
                .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null) return NotFound(new ResultViewModel<User>("Usuário não encontrado"));


            var url = $"https://localhost:0000/images/{fileName}";
            user.UpdatedImage(url);
            

            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<string>("Falha interna no servidor"));
            }


            return  Ok(new ResultViewModel<string>("Imagem alterada com sucesso"));
        }

    }
}
