using API.Dtos;
using API.Helpers.Errors;
using Core.Entities.Personalizadas;

namespace API.Services.ProductosPendientes;

public interface IProductoPendiente
{
    Task<ManagementResponse> ProcesoPendienteAutorizar(PendienteAutorizarDto pendienteAutorizarDto);
    Task<ManagementResponse> ProcesoAutorizar(AltaEscasezDto altaEscasezDto);
    Task<ManagementResponse> ProcesoRechazo(AltaEscasezDto altaEscasezDto);
    Task<string> RecuperarPlantilla(Plantilla plantilla);
    Task<bool> EnviarCorreoProv(Plantilla plantilla, string email);
    //Task<byte[]> ConvertHtmlToPdf(string html);
}
