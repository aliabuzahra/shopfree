using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopFree.Domain.Interfaces;
using ShopFree.Infrastructure.Data;

namespace ShopFree.Infrastructure.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantMiddleware> _logger;
    
    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(
        HttpContext context,
        ITenantService tenantService,
        ApplicationDbContext dbContext)
    {
        // Extract subdomain from host
        var host = context.Request.Host.Host;
        var subdomain = ExtractSubdomain(host);
        
        if (!string.IsNullOrEmpty(subdomain))
        {
            // Find store by subdomain
            var store = await dbContext.Stores
                .FirstOrDefaultAsync(s => s.Subdomain == subdomain);
            
            if (store != null)
            {
                tenantService.SetCurrentStoreId(store.Id);
                tenantService.SetCurrentSubdomain(subdomain);
                context.Items["StoreId"] = store.Id;
                context.Items["Store"] = store;
                context.Items["Subdomain"] = subdomain;
                
                _logger.LogInformation($"Tenant identified: {subdomain} (Store ID: {store.Id})");
            }
            else
            {
                _logger.LogWarning($"Store with subdomain '{subdomain}' not found");
            }
        }
        
        await _next(context);
    }
    
    private string? ExtractSubdomain(string host)
    {
        // Extract subdomain from host (e.g., "store1.shopfree.com" -> "store1")
        // For localhost: "store1.localhost:5000" -> "store1"
        var parts = host.Split('.');
        
        if (parts.Length >= 3)
        {
            // Production: store1.shopfree.com
            return parts[0];
        }
        else if (parts.Length == 2 && parts[1].StartsWith("localhost"))
        {
            // Development: store1.localhost:5000
            return parts[0];
        }
        else if (host.Contains("localhost") && host.Contains(":"))
        {
            // Development with port: store1.localhost:5000
            var localhostParts = host.Split(':');
            if (localhostParts.Length > 0)
            {
                var domainParts = localhostParts[0].Split('.');
                if (domainParts.Length >= 2)
                {
                    return domainParts[0];
                }
            }
        }
        
        return null;
    }
}

