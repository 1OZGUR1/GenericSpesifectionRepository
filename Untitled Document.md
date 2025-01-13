**Türkçe Dökümantasyon**

**Başlık: .NET Core'da Repository Pattern, Specification Pattern ve Entity Framework Core ile Veri Erişim Katmanı**

**Giriş:**
Bu dökümantasyon, .NET Core uygulamalarında veriye erişimi soyutlamak ve daha sürdürülebilir bir mimari oluşturmak için kullanılan Repository Pattern ve Specification Pattern'in Entity Framework Core (EF Core) ile nasıl birlikte kullanıldığını açıklar. Bu yaklaşım, veritabanı erişim mantığını uygulamanın iş mantığından ayırılmasına ve daha test edilebilir, esnek ve kolayca yönetilebilir bir kod tabanı oluşturulmasına yardımcı olur.

**Temel Kavramlar:**

*   **Repository Pattern:** Veri erişim mantığını soyutlayan ve veri kaynaklarına doğrudan erişimi engelleyen bir tasarım desenidir. Temel olarak CRUD (Create, Read, Update, Delete) işlemlerini kapsayan bir arayüz (IRepository) ve bu arayüzü uygulayan somut bir sınıf (Repository) ile çalışır.
*   **Specification Pattern:** Sorgu mantığını (filtreleme, sıralama, dahil etme) kapsülleyen ve yeniden kullanılabilir nesneler oluşturarak sorguların daha esnek ve modüler hale gelmesini sağlayan bir tasarım desenidir.  Temel olarak sorgu kriterlerini (ISpecification) tanımlayan bir arayüz ve bu arayüzü uygulayan soyut bir sınıf (BaseSpecification) ile çalışır.
*   **Entity Framework Core (EF Core):** .NET uygulamalarında veritabanı işlemlerini kolaylaştıran bir ORM (Object-Relational Mapper) aracıdır. Veritabanı tablolarını C# sınıfları olarak temsil ederek, SQL yazmak yerine C# kodları ile veri erişimi sağlar.

**Mimari Bileşenleri:**

1.  **`IEntity<TKey>` Arayüzü:**
    *   Veritabanı tablolarını temsil eden varlık sınıflarının (entity) sahip olması gereken ortak özellikleri tanımlar (`Id`, `CreatedDate`, `UpdatedDate`, `IsDeleted`).

2.  **`BaseEntity<TKey>` Soyut Sınıfı:**
    *   `IEntity<TKey>` arayüzünü uygulayan ve temel varlık özelliklerini somut olarak sağlayan soyut bir temel sınıf.

3.  **`ISpecification<T>` Arayüzü:**
    *   Varlıkları sorgulamak için kullanılan kriterleri (filtreleme, sıralama, dahil etme vb.) tanımlar.

4.  **`BaseSpecification<T>` Soyut Sınıfı:**
    *   `ISpecification<T>` arayüzünü uygulayan ve temel şartname özelliklerini (filtreleme, sıralama, include gibi) sağlayan soyut bir temel sınıf.

5.  **`IRepository<T, TKey>` Arayüzü:**
    *   Varlıklar üzerinde gerçekleştirilecek temel veri erişim işlemlerini (CRUD) tanımlar.

6.  **`Repository<T, TKey>` Somut Sınıfı:**
    *   `IRepository<T, TKey>` arayüzünü uygulayan ve EF Core kullanarak veritabanı işlemlerini gerçekleştiren somut bir sınıf.

7.  **`SpecificationEvaluator<T>` Sınıfı:**
    *   `ISpecification<T>` arayüzündeki kriterlere göre sorguları oluşturmaktan sorumludur.

**Nasıl Çalışır:**

1.  **Varlıklar (Entity):** Veritabanı tablolarını temsil eden C# sınıfları. `IEntity<TKey>` arayüzünü veya `BaseEntity<TKey>` sınıfını uygular.

2.  **Şartnameler (Specification):** Veri sorgularında kullanılan kriterleri belirleyen ve yeniden kullanılabilir nesnelerdir. `BaseSpecification<T>` sınıfından kalıtım alarak özel sorgu şartnameleri oluşturulabilir.

3.  **Repository:** Veri erişim mantığını soyutlar. Bir `IRepository<T, TKey>` örneği oluşturularak `GetByIdAsync`, `ListAsync`, `AddAsync` vb. metotlar ile veri tabanına erişilir.

4.  **Veri Erişim:** Repository sınıfı, EF Core kullanarak veritabanı işlemlerini gerçekleştirir. Şartnameler aracılığıyla, daha karmaşık sorgular (filtreleme, sıralama, include) kolayca tanımlanabilir.

**Kullanım Senaryoları:**

