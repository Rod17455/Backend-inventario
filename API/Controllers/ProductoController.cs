using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]
[Authorize]
public class ProductoController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    //private readonly IProductoRepository _productoRepository;
    public ProductoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        //_productoRepository = productoRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<ProductoDto>>> GetProducto([FromQuery] Params productoParams)
    {

        var resultado = await _unitOfWork.Productos.
            GetAllAsync(productoParams.PageIndex,
                        productoParams.PageSize, productoParams.Search ?? "");

        var listaProducto = _mapper.Map<List<ProductoDto>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<ProductoDto>(listaProducto, resultado.totalRegistros,
            productoParams.PageIndex, productoParams.PageSize, productoParams.Search);
    }

    [HttpGet("pendientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<ProductoDto>>> GetProductoPendiente([FromQuery] Params productoParams)
    {

        var resultado = await _unitOfWork.Productos.
            GetAllPendienteAsync(productoParams.PageIndex,
                        productoParams.PageSize, productoParams.Search ?? "");

        var listaProducto = _mapper.Map<List<ProductoDto>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<ProductoDto>(listaProducto, resultado.totalRegistros,
            productoParams.PageIndex, productoParams.PageSize, productoParams.Search);
    }

    [HttpGet("rechazadas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<ProductoDto>>> GetProductoRechazo([FromQuery] Params productoParams)
    {

        var resultado = await _unitOfWork.Productos.
            GetAllRechazoAsync(productoParams.PageIndex,
                        productoParams.PageSize, productoParams.Search ?? "");

        var listaProducto = _mapper.Map<List<ProductoDto>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<ProductoDto>(listaProducto, resultado.totalRegistros,
            productoParams.PageIndex, productoParams.PageSize, productoParams.Search);
    }

    [HttpGet("entregar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<ProductoDto>>> GetProductoEntregar([FromQuery] Params productoParams)
    {

        var resultado = await _unitOfWork.Productos.
            GetAllXEntregarAsync(productoParams.PageIndex,
                        productoParams.PageSize, productoParams.Search ?? "");

        var listaProducto = _mapper.Map<List<ProductoDto>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<ProductoDto>(listaProducto, resultado.totalRegistros,
            productoParams.PageIndex, productoParams.PageSize, productoParams.Search);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoDto>> GetId(int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id);
        if (producto == null)
            return NotFound(new ApiResponse(404, "El proveedor solicitado no existe"));

        return _mapper.Map<ProductoDto>(producto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Producto>> Post(ProductoDto productoDto)
    {
        var producto = _mapper.Map<Producto>(productoDto);

        var productoBd = await _unitOfWork.Productos.GetByIdAsync(productoDto.ID);
        if (productoBd != null)
            return NotFound(new ApiResponse(404, "No puedes repetir el mismo producto"));

        _unitOfWork.Productos.Add(producto);
        await _unitOfWork.SaveAsync();
        if (producto == null)
        {
            return BadRequest(new ApiResponse(400));
        }

        productoDto.ID = producto.ID;
        return CreatedAtAction(nameof(Post), new { id = producto.ID }, producto);
    }

    //INFORMACIÓN DE UN PRODUCTO
    [HttpPost("detalle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InformacionProducto>> PostOCDetalle(InformacionProducto detail)
    {

        try
        {
            var _detalle = await _unitOfWork.Productos.DetalleProducto(detail.ID);

            if (_detalle == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var response = new
            {
                Success = true,
                Data = _mapper.Map<InformacionProducto>(_detalle)
            };

            return Ok(response);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("edit/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoDto>> Put(int id, [FromBody] ProductoDto productoDto)
    {
        if (productoDto == null)
            return NotFound(new ApiResponse(404, "El producto solicitado no existe"));

        var productoBd = await _unitOfWork.Productos.GetByIdAsync(id);
        if (productoBd == null)
            return NotFound(new ApiResponse(404, "El producto solicitado no existe"));

        var producto = _mapper.Map<Producto>(productoDto);
        _unitOfWork.Productos.Update(producto);
        await _unitOfWork.SaveAsync();
        return productoDto;
    }


    [HttpPut("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id);
        if (producto == null)
            return NotFound(new ApiResponse(404, "El producto solicitado no existe"));

        _unitOfWork.Productos.Remove(producto);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }




}
