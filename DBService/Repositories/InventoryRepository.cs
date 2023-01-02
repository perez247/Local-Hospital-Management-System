using Application.Interfaces.IRepositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDBContext _context;
        public InventoryRepository(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<AppInventory> AppInventories()
        {
            return _context.AppInventories.AsQueryable();
        }

        public IQueryable<AppInventoryItem> AppInventoryItems()
        {
            return _context.AppInventoryItems.AsQueryable();
        }
    }
}
