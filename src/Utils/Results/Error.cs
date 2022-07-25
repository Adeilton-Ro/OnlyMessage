namespace Utils.Results;

public class Error
{
    public string? Detail { get; set; }

    public static Error OfNotFound(string name) => new() { Detail = $"{name} não foi encontrado(a)!" };
    public static Error OfUnauthorized(string resource) => new() { Detail = resource };
}