*   **Basit Veri Erişim:**
    *   Bir varlığı kimliğine göre getirmek.
    *   Tüm varlıkları listelemek.
*   **Gelişmiş Veri Erişim:**
    *   Belirli kriterlere göre varlıkları filtrelemek.
    *   İlişkili varlıkları dahil etmek.
    *   Verileri belirli bir özelliğe göre sıralamak.
    *   Sayfalama kullanarak büyük veri setlerini yönetmek.

**Avantajları:**

*   **Test Edilebilirlik:** Veritabanına bağımlılık azaltılarak testler daha kolay yazılır ve yürütülür.
*   **Bakım Kolaylığı:** Veritabanı erişim mantığı tek bir yerde toplandığından, değişiklikler kolayca yapılır.
*   **Yeniden Kullanılabilirlik:** Şartnameler ve repository sınıfları, farklı yerlerde tekrar tekrar kullanılabilir.
*   **Esneklik:** Farklı veri kaynaklarına geçiş daha kolaydır.

**Özet:**
Bu mimari, .NET Core uygulamalarında veriye erişimi düzenli bir şekilde soyutlayarak daha iyi bir kod tabanı oluşturmanıza yardımcı olur. Repository ve Specification pattern'leri sayesinde, veritabanı erişimi daha test edilebilir, esnek ve sürdürülebilir hale gelir.

**EN Document**

**Title: Data Access Layer with Repository Pattern, Specification Pattern, and Entity Framework Core in .NET Core**

**Introduction:**
This documentation explains how to use the Repository Pattern and Specification Pattern in conjunction with Entity Framework Core (EF Core) to abstract data access and create a more sustainable architecture in .NET Core applications. This approach helps separate database access logic from the application's business logic, leading to a more testable, flexible, and easily maintainable codebase.

**Core Concepts:**

*   **Repository Pattern:** A design pattern that abstracts data access logic and prevents direct access to data sources. It primarily works with an interface (IRepository) that covers CRUD (Create, Read, Update, Delete) operations and a concrete class (Repository) that implements this interface.
*   **Specification Pattern:** A design pattern that encapsulates query logic (filtering, sorting, including) and allows queries to be more flexible and modular by creating reusable objects. It primarily works with an interface (ISpecification) that defines query criteria and an abstract class (BaseSpecification) that implements this interface.
*   **Entity Framework Core (EF Core):** An ORM (Object-Relational Mapper) tool that simplifies database operations in .NET applications. It represents database tables as C# classes, enabling data access through C# code instead of writing SQL.

**Architectural Components:**

1.  **`IEntity<TKey>` Interface:**
    *   Defines the common properties that entity classes representing database tables should have (`Id`, `CreatedDate`, `UpdatedDate`, `IsDeleted`).

2.  **`BaseEntity<TKey>` Abstract Class:**
    *   An abstract base class that implements the `IEntity<TKey>` interface and provides concrete implementations for basic entity properties.

3.  **`ISpecification<T>` Interface:**
    *   Defines the criteria (filtering, sorting, including, etc.) used to query entities.

4.  **`BaseSpecification<T>` Abstract Class:**
    *   An abstract base class that implements the `ISpecification<T>` interface and provides basic specification properties (filtering, sorting, includes).

5.  **`IRepository<T, TKey>` Interface:**
    *   Defines the basic data access operations (CRUD) that will be performed on entities.

6.  **`Repository<T, TKey>` Concrete Class:**
    *   A concrete class that implements the `IRepository<T, TKey>` interface and performs database operations using EF Core.

7.  **`SpecificationEvaluator<T>` Class:**
    *   Responsible for building queries based on the criteria specified in the `ISpecification<T>` interface.

**How It Works:**

1.  **Entities:** C# classes representing database tables. Implement the `IEntity<TKey>` interface or extend the `BaseEntity<TKey>` class.

2.  **Specifications:** Reusable objects that determine the criteria used in data queries. Custom query specifications can be created by inheriting from the `BaseSpecification<T>` class.

3.  **Repository:** Abstracts the data access logic. Access the database by creating an instance of `IRepository<T, TKey>` and using methods such as `GetByIdAsync`, `ListAsync`, and `AddAsync`.

4.  **Data Access:** The Repository class performs database operations using EF Core. More complex queries (filtering, sorting, including) can easily be defined using specifications.

**Use Cases:**

*   **Simple Data Access:**
    *   Retrieving an entity by its ID.
    *   Listing all entities.
*   **Advanced Data Access:**
    *   Filtering entities based on specific criteria.
    *   Including related entities.
    *   Sorting data based on a property.
    *   Managing large datasets using pagination.

**Advantages:**

