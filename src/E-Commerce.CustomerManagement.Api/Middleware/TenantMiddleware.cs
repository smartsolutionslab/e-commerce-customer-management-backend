using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.Common.Infrastructure.Services;

namespace E_Commerce.CustomerManagement.Api.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        var tenantIdHeader = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        
        if (!string.IsNullOrEmpty(tenantIdHeader) && Guid.TryParse(tenantIdHeader, out var tenantGuid))
        {
            var tenantId = TenantId.Create(tenantGuid);
            tenantService.SetTenant(tenantId);
        }

        await _next(context);
    }
}
