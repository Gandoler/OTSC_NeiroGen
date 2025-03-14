using Domain.Services.IServices;
using Entities.Templates;

namespace Infrastructure.Business.SERVICES;

public class AddNewCongratulationsService : IAddNewCongratulationsService
{
    private readonly IGenerateCongratilation _generateCongratulationsService;
    private readonly IProxyApiClient _proxyApiClient;

    public AddNewCongratulationsService(IGenerateCongratilation generateCongratulationsService,
        IProxyApiClient proxyApiClient)
    {
        this._generateCongratulationsService = generateCongratulationsService;
        this._proxyApiClient = proxyApiClient;
    }
    
    public async Task<bool> AddNewCongratulations(PozdrikIdDto pozdrikIdDto)
    {
        AddIntAndPozhDto? inters =  await _proxyApiClient.GetAddIntAndCongratilationAsync(pozdrikIdDto);
        string? name = await _proxyApiClient.GetName(pozdrikIdDto);
        string? Congratulations;
        if (inters is not null)
        {
            Congratulations =await _generateCongratulationsService.GenerateAsync(name, inters.Interests, inters.Pozhelania);
        }
        else
        {
            Congratulations =await _generateCongratulationsService.GenerateAsync(name,String.Empty, String.Empty);

        }

        return await _proxyApiClient.CongratilationToProxyApiAsync(new PozdrStringDTO
        {
            _pozdr = Congratulations, _pozdrikId = pozdrikIdDto._pozdrikId
        });
    }
}