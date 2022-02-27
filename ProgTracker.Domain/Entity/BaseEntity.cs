using ProgTracker.Domain.Entity.Interfaces;
using ProgTracker.Domain.Model;
using ProgTracker.Domain.Model.Database;

namespace ProgTracker.Domain.Entity;

public class BaseEntity : IBaseEntity
{
    public Id Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}