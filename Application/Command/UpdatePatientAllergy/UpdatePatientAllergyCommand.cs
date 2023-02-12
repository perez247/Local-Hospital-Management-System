using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.UpdatePatientAllergy
{
    public class UpdatePatientAllergyCommand : TokenCredentials, IRequest<Unit>
    {
        [VerifyGuidAnnotation]
        public string? PatientId { get; set; }
        public string? Allergies { get; set; }
    }

    public class UpdatePateientAllergyHandler : IRequestHandler<UpdatePatientAllergyCommand, Unit>
    {
        private readonly IPatientRepository iPatientRepository;
        private readonly IDBRepository iDBRepository;

        public UpdatePateientAllergyHandler(IPatientRepository IPatientRepository, IDBRepository IDBRepository)
        {
            iPatientRepository = IPatientRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<Unit> Handle(UpdatePatientAllergyCommand request, CancellationToken cancellationToken)
        {
            var patient = await iPatientRepository.Patients()
                                                  .FirstOrDefaultAsync(x => x.Id.ToString() == request.PatientId);

            if (patient == null)
            {
                throw new CustomMessageException("Patient not found", System.Net.HttpStatusCode.NotFound);
            }

            patient.Allergies = request.Allergies;

            iDBRepository.Update<Patient>(patient);
            await iDBRepository.Complete();

            return Unit.Value;
        }
    }
}
