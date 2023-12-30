using Application.Utilities;
using MediatR;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.AuthEntities.GetLookUps
{
    public class GetLookUpsQuery : TokenCredentials, IRequest<IEnumerable<LookUpResponse>>
    { }

    public class GetLookUpsHandler : IRequestHandler<GetLookUpsQuery, IEnumerable<LookUpResponse>>
    {
        public async Task<IEnumerable<LookUpResponse>> Handle(GetLookUpsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<LookUpResponse>();

            // StaffRoleEnums
            var items = Enum.GetNames(typeof(StaffRoleEnum));
            foreach (var item in items)
            {
                list.Add(new LookUpResponse { Type = nameof(StaffRoleEnum), Name = item, Display = item.Replace("_", " ").FirstLetterUpperCase() });
            }

            // AppCostType
            items = Enum.GetNames(typeof(AppCostType));
            foreach (var item in items)
            {
                list.Add(new LookUpResponse { Type = nameof(AppCostType), Name = item, Display = item.Replace("_", " ").FirstLetterUpperCase() });
            }

            // PaymentStatus
            items = Enum.GetNames(typeof(PaymentStatus));
            foreach (var item in items)
            {
                list.Add(new LookUpResponse { Type = nameof(PaymentStatus), Name = item, Display = item.Replace("_", " ").FirstLetterUpperCase() });
            }

            // PaymentType
            items = Enum.GetNames(typeof(PaymentType));
            foreach (var item in items)
            {
                list.Add(new LookUpResponse { Type = nameof(PaymentType), Name = item, Display = item.Replace("_", " ").FirstLetterUpperCase() });
            }

            // AppTicketStatus
            items = Enum.GetNames(typeof(AppTicketStatus));
            foreach (var item in items)
            {
                list.Add(new LookUpResponse { Type = nameof(AppTicketStatus), Name = item, Display = item.Replace("_", " ").FirstLetterUpperCase() });
            }

            // AppInventoryType
            items = Enum.GetNames(typeof(AppInventoryType));
            foreach (var item in items)
            {
                list.Add(new LookUpResponse { Type = nameof(AppInventoryType), Name = item, Display = item.Replace("_", " ").FirstLetterUpperCase() });
            }

            // SurgeryTicketStatus
            items = Enum.GetNames(typeof(SurgeryTicketStatus));
            foreach (var item in items)
            {
                list.Add(new LookUpResponse { Type = nameof(SurgeryTicketStatus), Name = item, Display = item.Replace("_", " ").FirstLetterUpperCase() });
            }

            return list;
        }
    }
}
