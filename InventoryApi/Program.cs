using InventoryApi.Data;
using InventoryApi.repository;
using InventoryApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<LearningDbContext>(
    optins =>
    {
        optins.UseSqlServer(configuration.GetConnectionString(nameof(LearningDbContext)));
    });
builder.Services.AddControllers();
builder.Services.AddOpenApi();

//регистрация моих классов
builder.Services.AddScoped<IClientsRepository,ClientsRepository>();
builder.Services.AddScoped<IClientsServices, ClientsServices>();

builder.Services.AddScoped<IWorkesRepository, WorkesRepository>();
builder.Services.AddScoped<IWorkersServices, WorkersServices>();

builder.Services.AddScoped<IProductsRepository,ProductsRepository>();
builder.Services.AddScoped<IProductsServices, ProductsServices>();

builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddScoped<ITransactionsServices, TransactionsServices>();
//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
