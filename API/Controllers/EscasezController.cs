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

    [HttpGet("bandejaAutorizadas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<InformacionAutorizacion>>> GetAutorizadas([FromQuery] Params productoParams)
    {

        var resultado = await _unitOfWork.Autorizaciones.
            GetAllAsync(productoParams.PageIndex,
                        productoParams.PageSize, productoParams.Search ?? "");

        var listaAutorizada = _mapper.Map<List<InformacionAutorizacion>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<InformacionAutorizacion>(listaAutorizada, resultado.totalRegistros,
            productoParams.PageIndex, productoParams.PageSize, productoParams.Search);
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

    [HttpPost("autorizar")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostProcesoAutorizar(AltaEscasezDto proceso)
    {
        try
        {
            var result = await _productoPendiente.ProcesoAutorizar(proceso);

            if (result.Estatus == false)
            {
                return BadRequest(new ApiResponse(400, result.Mensaje ?? ""));
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR EN EL ENDPOINT DE ESCASEZ " + ex.Message);
            return BadRequest(new ApiResponse(500));
        }
    }

    [HttpPost("rechazar")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostProcesoRechazar(AltaEscasezDto proceso)
    {
        try
        {
            var result = await _productoPendiente.ProcesoRechazo(proceso);

            if (result.Estatus == false)
            {
                return BadRequest(new ApiResponse(400, result.Mensaje ?? ""));
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR EN EL ENDPOINT DE ESCASEZ " + ex.Message);
            return BadRequest(new ApiResponse(500));
        }
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

   

    //INFORMACIÓN DE UN PRODUCTO
    [HttpPost("detalle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DetalleEscasez>> PostDetalle(DetalleEscasez detail)
    {

        try
        {
            var _detalle = await _unitOfWork.Escasezes.DetalleEscasez(detail.ID);

            if (_detalle == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var response = new
            {
                Success = true,
                Data = _mapper.Map<DetalleEscasez>(_detalle)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR EN EL ENDPOINT DE DETALLE DE ESCASEZ " + ex.Message);
            return BadRequest(new ApiResponse(500));
        }
    }

    //INFORMACIÓN DE UN PRODUCTO
    [HttpPost("detalleAutorizacion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DetalleAutorizacion>> PostDetalleAutorizacion(DetalleAutorizacion detail)
    {
        try
        {
            var _detalle = await _unitOfWork.Autorizaciones.DetalleAutorizacion(detail.ID);

            if (_detalle == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var response = new
            {
                Success = true,
                Data = _mapper.Map<DetalleAutorizacion>(_detalle)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR EN EL ENDPOINT DE DETALLE DE ESCASEZ " + ex.Message);
            return BadRequest(new ApiResponse(500));
        }
    }
}
