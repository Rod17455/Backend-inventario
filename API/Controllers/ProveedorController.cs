using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]
[Route("api/[controller]")]
[ApiController]
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
    public async Task<ActionResult<IEnumerable<CteProvDto>>> GetProveedor()
    {
        var cteProv = await _unitOfWork.CteProvs.GetAllAsync();

        return _mapper.Map<List<CteProvDto>>(cteProv);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CteProvDto>> GetId(int id)
    {
        var cteprov = await _unitOfWork.CteProvs.GetByIdAsync(id);
        if(cteprov == null)
            return NotFound();

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
        if(cteProv == null)
        {
            return BadRequest();
        }

        cteProvDto.ID = cteProv.ID;
        return CreatedAtAction(nameof(Post), new { id = cteProv.ID }, cteProv);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CteProvDto>> Put(int id, [FromBody]CteProvDto cteProvDto)
    {
        if (cteProvDto == null)
            return NotFound();

        var cteProv = _mapper.Map<CteProv>(cteProvDto);
        _unitOfWork.CteProvs.Update(cteProv);
        await _unitOfWork.SaveAsync();
        return cteProvDto;
    }

    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var cteProv = await _unitOfWork.CteProvs.GetByIdAsync(id);
        if (cteProv == null)
            return NotFound();

        _unitOfWork.CteProvs.Remove(cteProv);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }



}
