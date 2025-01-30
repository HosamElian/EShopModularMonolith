
namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(ProductDto Product)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x=>x.Product.Name).NotEmpty().WithMessage("Name Is Required");
        RuleFor(x=>x.Product.Category).NotEmpty().WithMessage("Category Is Required");
        RuleFor(x=>x.Product.ImageFile).NotEmpty().WithMessage("ImageFile Is Required");
        RuleFor(x=>x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}


public class CreateProductHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            Guid.NewGuid(),
            command.Product.Name,
            command.Product.Category,
            command.Product.Description,
            command.Product.ImageFile,
            command.Product.Price
            );

        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateProductResult(product.Id);
    }
}
