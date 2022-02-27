namespace ProgTracker.Domain.Model.Database;

public struct Id
{
    public string Value { get; set; }

    public Id(string id)
    {
        Value = id;
    }

    public static bool operator ==(Id a, Id b)
    {
        return a.Value == b.Value;
    }

    public static bool operator !=(Id a, Id b)
    {
        return a.Value != b.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is Id id &&
               Value == id.Value;
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? base.GetHashCode();
    }

    public override string? ToString()
    {
        return Value?.ToString() ?? base.ToString();
    }
}