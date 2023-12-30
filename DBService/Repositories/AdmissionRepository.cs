using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class AdmissionRepository
    {
        private readonly AppDBContext _context;
        public AdmissionRepository(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<AdmissionPrescription> AppTickets()
        {
            return _context.AdmissionPrescriptions.AsQueryable();
        }
    }
}
