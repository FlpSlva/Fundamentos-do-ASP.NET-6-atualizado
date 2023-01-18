using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMVC(builder);
ConfigureServices(builder);

//builder.Services.AddDbContext<BlogDataContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("default")));

var app = builder.Build();
LoadConfiguration(app);
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");

    var smtp = new Configuration.SmtConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
}

void ConfigureMVC(WebApplicationBuilder builder)
{
    builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.SuppressModelStateInvalidFilter = true;
    });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<BlogDataContext>();
    builder.Services.AddTransient<TokenServices>();
    builder.Services.AddTransient<EmailService>();
}