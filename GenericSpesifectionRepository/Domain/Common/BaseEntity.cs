namespace GenericSpesifectionRepositoryApp.Domain.Common;

public abstract class BaseEntity<TKey> : IEntity<TKey> where TKey : struct
{
    public TKey Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
}