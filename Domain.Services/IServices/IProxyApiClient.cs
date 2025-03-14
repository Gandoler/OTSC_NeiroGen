
using Entities.Templates;

namespace Domain.Services.IServices;

public interface IProxyApiClient
{
    Task<bool> CongratilationToProxyApiAsync(PozdrStringDTO pozdrik);
    Task<AddIntAndPozhDto> GetAddIntAndCongratilationAsync(AppIdDto appId);
    Task <string> GetName(PozdrikIdDto pozdrikId);
}