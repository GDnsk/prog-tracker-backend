using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;

namespace ProgTracker.Domain.Entity.Interfaces;

public interface IBaseEntity
{
    public Id Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}