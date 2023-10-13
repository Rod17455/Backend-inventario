using API.Dtos;
using API.Helpers.Errors;
using API.Services;
using AutoMapper;
using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.CompilerServices;

namespace API.Controllers;

public class UsuarioController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly ILogger<UsuarioController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UsuarioController(IUserService userService, ILogger<UsuarioController> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userService = userService;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    //VER INFORMACIÓN DE UN USUARIO
    [HttpPost("detalle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InformacionUsuario>> PostOCDetalle(InformacionUsuario detail)
    {

        try
        {
            var _detalle = await _unitOfWork.Usuarios.DetalleUsuario(detail.ID);

            if (_detalle == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var response = new
            {
                Success = true,
                Data = _mapper.Map<InformacionUsuario>(_detalle)
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("edit/{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Put(int id, [FromBody] UsuarioDto usuarioDto)
    {
        if (usuarioDto == null)
            return NotFound(new ApiResponse(404, "El usuario solicitado no existe"));

        var usuarioBd = await _unitOfWork.Usuarios.GetByIdAsync(id);
        if (usuarioBd == null)
            return NotFound(new ApiResponse(404, "El usuario solicitado no existe"));

        var usuario = _mapper.Map<Usuario>(usuarioDto);
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.SaveAsync();
        return usuarioDto;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterDto model)
    {
        try
        {
            var result = await _userService.RegisterAsync(model);

            if(result.Estatus == false)
            {
                return NotFound(new ApiResponse(404, result.Mensaje));
            }

            return Ok(result.Mensaje);
        }
        catch(Exception ex)
        {
            _logger.LogError("Error en el endpoint de registrar usuario: " + ex.Message);
            return BadRequest(new ApiResponse(400));
        }
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        try
        {
            var result = await _userService.GetTokenAsync(model);

            if(result.EstaAutenticado == false)
            {
                return NotFound(new ApiResponse(404, "El usuario no existe"));
            }

            SetRefreshTokenInCookie(result.RefreshToken);
            return Ok(result);
        } catch(Exception ex)
        {
            _logger.LogError("Error en el endpoint de login: "+ex.Message);
            return BadRequest(new ApiResponse(400));
        }
    }

    [HttpPost("addrole")]
    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        try
        {
            var result = await _userService.AddRoleAsync(model);

            if(result.Estatus == false)
            {
                return NotFound(new ApiResponse(404, result.Mensaje));
            }

            return Ok(result.Mensaje);
        } catch(Exception ex)
        {
            _logger.LogError("Error en el endpoint de AddRoleAsync: " + ex.Message);
            return BadRequest(new ApiResponse(400));
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var response = await _userService.RefreshTokenAsync(refreshToken);
        if (!string.IsNullOrEmpty(response.RefreshToken))
            SetRefreshTokenInCookie(response.RefreshToken);
        return Ok(response);
    }

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(10),
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

    }


}
