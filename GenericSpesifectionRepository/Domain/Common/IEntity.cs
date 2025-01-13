namespace GenericSpesifectionRepositoryApp.Domain.Common;

public interface IEntity<TKey> where TKey : struct
{
    TKey Id { get; set; }

    DateTime CreatedDate { get; set; }

    DateTime? UpdatedDate { get; set; }

    bool IsDeleted { get; set; }
}