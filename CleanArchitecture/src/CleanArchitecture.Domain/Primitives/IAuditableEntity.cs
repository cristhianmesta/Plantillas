namespace CleanArchitecture.Domain.Primitives;

public interface IAuditableEntity
{
    public string CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
}
