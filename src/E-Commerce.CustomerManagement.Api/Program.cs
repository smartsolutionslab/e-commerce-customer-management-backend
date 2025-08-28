using E_Commerce.Common.Api.Extensions;
using E_Commerce.Common.Persistence.Extensions;
using E_Commerce.CustomerManagement.Api.Endpoints;
using E_Commerce.CustomerManagement.Api.Middleware;
using E_Commerce.CustomerManagement.Infrastructure.Extensions;
using E_Commerce.CustomerManagement.Infrastructure.Persistence;
using CommonApi = E_Commerce.Common.Api.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

CommonApi.AddApiVersioning(builder.Services);

builder.Services.AddCustomerManagementInfrastructure(builder.Configuration);

// API documentation
builder.Services.AddSwaggerWithVersioning();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Management API v1");
    });
}

app.UseHttpsRedirection();

// Custom middleware
app.UseMiddleware<TenantMiddleware>();

// Add customer endpoints
app.MapCustomerEndpoints();

// Health check
app.MapHealthChecks("/health");

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    context.Database.EnsureCreated();
}

app.Run();