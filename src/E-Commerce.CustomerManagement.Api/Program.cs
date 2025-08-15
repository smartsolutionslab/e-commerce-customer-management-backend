using E_Commerce.Common.Api.Extensions;
using E_Commerce.Common.Api.Middleware;
using E_Commerce.Common.Infrastructure.Extensions;
using E_Commerce.CustomerManagement.Api.Endpoints;
using E_Commerce.CustomerManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Common services
builder.Services.AddCommonServices();
builder.Services.AddCommonInfrastructure(builder.Configuration);
builder.Services.AddMultiTenancy();
builder.Services.AddApiVersioning();
builder.Services.AddCorsPolicies();

// Customer management specific services
builder.Services.AddCustomerManagementInfrastructure(builder.Configuration);

// Health checks
builder.Services.AddDatabaseHealthCheck<CustomerDbContext>();

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

// Security and middleware
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowFrontends");

// Authentication
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtAuthenticationMiddleware>();

// Add customer endpoints
app.MapCustomerEndpoints();

// Health check
app.MapHealthChecks("/health");

app.Run();
