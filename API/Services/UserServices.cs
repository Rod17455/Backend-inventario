using API.Controllers;
using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services;

public class UserServices : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly ILogger<UserServices> _logger;

    public UserServices(IOptions<JWT> jwt, ILogger<UserServices> logger, IUnitOfWork unitOfWork, IPasswordHasher<Usuario> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<ManagementResponse> RegisterAsync(RegisterDto registerDto)
    {
        ManagementResponse management = new ManagementResponse();

        var usuario = new Usuario
        {
            NomUser = registerDto.NomUser,
            Apellido = registerDto.Apellido,
            CorreoElectronico = registerDto.CorreoElectronico,
        };

        usuario.Password = _passwordHasher.HashPassword(usuario, registerDto.Password);

        var usuarioExiste = _unitOfWork.Usuarios
                                    .Find(u => u.NomUser.ToLower() == registerDto.NomUser.ToLower())
                                    .FirstOrDefault();

        if (usuarioExiste == null)
        {
            var rolPredeterminado = _unitOfWork.Roles
                                    .Find(u => u.Nombre == Authorizacion.rol_prdeterminado.ToString())
                                    .First();
            try
            {
                usuario.Roles.Add(rolPredeterminado);
                _unitOfWork.Usuarios.Add(usuario);
                await _unitOfWork.SaveAsync();

                management.Mensaje = $"El usuario  {registerDto.NomUser} ha sido registrado exitosamente";
                management.Estatus = true;
                return management;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                _logger.LogError("Error en añadir usuario: " + $"Error: {message}");
                
                management.Mensaje = "No se puede añadir otro usuario";
                management.Estatus = false;
                return management;
            }
        }
        else
        {
            management.Mensaje = $"El usuario con {registerDto.NomUser} ya se encuentra registrado.";
            management.Estatus = false;
            return management;
        }
    }

    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };
        }
    }

    public async Task<DatosUsuarioDto> RefreshTokenAsync(string refreshToken)
    {
        var datosUsuarioDto = new DatosUsuarioDto();

        var usuario = await _unitOfWork.Usuarios
                        .GetByRefreshAsync(refreshToken);

        if (usuario == null)
        {
            datosUsuarioDto.EstaAutenticado = false;
            datosUsuarioDto.Mensaje = $"El token no pertenece a ningún usuario.";
            return datosUsuarioDto;
        }

        var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

        if (!refreshTokenBd.IsActive)
        {
            datosUsuarioDto.EstaAutenticado = false;
            datosUsuarioDto.Mensaje = $"El token no está activo.";
            return datosUsuarioDto;
        }
        //Revocamos el Refresh Token actual y
        refreshTokenBd.Revoked = DateTime.UtcNow;
        //generamos un nuevo Refresh Token y lo guardamos en la Base de Datos
        var newRefreshToken = CreateRefreshToken();
        usuario.RefreshTokens.Add(newRefreshToken);
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.SaveAsync();
        //Generamos un nuevo Json Web Token 😊
        datosUsuarioDto.Id = usuario.ID;
        datosUsuarioDto.EstaAutenticado = true;
        JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
        datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        datosUsuarioDto.Email = usuario.CorreoElectronico;
        datosUsuarioDto.UserName = usuario.NomUser;
        datosUsuarioDto.Roles = usuario.Roles
                                        .Select(u => u.Nombre)
                                        .ToList();
        datosUsuarioDto.RefreshToken = newRefreshToken.Token;
        datosUsuarioDto.RefreshTokenExpiration = newRefreshToken.Expires;
        return datosUsuarioDto;
    }

    public async Task<DatosUsuarioDto> GetTokenAsync(LoginDto model)
    {
        DatosUsuarioDto datosUsuarioDto = new DatosUsuarioDto();
        var usuario = await _unitOfWork.Usuarios
                    .GetByUserNameAsync(model.Username);

        if (usuario == null)
        {
            datosUsuarioDto.EstaAutenticado = false;
            datosUsuarioDto.Mensaje = $"No existe ningún usuario con el username {model.Username}.";
            return datosUsuarioDto;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

        if (resultado == PasswordVerificationResult.Success)
        {
            datosUsuarioDto.Id = usuario.ID;
            datosUsuarioDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
            datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUsuarioDto.Email = usuario.CorreoElectronico;
            datosUsuarioDto.UserName = usuario.NomUser;
            datosUsuarioDto.Roles = usuario.Roles
                                            .Select(u => u.Nombre)
                                            .ToList();


            if (usuario.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = usuario.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                datosUsuarioDto.RefreshToken = activeRefreshToken.Token;
                datosUsuarioDto.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                datosUsuarioDto.RefreshToken = refreshToken.Token;
                datosUsuarioDto.RefreshTokenExpiration = refreshToken.Expires;
                usuario.RefreshTokens.Add(refreshToken);
                _unitOfWork.Usuarios.Update(usuario);
                await _unitOfWork.SaveAsync();
            }

            return datosUsuarioDto;
        }
        datosUsuarioDto.EstaAutenticado = false;
        datosUsuarioDto.Mensaje = $"Credenciales incorrectas para el usuario {usuario.NomUser}.";
        return datosUsuarioDto;
    }


    public async Task<ManagementResponse> AddRoleAsync(AddRoleDto model)
    {
        ManagementResponse management = new ManagementResponse();

        var usuario = await _unitOfWork.Usuarios
                    .GetByUserNameAsync(model.Username);

        if (usuario == null)
        {
            management.Mensaje = $"No existe algún usuario registrado con la cuenta {model.Username}.";
            management.Estatus = false;
            return management;
        }


        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);

        if (resultado == PasswordVerificationResult.Success)
        {


            var rolExiste = _unitOfWork.Roles
                                        .Find(u => u.Nombre.ToLower() == model.Role.ToLower())
                                        .FirstOrDefault();

            if (rolExiste != null)
            {
                var usuarioTieneRol = usuario.Roles
                                            .Any(u => u.ID == rolExiste.ID);

                if (usuarioTieneRol == false)
                {
                    usuario.Roles.Add(rolExiste);
                    _unitOfWork.Usuarios.Update(usuario);
                    await _unitOfWork.SaveAsync();
                }

                management.Mensaje = $"Rol {model.Role} agregado a la cuenta {model.Username} de forma exitosa.";
                management.Estatus = true;

                return management;
            }

            management.Mensaje = $"Rol {model.Role} no encontrado.";
            management.Estatus = false;

            return management;
        }
        management.Mensaje = $"Credenciales incorrectas para el usuario {usuario.NomUser}.";
        management.Estatus = false;
        return management;
    }

    private JwtSecurityToken CreateJwtToken(Usuario usuario)
    {
        var roles = usuario.Roles;
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
        {
            roleClaims.Add(new Claim("roles", role.Nombre));
        }
        var claims = new[]
        {
                                new Claim(JwtRegisteredClaimNames.Sub, usuario.NomUser),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                                new Claim("uid", usuario.ID.ToString()),
                                new Claim("userName", usuario.NomUser)

        }
        .Union(roleClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

}
