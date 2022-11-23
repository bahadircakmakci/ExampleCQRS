using ExampleCQRS.Application.Elasticsearch.BookElasticServices;
using ExampleCQRS.Infastructure.Context;
using ExampleCQRS.Infastructure.Repositories.Base.Concrete;
using ExampleCQRS.Infastructure.Repositories.Base.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CQRSDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IBookElasticsearchService, BookElasticsearchService>();
builder.Services.AddMediatR(Assembly.Load("ExampleCQRS.Domain"), Assembly.Load("ExampleCQRS.Application"), Assembly.Load("ExampleCQRS.Infastructure"));
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
