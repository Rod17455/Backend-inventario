using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using API.Services.ProductosPendientes;
using AutoMapper;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]
[Authorize]
public class EscasezController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IProductoPendiente _productoPendiente;
    private readonly ILogger<EscasezController> _logger;

    public EscasezController(IUnitOfWork unitOfWork, IMapper mapper, IProductoPendiente productoPendiente, ILogger<EscasezController> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _productoPendiente = productoPendiente;
        _logger = logger;
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<InformacionEscasez>>> GetEscasez([FromQuery] Params productoParams)
    {

        var resultado = await _unitOfWork.Escasezes.
            GetAllAsync(productoParams.PageIndex,
                        productoParams.PageSize, productoParams.Search ?? "");

        var listaProducto = _mapper.Map<List<InformacionEscasez>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<InformacionEscasez>(listaProducto, resultado.totalRegistros,
            productoParams.PageIndex, productoParams.PageSize, productoParams.Search);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostProcesoEscasez(PendienteAutorizarDto proceso)
    {
        try
        {
            var result = await _productoPendiente.ProcesoPendienteAutorizar(proceso);

            if(result.Estatus == false)
            {
                return BadRequest(new ApiResponse(400, result.Mensaje ?? ""));
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR EN EL ENDPOINT DE ESCASEZ "+ex.Message);
            return BadRequest(new ApiResponse(500));
        }
    }
}
