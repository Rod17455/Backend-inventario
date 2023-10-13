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

    //[HttpPost("register")]
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
        var cteprovBd = await _unitOfWork.CteProvs.GetByIdAsync(cteProvDto.ID);
        if (cteprovBd != null)
            return NotFound(new ApiResponse(404, "No puedes repetir el ID del proveedor"));

        _unitOfWork.CteProvs.Add(cteProv);
        await _unitOfWork.SaveAsync();
        if(cteProv == null)
        {
            return BadRequest(new ApiResponse(400));
        }

        cteProvDto.ID = cteProv.ID;
        return CreatedAtAction(nameof(Post), new { id = cteProv.ID }, cteProv);
    }
    
    [HttpPut("edit/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CteProvDto>> Put(int id, [FromBody]CteProvDto cteProvDto)
    {
        if (cteProvDto == null)
            return NotFound(new ApiResponse(404, "El proveedor solicitado no existe"));

        var cteprovBd = await _unitOfWork.CteProvs.GetByIdAsync(id);
        if (cteprovBd == null)
            return NotFound(new ApiResponse(404, "El proveedor solicitado no existe"));

        var cteProv = _mapper.Map<CteProv>(cteProvDto);
        _unitOfWork.CteProvs.Update(cteProv);
        await _unitOfWork.SaveAsync();
        return cteProvDto;
    }

    
    [HttpPut("delete/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var cteProv = await _unitOfWork.CteProvs.GetByIdAsync(id);
        if (cteProv == null)
            return NotFound(new ApiResponse(404, "El proveedor solicitado no existe"));

        _unitOfWork.CteProvs.Remove(cteProv);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }



}
