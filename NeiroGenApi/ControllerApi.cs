using Domain.Services.IServices;
using Entities.Templates;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace NeiroGenApi;

[ApiController]
[Route("api/GenerateCon")]
public class ControllerApi : ControllerBase
{
    private readonly IAddNewCongratulationsService _addNewCongratulationsService;

    public ControllerApi(IAddNewCongratulationsService addNewCongratulationsService)
    {
        _addNewCongratulationsService = addNewCongratulationsService;
    }

    /// <summary>
    /// Добавляет новое поздравление по ID.
    /// </summary>
    /// <param name="pozdrikId">ID Поздрика</param>
    /// <returns>Результат операции</returns>
    [HttpGet("{pozdrikId:int}")]
    [SwaggerOperation(Summary = "Добавить поздравление", Description = "Добавляет новое поздравление по указанному ID Поздрика." +
                                                                       "\n\n 6 - пример")]
    [SwaggerResponse(200, "Поздравление успешно добавлено", typeof(object))]
    [SwaggerResponse(400, "Ошибка при добавлении поздравления", typeof(object))]
    public async Task<IActionResult> GetIntAndPozh(int pozdrikId)
    {
        var response = await _addNewCongratulationsService.AddNewCongratulations(new PozdrikIdDto { _pozdrikId = pozdrikId });
        if (response)
        {
            return Ok(new { message = "Поздравление успешно добавлено!" });
        }
        else
        {
            return BadRequest(new { message = "Не удалось добавить поздравление." });
        }
    }
}