*   **Testability:** Reduced dependencies on the database make tests easier to write and execute.
*   **Maintainability:** Database access logic is centralized in one place, making changes easy.
*   **Reusability:** Specifications and repository classes can be reused in different parts of the application.
*   **Flexibility:** Easier to switch between different data sources.

**Summary:**
This architecture helps create a better codebase by systematically abstracting data access in .NET Core applications. Using the Repository and Specification patterns, database access becomes more testable, flexible, and maintainable.

<hr/>

**Örneği 1**

Ürünleri aramak, sıralamak ve sayfalama yapmak için tüm yapıyı birleştirelim.

**1. Adım: Yeni bir Specification oluşturun:**

```csharp
public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string nameSearch, int pageIndex, int pageSize)
        : base(p => p.Name.Contains(nameSearch))
    {
        ApplyPaging(pageIndex, pageSize);
        ApplyOrderBy(p => p.Name);
    }
}
```

**2. Adım: Repository'de Specification kullanarak sorgu yapın:**

```csharp
public class ProductService
{
    private readonly IRepository<Product, int> _productRepository;

    public ProductService(IRepository<Product, int> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetProductsAsync(string searchQuery, int pageIndex, int pageSize)
    {
        var spec = new ProductSpecification(searchQuery, pageIndex, pageSize);
        var products = await _productRepository.ListAsync(spec);
        return products;
    }
}
```

**Örneği 2**

Eğer çok sayıda kriter, `Include`, `ThenInclude`, ya da `Join` kullanmanız gerekiyorsa, **Specification Design Pattern**'ı, bu gibi durumları daha yönetilebilir ve esnek hale getirmek için oldukça faydalıdır. Bu durumda aşağıda, detaylı örnekler ile nasıl kullanabileceğinizi göstereceğim.

 1. **Çok Sayıda Kriter Kullanımı:**

Birden fazla filtreleme kriteri kullanarak sorgu oluşturmak için, `Criteria` özelliğine birden fazla filtreyi eklemeniz gerekecek. Bunun için `BaseSpecification` sınıfı içerisindeki `Criteria` ifadesini kullanabilirsiniz.

   **Örnek - Kriterli Sorgu Kullanımı:**

Diyelim ki, `Product` tablosunda ürün adı ve fiyat gibi kriterlerle sorgulama yapmamız gerekiyor. Bunun için şu şekilde bir Specification yazabilirsiniz:

```csharp
public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string name, decimal? minPrice, decimal? maxPrice)
        : base(p => (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
                    (!minPrice.HasValue || p.Price >= minPrice) &&
                    (!maxPrice.HasValue || p.Price <= maxPrice))
    {
    }
}
```

**Örneği 3**

**Çok Sayıda `Include` ve `ThenInclude` Kullanımı:**

`Include` ve `ThenInclude`, ilişkili verileri çekmek için kullanılır. Bu tür veriler, örneğin bir `Order` nesnesinin ilişkili olduğu `Customer` ve `Product` tablosu gibi durumlar için gereklidir.

 **Örnek - Birden Fazla `Include` ve `ThenInclude`:**

Diyelim ki `Order` nesnesini ve ilişkili `Customer` ve `Product` nesnelerini çekmek istiyorsunuz:

```csharp
public class OrderWithCustomerAndProductSpecification : BaseSpecification<Order>
{
    public OrderWithCustomerAndProductSpecification(int? customerId)
        : base(o => !customerId.HasValue || o.CustomerId == customerId)
    {
        AddInclude(o => o.Customer);  // First-level Include
        AddInclude(o => o.OrderItems); // Second-level Include for OrderItems
        AddInclude("OrderItems.Product"); // String Include (using Include path) for Product
    }
}
```

Bu örnekte:

- `AddInclude(o => o.Customer)` ile `Order` ile ilişkili `Customer` verisi dahil edilir.
- `AddInclude(o => o.OrderItems)` ile `Order` ile ilişkili `OrderItems` dahil edilir.
- `AddInclude("OrderItems.Product")` ile `OrderItems` içerisindeki her bir ürün (Product) dahil edilir.

**Örneği 4**
 
 **Join Kullanımı:**

Join işlemi yapmak için `IQueryable` üzerinde `Join` fonksiyonunu kullanabilirsiniz. Fakat, `Join` işlemi ile ilişkili verileri çekmek istiyorsanız, `SpecificationEvaluator` sınıfı üzerinden bunu eklemeniz gerekecek.

  **Örnek - `Join` Kullanımı:**

Örneğin, `Product` tablosu ile `Category` tablosunu birleştirerek belirli kategorideki ürünleri listelemek istiyoruz.

