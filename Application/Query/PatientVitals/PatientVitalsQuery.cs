using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Paginations;
using Application.Responses;
using Application.Utilities;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query.PatientVitals
{
    public class PatientVitalsQuery : TokenCredentials, IRequest<PaginationResponse<IEnumerable<PatientVitalResponse>>>
    {
        [SanitizePagination]
        public PaginationCommand? Pagination { get; set; }

        public PatientVitalsFilter? Filter { get; set; }
    }

    public class PatientVitalsHandler : IRequestHandler<PatientVitalsQuery, PaginationResponse<IEnumerable<PatientVitalResponse>>>
    {
        private readonly IPatientRepository iPatientRepository;

        public PatientVitalsHandler(IPatientRepository IPatientRepository)
        {
            iPatientRepository = IPatientRepository;
        }

        public async Task<PaginationResponse<IEnumerable<PatientVitalResponse>>> Handle(PatientVitalsQuery request, CancellationToken cancellationToken)
        {
            var vitals = await iPatientRepository.GetPatientVitals(request.Filter.PatientId, request.Pagination);

            if (vitals.Results.Count <= 0)
                return new PaginationResponse<IEnumerable<PatientVitalResponse>>() { PageNumber = request.Pagination.PageNumber, PageSize = request.Pagination.PageSize, totalItems = vitals.totalItems };

            var responses = vitals.Results.Select(x => PatientVitalResponse.Create(x));

            return request.Pagination.GenerateResponse<IEnumerable<PatientVitalResponse>, PatientVital>(responses, vitals);
        }
    }
}
