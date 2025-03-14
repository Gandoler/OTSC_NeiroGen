
using Entities.Templates;

namespace Domain.Services.IServices;

public interface IProxyApiClient
{
    Task<bool> CongratilationToProxyApiAsync(PozdrStringDTO pozdrik);
    Task<AddIntAndPozhDto?> GetAddIntAndCongratilationAsync(PozdrikIdDto pozdrikId);
    Task <string?> GetName(PozdrikIdDto pozdrikId);
}