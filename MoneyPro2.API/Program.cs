using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoneyPro2.API;
using MoneyPro2.API.Data;
using MoneyPro2.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
LoadConfiguration(builder);
ConfigureAuthentication(builder);
ConfigureServices(builder);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void LoadConfiguration(WebApplicationBuilder builder)
{
#pragma warning disable CS8601 // Desligando possivel atribuicao de referencia nula.
    Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
#pragma warning restore CS8601 // Religando possivel atribuicao de referencia nula.

    var smtp = new Configuration.SmtpConfiguration();
    builder.Configuration.GetSection("Smtp").Bind(smtp);
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
            ValidateAudience = false
        };
    });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("MoneyPro2");
    builder.Services.AddDbContext<MoneyPro2DataContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
}
