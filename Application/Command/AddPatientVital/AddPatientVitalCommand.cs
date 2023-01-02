using Application.Annotations;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
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
            var vital = new PatientVital
            {
                NurseId = request.getCurrentUserRequest().CurrentUser.Staff.Id,
                Data = request.Data
            };

            await iDBRepository.AddAsync<PatientVital>(vital);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
