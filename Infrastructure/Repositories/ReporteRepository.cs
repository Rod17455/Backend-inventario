using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class ReporteRepository : GenericRepository<Reporte>, IReporteRepository
{
    public ReporteRepository(InventarioContext context) : base(context)
    {
    }
}
