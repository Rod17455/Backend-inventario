using API.Dtos;
using API.Helpers.Errors;
using API.Services.Notificaciones.Email;
using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using DinkToPdf;
using Infrastructure.Data;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using System;
using System.Text;

namespace API.Services.ProductosPendientes;

public class ProductoPendiente : IProductoPendiente
{
    private readonly ILogger<ProductoPendiente> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly InventarioContext _inventarioContext;
    private readonly IEmailSender _email;
    //private readonly IEscasezRepository _escasezRepository;

    public ProductoPendiente(ILogger<ProductoPendiente> logger, IUnitOfWork unitOfWork, InventarioContext inventarioContext, IEmailSender emailSender)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _inventarioContext = inventarioContext;
        _email = emailSender;
        //_escasezRepository = escasezRepository;
    }

    public async Task<bool> EnviarCorreoProv(Plantilla plantilla, string email)
    {
        try
        {
            var _plantilla = await RecuperarPlantilla(plantilla);
            //string html = "<html><body><h1>Hello, World!</h1></body></html>";
            string html = PlantillaEjemplo(plantilla);
            var titulo = "REPORTE " + plantilla.Fecha + " " + plantilla.NomProv;
            // Cree una instancia de la clase Converter.
            StringReader sr = new StringReader(html);
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                try
                {
                    htmlparser.Parse(sr);
                } catch(Exception ex)
                {
                    _logger.LogError("Error en parsear el HTML: {0}", ex.Message);
                    return false;
                }

                
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                await _email.SendEmailAsync(email, titulo, _plantilla, bytes);
            }
            //var base64 = ConvertHtmlToPdf(_plantilla);

            return true;

        } catch(Exception ex) {
            _logger.LogError("Error en enviar correo al prov: {0}", ex.Message);
            return false;
        }
    }

    /*private byte[] ConvertHtmlToPdf(string htmlContent)
    {
        
    }*/

    public async Task<ManagementResponse> ProcesoAutorizar(AltaEscasezDto altaEscasezDto)
    {
        using (var dbContextTransaction = _inventarioContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        {
            ManagementResponse management = new ManagementResponse();
            try
            {
                var idEscasez = altaEscasezDto.IdEscasez;
                var idUsuario = altaEscasezDto.IdUsuario;
                var idProducto = altaEscasezDto.IdProducto;
                var fecha = altaEscasezDto.FechaAutorizacion;
                var stock = altaEscasezDto.Stock;

                var updateEstatusProducto = await _unitOfWork.Escasezes.ActualizarEstatusProducto(idProducto, 6, stock, "Color.fromARGB(255, 238, 23, 224)");
                if (updateEstatusProducto != 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede actualizar el estatus del producto";
                    management.Estatus = false;
                    return management;
                }

                var updateEstatusEscasez = await _unitOfWork.Escasezes.ActualizarEstatusEscasez(idEscasez, 7);
                if (updateEstatusEscasez != 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede actualizar el estatus del escasez del producto";
                    management.Estatus = false;
                    return management;
                }

                //Insertar Autorizacion
                Autorizacion autorizacion = new Autorizacion
                {
                    Id_Escasez = idEscasez,
                    Id_Usuario = idUsuario,
                    Fecha_Autoriza = DateTime.Parse(fecha),
                    Estatus = 7
                };

                var insertarAutorizacion = await _unitOfWork.Escasezes.InsertarEscasez(autorizacion);

                if (insertarAutorizacion != 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede insertar la autorización";
                    management.Estatus = false;
                    return management;
                }

                var email = await _unitOfWork.CteProvs.ObtenerEmail(idProducto);

                if (email == "")
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede obtener el email";
                    management.Estatus = false;
                    return management;
                }

                var plantilla = await _unitOfWork.Escasezes.DatosPlantilla(idEscasez);

                var result = await EnviarCorreoProv(plantilla, email);

                if(result == false){
                     await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede enviar el correo";
                    management.Estatus = false;
                    return management;
                }

                await dbContextTransaction.CommitAsync();

                management.Mensaje = "Se autorizo la entrega de producto";
                management.Estatus = true;
                return management;
            } catch(Exception ex)
            {
                _logger.LogError("ERROR EN EL SERVICIO DE AUTORIZAR:  " + ex.Message);
                await dbContextTransaction.RollbackAsync();
                management.Mensaje = "Error en el servidor";
                management.Estatus = false;

                return management;
            }

        }

        
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
                var updateEstatusProducto = await _unitOfWork.Escasezes.ActualizarEstatusProductoPendiente(idProducto, "Color.fromARGB(255, 61, 121, 242)");
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
                    Estatus = 4,
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

    public async Task<ManagementResponse> ProcesoRechazo(AltaEscasezDto altaEscasezDto)
    {
        using (var dbContextTransaction = _inventarioContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        {
            ManagementResponse management = new ManagementResponse();
            try
            {
                var idEscasez = altaEscasezDto.IdEscasez;
                var observacion = altaEscasezDto.Observacion;

                var updateRechazo = await _unitOfWork.Escasezes.ActualizarEstatusProductoRechazado(altaEscasezDto.IdProducto, "Color.fromARGB(255, 128, 0, 0)", observacion);

                var updateEstatusEscasez = await _unitOfWork.Escasezes.ActualizarEstatusEscasez(idEscasez, 5);
                if (updateEstatusEscasez != 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede actualizar el estatus del escasez del producto";
                    management.Estatus = false;
                    return management;
                }


                var updtaeObservacion = await _unitOfWork.Escasezes.ActualizarObservacion(idEscasez, observacion);
                if (updtaeObservacion != 1)
                {
                    await dbContextTransaction.RollbackAsync();
                    management.Mensaje = "No se puede actualizar la observación del escasez del producto";
                    management.Estatus = false;
                    return management;
                }


                await dbContextTransaction.CommitAsync();

                management.Mensaje = "Se actualizo la observación";
                management.Estatus = true;
                return management;

            } catch (Exception ex) 
            {
                _logger.LogError("ERROR EN EL SERVICIO DE AUTORIZAR:  " + ex.Message);
                await dbContextTransaction.RollbackAsync();
                management.Mensaje = "Error en el servidor";
                management.Estatus = false;

                return management;
            }
        }
    }

    private string PlantillaEjemplo(Plantilla plantilla)
    {

        try
        {
            string rutaPlantilla = "http://inventario659.byethost6.com/plantillas/index.html";

            var _plantilla = File.ReadAllText("Recursos\\adjunto.html");


            _plantilla = _plantilla.Replace("{NomProv}", plantilla.NomProv);
            _plantilla = _plantilla.Replace("{Fecha}", plantilla.Fecha);
            _plantilla = _plantilla.Replace("{IdAutorizacion}", plantilla.IdEscasez);
            _plantilla = _plantilla.Replace("{Direccion}", plantilla.DireccionProv);
            _plantilla = _plantilla.Replace("{NomProducto}", plantilla.NombreProducto);
            _plantilla = _plantilla.Replace("{Cantidad}", plantilla.Cantidad);
            _plantilla = _plantilla.Replace("{Precio}", plantilla.Precio);
            _plantilla = _plantilla.Replace("{Empleado}", plantilla.NombreEmpleado);

            string html = _plantilla.ToString();

            return html;
        } catch(Exception ex)
        {
            _logger.LogError($"Error en recuperar la plantilla para correos: ${ex.Message}");
            return "0";
        }
    }

    public async Task<string> RecuperarPlantilla(Plantilla plantilla)
    {
        try
        {
            string _plantilla = string.Empty;
             //System.IO.File.ReadAllText
            using (StreamReader reader = new StreamReader("Recursos\\index.html"))
            {
                _plantilla = reader.ReadToEnd();
            }

            _plantilla = _plantilla.Replace("{NomProv}", plantilla.NomProv);
            _plantilla = _plantilla.Replace("{Fecha}", plantilla.Fecha);
            _plantilla = _plantilla.Replace("{IdAutorizacion}", plantilla.IdEscasez);
            _plantilla = _plantilla.Replace("{Direccion}", plantilla.DireccionProv);
            _plantilla = _plantilla.Replace("{NomProducto}", plantilla.NombreProducto);
            _plantilla = _plantilla.Replace("{Cantidad}", plantilla.Cantidad);
            _plantilla = _plantilla.Replace("{Precio}", plantilla.Precio);
            _plantilla = _plantilla.Replace("{Empleado}", plantilla.NombreEmpleado);
            //_plantilla = _plantilla.Replace("{Imagen}", plantilla.Imagen);
            //_logger.LogInformation("IMAGEN: "+plantilla.Imagen);

            return await Task.FromResult(_plantilla);


        } catch(Exception ex)
        {
            _logger.LogError($"Error en recuperar plantilla de correo {0}",ex.Message);
            return "0";
        }
    }
}
