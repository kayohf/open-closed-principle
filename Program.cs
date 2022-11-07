// Utilizando o ProductFilter

var apple = new Product("Apple", Color.Green, Size.Small);
var tree = new Product("Tree", Color.Green, Size.Large);
var house = new Product("House", Color.Blue, Size.Large);

Product[] products = {apple, tree, house};

var productFilter = new ProductFilter();

var productFilterOCP = new ProductFilterOCP();
var greenColorSpec = new ColorSpecification(Color.Green);
var largeSizeSpec = new SizeSpecification(Size.Large);

var gerrLargeAndSpec = new AndSpecification<Product>(greenColorSpec, largeSizeSpec);
var greenLargeAndSpec = greenColorSpec & largeSizeSpec;

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

// Utilizando o filtro OCP com AndSpecification
Console.WriteLine("Filtro OCP + AndSpec");
foreach (var pr in productFilterOCP.Filter(products, gerrLargeAndSpec)) {
    Console.WriteLine(pr);
}

// Utilizando o filtro OCP com AndSpecification e operador
Console.WriteLine("Filtro OCP + AndSpec & Operator");
foreach (var pr in productFilterOCP.Filter(products, greenLargeAndSpec)) {
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
public class ProductFilterOCP : IFilter<Product>
{
    public IEnumerable<Product> Filter(IEnumerable<Product> items, SpecificationAbstract<Product> spec)
    {
        foreach (var i in items) {
            if (spec.IsSatisfied(i)) yield return i;
        }
    }
}

// Especificacoes e abstracoes

public abstract class SpecificationAbstract<T> {
    public abstract bool IsSatisfied(T p);

    public static SpecificationAbstract<T> operator
        &(SpecificationAbstract<T> primeiro, SpecificationAbstract<T> segundo) {
            return new AndSpecification<T>(primeiro, segundo);   
        }
}

// public interface ISpecification<T> {
//     bool IsSatisfied(T item);
// }

public interface IFilter<T> {
    IEnumerable<T> Filter(IEnumerable<T> items, SpecificationAbstract<T> spec);
}

public class AndSpecification<T> : SpecificationAbstract<T>
{
    private readonly SpecificationAbstract<T> _primeiro, _segundo;

    public AndSpecification(SpecificationAbstract<T> primeiro, SpecificationAbstract<T> segundo) {
        _primeiro = primeiro;
        _segundo = segundo;
    }
    public override bool IsSatisfied(T item)
    {
        return _primeiro.IsSatisfied(item) && _segundo.IsSatisfied(item);
    }
}

public class ColorSpecification : SpecificationAbstract<Product>
{
    private Color _color;
    
    public ColorSpecification(Color color) {
        _color = color;
    }

    public override bool IsSatisfied(Product product)
    {
        return product.color == _color;
    }
}

public class SizeSpecification : SpecificationAbstract<Product> {
    private Size _size;

    public SizeSpecification(Size size) {
        _size = size;
    }
 
    public override bool IsSatisfied(Product product) {
        return product.size == _size;
    }
}