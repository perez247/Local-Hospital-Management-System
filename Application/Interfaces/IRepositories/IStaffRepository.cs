using Application.Paginations;
using Application.Query.StaffPaymentHistory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IStaffRepository
    {
        IQueryable<Staff> Staff();
        IQueryable<AppCost> AppCosts();
        IQueryable<AppAppointment> AppAppointments();
        IQueryable<FinancialRecord> FinancialRecords();
        IQueryable<FinancialRequest> FinancialRequests();
        Task<AppUser> CreateStaff(AppUser newUser, string password);
        Task<PaginationDto<SalaryPaymentHistory>> GetStaffListWithPayment(StaffPaymentHistoryFilter filter, PaginationCommand command);
    }
}
