

namespace Catalog.Products.Features.GetProductById;

//public record GetProductByIdQuery(Guid ProductId)
//    : IQuery<GetProductByIdResult>;
//public record GetProductByIdResult(ProductDto Product);
public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == query.ProductId, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(query.ProductId);
        }

        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);

    }
}
