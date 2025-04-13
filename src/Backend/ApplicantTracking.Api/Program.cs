using ApplicantTracking.CrossCutting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();

builder.Services.AddApiConfiguration();
builder.Services.AddRateLimiting();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFilters();
builder.Services.AddSwagger();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

app.UseRateLimiter();
app.UseCustomSwagger(app.Environment);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.EnsureDatabaseMigrated(builder.Configuration);

await app.RunAsync();

//Exibir program nos testes
public partial class Program
{
    protected Program() { }
}
