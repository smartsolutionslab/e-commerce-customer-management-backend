using Asp.Versioning;
using E_Commerce.CustomerManagement.Application.Commands;
using E_Commerce.CustomerManagement.Application.Queries;
using E_Commerce.CustomerManagement.Application.DTOs;
using E_Commerce.CustomerManagement.Domain.ValueObjects;
using E_Commerce.Common.Domain.ValueObjects;
using E_Commerce.Common.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.CustomerManagement.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/customers")
            .WithApiVersionSet()
            .HasApiVersion(1, 0)
            .WithTags("Customers")
            .RequireAuthorization();

        // GET /api/v1/customers
        group.MapGet("/", GetCustomersAsync)
            .WithName("GetCustomers")
            .WithOpenApi();

        // GET /api/v1/customers/{id}
        group.MapGet("/{id:guid}", GetCustomerByIdAsync)
            .WithName("GetCustomerById")
            .WithOpenApi();

        // POST /api/v1/customers
        group.MapPost("/", CreateCustomerAsync)
            .WithName("CreateCustomer")
            .WithOpenApi();

        // PUT /api/v1/customers/{id}
        group.MapPut("/{id:guid}", UpdateCustomerAsync)
            .WithName("UpdateCustomer")
            .WithOpenApi();

        // DELETE /api/v1/customers/{id}
        group.MapDelete("/{id:guid}", DeleteCustomerAsync)
            .WithName("DeleteCustomer")
            .WithOpenApi();

        // GET /api/v1/customers/{id}/addresses
        group.MapGet("/{id:guid}/addresses", GetCustomerAddressesAsync)
            .WithName("GetCustomerAddresses")
            .WithOpenApi();

        // POST /api/v1/customers/{id}/addresses
        group.MapPost("/{id:guid}/addresses", AddCustomerAddressAsync)
            .WithName("AddCustomerAddress")
            .WithOpenApi();
    }

    private static async Task<IResult> GetCustomersAsync(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20,
        [FromQuery] string? search = null)
    {
        var query = new GetCustomersQuery(page, limit, search);
        var result = await queryDispatcher.DispatchAsync<GetCustomersQuery, List<CustomerResponse>>(query);

        return result.IsSuccess 
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> GetCustomerByIdAsync(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromRoute] Guid id)
    {
        var query = new GetCustomerByIdQuery(id);
        var result = await queryDispatcher.DispatchAsync<GetCustomerByIdQuery, CustomerResponse>(query);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Error);
    }

    private static async Task<IResult> CreateCustomerAsync(
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromBody] CreateCustomerRequest request,
        HttpContext context)
    {
        var tenantId = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        if (string.IsNullOrEmpty(tenantId))
            return Results.BadRequest("Tenant ID is required");

        var command = new CreateCustomerCommand(
            TenantId.Create(tenantId),
            request.Email,
            request.FirstName,
            request.LastName,
            request.DateOfBirth);

        var result = await commandDispatcher.DispatchAsync<CreateCustomerCommand, CustomerId>(command);

        return result.IsSuccess
            ? Results.Created($"/api/v1/customers/{result.Value}", result.Value)
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> UpdateCustomerAsync(
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromRoute] Guid id,
        [FromBody] UpdateCustomerRequest request)
    {
        var command = new UpdateCustomerCommand(
            CustomerId.Create(id),
            request.FirstName,
            request.LastName,
            request.DateOfBirth);

        var result = await commandDispatcher.DispatchAsync<UpdateCustomerCommand>(command);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> DeleteCustomerAsync(
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromRoute] Guid id)
    {
        var command = new DeleteCustomerCommand(CustomerId.Create(id));
        var result = await commandDispatcher.DispatchAsync<DeleteCustomerCommand>(command);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Error);
    }

    private static async Task<IResult> GetCustomerAddressesAsync(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromRoute] Guid id)
    {
        var query = new GetCustomerAddressesQuery(CustomerId.Create(id));
        var result = await queryDispatcher.DispatchAsync<GetCustomerAddressesQuery, List<AddressResponse>>(query);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Error);
    }

    private static async Task<IResult> AddCustomerAddressAsync(
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromRoute] Guid id,
        [FromBody] AddAddressRequest request)
    {
        var command = new AddCustomerAddressCommand(
            CustomerId.Create(id),
            request.Street,
            request.City,
            request.PostalCode,
            request.Country,
            request.IsDefault);

        var result = await commandDispatcher.DispatchAsync<AddCustomerAddressCommand, Guid>(command);

        return result.IsSuccess
            ? Results.Created($"/api/v1/customers/{id}/addresses/{result.Value}", result.Value)
            : Results.BadRequest(result.Error);
    }
}