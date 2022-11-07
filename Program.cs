// Utilizando o ProductFilter

var apple = new Product("Apple", Color.Green, Size.Small);
var tree = new Product("Tree", Color.Green, Size.Large);
var house = new Product("House", Color.Blue, Size.Large);

Product[] products = {apple, tree, house};

var productFilter = new ProductFilter();

var productFilterOCP = new ProductFilterOCP();
var greenColorSpec = new ColorSpecification(Color.Green);

// Utilizando o filtro normal
Console.WriteLine("Filtro normal");
foreach (var pr in productFilter.FilterByColor(products, Color.Green))
{
    Console.WriteLine(pr);
}

// Utilizand o filtro OCP
Console.WriteLine("Filtro OCP");
foreach (var pr in productFilterOCP.Filter(products, greenColorSpec)) {
    Console.WriteLine(pr);
}

// Record de Product e Enums relacionados
public enum Color { Red, Green, Blue }
public enum Size { Small, Medium, Large, Hude }
public record Product (string name, Color color, Size size);

// Filtro problematico
public class ProductFilter {
    public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color) {
        foreach (var p in products) {
            if (p.color == color) yield return p;
        }
    }

    public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size) {
        foreach (var p in products) {
            if (p.size == size) yield return p;
        }
    }

    public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color) {
        foreach (var p in products) {
            if (p.size == size && p.color == color) yield return p;
        }
    }
}

// Filtro utilizando OpenClosedPrinciple

public interface ISpecification<T> {
    bool IsSatisfied(T item);
}

public interface IFilter<T> {
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
}

public class ProductFilterOCP : IFilter<Product>
{
    public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
    {
        foreach (var i in items) {
            if (spec.IsSatisfied(i)) yield return i;
        }
    }
}

public class ColorSpecification : ISpecification<Product>
{
    private Color color;
    
    public ColorSpecification(Color color) {
        this.color = color;
    }

    public bool IsSatisfied(Product product)
    {
        return product.color == color;
    }
}