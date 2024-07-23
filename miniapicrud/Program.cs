using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using miniapicrud.AutoMapper;
using miniapicrud.Data;
using miniapicrud.IProductRepos;
using miniapicrud.Migrations;
using miniapicrud.Model;
using miniapicrud.Model.dto;
using miniapicrud.ProductValidator;
using miniapicrud.Repos;
using miniapicrud.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProductRepo, ProductRepost>();
builder.Services.AddScoped<IProductServices, ProductServices>();

builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAcccountService, AccountService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));



builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(

    opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    }

    );


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
       
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "Jwt",
        In = ParameterLocation.Header

    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
             
            },
             Array.Empty<string>()
          }
        });



});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
         policy => policy.RequireRole("Admin","User", "Administrator"));
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

});





var app = builder.Build();

    





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();






app.MapGet("getall", async (IProductServices productservice) =>
{
    return Results.Ok(await productservice.getall());

}).AllowAnonymous();



app.MapGet("getbyid/{id:int}",[Authorize(Roles ="Admin")]  async(IProductServices productservice, int id) =>{
    return Results.Ok(await productservice.getbyid(id));

});


app.MapPost("CreateProduct", async (AddRequestDto req,IProductServices productservice) =>
{

    return Results.Ok(await productservice.Add(req));
    
}).RequireAuthorization();

app.MapPut("UpdateProduct/", async (UpdateRequestDto req, IProductServices productservice) =>
{

    return Results.Ok(await productservice.Update(req));

});


app.MapDelete("DeleteProducts/{id:int}", async (IProductServices productservice,int id) =>
{

    return Results.Ok(await productservice.Delete(id));

});




app.MapPost("Register",async(IAcccountService account,RegisterDto reg) =>
{
    return Results.Ok(await account.Register(reg));

});

app.MapPost("login", async (IAcccountService account,LoginDto login) =>
{


    return Results.Ok(await account.Login(login));
    
}).AllowAnonymous();
 






var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi().RequireAuthorization();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
