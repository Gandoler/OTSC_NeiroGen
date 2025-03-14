namespace Domain.Services.IServices;

public interface IGenerateCongratilation
{
    Task<string?> GenerateAsync(string? name, string? interests, string? wishes);
}