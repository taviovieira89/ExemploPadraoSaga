using Microsoft.EntityFrameworkCore;
using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Services;
using ContratacaoService.Domain.Interfaces;
using ContratacaoService.Infrastructure.Data;
using ContratacaoService.Infrastructure.Repositories;
using ContratacaoService.Infrastructure.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ContratacaoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register dependencies for Hexagonal Architecture
builder.Services.AddScoped<IContratacaoRepository, ContratacaoRepository>();
builder.Services.AddScoped<IContratacaoAppService, ContratacaoAppService>();

// Configure HttpClient for PropostaService
builder.Services.AddHttpClient<IPropostaServiceHttpClient, PropostaServiceHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("PropostaServiceUrl"));
});

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
