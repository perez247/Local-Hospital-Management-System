using Application.Interfaces.IRepositories;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.Repositories
{
    public class AppSettingRepository : IAppSettingRepository
    {
        private readonly AppDBContext _context;
        public AppSettingRepository(AppDBContext context)
        {
            _context = context;
        }

        public IQueryable<AppSetting> AppSettings()
        {
            return _context.AppSettings;
        }
    }
}
