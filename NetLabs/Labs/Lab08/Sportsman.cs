namespace NetLabs.Labs.Lab08;

public sealed record Sportsman(
    int Id,
    string FirstName,
    string LastName,
    string MiddleName,
    int Year,
    bool IsMale,
    string SportName,
    string Achievements,
    string Prizes
)
{
    public object FullName => $"{LastName} {FirstName} {MiddleName}";
}
