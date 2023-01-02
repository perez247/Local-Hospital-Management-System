﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public class DomainEnums
    {
    }

    public enum ContractTypeEnum
    {
        staff,
        contract
    }

    public enum StaffRoleEnum
    {
        nurse,
        doctor,
        admin,
        pharmacy,
        lab,
        surgery,
        radiology,
        admission
    }

    public enum AppCostType
    {
        profit,
        expense,
        tax
    }

    public enum PaymentStatus
    {
        pending,
        owing,
        approved,
        canceled
    }

    public enum PaymentType
    {
        cash,
        bank_transfer,
        pos,
        internet,
        none
    }
    public enum AppTicketStatus
    {
        ongoing,
        concluded,
        canceled
    }
    public enum AppInventoryType
    {
        pharmacy,
        surgery,
        lab,
        radiology,
        admission
    }
    public enum SurgeryTicketStatus
    {
        success,
        failed,
        unknown
    }
}