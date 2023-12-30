using Application.Query.FinancialRecordEntities.GetRevenue;
using DBService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobs
{
    public class FinancialRecordBackgroundService : BackgroundService
    {
        public readonly IServiceScopeFactory ScopeFactory;

        public FinancialRecordBackgroundService(IServiceScopeFactory scopeFactory)
        {
            ScopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using (var scope = ScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

                    var record = await db.MonthlyFinanceRecords.OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync();
                
                    var date = DateTime.Today;

                    if (record == null)
                    {
                        var newRecord = new MonthlyFinanceRecord
                        {
                            Profit = 0,
                            Expense = 0,
                            Date = date.AddMonths(-1).ToUniversalTime(),
                            DisplayDate = date.ToUniversalTime(),
                        };

                        await db.AddAsync(newRecord);
                        await db.SaveChangesAsync();
                    } else
                    {
                        if (record.DisplayDate.Month != date.Date.Month)
                        {
                            var sums = await db.FinancialRecords.Where(x => x.DateCreated.Month ==  record.DisplayDate.Month && x.DateCreated.Year == record.DisplayDate.Year)
                                                                .GroupBy(x => true)
                                                                .Select(y => new GetRevenueResponse
                                                                {
                                                                    Profit = y.Where(x => x.CostType == Models.Enums.AppCostType.profit).Sum(a => a.Amount) ?? 0,
                                                                    Expense = y.Where(x => x.CostType == Models.Enums.AppCostType.expense).Sum(a => a.Amount) ?? 0
                                                                })
                                                                .FirstOrDefaultAsync();
                            
                            if (sums != null)
                            {
                                var newRecord = new MonthlyFinanceRecord
                                {
                                    Profit = record.Profit + sums.Profit.Value,
                                    Expense = record.Expense + sums.Expense.Value,
                                    Date = date.AddMonths(-1).ToUniversalTime(),
                                    DisplayDate = date.ToUniversalTime(),
                                };

                                await db.AddAsync(newRecord);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromDays(1));
                //await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}
