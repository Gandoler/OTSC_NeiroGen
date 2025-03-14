using Entities.Templates;

namespace Domain.Services.IServices;

public interface IAddNewCongratulationsService
{
    Task<bool> AddNewCongratulations(PozdrikIdDto pozdrikIdDto);
}