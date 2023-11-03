using Core.Entities;
using Core.Entities.Personalizadas;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly InventarioContext _context;

    public GenericRepository(InventarioContext context)
    {
        _context = context;
    }



    public virtual void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }

    public async Task<DetalleAutorizacion> DetalleAutorizacion(int idAutorizacion)
    {
        var consulta = (
           from a in _context.Autorizaciones
           join e in _context.Escasezes on a.Id_Escasez equals e.ID
           join u in _context.Usuarios on a.Id_Usuario equals u.ID
           join s in _context.Estatuses on a.Estatus equals s.ID
           join p in _context.Productos on e.ProductoId equals p.ID
           join prov in _context.CteProvs on p.CveProv equals prov.ID
           select new DetalleAutorizacion
           {
               ID = a.ID,
               Cantidad = e.Cant_Soli,
               Fecha_Autorizacion = a.Fecha_Autoriza,
               Nombre_Empleado = u.Apellido,
               Nombre_Producto = p.NomProd,
               Precio = e.Precio,
               Categoria = p.Categoria,
               Contacto = prov.Contacto,
               DescProd = p.DescProd,
               EmailUsuario = u.CorreoElectronico,
               NombreEmpresa = prov.NombreEmpresa,
               Stock = p.Stock,
               Telefono = prov.Telefono

           }).FirstOrDefaultAsync();

        return await consulta;
    }

    public async Task<DetalleEscasez> DetalleEscasez(int idEscasez)
    {
        var consulta = (
          from p in _context.Escasezes
          join c in _context.Productos on p.ProductoId equals c.ID
          join u in _context.Usuarios on p.UsuarioId equals u.ID
          join e in _context.Estatuses on p.Estatus equals e.ID
          join prov in _context.CteProvs on c.ID equals prov.ID
          where p.ID == idEscasez
          select new DetalleEscasez
          {
              ID = p.ID,
              Estatus = p.Estatus + " - " + e.NombEstatus,
              Cant_Soli = p.Cant_Soli,
              Fecha_Registro = p.Fecha_Registro,
              Nom_Producto = c.NomProd,
              Nom_Usuario = u.Apellido,
              Precio = p.Precio,
              Correo_Usuario = u.CorreoElectronico,
              Email_Prov = prov.CorreoElectronico,
              Nomb_Prov = prov.Contacto,
              Telefono_Prov = prov.Telefono,
              Nomb_Empresa = prov.NombreEmpresa,
              Id_Producto = c.ID
              
          }).FirstOrDefaultAsync();

        return await consulta;
    }

    public async Task<InformacionProducto> DetalleProducto(int idProducto)
    {
        var detalle = (
               from p in _context.Productos
               join c in _context.CteProvs on p.CveProv equals c.ID
               join e in _context.Estatuses on p.Estatus equals e.ID
               where p.ID == idProducto
               select new InformacionProducto
               {
                   NomProd = p.NomProd,
                   Categoria = p.Categoria,
                   DescProd = p.DescProd,
                   NombEmpresa = p.CveProv + " - " + c.NombreEmpresa,
                   Precio = p.Precio,
                   Stock = p.Stock,
                   Estatus = p.Estatus + " - " + e.NombEstatus,
                   NombProv = c.Contacto,
                   EmailProv = c.CorreoElectronico,
                   TelefonoProv = c.Telefono,
                   ID = p.ID
               }
           ).FirstOrDefaultAsync();

        return await detalle;
    }

    public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, 
        int pageSize, string search)
    {
        var totalRegistros = await _context.Set<T>()
                           .CountAsync();

        var registros = await _context.Set<T>()
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

        return (totalRegistros, registros);
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public virtual void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public virtual void Update(T entity)
    {
        _context.Set<T>()
            .Update(entity);
    }
}
