using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.StaffPaymentHistory;
using DBService.QueryHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBService.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public StaffRepository(AppDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IQueryable<Staff> Staff()
        {
            return _context.Staff.AsQueryable();
        }

        public IQueryable<AppCost> AppCosts()
        {
            return _context.AppCosts.AsQueryable();
        }

        public IQueryable<AppAppointment> AppAppointments()
        {
            return _context.AppAppointments.AsQueryable();
        }

        public IQueryable<FinancialRecord> FinancialRecords()
        {
            return _context.FinancialRecords.AsQueryable();
        }
        public IQueryable<FinancialRequest> FinancialRequests()
        {
            return _context.FinancialRequests.AsQueryable();
        }

        public async Task<AppUser> CreateStaff(AppUser newUser, string password)
        {
            var result = await _userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                throw new CustomMessageException(result.Errors.FirstOrDefault()?.Description?? string.Empty);
            }

            return newUser;
        }

        public async Task<PaginationDto<SalaryPaymentHistory>> GetStaffListWithPayment(StaffPaymentHistoryFilter filter, PaginationCommand command)
        {
            
            var query = _context.SalaryPaymentHistories
                                        .Include(x => x.Staff)
                                            .ThenInclude(s => s.AppUser)
                                       .AsQueryable();

            query = StaffQueryHelper.FilterPaymentHistory(query, filter);

            return await query.GenerateEntity(command);
        }
    }
}
