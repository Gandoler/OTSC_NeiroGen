using Domain.Services.IServices;
using Entities.Templates;
using Serilog;

namespace Infrastructure.Business.SERVICES;

public class AddNewCongratulationsService : IAddNewCongratulationsService
{
    private readonly IGenerateCongratilation _generateCongratulationsService;
    private readonly IProxyApiClient _proxyApiClient;
    private readonly ILogger _logger; // Серилог напрямую

    public AddNewCongratulationsService(
        IGenerateCongratilation generateCongratulationsService,
        IProxyApiClient proxyApiClient,
        ILogger logger) // Внедряем Serilog напрямую
    {
        _generateCongratulationsService = generateCongratulationsService;
        _proxyApiClient = proxyApiClient;
        _logger = logger;
    }
    
    public async Task<bool> AddNewCongratulations(PozdrikIdDto pozdrikIdDto)
    {
        _logger.Information("Начинаем добавление нового поздравления для ID: {PozdrikId}", pozdrikIdDto._pozdrikId);

        try
        {
            AddIntAndPozhDto? inters = await _proxyApiClient.GetAddIntAndCongratilationAsync(pozdrikIdDto);
            string? name = await _proxyApiClient.GetName(pozdrikIdDto);
            string? congratulations;

            if (inters is not null)
            {
                _logger.Information("Найдены интересы и пожелания для ID: {PozdrikId}", pozdrikIdDto._pozdrikId);
                congratulations = await _generateCongratulationsService.GenerateAsync(name, inters.Interests, inters.Pozhelania);
            }
            else
            {
                _logger.Warning("Не найдены интересы и пожелания для ID: {PozdrikId}, создаем стандартное поздравление", pozdrikIdDto._pozdrikId);
                congratulations = await _generateCongratulationsService.GenerateAsync(name, String.Empty, String.Empty);
            }

            bool result = await _proxyApiClient.CongratilationToProxyApiAsync(new PozdrStringDTO
            {
                _pozdr = congratulations, 
                _pozdrikId = pozdrikIdDto._pozdrikId
            });

            _logger.Information("Поздравление успешно отправлено в Proxy API: {Result}", result);
            _logger.Information("текст Поздравления : {congratulations}", congratulations);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Ошибка при добавлении нового поздравления для ID: {PozdrikId}", pozdrikIdDto._pozdrikId);
            return false;
        }
    }
}
