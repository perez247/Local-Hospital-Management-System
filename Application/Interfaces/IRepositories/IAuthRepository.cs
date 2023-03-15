﻿using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IAuthRepository
    {
        Task<bool> IsEmailAvailable(string emailAddress);
        Task<bool> CheckPasswordAndLockOn5FailedAttempts(AppUser User, string Password);
        Task RemoveFromAllRoles(AppUser user);
        Task AddToRoles(AppUser user, IEnumerable<string> roles);
    }
}
