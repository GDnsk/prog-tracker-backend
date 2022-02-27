using ProgTracker.Domain.Model.Database;

namespace ProgTracker.Domain.Entity;

public class Trace : BaseEntity
{
    public DbRef<User> User { get; set; }
    
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public DateTime? StartCoding { get; set; }
    public DateTime? EndCoding { get; set; }
    public DateTime? LastInteraction { get; set; }

    public class FileModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Language { get; set; }
    }
    
}