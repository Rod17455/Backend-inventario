﻿using API.Dtos;
using API.Helpers.Errors;

namespace API.Services.ProductosPendientes;

public interface IProductoPendiente
{
    Task<ManagementResponse> ProcesoPendienteAutorizar(PendienteAutorizarDto pendienteAutorizarDto);
    Task<ManagementResponse> ProcesoAutorizar(AltaEscasezDto altaEscasezDto);
    Task<ManagementResponse> ProcesoRechazo(AltaEscasezDto altaEscasezDto);
}