```csharp
public class ProductWithCategorySpecification : BaseSpecification<Product>
{
    public ProductWithCategorySpecification(int categoryId)
        : base(p => p.CategoryId == categoryId)
    {
    }

    protected override void ApplyJoins(IQueryable<Product> query)
    {
        query = query.Join(_dbContext.Categories, 
            p => p.CategoryId, 
            c => c.Id, 
            (p, c) => new { Product = p, Category = c });
    }
}
```

**Örneği 5**

 **Birden Fazla Join, Include ve GroupBy Kullanımı:**

Birden fazla `Join`, `Include`, ve `GroupBy` işlemi kullanmak için yine `Specification` pattern'ı ile sorgunuzu genişletebilirsiniz.

   **Örnek - Birden Fazla `Join`, `Include`, ve `GroupBy` Kullanımı:**

Diyelim ki, `Order` tablosu ile `Customer`, `OrderItems`, ve `Product` tablolarını birleştirerek, her bir ürün için sipariş adetlerini almak istiyoruz.

```csharp
public class OrderWithItemsAndCustomerSpecification : BaseSpecification<Order>
{
    public OrderWithItemsAndCustomerSpecification(DateTime startDate, DateTime endDate)
        : base(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
    {
        AddInclude(o => o.Customer);  // Include Customer
        AddInclude(o => o.OrderItems);  // Include OrderItems
        AddInclude("OrderItems.Product");  // Include Product via OrderItems

        ApplyOrderBy(o => o.OrderDate);  // Order by OrderDate
    }

    protected override void ApplyJoins(IQueryable<Order> query)
    {
        query = query
            .Join(_dbContext.Customers, o => o.CustomerId, c => c.Id, (o, c) => new { Order = o, Customer = c })
            .Join(_dbContext.OrderItems, oc => oc.Order.Id, oi => oi.OrderId, (oc, oi) => new { Order = oc.Order, Customer = oc.Customer, OrderItem = oi })
            .Join(_dbContext.Products, oic => oic.OrderItem.ProductId, p => p.Id, (oic, p) => new { Order = oic.Order, Customer = oic.Customer, OrderItem = oic.OrderItem, Product = p });
    }

    protected override void ApplyGroupBy(IQueryable<Order> query)
    {
        query = query.GroupBy(o => o.Product)
            .Select(g => new 
            {
                Product = g.Key,
                TotalQuantity = g.Sum(x => x.OrderItem.Quantity)
            });
    }
}
```

	Bu örnekte:

- `AddInclude` ile ilişkili `Customer`, `OrderItems`, ve `Product` tabloları dahil ediliyor.
- `ApplyJoins` ile `Order`, `Customer`, `OrderItems`, ve `Product` tabloları `Join` ile birleştiriliyor.
- `ApplyGroupBy` ile her bir `Product` için sipariş adetleri gruplandırılıyor.

**Örneği 6**

**Karmaşık Filtreleme, Join ve Include Kullanımı:**

Eğer çok daha karmaşık filtreleme koşulları ve `Include` işlemleri gerekiyorsa, her bir filtreyi ve ilişkili verileri daha modüler hale getirebilirsiniz. Aşağıda, karmaşık bir sorgu örneği bulunmaktadır.

 **Örnek - Karmaşık Sorgu:**
 
```csharp
public class ComplexOrderSpecification : BaseSpecification<Order>
{
    public ComplexOrderSpecification(DateTime startDate, DateTime endDate, int? customerId, string productName)
        : base(o => o.OrderDate >= startDate && o.OrderDate <= endDate &&
                    (!customerId.HasValue || o.CustomerId == customerId) &&
                    (string.IsNullOrEmpty(productName) || o.OrderItems.Any(oi => oi.Product.Name.Contains(productName))))
    {
        AddInclude(o => o.Customer);
        AddInclude(o => o.OrderItems);
        AddInclude("OrderItems.Product");

        ApplyOrderBy(o => o.OrderDate);
    }
}
```

Bu örnekte:

- `OrderDate` için tarih aralığı,
- `CustomerId` için belirli bir müşteri,
- `Product.Name` için belirli bir ürün adı filtresi uygulanıyor.

 Özet:

Çok sayıda kriter, `Include`, `ThenInclude`, ve `Join` kullanımında, **Specification Pattern** size oldukça esnek ve yönetilebilir bir çözüm sunar. Bu sayede:

- Sorgu karmaşıklığını izole edebilir,
- Kapsamlı veri çekme işlemlerini yönetebilirsiniz,
- Aynı zamanda sorgularınızı modüler ve tekrar kullanılabilir şekilde tutabilirsiniz.

Eğer projede çok fazla ilişkili veri ve karmaşık sorgular kullanıyorsanız, bu desen ile işlerinizi oldukça kolaylaştırabilirsiniz.