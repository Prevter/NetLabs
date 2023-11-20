namespace AlgLabs.Labs.Lab04;

public sealed record City(string Name, int Year, int Population)
{
    public override string ToString()
    {
        return $"{Name} ({Year} р.) - {Population:N0}";
    }
}
