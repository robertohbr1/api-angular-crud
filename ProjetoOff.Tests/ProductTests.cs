using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ProjetoOff.Api.Features.Products;

namespace ProjetoOff.Tests;

public class ProductTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreated()
    {
        // Arrange
        var request = new CreateProductRequest("Test Product", "Description", 10.50m);

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", request);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
        product.Should().NotBeNull();
        product!.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task GetProducts_ShouldReturnEmptyListInitially()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
        products.Should().NotBeNull();
    }
}
