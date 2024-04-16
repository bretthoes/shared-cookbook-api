using FluentValidation;
using Microsoft.EntityFrameworkCore;
using shared_cookbook_api.Data.Dtos;
using shared_cookbook_api.Data.Dtos.MappingProfiles;
using shared_cookbook_api.Repositories.Interfaces;
using shared_cookbook_api.Validators;
using SharedCookbookApi.Data;
using SharedCookbookApi.Repositories;
using SharedCookbookApi.Services;
using Newtonsoft.Json.Serialization;
using SharedCookbookApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());


// Add services to the container.

// Add repositories
builder.Services.AddScoped<ICookbookInvitationRepository, CookbookInvitationRepository>();
builder.Services.AddScoped<ICookbookRepository, CookbookRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();

// Add validators
builder.Services.AddScoped<IValidator<AuthenticationDto>, AuthenticationDtoValidator>();
builder.Services.AddScoped<IValidator<CookbookDto>, CookbookDtoValidator>();

builder.Services.AddSingleton<ISeedDataService, SeedDataService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomCors("AllowAllOrigins");

// Add Db context
builder.Services.AddDbContext<SharedCookbookContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(PersonMappings));
builder.Services.AddAutoMapper(typeof(CookbookMappings));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SharedCookbookContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

    }
    app.UseSwagger();
    app.UseSwaggerUI();
    app.SeedData();
}

app.UseCors("AllowAllOrigins");
// TODO uncomment this. only commented out for localhost usage
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
