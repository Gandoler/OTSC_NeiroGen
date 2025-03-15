using Domain.Services.IServices;
using Entities.Templates;
using Infrastructure.Business.SERVICES;
using Microsoft.AspNetCore.Mvc;

namespace NeiroGenApi;

[ApiController]
[Route("api/GenerateCon")]

public class ControllerApi: ControllerBase
{
    private readonly IAddNewCongratulationsService _addNewCongratulationsService;


    public ControllerApi(IAddNewCongratulationsService addNewCongratulationsService)
    {
        _addNewCongratulationsService = addNewCongratulationsService;
    }
    
    [HttpGet("{pozdrikId:int}")]
    public async Task<IActionResult> GetIntAndPozh(int pozdrikId)
    {
        var response =await _addNewCongratulationsService.AddNewCongratulations(new PozdrikIdDto{_pozdrikId = pozdrikId});
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