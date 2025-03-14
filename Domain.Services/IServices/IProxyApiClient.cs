
using Entities.Templates;

namespace Domain.Services.IServices;

public interface IProxyApiClient
{
    Task<bool> SendPozdrikToProxyApiAsync(PozdrStringDTO pozdrik);
    Task<AddIntAndPozhDto> GetAddIntAndPozdrikAsync(AppIdDto appId);
    Task <string> GetName(PozdrikIdDto pozdrikId);
}