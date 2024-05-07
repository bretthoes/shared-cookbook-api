using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using SharedCookbook.Api.Data.Dtos;
using SharedCookbook.Api.Data.Dtos.MappingProfiles;
using SharedCookbook.Api.Data;
using SharedCookbook.Api.Extensions;
using SharedCookbook.Api.Repositories.Interfaces;
using SharedCookbook.Api.Repositories;
using SharedCookbook.Api.Services;
using SharedCookbook.Api.Validators;
using SharedCookbook.Api.Handlers;

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
builder.Services.AddScoped<IValidator<CreateCookbookDto>, CreateCookbookDtoValidator>();

// Add services
builder.Services.AddSingleton<ISeedDataService, SeedDataService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

// Add logging
builder.Services.AddLogging(builder => builder.AddConsole());

// Add handlers
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCustomCors("AllowAllOrigins");
builder.Services.AddProblemDetails();

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
else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();

app.Run();
