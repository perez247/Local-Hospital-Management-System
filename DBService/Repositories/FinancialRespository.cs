using Application.Interfaces.IRepositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class FinancialRespository : IFinancialRespository
    {
        private readonly AppDBContext _context;
        public FinancialRespository(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<FinancialRecord> FinancialRecords()
        {
            return _context.FinancialRecords.AsQueryable();
        }

    }
}
