using Application.Query.UserEntities.GetUserList;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBService.QueryHelpers
{
    public static class UserQueryHelper
    {

        public static IQueryable<AppUser> FilterUserList(IQueryable<AppUser> query, GetUserListFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(filter.UserType))
            {
                if (filter.UserType.ToLower().Equals("patient"))
                {
                    query = query.Include(x => x.Patient)
                                    .ThenInclude(x => x.PatientContracts.OrderByDescending(y => y.DateCreated).Take(1))
                                        .ThenInclude(x => x.AppCost)
                                .Include(x => x.Patient)
                                    .ThenInclude(x => x.Company)
                                        .ThenInclude(x => x.CompanyContracts.OrderByDescending(y => y.DateCreated).Take(1))
                                            .ThenInclude(x => x.AppCost)
                                .Include(x => x.Patient)
                                    .ThenInclude(x => x.Company)
                                        .ThenInclude(x => x.AppUser)
                                .AsQueryable();

                    query = query.Where(x => x.Patient != null);
                }

                if (filter.UserType.ToLower().Equals("staff"))
                {
                    query = query.Include(x => x.Staff)
                                .Include(x => x.UserRoles).ThenInclude(y => y.Role)
                                .AsQueryable();

                    query = query.Where(x => x.Staff != null);
                }

                if (filter.UserType.ToLower().Equals("company"))
                {
                    query = query.Include(x => x.Company)
                                    .ThenInclude(x => x.CompanyContracts.OrderByDescending(y => y.DateCreated).Take(1))
                                        .ThenInclude(x => x.AppCost)
                                .AsQueryable();

                    query = query.Where(x => x.Company != null);
                }
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(i =>
                    (!string.IsNullOrEmpty(i.FirstName) && EF.Functions.Like(i.FirstName.ToLower(), $"%{filter.Name.ToLower()}%")) ||
                    (!string.IsNullOrEmpty(i.LastName) && EF.Functions.Like(i.LastName.ToLower(), $"%{filter.Name.ToLower()}%")) ||
                    (!string.IsNullOrEmpty(i.OtherName) && EF.Functions.Like(i.OtherName.ToLower(), $"%{filter.Name.ToLower()}%"))
                );
            }

            if (filter.IsCompany.HasValue)
            {
                if (filter.IsCompany.Value)
                {
                    query = query.Where(i => i.Patient.CompanyId.HasValue);
                }
                else
                {
                    query = query.Where(i => !i.Patient.CompanyId.HasValue);
                }
            }

            if (filter.PatientId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Patient.Id.ToString() == filter.PatientId);
            }

            if (filter.CompanyId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Company.Id.ToString() == filter.CompanyId);
            }

            if (filter.StaffId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Staff.Id.ToString() == filter.StaffId);
            }


            if (filter.UserId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Id.ToString() == filter.UserId);
            }

            if (filter.PatientCompanyId != Guid.Empty.ToString())
            {
                query = query.Where(x => x.Patient.Company != null && x.Patient.CompanyId.ToString() == filter.PatientCompanyId);
            }

            if (filter.Active.HasValue)
            {
                query = query.Where(i => i.Staff != null && i.Staff.Active == filter.Active.Value);
            }

            if (filter.ForIndividual.HasValue)
            {
                query = query.Where(x => x.Company.ForIndividual == filter.ForIndividual.Value);
            }

            if (!string.IsNullOrEmpty(filter.UserSearchId))
            {
                query = query.Where(i => EF.Functions.Like(i.Id.ToString().ToLower(), $"%{filter.UserSearchId.ToLower()}%"));
            }

            if (filter.Roles != null && filter.Roles.Count > 0)
            {
                query = query.Where(x => x.UserRoles.Any(y => filter.Roles.Contains(y.Role.Name)));
            }

            return query;
        }
    }
}
