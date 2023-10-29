﻿using Core.Entities;
using Core.Entities.Personalizadas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IEscasezRepository : IGenericRepository<InformacionEscasez>
{
    Task<int> ObtenerCantidadProducto(int idProducto);
    Task<decimal> ObtenerPrecioProducto(int idProducto);
    Task<int> ActualizarEstatusProductoPendiente(int idProducto);
    Task<int> InsertarEscasez(EscasezProducto escasezProducto);


}