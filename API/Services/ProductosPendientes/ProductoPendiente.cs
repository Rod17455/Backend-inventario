﻿using API.Dtos;
using API.Helpers.Errors;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services.ProductosPendientes;

public class ProductoPendiente : IProductoPendiente
{
    private readonly ILogger<ProductoPendiente> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventarioContext _inventarioContext;
    //private readonly IEscasezRepository _escasezRepository;

    public ProductoPendiente(ILogger<ProductoPendiente> logger, IUnitOfWork unitOfWork, InventarioContext inventarioContext)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _inventarioContext = inventarioContext;
        //_escasezRepository = escasezRepository;
    }

    public async Task<ManagementResponse> ProcesoPendienteAutorizar(PendienteAutorizarDto pendienteAutorizarDto)
    {
        using (var dbContextTransaction = _inventarioContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        {
            ManagementResponse management = new ManagementResponse();
            try
            {
                
                var idProducto = pendienteAutorizarDto.IdProducto;
                var cantidad = pendienteAutorizarDto.Cantidad;
                var idUsuario = pendienteAutorizarDto.IdUsuario;

                var cantidadBD = await _unitOfWork.Escasezes.ObtenerCantidadProducto(idProducto);
                var sumaCantidad = cantidad + cantidadBD;

                if(sumaCantidad > 50)
                {
                   
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "El limite de prendas es de 50";
                    management.Estatus = false;

                    return management;
                }

                //Modificar el estatus de producto
                var updateEstatusProducto = await _unitOfWork.Escasezes.ActualizarEstatusProductoPendiente(idProducto);
                if (updateEstatusProducto != 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede actualizar el estatus del producto";
                    management.Estatus = false;
                    return management;
                }

                var precioDB = await _unitOfWork.Escasezes.ObtenerPrecioProducto(idProducto);


                var escasez = new EscasezProducto
                {
                    ProductoId = idProducto,
                    Cant_Soli = sumaCantidad,
                    Fecha_Registro = DateTime.Now,
                    UsuarioId = idUsuario,
                    Estatus = "PENDIENTE",
                    Precio = sumaCantidad * precioDB
                };

                var insertar = await _unitOfWork.Escasezes.InsertarEscasez(escasez);

                if (insertar != 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede insertar el estatus del producto";
                    management.Estatus = false;
                    return management;
                }

                await dbContextTransaction.CommitAsync();

                management.Mensaje = "Agregado a productos por autorizar";
                management.Estatus = true;
                return management;



            } catch (Exception ex)
            {
                _logger.LogError("ERROR EN EL SERVICIO DE ESCASEZ DE PRODUCTOS: "+ex.Message);
                await dbContextTransaction.RollbackAsync();
                management.Mensaje = "Error en el servidor";
                management.Estatus = false;

                return management;
            }
        }
    }
}
