﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IAppSettingRepository
    {
        IQueryable<AppSetting> AppSettings();
    }
}
