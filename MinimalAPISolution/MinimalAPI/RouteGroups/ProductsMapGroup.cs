
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.EndPointFilters;
using MinimalAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MinimalAPI.RouteGroups
{
    public static class ProductsMapGroup
    {
        public static List<Product> products = new List<Product>()
        {
             new Product(){Id = 1, ProductName = "iPhone"},

             new Product(){Id = 2, ProductName = "macBook"}
        };

        public static RouteGroupBuilder ProductsAPI (this RouteGroupBuilder group)
        {
            group.MapGet("/", async (HttpContext context) => {

                //var content = string.Join('\n', products.Select(temp => temp.ToString()));

                //await context.Response.WriteAsync(JsonSerializer.Serialize(products));

                return Results.Ok(products);
            });

            group.MapGet("/{id:int}", async (HttpContext context, int id) =>
            {
                Product? product = products.FirstOrDefault(temp => temp.Id == id);

                if (product == null)
                {
                    //context.Response.StatusCode = 400;
                    //await context.Response.WriteAsync("Incorrect Product Id");
                    //return;

                    return Results.BadRequest(new { error = "Incorrect Product ID" });
                }

                //await context.Response.WriteAsync(JsonSerializer.Serialize(product));

                return Results.Ok(product);

            });

            group.MapPost("/", async (HttpContext context, Product product) => {

                products.Add(product);

                //await context.Response.WriteAsync("Product Added");

                return Results.Ok(new { message = "Product Added" });
            })
                .AddEndpointFilter<MyCustomEndpointFilter>()
                .AddEndpointFilter(async (EndpointFilterInvocationContext context, EndpointFilterDelegate next) =>
                {
                    var product = context.Arguments.OfType<Product>().FirstOrDefault();

                    if (product == null)
                    {
                        return Results.BadRequest("Products detail are not found in the request");
                    }

                    var validationContext = new ValidationContext(product);

                    List<ValidationResult> error = new List<ValidationResult>();

                    bool isValid = Validator.TryValidateObject(product, validationContext, error, true);

                    if (!isValid)
                    {
                        return Results.BadRequest(error.FirstOrDefault()?.ErrorMessage);
                    }

                    var result = next(context);

                    return result;
                    
                });

            group.MapPut("/{id}", async (HttpContext context, int id, [FromBody]Product product) =>
            {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);

                if (productFromCollection == null)
                {
                    //context.Response.StatusCode = 400;
                    //await context.Response.WriteAsync("Incorrect Product Id");
                    //return;
                    return Results.BadRequest(new { error = "Incorrect Product ID" });
                }

                productFromCollection.ProductName = product.ProductName;

                //await context.Response.WriteAsync("Product Updated");

                return Results.Ok(new { message = "Product Updated" });
            });

            group.MapDelete("/{id}", async (HttpContext context, int id) =>
            {
                Product? productFromCollection = products.FirstOrDefault(temp => temp.Id == id);

                if (productFromCollection == null)
                {
                    //context.Response.StatusCode = 400;
                    //await context.Response.WriteAsync("Incorrect Product Id");
                    //return;

                    //return Results.BadRequest(new {error = "Incorrect Product Id"});

                    return Results.ValidationProblem(new Dictionary<string, string[]> { { "id", new string[] { "Incorrect Product Id" } } });
                }

                products.Remove(productFromCollection);

                //await context.Response.WriteAsync("Product Deleted");

                return Results.Ok(new {message = "Product Deleted"});
            });



            return group;
        }
    }
}
