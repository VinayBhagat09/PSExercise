using ABCPharmacy.CoreAPI.MapperProfiles.Medicines;
using ABCPharmacy.CoreAPI.Middleware;
using ABCPharmacy.CoreAPI.Models.Validators;
using ABCPharmacy.CoreAPI.Services.Medicines;
using ABCPharmacy.CoreAPI.Services.Response;
using ABCPharmacy.CoreAPI.Services.SalesRecords;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "default", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(10); 
        options.QueueLimit = 0; 
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<ISalesRecordsService, SalesRecordsService>();
builder.Services.AddScoped<IResponseService, ResponseService>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt",rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUpdateMedicineDTOValidator>();
builder.Services.AddAutoMapper(typeof(MedicinesProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();
app.Run();
