using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Query.GetAppointments;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.GetTickets
{
    public class GetTicketsQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<AppTicketResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }
        public GetTicketsQueryFilter? Filter { get; set; }
    }

    public class GetTicketHandler : IRequestHandler<GetTicketsQuery, PaginationResponse<IEnumerable<AppTicketResponse>>>
    {
        private readonly ITicketRepository iTicketRepository;

        public GetTicketHandler(ITicketRepository ITicketRepository)
        {
            iTicketRepository = ITicketRepository;
        }
        public async Task<PaginationResponse<IEnumerable<AppTicketResponse>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
        {
            PaginationDto<AppTicket> ticketsFromDb;

            if (request.Filter.Full.HasValue && request.Filter.Full.Value)
            {
                ticketsFromDb = await iTicketRepository.GetTickets(request.Filter, request.Pagination);
            }
            else
            {
                ticketsFromDb = await iTicketRepository.GetLinerTickets(request.Filter, request.Pagination);
            }

            if (ticketsFromDb.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<AppTicketResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = ticketsFromDb.totalItems };

            var responses = ticketsFromDb.Results.Select(x => AppTicketResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<AppTicketResponse>, AppTicket>(responses, ticketsFromDb);
        }
    }
}
