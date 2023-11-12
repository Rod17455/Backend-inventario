using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]
[Authorize]
public class ProveedorController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ProveedorController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<CteProvDto>>> GetProveedor([FromQuery] Params cteProvParams)
    {
        
        var resultado = await _unitOfWork.CteProvs.
            GetAllAsync(cteProvParams.PageIndex, 
                        cteProvParams.PageSize, cteProvParams.Search ?? "");

        var listaCteProv = _mapper.Map<List<CteProvDto>>(resultado.registros);

        Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

        return new Pager<CteProvDto>(listaCteProv, resultado.totalRegistros,
            cteProvParams.PageIndex, cteProvParams.PageSize, cteProvParams.Search);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CteProvDto>> GetId(int id)
    {
        var cteprov = await _unitOfWork.CteProvs.GetByIdAsync(id);
        if(cteprov == null)
            return NotFound(new ApiResponse(404, "El proveedor solicitado no existe"));

        return _mapper.Map<CteProvDto>(cteprov);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CteProv>> Post(CteProvDto cteProvDto)
    {
        var cteProv = _mapper.Map<CteProv>(cteProvDto);
        
        _unitOfWork.CteProvs.Add(cteProv);
        await _unitOfWork.SaveAsync();
        Console.WriteLine("PROV: "+cteProv);
        if(cteProv == null)
        {
            return BadRequest(new ApiResponse(400));
        }

        cteProvDto.ID = cteProv.ID;
        return CreatedAtAction(nameof(Post), new { id = cteProv.ID }, cteProv);
        
    }

    [HttpPost("detalle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CteProv>> PostOCDetalle(CteProv detail)
    {

        try
        {
            var _detalle = await _unitOfWork.CteProvs.DetalleProv(detail.ID);

            if (_detalle == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var response = new
            {
                Success = true,
                Data = _mapper.Map<CteProv>(_detalle)
            };

            return Ok(response);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("edit")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CteProvDto>> Put([FromBody]CteProvDto cteProvDto)
    {

        var cteProv = _mapper.Map<CteProv>(cteProvDto);

        _unitOfWork.CteProvs.Update(cteProv);
        await _unitOfWork.SaveAsync();

        var result = new
            {
                Success = true,
                Message = "Proveedor Actualizado",
                Data = cteProvDto
            };

        return Ok(result);
    }

    
    [HttpDelete("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var cteProv = await _unitOfWork.CteProvs.GetByIdAsync(id);
        if (cteProv == null)
            return NotFound(new ApiResponse(404, "El proveedor solicitado no existe"));

        _unitOfWork.CteProvs.Remove(cteProv);
        await _unitOfWork.SaveAsync();

        var result = new
            {
                Success = true,
                Message = "Proveedor Eliminado"
            };

        return Ok(result);
    }



}
