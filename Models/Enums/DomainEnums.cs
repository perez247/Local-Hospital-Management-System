﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
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
        admission,
        hr,
        finance,
        staff,
        reception,
        other,
    }

    public enum AppCostType
    {
        profit,
        expense,
        tax,
        overall_ticket,
        part_ticket,
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
        admission,
        nurse,
        other,
    }
    public enum SurgeryTicketStatus
    {
        success,
        failed,
        unknown
    }

    public enum AppSettingType
    {
        billings
    }
}
