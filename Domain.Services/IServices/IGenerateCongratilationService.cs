namespace Domain.Services.IServices;

public interface IGenerateCongratilationService
{
    Task<string?> GenerateAsync(string name, string interests, string wishes);
}