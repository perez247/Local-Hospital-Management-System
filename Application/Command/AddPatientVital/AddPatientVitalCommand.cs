using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Application.Command.AddPatientVital
{
    public class AddPatientVitalCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }
        public string? Data { get; set; }
    }

    public class AddPatientVitalHandler : IRequestHandler<AddPatientVitalCommand, Unit>
    {

        private readonly IPatientRepository iPatientRepository;
        private readonly IDBRepository iDBRepository;

        public AddPatientVitalHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(AddPatientVitalCommand request, CancellationToken cancellationToken)
        {
            if (request.getCurrentUserRequest().CurrentUser == null)
            {
               throw new CustomMessageException("Acting user not found", System.Net.HttpStatusCode.NotFound);

            }

            var patientFromDb = await iPatientRepository.Patients()
                                                        .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);

            if (patientFromDb == null)
            {
                throw new CustomMessageException("Patient not found", System.Net.HttpStatusCode.NotFound);
            }

            var vital = new PatientVital
            {
                PatientId = patientFromDb.Id,
                NurseId = request.getCurrentUserRequest().CurrentUser.Staff.Id,
                Data = request.Data
            };

            await iDBRepository.AddAsync<PatientVital>(vital);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
