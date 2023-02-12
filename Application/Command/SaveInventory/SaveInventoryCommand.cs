using Application.Annotations;
using Application.Exceptions;
using Application.Interfaces.IRepositories;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command.SaveInventory
{
    public class SaveInventoryCommand : TokenCredentials, IRequest<SaveInventoryResponse>
    {
        [VerifyGuidAnnotation]
        public string? AppInventoryId { get; set; }
        public string? Name { get; set; }
        public bool? NotifyWhenLow { get; set; }
        public int? HowLow { get; set; }
        public string? AppInventoryType { get; set; }
        public string? Profile { get; set; }
    }

    public class SaveInventoryHandler : IRequestHandler<SaveInventoryCommand, SaveInventoryResponse>
    {
        private readonly IInventoryRepository iInventoryRepository;
        private readonly IDBRepository iDBRepository;

        public SaveInventoryHandler(IInventoryRepository IInventoryRepository, IDBRepository IDBRepository)
        {
            iInventoryRepository = IInventoryRepository;
            iDBRepository = IDBRepository;
        }
        public async Task<SaveInventoryResponse> Handle(SaveInventoryCommand request, CancellationToken cancellationToken)
        {
            string id = "";

            if (request.AppInventoryId == Guid.Empty.ToString())
            {
                AppInventory newAppInventory = new AppInventory();
                newAppInventory.Id= Guid.NewGuid();
                newAppInventory.Name = request.Name;
                newAppInventory.NotifyWhenLow = request.NotifyWhenLow.Value;
                newAppInventory.HowLow = request.HowLow.Value;
                newAppInventory.AppInventoryType = request.AppInventoryType.ParseEnum<AppInventoryType>();
                await iDBRepository.AddAsync<AppInventory>(newAppInventory);
                id = newAppInventory.Id.ToString();
            } else
            {
                var appInVentoryFromDB = await iInventoryRepository.AppInventories().FirstOrDefaultAsync(x => x.Id.ToString() == request.AppInventoryId);

                if (appInVentoryFromDB == null)
                {
                    throw new CustomMessageException("Inventory not found");
                }

                appInVentoryFromDB.Name = request.Name;
                appInVentoryFromDB.AppInventoryType = request.AppInventoryType.ParseEnum<AppInventoryType>();
                appInVentoryFromDB.NotifyWhenLow = request.NotifyWhenLow.Value;
                appInVentoryFromDB.HowLow = request.HowLow.Value;
                appInVentoryFromDB.Profile = request.Profile;

                iDBRepository.Update<AppInventory>(appInVentoryFromDB);
                id = appInVentoryFromDB.Id.ToString();
            }

            await iDBRepository.Complete();

            return new SaveInventoryResponse
            {
                InventoryId = id
            };
        }
    }
}
