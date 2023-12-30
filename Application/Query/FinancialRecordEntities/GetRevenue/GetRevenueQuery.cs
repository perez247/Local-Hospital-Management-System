using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.FinancialRecordEntities.GetRevenue
{
    public class GetRevenueQuery: TokenCredentials, IRequest<GetRevenueResponse>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class GetRevenueHandler : IRequestHandler<GetRevenueQuery, GetRevenueResponse>
    {
        private IFinancialRespository _financialRespository { get; set; }

        public GetRevenueHandler(IFinancialRespository financialRespository = null)
        {
            _financialRespository = financialRespository;
        }
        public async Task<GetRevenueResponse> Handle(GetRevenueQuery request, CancellationToken cancellationToken)
        {
            var time = (request.EndDate - request.StartDate).Value.TotalDays;
            if (time > 30)
            {
                return new GetRevenueResponse { Expense = 0, Profit = 0 };
            }

            var totalSum = await _financialRespository.FinancialRecords()
                                                  .Where(x => x.DateCreated >= request.StartDate && x.DateCreated <= request.EndDate)
                                                  .GroupBy(x => true)
                                                  .Select(y => new GetRevenueResponse
                                                  {
                                                      Profit = y.Where(x => x.CostType == Models.Enums.AppCostType.profit).Sum(a => a.Amount),
                                                      Expense = y.Where(x => x.CostType == Models.Enums.AppCostType.expense).Sum(a => a.Amount)
                                                  })
                                                  .FirstOrDefaultAsync();

            if (totalSum == null)
            {
                // Create new 
                totalSum = new GetRevenueResponse
                {
                    Expense = 0,
                    Profit = 0
                };
            }

            var lastMonthTotals = await _financialRespository.MonthlyFinancialRecords()
                                                       .OrderBy(x => x.DateCreated)
                                                       .FirstOrDefaultAsync();

            if (lastMonthTotals == null)
            {
                totalSum.TotalProfit = 0;
                totalSum.TotalExpense = 0;
            } else
            {
                totalSum.TotalProfit = lastMonthTotals.Profit;
                totalSum.TotalExpense = lastMonthTotals.Expense;
            }

            return totalSum;
        }
    }
}
