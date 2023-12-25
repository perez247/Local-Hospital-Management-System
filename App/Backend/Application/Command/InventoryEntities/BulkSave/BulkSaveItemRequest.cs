using Application.Annotations;
using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application.Command.InventoryEntities.BulkSave
{
    public class BulkSaveItemRequest
    {
        [VerifyGuidAnnotation]
        public string? InventoryId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Type { get; set; }

        [VerifyGuidAnnotation]
        public string? FoundId { get; set; }
    }
}
